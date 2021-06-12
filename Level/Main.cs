using Godot;
using System;

public class Main : Control
{
    public override void _Ready()
    {
        GetNode<VBoxContainer>("VBoxContainer").GetNode<Button>("Play").GrabFocus();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("menu"))
        {
            GetTree().Quit();
        }
    }

    public void switchToPlay()
    {
        GetTree().ChangeScene("res://Level/LevelSelect.tscn");
    }

    public void quit()
    {
        GetTree().Quit();
    }
}
