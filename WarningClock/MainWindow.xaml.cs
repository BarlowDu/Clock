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
        const int defaultWidth= 800;
        const int defaultHeight = 450;
        public MainWindow()
        {
            InitializeComponent();
            InitNotify();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitTimerClock();
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
            exit.Click += (sender, e) => { this.Close(); };
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { max, normal, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

        }

        #endregion


        #region TimerClock
        private void InitTimerClock()
        {


            var vm = new TimeViewModel();
            this.DataContext = vm;

            timerClock = new System.Timers.Timer();
            timerClock.Interval = 1000;
            timerClock.Elapsed += (_sender, _e) =>
            {
                ShowTime(vm);

            };
            timerClock.Start();
        }

        private void ShowTime(TimeViewModel vm)
        {
            var now = DateTime.Now;

            if (now.Minute == 59 && now.Second == 55)
            {
                ShowNormal();
            }

            if (now.Minute==0&&now.Second >= 0 && now.Second <= 10)
            {
                vm.Color = Color.FromRgb(255, 0, 0);
            }
            else
            {
                vm.Color = Color.FromRgb(0, 0, 0);
            }
            vm.Time = DateTime.Now;

        }
        #endregion




        private void HideForm(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            this.Topmost = false;
            this.WindowState = WindowState.Normal;
        }

        private void ShowNormal() {
            this.Dispatcher.Invoke(() => {
                this.Hide();
                this.WindowState = WindowState.Normal;
                this.Width = defaultWidth;
                this.Height = defaultHeight;
                //MessageBox.Show(string.Format("{0}*{1}", bounds.Width, bounds.Height));
                this.Close();
                this.Show();
                var bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                //double screenHeight = SystemParameters.FullPrimaryScreenHeight;
                //double screenWidth = SystemParameters.FullPrimaryScreenWidth;
                this.Topmost = true;
                this.Activate();
                this.Topmost = false;

            });

        }
        private void ShowMax()
        {
            this.Dispatcher.Invoke(() => {
                this.Hide();
                this.WindowState = WindowState.Maximized;
                this.Show();
                this.Topmost = true;
                this.Activate();
                this.Topmost = false;
            });
        }

    }
}
