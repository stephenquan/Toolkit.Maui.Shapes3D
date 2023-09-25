namespace maui_drawable_app;

public class Face
{
    public Color StrokeColor { get; set; } = Colors.Red;
    public float StrokeSize { get; set; } = 1;
    public Color FillColor { get; set; } = Colors.Red;
    public IList<int> Vertices { get; set; }
}
