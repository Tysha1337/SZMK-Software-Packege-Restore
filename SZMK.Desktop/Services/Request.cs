﻿using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using SZMK.Desktop.Models;
using SZMK.Desktop.BindingModels;

namespace SZMK.Desktop.Services
{
    /*Класс реализованный для выполненя запросов к базе данных, практически все запросы к базе реализованы в этом классе*/
    public class Request
    {
        private readonly String _ConnectString;

        public Request()
        {
            if (SystemArgs.DataBase != null)
            {
                _ConnectString = SystemArgs.DataBase.ToString();
            }
        }

        public bool GetAllMails()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Name\", \"MidName\", \"SurName\", \"Mail\"" +
                                                           $"FROM public.\"AllMail\"", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Mails.Add(new Mail(Reader.GetInt64(0), Reader.GetString(2), Reader.GetString(3), Reader.GetString(4), Reader.GetDateTime(1), Reader.GetString(5)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetAllPositions()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"Name\"" +
                                                            "FROM public.\"Position\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Positions.Add(new Position(Reader.GetInt64(0), Reader.GetString(1)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckConnect(String ConnectString)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT 1", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Connect.Close();
                                return true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        struct MailsOfUser
        {
            public Int64 ID_User;
            public Int64 ID_Mail;

            public MailsOfUser(Int64 IDUser, Int64 IDMail)
            {
                ID_Mail = IDMail;
                ID_User = IDUser;
            }
        }

        public bool GetAllUsers()
        {
            try
            {
                List<MailsOfUser> Temp = new List<MailsOfUser>(); //Письма всех пользователей

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID_Mail\", \"ID_User\"" +
                                                            "FROM public.\"AddUserMail\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Temp.Add(new MailsOfUser(Reader.GetInt64(1), Reader.GetInt64(0)));
                            }
                        }
                    }

                    using (var Command = new NpgsqlCommand($"SELECT public.\"User\".\"ID\", public.\"User\".\"ID_Position\", public.\"User\".\"DateCreate\"," +
                                                                  $"public.\"User\".\"Name\", public.\"User\".\"MidName\", public.\"User\".\"SurName\"," +
                                                                  $"public.\"DataReg\".\"Login\",public.\"DataReg\".\"HashPass\",public.\"DataReg\".\"UpdPassword\"" +
                                                           "FROM public.\"User\", public.\"DataReg\"" +
                                                           "WHERE public.\"User\".\"ID\" = public.\"DataReg\".\"ID\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Int64 ID = Reader.GetInt64(0);

                                List<MailsOfUser> MailsID = (from p in Temp
                                                             where p.ID_User == ID
                                                             select p).ToList(); //Письма текущего пользователя

                                List<Mail> UserMails = new List<Mail>();

                                foreach (Mail Mail in SystemArgs.Mails)
                                {
                                    foreach (MailsOfUser MailsOfUser in MailsID)
                                    {
                                        if (Mail.ID == MailsOfUser.ID_Mail)
                                        {
                                            UserMails.Add(Mail);
                                        }
                                    }
                                }

                                SystemArgs.Users.Add(new User(ID, Reader.GetString(3), Reader.GetString(4),
                                Reader.GetString(5), Reader.GetDateTime(2), Reader.GetInt64(1), UserMails, Reader.GetString(6), Reader.GetString(7), Reader.GetBoolean(8)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
                return false;
            }
        }

        public bool AddUser(User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"User\"(\"ID_Position\", \"DateCreate\", \"Name\", \"MidName\", \"SurName\")" +
                                                            $"VALUES({User.GetPosition().ID}, '{User.DateCreate}', '{User.Name}', '{User.MiddleName}', '{User.Surname}');", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"DataReg\"(\"ID\", \"DateUpd\", \"Login\", \"HashPass\", \"UpdPassword\")" +
                                                            $"VALUES({User.ID}, '{User.DateCreate}', '{User.Login}', '{User.HashPassword}', {User.UpdPassword}); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeUser(User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"User\"" +
                                                            $"SET \"ID_Position\" = {User.GetPosition().ID}, \"DateCreate\" = '{User.DateCreate}', \"Name\" = '{User.Name}', \"MidName\" = '{User.MiddleName}', \"SurName\" = '{User.Surname}' " +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\"" +
                                                            $"SET \"DateUpd\" = '{User.DateCreate}', \"Login\" = '{User.Login}', \"HashPass\" ='{User.HashPassword}', \"UpdPassword\" = {User.UpdPassword} " +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean UpdatePasswordText(String Password, User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\" " +
                                                            $"SET \"HashPass\" = '{Password}' " +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePassword(User User, Boolean Status)
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"DataReg\" " +
                                                            $"SET \"UpdPassword\" = {Status} " +
                                                            $"WHERE \"ID\" = {User.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(User User)
        {
            try
            {
                if (DeleteOrdersUser(User))
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"DELETE FROM public.\"User\"" +
                                                               $"WHERE \"ID\" = {User.ID}; ", Connect))
                        {
                            Command.ExecuteNonQuery();
                        }

                        Connect.Close();
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteOrdersUser(User User)
        {
            try
            {
                List<Int64> Temp = (from p in GetAllIdOrder(User.ID)
                                    select p).Distinct().ToList();
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    foreach (Int64 ID in Temp)
                    {
                        using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Orders\" WHERE \"Orders\".\"ID\"='{ID}'; ", Connect))
                        {
                            Command.ExecuteNonQuery();
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private List<Int64> GetAllIdOrder(Int64 UserID)
        {
            try
            {
                List<Int64> Temp = new List<Int64>();
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID_Order\" FROM public.\"AddStatus\" WHERE \"AddStatus\".\"ID_User\" = '{UserID}'; ", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Temp.Add(Reader.GetInt64(0));
                            }
                        }
                    }

                    Connect.Close();
                }

                return Temp;
            }
            catch
            {
                throw new Exception("Ошибка получения всех чертежей которые были связаны с данным пользователем");
            }
        }
        public bool DeleteMail(Mail Mail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"AllMail\"" +
                                                           $"WHERE \"ID\" = {Mail.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeMail(Mail Mail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"AllMail\"" +
                                                            $"SET \"DateCreate\" = '{Mail.DateCreate}', \"Name\" ='{Mail.Name}', \"MidName\" ='{Mail.MiddleName}', \"SurName\" ='{Mail.Surname}', \"Mail\" ='{Mail.MailAddress}'" +
                                                            $"WHERE \"ID\" = {Mail.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddMail(Mail Mail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AllMail\"(\"DateCreate\", \"Name\", \"MidName\", \"SurName\", \"Mail\")" +
                                                           $"VALUES('{Mail.DateCreate}', '{Mail.Name}', '{Mail.MiddleName}', '{Mail.Surname}', '{Mail.MailAddress}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertStatus(string Number, string List, long Status_ID, User User)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddStatus\" (\"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\") VALUES('{DateTime.Now}', '{Status_ID}', '{GetIDOrder(Number, List)}', '{User.ID}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateDateCreateStatus(DateTime StatusDate, long UserID, long OrderID, long StatusID)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"AddStatus\" SET \"DateCreate\"='{StatusDate}', \"ID_User\"='{UserID}' WHERE \"ID_Order\"='{OrderID}' AND \"ID_Status\"='{StatusID}';", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DownGradeStatus(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"AddStatus\" WHERE \"ID_Order\"='{Order.ID}' AND \"ID_Status\">='{Order.Status.ID}';", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddStatus\" (\"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\") VALUES('{DateTime.Now}', '{Order.Status.ID}', '{Order.ID}', '{Order.User.ID}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }
                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool StatusExist(long ID_Order, long ID_Status)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\" FROM public.\"AddStatus\" WHERE \"ID_Status\"='{ID_Status}' AND \"ID_Order\"='{ID_Order}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool GetAllStatus()
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"ID_Position\", \"Name\" FROM public.\"Status\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Statuses.Add(new Status(Reader.GetInt64(0), Reader.GetInt64(1), Reader.GetString(2)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool DeleteStatus(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"AddStatus\" WHERE \"ID_Status\" = '{Order.Status.ID}' AND \"ID_Order\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }


                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedOrder(String Number, String List)
        {
            try
            {
                bool flag = true;
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{List}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 1)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return flag;

            }
            catch
            {
                return false;
            }
        }
        public bool CheckedExecutorWork(String Number, String List, String QR)
        {
            try
            {
                bool flag = true;
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ExecutorWork\" FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{List}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetString(0) == QR.Split('_')[1])
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return flag;

            }
            catch
            {
                return false;
            }
        }
        public Int64 CheckedStatusOrderDB(String Number, String List)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    using (var Command = new NpgsqlCommand($"SELECT \"ID_Status\" FROM public.\"AddStatus\" WHERE \"ID_Order\"='{GetIDOrder(Number, List)}' ORDER BY \"DateCreate\" DESC LIMIT 1;", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                return Reader.GetInt64(0);
                            }
                        }
                    }

                    Connect.Close();
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
        public double GetWeightPreviousOrder(string Number, string List, string Mark)
        {
            try
            {
                double weight = 0;

                String ReplaceMark = "";

                String[] ExistingCharaterEnglish = new String[] { "A", "a", "B", "C", "c", "E", "e", "H", "K", "M", "O", "o", "P", "p", "T" };
                String[] ExistingCharaterRussia = new String[] { "А", "а", "В", "С", "с", "Е", "е", "Н", "К", "М", "О", "о", "Р", "р", "Т" };

                for (int i = 0; i < ExistingCharaterRussia.Length; i++)
                {
                    ReplaceMark = Mark.Replace(ExistingCharaterRussia[i], ExistingCharaterEnglish[i]);
                }

                string[] ListSplitted = List.Split('и');

                while (ListSplitted[0][0] == '0')
                {
                    ListSplitted[0] = ListSplitted[0].Remove(0, 1);
                }

                if (ListSplitted.Length == 2 && ListSplitted[1] == "1")
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT \"Weight\" FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{ListSplitted[0]}' AND \"Mark\"='{ReplaceMark}';", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    weight = Convert.ToDouble(Reader.GetString(0));
                                }
                            }
                        }

                        Connect.Close();
                    }
                }
                else if (ListSplitted.Length == 2)
                {
                    using (var Connect = new NpgsqlConnection(_ConnectString))
                    {
                        Connect.Open();

                        using (var Command = new NpgsqlCommand($"SELECT \"Weight\" FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{ListSplitted[0]}и{Convert.ToInt32(ListSplitted[1]) - 1}' AND \"Mark\"='{ReplaceMark}';", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    weight = Convert.ToDouble(Reader.GetString(0));
                                }
                            }
                        }

                        Connect.Close();
                    }
                }

