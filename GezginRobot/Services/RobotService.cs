using GezginRobot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GezginRobot.Services
{
    internal class RobotService
    {

        List<Hücre> komsularList = new List<Hücre>();
        public Hücre baslangicNoktasi = new Hücre();
        public Hücre bitisNoktasi = new Hücre();
        public string bitis;
        public string birÖnceki;
        public bool bittiMi;
        public List<Hücre> yolList = new List<Hücre>();


        List<Hücre> komsuList = new List<Hücre>();
        public List<Hücre> dogruYolList = new List<Hücre>();

        public int toplamHamle = 1;


        #region Problem1


        public Form RandomBaslangicNoktasiOlustur(Form1 form1, Robot robot, PictureBox[,] allTiles, List<Engel> engelList)
        {
            Random rnd = new Random();

            int i = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));
            int j = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));

            while (engelList.Find(x => x.Satir == i && x.Sutun == j).EngelTipi != 0)
            {
                i = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));
                j = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));
            }

            allTiles[i, j].BackColor = Color.DarkBlue;

            robot.SatirKordinat = i;
            robot.SutunKordinat = j;

            return form1;


        }

        public Form RandomBitisNoktasiOlustur(Form1 form1, PictureBox[,] allTiles, List<Engel> engelList)
        {
            Random rnd = new Random();

            int i = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));
            int j = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));

            while (engelList.Find(x => x.Satir == i && x.Sutun == j).EngelTipi != 0)
            {
                i = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));
                j = rnd.Next(1, (int)(Math.Sqrt(engelList.Count()) + 1));
            }

            allTiles[i, j].BackColor = Color.DarkRed;



            return form1;


        }

        public Form RobotHaritaKesfet(Form1 form1, Robot robot, PictureBox[,] allTiles, List<Hücre> hücreList, List<Engel> engelList)
        {
            baslangicNoktasi = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
            baslangicNoktasi.ZiyaretEdildiMi = true;

            KomsulariBul(robot, hücreList, allTiles, engelList);

            while (komsularList.Count > 0)
            {
                var sagaYolVarmi = komsularList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                var solaYolVarmi = komsularList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                var yukariYolVarmi = komsularList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                var asagiYolVarmi = komsularList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);

                //asağidaki sıralamadan ızgarayı sağ asaği sol yukari seklinde dönerek kesfediyor
                if (sagaYolVarmi != null)
                {
                    RobotGit2(robot, sagaYolVarmi);
                    RobotGidilenYoluAc(robot, allTiles);
                    KomsulariBul(robot, hücreList, allTiles, engelList);
                }
                else if (asagiYolVarmi != null)
                {
                    RobotGit2(robot, asagiYolVarmi);
                    RobotGidilenYoluAc(robot, allTiles);
                    KomsulariBul(robot, hücreList, allTiles, engelList);
                }
                else if (solaYolVarmi != null)
                {
                    RobotGit2(robot, solaYolVarmi);
                    RobotGidilenYoluAc(robot, allTiles);
                    KomsulariBul(robot, hücreList, allTiles, engelList);
                }
                else if (yukariYolVarmi != null)
                {
                    RobotGit2(robot, yukariYolVarmi);
                    RobotGidilenYoluAc(robot, allTiles);
                    KomsulariBul(robot, hücreList, allTiles, engelList);
                }
                else
                {
                    RobotGit2(robot, komsularList.First());
                    RobotGidilenYoluAc(robot, allTiles);
                    KomsulariBul(robot, hücreList, allTiles, engelList);
                }

                var templist = komsularList.Distinct().ToList();
                komsularList.Clear();
                komsularList.AddRange(templist);


            }

            BitisNeTarafta(baslangicNoktasi, bitisNoktasi);

            return form1;
        }



        public void KomsulariBul(Robot robot, List<Hücre> hücreList, PictureBox[,] allTiles, List<Engel> engelList)
        {
            var komsuFind = hücreList.FindAll(x => (x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1 && x.ZiyaretEdildiMi == false) ||
                                                  (x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat && x.ZiyaretEdildiMi == false) ||
                                                  (x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1 && x.ZiyaretEdildiMi == false) ||
                                                  (x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat && x.ZiyaretEdildiMi == false));

            KomsulariKontrolEt(komsuFind, allTiles, engelList);
        }

        public void KomsulariKontrolEt(List<Hücre> komsuFind, PictureBox[,] allTiles, List<Engel> engelList)
        {

            foreach (var komsu in komsuFind)
            {
                if (allTiles[komsu.Satir, komsu.Sutun].BackColor == Color.DarkRed)//bitis noktasi
                {
                    //eğerki bitis noktası ise set ediyor
                    bitisNoktasi.Satir = komsu.Satir;
                    bitisNoktasi.Sutun = komsu.Sutun;
                    bitisNoktasi.X = komsu.X;
                    bitisNoktasi.Y = komsu.Y;
                    bitisNoktasi.Genislik = komsu.Genislik;
                    bitisNoktasi.Yükseklik = komsu.Yükseklik;
                    bitisNoktasi.ZiyaretEdildiMi = true;

                }
                if (allTiles[komsu.Satir, komsu.Sutun].BackColor == Color.LightGray)
                {

                    komsularList.Add(komsu);

                }
            }

        }

        public void RobotGit2(Robot robot, Hücre gidilecekHücre)
        {
            robot.SatirKordinat = gidilecekHücre.Satir;
            robot.SutunKordinat = gidilecekHücre.Sutun;
            gidilecekHücre.ZiyaretEdildiMi = true;

            var silinecek = komsularList.Find(x => x.Satir == gidilecekHücre.Satir && x.Sutun == gidilecekHücre.Sutun);
            komsularList.Remove(silinecek);

        }

        public void RobotGidilenYoluAc(Robot robot, PictureBox[,] allTiles)
        {
            allTiles[robot.SatirKordinat, robot.SutunKordinat].BackColor = Color.White;//bulutun kaldırıldığı yer
        }



        public Form RobotEnKısaYoluGit(Form1 form1, Robot robot, PictureBox[,] allTiles, List<Hücre> hücreList, List<Engel> engelList, Hücre baslangicNoktasi, Hücre bitisNoktasi)
        {
            robot.SatirKordinat = baslangicNoktasi.Satir;
            robot.SutunKordinat = baslangicNoktasi.Sutun;

            bittiMi = false;


            var deneme = HesapYap(baslangicNoktasi, bitisNoktasi);

            while (bittiMi != true)
            {
                //sağa ve sola daha uzak ise
                if (deneme == "Sağ-Sol" || deneme == "Esit")
                {
                    if (bitis == "Sol Üst")
                    {
                        //sol
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.DarkRed && robot.SutunKordinat > bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //üst
                        while (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat > bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }

                    }
                    else if (bitis == "Sağ Üst")
                    {
                        //sağ
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.DarkRed && robot.SutunKordinat < bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //üst
                        while (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat > bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Sol Alt")
                    {
                        //sol
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.DarkRed && robot.SutunKordinat > bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //alt
                        while (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat < bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }

                    }
                    else if (bitis == "Sağ Alt")
                    {
                        //sağ
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.DarkRed && robot.SutunKordinat < bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }

                        //alt
                        while (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat < bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Üst")
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 2].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 2].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }
                        }
                        //üst
                        while (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat > bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Alt")
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 2].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 2].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //alt
                        while (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat < bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Sağ")
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat - 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 2, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat + 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 2, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //sağ
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.DarkRed && robot.SutunKordinat < bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else/*(bitis == "Sol")*/
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}


                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }
                            else if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat - 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 2, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }
                            else if (allTiles[robot.SatirKordinat + 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 2, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //sol
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.DarkRed && robot.SutunKordinat > bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                }
                //üst ve alta daha uzak ise

                else /*if(HesapYap(baslangicNoktasi, bitisNoktasi) == "Ust-Alt" || HesapYap(baslangicNoktasi, bitisNoktasi) == "Esit")*/
                {
                    if (bitis == "Sol Üst")
                    {
                        //üst
                        while (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat > bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //sol
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.DarkRed && robot.SutunKordinat > bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }


                    }
                    else if (bitis == "Sağ Üst")
                    {
                        //üst
                        while (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat > bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //sağ
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.DarkRed && robot.SutunKordinat < bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Sol Alt")
                    {
                        //alt
                        while (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat < bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //sol
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.DarkRed && robot.SutunKordinat > bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }


                    }
                    else if (bitis == "Sağ Alt")
                    {
                        //alt
                        while (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat < bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                        //sağ
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.DarkRed && robot.SutunKordinat < bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Üst")
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 2].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 2].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }
                        }
                        //üst
                        while (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat > bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Alt")
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 2].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 2].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 2].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 2);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //alt
                        while (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.DarkRed && robot.SatirKordinat < bitisNoktasi.Satir)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else if (bitis == "Sağ")
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat - 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 2, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }

                            else if (allTiles[robot.SatirKordinat + 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 2, robot.SutunKordinat + 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //sağ
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.DarkRed && robot.SutunKordinat < bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }
                    else/*(bitis == "Sol")*/
                    {
                        //1x1 ve 2x2 kare
                        if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.DarkRed && (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor != Color.LightPink || allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 1].BackColor != Color.LightPink))
                        {
                            if (allTiles[robot.SatirKordinat - 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }
                            else if (allTiles[robot.SatirKordinat + 1, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 1, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                //if (yolList.Count > 1)
                                //{
                                //    yolList.Remove(yolList.Last());
                                //}

                                RobotGit2(robot, temp);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //3x3 kare
                        else if (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.White && allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor != Color.DarkRed && allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.LightPink)
                        {
                            if (allTiles[robot.SatirKordinat - 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat - 2, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat - 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }
                            else if (allTiles[robot.SatirKordinat + 2, robot.SutunKordinat].BackColor == Color.White && allTiles[robot.SatirKordinat + 2, robot.SutunKordinat - 1].BackColor == Color.White)
                            {
                                var temp2 = hücreList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                                var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat + 2 && x.Sutun == robot.SutunKordinat);
                                RobotGit2(robot, temp);
                                yolList.Add(temp2);
                                yolList.Add(temp);

                                var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                                BitisNeTarafta(robotNerede, bitisNoktasi);

                            }


                        }
                        //sol
                        while (allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.White || allTiles[robot.SatirKordinat, robot.SutunKordinat - 1].BackColor == Color.DarkRed && robot.SutunKordinat > bitisNoktasi.Sutun)
                        {
                            var temp = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                            RobotGit2(robot, temp);
                            yolList.Add(temp);

                            var robotNerede = hücreList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);
                            BitisNeTarafta(robotNerede, bitisNoktasi);
                            break;
                        }
                    }

                }
            }

            if (bittiMi)
            {
                yolList.Remove(yolList.Last());
                foreach (var item in yolList)
                {

                    allTiles[item.Satir, item.Sutun].BackColor = Color.Chartreuse;
                }
            }


            return form1;
        }

        public string BitisNeTarafta(Hücre baslangicNoktasi, Hücre bitisNoktasi)
        {
            //bitip bitmediği kontrol ediliyor
            if (baslangicNoktasi.Satir == bitisNoktasi.Satir && baslangicNoktasi.Sutun == bitisNoktasi.Sutun)
            {
                bittiMi = true;
            }
            //koordinatlar kontrol edilerek bitişin ne tarafta kaldığı setleniyor
            //bitis üstte
            if (baslangicNoktasi.Satir > bitisNoktasi.Satir)
            {
                //bitis solda ve üstte
                if (baslangicNoktasi.Sutun > bitisNoktasi.Sutun)
                {
                    bitis = "Sol Üst";

                    return bitis;
                }
                //bitis sağda ve üstte
                else if (baslangicNoktasi.Sutun < bitisNoktasi.Sutun)
                {
                    bitis = "Sağ Üst";

                    return bitis;
                }
                //bitis sadece üstte
                else
                {
                    bitis = "Üst";

                    return bitis;
                }
            }
            //bitis altta
            else if (baslangicNoktasi.Satir < bitisNoktasi.Satir)
            {
                //bitis solda ve altta
                if (baslangicNoktasi.Sutun > bitisNoktasi.Sutun)
                {
                    bitis = "Sol Alt";

                    return bitis;
                }
                //bitis sağda ve altta
                else if (baslangicNoktasi.Sutun < bitisNoktasi.Sutun)
                {
                    bitis = "Sağ Alt";

                    return bitis;
                }
                //bitis sadece altta
                else
                {
                    bitis = "Alt";

                    return bitis;
                }
            }
            //ikisi de aynı satırda
            else
            {
                //bitis solda
                if (baslangicNoktasi.Sutun > bitisNoktasi.Sutun)
                {
                    bitis = "Sol";

                    return bitis;
                }
                //bitis sağda
                else/*(baslangicNoktasi.Sutun < bitisNoktasi.Sutun)*/
                {
                    bitis = "Sağ";

                    return bitis;
                }

            }
        }

        public string HesapYap(Hücre baslangicNoktasi, Hücre bitisNoktasi)
        {
            //yukarı aşağı ve sağa sola olan uzaklığa göre ne tarafa daha  çok gitmesi gerektiğini buluyor
            int xUzaklik;
            int yUzaklik;

            if (baslangicNoktasi.Satir > bitisNoktasi.Satir)
            {
                yUzaklik = baslangicNoktasi.Satir - bitisNoktasi.Satir;
            }
            else if (baslangicNoktasi.Satir < bitisNoktasi.Satir)
            {
                yUzaklik = bitisNoktasi.Satir - baslangicNoktasi.Satir;
            }
            else
            {
                yUzaklik = 0;
            }

            if (baslangicNoktasi.Sutun > bitisNoktasi.Sutun)
            {
                xUzaklik = baslangicNoktasi.Sutun - bitisNoktasi.Sutun;
            }
            else if (baslangicNoktasi.Sutun < bitisNoktasi.Sutun)
            {
                xUzaklik = bitisNoktasi.Sutun - baslangicNoktasi.Sutun;
            }
            else
            {
                xUzaklik = 0;
            }

            if (xUzaklik > yUzaklik)
            {
                return "Sağ-Sol";
            }
            else if (yUzaklik > xUzaklik)
            {
                return "Üst-Alt";
            }
            else
            {
                return "Esit";
            }
        }

        #endregion


        #region Problem2    

        public Form Problem2MazeCoz(Form1 form1, Robot robot, List<Hücre> wallList, PictureBox[,] mazeTiles)
        {
            mazeTiles[robot.SatirKordinat, robot.SutunKordinat].BackColor = Color.White;//robotun baslangıcın bulutunu açtığı yer

            var listFind = wallList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat);//robotun wallListte olduğu yer
            listFind.ZiyaretEdildiMi = true;
            dogruYolList.Add(listFind);//doğru yol listesine eklenen yer



            KomsuBul(robot, wallList, mazeTiles);

            while (komsuList.Count > 0)
            {
                var sagaYolVarmi = komsuList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1);
                var solaYolVarmi = komsuList.Find(x => x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1);
                var yukariYolVarmi = komsuList.Find(x => x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat);
                var asagiYolVarmi = komsuList.Find(x => x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat);
                //yol var mı diye tekrar kontol ediliyor
                //eğer var ise aşağıdaki sıraya göre gidiliyor


                if (sagaYolVarmi != null)
                {
                    RobotGit(robot, sagaYolVarmi);
                    KomsuBul(robot, wallList, mazeTiles);
                    //gidilen hücrenin tekrar komşuları bulunuyor
                    dogruYolList.Add(sagaYolVarmi);
                    //doğru yol listesine ekleniyor
                    toplamHamle++;
                }
                else if (asagiYolVarmi != null)
                {
                    RobotGit(robot, asagiYolVarmi);
                    KomsuBul(robot, wallList, mazeTiles);
                    dogruYolList.Add(asagiYolVarmi);
                    toplamHamle++;
                }
                else if (solaYolVarmi != null)
                {
                    RobotGit(robot, solaYolVarmi);
                    KomsuBul(robot, wallList, mazeTiles);
                    dogruYolList.Add(solaYolVarmi);
                    toplamHamle++;
                }
                else if (yukariYolVarmi != null)
                {
                    RobotGit(robot, yukariYolVarmi);
                    KomsuBul(robot, wallList, mazeTiles);
                    dogruYolList.Add(yukariYolVarmi);
                    toplamHamle++;
                }
                else
                {
                    //cıkmaz yol old anladı

                    var robotBurayaGitti = komsuList.Last();
                    RobotGit(robot, komsuList.Last());
                    //robot son bulunan komşuya gidiyor
                    
                    var index = IndexBul(robot, dogruYolList);
                    //robotun son gittiği hücrenin doğru yol listesine doğru sırada eklenebilmesi için robotun sağında veya solunda,üstünde,altında kalan doğru yolun
                    //indexi bulunuyor
                    

                    for(int i = dogruYolList.Count - 1; i > index ; i--)
                    {
                        dogruYolList.RemoveAt(i);
                        //yanlış olan yol listenin sonundan çıkartılıyor doğru indexe gelince duruluyor
                    }

                    dogruYolList.Add(robotBurayaGitti);
                    //robotun son gittiği yer listeye ekleniyor

                    KomsuBul(robot, wallList, mazeTiles);

                    toplamHamle++;

                }

                mazeTiles[robot.SatirKordinat, robot.SutunKordinat].BackColor = Color.White;//robotun gittiği yol


                //bitiş kontrolü
                if (mazeTiles[robot.SatirKordinat, robot.SutunKordinat + 1].BackColor == Color.LightBlue)
                {
                    MessageBox.Show("Labirent Çözüldü", "Tebrikler", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var item in dogruYolList)
                    {
                        mazeTiles[item.Satir, item.Sutun].BackColor = Color.HotPink;

                    }

                    break;
                }



            }


            
            return form1;
        }

        public int IndexBul(Robot robot, List<Hücre> dogruYolList)
        {
            var temp = dogruYolList.FindIndex(x => (x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat) || (x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat) ||
                                                   (x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1) || (x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1));
            //robotun 4 tarafındaki hücrelerin listedeki indexi bulunuyor çünkü bunlardan biriyle bağlantılı olmak zorunda
            return temp;
        }

        public void KomsuBul(Robot robot, List<Hücre> wallList, PictureBox[,] mazeTiles)
        {
            var komsuFind = wallList.FindAll(x => (x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat + 1 && x.ZiyaretEdildiMi == false) ||
                                                  (x.Satir == robot.SatirKordinat + 1 && x.Sutun == robot.SutunKordinat && x.ZiyaretEdildiMi == false) ||
                                                  (x.Satir == robot.SatirKordinat && x.Sutun == robot.SutunKordinat - 1 && x.ZiyaretEdildiMi == false) ||
                                                  (x.Satir == robot.SatirKordinat - 1 && x.Sutun == robot.SutunKordinat && x.ZiyaretEdildiMi == false));
            //sağı solu üstü altı kontol ediliyor gidildi mi diye gidilmediyse ekleniyor
            KomsuKontrol(komsuFind, mazeTiles);
        }

        public void KomsuKontrol(List<Hücre> komsuFind, PictureBox[,] mazeTiles)
        {
            foreach (var komsu in komsuFind)
            {
                if (mazeTiles[komsu.Satir, komsu.Sutun].BackColor == Color.Gray)
                {

                    komsuList.Add(komsu);
                    //eğer bulutlu ise yani duvar değilse komşu listesine ekleniyor

                }
            }

        }

        public void RobotGit(Robot robot, Hücre gidilecekHücre)
        {
            robot.SatirKordinat = gidilecekHücre.Satir;
            robot.SutunKordinat = gidilecekHücre.Sutun;
            gidilecekHücre.ZiyaretEdildiMi = true;
            //robotun koordinatları setleniyor

            var silinecek = komsuList.Find(x => x.Satir == gidilecekHücre.Satir && x.Sutun == gidilecekHücre.Sutun);
            komsuList.Remove(silinecek);
            //gidildiği için listeden siliniyor



        }


        #endregion


        

    }
}
