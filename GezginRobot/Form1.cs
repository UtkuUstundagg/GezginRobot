using GezginRobot.Classes;
using GezginRobot.Services;
using System.Diagnostics;
using System.Net;

namespace GezginRobot
{
    public partial class Form1 : Form
    {
        public int boyutX;
        public int boyutY;


        public Form1()
        {
            InitializeComponent();
        }

        Stopwatch stopwatch = new Stopwatch();

        IzgaraService �zgaraService = new IzgaraService();
        RobotService robotService = new RobotService();

        Robot robot = new Robot();

        #region Problem1


        private void BTNurl1_Click(object sender, EventArgs e)
        {
            TBUrl.Text = "http://bilgisayar.kocaeli.edu.tr/prolab2/url1.txt";
        }

        private void BTNurl2_Click(object sender, EventArgs e)
        {
            TBUrl.Text = "http://bilgisayar.kocaeli.edu.tr/prolab2/url2.txt";
        }


        private void BTNbulutla_Click(object sender, EventArgs e)
        {

            WebClient wc = new WebClient();
            string okunanDosya = wc.DownloadString(TBUrl.Text);

            �zgaraService.Problem1IzgaraCiz(this, okunanDosya);
            �zgaraService.Problem1IzgaraBulutla(this, �zgaraService.allTiles,�zgaraService.h�creList);

        }

        private void BTNkesfet_Click(object sender, EventArgs e)
        {
            �zgaraService.Problem1EngelleriEkle(this, �zgaraService.allTiles, �zgaraService.engelList);

            robotService.RandomBaslangicNoktasiOlustur(this, robot, �zgaraService.allTiles, �zgaraService.engelList);
            robotService.RandomBitisNoktasiOlustur(this, �zgaraService.allTiles, �zgaraService.engelList);
            robotService.RobotHaritaKesfet(this, robot, �zgaraService.allTiles, �zgaraService.h�creList, �zgaraService.engelList);
        }

        private void BTNcoz_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            robotService.RobotEnK�saYoluGit(this, robot, �zgaraService.allTiles, �zgaraService.h�creList, �zgaraService.engelList, robotService.baslangicNoktasi, robotService.bitisNoktasi);
            TBad�m1.Text = robotService.yolList.Count.ToString();

            stopwatch.Stop();

            textBox1.Text = stopwatch.Elapsed.Seconds.ToString();

        }

        private void BTNtemizle_Click(object sender, EventArgs e)
        {
            TBUrl.Text = " ";
            TBad�m1.Text = " ";

            �zgaraService.Problem1EkranTemizle(this, �zgaraService.allTiles, �zgaraService.engelList);
            robotService.yolList.Clear();
        }

        #endregion

        #region Problem2


        private void BTNolustur_Click(object sender, EventArgs e)
        {
            boyutX = Convert.ToInt32(TBX2.Text);
            boyutY = Convert.ToInt32(TBY2.Text);

            
            �zgaraService.Problem2IzgaraCiz(this, boyutX, boyutY);
            �zgaraService.Problem2MazeOlustur(this, �zgaraService.mazeTiles, boyutX, boyutY);


        }

        private void BTNyenile2_Click(object sender, EventArgs e)
        {
            �zgaraService.Problem2EkranTemizle(this, �zgaraService.mazeTiles, boyutX, boyutY);
            �zgaraService.Problem2IzgaraCiz(this, boyutX, boyutY);
            �zgaraService.Problem2MazeOlustur(this, �zgaraService.mazeTiles, boyutX, boyutY);
            TBAd�m.Text = " ";
        }

        private void BTNbaslat2_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            Robot robot = new Robot();

            robot.SatirKordinat = 1;
            robot.SutunKordinat = 1;

            robotService.Problem2MazeCoz(this, robot, �zgaraService.wallList, �zgaraService.mazeTiles);
            TBAd�m.Text = robotService.dogruYolList.Count.ToString();

            stopwatch.Stop();
            textBox1.Text = stopwatch.Elapsed.Seconds.ToString();

            TBtopHamle.Text = robotService.toplamHamle.ToString();

        }


        #endregion


    }
}