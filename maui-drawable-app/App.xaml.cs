namespace maui_drawable_app
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