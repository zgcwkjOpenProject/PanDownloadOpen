using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PanDownloadOpen
{
    class Program
    {
        public static bool Status = false;//存储状态

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("作者：zgcwkj 20200417");
            Console.WriteLine("功能：跳过 PanDownload 的服务验证，实现打开程序");
            Console.WriteLine("参考：https://github.com/TkzcM/pandownload-fake-server");
            Console.WriteLine("开源：https://github.com/zgcwkj/PanDownloadOpen");
            //==> 代码实现
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(">> 程序自检 Host 文件是否有残留");
            if (HostsFile.HostTesting())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("发现上次的 Host 文件没有清理干净！");
                HostsFile.HostReduction();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("现在已经为你清理了！");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("继续使用，请输入任意字符，空字符直接退出（按回车）");
                string str = Console.ReadLine();
                if (str == "") { Environment.Exit(0); }
            }
            string path = Environment.CurrentDirectory + @"\PanDownload.exe";
            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("请将本程序放在和 PanDownload.exe 同级的目录下运行");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(">> 按回车退出程序 (^_^)///");
                Console.ReadLine();
            }
            else
            {
                #region 验证Host
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("自动修改 Host 文件");
                HostsFile.HostChange();
                IPHostEntry host = Dns.GetHostEntry("pandownload.com");
                if (host.AddressList.Count() != 0 && host.AddressList[0].ToString().Contains("127.0.0.1"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("验证 Host 文件通过");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("验证 Host 文件失败");
                    Console.WriteLine("请尝试离线使用");
                    Console.WriteLine("回车直接退出");
                    HostsFile.HostReduction();
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                #endregion

                #region 启动程序
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("正在启动  PanDownload.exe  程序");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;//启动的程序路径
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(startInfo);
                #endregion

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("启动服务监听");
                HttpServer.Open();
                while (true)
                {
                    if (Status)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("自动还原 Host 文件");
                        HostsFile.HostReduction();
                        break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("程序自毁将在 5 秒后执行");
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}