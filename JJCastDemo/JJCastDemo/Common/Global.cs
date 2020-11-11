﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JJCastDemo.Common
{
    class Global
    {
        public static void WriteLog(string msg)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath + @"\Log\");
            if (!directoryInfo.Exists) directoryInfo.Create();
            try
            {
                int deleteDay = 3;
                string drDate = DateTime.Today.AddDays(-deleteDay).ToString("yyyyMMdd");
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.log"))
                {
                    if (drDate.CompareTo(fileInfo.LastWriteTime.ToString("yyyyMMdd")) > 0)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                        fileInfo.Delete();
                    }
                }
            }
            catch (Exception) { }

            string path = Application.StartupPath + @"\Log\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            FileInfo file = new FileInfo(path);

            if(!file.Exists)
            {
                FileStream fileStream = file.Create();
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                streamWriter.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] 로그생성");

                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
            }

            StreamWriter stream = File.AppendText(path);
            stream.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg);
            stream.Flush();
            stream.Close();
        }
    }
}
