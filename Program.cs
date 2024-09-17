using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QROperationsLoader
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //===========================================================================================
            //1 - загрузка всех файлов из папки при первом запуске
            //===========================================================================================

            string scandir = Settings.Default.scandir;
            List<string> listfiles = new List<string>();
            LoadFileToDB lfdb = new LoadFileToDB();
            FilesScanner fs = new FilesScanner();
            listfiles = fs.GetFilesToLoad();

            if (listfiles != null)
            {
                if (listfiles.Count > 0)
                {
                                    
                    //перебор файлов и отправка по одному на вставку
                    foreach (string fname in listfiles)
                    {

                        lfdb.InsertFileToDB(scandir + @"\" + fname);
                        //Console.WriteLine(scandir+@"\"+fname);

                    }
                } else {
                    
                        Console.WriteLine("Не найдено файлов для обработки в каталоге загрузки!");
                        //return;
                }
            }
            Console.Read();

        }
    }
}
