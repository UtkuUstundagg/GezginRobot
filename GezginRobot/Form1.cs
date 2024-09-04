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

        IzgaraService ýzgaraService = new IzgaraService();
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

            ýzgaraService.Problem1IzgaraCiz(this, okunanDosya);
            ýzgaraService.Problem1IzgaraBulutla(this, ýzgaraService.allTiles,ýzgaraService.hücreList);

        }

        private void BTNkesfet_Click(object sender, EventArgs e)
        {
            ýzgaraService.Problem1EngelleriEkle(this, ýzgaraService.allTiles, ýzgaraService.engelList);

            robotService.RandomBaslangicNoktasiOlustur(this, robot, ýzgaraService.allTiles, ýzgaraService.engelList);
            robotService.RandomBitisNoktasiOlustur(this, ýzgaraService.allTiles, ýzgaraService.engelList);
            robotService.RobotHaritaKesfet(this, robot, ýzgaraService.allTiles, ýzgaraService.hücreList, ýzgaraService.engelList);
        }

        private void BTNcoz_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            robotService.RobotEnKýsaYoluGit(this, robot, ýzgaraService.allTiles, ýzgaraService.hücreList, ýzgaraService.engelList, robotService.baslangicNoktasi, robotService.bitisNoktasi);
            TBadým1.Text = robotService.yolList.Count.ToString();

            stopwatch.Stop();

            textBox1.Text = stopwatch.Elapsed.Seconds.ToString();

        }

        private void BTNtemizle_Click(object sender, EventArgs e)
        {
            TBUrl.Text = " ";
            TBadým1.Text = " ";

            ýzgaraService.Problem1EkranTemizle(this, ýzgaraService.allTiles, ýzgaraService.engelList);
            robotService.yolList.Clear();
        }

        #endregion

        #region Problem2


        private void BTNolustur_Click(object sender, EventArgs e)
        {
            boyutX = Convert.ToInt32(TBX2.Text);
            boyutY = Convert.ToInt32(TBY2.Text);

            
            ýzgaraService.Problem2IzgaraCiz(this, boyutX, boyutY);
            ýzgaraService.Problem2MazeOlustur(this, ýzgaraService.mazeTiles, boyutX, boyutY);


        }

        private void BTNyenile2_Click(object sender, EventArgs e)
        {
            ýzgaraService.Problem2EkranTemizle(this, ýzgaraService.mazeTiles, boyutX, boyutY);
            ýzgaraService.Problem2IzgaraCiz(this, boyutX, boyutY);
            ýzgaraService.Problem2MazeOlustur(this, ýzgaraService.mazeTiles, boyutX, boyutY);
            TBAdým.Text = " ";
        }

        private void BTNbaslat2_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            Robot robot = new Robot();

            robot.SatirKordinat = 1;
            robot.SutunKordinat = 1;

            robotService.Problem2MazeCoz(this, robot, ýzgaraService.wallList, ýzgaraService.mazeTiles);
            TBAdým.Text = robotService.dogruYolList.Count.ToString();

            stopwatch.Stop();
            textBox1.Text = stopwatch.Elapsed.Seconds.ToString();

            TBtopHamle.Text = robotService.toplamHamle.ToString();

        }


        #endregion


    }
}