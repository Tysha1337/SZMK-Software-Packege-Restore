using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZMK.TeklaInteraction.Shared.ViewModels
{
    public class DetailViewModel
    {
        private long _ID;
        private string _Name;
        private string _Position;
        private long _Count;
        private string _Profile;
        private double _Width;
        private double _Lenght;
        private double _Weight;
        private double _Height;
        private string _Diameter;
        private double _SubtotalWeight;
        private string _MarkSteel;
        private string _Discription;
        private double _GMlenght;
        private double _GMwidth;
        private double _GMheight;
        private string _Machining;
        private string _MethodOfPaintingRAL;
        private double _PaintingArea;
        private string _GostName;
        private string _FlangeThickness;
        private string _PlateThickness;


        public DetailViewModel(string Name, string Position, string Count, string Profile, double Width, double Lenght, double Weight, double Height, string Diameter, string MarkSteel, string Discription, double GMLenght, double GMWidth, double GMHeight, string Machining, string MethodOfPaintingRAL, double PaintingArea, string GostName, string FlangeThickness, string PlateThickness)
        {
            string Error = "";
            try
            {
                _Name = Name;

                if (String.IsNullOrEmpty(Position))
                {
                    Error = $"Не заполнена позиция детали";
                    throw new Exception();
                }
                else
                {
                    _Position = Position.Trim();
                }

                Error = $"Позиция {_Position}: Количество деталей должно быть целым числом";
                _Count = Convert.ToInt32(Count.Trim());

                _Width = Width;

                _Lenght = Lenght;

                _Weight = Weight;

                _Height = Height;

                _Diameter = Diameter.Trim();

                _SubtotalWeight = _Count * _Weight;

                if (String.IsNullOrEmpty(MarkSteel))
                {
                    Error = $"Позиция {_Position}: Не заполнена марка стали детали";
                    throw new Exception();
                }
                else
                {
                    _MarkSteel = MarkSteel.Trim();
                }

                _Discription = Discription.Trim();

                _GMlenght = GMLenght;

                _GMwidth = GMHeight;

                _GMheight = GMHeight;

                _Machining = Machining.Trim();

                _MethodOfPaintingRAL = MethodOfPaintingRAL.Trim();

                _PaintingArea = PaintingArea;

                _GostName = GetGostName(GostName, Profile);

                _FlangeThickness = FlangeThickness.Trim();
                _PlateThickness = PlateThickness.Trim();

                if (String.IsNullOrEmpty(Profile))
                {
                    Error = $"Позиция {_Position}: Не заполнен профиль детали";
                    throw new Exception();
                }
                else
                {
                    _Profile = GetProfile(Profile.Trim());
                }

            }
            catch
            {
                throw new Exception(Error);
            }
        }

        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value > 0)
                {
                    _ID = value;
                }
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public string Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }
        public long Count
        {
            get
            {
                return _Count;
            }
            set
            {
                if (value > 0)
                {
                    _Count = value;
                }
            }
        }
        public string Profile
        {
            get
            {
                return _Profile;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Profile = value;
                }
            }
        }
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (value > 0)
                {
                    _Width = value;
                }
            }
        }
        public double Lenght
        {
            get
            {
                return _Lenght;
            }
            set
            {
                if (value > 0)
                {
                    _Lenght = value;
                }
            }
        }
        public double Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                if (value > 0)
                {
                    _Weight = value;
                }
            }
        }
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (value > 0)
                {
                    _Height = value;
                }
            }
        }
        public string Diameter
        {
            get
            {
                return _Diameter;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Diameter = value;
                }
            }
        }
        public double SubTotalWeight
        {
            get
            {
                return _SubtotalWeight;
            }
            set
            {
                if (value > 0)
                {
                    _SubtotalWeight = value;
                }
            }
        }
        public string MarkSteel
        {
            get
            {
                return _MarkSteel;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _MarkSteel = value;
                }
            }
        }
        public string Discription
        {
            get
            {
                return _Discription;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Discription = value;
                }
            }
        }
        public double GMLenght
        {
            get
            {
                return _GMlenght;
            }
            set
            {
                if (value > 0)
                {
                    _GMlenght = value;
                }
            }
        }
        public double GMWidth
        {
            get
            {
                return _GMwidth;
            }
            set
            {
                if (value > 0)
                {
                    _GMwidth = value;
                }
            }
        }
        public double GMHeight
        {
            get
            {
                return _GMheight;
            }
            set
            {
                if (value > 0)
                {
                    _GMheight = value;
                }
            }
        }
        public string Machining
        {
            get
            {
                return _Machining;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Machining = value;
                }
            }
        }
        public string MethodOfPaintiongRAL
        {
            get
            {
                return _MethodOfPaintingRAL;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _MethodOfPaintingRAL = value;
                }
            }
        }
        public double PaintingArea
        {
            get
            {
                return _PaintingArea;
            }
            set
            {
                if (value > 0)
                {
                    _PaintingArea = value;
                }
            }
        }
        public string GostName
        {
            get
            {
                return _GostName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _GostName = value;
                }
            }
        }
        public string FlangeThickness
        {
            get
            {
                return _FlangeThickness;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _FlangeThickness = value;
                }
            }
        }
        public string PlateThickness
        {
            get
            {
                return _PlateThickness;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _PlateThickness = value;
                }
            }
        }

        private string GetProfile(string Profile)
        {
            try
            {
                int Index = -1;

                string[] Arguments = new string[] { "PL" };
                for (int i = 0; i < Arguments.Length; i++)
                {
                    if (Profile.IndexOf(Arguments[i]) != -1)
                    {
                        Index = i;
                        break;
                    }
                }
                switch (Index)
                {
                    case 0:
                        double var4 = Convert.ToDouble(Profile.Replace('.', ',').Substring(2, Profile.IndexOf("*") - 2));
                        double var5 = Convert.ToDouble(Profile.Replace('.', ',').Substring(1 + Profile.IndexOf("*"), Profile.Length - Profile.IndexOf("*") - 1));

                        if (var4 > var5)
                        {
                            return "-" + var5.ToString();
                        }
                        else
                        {
                            return "-" + var4.ToString();
                        }
                }
                return Profile;
            }
            catch
            {
                return Profile;
            }
        }
        private string GetGostName(string GostName, string Profile)
        {
            try
            {
                if (!String.IsNullOrEmpty(GostName.Replace(" ", "")))
                {
                    return GostName;
                }
                else
                {
                    int Index = -1;

                    string[] Arguments = new string[] { "PL", "ПВ", "Риф", "Чеч" };
                    for (int i = 0; i < Arguments.Length; i++)
                    {
                        if (Profile.IndexOf(Arguments[i]) != -1)
                        {
                            Index = i;
                            break;
                        }
                    }
                    switch (Index)
                    {
                        case 0:
                            if (_MarkSteel.IndexOf("P34") == -1)
                            {
                                return "ГОСТ 19903-74";
                            }
                            else
                            {
                                return "";
                            }
                        case 1:
                            return "ТУ 36.26.11-5-89";
                        case 2:
                            return "ГОСТ 8568-77";
                        case 3:
                            return "ГОСТ 8568-77";
                    }

                    return GostName;
                }
            }
            catch
            {
                return GostName;
            }
        }
    }
}
