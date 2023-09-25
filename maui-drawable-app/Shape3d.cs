
using System.Numerics;

namespace maui_drawable_app;

public class Shape3d
{
    public IList<Vector3> Points { get; set; }
    public IList<Face> Faces { get; set; }
    public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
    public bool HiddenFaceRemoval { get; set; } = true;

    public Point Project(Vector3 pt, Point Center)
    {
        Vector3 pt2 = Vector3.Transform(pt, Transform);
        return new Point(
            Math.Round(Center.X + pt2.X / (1 - pt2.Z / 800)),
            Math.Round(Center.Y + pt2.Y / (1 - pt2.Z / 800))
            );
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
            if (HiddenFaceRemoval)
            {
                Point pt0 = pts[f.Vertices[0]];
                Point pt1 = pts[f.Vertices[1]];
                Point pt2 = pts[f.Vertices[2]];
                Point v1 = new Point(pt1.X - pt0.X, pt1.Y - pt0.Y);
                Point v2 = new Point(pt2.X - pt1.X, pt2.Y - pt1.Y);
                double vz = v1.X * v2.Y - v1.Y * v2.X;
                if (vz <= 0) continue;
            }
            PathF path = new PathF();
            path.MoveTo((float)pts[f.Vertices[0]].X, (float)pts[f.Vertices[0]].Y);
            for (int i = 1;  i < f.Vertices.Count; i++)
            {
                path.LineTo((float)pts[f.Vertices[i]].X, (float)pts[f.Vertices[i]].Y);
            }
            path.Close();
            canvas.FillColor = f.FillColor;
            canvas.FillPath(path);
            canvas.StrokeColor = f.StrokeColor;
            canvas.StrokeSize = f.StrokeSize;
            canvas.DrawPath(path);
        }
    }
}
