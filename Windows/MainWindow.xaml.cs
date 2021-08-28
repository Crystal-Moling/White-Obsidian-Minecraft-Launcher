using System.Windows;
using System.Windows.Input;
using Panuon.UI.Silver;

namespace White_Obsidian_Minecraft_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializationFunctionGrid();
            //ShowFirstLauncgGuide();
        }

        public bool IsTg_BtnChecked = false;
        public bool IsFirstLaunch = true;

        public void InitializationFunctionGrid()//初始化功能网格
        {
            MenuGrid.Visibility = Visibility.Hidden;
            SettingGrid.Visibility = Visibility.Hidden;
            AccountGrid.Visibility = Visibility.Hidden;
            VersionGrid.Visibility = Visibility.Hidden;
        }

        public void ShowFirstLauncgGuide()//初次运行向导
        {
            if (IsFirstLaunch == true)
            {
                img_bg.Opacity = 0.3;
                FirstLaunchGuide.Visibility = Visibility.Visible;
            }
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
                MenuGrid.Visibility = Visibility.Hidden;
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
            if (SettingGrid.Visibility == Visibility.Hidden && AccountGrid.Visibility == Visibility.Hidden && VersionGrid.Visibility == Visibility.Hidden)
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

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void LaunchBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializationFunctionGrid();
            if (IsTg_BtnChecked == true)
            {
                MenuGrid.Visibility = Visibility.Visible;
                Tg_Btn.IsChecked = false;
                img_bg.Opacity = 1;
            }
            else
            {
                Tg_Btn.IsChecked = false;
                img_bg.Opacity = 1;
            }
        }

        private void SettingButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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
            InitializationFunctionGrid();
            VersionGrid.Visibility = Visibility.Visible;
            Tg_Btn.IsChecked = false;
            img_bg.Opacity = 0.3;
        }
    }
}
