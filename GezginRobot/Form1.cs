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

        IzgaraService ızgaraService = new IzgaraService();
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

            ızgaraService.Problem1IzgaraCiz(this, okunanDosya);
            ızgaraService.Problem1IzgaraBulutla(this, ızgaraService.allTiles,ızgaraService.hücreList);

        }

        private void BTNkesfet_Click(object sender, EventArgs e)
        {
            ızgaraService.Problem1EngelleriEkle(this, ızgaraService.allTiles, ızgaraService.engelList);

            robotService.RandomBaslangicNoktasiOlustur(this, robot, ızgaraService.allTiles, ızgaraService.engelList);
            robotService.RandomBitisNoktasiOlustur(this, ızgaraService.allTiles, ızgaraService.engelList);
            robotService.RobotHaritaKesfet(this, robot, ızgaraService.allTiles, ızgaraService.hücreList, ızgaraService.engelList);
        }

        private void BTNcoz_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            robotService.RobotEnKısaYoluGit(this, robot, ızgaraService.allTiles, ızgaraService.hücreList, ızgaraService.engelList, robotService.baslangicNoktasi, robotService.bitisNoktasi);
            TBadım1.Text = robotService.yolList.Count.ToString();

            stopwatch.Stop();

            textBox1.Text = stopwatch.Elapsed.Seconds.ToString();

        }

        private void BTNtemizle_Click(object sender, EventArgs e)
        {
            TBUrl.Text = " ";
            TBadım1.Text = " ";

            ızgaraService.Problem1EkranTemizle(this, ızgaraService.allTiles, ızgaraService.engelList);
            robotService.yolList.Clear();
        }

        #endregion

        #region Problem2


        private void BTNolustur_Click(object sender, EventArgs e)
        {
            boyutX = Convert.ToInt32(TBX2.Text);
            boyutY = Convert.ToInt32(TBY2.Text);

            
            ızgaraService.Problem2IzgaraCiz(this, boyutX, boyutY);
            ızgaraService.Problem2MazeOlustur(this, ızgaraService.mazeTiles, boyutX, boyutY);


        }

        private void BTNyenile2_Click(object sender, EventArgs e)
        {
            ızgaraService.Problem2EkranTemizle(this, ızgaraService.mazeTiles, boyutX, boyutY);
            ızgaraService.Problem2IzgaraCiz(this, boyutX, boyutY);
            ızgaraService.Problem2MazeOlustur(this, ızgaraService.mazeTiles, boyutX, boyutY);
            TBAdım.Text = " ";
        }

        private void BTNbaslat2_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            Robot robot = new Robot();

            robot.SatirKordinat = 1;
            robot.SutunKordinat = 1;

            robotService.Problem2MazeCoz(this, robot, ızgaraService.wallList, ızgaraService.mazeTiles);
            TBAdım.Text = robotService.dogruYolList.Count.ToString();

            stopwatch.Stop();
            textBox1.Text = stopwatch.Elapsed.Seconds.ToString();

            TBtopHamle.Text = robotService.toplamHamle.ToString();

        }


        #endregion


    }
}