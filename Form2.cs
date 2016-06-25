using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace _13253039proje
{
    public partial class Form2 : Form
    {
        SqlConnection baglanti = new SqlConnection();
        VeriTabaniIslemleri db;
        public Form2()
        {
            InitializeComponent();
            db = new VeriTabaniIslemleri();
        }
        private void Form2_Load(object sender, EventArgs e)
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
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = baglanti;
                command.CommandText = "Select * From IslemTuruTBL";
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
        //private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex > -1)
        //    {
        //        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
        //        {
        //            if ((Convert.ToInt32(dataGridView1[0, e.RowIndex].Value) == 1))
        //            {
        //                MessageBox.Show("aa");
        //            }
        //        }
        //    }
        //}

        private void button1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Single.Parse(textBox1.Text) == 0)
            {
                string kayit2 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye+'" + textBox4.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                string kayit = "update HesaplarTBL set bakiye=bakiye+'" + textBox4.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                if (db.EkleSilGuncelle(kayit) > 0)
                { }
                if (db.EkleSilGuncelle(kayit2) > 0)
                {
                    if (db.EkleSilGuncelle("Insert into IslemlerTBL values('" + textBox6.Text + "','" + 1 + "','" + textBox4.Text + "','" + dateTimePicker2.Text + "')") > 0)
                        MessageBox.Show("Bakiye Eklendi");
                    // MessageBox.Show("islemler Hesap Açıldı");
                    textBox2.Text=(Single.Parse(textBox2.Text)+Single.Parse(textBox4.Text)).ToString();
                    textBox3.Text = (Single.Parse(textBox3.Text) + Single.Parse(textBox4.Text)).ToString();
                }
            }
            else if (Single.Parse(textBox1.Text) < 0 )
            {
                    string kayit = "update HesaplarTBL set ekHesap='" + 0 + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                    string kayit2 = "update HesaplarTBL set bakiye=bakiye+'" + (Single.Parse(textBox4.Text) + Single.Parse(textBox1.Text)).ToString() + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                    string kayit3 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye+'" + textBox4.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                    if (db.EkleSilGuncelle(kayit) > 0)
                    { }
                    if (db.EkleSilGuncelle(kayit2) > 0)
                    {}
                    if (db.EkleSilGuncelle(kayit3) > 0)
                    {
                        if (db.EkleSilGuncelle("Insert into IslemlerTBL values('" + textBox6.Text + "','" + 1 + "','" + textBox4.Text + "','" + dateTimePicker2.Text + "')") > 0)
                              MessageBox.Show("Bakiye Eklendi");
                        //MessageBox.Show("islemler Hesap Açıldı");
                        textBox2.Text=(Single.Parse(textBox2.Text)+(Single.Parse(textBox4.Text)+Single.Parse(textBox1.Text))).ToString();
                        textBox3.Text = (Single.Parse(textBox3.Text) +Single.Parse(textBox4.Text)).ToString();
                        textBox1.Text="0";
                    }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

        }

        private void button5_Click(object sender, EventArgs e)
        {


            if (int.Parse(textBox7.Text) <= 750)
            {
                //if ((string.Compare(textBox2.Text, textBox7.Text) == 1 || string.Compare(textBox2.Text, textBox7.Text) == 0) && Single.Parse(textBox2.Text) != 0)
                //{
                if(Single.Parse(textBox2.Text) >=Single.Parse(textBox7.Text) && Single.Parse(textBox2.Text)!=0)
                {

                    string kayit = "update HesaplarTBL set bakiye=bakiye-'" + textBox7.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                    if (db.EkleSilGuncelle(kayit) > 0)
                    { }
                    string kayit2 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye-'" + textBox7.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                    if (db.EkleSilGuncelle(kayit2) > 0)
                    {

                        if (db.EkleSilGuncelle("Insert into IslemlerTBL values('" + textBox6.Text + "','" + 2 + "','" + textBox7.Text + "','" + dateTimePicker3.Text + "')") > 0)
                            MessageBox.Show("Bakiye Silindi");
                        //MessageBox.Show("islemler Hesap Açıldı");
                        textBox2.Text = (Single.Parse(textBox2.Text) - Single.Parse(textBox7.Text)).ToString();
                        textBox3.Text = (Single.Parse(textBox3.Text) - Single.Parse(textBox7.Text)).ToString();

                    }
                }
                else
                {
                    if (Single.Parse(textBox7.Text) - Single.Parse(textBox2.Text) < 400 && Single.Parse(textBox1.Text) + (Single.Parse(textBox2.Text) - Single.Parse(textBox7.Text))>= -400)
                    {
                        string kayit = "update HesaplarTBL set bakiye='" + 0 + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                        string kayit2 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye-'" + textBox7.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                        if (db.EkleSilGuncelle(kayit) > 0)
                        { }
                        if (db.EkleSilGuncelle(kayit2) > 0)
                        { }
                        string kayit3 = "update HesaplarTBL set ekHesap=ekHesap-'" + (Single.Parse(textBox7.Text) - Single.Parse(textBox2.Text)).ToString() + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                        //string kayit4 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye-'" + textBox4.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                        if (db.EkleSilGuncelle(kayit3) > 0)
                        {
                            if (db.EkleSilGuncelle("Insert into IslemlerTBL values('" + textBox6.Text + "','" + 2 + "','" + textBox7.Text + "','" + dateTimePicker3.Text + "')") > 0)
                                MessageBox.Show("bakiye silindi");
                            // MessageBox.Show("islemler Hesap Açıldı");
                        }
                        textBox1.Text = (Single.Parse(textBox1.Text) - (Single.Parse(textBox7.Text) - Single.Parse(textBox2.Text))).ToString();
                        textBox3.Text = (Single.Parse(textBox3.Text) - Single.Parse(textBox7.Text)).ToString();
                        textBox2.Text = "0";
                    }
                    else
                    {
                        MessageBox.Show("Yetersiz Bakiye");
                    }

                }
            }
            else
            {
                MessageBox.Show("Günlük 750 TL Çekilebilir");
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                if (Single.Parse(textBox2.Text) == 0 || Single.Parse(textBox2.Text)<Single.Parse(textBox9.Text))
                {
                    MessageBox.Show("Bakiye Yetersiz");
                }
                else
                {
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "Select * from HesaplarTBL";
                    SqlDataReader reader = komut.ExecuteReader();

                        while (reader.Read())
                        {
                            if (textBox8.Text == reader.GetInt32(0).ToString())
                            {
                                count++;
                                string kayit = "update HesaplarTBL set bakiye=bakiye-'" + textBox9.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                                if (db.EkleSilGuncelle(kayit) > 0)
                                { }
                                string kayit3 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye-'" + textBox9.Text + "' where MusteriNo=" + textBox5.Text + "and hesapNo=" + textBox6.Text;
                                if (db.EkleSilGuncelle(kayit3) > 0)
                                { }
                                string kayit4 = "update HesaplarTBL set kullanilabilirBakiye=kullanilabilirBakiye+'" + textBox9.Text + "' where hesapNo=" + textBox8.Text;
                                if (db.EkleSilGuncelle(kayit4) > 0)
                                { }
                                string kayit2 = "update HesaplarTBL set bakiye=bakiye+'" + textBox9.Text + "'where hesapNo=" + textBox8.Text;
                                if (db.EkleSilGuncelle(kayit2) > 0)
                                {
                                    //MessageBox.Show("Havale gerçekleşti");
                                    if (db.EkleSilGuncelle("Insert into IslemlerTBL values('" + textBox6.Text + "','" + 3 + "','" + textBox9.Text + "','" + dateTimePicker1.Text + "')") > 0)
                                        //MessageBox.Show("islemler Hesap Açıldı");
                                        if (db.EkleSilGuncelle("Insert into havaleTBL values((select max(islemNo) from IslemlerTBL)"+",'"+ textBox6.Text + "','" + textBox8.Text + "','" + dateTimePicker1.Text + "')") > 0)
                                            MessageBox.Show("HAVALE YAPILDI");
                                    // MessageBox.Show("havale Hesap Açıldı");
                                }
                                textBox2.Text = (Single.Parse(textBox2.Text) - Single.Parse(textBox9.Text)).ToString();
                                textBox3.Text = (Single.Parse(textBox3.Text) - Single.Parse(textBox9.Text)).ToString();
                            }
                        }
                        if (count == 0)
                        {
                            MessageBox.Show("müşteri bulunamadı");
                        }
                        reader.Close();
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            try
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "SELECT * FROM IslemlerTBL WHERE hesapNo='" + textBox6.Text + "'and tarih BETWEEN '" + dateTimePicker4.Text + "' AND '" + dateTimePicker5.Text + "'";
                SqlDataReader reader = komut.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetInt32(2) == 1)
                    {
                        listBox1.Items.Add(reader.GetDecimal(3));
                    }
                    else if (reader.GetInt32(2) == 2)
                    {

                        listBox2.Items.Add(reader.GetDecimal(3));
                    }
                    else if (reader.GetInt32(1).ToString() == textBox6.Text)
                    {
                        listBox3.Items.Add(reader.GetInt32(1)+"  "+reader.GetDecimal(3));
                    }
                }
                reader.Close();  
                }
            
            catch
            {
                MessageBox.Show("hata");
            }
            try
            {
                SqlCommand komut2 = new SqlCommand();
                komut2.Connection = baglanti;
                komut2.CommandText = "select * from IslemlerTBL where islemTuru='" + 3 + "'and hesapNo in (SELECT gonderenHesap FROM havaleTBL WHERE alanHesap='" + textBox6.Text + "'and tarih BETWEEN'" + dateTimePicker4.Text + "' AND '" + dateTimePicker5.Text+ "')" ;
                SqlDataReader readerr = komut2.ExecuteReader();

                while (readerr.Read())
                {
                    listBox4.Items.Add(readerr.GetInt32(1) + "  " + readerr.GetDecimal(3));

                }
                readerr.Close();
            }
            catch 
            {
                MessageBox.Show("hata");
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
    }
}