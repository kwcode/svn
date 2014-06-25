using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;

namespace WPF.CustomControl
{
    /// <summary>
    /// Interaction logic for HLProgressBar.xaml
    /// </summary>
    public partial class HLProgressBar : UserControl
    {
        private List<Ellipse> _elliList = new List<Ellipse>();
        private  Timer _dtimer = null;
        private double defaultSize = 20;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public HLProgressBar()
        {
            InitializeComponent();
            SizeChanged += new SizeChangedEventHandler(HLProgressBar_SizeChanged);
            Visibility = System.Windows.Visibility.Collapsed;
        }

        void HLProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Init();
        }

        #region Private Methods

        private void Init()
        {
            layContainer.Width = this.ActualWidth / 5;
            layContainer.Height = this.ActualHeight / 5;
            _SmallCircualWidth = layContainer.Width / 5;
            _SmallCircualHeight = layContainer.Height / 5;

            if (_SmallCircualWidth > _SmallCircualHeight)
                _SmallCircualWidth = _SmallCircualHeight;
            else
                _SmallCircualHeight = _SmallCircualWidth;

            layContainer.Children.Clear();

            double angle = 360 / SmallCircularCount;
            Ellipse elli = null;
            double d = 0;
            for (int k = 0; k < SmallCircularCount; k++)
            {
                elli = new Ellipse();
                elli.Name = "Elli" + k.ToString();
                elli.Width = SmallCircualWidth;
                elli.Height = SmallCircualHeight;

                elli.Stroke = SmallCircularBrush;
                elli.Fill = SmallCircularBrush;

                if (!IsHalfCircular)
                    d = (k + 1.0 + SmallCircularCount - 1) / SmallCircularCount;
                elli.Opacity = d > 1 ? d - 1 : d;

                elli.RenderTransform = new RotateTransform(45 + k * angle, SmallCenterX, SmallCenterY);

                _elliList.Add(elli);

                layContainer.Children.Add(elli);
            }

            if (IsHalfCircular)
            {
                int half = _elliList.Count / 2;
                for (int k = 0; k < half; k++)
                {
                    elli = _elliList[k];
                    d = (k * 2 + 1.0 + SmallCircularCount - 1) / SmallCircularCount;
                    elli.Opacity = d > 1 ? d - 1 : d;
                }
                for (int k = half; k < _elliList.Count; k++)
                {
                    elli = _elliList[k];

                    elli.Opacity = _elliList[k - half].Opacity;
                }
            }
        }

        void _dtimer_DoSomething(object obj)
        {
            Stop2();

            int count = _elliList.Count;

            List<double> tlist = new List<double>();
            foreach (UIElement ui in _elliList)
            {
                tlist.Add(ui.Opacity);
            }
            for (int k = 0; k < count; k++)
            {
                if (k == 0)
                {
                    _elliList[k].Opacity = tlist[count - 1];
                }
                else
                {
                    _elliList[k].Opacity = tlist[k - 1];
                }
            }

            Start();
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            Stop2();

            Visibility = System.Windows.Visibility.Visible;

            //_dtimer = new DTimer();
            //_dtimer.DoSomething -= _dtimer_DoSomething;
            //_dtimer.DoSomething += _dtimer_DoSomething;
            //_dtimer.Start(SmallCircularTime);
        }

        private void Stop2()
        {
            if (_dtimer != null)
                _dtimer.Stop();
        }

        public void Stop()
        {
            Stop2();
            Visibility = System.Windows.Visibility.Collapsed;
        }
         
        #endregion

        #region Properties

        private double _SmallCircualWidth = 0;
        public double SmallCircualWidth
        {
            get
            {
                if (_SmallCircualWidth < defaultSize)
                    _SmallCircualWidth = defaultSize;
                return _SmallCircualWidth;
            }
            set
            {
                _SmallCircualWidth = value;
            }
        }

        private double _SmallCircualHeight = 0;
        public double SmallCircualHeight
        {
            get
            {
                if (_SmallCircualHeight < defaultSize)
                    _SmallCircualHeight = defaultSize;
                return _SmallCircualHeight;
            }
            set
            {
                _SmallCircualHeight = value;
            }
        }

        public double SmallCenterX
        {
            get
            {
                return layContainer.Width / 2;
            }
        }

        public double SmallCenterY
        {
            get
            {
                return layContainer.Height / 2;
            }
        }

        private int _CircularCount = 12;
        public int SmallCircularCount
        {
            get
            {
                if (_CircularCount < 2)
                    _CircularCount = 2;
                return _CircularCount;
            }
            set
            {
                _CircularCount = value;
            }
        }

        private Brush _brush = null;
        public Brush SmallCircularBrush
        {
            get
            {
                if (_brush == null)
                    _brush = new SolidColorBrush(Colors.DarkSlateGray);
                return _brush;
            }
            set
            {
                _brush = value;
            }
        }

        private double _SmallCircularTime = 500;
        public double SmallCircularTime
        {
            get
            {
                if (_SmallCircularTime <= 10)
                    _SmallCircularTime = 10;
                return _SmallCircularTime;
            }
            set
            {
                _SmallCircularTime = value;
            }
        }

        public bool IsHalfCircular
        {
            get;
            set;
        } 

        #endregion
    }
}
