using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarningClock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Timers.Timer timerClock;
        TimeWindow timeWindow;
        TimeViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            InitNotify();
            InitLocation();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            //this.Hide();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowUtil.HideAltTab(this);
            InitTimerClock();
            //this.Hide();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void InitLocation()
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;

            this.Left = screenWidth - this.Width;
            this.Top = screenHeight - this.Height;
        }

        #region notify

        private void InitNotify()
        {

            System.Windows.Forms.NotifyIcon notifyIcon = null;
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "整点报时服务";//最小化到托盘时，鼠标点击时显示的文本
            notifyIcon.Icon = WarningClock.Properties.Resources.clock;//程序图标
            notifyIcon.Visible = true;


            System.Windows.Forms.MenuItem max = new System.Windows.Forms.MenuItem("全屏");
            max.Click += (sender, e) => { ShowMax(); };

            System.Windows.Forms.MenuItem normal = new System.Windows.Forms.MenuItem("正常");
            normal.Click += (sender, e) => { ShowNormal(); };



            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += (sender, e) =>
            {
                timeWindow?.Close();
                this.Close();
            };
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { max, normal, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

        }

        #endregion


        #region TimerClock
        private void InitTimerClock()
        {
            viewModel = new TimeViewModel();
            this.DataContext = viewModel;
            timerClock = new System.Timers.Timer();
            timerClock.Interval = 1000;
            timerClock.Elapsed += (_sender, _e) =>
            {
                ShowTime();

            };
            timerClock.Start();
        }



        private void ShowTime()
        {
            var now = DateTime.Now;

            if (now.Minute == 59 && now.Second == 55)
            {
                ShowNormal();
            }

            if (now.Minute == 0 && now.Second >= 0 && now.Second <= 10)
            {
                viewModel.Color = Color.FromRgb(255, 0, 0);
            }
            else
            {
                viewModel.Color = Color.FromRgb(0, 0, 0);
            }
            viewModel.Time = DateTime.Now;

        }
        #endregion






        #region show timewindow
        private void ShowNormal()
        {

            if (timeWindow == null)
            {
                ShowTimeWindow();
                return;
            }
            if (timeWindow.WindowState == WindowState.Normal)
            {
                timeWindow.Activate();
                return;
            }

            timeWindow.Close();

            ShowTimeWindow();

        }
        private void ShowMax()
        {
            if (timeWindow != null)
            {
                if (timeWindow.WindowState == WindowState.Maximized)
                {
                    timeWindow.Activate();
                    return;
                }
                timeWindow.Close();
            }
            ShowTimeWindow(WindowState.Maximized);

        }

        private void ShowTimeWindow(WindowState windowState = WindowState.Normal)
        {
            Dispatcher.Invoke(() =>
            {
                var result = new TimeWindow(viewModel)
                {
                    WindowState = windowState
                };
                result.Closed += (object sender, EventArgs e) =>
                {
                    if (sender == this.timeWindow)
                    {
                        this.timeWindow = null;
                    }
                };
                this.timeWindow = result;
                result.Topmost = true;
                result.Show();
                result.Activate();
            });
        }
        #endregion
    }

}
