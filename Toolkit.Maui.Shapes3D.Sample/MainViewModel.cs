using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;

namespace Toolkit.Maui.Shapes3D.Sample;

public class MainViewModel : INotifyPropertyChanged
{
    public bool Wireframe { get; set; } = false;
    public RenderMode Mode => Wireframe ? RenderMode.Wireframe : RenderMode.Solid;

    IList<Vector3> _cubePoints = new List<Vector3>()
        {
            new Vector3() { X = -100, Y = -100, Z = -100 },
            new Vector3() { X = 100, Y = -100, Z = -100 },
            new Vector3() { X = -100, Y = 100, Z = -100 },
            new Vector3() { X = 100, Y = 100, Z = -100 },
            new Vector3() { X = -100, Y = -100, Z = 100 },
            new Vector3() { X = 100, Y = -100, Z = 100 },
            new Vector3() { X = -100, Y = 100, Z = 100 },
            new Vector3() { X = 100, Y = 100, Z = 100 }
        };
    public IList<Vector3> CubePoints => _cubePoints;

    IList<Face> _cubeFaces = new List<Face>()
        {
            new Face() { Vertices = new List<int> {1,0,2,3}, Color = Colors.Red },
            new Face() { Vertices = new List<int> {4,5,7,6}, Color = Colors.Orange },
            new Face() { Vertices = new List<int> {2,0,4,6}, Color = Colors.Yellow },
            new Face() { Vertices = new List<int> {1,3,7,5}, Color = Colors.White },
            new Face() { Vertices = new List<int> {0,1,5,4}, Color = Colors.Blue },
            new Face() { Vertices = new List<int> {3,2,6,7}, Color = Colors.Green }
        };
    public IList<Face> CubeFaces => _cubeFaces;

    private Matrix4x4 _cubeTransform = Matrix4x4.Identity;
    public Matrix4x4 CubeTransform
    {
        get => _cubeTransform;
        set
        {
            _cubeTransform = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CubeTransform)));
        }
    }

    private IDispatcherTimer _timer;

    public Matrix4x4 Delta { get; set; } =
        Matrix4x4.Multiply(
            Matrix4x4.CreateRotationX((float)(0.3 * Math.PI / 180)),
            Matrix4x4.CreateRotationY((float)(0.5 * Math.PI / 180))
            );

    public MainViewModel()
    {
        ToggleRenderModeCommand = new Command(() => ToggleRenderMode());

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToggleRenderModeCommand)));
        _timer = App.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Tick += (s, e) => { CubeTransform = Matrix4x4.Multiply(CubeTransform, Delta); };
        _timer.Start();
    }

    public ICommand ToggleRenderModeCommand { get; set; }

    public void ToggleRenderMode()
    {
        Wireframe = !Wireframe;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Wireframe)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mode)));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
