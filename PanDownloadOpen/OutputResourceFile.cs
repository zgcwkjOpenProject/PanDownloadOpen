using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PanDownloadOpen
{
    /// <summary>
    /// 读取嵌入资源并输出到本地
    /// </summary>
    public class OutputResourceFile
    {
        /// <summary>
        /// 存储二进制的数据
        /// </summary>
        private byte[] Byte;

        /// <summary>
        /// 读取内部嵌入资源
        /// </summary>
        /// <param name="name">资源所在的命名空间的名称</param>
        public OutputResourceFile(string name)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(name);
            Byte = new byte[stream.Length];
            stream.Read(Byte, 0, Byte.Length);
            stream.Close();
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        public byte[] GetFile()
        {
            return Byte;
        }
    }
}