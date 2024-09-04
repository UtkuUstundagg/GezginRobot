using GezginRobot.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GezginRobot.Services
{
    internal class IzgaraService
    {

        public PictureBox[,] allTiles;
        public List<Hücre> hücreList = new List<Hücre>();
        public List<Engel> engelList = new List<Engel>();


        //problem 2

        public PictureBox[,] mazeTiles;
        public PictureBox baslangicTile;

        public List<Hücre> tileList = new List<Hücre>();
        public List<Hücre> wallList = new List<Hücre>();
        public List<Hücre> cerceveList = new List<Hücre>();

        private int boxSizex;
        private int boxSizey;

        public int BoxSizex { get => boxSizex; set => boxSizex = value; }
        public int BoxSizey { get => boxSizey; set => boxSizey = value; }


        #region Problem1

        
        public Form Problem1IzgaraCiz(Form1 form1, string okunanTxt)
        {

            string[] splitList = okunanTxt.Split('\n');
            BoxSizeBul(splitList.Count() + 2, splitList.Count() + 2);

            char[] satirArray = okunanTxt.ToCharArray();
            char[,] matris = new char[splitList.Count() + 1, splitList.Count() + 1];

            int sayac = 0;
            for (int i = 0; i < splitList.Count(); i++)
            {

                for (int j = 0; j < splitList.Count(); j++)
                {

                    if (satirArray[sayac] != '\n')
                    {
                        matris[i, j] = satirArray[sayac];
                        sayac++;
                    }
                    else
                    {
                        matris[i, j] = satirArray[sayac + 1];
                        sayac = sayac + 2;
                    }
                }
            }


            allTiles = new PictureBox[splitList.Count() + 2, splitList.Count() + 2];

            for (int satir = 0; satir < splitList.Count() + 2; satir++)
            {

                for (int sutun = 0; sutun < splitList.Count() + 2; sutun++)
                {

                    if (satir == 0 || satir == splitList.Count() + 1 || sutun == 0 || sutun == splitList.Count() + 1)//çerçeve ise
                    {
                        allTiles[sutun, satir] = new PictureBox();
                        int xPos = 150 + (satir * BoxSizex);
                        int yPos = 10 + (sutun * BoxSizey);
                        allTiles[sutun, satir].SetBounds(xPos, yPos, BoxSizex, BoxSizey);
                        allTiles[sutun, satir].BackColor = Color.Black;

                        Hücre hücre = new Hücre();
                        hücre.Satir = satir;
                        hücre.Sutun = sutun;
                        hücre.X = xPos;
                        hücre.Y = yPos;
                        hücre.Genislik = BoxSizex;
                        hücre.Yükseklik = BoxSizey;

                        hücreList.Add(hücre);
                    }
                    else//değil ise
                    {
                        allTiles[sutun, satir] = new PictureBox();
                        int xPos = 150 + (satir * BoxSizex);
                        int yPos = 10 + (sutun * BoxSizey);
                        allTiles[sutun, satir].SetBounds(xPos, yPos, BoxSizex, BoxSizey);
                        allTiles[sutun, satir].BackColor = Color.White;

                        Hücre hücre = new Hücre();
                        hücre.Satir = satir;
                        hücre.Sutun = sutun;
                        hücre.X = xPos;
                        hücre.Y = yPos;
                        hücre.Genislik = BoxSizex;
                        hücre.Yükseklik = BoxSizey;

                        hücreList.Add(hücre);

                        Engel engel = new Engel();
                        engel.EngelTipi = Convert.ToInt32(matris[satir - 1, sutun - 1]) - 48;
                        engel.Satir = satir;
                        engel.Sutun = sutun;
                        engelList.Add(engel);
                    }

                    form1.Controls.Add(allTiles[sutun, satir]);
                    allTiles[sutun, satir].BorderStyle = BorderStyle.FixedSingle;
                }
            }





            return form1;
        }

        public Form Problem1IzgaraBulutla(Form1 form1, PictureBox[,] allTiles, List<Hücre> hücreList)
        {
            foreach (var item in hücreList)
            {
                if (item.Satir == 0 || item.Satir == Math.Sqrt(hücreList.Count()) - 1 || item.Sutun == 0 || item.Sutun == Math.Sqrt(hücreList.Count()) - 1)//çerçeve ise
                {
                    allTiles[item.Satir, item.Sutun].BackColor = Color.Black;
                }
                else
                {
                    allTiles[item.Satir, item.Sutun].BackColor = Color.LightGray;
                }


            }




            return form1;
        }



        public Form Problem1EngelleriEkle(Form1 form1, PictureBox[,] allTiles, List<Engel> engelList)
        {
            foreach (var item in engelList)
            {
                if (item.EngelTipi == 1)
                {
                    allTiles[item.Satir, item.Sutun].BackColor = Color.LightBlue;
                }
                if (item.EngelTipi == 2)
                {
                    allTiles[item.Satir, item.Sutun].BackColor = Color.LightGreen;
                }
                if (item.EngelTipi == 3)
                {
                    allTiles[item.Satir, item.Sutun].BackColor = Color.LightPink;
                }

            }




            return form1;
        }

        public Form Problem1EkranTemizle(Form1 form1, PictureBox[,] allTiles, List<Engel> engelList)
        {
            for (int i = 0; i < Math.Sqrt(engelList.Count()) + 2; i++)
            {
                for (int j = 0; j < Math.Sqrt(engelList.Count()) + 2; j++)
                {
                    form1.Controls.Remove(allTiles[i, j]);
                    allTiles[i, j] = null;
                }
            }

            hücreList.Clear();
            engelList.Clear();
            

            return form1;
        }

        #endregion


        #region Problem2


        public Form Problem2IzgaraCiz(Form1 form1, int boyutX, int boyutY)
        {
            BoxSizeBul(boyutX, boyutY);
            mazeTiles = new PictureBox[boyutX, boyutY];

            for (int i = 0; i < boyutX; i++)
            {
                for (int j = 0; j < boyutY; j++)
                {
                    Hücre hücre = new Hücre();

                    mazeTiles[i, j] = new PictureBox();
                    int xPos = 150 + (j * boxSizex);
                    int yPos = 10 + (i * boxSizey);
                    mazeTiles[i, j].SetBounds(xPos, yPos, BoxSizex, BoxSizey);

                    hücre.Satir = i;
                    hücre.Sutun = j;
                    hücre.X = xPos;
                    hücre.Y = yPos;
                    hücre.Genislik = BoxSizex;
                    hücre.Yükseklik = BoxSizey;

                    tileList.Add(hücre);

                    if ((i == 0) || (i == boyutX - 1) || (j == 0) || (j == boyutY - 1))//çerçeve
                    {
                        mazeTiles[i, j].BackColor = Color.Black;
                    }
                    else//değil ise
                    {
                        mazeTiles[i, j].BackColor = Color.Gray;
                        mazeTiles[i, j].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    }

                    if ((i == 0 && j == 1) || (i == boyutX - 1 && j == boyutY - 2))//baslangic ve bitis
                    {
                        mazeTiles[i, j].BackColor = Color.LightBlue;
                    }

                    form1.Controls.Add(mazeTiles[i, j]);
                }
            }

            return form1;
        }

        public Form Problem2MazeOlustur(Form1 form1, PictureBox[,] mazeTiles, int boyutX, int boyutY)
        {
            //arkaplanı siyah yaptım
            foreach (PictureBox pb in mazeTiles)
            {
                pb.BackColor = Color.Black;
            }

            //başlangıç bitiş noktasını ayarladım
            mazeTiles[1, 0].BackColor = Color.LightBlue;
            mazeTiles[boyutX - 2, boyutY - 1].BackColor = Color.LightBlue;

            //ortadaki duvar listesini oluşturdum
            foreach (Hücre item in tileList)
            {

                if ((item.Satir == 0) || (item.Satir == boyutX - 1) || (item.Sutun == 0) || (item.Sutun == boyutY - 1))
                {
                    cerceveList.Add(item);
                }
            }

            wallList.AddRange(tileList.Except(cerceveList));

            //tüm kareleri blocked yaptım
            foreach (Hücre item in wallList)
            {
                item.Durum = "Blocked";
            }


            Random rnd = new Random();
            var baslangic = wallList.Find(x => x.Satir == 1 && x.Sutun == 1);
            baslangic.Durum = "Passage";

            var adayTiles = new List<Hücre>();

            // Get cell's wall candidates
            //baslangicin etrafındaki duvarlar bulunuyor
            adayTiles.AddRange(AdayTileBul(baslangic, false));
            while (adayTiles.Count > 0)
            {
                // Pick random cell from candidate collection
                //bulunanlardan biri rastgele seçildi
                var secilenHücre = adayTiles.ElementAt(rnd.Next(0, adayTiles.Count));//başlangıç

                // Get cell's path candidates
                //seçilen hücrenin yakınındaki pathler bulundu
                var pathHücre = AdayTileBul(secilenHücre, true);

                if (pathHücre.Count > 0)
                {
                    // Connect random path candidate with cell
                    //secilen hücre ile path hücre birleştirildi ve yol oluşturuldu
                    HücreleriBirlestir(pathHücre[rnd.Next(0, pathHücre.Count)], secilenHücre);
                }

                // Add this candidate cell's wall candidates to collection to process
                // seçilen hücrenin etrafındaki duvarlar bulundu
                var temp = AdayTileBul(secilenHücre, false);

                foreach (var item in temp)
                {
                    var kontrol = adayTiles.Find(x => x.Satir == item.Satir && x.Sutun == item.Sutun);
                    if (kontrol == null)
                    {
                        adayTiles.Add(item);
                        //tekrardan aday olabilecek listeye eklendi
                        //listede tekrar etme durumu olduğu için eklediğim kontrol
                    }
                }

                // Remove this candidate call from collection
                //seçilen hücre listeden çıkartıldı
                adayTiles.Remove(secilenHücre);
            }

            EkranaBas(form1, wallList);
            return form1;
        }

        private IList<Hücre> AdayTileBul(Hücre hücre, bool pathMi)
        {
            var adayPathTiles = new List<Hücre>();
            var adayWallTiles = new List<Hücre>();

            var kuzey = new Hücre { Satir = hücre.Satir - 2, Sutun = hücre.Sutun };
            var dogu = new Hücre { Satir = hücre.Satir, Sutun = hücre.Sutun + 2 };
            var guney = new Hücre { Satir = hücre.Satir + 2, Sutun = hücre.Sutun };
            var bati = new Hücre { Satir = hücre.Satir, Sutun = hücre.Sutun - 2 };


            //2 üst tarafındaki hücreler duvar mı path mi diye bakılır
            if (wallList.Find(x => x.Satir == kuzey.Satir && x.Sutun == kuzey.Sutun) != null &&//çerçevenin dışında mı diye bakıyor
                     (wallList.Find(x => x.Satir == kuzey.Satir && x.Sutun == kuzey.Sutun).Durum == "Passage"))//eğer içindeyse yol mu diye bakıyor
            {
                adayPathTiles.Add(kuzey);
            }
            else
            {
                if (wallList.Find(x => x.Satir == kuzey.Satir && x.Sutun == kuzey.Sutun) != null)
                {
                    adayWallTiles.Add(kuzey);
                }
            }



            if (wallList.Find(x => x.Satir == dogu.Satir && x.Sutun == dogu.Sutun) != null &&
                     (wallList.Find(x => x.Satir == dogu.Satir && x.Sutun == dogu.Sutun).Durum == "Passage"))
            {
                adayPathTiles.Add(dogu);
            }
            else
            {
                if (wallList.Find(x => x.Satir == dogu.Satir && x.Sutun == dogu.Sutun) != null)
                {
                    adayWallTiles.Add(dogu);
                }
            }


            if (wallList.Find(x => x.Satir == guney.Satir && x.Sutun == guney.Sutun) != null &&
                     (wallList.Find(x => x.Satir == guney.Satir && x.Sutun == guney.Sutun).Durum == "Passage"))
            {
                adayPathTiles.Add(guney);
            }
            else
            {
                if (wallList.Find(x => x.Satir == guney.Satir && x.Sutun == guney.Sutun) != null)
                {
                    adayWallTiles.Add(guney);
                }
            }


            if (wallList.Find(x => x.Satir == bati.Satir && x.Sutun == bati.Sutun) != null &&
                     (wallList.Find(x => x.Satir == bati.Satir && x.Sutun == bati.Sutun).Durum == "Passage"))
            {
                adayPathTiles.Add(bati);
            }
            else
            {
                if (wallList.Find(x => x.Satir == bati.Satir && x.Sutun == bati.Sutun) != null)
                {
                    adayWallTiles.Add(bati);
                }
            }

            //gönderilen bool'a göre duvarları veya pathlistleri döner
            if (pathMi)
            {
                return adayPathTiles;
            }
            else
            {
                return adayWallTiles;
            }
        }

        private void HücreleriBirlestir(Hücre hücre1, Hücre hücre2)
        {
            var satir = (hücre1.Satir + hücre2.Satir) / 2;
            var sutun = (hücre1.Sutun + hücre2.Sutun) / 2;

            wallList.Find(x => x.Satir == hücre2.Satir && x.Sutun == hücre2.Sutun).Durum = "Passage";//3. kutuyu passage yapıyor
            var sonuc1 = wallList.Find(x => x.Satir == hücre2.Satir && x.Sutun == hücre2.Sutun);

            wallList.Find(x => x.Satir == satir && x.Sutun == sutun).Durum = "Passage";//2. kutuyu passage yapıyor
            var sonuc2 = wallList.Find(x => x.Satir == satir && x.Sutun == sutun);

            //sonuc değişkenleri benim kontrol amaçlı yaptığım değişkenler

        }

        private void BoxSizeBul(int boyutX, int boyutY)
        {
            BoxSizex = 600 / boyutX;
            BoxSizey = 600 / boyutY;
        }

        private Form EkranaBas(Form1 form1, List<Hücre> wallList)
        {
            foreach (var item in wallList)
            {
                if (item.Durum == "Passage")
                {
                    var pb = mazeTiles[item.Satir, item.Sutun];
                    pb.BackColor = Color.Gray;//bulutlama işleminin yapıldığı yer
                }
            }

            return form1;
        }

        public Form Problem2EkranTemizle(Form1 form1, PictureBox[,] mazeTile, int boyutX, int boyutY)
        {
            for (int i = 0; i < boyutX; i++)
            {
                for (int j = 0; j < boyutY; j++)
                {
                    form1.Controls.Remove(mazeTile[i, j]);
                    mazeTile[i, j] = null;
                }
            }
            cerceveList.Clear();
            tileList.Clear();
            wallList.Clear();
            baslangicTile = null;



            return form1;
        }

        #endregion
    }
}

