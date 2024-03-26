using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace customerfeedback
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string birim = comboBox4.SelectedIndex.ToString(); // Seçilen birimi al

            // Diğer verileri değişkenlere atayalım
            int hizmet = 0, urun = 0, icmimari = 0;

            if (comboBox1.SelectedItem != null)
                hizmet = Convert.ToInt32(comboBox1.SelectedItem);

            if (comboBox2.SelectedItem != null)
                urun = Convert.ToInt32(comboBox2.SelectedItem);

            if (comboBox3.SelectedItem != null)
                icmimari = Convert.ToInt32(comboBox3.SelectedItem);

            string yorum = richTextBox1.Text;
            DateTime zaman = DateTime.Now;

            // Veritabanı bağlantı cümlesi
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Asus\Documents\dijitalhancı.accdb;Persist Security Info=False;";

            // SQL sorgusu
            string insertQuery = "INSERT INTO ";

            // Seçilen birime göre sorgu oluştur
            string indirimKodu = "";
            switch (birim)
            {
                case "0": // Onofis
                    
                    insertQuery += "onofis (Hizmet, Ürün, Icmimari, Yorum, Zaman) VALUES ";
                    break;
                case "1": // Etevi
                    indirimKodu = "1750STK15";
                    insertQuery += "etevi (Hizmet, Ürün, Icmimari, Yorum, Zaman,Kod) VALUES ";
                    break;
                case "2": // Analokanta
                    indirimKodu = "1750RST15";
                    insertQuery += "analokanta (Hizmet, Ürün, Icmimari, Yorum, Zaman,Kod) VALUES ";
                    break;
                case "3": // Havuzkafe
                    indirimKodu = "1750PB15";
                    insertQuery += "havuzkafe (Hizmet, Ürün, Icmimari, Yorum, Zaman,Kod) VALUES ";
                    break;
                case "4": // Lobi
                    indirimKodu = "1750LB15";
                    insertQuery += "lobi (Hizmet, Ürün, Icmimari, Yorum, Zaman,Kod) VALUES ";
                    break;
                case "5": // Oda servisi
                    indirimKodu = "1750RS15";
                    insertQuery += "odaservisi (Hizmet, Ürün, Icmimari, Yorum, Zaman,Kod) VALUES ";
                    break;
                default:
                    MessageBox.Show("Geçersiz birim seçildi.");
                    return;
            }

            insertQuery += "(@Hizmet, @Ürün, @Icmimari, @Yorum, @Zaman, @Kod)";

            // Bağlantı ve komut oluşturma
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(insertQuery, connection))
            {
                // Parametreleri ekleme
                command.Parameters.Add("@Hizmet", OleDbType.Integer).Value = hizmet;
                command.Parameters.Add("@Ürün", OleDbType.Integer).Value = urun;
                command.Parameters.Add("@Icmimari", OleDbType.Integer).Value = icmimari;
                command.Parameters.Add("@Yorum", OleDbType.VarChar).Value = yorum; // Uzun metin için VarChar kullanılabilir.
                command.Parameters.Add("@Zaman", OleDbType.Date).Value = zaman;
                // Diğer parametreleri ekledikten sonra indirim kodunu da ekleyin
                command.Parameters.Add("@Kod", OleDbType.VarChar).Value = indirimKodu;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Veri başarıyla eklendi. Birimde %15 indirim kodunuz: " + indirimKodu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }

            // ComboBox'ları ve richTextBox'ı temizleyelim
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            richTextBox1.Clear();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
