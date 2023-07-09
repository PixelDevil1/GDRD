using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace RGGD
{
    public static class Locator
    {
        public static DataContainer Data { get; } = new DataContainer();
    }

    public class DataContainer
    {
        private int _poolCount = 0;

        public DataContainer()
        {
            List<Image> images = new List<Image>();

            images.Add(RGGD.Properties.Resources._0);
            images.Add(RGGD.Properties.Resources._1);
            images.Add(RGGD.Properties.Resources._2);
            images.Add(RGGD.Properties.Resources._3);
            images.Add(RGGD.Properties.Resources._4);
            images.Add(RGGD.Properties.Resources._5);
            images.Add(RGGD.Properties.Resources._6);
            images.Add(RGGD.Properties.Resources._7);
            images.Add(RGGD.Properties.Resources._8);
            images.Add(RGGD.Properties.Resources._9);

            ScoreImages = images;
            TimerCount = 10;
            TimerCount2 = 2;
        }

        public Action<int> PoolCountChangeEvent { get; set; }
        public IReadOnlyList<Image> ScoreImages { get; }
        public PictureBox Hero { get; set; }
        public List<Pool> Pools { get; } = new List<Pool>();
        public int PoolCount
        {
            get => _poolCount;
            set
            {
                _poolCount = value;
                PoolCountChangeEvent?.Invoke(value);
            }
        }
        public int TimerCount { get; set; }
        public int TimerCount2 { get; set; }
    }
}