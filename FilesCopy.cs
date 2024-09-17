using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QROperationsLoader
{
    internal class FilesCopy
    {


        public void FileCopyToBackup(string filetocopy)
        {

            string currdir = Directory.GetCurrentDirectory();
            //поиск директории и ее создание если не найдена
            string target = currdir + @"\Backup";
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }


            //копируем файл в папку бэкапов
            //сначала попытаемся найти файл с таким именем, чтобы очередная копия была с новым именем
            string filename = Path.GetFileName(filetocopy);

            if (!File.Exists(target + @"\" + filename))
            {
                //если не найден файл, то копируем

                string sourceDir = Settings.Default.scandir;
                string backupDir = target;

                File.Copy(
                    Path.Combine(sourceDir, filename),
                    Path.Combine(backupDir, filename)
                );
            }
            else
            {
                //если  файл найден, копируем с другим именем
                string ext = Path.GetExtension(filename); // returns .exe
                string fname = Path.GetFileNameWithoutExtension(filename); // returns File
                string current_date = DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
                string filenameto = fname + "_new" + current_date + ext;

                string sourceDir = Settings.Default.scandir;
                string backupDir = target;

                File.Copy(
                    Path.Combine(sourceDir, filename),
                    Path.Combine(backupDir, filenameto)
                );
            }




        }



        public void FileCopyToNone(string filetocopy)
        {

            string currdir = Directory.GetCurrentDirectory();
            //поиск директории и ее создание если не найдена
            string target = currdir + @"\None";
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            string filename = Path.GetFileName(filetocopy);

            if (!File.Exists(target + @"\" + filename))
            {
                //если не найден файл, то копируем

                string sourceDir = Settings.Default.scandir;
                string backupDir = target;

                File.Copy(
                    Path.Combine(sourceDir, filename),
                    Path.Combine(backupDir, filename)
                );
            }

        }




        public void FileCopyToBad(string filetocopy)
        {

            string currdir = Directory.GetCurrentDirectory();
            //поиск директории и ее создание если не найдена
            string target = currdir + @"\Bad";
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            string filename = Path.GetFileName(filetocopy);

            if (!File.Exists(target + @"\" + filename))
            {
                //если не найден файл, то копируем

                string sourceDir = Settings.Default.scandir;
                string backupDir = target;

                File.Copy(
                    Path.Combine(sourceDir, filename),
                    Path.Combine(backupDir, filename)
                );
            }

        }



    }
}
