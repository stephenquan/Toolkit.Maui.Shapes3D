using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;

namespace Toolkit.Maui.Shapes3D.Sample;

public class MainViewModel : INotifyPropertyChanged
{
    public bool Wireframe { get; set; } = true;
    public RenderMode Mode => Wireframe ? RenderMode.Wireframe : RenderMode.Solid;

    public Shape3D Cube = new Shape3D()
    {
        Points = new List<Vector3>()
        {
            new Vector3() { X = -100, Y = -100, Z = -100 },
            new Vector3() { X = 100, Y = -100, Z = -100 },
            new Vector3() { X = -100, Y = 100, Z = -100 },
            new Vector3() { X = 100, Y = 100, Z = -100 },
            new Vector3() { X = -100, Y = -100, Z = 100 },
            new Vector3() { X = 100, Y = -100, Z = 100 },
            new Vector3() { X = -100, Y = 100, Z = 100 },
            new Vector3() { X = 100, Y = 100, Z = 100 }
        },

        Faces = new List<Face>()
        {
            new Face() { Vertices = new List<int> {1,0,2,3}, Color = Colors.Red },
            new Face() { Vertices = new List<int> {4,5,7,6}, Color = Colors.Orange },
            new Face() { Vertices = new List<int> {2,0,4,6}, Color = Colors.Yellow },
            new Face() { Vertices = new List<int> {1,3,7,5}, Color = Colors.White },
            new Face() { Vertices = new List<int> {0,1,5,4}, Color = Colors.Blue },
            new Face() { Vertices = new List<int> {3,2,6,7}, Color = Colors.Green }
        }
    };

    private IDispatcherTimer _timer;

    public EventHandler Frame;

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
        _timer.Tick += (s, e) =>
        {
            Cube.Transform = Matrix4x4.Multiply(Cube.Transform, Delta);
            Frame?.Invoke(this, EventArgs.Empty);
        };
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
