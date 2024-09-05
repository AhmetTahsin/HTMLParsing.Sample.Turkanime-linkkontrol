using HtmlAgilityPack;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Kontrol edilecek anime bölümü URL'si
        string url = Console.ReadLine();
            //"https://www.turkanime.co/video/ore-wa-subete-wo-8-bolum";

        string seriName = FindSeriName(url);

        int seriChapter = int.Parse(FindSeriChapter(url)) + 1;


        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Web sayfasını indir
                var response = await client.GetStringAsync(url);

                // HTML'yi parse et
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);


                // Aktif butonu tespit etme - link yapısı belirli bir href içerir
                var hrefValue = string.Concat("video", seriName, seriChapter, "-bolum");
                var buttonAktif = htmlDocument.DocumentNode.SelectSingleNode($"//a[contains(@class, 'btn-danger') and contains(@href, '{hrefValue}')]");

                //var buttonAktif = htmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class, 'btn-danger') and contains(@href, 'video/ore-wa-subete-wo-9-bolum')]");


                // Buton durumu kontrolü
                if (buttonAktif != null)
                {
                    Console.WriteLine("Yeni bölüm geldi!");
                }
                else
                {
                    Console.WriteLine("Henüz yeni bölüm yok.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
            }
        }
    }


    static string FindSeriName(string url)
    {
        int bolumLastIndex = url.IndexOf("-bolum");


        do{
            bolumLastIndex--;
        } while (url[bolumLastIndex] != '-');


        int bolumFirstIndex = url.IndexOf("video/") + 5;

        string bolumName = "";

        for (int i = bolumFirstIndex; i <= bolumLastIndex; i++)
        {
            bolumName += url[i];
        }

        return bolumName;

    }


    static string FindSeriChapter(string url)
    {
        int bolumIndex = url.IndexOf("-bolum");

        int chapterStart = bolumIndex-1;

        string bolumNo="";

        while (url[chapterStart] != '-')
        {
            bolumNo += url[chapterStart];
            chapterStart--;
        }
        return new string(bolumNo.Reverse().ToArray());
    }
}