using System.IO;

namespace PanDownloadOpen
{
    public class FileOpen
    {
        //static string pandownload_com = "64.52.84.68 pandownload.com"
        static string pandownload_com = "127.0.0.1 pandownload.com";

        /// <summary>
        /// 检测 Host
        /// </summary>
        public static bool HostTesting()
        {
            string host = GetHost();
            if (host.Contains(pandownload_com))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更改 Host
        /// </summary>
        public static void HostChange()
        {
            string host = GetHost();
            if (!host.Contains(pandownload_com))
            {
                host += "\r\n" + pandownload_com;
            }
            SetHost(host);
        }

        /// <summary>
        /// 还原 Host
        /// </summary>
        public static void HostReduction()
        {
            string host = GetHost();
            host = host.Replace("\r\n" + pandownload_com, "");
            SetHost(host);
        }

        /// <summary>
        /// 获取 Host
        /// </summary>
        public static string GetHost()
        {
            string path = @"C:\Windows\System32\drivers\etc\hosts";//文件路径
            return File.ReadAllText(path);
        }

        /// <summary>
        /// 设置 Host
        /// </summary>
        public static void SetHost(string contents)
        {
            string path = @"C:\Windows\System32\drivers\etc\hosts";//文件路径
            File.SetAttributes(path, File.GetAttributes(path) & (~FileAttributes.ReadOnly));//取消只读
            File.WriteAllText(path, contents);
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.ReadOnly);//设置只读
        }
    }
}