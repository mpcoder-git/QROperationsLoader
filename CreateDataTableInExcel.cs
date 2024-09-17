
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QROperationsLoader
{
    internal class CreateDataTableInExcel
    {

        public DataTable GetDataTable(string[,] arrData) {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("operation_uid");
            dataTable.Columns.Add("date_of");
            dataTable.Columns.Add("qr_number");
            dataTable.Columns.Add("sbp_operation");
            dataTable.Columns.Add("sbp_transaction");
            dataTable.Columns.Add("operator_name");
            dataTable.Columns.Add("operator_inn");

            for (int i = 1; i < arrData.GetLength(0); i++)
            {

                //получим дату и время, преобразуем в нормальный вид
                
                string datetstr = arrData[i, 0].ToString();
                DateTime dateTimeT = new DateTime(1899, 12, 30).AddDays(int.Parse(datetstr));

                //DateOnly testDateOnly = DateOnly.FromDateTime(dateTimeT);
                //DateTime dateTime = DateTime.Parse(datetstr);
                // Извлекаем дату из строки
                //DateTime date = dateTimeT.Date;
                //string dateToInsert = date.ToString();




                string timestr = arrData[i, 1].ToString();
                string format = "dd.mm.yyyy hh:mm:ss";
                DateTime dateTime_timestr = DateTime.Parse(timestr);
                //DateTime dateTime_timestr = DateTime.ParseExact(timestr, format, CultureInfo.InvariantCulture);
                // Извлечение времени
                TimeSpan time = dateTime_timestr.TimeOfDay;
                //string timeToInsert = time.ToString();

                DateTime resultDT = dateTimeT + time;
                string datetimetoinsert = resultDT.ToString();


                string operation_uid = arrData[i,7].ToString();
                string date_of = datetimetoinsert;
                string qr_number = arrData[i, 12].ToString();
                string sbp_operation = arrData[i, 14].ToString();
                string sbp_transaction = arrData[i, 15].ToString();
                string operator_name = arrData[i, 2].ToString();
                string operator_inn = arrData[i, 3].ToString();
                



                DataRow newRow = dataTable.NewRow();
                newRow.ItemArray = new object[] { operation_uid, date_of,  qr_number, sbp_operation, sbp_transaction, operator_name, operator_inn };
                dataTable.Rows.Add(newRow);
            }



                return dataTable; 
        }


    }
}
