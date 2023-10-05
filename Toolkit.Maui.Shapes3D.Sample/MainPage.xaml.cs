namespace Toolkit.Maui.Shapes3D.Sample;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel VM)
    {
        BindingContext = VM;
        InitializeComponent();
    }
}
