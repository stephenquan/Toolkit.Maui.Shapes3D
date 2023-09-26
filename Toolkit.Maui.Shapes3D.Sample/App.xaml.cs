namespace Toolkit.Maui.Shapes3D.Sample
{
    public partial class App : Application
    {
        public App()
        {
            UserAppTheme = AppTheme.Dark;

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}