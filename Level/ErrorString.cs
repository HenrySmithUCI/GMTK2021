using Godot;
using System;

public class ErrorString : Label
{
    float countdown = 0f;
    public void show(string text)
    {
        Text = text;
        Visible = true;
        countdown = 1f;
    }

    public override void _Process(float delta)
    {
        if(countdown <= 0)
        {
            Visible = false;
        }
        else
        {
            countdown -= delta;
        }
    }
}
