using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Panuon.UI.Silver;
using White_Obsidian_Minecraft_Launcher.LaunchCore;
using KMCCC.Launcher;
using KMCCC.Authentication;

namespace White_Obsidian_Minecraft_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public static LauncherCore Core = LauncherCore.Create();
        public bool IsTg_BtnChecked = false;
        public bool IsFirstLaunch = true;

        //初始化程序
        public MainWindow()
        {
            InitializeComponent();
            //ShowFirstLauncgGuide();
            InitializationLaunchCore();
            InitializationFunctionGrid();
        }

        //初始化功能网格
        public void InitializationFunctionGrid()
        {
            MenuGrid.Visibility = SettingGrid.Visibility = AccountGrid.Visibility = VersionGrid.Visibility = Pop_ups_AddAccount.Visibility = Visibility.Collapsed;
        }

        //初始化启动核心
        public void InitializationLaunchCore()
        {
            ToGetJavaList();
            ToGetVersionList();
        }




        //取Java路径
        public void ToGetJavaList()
        {
            var jList = GetJavaList();
            JavaCombo.ItemsSource = jList;
        }

        public List<string> GetJavaList()
        {
            return (List<string>)GetJavaPath.FindJava().ToList();
        }

        //获取版本列表
        public void ToGetVersionList()
        {
            var versions = Core.GetVersions().ToArray();
            VersionCombo.ItemsSource = versions;
            VersionCombo.DisplayMemberPath = "Id";
        }

        //退出程序
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //启动游戏
        private void LaunchBtn_Click(object sender, RoutedEventArgs e)
        {
            Core.JavaPath = JavaCombo.Text;
            var ver = (KMCCC.Launcher.Version)VersionCombo.SelectedItem;
            var result = Core.Launch(new LaunchOptions
            {
                Version = ver, //Ver为Versions里你要启动的版本名字
                MaxMemory = 1024, //最大内存，int类型
                Authenticator = new OfflineAuthenticator(AccountCombo.Text), //离线启动，ZhaiSoul那儿为你要设置的游戏名
                //Authenticator = new YggdrasilLogin("邮箱", "密码", true), // 正版启动，最后一个为是否twitch登录
                Mode = LaunchMode.MCLauncher, //启动模式，这个我会在后面解释有哪几种
                //Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                //Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
            });
        }

        //初次运行向导
        //public void ShowFirstLauncgGuide()
        //{
        //    if (IsFirstLaunch == true)
        //    {
        //        img_bg.Opacity = 0.3;
        //        FirstLaunchGuide.Visibility = Visibility.Visible;
        //    }
        //}




        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // 设置工具提示可见性

            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_setting.Visibility = Visibility.Collapsed;
                tt_user.Visibility = Visibility.Collapsed;
                tt_version.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_setting.Visibility = Visibility.Visible;
                tt_user.Visibility = Visibility.Visible;
                tt_version.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.Visibility == Visibility.Visible)
            {
                img_bg.Opacity = 1;
                MenuGrid.Visibility = Visibility.Collapsed;
                IsTg_BtnChecked = false;
            }
            if (SettingGrid.Visibility == Visibility.Visible && AccountGrid.Visibility == Visibility.Visible && VersionGrid.Visibility == Visibility.Visible)
            {
                img_bg.Opacity = 1;
                IsTg_BtnChecked = false;
            }
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            if (SettingGrid.Visibility == Visibility.Collapsed && AccountGrid.Visibility == Visibility.Collapsed && VersionGrid.Visibility == Visibility.Collapsed)
            {
                img_bg.Opacity = 0.3;
                MenuGrid.Visibility = Visibility.Visible;
                IsTg_BtnChecked = true;
            }
        }

        private void BG_PreviewMouseLeftButtomDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MenuButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializationFunctionGrid();
            Tg_Btn.IsChecked = false;
            img_bg.Opacity = 1;
        }

        private void SettingButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToGetJavaList();
            InitializationFunctionGrid();
            SettingGrid.Visibility = Visibility.Visible;
            Tg_Btn.IsChecked = false;
            img_bg.Opacity = 0.3;
        }

        private void AccountButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializationFunctionGrid();
            AccountGrid.Visibility = Visibility.Visible;
            Tg_Btn.IsChecked = false;
            img_bg.Opacity = 0.3;
        }

        private void VersionButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToGetVersionList();
            InitializationFunctionGrid();
            VersionGrid.Visibility = Visibility.Visible;
            Tg_Btn.IsChecked = false;
            img_bg.Opacity = 0.3;
        }

        private void AddAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            Pop_ups.Visibility = Pop_ups_AddAccount.Visibility = Visibility.Visible;
        }

        private void AddAccount_Canceled(object sender, RoutedEventArgs e)
        {
            AddAccountTextBox.Text = "";
            Pop_ups.Visibility = Pop_ups_AddAccount.Visibility = Visibility.Collapsed;
        }

        private void AddAccount_Confirmed(object sender, RoutedEventArgs e)
        {
            AccountCombo.Items.Add(AddAccountTextBox.Text);
            AddAccountTextBox.Text = "";
            Pop_ups.Visibility = Pop_ups_AddAccount.Visibility = Visibility.Collapsed;
        }
    }
}
