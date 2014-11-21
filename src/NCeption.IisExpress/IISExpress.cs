using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NCeption.Web
{
    class IISExpress : IDisposable
    {
        private static class NativeMethods
        {
            // Methods
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr GetTopWindow(IntPtr hWnd);
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        }

        private static void SendStopMessageToProcess(int pid)
        {
            try
            {
                for (IntPtr ptr = NativeMethods.GetTopWindow(IntPtr.Zero); ptr != IntPtr.Zero; ptr = NativeMethods.GetWindow(ptr, 2))
                {
                    uint num;
                    
                    NativeMethods.GetWindowThreadProcessId(ptr, out num);
                    
                    if (pid != num) continue;

                    var hWnd = new HandleRef(null, ptr);
                    
                    NativeMethods.PostMessage(hWnd, 0x12, IntPtr.Zero, IntPtr.Zero);
                    
                    return;
                }
            }
            catch (ArgumentException)
            {
            }
        }

        const string PATH = "path";
        const string PORTNUMBER = "port";

        readonly Process process;

        IISExpress(string path, int portNumber)
        {
            var arguments = new StringBuilder();
            arguments.AppendFormat("/{0}:{1} ", PATH, path);

            arguments.AppendFormat("/{0}:{1} ", PORTNUMBER, portNumber);
            
            process = Process.Start(new ProcessStartInfo
            {
                FileName = GetDefaultIisExpressPath(),
                Arguments = arguments.ToString(),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }

        public static IISExpress Start(string projectRoot, int portNumber)
        {
            return new IISExpress(projectRoot, portNumber);
        }

        private void Stop()
        {
            SendStopMessageToProcess(process.Id);
            process.Close();
            process.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }

        private static string GetDefaultIisExpressPath()
        {
            const string iisExpressX86Path = @"c:\program files (x86)\IIS Express\IISExpress.exe";
            const string iisExpressX64Path = @"c:\program files\IIS Express\IISExpress.exe";

            return File.Exists(iisExpressX86Path) ? iisExpressX86Path : iisExpressX64Path;
        }
    }
}