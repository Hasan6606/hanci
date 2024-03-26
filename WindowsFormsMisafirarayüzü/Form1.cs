using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsMisafirarayüzü
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Asus\Documents\dijitalhancı.accdb;Persist Security Info=False;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // RadioButton ve DateTimePicker kontrolleri doğru isimlendirilmiş olmalıdır.
            // Örneğin, radioButtonErkek, dateTimePickerGiris vb. gibi.
            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            // MaskedTextBox'tan güvenli bir şekilde tarih almak için TryParseExact kullanılır
            DateTime dogumTarihi;
            bool validDate = DateTime.TryParseExact(maskedTextBoxDogumTarihi.Text, "dd.MM.yyyy",
                                                     CultureInfo.InvariantCulture,
                                                     DateTimeStyles.None,
                                                     out dogumTarihi);

            if (!validDate)
            {
                MessageBox.Show("Lütfen geçerli bir doğum tarihi girin (GG.AA.YYYY).");
                return;
            }
            string kimlikNo = txtKimlikNo.Text;


            string telefon = txtTelefon.Text;
            string email = txtEposta.Text;
            DateTime giris = dateTimePickerGiris.Value;
            DateTime cikis = dateTimePickerCikis.Value;
            string odaNumarasi = txtOdaNumarasi.Text;
            string kartId = txtKartId.Text;


            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Ziyaretciler (Ad, Soyad, DogumTarihi, KimlikNo,Telefon, Email, GirisTarihi, CikisTarihi, OdaNumarasi,KartId) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?,?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Parametreleri sırasıyla ekleyin
                        cmd.Parameters.Add("@Ad", OleDbType.VarChar).Value = ad;
                        cmd.Parameters.Add("@Soyad", OleDbType.VarChar).Value = soyad;
                        // ...
                        cmd.Parameters.Add("@DogumTarihi", OleDbType.Date).Value = dogumTarihi;

                        cmd.Parameters.Add("@KimlikNo", OleDbType.VarChar).Value = kimlikNo;
                        cmd.Parameters.Add("@Telefon", OleDbType.VarChar).Value = telefon;



                        cmd.Parameters.Add("@Email", OleDbType.VarChar).Value = email;
                        cmd.Parameters.Add("@GirisTarihi", OleDbType.VarChar).Value = giris;

                        cmd.Parameters.Add("@CikisTarihi", OleDbType.VarChar).Value = cikis;

                        cmd.Parameters.Add("@OdaNumarasi", OleDbType.VarChar).Value = odaNumarasi;
                        cmd.Parameters.Add("@KartId", OleDbType.VarChar).Value = kartId;



                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Yeni ziyaretçi başarıyla eklendi.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message);
                }
            }
            // Formu temizle
            ClearForm();
        }
        private void ClearForm()
        {
            // Formdaki tüm metin kutularını temizle
            txtAd.Clear();
            txtSoyad.Clear();
            maskedTextBoxDogumTarihi.Clear();
            txtKimlikNo.Clear();
            txtTelefon.Clear();
            txtEposta.Clear();
            dateTimePickerGiris.Value = DateTime.Now;
            dateTimePickerCikis.Value = DateTime.Now;
            txtOdaNumarasi.Clear();
            txtKartId.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string kimlikNo = txtKimlikNo.Text;

            // Veritabanında TC Kimlik Numarası'na göre silme işlemi yapılıyor
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // Önce veritabanında bu T.C. Kimlik Numarası'na sahip bir kayıt olup olmadığını kontrol ediyoruz
                string checkQuery = "SELECT COUNT(*) FROM Ziyaretciler WHERE KimlikNo = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("?", kimlikNo);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Kayıt varsa, silme işlemi yapılıyor
                        string deleteQuery = "DELETE FROM Ziyaretciler WHERE KimlikNo= ?";
                        using (OleDbCommand cmd = new OleDbCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("?", kimlikNo);
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Kullanıcı veritabanından başarıyla silindi.");
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı silinirken bir hata oluştu.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu T.C. Kimlik Numarasına sahip bir kullanıcı yok.");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Formdan alınan TCKimlik numarası
            string KimlikNo = txtKimlikNo.Text;
            // Tarihleri kontrol etmek için bir referans tarihi belirle
            DateTime referansTarih = DateTime.Today; // veya uygulamanız için mantıklı olan başka bir tarih

            // SQL sorgusu ve parametreler listesi
            List<string> setClauses = new List<string>();
            List<OleDbParameter> parameters = new List<OleDbParameter>();

            // Ad alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtAd.Text))
            {
                setClauses.Add("Ad = ?");
                parameters.Add(new OleDbParameter("?", txtAd.Text));
            }

            // Soyad alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtSoyad.Text))
            {
                setClauses.Add("Soyad = ?");
                parameters.Add(new OleDbParameter("?", txtSoyad.Text));
            }
            if (DateTime.TryParse(maskedTextBoxDogumTarihi.Text, out DateTime dogumTarihi))
            {
                setClauses.Add("DogumTarihi = ?");
                parameters.Add(new OleDbParameter("?", dogumTarihi));
            }
            // Telefon alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtTelefon.Text))
            {
                setClauses.Add("Telefon = ?");
                parameters.Add(new OleDbParameter("?", txtTelefon.Text));
            }

            // Email alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtEposta.Text))
            {
                setClauses.Add("Email = ?");
                parameters.Add(new OleDbParameter("?", txtEposta.Text));
            }
            // Giriş tarihi alanı güncelleniyorsa ve değeri referans tarihten farklıysa
            if (dateTimePickerGiris.Value.Date != referansTarih)
            {
                setClauses.Add("GirisTarihi = ?");
                parameters.Add(new OleDbParameter("?", dateTimePickerGiris.Value));
            }

            // Çıkış tarihi alanı güncelleniyorsa ve değeri referans tarihten farklıysa
            if (dateTimePickerCikis.Value.Date != referansTarih)
            {
                setClauses.Add("CikisTarihi = ?");
                parameters.Add(new OleDbParameter("?", dateTimePickerCikis.Value));
            }

            // Oda numarası alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtOdaNumarasi.Text))
            {
                setClauses.Add("OdaNumarasi = ?");
                parameters.Add(new OleDbParameter("?", txtOdaNumarasi.Text));
            }


            // Kart Id
            if (!string.IsNullOrEmpty(txtKartId.Text))
            {
                setClauses.Add("KartId = ?");
                parameters.Add(new OleDbParameter("?", txtKartId.Text));
            }



            // Eğer hiçbir alan güncellenmiyorsa, işlemi bitir.
            if (setClauses.Count == 0)
            {
                MessageBox.Show("Güncellenecek bir bilgi girilmedi.");
                return;
            }

            // SQL sorgusunu oluştur.
            string updateQuery = $"UPDATE Ziyaretciler SET {string.Join(", ", setClauses)} WHERE KimlikNo = ?";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                {
                  
                    // Parametreleri komuta ekle
                    foreach (var parameter in parameters)
                    {
                        updateCmd.Parameters.Add(parameter);
                    }
                

                    // Son parametre olarak TCKimlik numarasını ekle
                    updateCmd.Parameters.Add(new OleDbParameter("?", KimlikNo));

                    
                    // Güncelleme işlemini çalıştır
                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Ziyaretçi bilgileri başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme sırasında bir hata oluştu ya da belirtilen kimlik numarasıyla eşleşen bir kayıt bulunamadı.");
                    }
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}

    





