using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using SZMK.TeklaInteraction.Shared.BindingModels;
using SZMK.TeklaInteraction.Shared.Services;
using SZMK.TeklaInteraction.Shared.ViewModels;
using SZMK.TeklaInteraction.Tekla2017.Services.Server.Interfaces;
using SZMK.TeklaInteraction.Tekla2017.Views.Main;
using SZMK.TeklaInteraction.Tekla2017.Views.Shared.Interfaces;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using ModelObject = Tekla.Structures.Model.ModelObject;

namespace SZMK.TeklaInteraction.Tekla2017.Services.Server
{
    class Tekla : ITekla
    {
        private readonly Logger logger;
        private readonly MailLogger maillogger;
        private readonly INotifyProgress notify;

        public Tekla(INotifyProgress notify)
        {
            logger = LogManager.GetCurrentClassLogger();
            maillogger = new MailLogger();
            this.notify = notify;
        }

        Model model;
        DrawingHandler CourretDrawingHandler;

        Shared.Models.Model Model;
        public List<Shared.Models.Drawing> Drawings;
        public List<StringErrorBindingModel> DrawingErrors;
        public List<StringErrorBindingModel> DetailsErrors;
        public List<StringErrorBindingModel> DetailsWarnings;

        public bool CheckConnect()
        {
            model = new Model();
            CourretDrawingHandler = new DrawingHandler();

            if (model.GetConnectionStatus() && CourretDrawingHandler.GetConnectionStatus())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void GetData(Shared.Models.User user)
        {
            try
            {
                DrawingEnumerator SelectedDrawings = CourretDrawingHandler.GetDrawingSelector().GetSelected();

                logger.Info("Чертежи успешно получены");

                Drawings = new List<Shared.Models.Drawing>();

                DrawingErrors = new List<StringErrorBindingModel>();
                DetailsErrors = new List<StringErrorBindingModel>();
                DetailsWarnings = new List<StringErrorBindingModel>();

                while (SelectedDrawings.MoveNext())
                {
                    try
                    {

                        if (SelectedDrawings.Current is AssemblyDrawing)
                        {
                            Assembly assembly = model.SelectModelObject(((SelectedDrawings.Current as AssemblyDrawing)).AssemblyIdentifier) as Assembly;

                            if (!ChechedDate(assembly))
                            {
                                throw new Exception("Не заполнено поле \"Дата\"");
                            }

                            GetDrawing(assembly, SelectedDrawings.Current as AssemblyDrawing);
                        }
                    }
                    catch (Exception E)
                    {
                        String Number = "";
                        String List = "";
                        Assembly assembly = model.SelectModelObject(((SelectedDrawings.Current as AssemblyDrawing)).AssemblyIdentifier) as Assembly;
                        assembly.GetReportProperty("CUSTOM.Zakaz", ref Number);
                        assembly.GetReportProperty("CUSTOM.Drw_SheetRev", ref List);
                        logger.Error(E.ToString());
                        DrawingErrors.Add(new StringErrorBindingModel { Order = Number, List = List, Error = E.Message });
                    }
                }

                logger.Info("Чертежи добавлены");

                ModelInfo modelInfo = model.GetInfo();
                string ModelPath = modelInfo.ModelPath;

                if (ModelPath.Substring(0, 2) != @"\\")
                {
                    using (var managementObject = new ManagementObject())
                    {
                        managementObject.Path = new ManagementPath($"Win32_LogicalDisk='{ModelPath.Substring(0, 2)}'");
                        var driveType = (DriveType)(uint)managementObject["DriveType"];
                        var networkPath = Convert.ToString(managementObject["ProviderName"]);

                        ModelPath = networkPath + ModelPath.Remove(0, 2);
                    }
                }

                ModelPath = ModelPath.Replace("tekla-fs", "10.0.7.249");

                Model = new Shared.Models.Model { DateCreate = DateTime.Now, Path = ModelPath, Drawings = Drawings };

                notify.Close();

                if (DetailsWarnings.Count > 0)
                {
                    ReportWarnings Report = new ReportWarnings();
                    Report.Report_DGV.AutoGenerateColumns = false;
                    Report.Report_DGV.DataSource = DetailsWarnings;

                    if (Report.ShowDialog() != DialogResult.OK)
                    {
                        for (int i = 0; i < DetailsWarnings.Count; i++)
                        {
                            Drawings.RemoveAll(p => p.Order == DetailsWarnings[i].Order && p.List == DetailsWarnings[i].List);
                        }
                    }
                }

                if (DetailsErrors.Count > 0)
                {
                    ReportErrors Report = new ReportErrors();
                    Report.Text = "Отчет об ошибках деталей";
                    Report.label1.Text = "Отчет об ошибках деталей";
                    Report.Report_DGV.AutoGenerateColumns = false;
                    Report.Report_DGV.DataSource = DetailsErrors;

                    Report.ShowDialog();
                }

                if (DrawingErrors.Count > 0)
                {
                    ReportErrors Report = new ReportErrors();
                    Report.Text = "Отчет об ошибках чертежей";
                    Report.label1.Text = "Отчет об ошибках чертежей";
                    Report.Report_DGV.AutoGenerateColumns = false;
                    Report.Report_DGV.DataSource = DrawingErrors;

                    Report.ShowDialog();
                }

                if (Model.Drawings.Count() > 0)
                {
                    logger.Info("Начат показ чертежей");

                    Operations operations = new Operations();
                    operations.ShowData(Model, user);
                }
            }
            catch (Exception E)
            {
                maillogger.SendErrorLog(E.ToString());
                logger.Error(E.ToString());
                MessageBox.Show("Ошибка операции", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private List<Shared.Models.Detail> AddMainDetailDrawingObjects(AssemblyDrawing parentDrawing)
        {
            try
            {
                List<Shared.Models.Detail> Details = new List<SZMK.TeklaInteraction.Shared.Models.Detail>();

                Assembly assembly = model.SelectModelObject(parentDrawing.AssemblyIdentifier) as Assembly;

                ModelObject modelObject = assembly.GetMainPart();

                Details.Add(GetDetailAttribute(assembly, modelObject, 1));

                AddsSecondariesDrawingObjectsToTreeNode(assembly, Details);

                return Details;
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        private void AddsSecondariesDrawingObjectsToTreeNode(Assembly assembly, List<SZMK.TeklaInteraction.Shared.Models.Detail> Details)
        {
            string Error = "Ошибка написания позиции детали";

            try
            {
                ArrayList secondaries = assembly.GetSecondaries();

                string _position = "";

                for (int i = 0; i < secondaries.Count; i++)
                {
                    ModelObject modelObject = secondaries[i] as ModelObject;

                    modelObject.GetReportProperty("PART_POS", ref _position);
                    long CountDetail = 0;

                    if (Details.Where(p => p.Position == _position).Count() > 0)
                    {
                        CountDetail = Details.Where(p => p.Position == _position).FirstOrDefault().Count;

                        Details.RemoveAll(p => p.Position == _position);
                    }

                    Details.Add(GetDetailAttribute(assembly, modelObject, CountDetail + 1));
                }
            }
            catch (Exception E)
            {
                throw new Exception(Error, E);
            }
        }
        private Shared.Models.Detail GetDetailAttribute(Assembly assembly, ModelObject modelObject, Int64 CountDetail)
        {
            try
            {
                string _name = "";
                int _startnumber = 0;
                string _position = "";
                string _profile = "";
                double _width = 0;
                double _lenght = 0;
                double _weight = 0;
                double _height = 0;
                string _diameter = "";
                string _markSteel = "";
                string _discription = "";
                double _gmlenght = 0;
                double _gmwidth = 0;
                double _gmheight = 0;
                string _machining = "";
                string _methodOfPaintingRAL = "";
                double _paintingArea = 0;
                string _gostName = "";
                string _flangeThickness = "";
                string _plateThickness = "";

                _name = GetNameDetail(assembly, modelObject);
                modelObject.GetReportProperty("PART_START_NUMBER", ref _startnumber);
                modelObject.GetReportProperty("PART_POS", ref _position);
                modelObject.GetReportProperty("PROFILE", ref _profile);
                modelObject.GetReportProperty("WIDTH", ref _width);
                modelObject.GetReportProperty("LENGTH", ref _lenght);
                modelObject.GetReportProperty("CUSTOM.SZ_PartWeight", ref _weight);
                modelObject.GetReportProperty("PROFILE.HEIGHT", ref _height);
                modelObject.GetReportProperty("PROFILE.DIAMETER", ref _diameter);
                modelObject.GetReportProperty("MATERIAL", ref _markSteel);
                _discription = GetDiscriptrion(modelObject);
                modelObject.GetReportProperty("ASSEMBLY.LENGTH", ref _gmlenght);
                modelObject.GetReportProperty("ASSEMBLY.WIDTH", ref _gmwidth);
                modelObject.GetReportProperty("ASSEMBLY.HEIGHT", ref _gmheight);
                _machining = GetMachining(modelObject);
                _methodOfPaintingRAL = GetMethodOfPainting(modelObject);
                modelObject.GetReportProperty("ASSEMBLY.AREA", ref _paintingArea);
                modelObject.GetReportProperty("PROFILE.GOST_NAME", ref _gostName);
                modelObject.GetReportProperty("PROFILE.FLANGE_THICKNESS_1", ref _flangeThickness);
                modelObject.GetReportProperty("PROFILE.PLATE_THICKNESS", ref _plateThickness);

                DetailViewModel detailViewModel = new DetailViewModel(_name, _position, CountDetail.ToString(), _profile, _width, _lenght, _weight, _height, _diameter, _markSteel, _discription, _gmlenght, _gmwidth, _gmheight, _machining, _methodOfPaintingRAL, _paintingArea, _gostName, _flangeThickness, _plateThickness);

                return new Shared.Models.Detail
                {
                    StartNumber = _startnumber,
                    Name = detailViewModel.Name,
                    Position = detailViewModel.Position,
                    Count = CountDetail,
                    Profile = detailViewModel.Profile,
                    Width = Convert.ToDouble(detailViewModel.Width.ToString("F2")),
                    Lenght = Convert.ToDouble(detailViewModel.Lenght.ToString("F2")),
                    Weight = Convert.ToDouble(detailViewModel.Weight.ToString("F2")),
                    Height = Convert.ToDouble(detailViewModel.Height.ToString("F2")),
                    Diameter = detailViewModel.Diameter,
                    SubtotalWeight = Convert.ToDouble(detailViewModel.SubTotalWeight.ToString("F2")),
                    MarkSteel = detailViewModel.MarkSteel,
                    Discription = detailViewModel.Discription,
                    GMlenght = Convert.ToDouble(detailViewModel.GMLenght.ToString("F2")),
                    GMwidth = Convert.ToDouble(detailViewModel.GMWidth.ToString("F2")),
                    GMheight = Convert.ToDouble(detailViewModel.GMHeight.ToString("F2")),
                    Machining = detailViewModel.Machining,
                    MethodOfPaintingRAL = detailViewModel.MethodOfPaintiongRAL,
                    PaintingArea = Convert.ToDouble((detailViewModel.PaintingArea / 1000000).ToString("F2")),
                    GostName = detailViewModel.GostName,
                    FlangeThickness = detailViewModel.FlangeThickness,
                    PlateThickness = detailViewModel.PlateThickness
                };
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        public bool GetDrawing(Assembly assembly, AssemblyDrawing parentDrawing)
        {
            try
            {
                string _assembly = "";
                string _order = "";
                string _place = "";
                string _list = "";
                string _mark = "";
                string _executor = "";
                double _weightMark = 0;
                int _countMark = 0;
                double _subTotalWeight = 0;
                double _subTotallenght = 0;
                long _countDetail = 0;

                assembly.GetReportProperty("LENGTH", ref _subTotallenght);

                _assembly = assembly.Name;

                assembly.GetReportProperty("CUSTOM.Zakaz", ref _order);

                if (!CheckedOrder(_order))
                {
                    if (MessageBox.Show($"Номер заказа {_order} записан не по шаблону, подтвердить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        throw new Exception($"Номер заказа должен быть записан по шаблону 0000(00)");
                    }
                }

                assembly.GetReportProperty("DRAWING.USERDEFINED.ru_mesto", ref _place);
                assembly.GetReportProperty("CUSTOM.Drw_SheetRev", ref _list);

                string[] splitter = _list.Split('и');

                while (splitter[0][0] == '0')
                {
                    splitter[0] = splitter[0].Remove(0, 1);
                }

                if (splitter.Length != 1)
                {
                    _list = splitter[0] + "и" + splitter[1];
                }
                else
                {
                    _list = splitter[0];
                }

                assembly.GetReportProperty("ASSEMBLY_POS", ref _mark);
                assembly.GetReportProperty("DRAWING.USERDEFINED.ru_11_fam_dop", ref _executor);

                if (!CheckedExecutor(_executor))
                {
                    throw new Exception($"Исполнитель не указан");
                }
                else
                {
                    try
                    {
                        _executor = _executor.Replace(" ", "");
                        _executor = _executor.Insert(_executor.IndexOf('.') - 1, " ");
                    }
                    catch
                    {
                        throw new Exception($"Исполнитель указан не по шаблону, Шаблон: Иванов И.И.");
                    }
                }

                assembly.GetReportProperty("CUSTOM.SZ_AssWeight", ref _weightMark);
                assembly.GetReportProperty("MODEL_TOTAL", ref _countMark);

                _subTotalWeight = _weightMark * _countMark;

                if (!CheckedUnique(_order, _list, _mark))
                {
                    throw new Exception($"Чертеж с Номером:{_order},Листом:{_list}, Маркой:{_mark} уже существует");
                }

                List<Shared.Models.Detail> Details = AddMainDetailDrawingObjects(parentDrawing);

                _countDetail = Details.Sum(p => p.Count);

                bool BadStartNumber = false;

                for (int i = 0; i < Details.Count; i++)
                {
                    if (Details[i].StartNumber.ToString()[0] == '9')
                    {
                        BadStartNumber = true;
                        DetailsErrors.Add(new StringErrorBindingModel { Order = _order.Replace(" ", ""), List = _list, Error = $"В детали с позицией {Details[i].Position} начальный номер равен {Details[i].StartNumber}" });
                    }
                    else if (Details[i].StartNumber != 1)
                    {
                        DetailsWarnings.Add(new StringErrorBindingModel { Order = _order.Replace(" ", ""), List = _list, Error = $"В детали с позицией {Details[i].Position} начальный номер равен {Details[i].StartNumber}" });
                    }
                }

                if (!BadStartNumber)
                {
                    Drawings.Add(new Shared.Models.Drawing { Assembly = _assembly, Order = _order.Replace(" ", ""), Place = _place, List = _list, Mark = _mark, Executor = _executor, WeightMark = Convert.ToDouble(_weightMark.ToString("F2")), CountMark = _countMark, SubTotalWeight = Convert.ToDouble(_subTotalWeight.ToString("F2")), SubTotalLenght = Convert.ToDouble(_subTotallenght.ToString("F2")), CountDetail = _countDetail, Details = Details });
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        public bool ChechedDate(Assembly assembly)
        {
            try
            {
                string StringAnswer = "";
                assembly.GetReportProperty("DRAWING.USERDEFINED.ru_date", ref StringAnswer);
                if (String.IsNullOrEmpty(StringAnswer))
                {
                    return false;
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        public bool CheckedUnique(string order, string list, string mark)
        {
            try
            {
                if (Drawings.Where(p => p.Order == order && p.List == list && p.Mark == mark).Count() != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        public bool CheckedOrder(string order)
        {
            try
            {
                int firstNum = Convert.ToInt32(order.Substring(0, order.IndexOf('(')));
                string secondNum = order.Substring(order.IndexOf('(') + 1, order.IndexOf(')') - order.IndexOf('(') - 1);

                string lastNum = order.Remove(0, order.IndexOf(')') + 1);

                if (!String.IsNullOrEmpty(lastNum))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedExecutor(string executor)
        {
            try
            {
                if (String.IsNullOrEmpty(executor))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        private String GetDiscriptrion(ModelObject modelObject)
        {
            try
            {
                int IntAnswer = 0;
                modelObject.GetReportProperty("HAS_HOLES", ref IntAnswer);
                if (IntAnswer == 0)
                {
                    return "";
                }
                else
                {
                    return "Отв. ";
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        private String GetMachining(ModelObject modelObject)
        {
            try
            {
                string StringAnswer = "";
                modelObject.GetReportProperty("PROFILE_TYPE", ref StringAnswer);
                string tempA = "";
                string tempB = "";
                if (StringAnswer == "B")
                {
                    modelObject.GetReportProperty("VOLUME", ref tempA);
                    modelObject.GetReportProperty("VOLUME_NET", ref tempB);
                    if (tempA == tempB)
                    {
                        StringAnswer = "";
                    }
                    else
                    {
                        StringAnswer = "Фаска/Стр. ";
                    }
                }
                else
                {
                    StringAnswer = "";
                }
                modelObject.GetReportProperty("USERDEFINED.comment", ref tempA);
                if (tempA == "0")
                {
                    return StringAnswer += "";
                }
                else
                {
                    return StringAnswer += tempA;
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        private String GetMethodOfPainting(ModelObject modelObject)
        {
            try
            {
                string StringAnswer = "";
                modelObject.GetReportProperty("ASSEMBLY.MAINPART.FINISH", ref StringAnswer);
                if (StringAnswer == "")
                {
                    modelObject.GetReportProperty("USERDEFINED.Obrabotka", ref StringAnswer);
                    return StringAnswer;
                }
                else
                {
                    return StringAnswer;
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message, E);
            }
        }
        private String GetNameDetail(Assembly assembly, ModelObject modelObject)
        {
            string _name = "";

            modelObject.GetReportProperty("ADVANCED_OPTION.XS_DRAWING_PLOT_FILE_NAME_W", ref _name);

            string temp = _name;

            while (true)
            {
                temp = temp.Remove(0, _name.IndexOf('%') + 1);

                string parm = temp.Substring(0, temp.IndexOf('%'));

                _name = _name.Replace($"%{parm}%", GetParametersDetail(assembly, modelObject, parm));

                temp = _name;

                if (temp.IndexOf('%') == -1)
                {
                    break;
                }
            }

            return _name;
        }

        private String GetParametersDetail(Assembly assembly, ModelObject modelObject, string parm)
        {
            string strdata = "";
            int intdata = 0;
            double dbldata = 0;

            if (parm == "NAME" || parm == "DRAWING_NAME")
            {
                string prefix = "";

                modelObject.GetReportProperty("PREFIX", ref prefix);

                if (prefix != "")
                {
                    modelObject.GetReportProperty("PART_POS", ref strdata);

                    return prefix + "_" + strdata;
                }
                else
                {
                    modelObject.GetReportProperty("PART_POS", ref strdata);

                    return strdata;
                }
            }
            if (parm == "NAME.-" || parm == "DRAWING_NAME.-")
            {
                string prefix = "";

                modelObject.GetReportProperty("PREFIX", ref prefix);

                if (prefix != "")
                {
                    modelObject.GetReportProperty("PART_POS", ref strdata);

                    return prefix + "-" + strdata;
                }
                else
                {
                    modelObject.GetReportProperty("PART_POS", ref strdata);

                    return strdata;
                }
            }
            if (parm == "NAME." || parm == "DRAWING_NAME.")
            {
                string prefix = "";

                modelObject.GetReportProperty("PREFIX", ref prefix);

                if (prefix != "")
                {
                    modelObject.GetReportProperty("PART_POS", ref strdata);

                    return prefix + strdata;
                }
                else
                {
                    modelObject.GetReportProperty("PART_POS", ref strdata);

                    return strdata;
                }
            }
            if (parm == "REV" || parm == "REVISION" || parm == "DRAWING_REVISION")
            {
                modelObject.GetReportProperty("DRAWING.REVISION.NUMBER", ref intdata);

                return intdata.ToString();
            }
            if (parm == "REV_MARK" || parm == "REVISION_MARK" || parm == "DRAWING_REVISION_MARK")
            {
                modelObject.GetReportProperty("DRAWING.REVISION.MARK", ref strdata);

                return strdata;
            }
            if (parm == "TITLE" || parm == "DRAWING_TITLE")
            {
                modelObject.GetReportProperty("DRAWING.TITLE", ref strdata);

                return strdata;
            }
            if (parm.IndexOf("UDA:") != -1)
            {
                modelObject.GetReportProperty("DRAWING." + parm.Remove(0, 4), ref strdata);

                if (strdata != "")
                {
                    return strdata;
                }

                modelObject.GetReportProperty("DRAWING." + parm.Remove(0, 4), ref intdata);

                if (intdata != 0)
                {
                    return intdata.ToString();
                }

                modelObject.GetReportProperty("DRAWING." + parm.Remove(0, 4), ref dbldata);

                if (dbldata != 0)
                {
                    return dbldata.ToString();
                }
            }
            if (parm.IndexOf("REV? - ") != -1)
            {
                int rev = 0;

                modelObject.GetReportProperty("DRAWING.REVISION.NUMBER", ref rev);

                if (rev != 0)
                {
                    strdata = parm.Remove(0, 8);

                    return strdata.Remove(strdata.Length - 1);
                }
                else
                {
                    return "";
                }
            }
            if (parm.IndexOf("REVISION? - ") != -1)
            {
                int rev = 0;

                modelObject.GetReportProperty("DRAWING.REVISION.NUMBER", ref rev);

                if (rev != 0)
                {
                    strdata = parm.Remove(0, 13);

                    return strdata.Remove(strdata.Length - 1);
                }
                else
                {
                    return "";
                }
            }
            if (parm.IndexOf("DRAWING_REVISION? - ") != -1)
            {
                int rev = 0;

                modelObject.GetReportProperty("DRAWING.REVISION.NUMBER", ref rev);

                if (rev != 0)
                {
                    strdata = parm.Remove(0, 21);

                    return strdata.Remove(strdata.Length - 1);
                }
                else
                {
                    return "";
                }
            }
            if (parm.IndexOf("TPL:") != -1)
            {
                modelObject.GetReportProperty("DRAWING." + parm.Remove(0, 4), ref strdata);

                if (strdata != "")
                {
                    return strdata;
                }

                modelObject.GetReportProperty("DRAWING." + parm.Remove(0, 4), ref intdata);

                if (intdata == 0)
                {
                    return intdata.ToString();
                }

                modelObject.GetReportProperty("DRAWING." + parm.Remove(0, 4), ref dbldata);

                if (dbldata == 0)
                {
                    return dbldata.ToString();
                }
            }

            return "";
        }

    }
}
