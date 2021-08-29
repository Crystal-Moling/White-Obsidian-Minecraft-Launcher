using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Panuon.UI.Silver;
using White_Obsidian_Minecraft_Launcher.LaunchCore;
using KMCCC.Launcher;
using KMCCC.Authentication;
using System;

namespace White_Obsidian_Minecraft_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public static LauncherCore Core = LauncherCore.Create();
        public bool IsTg_BtnChecked = false;




        //初始化程序
        public MainWindow()
        {
            InitializeComponent();
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
            List<string> jList = GetJavaList();
            JavaCombo.ItemsSource = jList;
        }

        public List<string> GetJavaList()
        {
            return GetJavaPath.FindJava().ToList();
        }

        //获取版本列表
        public void ToGetVersionList()
        {
            var versions = Core.GetVersions().ToArray();
            VersionCombo.ItemsSource = versions;
            VersionCombo.DisplayMemberPath = "Id";
        }

        //启动游戏变量赋值
        public void LaunchGame()
        {

            Core.JavaPath = JavaCombo.Text;
            var ver = (KMCCC.Launcher.Version)VersionCombo.SelectedItem;
            int MaxMemoryies;
            if (string.IsNullOrEmpty(MaxMemoryTextBox.Text))
            {
                MaxMemoryies = 1024;
            }
            else
            {
                MaxMemoryies = Convert.ToInt32(MaxMemoryTextBox.Text);
            }
            CoreLauncher(ver, MaxMemoryies, new OfflineAuthenticator(AccountCombo.Text));
            //CoreLauncher(ver, MaxMemoryies, new YggdrasilLogin("邮箱", "密码", true); // 正版启动，最后一个为是否twitch登录
        }

        //启动游戏
        public void CoreLauncher(KMCCC.Launcher.Version ver, int MaxMemories, IAuthenticator Auth)
        {
            LaunchResult result;
            if ((bool)(VersionIsolationCheckBox.IsChecked == true))
            {
                result = Core.Launch(new LaunchOptions
                {
                    Version = ver,
                    MaxMemory = MaxMemories,
                    Authenticator = Auth,
                    Mode = LaunchMode.MCLauncher
                    //Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                    //Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
                });
            }
            else
            {
                result = Core.Launch(new LaunchOptions
                {
                    Version = ver,
                    MaxMemory = MaxMemories,
                    Authenticator = Auth,
                    //Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                    //Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
                });
            }

            if (!result.Success)
            {
                //MessageBox.Show(result.ErrorMessage, result.ErrorType.ToString());
                switch (result.ErrorType)
                {
                    case ErrorType.NoJAVA:
                        MessageBoxX.Show("你系统的Java有异常，可能你非正常途径删除过Java，请尝试重新安装Java\n详细信息：" + result.ErrorMessage, "错误");
                        break;
                    case ErrorType.AuthenticationFailed:
                        MessageBoxX.Show("正版验证失败！请检查你的账号密码", "账号错误\n详细信息：" + result.ErrorMessage);
                        break;
                    case ErrorType.UncompressingFailed:
                        MessageBoxX.Show("可能的多开或文件损坏，请确认文件完整且不要多开\n如果你不是多开游戏的话，请检查libraries文件夹是否完整\n详细信息：" + result.ErrorMessage, "可能的多开或文件损坏");
                        break;
                    default:
                        MessageBoxX.Show(
                            result.ErrorMessage + "\n" +
                            (result.Exception == null ? string.Empty : result.Exception.StackTrace),
                            "启动错误，请将此窗口截图向开发者寻求帮助");
                        break;
                }
            }
        }

        //退出程序
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //启动游戏
        private void LaunchBtn_Click(object sender, RoutedEventArgs e)
        {
            LaunchGame();
        }




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
