
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace QROperationsLoader
{
    internal class LoadFileToDB
    {

        public void InsertFileToDB(string filename)
        {

            Console.WriteLine("Загружается файл: " + filename);
            OracleTransaction transaction;
            ////CSVLoader csv = new CSVLoader();

            ExcelLoader exl = new ExcelLoader();
            CreateDataTableInExcel dtexl = new CreateDataTableInExcel();
            string[,] arrData = exl.LoadExcelFile(filename);
           
            DataTable dataTable = dtexl.GetDataTable(arrData);

            if (dataTable == null ) { return; }
            
            //откроем соединение с базой  
            OracleConnection connection = OracleDB.GetConnectionOracle();

                try
                {
                    // Открываем подключение
                    //connection.Open();
                    // если подключение открыто
                    //if (connection.State == ConnectionState.Open)
                    {
                        
                        
                        
                        string tablename = Settings.Default.tablename;

                        int cnt = 0;
                        int itercount = 0;
                        int povtor = 0;
                        
                        
                        List<string> log_file_information = new List<string>(); //лист дополнительных строк в лог файле


                    //перебор строк таблицы
                    transaction = null;
                        foreach (DataRow row in dataTable.Rows)
                        {

                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                Console.WriteLine("Подключение открыто");                                                               
                                
                            }
                            if (transaction == null)
                            {
                                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                            }
                            
                            itercount = itercount + 1;

                            string operation_uid =  (string)row["operation_uid"];
                            string date_of =  (string)row["date_of"];
                            string qr_number =        (string)row["qr_number"];         
                            string sbp_operation =   (string)row["sbp_operation"];
                            string sbp_transaction = (string)row["sbp_transaction"];
                            string operator_name =     (string)row["operator_name"];
                            string operator_inn =     (string)row["operator_inn"];

                            string original_line = operation_uid + " - " + date_of + " - " + qr_number + " - " + sbp_operation + " - " + sbp_transaction + " - " + operator_name + " - " + operator_inn;


                                string SQL = @"insert into " + tablename + " (operation_uid, date_of, qr_number, sbp_operation, sbp_transaction, operator_name, operator_inn) " +
                                             "values ('" + operation_uid + "', to_date('" + date_of + "', 'DD.MM.YYYY HH24:MI:SS'), '" + qr_number + "', '" + sbp_operation + "', '" + sbp_transaction + "', '" + operator_name + "', '" + operator_inn + "')";

                                OracleCommand command = new OracleCommand(SQL, connection);
                                command.Transaction = transaction;

                                try
                                {
                                    int number = command.ExecuteNonQuery();
                                    cnt = cnt + number;

                                    //если и является сотой строкой или и является последней строкой - то коммитим
                                    if ((itercount % 100 == 0) )
                                    {
                                        Console.WriteLine("Закомичено "+ itercount + " строк");
                                        transaction.Commit();
                                        transaction = null;
                                        
                                        //достигнут конец
                                        if (itercount == dataTable.Rows.Count)
                                        {
                                            Logger logger = new Logger();
                                            string current_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                                            string log_string = current_date + " - Файл: '" + filename + "' успешно загружен в базу. Загружено строк: " + cnt.ToString() + ". Пропущено провторов: " + povtor.ToString();
                                            logger.SaveToLogFile(log_string);

                                            //если есть дополнительные строки для записи в лог - запишем их
                                            if (log_file_information.Count > 0)
                                            {
                                                logger.SaveListToFile(log_file_information);
                                            }


                                            //делаем копию файла в бэкап
                                            FilesCopy fc = new FilesCopy();
                                            fc.FileCopyToBackup(filename);
                                        }



                                    }


                                }
                                catch (Exception ex2)
                                {
                                    //Console.WriteLine(ex2.Message);

                                    if (ex2.HResult == -2147467259)
                                    {
                                        povtor = povtor + 1;
                                        log_file_information.Add("Строка с номером: " + itercount.ToString() + " и содержащая: " + original_line.ToString() + "  - не попала в базу из за повтора.");
                                        //Console.WriteLine("Ошибка повторной записи");
                                        continue; //пропускаем итерацию при ошибке повторной записи
                                    }


                                    //Console.WriteLine("Ошибка записи! Будет произведен откат данных");
                                    transaction.Rollback();
                                    //log_file_information.Add("Не удалось сохранить данные! Будет произведен откат");


                                    Logger logger = new Logger();
                                    string current_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                                    string log_string = current_date + " ОШИБКА!!! Файл: " + filename + " произошла ошибка:" + ex2.Message;
                                    logger.SaveToLogFile(log_string);

                                    //если есть дополнительные строки для записи в лог - запишем их
                                    if (log_file_information.Count > 0)
                                    {
                                        logger.SaveListToFile(log_file_information);
                                    }

                                    connection.Close();
                                    Environment.Exit(0);


                                }// блок ошибки
                           

                    } //конец цикла перебора строк таблицы

                    


                       /*
                    //если все записи сдублированы
                    if (itercount > 0 && cnt == 0)
                    {

                        Logger logger = new Logger();
                        string current_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                        string log_string = current_date + " - Файл: '" + filename + "' успешно обработан. Загружено строк: " + cnt.ToString() + ". Пропущено провторов: " + povtor.ToString();
                        logger.SaveToLogFile(log_string);

                        //если есть дополнительные строки для записи в лог - запишем их
                        if (log_file_information.Count > 0)
                        {
                            logger.SaveListToFile(log_file_information);
                        }


                        //делаем копию файла в бэкап
                        FilesCopy fc = new FilesCopy();
                        fc.FileCopyToBackup(filename);

                    }
                    */

                    //если есть повторы b
                    if (itercount == dataTable.Rows.Count)
                    {
                        transaction.Commit();

                        Logger logger = new Logger();
                        string current_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                        string log_string = current_date + " - Файл: '" + filename + "' успешно загружен в базу. Загружено строк: " + cnt.ToString() + ". Пропущено провторов: " + povtor.ToString();
                        logger.SaveToLogFile(log_string);

                        //если есть дополнительные строки для записи в лог - запишем их
                        if (log_file_information.Count > 0)
                        {
                            logger.SaveListToFile(log_file_information);
                        }


                        //делаем копию файла в бэкап
                        FilesCopy fc = new FilesCopy();
                        fc.FileCopyToBackup(filename);

                    }

                        //if (((itercount == lines.Length)) && itercount > 0)
                        
                        /*
                        //достигнут конец
                        if (((itercount == dataTable.Rows.Count)) && itercount > 0)
                        {
                            transaction.Commit();

                            //есть или нет повторов - то файл хороший


                            Logger logger = new Logger();
                            string current_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                            string log_string = current_date + " - Файл: '" + filename + "' успешно загружен в базу. Загружено строк: " + cnt.ToString() + ". Пропущено провторов: " + povtor.ToString();
                            logger.SaveToLogFile(log_string);

                            //если есть дополнительные строки для записи в лог - запишем их
                            if (log_file_information.Count > 0)
                            {
                                logger.SaveListToFile(log_file_information);
                            }


                            //делаем копию файла в бэкап
                            FilesCopy fc = new FilesCopy();
                            fc.FileCopyToBackup(filename);

                            


                        }// если достигнут конец
                        */







                        Console.WriteLine($"Добавлено объектов: {cnt}");
                    }


                }
                catch (IndexOutOfRangeException ex)
                {
                    //неправильный формат файла
                    //Console.WriteLine(ex.Message);
                    //делаем копию файла в бэды
                    FilesCopy fcn = new FilesCopy();
                    fcn.FileCopyToBad(filename);

                    Logger logger = new Logger();
                    string current_date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                    string log_string = current_date + " ОШИБКА!!! Файл: " + filename + " не удалось прочитать нужные колонки из файла! Файл помещен в Bad";
                    logger.SaveToLogFile(log_string);



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // если подключение открыто
                    if (connection.State == ConnectionState.Open)
                    {
                        // закрываем подключение
                        connection.Close();
                        Console.WriteLine("Подключение закрыто...");
                        //Console.Read();
                    }



                }


           



        }

    }
}
