using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using easyFramework.Sys.Data;
using easyFramework.Sys;
using System.Data.SqlClient;

namespace Tests
{
    [TestFixture]
    class Test_DataMethodsClientInfo
    {
        [Test]
        public void TestSet1()
        {
            ClientInfo ci = new ClientInfo("CLIENT1", connstr());
            {
                string sql = "IF NOT EXISTS(SELECT * FROM sysobjects where name ='tt1') CREATE TABLE tt1 (f1 nvarchar(255));";
                SqlConnection conn = new SqlConnection(connstr());
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Recordset rs = DataMethodsClientInfo.gRsGetTable(ci, "tt1", "f1", ""); 
        }

        private string connstr()
        {
            return "data source=127.0.0.1;initial catalog=ef1;pwd=ppp22==;uid=sa;";
        }
    }
}
