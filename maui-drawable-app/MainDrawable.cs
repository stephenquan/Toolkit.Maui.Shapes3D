using Microsoft.Maui.Graphics;

namespace maui_drawable_app;

public class MainDrawable : IDrawable
{
    private MainViewModel _vm;
    private MainViewModel VM
        => _vm ??= ServiceHelper.GetService<MainViewModel>();

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        VM.Cube.Draw(canvas, dirtyRect);
    }
}
