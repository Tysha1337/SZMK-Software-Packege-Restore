﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.TeklaInteraction.Shared.Services;
using SZMK.TeklaInteraction.Shared.Services.Interfaces;
using SZMK.TeklaInteraction.Tekla2018i.Common;

namespace SZMK.TeklaInteraction.Tekla2018i
{
    static class Program
    {
        public static readonly ApplicationContext Context = new ApplicationContext();
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ru-RU");
            Application.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
            var controller = new ApplicationController(new LightInjectAdapder())
                        .RegisterView<Views.Auth.Interfaces.IMain, Views.Auth.Main>()
                        .RegisterService<Services.Auth.Interfaces.IOperations, Services.Auth.Operations>()
                        .RegisterService<Services.Server.Interfaces.IServer, Services.Server.Server>()
                        .RegisterService<IHash, Hash>()
                        .RegisterService<IMailLogger, MailLogger>()
                        .RegisterService<IEncrypton, Encryption>()
                        .RegisterService<ILogin, Login>()
                        .RegisterInstance(new ApplicationContext());

            controller.Run<Presenters.Auth.Main>();
        }
    }
}
