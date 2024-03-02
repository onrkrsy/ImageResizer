# ImageResizer
# Görsel Yeniden Boyutlandırma Aracı

Bu araç, belirli bir klasördeki tüm görselleri (resim dosyalarını) kullanıcı tarafından belirlenen maksimum genişlik, yükseklik ve dosya boyutu sınırlamalarına göre otomatik olarak yeniden boyutlandırır. Araç, alt klasörler dahil olmak üzere belirtilen dizinde bulunan tüm uygun görsel dosyalarını işler.

## Özellikler

- Desteklenen dosya formatları: JPG, JPEG, PNG, BMP.
- Maksimum genişlik ve yükseklik sınırlamalarına göre otomatik yeniden boyutlandırma.
- Maksimum dosya boyutu sınırlamasına göre kalite ayarlaması.
- Alt klasörlerdeki görselleri de işleme yeteneği.

## Kullanım

### Gereksinimler

Bu aracı kullanabilmek için bilgisayarınızda .NET Core SDK'nın yüklü olması gerekmektedir. .NET Core SDK'yı [buradan](https://dotnet.microsoft.com/download) indirebilirsiniz.

### Başlarken

1. Uygulamayı çalıştırmak için komut istemcisini açın ve uygulamanın bulunduğu dizine gidin.
2. Aşağıdaki komutu kullanarak uygulamayı başlatın:
   ```bash
   dotnet run
   ```
3. Uygulama, görsellerin yeniden boyutlandırılmasını istediğiniz dizini (path) girmenizi isteyecektir. İlgili dizin yolu girildikten sonra, işleme başlamak için `Y` yanıtını verin.

### Yapılandırma

- Maksimum genişlik (`maxWidth`), yükseklik (`maxHeight`) ve dosya boyutu (`maxFileSize`) sınırlamaları kod içerisinde önceden tanımlanmıştır. Bu değerleri ihtiyacınıza göre değiştirmelisiniz.

## Notlar

- İşlem sırasında orijinal dosyalar yeniden boyutlandırılmış görsellerle değiştirilecektir. Orijinal görsellerinizi kaybetmek istemiyorsanız, işleme başlamadan önce bir yedek almanızı öneririz.
- Araç, belirtilen dosya boyutu sınırının altında en iyi kaliteyi sağlamaya çalışırken, bazı durumlarda sıkı dosya boyutu sınırlamaları nedeniyle görsel kalitesinde düşüşler yaşanabilir.
