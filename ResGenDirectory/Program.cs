using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ResGenDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = @"C:\dd";
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException("Can't find source directory");
            }


            var destinationDirectory = @"C:\dd";

            //if (!String.IsNullOrEmpty(args[1]))
            //{
            //    if (Directory.Exists(args[1]))
            //    {
            //        destinationDirectory = args[1];
            //    }
            //    else
            //    {
            //        throw new DirectoryNotFoundException("Can't find destination directory.");
            //    }
            //}

            var files = Directory.GetFiles(directory, "*.resx", SearchOption.AllDirectories);
            string cd = @"cd C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools";
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;

            //输出信息
            p.StartInfo.RedirectStandardOutput = false;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(cd);
            List<string> vs = new List<string>();
            foreach (var file in files)
            {
                try
                {
                    var command = new StringBuilder();
                    command.Append(@"resgen   ");
                    command.Append(Path.GetFullPath(file));
                    command.Append(" ");

                    if (!String.IsNullOrEmpty(destinationDirectory))
                    {
                        var subDirectories = Path.GetFullPath(file).Replace(directory, "");

                        var combinded = destinationDirectory + subDirectories;

                        var combinedDirectory = Path.GetDirectoryName(combinded);

                        if (!Directory.Exists(combinedDirectory))
                        {
                            Console.WriteLine("Creating Directory: {0} ", combinedDirectory);
                            Directory.CreateDirectory(combinedDirectory);
                        }

                        string aa = string.Format("{0}\\{1}{2}", Path.GetDirectoryName(combinded),
                                            "ACalE.Localization.Resources." + Path.GetFileNameWithoutExtension(combinded), ".resources");
                        vs.Add(aa);
                        command.AppendFormat(aa);
                    }
                    else
                    {
                        command.Append(Path.GetFileNameWithoutExtension(file));
                        command.Append(".txt");
                    }

                    p.StandardInput.WriteLine(command);
                    //var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
                    p.StandardInput.WriteLine(" ");
                }
                catch (Exception objException)
                {
                    // Log the exception
                }
            }


            //.\al.exe / t:lib / embed:"F:\aa\ACalE.Localization.Resources.Common.zh-Hant.resources" / embed:"F:\aa\ACalE.Localization.Resources.SysSettings.zh-Hant.resources" / culture:zh - Hant /out:"F:\aa\ACalE.Localization.resources.dll"
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"al.exe /t:lib ");
                //链接为dll
                foreach (var item in vs)
                {
                    stringBuilder.Append($" /embed:\"{item}\" ");
                }
                stringBuilder.Append($" /culture:zh-Hant  /out:\"{destinationDirectory}/ACalE.Localization.resources.dll\"");
                p.StandardInput.WriteLine(stringBuilder);
            }
            catch (Exception ex)
            {

            }



            //退出cmd.exe
            p.StandardInput.WriteLine("exit");

            p.StandardInput.WriteLine("exit");
            //p.StandardOutput.ReadToEnd();
            p.Close();
        }
    }
}
