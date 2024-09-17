using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QROperationsLoader
{
    internal class OracleDB
    {

        //создание подключения
        public static OracleConnection GetConnectionOracle()
        {


            string host = Settings.Default.host;
            int port = int.Parse(Settings.Default.port);
            string sid = Settings.Default.sid;
            string user = Settings.Default.user;
            string password = Settings.Default.password;


            //string connString = "user id=acquiring;password=1;data source=//192.168.1.41:1521/magicash";
            string connString = "user id="+user+";password="+ password +";data source=//"+ host + ":"+port+"/"+sid+"";

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = connString;

            return conn;

        }




    }
}
