﻿using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.TeklaInteraction.Shared.Models;
using SZMK.TeklaInteraction.Shared.Services;
using SZMK.TeklaInteraction.Tekla2018i.Services.Server.Interfaces;
using SZMK.TeklaInteraction.Tekla2018i.Views.Shared;

namespace SZMK.TeklaInteraction.Tekla2018i.Services.Server
{
    class Server : IServer
    {
        private readonly Logger logger;
        private readonly MailLogger maillogger;

        public Server()
        {
            logger = LogManager.GetCurrentClassLogger();
            maillogger = new MailLogger();
        }

        public void StartServer(User user)
        {
            try
            {
                var server = new NamedPipeServerStream("Tekla2018i");
                StreamReader reader = new StreamReader(server);
                while (true)
                {
                    server.WaitForConnection();

                    logger.Info("Клиент подключился");

                    Loading Load = new Loading();
                    Load.Show();

                    Tekla tekla = new Tekla(Load);
                    if (tekla.CheckConnect())
                    {
                        logger.Info("Модель подключена");

                        tekla.GetData(user);
                    }
                    else
                    {
                        logger.Info("Модель не подключена");

                        MessageBox.Show("Ошибка подключения модели", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    server.Disconnect();
                }
            }
            catch (Exception E)
            {
                maillogger.SendErrorLog(E.ToString());
                logger.Error(E.ToString());
                StartServer(user);
            }
        }
    }
}