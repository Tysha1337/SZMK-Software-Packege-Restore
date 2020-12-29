using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SZMK.LauncherUpdater
{
    public class BaseProgram
    {
        private readonly static string Path = Directory.GetCurrentDirectory() + @"\Settings\connect.conf";

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        public static extern UInt32 RmStartSession(out UInt32 pSessionHandle, UInt32 dwSessionFlags, string strSessionKey);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        public static extern UInt32 RmRegisterResources(UInt32 dwSessionHandle,
        UInt32 nFiles, string[] rgsFilenames, UInt32 nApplications,
        UInt32 rgApplications, UInt32 nServices, UInt32 rgsServiceNames);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        public static extern UInt32 RmGetList(UInt32 dwSessionHandle, out UInt32 pnProcInfoNeeded,
        ref UInt32 pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref UInt32 lpdwRebootReasons);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        public static extern UInt32 RmEndSession(UInt32 dwSessionHandle);

        const UInt32 RmRebootReasonNone = 0x0;

        public static string Port;
        public static string Server;

        protected static bool GetParametersConnect()
        {
            try
            {
                if (!File.Exists(Path))
                {
                    throw new Exception();
                }

                using (StreamReader sr = new StreamReader(File.Open(Path, FileMode.Open)))
                {
                    Server = sr.ReadLine();
                    Port = sr.ReadLine();
                }

                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message, Ex);
            }
        }

        protected static bool CheckConnect()
        {
            try
            {
                TcpClient tcpClient = new TcpClient();

                var result = tcpClient.BeginConnect(Server, Convert.ToInt32(Port), null, null);

                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                if (!success)
                {
                    throw new Exception();
                }

                NetworkStream outputStream = tcpClient.GetStream();
                BinaryWriter writer = new BinaryWriter(outputStream);
                writer.Write(true);
                tcpClient.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
        protected static List<Process> GetLockProcesses(string path)
        {
            uint handle;
            string key = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();
            uint res = RmStartSession(out handle, (uint)0, key);
            if (res != 0)
            {
                throw new Exception("Could not begin restart session. " +
                                    "Unable to determine file locker.");
            }
            try
            {
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfoNeeded = 0, pnProcInfo = 0, lpdwRebootReasons = RmRebootReasonNone;
                string[] resources = new string[] { path };
                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, 0, 0, 0);
                if (res != 0)
                {
                    throw new Exception("Could not register resource.");
                }

                res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null,
                                ref lpdwRebootReasons);
                if (res == ERROR_MORE_DATA)
                {
                    // Create an array to store the process results. 
                    RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;
                    // Get the list. 
                    res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                    if (res == 0)
                    {
                        processes = new List<Process>((int)pnProcInfo);
                        // Enumerate all of the results and add them to the  
                        // list to be returned. 
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            try
                            {
                                processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                            }
                            // Catch the error in case the process is no longer running. 
                            catch (ArgumentException) { }
                        }
                    }
                    else
                    {
                        throw new Exception("Could not list processes locking resource");
                    }
                }
                else if (res != 0)
                {
                    throw new Exception("Could not list processes locking resource." +
                                        "Failed to get size of result.");
                }
            }
            finally
            {
                RmEndSession(handle);
            }
            return processes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RM_UNIQUE_PROCESS
        {
            // The product identifier (PID). 
            public int dwProcessId;
            // The creation time of the process. 
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }
        /// <summary> 
        /// Describes an application that is to be registered with the Restart Manager. 
        /// </summary> 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RM_PROCESS_INFO
        {
            const int CCH_RM_MAX_APP_NAME = 255;
            const int CCH_RM_MAX_SVC_NAME = 63;

            // Contains an RM_UNIQUE_PROCESS structure that uniquely identifies the 
            // application by its PID and the time the process began. 
            public RM_UNIQUE_PROCESS Process;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            // If the process is a service, this parameter returns the  
            // long name for the service. 
            public string strAppName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            // If the process is a service, this is the short name for the service. 
            public string strServiceShortName;
            // Contains an RM_APP_TYPE enumeration value. 
            public RM_APP_TYPE ApplicationType;
            // Contains a bit mask that describes the current status of the application. 
            public uint AppStatus;
            // Contains the Terminal Services session ID of the process. 
            public uint TSSessionId;
            // TRUE if the application can be restarted by the  
            // Restart Manager; otherwise, FALSE. 
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }
        /// <summary> 
        /// Specifies the type of application that is described by 
        /// the RM_PROCESS_INFO structure. 
        /// </summary> 
        public enum RM_APP_TYPE
        {
            // The application cannot be classified as any other type. 
            RmUnknownApp = 0,
            // A Windows application run as a stand-alone process that 
            // displays a top-level window. 
            RmMainWindow = 1,
            // A Windows application that does not run as a stand-alone 
            // process and does not display a top-level window. 
            RmOtherWindow = 2,
            // The application is a Windows service. 
            RmService = 3,
            // The application is Windows Explorer. 
            RmExplorer = 4,
            // The application is a stand-alone console application. 
            RmConsole = 5,
            // A system restart is required to complete the installation because 
            // a process cannot be shut down. 
            RmCritical = 1000
        }
    }
}
