using System.Threading;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing;

namespace RGGD
{
    public partial class Form1 : Form
    {
        private static readonly Random _random = new Random();

        private readonly PoolPoolCounters _poolOfCounters;
        private readonly AutoResetEvent _creatorPoolsResetEvent = new AutoResetEvent(true);
        private readonly int _maxPools = 10;
        private readonly PictureBox _timerCounter;
        private readonly PictureBox _timerCounter2;

        private bool _isGreenDemon = true;

        public Form1()
        {
            InitializeComponent();
            _poolOfCounters = new PoolPoolCounters(this.Controls);
            _timerCounter = TimerCounter;
            _timerCounter2 = TimerCounter2;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Locator.Data.Hero = Soul;
            InitPoolCountUpdate();
            InitDemonSwitcher();
            InitPoolGenerator();
            InitCountdown();
        }

        // events

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.W) && Soul.Top > 132)
            {
                Soul.Top -= 15;
            };
            if (Keyboard.IsKeyDown(Key.A) && Soul.Left > 140)
            {
                Soul.Left -= 15;
            };
            if (Keyboard.IsKeyDown(Key.S) && Soul.Bottom < 882)
            {
                Soul.Top += 15;
            };
            if (Keyboard.IsKeyDown(Key.D) && Soul.Right < 1136)
            {
                Soul.Left += 15;
            };

            if ((Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.D)) && !_isGreenDemon)
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        {
                            GameOver.Visible = true;
                            GameOver.BringToFront();
                            await Task.Delay(1000);
                        }
                        Application.Exit();

                    }
                });
            }
        }

        // private

        private void InitPoolCountUpdate()
        {
            Locator.Data.PoolCountChangeEvent += value =>
            {
                string valueStr = value.ToString();
                Point startPosition = new Point(16, 16);

                for (int i = 0; i <valueStr.Length ; i++)
                {
                    this.Invoke(new Action(() => 
                    {
                        var poolCounter = _poolOfCounters[i];

                        if (poolCounter == null)
                        {
                            poolCounter = new PictureBox()
                            {
                                Width = 32,
                                Height = 32
                            };

                            _poolOfCounters.Add(poolCounter);
                        }

                        poolCounter.BringToFront();

                        poolCounter.Location = new Point(startPosition.X + 32 * i, startPosition.Y);
                        poolCounter.Image = Locator.Data.ScoreImages[int.Parse($"{valueStr[i]}")];
                    }));
                }
            };

            Locator.Data.PoolCount = 0;
        }

        private void InitDemonSwitcher()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(_random.Next(1000, 15000));

                    // swap on reacted demon image
                    if (_isGreenDemon)
                    {
                        Demon.Invoke(new Action(() => Demon.Image = RGGD.Properties.Resources._REDACTED_));

                        await Task.Delay(500);
                    }

                    // set demon image
                    Demon.Invoke(new Action(() => Demon.Image = _isGreenDemon ? RGGD.Properties.Resources.RedDemon : RGGD.Properties.Resources.GreenDemon1));
                    Soul.Invoke(new Action(() => Soul.Image = _isGreenDemon ? RGGD.Properties.Resources.TrappedRedSoul : RGGD.Properties.Resources.TrappedGreenSoul1));
                    _isGreenDemon = !_isGreenDemon;
                }
            });
        }

        private void InitCountdown()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    Locator.Data.TimerCount--;

                    if (Locator.Data.TimerCount == -1)
                    {
                        Locator.Data.TimerCount2--;
                        Locator.Data.TimerCount = 9;
                    }

                    _timerCounter2.Invoke(new Action(() => _timerCounter2.Image = Locator.Data.ScoreImages[Locator.Data.TimerCount2]));
                    _timerCounter.Invoke(new Action(() => _timerCounter.Image = Locator.Data.ScoreImages[Locator.Data.TimerCount%10]));

                    if (Locator.Data.TimerCount2 == 0 && Locator.Data.TimerCount == 0)
                    {
                        {
                            GameOver.Visible = true;
                            GameOver.BringToFront();
                            await Task.Delay(1000);
                        }
                        Application.Exit();
                    }
                }
            });
        }

        private void InitPoolGenerator()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _creatorPoolsResetEvent.WaitOne();

                    if (Locator.Data.PoolCount >= _maxPools)
                        break;

                    var pool = new Pool();

                    pool.ResetEvent = _creatorPoolsResetEvent;

                    pool.SetRandomLocation();

                    Locator.Data.Pools.Add(pool);
                    
                    this.Invoke(new Action<PictureBox>((toAdd) => this.Controls.Add(toAdd)), pool);
                }
            });
        }
    }
}