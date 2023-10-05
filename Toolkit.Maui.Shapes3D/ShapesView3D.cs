using System.Numerics;

namespace Toolkit.Maui.Shapes3D;

public enum RenderMode
{
    Solid,
    Wireframe
};

public class ShapesView3D : GraphicsView, IDrawable
{
    public static readonly BindableProperty PointsProperty
       = BindableProperty.Create(nameof(Points), typeof(IList<Vector3>), typeof(ShapesView3D), propertyChanged: RequestInvalidate);
    public static readonly BindableProperty FacesProperty
       = BindableProperty.Create(nameof(Faces), typeof(IList<Face>), typeof(ShapesView3D), propertyChanged: RequestInvalidate);
    public static readonly BindableProperty TransformProperty
       = BindableProperty.Create(nameof(Transform), typeof(Matrix4x4), typeof(ShapesView3D), Matrix4x4.Identity, propertyChanged: RequestInvalidate);
    public static readonly BindableProperty HiddenFaceDashPatternProperty
       = BindableProperty.Create(nameof(HiddenFaceDashPattern), typeof(float[]), typeof(ShapesView3D), new float[] { 2, 10 }, propertyChanged: RequestInvalidate);
    public static readonly BindableProperty ModeProperty
       = BindableProperty.Create(nameof(Mode), typeof(RenderMode), typeof(ShapesView3D), RenderMode.Solid, propertyChanged: RequestInvalidate);

    public IList<Vector3> Points
    {
        get => (IList<Vector3>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }
    public IList<Face> Faces
    {
        get => (IList<Face>)GetValue(FacesProperty);
        set => SetValue(FacesProperty, value);
    }
    public Matrix4x4 Transform
    {
        get => (Matrix4x4)GetValue(TransformProperty);
        set => SetValue(TransformProperty, value);
    }
    public float[] HiddenFaceDashPattern
    {
        get => (float[])GetValue(HiddenFaceDashPatternProperty);
        set => SetValue(HiddenFaceDashPatternProperty, value);
    }
    public RenderMode Mode
    {
        get => (RenderMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    private static void RequestInvalidate(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GraphicsView)
        {
            ((GraphicsView)bindable).Invalidate();
        }
    }

    public Point Project(Vector3 pt, Point Center)
    {
        Vector3 pt2 = Vector3.Transform(pt, Transform);
        return new Point(
            Math.Round(Center.X + pt2.X / (1 - pt2.Z / 800)),
            Math.Round(Center.Y + pt2.Y / (1 - pt2.Z / 800))
            );
    }

    public ShapesView3D()
    {
        Drawable = this;
        BackgroundColor = Colors.LightSteelBlue;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (Points == null)
        {
            return;
        }

        if (Faces == null)
        {
            return;
        }

        List<Point> pts = new List<Point>(Points.Select(pt => Project(pt, dirtyRect.Center)));
        foreach (var f in Faces)
        {
            Point pt0 = pts[f.Vertices[0]];
            Point pt1 = pts[f.Vertices[1]];
            Point pt2 = pts[f.Vertices[2]];
            Point v1 = new Point(pt1.X - pt0.X, pt1.Y - pt0.Y);
            Point v2 = new Point(pt2.X - pt1.X, pt2.Y - pt1.Y);
            double vz = v1.X * v2.Y - v1.Y * v2.X;
            bool HiddenFace = (vz <= 0);

            if (Mode == RenderMode.Solid)
            {
                if (f.Color == Colors.Transparent) continue;
                if (HiddenFace) continue;
            }

            PathF path = new PathF();
            path.MoveTo((float)pts[f.Vertices[0]].X, (float)pts[f.Vertices[0]].Y);
            for (int i = 1; i < f.Vertices.Count; i++)
            {
                path.LineTo((float)pts[f.Vertices[i]].X, (float)pts[f.Vertices[i]].Y);
            }
            path.Close();

            if (Mode == RenderMode.Solid)
            {
                canvas.FillColor = f.Color;
                canvas.FillPath(path);
            }

            if (Mode == RenderMode.Wireframe)
            {
                canvas.StrokeColor = f.Color;
                canvas.StrokeSize = 1;
                canvas.StrokeDashPattern = HiddenFace ? HiddenFaceDashPattern : null;
                canvas.DrawPath(path);
            }
        }
    }
}
