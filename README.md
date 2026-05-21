# 📊 Mağaza Otomasyon ve Raporlama Sistemi

Bu proje, bir teknoloji mağazasının stok yönetimini, personel takibini ve muhasebe süreçlerini tek bir merkezden yürütmek amacıyla geliştirilmiş masaüstü tabanlı bir otomasyon yazılımıdır. İlişkisel veritabanı sunucularına ihtiyaç duymadan, tamamen **dosya tabanlı (.csv)** bir veri mimarisiyle çalışır.

Sistem, geleneksel bellek tüketen çoklu form pencereleri yerine, ana belleği yormayan ve her işletim sisteminde akıcı çalışan **UserControl** tabanlı dinamik bir panel mimarisine sahiptir.

---

## 🚀 Özellikler ve 10 Farklı Form Fonksiyonu

Sistem tek bir ana gövde üzerinde tam 10 farklı işlevsel ekranı dinamik olarak yükler:

1. **Güvenli Giriş Sistemi (Login):** Personel kullanıcı adı ve şifre kontrolü ile yetkilendirme.
2. **Ana Panel (Dashboard):** Aktif kullanıcıyı karşılayan ve sistem özetini sunan karşılama ekranı.
3. **Mevcut Stok Listesi:** Tüm ürünlerin barkod, kategori, miktar ve birim fiyatlarını `DataGridView` üzerinde listeleme.
4. **Yeni Ürün Ekleme:** Hata payını sıfırlayan sayısal veri doğrulamalı (Validasyon) ürün kayıt formu.
5. **Stoktan Ürün Çıkarma:** Benzersiz Ürün ID (Barkod) üzerinden stoktan kalıcı ürün silme.
6. **Aktif Personel Listesi:** Mağazada çalışan tüm personellerin bilgilerini tablolama.
7. **Yeni Personel Ekleme:** Sisteme erişim sağlayacak yeni kasiyer/yönetici kartı oluşturma.
8. **Personel İlişik Kesme:** Kullanıcı adı üzerinden personel yetki ve kayıtlarını sistemden kaldırma.
9. **Hızlı Perakende Satış:** Ürün ID ve adet doğrulamasıyla anlık fatura kesme ve satış günlüğüne işleme.
10. **Finansal Raporlama ve Çıktı Sistemi:** * Belirlenen iki tarih arasındaki mağaza cirosunu ekranda hesaplama.
    * Tek tıkla Masaüstüne resmi **Gün Sonu** veya **Ay Sonu** raporu (.txt) çıkarma.
    * SaveFileDialog entegrasyonu ile raporu sistemdeki **istenen herhangi bir konuma** (Flash bellek vb.) serbestçe kaydetme.

---

## 📂 Veri Tabanı ve Klasör Yapısı

Sistem, verilerin taşınabilirliğini sağlamak için programın çalıştığı dizindeki `data/` klasörünü baz alır:

```text
cash_register_automation/
│
├── data/
│   ├── kullanicilar.csv   # Personel kimlik ve giriş bilgileri
│   ├── stok.csv           # Ürünlerin güncel mali ve adet bilgileri
│   └── satislar.csv       # Kronolojik satış günlükleri (Log)
│
└── Program.cs             # Kaynak kod dosyası
