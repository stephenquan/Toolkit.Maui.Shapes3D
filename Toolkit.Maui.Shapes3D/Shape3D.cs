using System.Numerics;

namespace Toolkit.Maui.Shapes3D;

public enum RenderMode
{
    Solid,
    Wireframe
};

public class Shape3D
{
    public IList<Vector3> Points { get; set; }
    public IList<Face> Faces { get; set; }
    public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
    public float[] HiddenFaceDashPattern { get; set; } = new float[] { 2, 10};

    public Point Project(Vector3 pt, Point Center)
    {
        Vector3 pt2 = Vector3.Transform(pt, Transform);
        return new Point(
            Math.Round(Center.X + pt2.X / (1 - pt2.Z / 800)),
            Math.Round(Center.Y + pt2.Y / (1 - pt2.Z / 800))
            );
    }

    public void Draw(ICanvas canvas, RectF dirtyRect, RenderMode Mode = RenderMode.Solid)
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