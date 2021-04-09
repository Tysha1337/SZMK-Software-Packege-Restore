using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.Desktop.ViewModel
{
    public class RevisionViewModel
    {
        private long _Id;
        private DateTime _DateCreate;
        private string _CreatedBy;
        private string _Information;
        private string _Description;
        private string _LastApprovedBy;

        public RevisionViewModel(string DateCreate, string CreatedBy, string Information, string Description, string LastApprovedBy, string List)
        {
            string Error = "";
            try
            {
                if (List.Split('и').Length > 1)
                {
                    if (!String.IsNullOrEmpty(DateCreate))
                    {
                        _DateCreate = Convert.ToDateTime(DateCreate);
                    }
                    else
                    {
                        DateCreate = "01.01.1970";
                        Error = $"Не заполнено поле \"Дата\" в редакции";
                        throw new Exception();
                    }
                    if (!String.IsNullOrEmpty(CreatedBy))
                    {
                        _CreatedBy = CreatedBy;
                    }
                    else
                    {
                        Error = $"Не заполнено поле \"Выполнил\" в редакции";
                        throw new Exception();
                    }
                    if (!String.IsNullOrEmpty(Information))
                    {
                        _Information = Information;
                    }
                    else
                    {
                        Error = $"Не заполнен номер СЗ в поле \"№ Док\" в редакции";
                        throw new Exception();
                    }
                    if (!String.IsNullOrEmpty(Description))
                    {
                        _Description = Description;
                    }
                    else
                    {
                        Error = $"Не заполнено описание в поле \"Описание\" в редакции";
                        throw new Exception();
                    }
                    if (!String.IsNullOrEmpty(LastApprovedBy))
                    {
                        _LastApprovedBy = LastApprovedBy;
                    }
                    else
                    {
                        Error = $"Не заполнено основание изменения в поле \"Утвердил\" в редакции";
                        throw new Exception();
                    }
                }
                else
                {
                    _DateCreate = new DateTime(1970, 1, 1);
                    _CreatedBy = "Исполнитель не найден";
                    _Information = "Информация не найдена";
                    _Description = "Описание не найдено";
                    _LastApprovedBy = "Основание не найдено";
                }
            }
            catch
            {
                throw new Exception(Error);
            }
        }

        public long Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (value > 0)
                {
                    _Id = value;
                }
            }
        }
        public DateTime DateCreate
        {
            get
            {
                return _DateCreate;
            }
            set
            {
                _DateCreate = value;
            }
        }
        public string CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _CreatedBy = value;
                }
            }
        }
        public string Information
        {
            get
            {
                return _Information;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Information = value;
                }
            }
        }
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _Description = value;
                }
            }
        }
        public string LastApprovedBy
        {
            get
            {
                return _LastApprovedBy;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _LastApprovedBy = value;
                }
            }
        }
    }
}
