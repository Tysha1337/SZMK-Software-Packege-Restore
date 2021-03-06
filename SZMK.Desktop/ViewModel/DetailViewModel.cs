﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.Desktop.ViewModel
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
        private string _Height;
        private string _Diameter;
        private double _SubtotalWeight;
        private string _MarkSteel;
        private string _Discription;
        private string _Machining;
        private string _MethodOfPaintingRAL;
        private double _PaintingArea;
        private string _GostName;
        private string _FlangeThickness;
        private string _PlateThickness;


        public DetailViewModel(string Name, string Position, string Count, string Profile, string Width, string Lenght, string Weight, string Height, string Diameter, string SubtotalWeight, string MarkSteel, string Discription, string Machining, string MethodOfPaintingRAL, string PaintingArea, string GostName, string FlangeThickness, string PlateThickness)
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

                Error = $"Позиция {_Position}: Высота детали должна быть целым или вещественным числом";
                _Width = Convert.ToDouble(Width.Trim().Replace(".", ","));

                Error = $"Позиция {_Position}: Длина детали должна быть целым или вещественным числом";
                _Lenght = Convert.ToDouble(Lenght.Trim().Replace(".", ","));

                if (_Lenght == 0)
                {
                    Error = $"Позиция {_Position}: Длина детали не может быть равна 0";
                    throw new Exception();
                }

                Error = $"Позиция {_Position}: Вес детали должна быть целым или вещественным числом";
                _Weight = Convert.ToDouble(Weight.Trim().Replace(".", ","));

                _Height = Height.Trim().Replace(".", ",");

                _Diameter = Diameter.Trim().Replace(".", ",");

                Error = $"Позиция {_Position}: Итоговый вес детали должна быть целым или вещественным числом";
                _SubtotalWeight = Convert.ToDouble(SubtotalWeight.Trim().Replace(".", ","));

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

                _Machining = Machining.Trim();

                _MethodOfPaintingRAL = MethodOfPaintingRAL.Trim();

                Error = $"Позиция {_Position}: Площадь покраски детали должна быть целым или вещественным числом";
                _PaintingArea = Convert.ToDouble(PaintingArea.Trim().Replace(".", ","));

                if (String.IsNullOrEmpty(GostName))
                {
                    Error = $"Позиция {_Position}: Не заполнен гост стали детали";
                    throw new Exception();
                }
                else
                {
                    _GostName = GostName.Trim();
                }

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
        public string Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
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
                        double var4 = Convert.ToDouble(Profile.Replace('.',',').Substring(2, Profile.IndexOf("x") - 2));
                        double var5 = Convert.ToDouble(Profile.Replace('.', ',').Substring(1 + Profile.IndexOf("x"), Profile.Length - Profile.IndexOf("x") - 1));

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
    }
}
