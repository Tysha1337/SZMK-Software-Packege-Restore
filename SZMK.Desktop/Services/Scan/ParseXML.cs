using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SZMK.Desktop.BindingModels;
using SZMK.Desktop.Models;
using SZMK.Desktop.ViewModel;
using SZMK.Desktop.Views.KB;
using SZMK.Desktop.Views.Shared;
using SZMK.Desktop.Views.Shared.Interfaces;

namespace SZMK.Desktop.Services.Scan
{
    public class ParseXML : BaseScanOrder
    {
        private INotifyProcess notify;
        public List<Order> Orders { get; set; }
        public List<StringErrorBindingModels> WarningOrders { get; set; }
        public List<StringErrorBindingModels> ErrorOrders { get; set; }
        public List<TreeNode> TreeNodes { get; set; }
        public List<OrderScanSession> OrderScanSession { get; set; }
        public void Start(string FileName, string ModelPath, INotifyProcess notify)
        {
            try
            {
                this.notify = notify;

                notify.SetMaximum(1);
                notify.Notify(0, "Начато создание объектов");

                Orders = new List<Order>();
                TreeNodes = new List<TreeNode>();

                notify.Notify(0, "Создание объектов успешно завершено");

                Model Model = GetModel(ModelPath);
                Orders = GetOrders(FileName, Model);

                TreeNodes.Add(GetTreeNodeModel(Model, Orders));
            }
            catch (Exception Ex)
            {
                notify.CloseAsync();

                Orders.Clear();
                TreeNodes.Clear();

                throw new Exception(Ex.Message, Ex);
            }
        }
        private Model GetModel(string path)
        {
            try
            {
                string ModelPath = path;

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

                return new Model { ID = 0, DateCreate = DateTime.Now, Path = ModelPath };
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private List<Order> GetOrders(string FileName, Model Model)
        {
            try
            {
                List<Order> SucsessfulOrders = new List<Order>();
                WarningOrders = new List<StringErrorBindingModels>();
                ErrorOrders = new List<StringErrorBindingModels>();

                XDocument doc = XDocument.Load(FileName);

                int CountIter = 1;

                int CountDrawing = doc.Element("Export").Elements("Сборка").Count();

                notify.SetMaximum(CountDrawing);
                string Number = "";
                string List = "";
                string Mark = "";
                string Executor = "";
                string Lenght = "";
                string Weight = "";
                string CountMarks = "";
                string Error = "";

                foreach (var assembly in doc.Element("Export").Elements("Сборка"))
                {
                    notify.Notify(CountIter, $"Обработка {CountIter} чертежа из {CountDrawing}");

                    try
                    {
                        Error = "Заказ";
                        Number = assembly.Element("Заказ").Value.Trim();
                        Error = "Лист";
                        List = assembly.Element("Лист").Value.Trim();
                        Error = "Марка";
                        Mark = assembly.Element("Марка").Value.Trim();
                        if (CheckMark(Mark))
                        {
                            throw new Exception(@"В написании марки нельзя использовать / \ * : ? | "" < > _ ");
                        }
                        Error = "Разработчик_чертежа";
                        Executor = assembly.Element("Разработчик_чертежа").Value.Trim();
                        Error = "Г.М_длина";
                        Lenght = assembly.Element("Деталь").Element("Г.М_длина").Value.Trim();
                        Error = "Масса_итого";
                        Weight = assembly.Element("Масса_итого").Value.Trim();
                        Error = "Кол_во_марок";
                        CountMarks = assembly.Element("Кол_во_марок").Value.Trim();

                        Order CheckedOrder = new Order(0, DateTime.Now, Number, Executor, "Исполнитель не определен", List, Mark, Lenght, Weight, GetWeightDifferent(Number, List, Mark, Weight), null, DateTime.Now, null, Model, GetRevision(assembly, Number, List), null, null, false, false, CountMarks, new List<Detail>());

                        GetDetails(CheckedOrder.Details, assembly);

                        SucsessfulOrders.Add(CheckedOrder);
                    }
                    catch (NullReferenceException Ex)
                    {
                        ErrorOrders.Add(new StringErrorBindingModels { Order = Number.Trim(), List = List.Trim(), Error = $"Отсутвует поле {Error}" });
                    }
                    catch (Exception Ex)
                    {
                        ErrorOrders.Add(new StringErrorBindingModels { Order = Number.Trim(), List = List.Trim(), Error = Ex.Message });
                    }
                }

                return SucsessfulOrders;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void GetDetails(List<Detail> details, XElement assembly)
        {
            string Error = "";

            try
            {
                foreach (var detail in assembly.Elements("Деталь"))
                {
                    Error = "ADVANCED_OPTION.XS_DRAWING_PLOT_FILE_NAME_W";
                    string Name = GetNameDetail(assembly, detail);

                    Error = "Позиция_детали";
                    string Position = detail.Element("Позиция_детали").Value;

                    Error = "Кол_во_деталей";
                    string Count = detail.Element("Кол_во_деталей").Value;

                    Error = "Профиль";
                    string Profile = detail.Element("Профиль").Value;

                    Error = "Ширина";
                    string Width = detail.Element("Ширина").Value;

                    Error = "Длина";
                    string Lenght = detail.Element("Длина").Value;

                    Error = "Масса1шт";
                    string Weight = detail.Element("Масса1шт").Value;

                    Error = "HEIGHT";
                    string Height = detail.Element("HEIGHT").Value;

                    Error = "DIAMETER";
                    string Diameter = detail.Element("DIAMETER").Value;

                    Error = "Масса_всех";
                    string SubtotalWeight = detail.Element("Масса_всех").Value;

                    Error = "Марка_стали";
                    string MarkSteel = detail.Element("Марка_стали").Value;

                    Error = "Примечание";
                    string Discription = detail.Element("Примечание").Value;

                    Error = "Мех.обр";
                    string Machining = detail.Element("Мех.обр").Value;

                    Error = "Способ_покраски_RAL";
                    string MethodOfPaintingRAL = detail.Element("Способ_покраски_RAL").Value;

                    Error = "Площадь_покраски";
                    string PaintingArea = detail.Element("Площадь_покраски").Value;

                    Error = "GOST_NAME";
                    string GostName = detail.Element("GOST_NAME").Value;

                    Error = "FLANGE_THICKNESS_1";
                    string FlangeThickness = detail.Element("FLANGE_THICKNESS_1").Value;

                    Error = "PLATE_THICKNESS";
                    string PlateThickness = detail.Element("PLATE_THICKNESS").Value;

                    DetailViewModel detailViewModel = new DetailViewModel(
                        Name,
                        Position,
                        Count,
                        Profile,
                        Width,
                        Lenght,
                        Weight,
                        Height,
                        Diameter,
                        SubtotalWeight,
                        MarkSteel,
                        Discription,
                        Machining,
                        MethodOfPaintingRAL,
                        PaintingArea,
                        GostName,
                        FlangeThickness,
                        PlateThickness
                        );

                    details.Add(new Detail
                    {
                        Name = detailViewModel.Name,
                        Position = detailViewModel.Position,
                        Count = detailViewModel.Count,
                        Profile = detailViewModel.Profile,
                        Width = detailViewModel.Width,
                        Lenght = detailViewModel.Lenght,
                        Weight = detailViewModel.Weight,
                        Height = detailViewModel.Height,
                        Diameter = detailViewModel.Diameter,
                        SubtotalWeight = detailViewModel.SubTotalWeight,
                        MarkSteel = detailViewModel.MarkSteel,
                        Discription = detailViewModel.Discription,
                        Machining = detailViewModel.Machining,
                        MethodOfPaintingRAL = detailViewModel.MethodOfPaintiongRAL,
                        PaintingArea = detailViewModel.PaintingArea,
                        GostName = detailViewModel.GostName,
                        FlangeThickness = detailViewModel.FlangeThickness,
                        PlateThickness = detailViewModel.PlateThickness
                    });
                }
            }
            catch (NullReferenceException Ex)
            {
                throw new Exception($"В детали Отсутвует поле: {Error}");
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private TreeNode GetTreeNodeModel(Model Model, List<Order> IterOrders)
        {
            try
            {
                TreeNode tnModel = new TreeNode();
                tnModel.Text = "Модель: " + Model.Path;

                for (int i = 0; i < IterOrders.Count; i++)
                {
                    tnModel.Nodes.Add(GetTreeNodeOrder(IterOrders[i]));
                }

                return tnModel;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private TreeNode GetTreeNodeOrder(Order Order)
        {
            try
            {
                TreeNode Drawing = new TreeNode($"Номер: {Order.Number}, Лист: {Order.List}, Марка: {Order.Mark}");

                TreeNode Number = new TreeNode($"Номер: {Order.Number}");
                Drawing.Nodes.Add(Number);
                TreeNode List = new TreeNode($"Лист: {Order.List}");
                Drawing.Nodes.Add(List);
                TreeNode Mark = new TreeNode($"Марка: {Order.Mark}");
                Drawing.Nodes.Add(Mark);
                TreeNode Executor = new TreeNode($"Исполнитель: {Order.Executor}");
                Drawing.Nodes.Add(Executor);
                TreeNode Lenght = new TreeNode($"Длина: {Order.Lenght}");
                Drawing.Nodes.Add(Lenght);
                TreeNode Weight = new TreeNode($"Вес: {Order.Weight}");
                Drawing.Nodes.Add(Weight);

                Drawing.Nodes.Add(GetTreeNodeDetails(Order.Details));

                return Drawing;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private TreeNode GetTreeNodeDetails(List<Detail> Details)
        {
            try
            {
                TreeNode tnDetails = new TreeNode("Детали");

                for (int i = 0; i < Details.Count; i++)
                {
                    TreeNode Detail = new TreeNode($"Позиция: {Details[i].Position}");

                    TreeNode Count = new TreeNode($"Кол-во: {Details[i].Count}");
                    Detail.Nodes.Add(Count);
                    TreeNode Profile = new TreeNode($"Профиль: {Details[i].Profile}");
                    Detail.Nodes.Add(Profile);
                    TreeNode Width = new TreeNode($"Высота: {Details[i].Width}");
                    Detail.Nodes.Add(Width);
                    TreeNode Lenght = new TreeNode($"Длина: {Details[i].Lenght}");
                    Detail.Nodes.Add(Lenght);
                    TreeNode Weight = new TreeNode($"Вес: {Details[i].Weight}");
                    Detail.Nodes.Add(Weight);
                    TreeNode SubtotalWeight = new TreeNode($"Общий вес: {Details[i].SubtotalWeight}");
                    Detail.Nodes.Add(SubtotalWeight);
                    TreeNode MarkSteel = new TreeNode($"Марка стали: {Details[i].MarkSteel}");
                    Detail.Nodes.Add(MarkSteel);
                    TreeNode Discription = new TreeNode($"Примечание: {Details[i].Discription}");
                    Detail.Nodes.Add(Discription);
                    TreeNode Machining = new TreeNode($"Мех. обр.: {Details[i].Machining}");
                    Detail.Nodes.Add(Machining);
                    TreeNode MethodOfPaintingRAL = new TreeNode($"Способ покрасики RAL: {Details[i].MethodOfPaintingRAL}");
                    Detail.Nodes.Add(MethodOfPaintingRAL);
                    TreeNode PaintingArea = new TreeNode($"Площадь покраски: {Details[i].PaintingArea}");
                    Detail.Nodes.Add(PaintingArea);
                    TreeNode Name = new TreeNode($"Наименование детали: {Details[i].Name}");
                    Detail.Nodes.Add(Name);

                    tnDetails.Nodes.Add(Detail);
                }

                return tnDetails;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        public void CheckedData()
        {
            try
            {
                notify.SetMaximum(Orders.Count);
                OrderScanSession = new List<OrderScanSession>();
                for (int i = 0; i < Orders.Count; i++)
                {
                    notify.Notify(i, $"Проверка {i + 1} чертежа из {Orders.Count}");

                    SetResult(Orders[i], OrderScanSession, true);
                }
            }
            catch (Exception Ex)
            {
                notify.CloseAsync();
                OrderScanSession.Clear();
                throw new Exception(Ex.Message, Ex);
            }
        }
        public bool CheckDetails()
        {
            List<OrderPathDetailsBindingModel> pathDetails = new UnLoadSpecific().CheckDetails(Orders);

            KB_ReportCheckDetail report = new KB_ReportCheckDetail(pathDetails);

            if (report.ShowDialog() == DialogResult.OK)
            {
                foreach (var path in pathDetails)
                {
                    foreach (var drawing in Orders.Where(p => p.Number == path.Order))
                    {
                        drawing.PathDetails = new PathDetails { DateCreate = DateTime.Now, PathDWG = path.PathDWG, PathPDF = path.PathPDF, PathDXF = path.PathDXF };
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckMark(string Mark)
        {
            char[] nonchars = new char[] { '/', '\\', '*', ':', '?', '|', '"', '<', '>', '_' };

            for (int i = 0; i < nonchars.Length; i++)
            {
                if (Mark.IndexOf(nonchars[i]) != -1)
                {
                    return true;
                }
            }

            return false;
        }
        private String GetNameDetail(XElement assembly, XElement detail)
        {
            string _name = assembly.Element("ADVANCED_OPTION.XS_DRAWING_PLOT_FILE_NAME_W").Value.Trim();

            string temp = _name;

            while (true)
            {
                temp = temp.Remove(0, _name.IndexOf('%') + 1);

                string parm = temp.Substring(0, temp.IndexOf('%'));

                _name = _name.Replace($"%{parm}%", GetParametersDetail(detail, parm));

                temp = _name;

                if (temp.IndexOf('%') == -1)
                {
                    break;
                }
            }

            return _name;
        }

        private String GetParametersDetail(XElement detail, string parm)
        {
            string strdata = "";

            if (parm == "NAME" || parm == "DRAWING_NAME")
            {
                string prefix = detail.Element("PART_PREFIX").Value.Trim();

                if (prefix != "")
                {
                    strdata = detail.Element("Позиция_детали").Value.Trim();

                    return prefix + "_" + strdata;
                }
                else
                {
                    strdata = detail.Element("Позиция_детали").Value.Trim();

                    return strdata;
                }
            }
            if (parm == "NAME.-" || parm == "DRAWING_NAME.-")
            {
                string prefix = detail.Element("PART_PREFIX").Value.Trim();

                if (prefix != "")
                {
                    strdata = detail.Element("Позиция_детали").Value.Trim();

                    return prefix + "-" + strdata;
                }
                else
                {
                    strdata = detail.Element("Позиция_детали").Value.Trim();

                    return strdata;
                }
            }
            if (parm == "NAME." || parm == "DRAWING_NAME.")
            {
                string prefix = detail.Element("PART_PREFIX").Value.Trim();

                if (prefix != "")
                {
                    strdata = detail.Element("Позиция_детали").Value.Trim();

                    return prefix + strdata;
                }
                else
                {
                    strdata = detail.Element("Позиция_детали").Value.Trim();

                    return strdata;
                }
            }
            if (parm == "REV" || parm == "REVISION" || parm == "DRAWING_REVISION")
            {
                strdata = detail.Element("DRAWING.REVISION.NUMBER").Value.Trim();

                return strdata;
            }
            if (parm == "REV_MARK" || parm == "REVISION_MARK" || parm == "DRAWING_REVISION_MARK")
            {
                strdata = detail.Element("DRAWING.REVISION.MARK").Value.Trim();

                return strdata;
            }
            if (parm == "TITLE" || parm == "DRAWING_TITLE")
            {
                strdata = detail.Element("DRAWING.TITLE").Value.Trim();

                return strdata;
            }
            if (parm.IndexOf("UDA:") != -1)
            {
                strdata = detail.Element("DRAWING." + parm.Remove(0, 4)).Value.Trim();

                return strdata;
            }
            if (parm.IndexOf("REV? - ") != -1)
            {
                int rev = 0;

                rev = Convert.ToInt32(detail.Element("DRAWING.REVISION.NUMBER").Value.Trim());

                if (rev != 0)
                {
                    strdata = parm.Remove(0, 7);

                    return strdata;
                }
                else
                {
                    return "";
                }
            }
            if (parm.IndexOf("REVISION? - ") != -1)
            {
                int rev = 0;

                rev = Convert.ToInt32(detail.Element("DRAWING.REVISION.NUMBER").Value.Trim());

                if (rev != 0)
                {
                    strdata = parm.Remove(0, 12);

                    return strdata;
                }
                else
                {
                    return "";
                }
            }
            if (parm.IndexOf("DRAWING_REVISION? - ") != -1)
            {
                int rev = 0;

                rev = Convert.ToInt32(detail.Element("DRAWING.REVISION.NUMBER").Value.Trim());

                if (rev != 0)
                {
                    strdata = parm.Remove(0, 20);

                    return strdata;
                }
                else
                {
                    return "";
                }
            }
            if (parm.IndexOf("TPL:") != -1)
            {
                strdata = detail.Element("DRAWING." + parm.Remove(0, 4)).Value.Trim();

                return strdata;
            }

            return "";
        }

        private Revision GetRevision(XElement assembly, string Number, string List)
        {
            string Error = "";

            string _dateCreate = "";
            string _createdBy = "";
            string _information = "";
            string _description = "";
            string _lastApptovedBy = "";

            try
            {
                try
                {
                    Error = "Не найдено поле \"DRAWING.REVISION.DATE_CREATE\"";
                    _dateCreate = assembly.Element("DRAWING.REVISION.DATE_CREATE").Value.Trim();
                }
                catch
                {
                    _dateCreate = "01.01.1970";
                }

                try
                {
                    Error = "Не найдено поле \"DRAWING.REVISION.CREATED_BY\"";
                    _createdBy = assembly.Element("DRAWING.REVISION.CREATED_BY").Value.Trim();
                }
                catch
                {
                    _createdBy = "Исполнитель не найден";
                }

                try
                {
                    Error = "Не найдено поле \"DRAWING.REVISION.INFO2\"";
                    _information = assembly.Element("DRAWING.REVISION.INFO2").Value.Trim();
                }
                catch
                {
                    _information = "Информация не найдена";
                }

                try
                {
                    Error = "Не найдено поле \"DRAWING.REVISION.DESCRIPTION\"";
                    _description = assembly.Element("DRAWING.REVISION.DESCRIPTION").Value.Trim();
                }
                catch
                {
                    _description = "Описание не найдено";
                }

                try
                {
                    Error = "Не найдено поле \"DRAWING.REVISION.LAST_APPROVED_BY\"";
                    _lastApptovedBy = assembly.Element("DRAWING.REVISION.LAST_APPROVED_BY").Value.Trim();
                }
                catch
                {
                    _lastApptovedBy = "Основание не найдено";
                }

                RevisionViewModel viewModel = new RevisionViewModel(_dateCreate, _createdBy, _information, _description, _lastApptovedBy, List);

                return new Revision
                {
                    DateCreate = viewModel.DateCreate,
                    CreatedBy = viewModel.CreatedBy,
                    Information = viewModel.Information,
                    Description = viewModel.Description,
                    LastApptovedBy = viewModel.LastApprovedBy
                };
            }
            //catch (NullReferenceException Ex)
            //{
            //    throw new Exception(Error, Ex);
            //}
            catch (Exception Ex)
            {
                WarningOrders.Add(new StringErrorBindingModels { Order = Number, List = List, Error = Ex.Message });

                return new Revision
                {
                    DateCreate = Convert.ToDateTime(_dateCreate),
                    CreatedBy = _createdBy,
                    Information = _information,
                    Description = _description,
                    LastApptovedBy = _lastApptovedBy
                };
            }
        }
        private string GetWeightDifferent(string Number, string List, string Mark, string Weight)
        {

            double previosWeight = SystemArgs.Request.GetWeightPreviousOrder(Number, List, Mark);

            if (previosWeight != 0)
            {
                previosWeight = Convert.ToDouble(Weight.Trim().Replace(".", ",")) - previosWeight;
            }

            return previosWeight.ToString("F2");
        }
    }
}
