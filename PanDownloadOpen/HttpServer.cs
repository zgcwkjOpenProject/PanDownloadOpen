using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PanDownloadOpen
{
    public class HttpServer
    {
        public static void Open()
        {
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketWatch.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 80));
            socketWatch.Listen(20); // 参数表示最多可容纳的等待接受的传入连接数，不包含已经建立连接的

            Thread thread = new Thread(delegate (object obj)
            {
                Socket socketListen = (Socket)obj;
                while (true)
                {
                    using (Socket socket = socketListen.Accept())
                    {
                        byte[] data = new byte[1024 * 1024 * 4]; // 浏览器发来的数据
                        int length = socket.Receive(data, 0, data.Length, SocketFlags.None);
                        if (length > 0)
                        {
                            string requestText = Encoding.UTF8.GetString(data, 0, length);
                            byte[] body = GetBody(requestText);
                            byte[] head = GetHead(body);
                            try
                            {
                                socket.Send(head);
                                socket.Send(body);
                            }
                            catch { }
                        }

                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                }
            });

            thread.IsBackground = true;
            thread.Start(socketWatch);
        }

        public static byte[] GetHead(byte[] body, string contentType = "text/html")
        {
            string headStr = @"HTTP/1.1 200 OK
Content-Length: " + body.Length + @"
Content-Type: " + contentType + @"; charset=utf-8
Date: " + string.Format("{0:R}", DateTime.Now) + @"
Server: Web Server

";
            return Encoding.UTF8.GetBytes(headStr);
        }

        public static byte[] GetBody(string data)
        {
            string bodyStr;//存储准备返回的文本
            Console.ForegroundColor = ConsoleColor.Yellow;//更正控制台输出的字体颜色
            if (data.Contains("/api/init?clienttype"))//http://pandownload.com/api/init?clienttype=0&referral=&t=000&version=2.2.2
            {
                Console.WriteLine("抓取到 /api/init?clienttype 的请求，已处理！");
                bodyStr = "{\"srecord\":{\"autoQuery\":true},\"loginurl\":{\"url\":\"http:\\/\\/pandownload.com\\/bdlogin.html\"},\"wke\":\"http:\\/\\/dl.pandownload.club\\/dl\\/node-190312.dll\",\"pcscfg\":{\"appid\":250528,\"ua\":\"\",\"ct\":0},\"flag\":1,\"ad\":{\"url\":\"https:\\/\\/pandownload.com\\/donate.html\",\"image\":\"http:\\/\\/pandownload.com\\/images\\/donate.png\",\"attribute\":\"width=\\\"88\\\" height=\\\"100\\\" padding=\\\"0,0,5,0\\\"\",\"rand\":100},\"bdc\":[\"lovely\"],\"timestamp\":000,\"code\":0,\"message\":\"success\"}";
            }
            else if (data.Contains("/api/script/list?clienttype"))//http://pandownload.com/api/script/list?clienttype=0&referral=&t=000&version=2.2.2
            {
                Console.WriteLine("抓取到 /api/script/list?clienttype 的请求，已处理！");
                bodyStr = "{\"scripts\":[{\"name\":\"search_pandown.lua\",\"remove\":true},{\"name\":\"search_ncckl.lua\",\"remove\":true},{\"name\":\"search_quzhuanpan.lua\",\"remove\":true},{\"name\":\"anime_01.lua\",\"remove\":true},{\"name\":\"anime_02.lua\",\"remove\":true},{\"name\":\"anime_dilidili.lua\",\"remove\":true},{\"name\":\"anime\",\"remove\":true},{\"name\":\"s\",\"id\":2,\"url\":\"http:\\/\\/pandownload.com\\/static\\/scripts\\/s008\",\"md5\":\"8dfd9a6c08d06bec27ae358f315cca8f\"},{\"name\":\"download_pcs.lua\",\"id\":1000,\"url\":\"http:\\/\\/pandownload.com\\/static\\/scripts\\/download_pcs.lua\",\"md5\":\"38770cd3e9bcd62f7212941b51ca1378\"},{\"name\":\"default\",\"id\":0,\"url\":\"http:\\/\\/pandownload.com\\/static\\/scripts\\/default_0.6.7_3fee3733\",\"md5\":\"a1124f076924209d0322078000cdc882\",\"key\":\"568729a30cee34aec0e6fc7a6e303272\"}],\"code\":0,\"message\":\"success\"}";
            }
            else if (data.Contains("/api/latest?clienttype"))//http://pandownload.com/api/latest?clienttype=0&referral=&t=000&version=2.2.2
            {
                Console.WriteLine("抓取到 /api/script/list?clienttype 的请求，已处理！");
                bodyStr = "{\"version\":\"2.3.3\",\"url\":\"https:\\/\\/dl1.cnponer.com\\/files\\/PanDownload_v2.2.2.zip\",\"web\":\"https:\\/\\/www.lanzous.com\\/i8ua9na\",\"detail\":\"\\u66f4\\u65b0\\u65f6\\u95f4: 2020-04-15\\n\\u66f4\\u65b0\\u5185\\u5bb9:\\n1. \\u89e3\\u5f00\\u0020\\u0050\\u0061\\u006e\\u0044\\u006f\\u0077\\u006e\\u006c\\u006f\\u0061\\u0064\\u0020\\u5199\\u7684\\u5de5\\u5177\\uff01\\n2. \\u5f00\\u6e90\\u5730\\u5740\\uff1ahttps://github.com/zgcwkj/PanDownloadOpen\",\"md5\":\"null\",\"code\":0,\"message\":\"success\"}";
            }
            else if (data.Contains("/bdlogin.html"))//http://pandownload.com/bdlogin.html
            {
                Console.WriteLine("抓取到 /bdlogin.html 的请求，已处理！");
                return new OutputResourceFile("PanDownloadOpen.server.bdlogin.html").GetFile();
            }
            else if (data.Contains("/api/latest-old"))//http://pandownload.com/api/latest-old
            {
                Console.WriteLine("抓取到 /api/latest-old 的请求，已处理！");
                return new OutputResourceFile("PanDownloadOpen.server.api.latest-old").GetFile();
            }
            else if (data.Contains("/static/scripts/default_0.6.7_3fee3733"))//http://pandownload.com/static/scripts/default_0.6.7_3fee3733
            {
                Console.WriteLine("抓取到 /static/scripts/default_0.6.7_3fee3733 的请求，已处理！");
                return new OutputResourceFile("PanDownloadOpen.server.static.scripts.default_0.6.7_3fee3733").GetFile();
            }
            else if (data.Contains("/static/scripts/download_pcs.lua"))//http://pandownload.com/static/scripts/download_pcs.lua
            {
                Console.WriteLine("抓取到 /static/scripts/download_pcs.lua 的请求，已处理！");
                return new OutputResourceFile("PanDownloadOpen.server.static.scripts.download_pcs.lua").GetFile();
            }
            else if (data.Contains("/static/scripts/s008"))//http://pandownload.com/static/scripts/s008
            {
                Console.WriteLine("抓取到 /static/scripts/s008 的请求，已处理！");
                return new OutputResourceFile("PanDownloadOpen.server.static.scripts.s008").GetFile();
            }
            else
            {
                Console.WriteLine("抓取到的未知请求 > " + data);
                bodyStr = "请还原 Host ！By：<a href='http://blog.zgcwkj.cn'>zgcwkj</a>";
            }
            return Encoding.UTF8.GetBytes(bodyStr);
        }
    }
}