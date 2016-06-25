using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace _13253039proje
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti = new SqlConnection();
        VeriTabaniIslemleri db;
        public Form1()
        {
            InitializeComponent();
            db = new VeriTabaniIslemleri();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string str = "Data Source=.;Initial Catalog=BankaOtomasyon;Integrated Security=True";
            baglanti.ConnectionString = str;
            try
            {
                baglanti.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("bağlantı hatası");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Select * from MusteriTBL";
                SqlDataReader reader = komut.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {

                    if (textBox2.Text == reader.GetInt32(0).ToString())
                    {
                        tabControl1.SelectedIndex = 1;
                        adtextBox1.Text = reader.GetString(1);
                        soyadtextBox2.Text = reader.GetString(2);
                        adrestextBox3.Text = reader.GetString(3);
                        telefontextBox4.Text = reader.GetString(4);
                        emailtextBox5.Text = reader.GetString(5);
                        count++;
                        break;
                    }

                }
                if (count == 0)
                {
                    DialogResult result = MessageBox.Show("Bu Numaraya Ait Musteri ve Hesap Bulunmamaktadır Eklemek İstermisiniz?", "onay", MessageBoxButtons.YesNo);//Kullanıcıya emin misiniz diye sormak için
                    if (result == DialogResult.Yes)
                    {
                        tabControl1.SelectedIndex = 1;
                    }
                }
                reader.Close();
            }
            catch
            {
                baglanti.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string email = emailtextBox5.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                MessageBox.Show(email + "  Gecerli ");
            else
            {
                MessageBox.Show(email + "  Gecersiz!! Ekleme Yapılamadı!! ");
                emailtextBox5.Text = "";
            }
            if (match.Success)
            {
                int count = 0;
                try
                {
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "Select * from MusteriTBL";
                    SqlDataReader reader = komut.ExecuteReader();
                    while (reader.Read())
                    {
                        if (textBox2.Text == reader.GetInt32(0).ToString())
                        {
                            adtextBox1.Text = reader.GetString(1);
                            soyadtextBox2.Text = reader.GetString(2);
                            adrestextBox3.Text = reader.GetString(3);
                            telefontextBox4.Text = reader.GetString(4);
                            emailtextBox5.Text = reader.GetString(5);
                            count++;
                            DialogResult result = MessageBox.Show("Hesabınız Bulunmaktadır Yinede Hesap Açmak İstiyormusunuz", "HESAP ONAY", MessageBoxButtons.YesNo);//Kullanıcıya emin misiniz diye sormak için
                            if (result == DialogResult.Yes)
                            {
                                if (db.EkleSilGuncelle("Insert into HesaplarTBL values('" + textBox2.Text + "','" + 0 + "','" + 0 + "','" + 400 + "')") > 0)
                                    MessageBox.Show("Hesap Açıldı");
                                break;
                            }
                        }
                    }
                    if (count == 0)
                    {
                        textBox2.Text = "";
                        if (db.EkleSilGuncelle("Insert into MusteriTBL values('" + adtextBox1.Text + "','" + soyadtextBox2.Text + "','"
                       + adrestextBox3.Text + "','" + telefontextBox4.Text + "','" + emailtextBox5.Text + "')") > 0)
                            MessageBox.Show("Müşteri Eklendi");
                        if (db.EkleSilGuncelle("Insert into HesaplarTBL values((select max(musteriNo) from MusteriTBL) ,0,0,400)") > 0)
                            MessageBox.Show("Hesap Açıldı");
                    }
                    reader.Close();
                }
                catch
                {
                    MessageBox.Show("Başka Bir hata Var!");
                    baglanti.Close();
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                string kayit = "update MusteriTBL set ad='" + adtextBox1.Text + "',soyad='" + soyadtextBox2.Text + "',adres='" + adrestextBox3.Text + "',telefon='" + telefontextBox4.Text + "',email='" + emailtextBox5.Text +
                         "' Where musteriNo=" + "(select max(musteriNo) from MusteriTBL)";
                if (db.EkleSilGuncelle(kayit) > 0)
                    MessageBox.Show("Güncelleme İşlemi Başarıyla Gerçekleşti");
            }
            else
            {
                string kayit = "update MusteriTBL set ad='" + adtextBox1.Text + "',soyad='" + soyadtextBox2.Text + "',adres='" + adrestextBox3.Text + "',telefon='" + telefontextBox4.Text + "',email='" + emailtextBox5.Text +
                         "' Where musteriNo=" + Convert.ToInt32(textBox2.Text);
                if (db.EkleSilGuncelle(kayit) > 0)
                    MessageBox.Show("Güncelleme İşlemi Başarıyla Gerçekleşti");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = baglanti;
                command.CommandText = "Select * From HesaplarTBL Where musteriNo  like ('" + Convert.ToInt32(textBox3.Text) + "%')";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception)
            {
                //MessageBox.Show("Veritabanı Bağlantı Problemi!");
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Form2 frm = new Form2();
                frm.textBox6.Text = dataGridView1[0, e.RowIndex].Value.ToString();
                frm.textBox5.Text = dataGridView1[1, e.RowIndex].Value.ToString();
                frm.textBox1.Text = dataGridView1[2, e.RowIndex].Value.ToString();
                frm.textBox2.Text = dataGridView1[3, e.RowIndex].Value.ToString();
                frm.textBox3.Text = dataGridView1[4, e.RowIndex].Value.ToString();
                frm.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string komut="delete from havaleTBL where gonderenHesap='"+textBox4.Text +"'or alanHesap='" + textBox4.Text+"'";
            string komut1="delete from IslemlerTBL where hesapNo='"+textBox4.Text+"'";
            string komut2 = "Delete From HesaplarTBL Where hesapNo ='" + textBox4.Text+"'"  + "and bakiye=" + 0;
            DialogResult result = MessageBox.Show("Silmek istediğinize emin misiniz?", "Silme onayı", MessageBoxButtons.YesNo);//Kullanıcıya emin misiniz diye sormak için
            if (result == DialogResult.Yes)
            {
                if (db.EkleSilGuncelle(komut) > 0)
                { }
                if (db.EkleSilGuncelle(komut1) > 0)
                { }
                if (db.EkleSilGuncelle(komut2) > 0)
                {
                    MessageBox.Show("Silme Başarılı");
                }
                else
                {
                    MessageBox.Show("Hesabınız Olmayabilir veya Bakiyeniz 0 Değil");
                }
            }
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            listBox1.Text = "";
            listBox2.Text = "";
            listBox3.Text = "";
            decimal toplam = 0;
            decimal toplam1 = 0;
            decimal toplam2 = 0;
            try
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Select * from IslemlerTBL";
                SqlDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.ToDateTime(dateTimePicker1.Text) == reader.GetDateTime(4) && reader.GetInt32(2) == 1)
                    {
                        decimal sayi = reader.GetDecimal(3);
                        toplam = toplam + sayi;
                    }
                    if (Convert.ToDateTime(dateTimePicker1.Text) == reader.GetDateTime(4) && reader.GetInt32(2) == 2)
                    {
                        decimal sayi = reader.GetDecimal(3);
                        toplam1 = toplam1 + sayi;
                    }  
                }
                listBox1.Items.Add(toplam.ToString());
                listBox2.Items.Add(toplam1.ToString());
                
                reader.Close();
                SqlCommand komut2 = new SqlCommand();
                komut2.Connection = baglanti;
                komut2.CommandText = "Select * from HesaplarTBL";
                SqlDataReader readerr = komut2.ExecuteReader();
                while (readerr.Read())
                {
                    decimal sayi = readerr.GetDecimal(4);
                    toplam2 = toplam2 + sayi;
                }
                readerr.Close();
                listBox3.Items.Add(toplam2.ToString());  
            }
            catch
            {
                MessageBox.Show("bağlantı hatası");
                baglanti.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (db.EkleSilGuncelle("Insert into KullaniciTBL values('" + kullaniciAdtextBox1.Text + "','" + kullaniciSifretextBox5.Text + "')") > 0)
                    MessageBox.Show("Kullanici Başarıyla Eklendi");
            }
            catch
            {
                MessageBox.Show("Aynı kullanıcı adıyla islem yapılamaz");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

            string komut = "Delete From KullaniciTBL Where kullaniciAdi ='" + silinecekKullaniciAdtextBox1.Text + "'";
            DialogResult result = MessageBox.Show("Silmek istediğinize emin misiniz?", "Silme onayı", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (db.EkleSilGuncelle(komut) > 0)
                    MessageBox.Show("Silme Başarılı");
                else
                    MessageBox.Show("bu kullanıcı bulunmamakta");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Select * from KullaniciTBL";
                SqlDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    if (kullaniciAdGüncellemetextBox1.Text == reader.GetString(0) && kullaniciSifeGüncellemetextBox5.Text==reader.GetString(1))
                    {
                        tabControl1.SelectedIndex = 10;
                        count ++;
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("ŞİFRENİZ İLE KULLANICI ADINIZ UYUŞMUYOR");
                }
                reader.Close();
            }
            catch
            {
                MessageBox.Show("Hata");
                baglanti.Close();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string kayit = "update KullaniciTBL set sifre='" + textBox5.Text+"', kullaniciAdi='"+textBox1.Text+ "' Where kullaniciAdi='" + kullaniciAdGüncellemetextBox1.Text+"'";
            if (db.EkleSilGuncelle(kayit) > 0)
                MessageBox.Show("Güncelleme İşlemi Başarılı");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            tabControl2.SelectedIndex = 0;
            textBox6.Text = "";
            textBox7.Text = "";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 2;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Select * from KullaniciTBL";
                SqlDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    if (textBox6.Text == reader.GetString(0) && textBox7.Text== reader.GetString(1))
                    {
                        tabControl1.SelectedIndex = 11;
                        count++;
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("ŞİFRENİZ İLE KULLANICI ADINIZ UYUŞMUYOR");
                }
                reader.Close();
            }
            catch
            {
                MessageBox.Show("Hata");
                baglanti.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex=4;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex=8;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 9;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex=7;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button16_Click(object sender, EventArgs e)
        {listBox1.Items.Clear();
        listBox2.Items.Clear();
        listBox3.Items.Clear();
            tabControl1.SelectedIndex = 11;
            tabControl2.SelectedIndex = 0;
            kullaniciAdtextBox1.Text = "";
            kullaniciSifretextBox5.Text = "";
            silinecekKullaniciAdtextBox1.Text = "";
            textBox1.Text = "";
            textBox5.Text = "";
            kullaniciAdGüncellemetextBox1.Text = "";
            kullaniciSifeGüncellemetextBox5.Text = "";
            textBox4.Text = "";
            

        }

        private void button25_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("MÜŞTERİ KAYDINIZ VARMIDIR?", "KAYIT ONAYI", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                tabControl1.SelectedIndex = 3;
            }
            else
            {
                tabControl1.SelectedIndex = 1;
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 11;
            tabControl2.SelectedIndex = 0;
            adtextBox1.Text = "";
            soyadtextBox2.Text = "";
            adrestextBox3.Text = "";
            telefontextBox4.Text = "";
            emailtextBox5.Text = "";
            textBox2.Text="";
        }

       

        private void emailtextBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}