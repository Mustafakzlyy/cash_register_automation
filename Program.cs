using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VatanBilgisayarOtomasyonu
{
    public class LoginForm : Form
    {
        private TextBox txtKullanici;
        private TextBox txtSifre;
        private Button btnGiris;
        private Label lblKullanici;
        private Label lblSifre;
        private string csvYolu;

        public LoginForm()
        {
            csvYolu = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "kullanicilar.csv");

            this.Text = "Personel Giriş Sistemi";
            this.Size = new Size(380, 260);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblKullanici = new Label() { Text = "Kullanıcı Adı:", Left = 40, Top = 50, Width = 100 };
            txtKullanici = new TextBox() { Left = 150, Top = 46, Width = 160 };

            lblSifre = new Label() { Text = "Şifre:", Left = 40, Top = 90, Width = 100 };
            txtSifre = new TextBox() { Left = 150, Top = 86, Width = 160, PasswordChar = '*' };

            btnGiris = new Button() { Text = "Sisteme Giriş Yap", Left = 150, Top = 140, Width = 160, Height = 35 };
            btnGiris.Click += BtnGiris_Click;

            this.Controls.Add(lblKullanici);
            this.Controls.Add(txtKullanici);
            this.Controls.Add(lblSifre);
            this.Controls.Add(txtSifre);
            this.Controls.Add(btnGiris);
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            string girilenKullanici = txtKullanici.Text.Trim();
            string girilenSifre = txtSifre.Text.Trim();
            bool girisBasarili = false;

            if (File.Exists(csvYolu))
            {
                var satirlar = File.ReadAllLines(csvYolu);
                foreach (var satir in satirlar)
                {
                    var hucreler = satir.Split(',');
                    if (hucreler.Length >= 5)
                    {
                        if (hucreler[0].Trim() == girilenKullanici && hucreler[4].Trim() == girilenSifre)
                        {
                            girisBasarili = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı veri dosyası bulunamadı! Lütfen programın çalıştığı yerde 'data' klasörü ve içinde 'kullanicilar.csv' olduğundan emin olun.");
                return;
            }

            if (girisBasarili)
            {
                this.Hide();
                AnaForm anaPanel = new AnaForm(girilenKullanici);
                anaPanel.Show();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre girdiniz!");
            }
        }
    }

    public class AnaForm : Form
    {
        private string aktifKullanici;
        private Panel solMenu;
        private Panel anaIcerikPaneli;
        
        private string kullaniciCsv;
        private string stokCsv;
        private string satisCsv;
        private bool uygulamaKapatiliyor = false;

        public AnaForm(string kullanici)
        {
            aktifKullanici = kullanici;
            string dataDizini = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            kullaniciCsv = Path.Combine(dataDizini, "kullanicilar.csv");
            stokCsv = Path.Combine(dataDizini, "stok.csv");
            satisCsv = Path.Combine(dataDizini, "satislar.csv");

            this.Text = "Otomasyon Ana Yönetim Merkezi - Aktif Kullanıcı: " + aktifKullanici;
            this.Size = new Size(1100, 680);
            this.StartPosition = FormStartPosition.CenterScreen;

            solMenu = new Panel() { Width = 230, Dock = DockStyle.Left, BackColor = Color.FromArgb(41, 57, 85) };
            anaIcerikPaneli = new Panel() { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };

            this.Controls.Add(anaIcerikPaneli);
            this.Controls.Add(solMenu);

            MenuButonlariniOlustur();
            FormGoster(new HoşgeldinGörünümü(aktifKullanici));
        }

        private void MenuButonlariniOlustur()
        {
            Label lblBaslik = new Label() { Text = "X", Top = 15, Left = 10, Width = 210, Height = 30, ForeColor = Color.White, Font = new Font("Arial", 14, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            solMenu.Controls.Add(lblBaslik);

            string[] butonlar = { "Ana Panel", "Ürün Listesi", "Ürün Ekle", "Ürün Çıkar", "Personel Listesi", "Personel Ekle", "Personel Çıkar", "Yeni Satış Yap", "Son Satışlar", "Ciro Raporu", "Güvenli Çıkış" };
            int baslangicTop = 60;

            for (int i = 0; i < butonlar.Length; i++)
            {
                Button btn = new Button()
                {
                    Text = butonlar[i],
                    Top = baslangicTop + (i * 48),
                    Left = 10,
                    Width = 210,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += MenuButon_Click;
                solMenu.Controls.Add(btn);
            }
        }

        private void MenuButon_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            if (btn.Text == "Ana Panel") FormGoster(new HoşgeldinGörünümü(aktifKullanici));
            else if (btn.Text == "Ürün Listesi") FormGoster(new UrunListesiGörünümü(stokCsv));
            else if (btn.Text == "Ürün Ekle") FormGoster(new UrunEkleGörünümü(stokCsv));
            else if (btn.Text == "Ürün Çıkar") FormGoster(new UrunCikarGörünümü(stokCsv));
            else if (btn.Text == "Personel Listesi") FormGoster(new ElemanListesiGörünümü(kullaniciCsv));
            else if (btn.Text == "Personel Ekle") FormGoster(new ElemanEkleGörünümü(kullaniciCsv));
            else if (btn.Text == "Personel Çıkar") FormGoster(new ElemanCikarGörünümü(kullaniciCsv));
            else if (btn.Text == "Yeni Satış Yap") FormGoster(new SatisYapGörünümü(stokCsv, satisCsv, aktifKullanici));
            else if (btn.Text == "Son Satışlar") FormGoster(new SonSatislarGörünümü(satisCsv));
            else if (btn.Text == "Ciro Raporu") FormGoster(new CiroRaporuGörünümü(satisCsv));
            else if (btn.Text == "Güvenli Çıkış")
            {
                uygulamaKapatiliyor = true;
                Application.Restart();
            }
        }

        private void FormGoster(Control icerik)
        {
            anaIcerikPaneli.Controls.Clear();
            icerik.Dock = DockStyle.Fill;
            anaIcerikPaneli.Controls.Add(icerik);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!uygulamaKapatiliyor)
            {
                uygulamaKapatiliyor = true;
                Application.Exit();
            }
        }
    }

    public class HoşgeldinGörünümü : UserControl
    {
        public HoşgeldinGörünümü(string kullanici)
        {
            Label lbl = new Label() { Text = "Otomasyon Sistemine Hoş Geldiniz\n\nAktif Kullanıcı Yetkisi: " + kullanici + "\n\nLütfen işlem yapmak için sol menüyü kullanın.", Top = 100, Left = 50, Width = 600, Height = 200, Font = new Font("Arial", 14, FontStyle.Regular) };
            this.Controls.Add(lbl);
        }
    }

    public class UrunListesiGörünümü : UserControl
    {
        private DataGridView grid;

        public UrunListesiGörünümü(string yol)
        {
            Label lblBaslik = new Label() { Text = "Mevcut Mağaza Stok Listesi", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            grid = new DataGridView() { Left = 30, Top = 70, Width = 750, Height = 450, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, ReadOnly = true };
            this.Controls.Add(grid);

            grid.Columns.Add("Id", "Ürün Kodu");
            grid.Columns.Add("Ad", "Ürün Adı");
            grid.Columns.Add("Kat", "Kategori");
            grid.Columns.Add("Stok", "Stok Miktarı");
            grid.Columns.Add("Fiyat", "Birim Fiyat");

            if (File.Exists(yol))
            {
                var satirlar = File.ReadAllLines(yol);
                foreach (var satir in satirlar)
                {
                    var h = satir.Split(',');
                    if (h.Length >= 5)
                    {
                        int stokMiktari = 0;
                        double birimFiyat = 0;
                        int.TryParse(h[3].Trim(), out stokMiktari);
                        double.TryParse(h[4].Trim(), out birimFiyat);
                        grid.Rows.Add(h[0], h[1], h[2], stokMiktari, birimFiyat.ToString("N2") + " TL");
                    }
                }
            }
        }
    }

    public class UrunEkleGörünümü : UserControl
    {
        private string dosyaYolu;
        private TextBox txtId, txtAd, txtKat, txtStok, txtFiyat;
        private Button btnKaydet;

        public UrunEkleGörünümü(string yol)
        {
            dosyaYolu = yol;

            Label lblBaslik = new Label() { Text = "Yeni Teknolojik Ürün Ekleme Formu", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            string[] etiketler = { "Ürün ID (Barkod):", "Ürün Adı:", "Kategori:", "Stok Adedi (Sayı):", "Satış Fiyatı (Sayı):" };
            for (int i = 0; i < etiketler.Length; i++)
            {
                this.Controls.Add(new Label() { Text = etiketler[i], Left = 30, Top = 80 + (i * 40), Width = 120 });
            }

            txtId = new TextBox() { Left = 160, Top = 76, Width = 200 };
            txtAd = new TextBox() { Left = 160, Top = 116, Width = 200 };
            txtKat = new TextBox() { Left = 160, Top = 156, Width = 200 };
            txtStok = new TextBox() { Left = 160, Top = 196, Width = 200 };
            txtFiyat = new TextBox() { Left = 160, Top = 236, Width = 200 };

            btnKaydet = new Button() { Text = "Ürünü Stoğa Kaydet", Left = 160, Top = 290, Width = 200, Height = 35 };
            btnKaydet.Click += BtnKaydet_Click;

            this.Controls.Add(txtId); this.Controls.Add(txtAd); this.Controls.Add(txtKat);
            this.Controls.Add(txtStok); this.Controls.Add(txtFiyat); this.Controls.Add(btnKaydet);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtAd.Text) || string.IsNullOrEmpty(txtStok.Text) || string.IsNullOrEmpty(txtFiyat.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            int stokTest;
            double fiyatTest;

            if (!int.TryParse(txtStok.Text.Trim(), out stokTest) || !double.TryParse(txtFiyat.Text.Trim(), out fiyatTest))
            {
                MessageBox.Show("Stok adedi tam sayı, fiyat ise sayısal bir değer olmalıdır!");
                return;
            }

            string yeniSatir = $"{txtId.Text.Trim()},{txtAd.Text.Trim()},{txtKat.Text.Trim()},{stokTest},{fiyatTest}\n";
            File.AppendAllText(dosyaYolu, yeniSatir);
            MessageBox.Show("Ürün başarıyla sayısal değerleriyle stoğa eklendi!");
            
            txtId.Clear(); txtAd.Clear(); txtKat.Clear(); txtStok.Clear(); txtFiyat.Clear();
        }
    }

    public class UrunCikarGörünümü : UserControl
    {
        private string dosyaYolu;
        private TextBox txtId;
        private Button btnSil;

        public UrunCikarGörünümü(string yol)
        {
            dosyaYolu = yol;

            Label lblBaslik = new Label() { Text = "Stoktan Ürün Silme Formu", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            Label lblId = new Label() { Text = "Silinecek Ürün ID:", Left = 30, Top = 90, Width = 120 };
            txtId = new TextBox() { Left = 160, Top = 86, Width = 200 };
            btnSil = new Button() { Text = "Ürünü Kalıcı Olarak Sil", Left = 160, Top = 130, Width = 200, Height = 35 };
            btnSil.Click += BtnSil_Click;

            this.Controls.Add(lblBaslik); this.Controls.Add(lblId); this.Controls.Add(txtId); this.Controls.Add(btnSil);
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            string arananId = txtId.Text.Trim();
            if (string.IsNullOrEmpty(arananId)) return;

            if (File.Exists(dosyaYolu))
            {
                var satirlar = File.ReadAllLines(dosyaYolu);
                var yeniList = new System.Collections.Generic.List<string>();
                bool bulundu = false;

                foreach (var satir in satirlar)
                {
                    var hucre = satir.Split(',');
                    if (hucre.Length > 0 && hucre[0].Trim() == arananId)
                    {
                        bulundu = true;
                        continue;
                    }
                    yeniList.Add(satir);
                }

                if (bulundu)
                {
                    File.WriteAllLines(dosyaYolu, yeniList);
                    MessageBox.Show("Ürün stoktan tamamen kaldırıldı.");
                    txtId.Clear();
                }
                else
                {
                    MessageBox.Show("Böyle bir ürün kodu bulunamadı!");
                }
            }
        }
    }

    public class ElemanEkleGörünümü : UserControl
    {
        private string dosyaYolu;
        private TextBox txtKullanici, txtAd, txtSoyad, txtEposta, txtSifre;
        private Button btnEkle;

        public ElemanEkleGörünümü(string yol)
        {
            dosyaYolu = yol;

            Label lblBaslik = new Label() { Text = "Yeni Personel Kayıt Formu", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            string[] etiketler = { "Kullanıcı Adı:", "İsim:", "Soyisim:", "E-Posta Adresi:", "Sisteme Giriş Şifresi:" };
            for (int i = 0; i < etiketler.Length; i++)
            {
                this.Controls.Add(new Label() { Text = etiketler[i], Left = 30, Top = 80 + (i * 40), Width = 130 });
            }

            txtKullanici = new TextBox() { Left = 170, Top = 76, Width = 200 };
            txtAd = new TextBox() { Left = 170, Top = 116, Width = 200 };
            txtSoyad = new TextBox() { Left = 170, Top = 156, Width = 200 };
            txtEposta = new TextBox() { Left = 170, Top = 196, Width = 200 };
            txtSifre = new TextBox() { Left = 170, Top = 236, Width = 200 };

            btnEkle = new Button() { Text = "Personel Kartı Oluştur", Left = 170, Top = 290, Width = 200, Height = 35 };
            btnEkle.Click += BtnEkle_Click;

            this.Controls.Add(txtKullanici); this.Controls.Add(txtAd); this.Controls.Add(txtSoyad);
            this.Controls.Add(txtEposta); this.Controls.Add(txtSifre); this.Controls.Add(btnEkle);
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKullanici.Text) || string.IsNullOrEmpty(txtSifre.Text)) return;

            string veri = $"{txtKullanici.Text.Trim()},{txtAd.Text.Trim()},{txtSoyad.Text.Trim()},{txtEposta.Text.Trim()},{txtSifre.Text.Trim()}\n";
            File.AppendAllText(dosyaYolu, veri);
            MessageBox.Show("Yeni çalışan sisteme başarıyla kaydedildi.");

            txtKullanici.Clear(); txtAd.Clear(); txtSoyad.Clear(); txtEposta.Clear(); txtSifre.Clear();
        }
    }

    public class ElemanCikarGörünümü : UserControl
    {
        private string dosyaYolu;
        private TextBox txtKullanici;
        private Button btnSil;

        public ElemanCikarGörünümü(string yol)
        {
            dosyaYolu = yol;

            Label lblBaslik = new Label() { Text = "Personel İlişik Kesme Formu", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            Label lblUser = new Label() { Text = "Personel Kullanıcı Adı:", Left = 30, Top = 90, Width = 140 };
            txtKullanici = new TextBox() { Left = 180, Top = 86, Width = 200 };
            btnSil = new Button() { Text = "Yetkilerini İptal Et ve Sil", Left = 180, Top = 130, Width = 200, Height = 35 };
            btnSil.Click += BtnSil_Click;

            this.Controls.Add(lblBaslik); this.Controls.Add(lblUser); this.Controls.Add(txtKullanici); this.Controls.Add(btnSil);
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            string aranan = txtKullanici.Text.Trim();
            if (string.IsNullOrEmpty(aranan)) return;

            if (File.Exists(dosyaYolu))
            {
                var satirlar = File.ReadAllLines(dosyaYolu);
                var yeniList = new System.Collections.Generic.List<string>();
                bool kontrol = false;

                foreach (var satir in satirlar)
                {
                    var hucre = satir.Split(',');
                    if (hucre.Length > 0 && hucre[0].Trim() == aranan)
                    {
                        kontrol = true;
                        continue;
                    }
                    yeniList.Add(satir);
                }

                if (kontrol)
                {
                    File.WriteAllLines(dosyaYolu, yeniList);
                    MessageBox.Show("Personel kaydı sistemden güvenli biçimde silindi.");
                    txtKullanici.Clear();
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı.");
                }
            }
        }
    }

    public class ElemanListesiGörünümü : UserControl
    {
        private DataGridView grid;

        public ElemanListesiGörünümü(string yol)
        {
            Label lblBaslik = new Label() { Text = "Aktif Çalışan Listesi", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            grid = new DataGridView() { Left = 30, Top = 70, Width = 750, Height = 450, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, ReadOnly = true };
            this.Controls.Add(grid);

            grid.Columns.Add("User", "Kullanıcı Adı");
            grid.Columns.Add("Name", "İsim");
            grid.Columns.Add("Surname", "Soyisim");
            grid.Columns.Add("Mail", "E-Posta");

            if (File.Exists(yol))
            {
                var satirlar = File.ReadAllLines(yol);
                foreach (var satir in satirlar)
                {
                    var h = satir.Split(',');
                    if (h.Length >= 4)
                    {
                        grid.Rows.Add(h[0], h[1], h[2], h[3]);
                    }
                }
            }
        }
    }

    public class SatisYapGörünümü : UserControl
    {
        private string stokPath, satisPath, kasiyer;
        private TextBox txtUrunId, txtAdet, txtFiyat;
        private Button btnSatis;

        public SatisYapGörünümü(string sPath, string saPath, string user)
        {
            stokPath = sPath; satisPath = saPath; kasiyer = user;

            Label lblBaslik = new Label() { Text = "Hızlı Fatura ve Perakende Satış Formu", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            string[] etiketler = { "Satılacak Ürün ID:", "Satış Adedi (Sayı):", "Birim Fiyatı (Sayı):" };
            for (int i = 0; i < etiketler.Length; i++)
            {
                this.Controls.Add(new Label() { Text = etiketler[i], Left = 30, Top = 80 + (i * 40), Width = 120 });
            }

            txtUrunId = new TextBox() { Left = 160, Top = 76, Width = 200 };
            txtAdet = new TextBox() { Left = 160, Top = 116, Width = 200 };
            txtFiyat = new TextBox() { Left = 160, Top = 156, Width = 200 };

            btnSatis = new Button() { Text = "Faturayı Kes ve Satışı Onayla", Left = 160, Top = 210, Width = 200, Height = 35 };
            btnSatis.Click += BtnSatis_Click;

            this.Controls.Add(txtUrunId); this.Controls.Add(txtAdet); this.Controls.Add(txtFiyat); this.Controls.Add(btnSatis);
        }

        private void BtnSatis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrunId.Text) || string.IsNullOrEmpty(txtAdet.Text) || string.IsNullOrEmpty(txtFiyat.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            int adet;
            double birimFiyat;

            if (!int.TryParse(txtAdet.Text.Trim(), out adet) || !double.TryParse(txtFiyat.Text.Trim(), out birimFiyat))
            {
                MessageBox.Show("Adet tam sayı, birim fiyatı sayısal olmalıdır!");
                return;
            }

            double toplam = adet * birimFiyat;
            string bugun = DateTime.Now.ToString("yyyy-MM-dd");
            string veri = $"{bugun},{kasiyer},{txtUrunId.Text.Trim()},{adet},{toplam}\n";

            File.AppendAllText(satisPath, veri);
            MessageBox.Show($"Satış başarıyla gerçekleştirildi.\nToplam Tahsil Edilen: {toplam.ToString("N2")} TL");

            txtUrunId.Clear(); txtAdet.Clear(); txtFiyat.Clear();
        }
    }

    public class SonSatislarGörünümü : UserControl
    {
        private DataGridView grid;

        public SonSatislarGörünümü(string yol)
        {
            Label lblBaslik = new Label() { Text = "Mağaza Genel Son Satış Günlüğü", Top = 20, Left = 30, Width = 400, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            grid = new DataGridView() { Left = 30, Top = 70, Width = 750, Height = 450, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, ReadOnly = true };
            this.Controls.Add(grid);

            grid.Columns.Add("Tarih", "İşlem Tarihi");
            grid.Columns.Add("Kasiyer", "İşlemi Yapan");
            grid.Columns.Add("Urun", "Ürün Kodu");
            grid.Columns.Add("Adet", "Miktar");
            grid.Columns.Add("Tutar", "Toplam İşlem Tutarı");

            if (File.Exists(yol))
            {
                var satirlar = File.ReadAllLines(yol);
                foreach (var satir in satirlar)
                {
                    var h = satir.Split(',');
                    if (h.Length >= 5)
                    {
                        double tutar = 0;
                        double.TryParse(h[4].Trim(), out tutar);
                        grid.Rows.Add(h[0], h[1], h[2], h[3], tutar.ToString("N2") + " TL");
                    }
                }
            }
        }
    }

    public class CiroRaporuGörünümü : UserControl
    {
        private string dosyaYolu;
        private DateTimePicker dtBas, dtBit;
        private Button btnHesap, btnMasaustuGun, btnMasaustuAy, btnSerbestKaydet;
        private Label lblSonuc;

        public CiroRaporuGörünümü(string yol)
        {
            dosyaYolu = yol;

            Label lblBaslik = new Label() { Text = "Dönemsel Finansal Ciro Raporu ve Çıktı Alma Paneli", Top = 20, Left = 30, Width = 500, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(lblBaslik);

            Label lbl1 = new Label() { Text = "Başlangıç Tarihi:", Left = 30, Top = 80, Width = 120 };
            dtBas = new DateTimePicker() { Left = 160, Top = 76, Width = 150, Format = DateTimePickerFormat.Short };

            Label lbl2 = new Label() { Text = "Bitiş Tarihi:", Left = 30, Top = 120, Width = 120 };
            dtBit = new DateTimePicker() { Left = 160, Top = 116, Width = 150, Format = DateTimePickerFormat.Short };

            btnHesap = new Button() { Text = "Ekranda Hesapla", Left = 160, Top = 165, Width = 150, Height = 35 };
            btnHesap.Click += BtnHesap_Click;

            lblSonuc = new Label() { Left = 30, Top = 220, Width = 500, Height = 40, Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.DarkGreen, Text = "Toplam Mağaza Cirosu: 0.00 TL" };

            btnMasaustuGun = new Button() { Text = "Gün Sonu Raporu Al (Masaüstü)", Left = 30, Top = 280, Width = 230, Height = 40 };
            btnMasaustuGun.Click += BtnMasaustuGun_Click;

            btnMasaustuAy = new Button() { Text = "Ay Sonu Raporu Al (Masaüstü)", Left = 280, Top = 280, Width = 230, Height = 40 };
            btnMasaustuAy.Click += BtnMasaustuAy_Click;

            btnSerbestKaydet = new Button() { Text = "İstediğim Yere Rapor Kaydet...", Left = 30, Top = 340, Width = 480, Height = 40, BackColor = Color.LightBlue };
            btnSerbestKaydet.Click += BtnSerbestKaydet_Click;

            this.Controls.Add(lbl1); this.Controls.Add(dtBas);
            this.Controls.Add(lbl2); this.Controls.Add(dtBit);
            this.Controls.Add(btnHesap); this.Controls.Add(lblSonuc);
            this.Controls.Add(btnMasaustuGun); this.Controls.Add(btnMasaustuAy);
            this.Controls.Add(btnSerbestKaydet);
        }

        private double CiroHesapla(DateTime baslangic, DateTime bitis)
        {
            double kasa = 0;
            if (File.Exists(dosyaYolu))
            {
                var satirlar = File.ReadAllLines(dosyaYolu);
                foreach (var satir in satirlar)
                {
                    var h = satir.Split(',');
                    if (h.Length >= 5)
                    {
                        DateTime t;
                        if (DateTime.TryParse(h[0].Trim(), out t))
                        {
                            if (t.Date >= baslangic.Date && t.Date <= bitis.Date)
                            {
                                double satirTutar = 0;
                                double.TryParse(h[4].Trim(), out satirTutar);
                                kasa += satirTutar;
                            }
                        }
                    }
                }
            }
            return kasa;
        }

        private void BtnHesap_Click(object sender, EventArgs e)
        {
            double sonuc = CiroHesapla(dtBas.Value, dtBit.Value);
            lblSonuc.Text = $"Seçilen Tarihler Arası Toplam Mağaza Cirosu: {sonuc:N2} TL";
        }

        private void BtnMasaustuGun_Click(object sender, EventArgs e)
        {
            double bugunkuCiro = CiroHesapla(DateTime.Now, DateTime.Now);
            string masaustuYolu = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Vatan_GunSonu_Raporu.txt");
            
            string raporIcerik = "=========================================\n" +
                                 "GÜN SONU MALİ RAPORU\n" +
                                 "=========================================\n" +
                                 "Rapor Tarihi: " + DateTime.Now.ToShortDateString() + "\n" +
                                 "Günlük Toplam Toplanan Ciro: " + bugunkuCiro.ToString("N2") + " TL\n" +
                                 "=========================================\n" +
                                 "Bu rapor otomasyon tarafından otomatik üretilmiştir.";

            File.WriteAllText(masaustuYolu, raporIcerik);
            MessageBox.Show("Gün sonu mali raporu masaüstünüze başarıyla kaydedildi!\nDosya: " + masaustuYolu);
        }

        private void BtnMasaustuAy_Click(object sender, EventArgs e)
        {
            DateTime ayBaslangic = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime ayBitis = DateTime.Now;
            double aylikCiro = CiroHesapla(ayBaslangic, ayBitis);

            string masaustuYolu = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Vatan_AySonu_Raporu.txt");
            
            string raporIcerik = "=========================================\n" +
                                 "AY SONU MALİ RAPORU\n" +
                                 "=========================================\n" +
                                 "Dönem: " + ayBaslangic.ToShortDateString() + " - " + ayBitis.ToShortDateString() + "\n" +
                                 "Aylık Toplam Akümüle Ciro: " + aylikCiro.ToString("N2") + " TL\n" +
                                 "=========================================\n" +
                                 "Bu rapor otomasyon tarafından otomatik üretilmiştir.";

            File.WriteAllText(masaustuYolu, raporIcerik);
            MessageBox.Show("Ay sonu mali raporu masaüstünüze başarıyla kaydedildi!\nDosya: " + masaustuYolu);
        }

        private void BtnSerbestKaydet_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Metin Dosyası (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
            sfd.FileName = "Vatan_Ozel_Dönem_Raporu.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                double ozelCiro = CiroHesapla(dtBas.Value, dtBit.Value);
                string raporIcerik = "=========================================\n" +
                                     "ÖZEL DÖNEM RAPORU\n" +
                                     "=========================================\n" +
                                     "Seçilen Filtre: " + dtBas.Value.ToShortDateString() + " - " + dtBit.Value.ToShortDateString() + "\n" +
                                     "Hesaplanan Net Dönem Cirosu: " + ozelCiro.ToString("N2") + " TL\n" +
                                     "=========================================\n";

                File.WriteAllText(sfd.FileName, raporIcerik);
                MessageBox.Show("Mali rapor istediğiniz konuma başarıyla kaydedildi:\n" + sfd.FileName);
            }
        }
    }

    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}