using Godot;
using System;

public class Main : Control
{
    public void switchToPlay()
    {
        GetTree().ChangeScene("res://Ian/LevelOne.tscn");
    }

    public void quit()
    {
        GetTree().Quit();
    }
}
