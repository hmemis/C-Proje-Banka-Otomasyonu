using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace _13253039proje
{
    class VeriTabaniIslemleri
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=BankaOtomasyon;Integrated Security=True");

        public VeriTabaniIslemleri()
        {
            try
            {
                connection.Open();
            }
            catch
            {
                throw new Exception("Veritabanı bağlantı hatası");
            }
        }


        public int EkleSilGuncelle(string komut)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = komut;
                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw new Exception(komut+" Veritabanına ekleme sırasında bir hata meydana geldi");
            }
           
        }
    }
}
