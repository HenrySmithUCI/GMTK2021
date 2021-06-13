using Godot;
using System;

public class Key : Control
{
    bool fadeOut = false;

    public override void _Process(float delta)
    {
        if (fadeOut)
        {
            Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, Modulate.a - (0.5f * delta));
            if (Modulate.a < 0.01)
            {
                Modulate = new Color(1, 1, 1, 0);
                fadeOut = false;
            }
        }
    }

    public void turnInvisible()
    {
        fadeOut = true;
    }

    public void turnVisible()
    {
        GetParent().GetNode<Timer>("VisibilityTimer").Start();
        Modulate = new Color(1, 1, 1, 0.72f);
    }
}
