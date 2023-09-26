namespace Toolkit.Maui.Shapes3D.Sample;

public partial class MainPage : ContentPage
{
    private MainViewModel VM { get; set; }

    public MainPage(MainViewModel VM)
    {
        this.VM = VM;
        BindingContext = VM;
        VM.Frame += OnFrame;
        InitializeComponent();
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