using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GezginRobot.Classes
{
    internal class Hücre
    {
        private int satir;
        private int sutun;
        private int x;
        private int y;
        private int genislik;
        private int yükseklik;


        private bool ziyaretEdildiMi;
        private string durum;


        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Genislik { get => genislik; set => genislik = value; }
        public int Yükseklik { get => yükseklik; set => yükseklik = value; }
        public bool ZiyaretEdildiMi { get => ziyaretEdildiMi; set => ziyaretEdildiMi = value; }
        public string Durum { get => durum; set => durum = value; }
        public int Satir { get => satir; set => satir = value; }
        public int Sutun { get => sutun; set => sutun = value; }

    }
}
