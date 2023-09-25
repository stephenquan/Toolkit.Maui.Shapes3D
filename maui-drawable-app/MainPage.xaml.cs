namespace maui_drawable_app;

public partial class MainPage : ContentPage
{
    MainViewModel VM;

    public MainPage(MainViewModel VM)
    {
        this.VM = VM;
        InitializeComponent();

        VM.Frame += OnFrame;
    }

    ~MainPage()
    {
        VM.Frame -= OnFrame;
    }

    private void OnFrame(object sender, EventArgs e)
    {
        graphicsView.Invalidate();
    }
}