using System.Numerics;

namespace maui_drawable_app;

public class MainViewModel
{
    public Shape3d Cube = new Shape3d()
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
            new Face() { Vertices = new List<int> {1,0,2,3}, StrokeColor = Colors.Red, FillColor = Colors.Red },
            new Face() { Vertices = new List<int> {4,5,7,6}, StrokeColor = Colors.Orange, FillColor = Colors.Orange },
            new Face() { Vertices = new List<int> {2,0,4,6}, StrokeColor = Colors.Yellow, FillColor = Colors.Yellow },
            new Face() { Vertices = new List<int> {1,3,7,5}, StrokeColor = Colors.White, FillColor = Colors.White },
            new Face() { Vertices = new List<int> {0,1,5,4}, StrokeColor = Colors.Blue, FillColor = Colors.Blue },
            new Face() { Vertices = new List<int> {3,2,6,7}, StrokeColor = Colors.Green, FillColor = Colors.Green }
        }
    };

    //private Timer;
    private IDispatcherTimer _timer;

    public EventHandler Frame;

    public Matrix4x4 Delta { get; set; } =
        Matrix4x4.Multiply(
            Matrix4x4.CreateRotationX((float)(0.3 * Math.PI / 180)),
            Matrix4x4.CreateRotationY((float)(0.5 * Math.PI / 180))
            );

    public MainViewModel()
    {
        _timer = App.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Tick += (s, e) =>
        {
            Cube.Transform = Matrix4x4.Multiply(Cube.Transform, Delta);
            Frame?.Invoke(this, EventArgs.Empty);
        };
        _timer.Start();
    }
}
