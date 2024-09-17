using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QROperationsLoader
{
    internal class FilesScanner
    {
        string scandir = Settings.Default.scandir;

        public List<string> GetFilesToLoad()
        {

            List<string> list_filesinscandir = new List<string>();
            
            

            //поиск директории и ее создание если не найдена
            string target = scandir;
            if (!Directory.Exists(target))
            {
                //
                Console.WriteLine("Каталог сканирования файлов не найден! Каталог загрузки создан. Путь загрузки: "+ target);
                Directory.CreateDirectory(target);
                return null;
            }
            
            DirectoryInfo dir = new DirectoryInfo(scandir);
            foreach (var d in dir.GetFiles("*.xlsx"))
            {
                
                //список нужен, чтобы сравнивать с другими списками при необходимости
                list_filesinscandir.Add((d.Name).ToString());
                
            }

            return list_filesinscandir;

        }
    
    
    
    }
}