                return weight;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public List<Order> GetHistoryOrders(string Number, string List)
        {
            try
            {
                SystemArgs.Models.Clear();
                SystemArgs.PathDetails.Clear();
                SystemArgs.PathArhives.Clear();
                SystemArgs.Revisions.Clear();
                SystemArgs.Comments.Clear();

                GetAllModels();
                GetAllPathDetails();
                GetAllPathArhive();
                GetAllRevision();
                GetAllComment();

                List<Order> HistoryOrders = new List<Order>();

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Executor\",\"ExecutorWork\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\", \"WeightDifferent\",\"Hide\", \"Canceled\",\"Finished\",\"ID_TypeAdd\", \"ID_Model\", \"ID_PathDetails\",\"ID_PathArhive\",\"ID_Revision\",\"ID_Comment\"" +
                                                            $" FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND (\"List\" LIKE '{List + "и"}%' OR \"List\"='{List}');", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                TypeAdd TempTypeAdd = null;

                                if (!Reader.IsDBNull(13))
                                {
                                    TempTypeAdd = SystemArgs.TypesAdds.FindAll(p => p.ID == Reader.GetInt64(13)).FirstOrDefault();
                                }

                                Model TempModel = null;

                                if (!Reader.IsDBNull(14))
                                {
                                    TempModel = SystemArgs.Models.FindAll(p => p.ID == Reader.GetInt64(14)).FirstOrDefault();
                                }

                                PathDetails TempPathDetails = null;

                                if (!Reader.IsDBNull(15))
                                {
                                    TempPathDetails = SystemArgs.PathDetails.FindAll(p => p.ID == Reader.GetInt64(15)).FirstOrDefault();
                                }

                                PathArhive TempPathArhive = null;

                                if (!Reader.IsDBNull(16))
                                {
                                    TempPathArhive = SystemArgs.PathArhives.FindAll(p => p.ID == Reader.GetInt64(16)).FirstOrDefault();
                                }

                                Revision TempRevision = null;

                                if (!Reader.IsDBNull(17))
                                {
                                    TempRevision = SystemArgs.Revisions.FindAll(p => p.ID == Reader.GetInt64(17)).FirstOrDefault();
                                }

                                Comment TempComment = null;

                                if (!Reader.IsDBNull(18))
                                {
                                    TempComment = SystemArgs.Comments.FindAll(p => p.ID == Reader.GetInt64(18)).FirstOrDefault();
                                }

                                HistoryOrders.Add(new Order(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(2), Reader.GetString(3), Reader.GetString(5), Reader.GetString(6), Convert.ToDouble(Reader.GetString(7)), Convert.ToDouble(Reader.GetString(8)), Convert.ToDouble(Reader.GetString(9)), null, DateTime.Now, TempTypeAdd, TempModel, TempPathDetails, TempPathArhive, TempRevision, TempComment, null, new BlankOrder(), Reader.GetBoolean(10), Reader.GetBoolean(11), Reader.GetBoolean(12)));
                            }
                        }
                    }

                    Connect.Close();
                }
                return HistoryOrders;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool InsertBlankOrder(String QR)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"BlankOrder\"(\"DateCreate\", \"QR\") VALUES('{DateTime.Now}', '{QR}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertBlankOrderOfOrders(List<long> OrdersID, String QR)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();
                    foreach (long ID in OrdersID)
                    {
                        using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddBlank\"(\"DateCreate\", \"ID_BlankOrder\", \"ID_Order\") VALUES('{DateTime.Now}', '{SystemArgs.Request.GetIDBlankOrder(QR)}', '{ID}')", Connect))
                        {
                            Command.ExecuteNonQuery();
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool FindedOrdersInAddBlankOrder(String QR, String Number, String List)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID_Order\") FROM public.\"AddBlank\" WHERE \"ID_BlankOrder\"='{GetIDBlankOrder(QR)}' AND \"ID_Order\"='{GetIDOrder(Number, List)}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) >= 1)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public Int64 GetIDBlankOrder(String QR)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\" FROM public.\"BlankOrder\" WHERE \"QR\"='{QR}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                return Reader.GetInt64(0);
                            }
                        }
                    }

                    Connect.Close();
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
        public bool UpdateBlankOrder(String QR, List<Order> Orders)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"BlankOrder\" SET \"QR\" = '{QR}' WHERE \"BlankOrder\".\"ID\" = '{SystemArgs.RequestLinq.GetOldIDBlankOrder(Orders)}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CanceledOrder(bool Cancelled, long ID)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"Canceled\" = {Cancelled}" +
                                                           $" WHERE \"ID\" = {ID};", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetAllBlankOrder()
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"QR\" FROM public.\"BlankOrder\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.BlankOrders.Add(new BlankOrder(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(2)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool InsertOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"Orders\"(" +
                                                            "\"DateCreate\", \"Executor\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\",\"WeightDifferent\", \"Canceled\" )" +
                                                            $"VALUES('{Order.DateCreate}', '{Order.Executor}', '{Order.Number}', '{Order.List}', '{Order.Mark}', '{Order.Lenght}', '{Order.Weight}', '{Order.WeightDifferent}', {Order.Canceled});", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                return false;
            }
        }
        public bool DeleteOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Orders\" WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"Executor\" = '{Order.Executor}',\"ExecutorWork\" = '{Order.ExecutorWork}', \"Number\" = '{Order.Number}', \"List\" = '{Order.List}', \"Mark\" = '{Order.Mark}', \"Lenght\" = '{Order.Lenght}', \"Weight\" = '{Order.Weight}', \"Hide\" = '{Order.Hide}', \"Canceled\" = '{Order.Canceled}',\"Finished\" = '{Order.Finished}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetAllTypesAdd()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\",\"DateCreate\", \"Discription\"" +
                                        $" FROM public.\"TypeAdd\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.TypesAdds.Add(new TypeAdd { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), Discriprion = Reader.GetString(2) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool GetAllModels()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\",\"Path\"" +
                                        $" FROM public.\"Model\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Models.Add(new Model { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), Path = Reader.GetString(2) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool GetAllOrders()
        {
            try
            {
                SystemArgs.Models.Clear();
                SystemArgs.PathDetails.Clear();
                SystemArgs.PathArhives.Clear();
                SystemArgs.Revisions.Clear();
                SystemArgs.Comments.Clear();

                GetAllModels();
                GetAllPathDetails();
                GetAllPathArhive();
                GetAllRevision();
                GetAllComment();

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Executor\",\"ExecutorWork\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\", \"WeightDifferent\", \"Hide\", \"Canceled\",\"Finished\",\"ID_TypeAdd\", \"ID_Model\", \"ID_PathDetails\", \"ID_PathArhive\", \"ID_Revision\", \"ID_Comment\"" +
                                                            $" FROM public.\"Orders\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                TypeAdd TempTypeAdd = null;

                                if (!Reader.IsDBNull(13))
                                {
                                    TempTypeAdd = SystemArgs.TypesAdds.FindAll(p => p.ID == Reader.GetInt64(13)).FirstOrDefault();
                                }

                                Model TempModel = null;

                                if (!Reader.IsDBNull(14))
                                {
                                    TempModel = SystemArgs.Models.FindAll(p => p.ID == Reader.GetInt64(14)).FirstOrDefault();
                                }

                                PathDetails TempPathDetails = null;

                                if (!Reader.IsDBNull(15))
                                {
                                    TempPathDetails = SystemArgs.PathDetails.FindAll(p => p.ID == Reader.GetInt64(15)).FirstOrDefault();
                                }

                                PathArhive TempPathArhive = null;

                                if (!Reader.IsDBNull(16))
                                {
                                    TempPathArhive = SystemArgs.PathArhives.FindAll(p => p.ID == Reader.GetInt64(16)).FirstOrDefault();
                                }

                                Revision TempRevision = null;

                                if (!Reader.IsDBNull(17))
                                {
                                    TempRevision = SystemArgs.Revisions.FindAll(p => p.ID == Reader.GetInt64(17)).FirstOrDefault();
                                }

                                Comment TempComment = null;

                                if (!Reader.IsDBNull(18))
                                {
                                    TempComment = SystemArgs.Comments.FindAll(p => p.ID == Reader.GetInt64(18)).FirstOrDefault();
                                }

                                SystemArgs.Orders.Add(new Order(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(2), Reader.GetString(3), Reader.GetString(5), Reader.GetString(6), Convert.ToDouble(Reader.GetString(7)), Convert.ToDouble(Reader.GetString(8)), Convert.ToDouble(Reader.GetString(9)), null, DateTime.Now, TempTypeAdd, TempModel, TempPathDetails, TempPathArhive, TempRevision, TempComment, null, new BlankOrder(), Reader.GetBoolean(10), Reader.GetBoolean(11), Reader.GetBoolean(12)));
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public List<Order> GetAllOrdersForReport()
        {
            try
            {
                List<Order> orders = new List<Order>();

                SystemArgs.Models.Clear();
                SystemArgs.PathDetails.Clear();
                SystemArgs.PathArhives.Clear();
                SystemArgs.Revisions.Clear();
                SystemArgs.Comments.Clear();

                GetAllModels();
                GetAllPathDetails();
                GetAllPathArhive();
                GetAllRevision();
                GetAllComment();

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Executor\",\"ExecutorWork\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\", \"WeightDifferent\", \"Hide\", \"Canceled\",\"Finished\",\"ID_TypeAdd\", \"ID_Model\", \"ID_PathDetails\", \"ID_PathArhive\", \"ID_Revision\", \"ID_Comment\"" +
                                                            $" FROM public.\"Orders\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                TypeAdd TempTypeAdd = null;

                                if (!Reader.IsDBNull(13))
                                {
                                    TempTypeAdd = SystemArgs.TypesAdds.FindAll(p => p.ID == Reader.GetInt64(13)).FirstOrDefault();
                                }

                                Model TempModel = null;

                                if (!Reader.IsDBNull(14))
                                {
                                    TempModel = SystemArgs.Models.FindAll(p => p.ID == Reader.GetInt64(14)).FirstOrDefault();
                                }

                                PathDetails TempPathDetails = null;

                                if (!Reader.IsDBNull(15))
                                {
                                    TempPathDetails = SystemArgs.PathDetails.FindAll(p => p.ID == Reader.GetInt64(15)).FirstOrDefault();
                                }

                                PathArhive TempPathArhive = null;

                                if (!Reader.IsDBNull(16))
                                {
                                    TempPathArhive = SystemArgs.PathArhives.FindAll(p => p.ID == Reader.GetInt64(16)).FirstOrDefault();
                                }

                                Revision TempRevision = null;

                                if (!Reader.IsDBNull(17))
                                {
                                    TempRevision = SystemArgs.Revisions.FindAll(p => p.ID == Reader.GetInt64(17)).FirstOrDefault();
                                }

                                Comment TempComment = null;

                                if (!Reader.IsDBNull(18))
                                {
                                    TempComment = SystemArgs.Comments.FindAll(p => p.ID == Reader.GetInt64(18)).FirstOrDefault();
                                }

                                orders.Add(new Order(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(2), Reader.GetString(3), Reader.GetString(5), Reader.GetString(6), Convert.ToDouble(Reader.GetString(7)), Convert.ToDouble(Reader.GetString(8)), Convert.ToDouble(Reader.GetString(9)), null, DateTime.Now, TempTypeAdd, TempModel, TempPathDetails, TempPathArhive, TempRevision, TempComment, null, new BlankOrder(), Reader.GetBoolean(10), Reader.GetBoolean(11), Reader.GetBoolean(12)));
                            }
                        }
                    }

                    Connect.Close();
                }
                return orders;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public long GetLastIDOrder()
        {
            try
            {
                Int64 IndexOrder = -1;

                using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT last_value FROM \"Orders_ID_seq\"", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                IndexOrder = Reader.GetInt64(0);
                            }
                        }
                    }
                }

                return IndexOrder;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public List<long> GetIDOrdersForCancelled(string List, string Number)
        {
            try
            {
                List<long> ids = new List<long>();

                using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\" FROM public.\"Orders\" WHERE(\"List\" LIKE '{List + "и"}%' OR \"List\"='{List}') AND \"Number\"='{Number}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ids.Add(Reader.GetInt64(0));
                            }
                        }
                    }
                }

                return ids;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public Order GetOrder(string List, string Number)
        {
            try
            {
                SystemArgs.Models.Clear();

                GetAllModels();

                SystemArgs.PathDetails.Clear();

                GetAllPathDetails();

                SystemArgs.PathArhives.Clear();

                GetAllPathArhive();

                SystemArgs.Revisions.Clear();

                GetAllRevision();

                SystemArgs.Comments.Clear();

                GetAllComment();

                Order Temp = null;

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Executor\",\"ExecutorWork\", \"Number\", \"List\", \"Mark\", \"Lenght\", \"Weight\", \"WeightDifferent\", \"Hide\", \"Canceled\",\"Finished\",\"ID_TypeAdd\", \"ID_Model\", \"ID_PathDetails\", \"ID_PathArhive\", \"ID_Revision\", \"ID_Comment\"" +
                                                            $" FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND \"List\"='{List}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                TypeAdd TempTypeAdd = null;

                                if (!Reader.IsDBNull(13))
                                {
                                    TempTypeAdd = SystemArgs.TypesAdds.FindAll(p => p.ID == Reader.GetInt64(13)).FirstOrDefault();
                                }

                                Model TempModel = null;

                                if (!Reader.IsDBNull(14))
                                {
                                    TempModel = SystemArgs.Models.FindAll(p => p.ID == Reader.GetInt64(14)).FirstOrDefault();
                                }

                                PathDetails TempPathDetails = null;

                                if (!Reader.IsDBNull(15))
                                {
                                    TempPathDetails = SystemArgs.PathDetails.FindAll(p => p.ID == Reader.GetInt64(15)).FirstOrDefault();
                                }

                                PathArhive TempPathArhive = null;

                                if (!Reader.IsDBNull(16))
                                {
                                    TempPathArhive = SystemArgs.PathArhives.FindAll(p => p.ID == Reader.GetInt64(16)).FirstOrDefault();
                                }

                                Revision TempRevision = null;

                                if (!Reader.IsDBNull(17))
                                {
                                    TempRevision = SystemArgs.Revisions.FindAll(p => p.ID == Reader.GetInt64(17)).FirstOrDefault();
                                }

                                Comment TempComment = null;

                                if (!Reader.IsDBNull(18))
                                {
                                    TempComment = SystemArgs.Comments.FindAll(p => p.ID == Reader.GetInt64(18)).FirstOrDefault();
                                }

                                Temp = new Order(Reader.GetInt64(0), Reader.GetDateTime(1), Reader.GetString(4), Reader.GetString(2), Reader.GetString(3), Reader.GetString(5), Reader.GetString(6), Convert.ToDouble(Reader.GetString(7)), Convert.ToDouble(Reader.GetString(8)), Convert.ToDouble(Reader.GetString(9)), null, DateTime.Now, TempTypeAdd, TempModel, TempPathDetails, TempPathArhive, TempRevision, TempComment, null, new BlankOrder(), Reader.GetBoolean(10), Reader.GetBoolean(11), Reader.GetBoolean(12));
                            }
                        }
                    }

                    Connect.Close();
                }
                return Temp;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }

        public Int64 GetIDOrder(String Number, String List)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\" FROM public.\"Orders\" WHERE \"List\" = '{List}' AND \"Number\"='{Number}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                return Reader.GetInt64(0);
                            }
                        }
                    }

                    Connect.Close();
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public struct IDandWeight
        {
            public Int64 ID { get; set; }
            public double Weight { get; set; }
        }

        public List<IDandWeight> GetWeightOrders()
        {

            try
            {
                List<IDandWeight> weights = new List<IDandWeight>();

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\",\"Weight\" FROM public.\"Orders\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                weights.Add(new IDandWeight { ID = Reader.GetInt64(0), Weight = Convert.ToDouble(Reader.GetString(1)) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return weights;
            }
            catch
            {
                return null;
            }
        }

        public bool FinishedOrder(bool Finished, long ID)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"Finished\"='{Finished}' WHERE \"ID\"='{ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateExecutorWorkOrder(string Executor, long ID)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ExecutorWork\"='{Executor}' WHERE \"ID\"='{ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Int32 CheckedNumberAndList(Order order, bool XML)
        {
            try
            {
                int flag = -1;
                String[] Temp = order.List.Split('и');
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"List\" = '{order.List}' AND \"Number\"='{order.Number}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    if (Temp.Length != 1)
                                    {
                                        if (CheckedFirstCancelledOrder(order.Number, Temp[0]))
                                        {
                                            if (MessageBox.Show("Заменяемый чертеж отсутсвует. Добавить новый?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                            {
                                                flag = 2;
                                            }
                                        }
                                        else
                                        {
                                            flag = 2;
                                        }
                                    }
                                    else
                                    {
                                        flag = 0;
                                    }
                                }
                                else if (CheckedDataMatrixUpdate(order) && XML)
                                {
                                    Order OldOrder = GetOrder(order.List, order.Number);

                                    if (GetIdStatusOrder(OldOrder.ID) >= 3)
                                    {
                                        flag = 6;
                                    }
                                    else
                                    {
                                        flag = 4;
                                    }
                                }
                                else if (CheckedDetailsUpdate(order) && XML)
                                {
                                    Order OldOrder = GetOrder(order.List, order.Number);

                                    if (GetIdStatusOrder(OldOrder.ID) >= 3)
                                    {
                                        flag = 6;
                                    }
                                    else
                                    {
                                        flag = 5;
                                    }
                                }
                                else if ((CheckedStatusOrderDB(order.Number, order.List) != -1) && (CheckedStatusOrderDB(order.Number, order.List) == SystemArgs.User.StatusesUser.First().ID - 1))
                                {
                                    flag = 3;
                                }
                                else
                                {
                                    flag = 1;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return flag;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                return -1;
            }
        }
        private bool CheckedDataMatrixUpdate(Order order)
        {
            bool flag = false;
            using (var Connect = new NpgsqlConnection(_ConnectString))
            {
                Connect.Open();
                using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"Number\"='{order.Number}' AND \"List\"='{order.List}' AND \"Mark\"='{order.Mark}' AND \"Executor\"='{order.Executor}' AND \"Lenght\"='{order.Lenght}' AND \"Weight\"='{order.Weight}';", Connect))
                {
                    using (var Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            if (Reader.GetInt64(0) == 0)
                            {
                                flag = true;
                            }
                        }
                    }
                }
                Connect.Close();
            }
            return flag;
        }
        public bool CheckedDetailsUpdate(Order order)
        {
            bool flag = false;
            Order OldOrder = GetOrder(order.List, order.Number);
            List<Detail> OldDetails = GetDetails(OldOrder.ID);

            if (OldDetails.Count == order.Details.Count)
            {
                flag = true;
            }

            if (flag)
            {

                for (int i = 0; i < OldDetails.Count && flag; i++)
                {
                    if (order.Details.FirstOrDefault(p => p.Position == OldDetails[i].Position) == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (order.Details.FirstOrDefault(p => p.Position == OldDetails[i].Position).MarkSteel != OldDetails[i].MarkSteel)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return !flag;
        }
        private bool CheckedFirstCancelledOrder(String Number, String List)
        {
            bool flag = false;
            using (var Connect = new NpgsqlConnection(_ConnectString))
            {
                Connect.Open();
                using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"Number\"='{Number}' AND (\"List\" = '{List}' OR \"List\" LIKE '{List + "и%"}');", Connect))
                {
                    using (var Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            if (Reader.GetInt64(0) == 0)
                            {
                                flag = true;
                            }
                        }
                    }
                }
                Connect.Close();
            }
            return flag;
        }
        public bool CheckedNumberAndMark(String Number, String Mark)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"Mark\" = '{Mark}' AND \"Number\"='{Number}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool GetAllBlankOrderofOrders()
        {
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_BlankOrder\", \"ID_Order\" FROM public.\"AddBlank\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.BlankOrderOfOrders.Add(new BlankOrderOfOrder(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }

        public bool GetAllStatusOfUser()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DateCreate\", \"ID_Status\", \"ID_Order\", \"ID_User\" FROM public.\"AddStatus\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.StatusOfOrders.Add(new StatusOfOrder(Reader.GetDateTime(0), Reader.GetInt64(1), Reader.GetInt64(2), Reader.GetInt64(3)));
                            }
                        }
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public long GetIdStatusOrder(long IDOrder)
        {
            using (var Connect = new NpgsqlConnection(_ConnectString))
            {
                Connect.Open();
                using (var Command = new NpgsqlCommand($"SELECT LAST_VALUE(\"ID_Status\") OVER(ORDER BY \"DateCreate\" RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) result FROM public.\"AddStatus\" WHERE \"ID_Order\"='{IDOrder}' LIMIT 1;", Connect))
                {
                    using (var Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            return Reader.GetInt64(0);
                        }
                    }
                }
                Connect.Close();
            }

            return -1;
        }
        public Int64 GetAutoIDDetail()
        {
            try
            {
                Int64 Index = 0;
                using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT last_value FROM \"Detail_ID_seq\"", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Index = Reader.GetInt64(0);
                            }
                        }
                    }
                }
                return Index;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                throw new Exception(e.ToString());
            }
        }
        public bool InsertDetail(Detail Detail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"Detail\"(\"Position\",\"Count\",\"Profile\",\"Width\",\"Lenght\",\"Weight\",\"Height\",\"Diameter\",\"SubtotalWeight\",\"MarkSteel\",\"Discription\",\"Machining\", \"MethodOfPaintingRAL\", \"PaintingArea\",\"GostName\",\"FlangeThickness\",\"PlateThickness\",\"Name\") " +
                        $"VALUES('{Detail.Position}','{Detail.Count}','{Detail.Profile}','{Detail.Width}','{Detail.Lenght}','{Detail.Weight}','{Detail.Height}','{Detail.Diameter}','{Detail.SubtotalWeight}','{Detail.MarkSteel}','{Detail.Discription}','{Detail.Machining}', '{Detail.MethodOfPaintingRAL}', '{Detail.PaintingArea}','{Detail.GostName}','{Detail.FlangeThickness}','{Detail.PlateThickness}','{Detail.Name}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                return false;
            }
        }
        public bool InsertAddDetail(Order Order, Detail Detail)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"AddDetail\"(\"DateCreate\", \"OrderID\", \"DetailID\") VALUES('{DateTime.Now}', '{Order.ID}', '{Detail.ID}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                return false;
            }
        }
        public List<Detail> GetDetails(Int64 IDOrder)
        {
            try
            {
                List<Detail> details = new List<Detail>();
                List<Int64> IDDetails = new List<Int64>();
                using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DetailID\" FROM public.\"AddDetail\" WHERE \"OrderID\"='{IDOrder}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                IDDetails.Add(Reader.GetInt64(0));
                            }
                        }
                    }
                    for (int i = 0; i < IDDetails.Count; i++)
                    {
                        using (var Command = new NpgsqlCommand($"SELECT \"ID\",\"Name\",\"Position\",\"Count\", \"Profile\",\"Width\",\"Lenght\",\"Weight\",\"Height\",\"Diameter\", \"SubtotalWeight\", \"MarkSteel\",\"Discription\",\"Machining\",\"MethodOfPaintingRAL\",\"PaintingArea\",\"GostName\",\"FlangeThickness\",\"PlateThickness\" FROM public.\"Detail\" WHERE \"ID\"='{IDDetails[i]}';", Connect))
                        {
                            using (var Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    details.Add(new Detail
                                    {
                                        ID = Reader.GetInt64(0),
                                        Name = !Reader.IsDBNull(1) ? Reader.GetString(1) : "",
                                        Position = !Reader.IsDBNull(2) ? Reader.GetString(2) : "",
                                        Count = !Reader.IsDBNull(3) ? Reader.GetInt64(3) : -1,
                                        Profile = Reader.GetString(4),
                                        Width = !Reader.IsDBNull(5) ? Convert.ToDouble(Reader.GetString(5)) : -1,
                                        Lenght = !Reader.IsDBNull(6) ? Convert.ToDouble(Reader.GetString(6)) : -1,
                                        Weight = !Reader.IsDBNull(7) ? Convert.ToDouble(Reader.GetString(7)) : -1,
                                        Height = !Reader.IsDBNull(8) ? Reader.GetString(8) : "",
                                        Diameter = !Reader.IsDBNull(9) ? Reader.GetString(9) : "",
                                        SubtotalWeight = Convert.ToDouble(Reader.GetString(10)),
                                        MarkSteel = Reader.GetString(11),
                                        Discription = !Reader.IsDBNull(12) ? Reader.GetString(12) : "",
                                        Machining = !Reader.IsDBNull(13) ? Reader.GetString(13) : "",
                                        MethodOfPaintingRAL = !Reader.IsDBNull(14) ? Reader.GetString(14) : "",
                                        PaintingArea = !Reader.IsDBNull(15) ? Convert.ToDouble(Reader.GetString(15)) : -1,
                                        GostName = !Reader.IsDBNull(16) ? Reader.GetString(16) : "",
                                        FlangeThickness = !Reader.IsDBNull(17) ? Reader.GetString(17) : "",
                                        PlateThickness = !Reader.IsDBNull(18) ? Reader.GetString(18) : ""
                                    });
                                }
                            }
                        }
                    }
                    Connect.Close();
                }
                return details;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                throw new Exception(e.ToString());
            }
        }
        public bool DeleteDetails(Int64 IDOrder)
        {
            try
            {
                List<Int64> IDDetails = new List<Int64>();
                using (var Connect = new NpgsqlConnection(SystemArgs.DataBase.ToString()))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"DetailID\" FROM public.\"AddDetail\" WHERE \"OrderID\"='{IDOrder}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                IDDetails.Add(Reader.GetInt64(0));
                            }
                        }
                    }
                    for (int i = 0; i < IDDetails.Count; i++)
                    {
                        using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Detail\" WHERE \"ID\"='{IDDetails[i]}';", Connect))
                        {
                            Command.ExecuteNonQuery();
                        }
                    }
                    Connect.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                throw new Exception(e.ToString());
            }
        }
        public bool InsertModel(Model Model)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"Model\"(\"DateCreate\", \"Path\") VALUES('{Model.DateCreate}', '{Model.Path}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool ModelExist(Model Model)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Path\" FROM public.\"Model\" WHERE \"Path\"='{Model.Path}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Model GetModel(Model Model)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Path\" FROM public.\"Model\" WHERE \"Path\"='{Model.Path}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Model = new Model { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), Path = Reader.GetString(2) };
                            }
                        }
                    }

                    Connect.Close();
                }

                return Model;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool SetModelOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_Model\" = '{Order.Model.ID}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetModelAllOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_Model\" = '{Order.Model.ID}' WHERE \"Number\" = '{Order.Number}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedNeedRemoveModel(Model model)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"ID_Model\" = '{model.ID}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteModel(Model model)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Model\"" +
                                                           $"WHERE \"ID\" = {model.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetTypeAddOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_TypeAdd\" = '{Order.TypeAdd.ID}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public String GetExecutor(String Number, String List)
        {
            try
            {
                String Executor = "";

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"Executor\" FROM public.\"Orders\" WHERE \"List\" = '{List}' AND \"Number\"='{Number}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Executor = Reader.GetString(0);
                            }
                        }
                    }

                    Connect.Close();
                }

                return Executor;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }

        public bool GetAllPathDetails()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\",\"PathDWG\",\"PathPDF\",\"PathDXF\"" +
                                        $" FROM public.\"PathDetails\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.PathDetails.Add(new PathDetails { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), PathDWG = Reader.GetString(2), PathPDF = Reader.GetString(3), PathDXF = Reader.GetString(4) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool InsertPathDetails(PathDetails PathDetails)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"PathDetails\"(\"DateCreate\", \"PathDWG\", \"PathPDF\", \"PathDXF\") VALUES('{PathDetails.DateCreate}', '{PathDetails.PathDWG}', '{PathDetails.PathPDF}', '{PathDetails.PathDXF}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool PathDetailsExist(PathDetails PathDetails)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"PathDWG\", \"PathPDF\", \"PathDXF\" FROM public.\"PathDetails\" WHERE \"PathDWG\"='{PathDetails.PathDWG}' AND \"PathPDF\"='{PathDetails.PathPDF}' AND \"PathDXF\"='{PathDetails.PathDXF}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public PathDetails GetPathDetails(PathDetails PathDetails)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"PathDWG\", \"PathPDF\", \"PathDXF\" FROM public.\"PathDetails\" WHERE \"PathDWG\"='{PathDetails.PathDWG}' AND \"PathPDF\"='{PathDetails.PathPDF}' AND \"PathDXF\"='{PathDetails.PathDXF}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                PathDetails = new PathDetails { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), PathDWG = Reader.GetString(2), PathPDF = Reader.GetString(3), PathDXF = Reader.GetString(4) };
                            }
                        }
                    }

                    Connect.Close();
                }

                return PathDetails;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool SetPathDetailsOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_PathDetails\" = '{Order.PathDetails.ID}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetPathDetailsAllOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_PathDetails\" = '{Order.PathDetails.ID}' WHERE \"Number\" = '{Order.Number}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedNeedRemovePathDetails(PathDetails pathDetails)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"ID_PathDetails\" = '{pathDetails.ID}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DeletePathDetails(PathDetails pathDetails)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"PathDetails\"" +
                                                           $"WHERE \"ID\" = {pathDetails.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetAllPathArhive()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\",\"Path\"" +
                                        $" FROM public.\"PathArhive\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.PathArhives.Add(new PathArhive { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), Path = Reader.GetString(2) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool InsertPathArhive(PathArhive PathArhive)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"PathArhive\"(\"DateCreate\", \"Path\") VALUES('{PathArhive.DateCreate}', '{PathArhive.Path}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool PathArhiveExist(PathArhive PathArhive)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Path\" FROM public.\"PathArhive\" WHERE \"Path\"='{PathArhive.Path}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public PathArhive GetPathArhive(PathArhive PathArhive)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"Path\" FROM public.\"PathArhive\" WHERE \"Path\"='{PathArhive.Path}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                PathArhive = new PathArhive { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), Path = Reader.GetString(2) };
                            }
                        }
                    }

                    Connect.Close();
                }

                return PathArhive;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool SetPathArhiveOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_PathArhive\" = '{Order.PathArhive.ID}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetPathArhiveAllOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_PathArhive\" = '{Order.PathArhive.ID}' WHERE \"Number\" = '{Order.Number}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedNeedRemovePathArhive(PathArhive PathArhive)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"ID_PathArhive\" = '{PathArhive.ID}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DeletePathArhive(PathArhive PathArhive)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"PathArhive\"" +
                                                           $"WHERE \"ID\" = {PathArhive.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetAllRevision()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"CreatedBy\", \"Information\", \"Description\", \"LastApptovedBy\"" +
                                        $" FROM public.\"Revision\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                SystemArgs.Revisions.Add(new Revision { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), CreatedBy = Reader.GetString(2), Information = Reader.GetString(3), Description = Reader.GetString(4), LastApptovedBy = Reader.GetString(5) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool InsertRevision(Revision Revision)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"Revision\"(\"DateCreate\", \"CreatedBy\", \"Information\", \"Description\", \"LastApptovedBy\") VALUES('{Revision.DateCreate}', '{Revision.CreatedBy}', '{Revision.Information}', '{Revision.Description}', '{Revision.LastApptovedBy}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool RevisionExist(Revision Revision)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"CreatedBy\", \"Information\", \"Description\", \"LastApptovedBy\" FROM public.\"Revision\" WHERE \"DateCreate\"='{Revision.DateCreate}' AND \"CreatedBy\"='{Revision.CreatedBy}' AND \"Information\"='{Revision.Information}' AND \"Description\"='{Revision.Description}' AND \"LastApptovedBy\"='{Revision.LastApptovedBy}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Revision GetRevision(Revision Revision)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"CreatedBy\", \"Information\", \"Description\", \"LastApptovedBy\" FROM public.\"Revision\" WHERE \"DateCreate\"='{Revision.DateCreate}' AND \"CreatedBy\"='{Revision.CreatedBy}' AND \"Information\"='{Revision.Information}' AND \"Description\"='{Revision.Description}' AND \"LastApptovedBy\"='{Revision.LastApptovedBy}';;", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                Revision = new Revision { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), CreatedBy = Reader.GetString(2), Information = Reader.GetString(3), Description = Reader.GetString(4), LastApptovedBy = Reader.GetString(5) };
                            }
                        }
                    }

                    Connect.Close();
                }

                return Revision;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool SetRevisionOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_Revision\" = '{Order.Revision.ID}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedNeedRemoveRevision(Revision Revision)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"ID_Revision\" = '{Revision.ID}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteRevision(Revision Revision)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Revision\"" +
                                                           $"WHERE \"ID\" = {Revision.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GetAllComment()
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"ID_User\", \"Text\"" +
                                        $" FROM public.\"Comment\";", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                User TempUser = SystemArgs.Users.FindAll(p => p.ID == Reader.GetInt64(2)).FirstOrDefault();

                                SystemArgs.Comments.Add(new Comment { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), User = TempUser, Text = Reader.GetString(3) });
                            }
                        }
                    }

                    Connect.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool InsertComment(Comment Comment)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"INSERT INTO public.\"Comment\"(\"DateCreate\", \"ID_User\", \"Text\") VALUES('{Comment.DateCreate}', '{Comment.User.ID}', '{Comment.Text}'); ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CommentExist(Comment Comment)
        {
            Boolean flag = false;
            try
            {

                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"ID_User\", \"Text\" FROM public.\"Comment\" WHERE \"DateCreate\"='{Comment.DateCreate}' AND \"ID_User\"='{Comment.User.ID}' AND \"Text\"='{Comment.Text}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                flag = true;
                            }
                        }
                    }

                    Connect.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Comment GetComment(Comment Comment)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT \"ID\", \"DateCreate\", \"ID_User\", \"Text\" FROM public.\"Comment\" WHERE \"DateCreate\"='{Comment.DateCreate}' AND \"ID_User\"='{Comment.User.ID}' AND \"Text\"='{Comment.Text}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                User TempUser = SystemArgs.Users.FindAll(p => p.ID == Reader.GetInt64(2)).FirstOrDefault();

                                Comment = new Comment { ID = Reader.GetInt64(0), DateCreate = Reader.GetDateTime(1), User = TempUser, Text = Reader.GetString(3) };
                            }
                        }
                    }

                    Connect.Close();
                }

                return Comment;
            }
            catch (Exception E)
            {
                throw new Exception(E.ToString());
            }
        }
        public bool SetCommentOrder(Order Order)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"UPDATE public.\"Orders\" SET \"ID_Comment\" = '{Order.Comment.ID}' WHERE \"ID\" = '{Order.ID}'; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckedNeedRemoveComment(Comment Comment)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"SELECT COUNT(\"ID\") FROM public.\"Orders\" WHERE \"ID_Comment\" = '{Comment.ID}';", Connect))
                    {
                        using (var Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                if (Reader.GetInt64(0) == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    Connect.Close();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteComment(Comment Comment)
        {
            try
            {
                using (var Connect = new NpgsqlConnection(_ConnectString))
                {
                    Connect.Open();

                    using (var Command = new NpgsqlCommand($"DELETE FROM public.\"Comment\"" +
                                                           $"WHERE \"ID\" = {Comment.ID}; ", Connect))
                    {
                        Command.ExecuteNonQuery();
                    }

                    Connect.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
