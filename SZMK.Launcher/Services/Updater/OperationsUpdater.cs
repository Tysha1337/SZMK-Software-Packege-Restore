using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SZMK.Launcher.Models;
using SZMK.Launcher.Views;

namespace SZMK.Launcher.Services.Updater
{
    public class OperationsUpdater : BaseService
    {
        private readonly Logger logger;
        private string Version;
        private Main notify;

        public OperationsUpdater(Main notify)
        {
            try
            {
                logger = LogManager.GetCurrentClassLogger();
                this.notify = notify;
                GetVersionProduct();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void GetVersionProduct()
        {
            try
            {
                notify.Notify(0, "Попытка получения версии SZMK.LauncherUpdater");
                notify.SetMaximum(1);

                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(Directory.GetCurrentDirectory() + @"\Updater\SZMK.LauncherUpdater.exe");
                Version = myFileVersionInfo.FileVersion;
                notify.Notify(0, "Версия SZMK.LauncherUpdater успешно получены");
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        public bool CheckedUpdate()
        {
            try
            {
                notify.SetMaximum(100);
                notify.Notify(0, "Попытка подключения к серверу обновления");
                bool NeedUpdate = false;
                if (CheckConnect())
                {
                    notify.Notify(20, "Подключение к серверу обновления успешно");
                    TcpClient tcpClient = new TcpClient(Server, Convert.ToInt32(Port));

                    using (NetworkStream outputStream = tcpClient.GetStream())
                    {
                        using (BinaryWriter writer = new BinaryWriter(outputStream))
                        {
                            using (BinaryReader reader = new BinaryReader(outputStream))
                            {
                                notify.Notify(40, "Отправка параметров");
                                writer.Write(false);
                                writer.Write(false);

                                notify.Notify(60, "Отправка параметров");
                                writer.Write("SZMK.LauncherUpdater");

                                notify.Notify(80, "Отправка параметров");
                                writer.Write(Environment.MachineName);
                                writer.Write(Version);

                                notify.Notify(100, "Получение необходимости обновления");
                                NeedUpdate = reader.ReadBoolean();
                            }
                        }
                    }
                }

                return NeedUpdate;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        public bool Update()
        {
            try
            {
                notify.Notify(0, "Начало получения обновления SZMK.LauncherUpdater");
                DownloadFiles();
                RemoveAndCopeFiles();
                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void DownloadFiles()
        {
            try
            {
                TcpClient tcpClient = new TcpClient(Server, Convert.ToInt32(Port));

                using (NetworkStream Stream = tcpClient.GetStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(Stream))
                    {
                        using (BinaryReader reader = new BinaryReader(Stream))
                        {

                            notify.SetMaximum(1);
                            notify.Notify(0, "Отправка параметров обновления");
                            writer.Write(false);
                            writer.Write(true);

                            writer.Write("SZMK.LauncherUpdater");
                            writer.Write(Environment.MachineName);
                            writer.Write(Version);
                            notify.Notify(1, "Отправка параметров прошла успешна");

                            notify.Notify(0, "Получение файла информации обновления");

                            using (FileStream fileStream = File.Open(Directory.GetCurrentDirectory() + @"\InfoUpdater.conf", FileMode.Create))
                            {
                                long lenght = reader.ReadInt64();

                                long totalBytes = 0;
                                int readBytes = 0;
                                byte[] buffer = new byte[8192];

                                do
                                {
                                    readBytes = Stream.Read(buffer, 0, buffer.Length);
                                    fileStream.Write(buffer, 0, readBytes);
                                    totalBytes += readBytes;
                                } while (tcpClient.Connected && totalBytes < lenght);
                            }
                            notify.Notify(1, "Получение файла информации обновления успешно");

                            writer.Write(true);

                            int CountFiles = reader.ReadInt32();

                            writer.Write(true);

                            notify.SetMaximum(CountFiles);

                            for (int i = 0; i < CountFiles; i++)
                            {
                                string PathFile = reader.ReadString();

                                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\TempUpdater\" + Path.GetDirectoryName(PathFile));

                                long lenght = reader.ReadInt64();

                                using (FileStream fileStream = File.Open(Directory.GetCurrentDirectory() + @"\TempUpdater\" + PathFile, FileMode.Create))
                                {
                                    long totalBytes = 0;
                                    int readBytes = 0;
                                    byte[] buffer = new byte[8192];

                                    do
                                    {
                                        readBytes = Stream.Read(buffer, 0, buffer.Length);
                                        fileStream.Write(buffer, 0, readBytes);
                                        totalBytes += readBytes;
                                    } while (tcpClient.Connected && totalBytes < lenght);
                                }
                                writer.Write(i);
                                notify.Notify(i + 1, $"Скачивание файлов обновления {i + 1} из {CountFiles}");
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void RemoveAndCopeFiles()
        {
            try
            {

                List<FileAndMove> files = GetFiles();

                CheckOldFiles();

                notify.Notify(0, "Начало удаление старых файлов Updater'a");
                notify.SetMaximum(1);
                foreach (var file in files.FindAll(p => p.Move.Contains("Remove")))
                {
                    File.Delete(Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName);
                    if (Directory.GetDirectories(Path.GetDirectoryName(Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName)).Count() == 0 && Directory.GetFiles(Path.GetDirectoryName(Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName)).Length == 0)
                    {
                        Directory.Delete(Path.GetDirectoryName(Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName));
                    }
                }
                notify.Notify(1, "Удаление старых файлов Updater'a успешно");

                notify.Notify(0, "Обновление файлов Updater'a");
                foreach (var file in files.FindAll(p => !p.Move.Contains("Remove")))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName));
                    }

                    File.Copy(Directory.GetCurrentDirectory() + @"\TempUpdater\" + file.FileName, Directory.GetCurrentDirectory() + @"\Updater\" + file.FileName, true);
                }
                notify.Notify(1, "Обновление файлов Updater'a успешно");
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private void CheckOldFiles()
        {
            try
            {
                notify.Notify(0, "Начало проверки старых файлов Updater'a");
                notify.SetMaximum(1);
                while (true)
                {
                    List<FileAndMove> files = GetFiles();

                    string failProcesses = "";

                    for (int i = 0; i < files.Count; i++)
                    {
                        string CurretPath = Directory.GetCurrentDirectory() + @"\Updater\" + files[i].FileName;

                        if (File.Exists(CurretPath))
                        {
                            List<Process> LockProcesses = GetLockProcesses(CurretPath);
                            if (LockProcesses.Count > 0)
                            {
                                failProcesses = "Файл: " + CurretPath + " занят процессом(ами)";
                                for (int j = 0; j < LockProcesses.Count; j++)
                                {
                                    failProcesses += Environment.NewLine + LockProcesses[j].ProcessName;
                                }
                                failProcesses += Environment.NewLine + "Завершите данные процессы и запустите заново программу";

                                break;
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(failProcesses))
                    {
                        if (MessageBox.Show(failProcesses, "Внимание", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) != DialogResult.Retry)
                        {
                            Application.Exit();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                notify.Notify(1, "Проверка старых файлов Updater'a успешно");
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
        private List<FileAndMove> GetFiles()
        {
            try
            {
                XDocument info = XDocument.Load(Directory.GetCurrentDirectory() + @"\InfoUpdater.conf");

                List<FileAndMove> files = new List<FileAndMove>();

                foreach (var file in info.Element("Program").Elements("File"))
                {
                    files.Add(new FileAndMove { FileName = file.Element("FileName").Value, Move = file.Element("Move").Value });
                }

                return files;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }
    }
}
