using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGGD
{
    public class Pool : PictureBox
    {
        private static readonly Random _random = new Random();
        private readonly CancellationTokenSource _source = new CancellationTokenSource();

        public AutoResetEvent ResetEvent { get; set; }

        public Pool()
        {
            Initialize();
        }

        private void Initialize()
        {
            Image = RGGD.Properties.Resources.CleansingPool;
            Width = 128;
            Height = 128;

            Task.Run(CollisionWithHero, _source.Token);
        }

        public void SetRandomLocation()
        {
            Point newPosition;

            while (true)
            {
                newPosition = GeneratePosition();

                if (CheckSpawn(new Rectangle(newPosition, this.Size)))
                {
                    this.Location = newPosition;
                    break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _source.Cancel();

            base.Dispose(disposing);
        }


        private bool CheckSpawn(Rectangle bounds)
        {
            bool result = Locator.Data.Hero.Bounds.IntersectsWith(bounds);

            foreach (var pool in Locator.Data.Pools)
                if (!result)
                    result = pool.Bounds.IntersectsWith(bounds);
                else
                    return false;

            return true;
        }

        private async Task CollisionWithHero()
            {
            while (true)
            {
                await Task.Delay(100);

                if (!Locator.Data.Hero.Bounds.IntersectsWith(this.Bounds))
                    continue;

                this.Invoke(new Action(() => this.SetRandomLocation()));

                Locator.Data.PoolCount++;

                ResetEvent.Set();
            }
        }

        private static Point GeneratePosition() => new Point(_random.Next(128, 1024), _random.Next(128, 768));
    }
}