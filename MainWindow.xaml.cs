using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpaceGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer = new();
        private readonly WriteableBitmap _bitmap;
        private readonly Random _rng = new();
        private int _f;

        public MainWindow()
        {
            InitializeComponent();
            _bitmap = new((int)image.Width, (int)image.Height, 96, 100, PixelFormats.Bgr32, null);
            image.Source = _bitmap;
            _timer.Interval = TimeSpan.FromSeconds(0.00001);
            _timer.Tick += Tick;
            _timer.Start();
        }
        

        private void Tick(object? sender, EventArgs e)
        {
            try
            {
                _bitmap.Lock();
                for (int i = 0; i < 1000; i++)
                {
                    var x = _rng.Next(_bitmap.PixelWidth);
                    var y = _rng.Next(_bitmap.PixelHeight);
                    var ptr = _bitmap.BackBuffer + x * 4 + _bitmap.BackBufferStride * y;
                    var (r, g, b) = (_f, _f + 30, _f + 60);
                    unsafe
                    {
                        *((int*)ptr) = (r << 16) | (g << 8) | b;
                    }

                    _bitmap.AddDirtyRect(new(x, y, 1, 1));
                }
            }
            finally
            {
                _bitmap.Unlock();
            }
            _f += 20;
        }
    }
}
