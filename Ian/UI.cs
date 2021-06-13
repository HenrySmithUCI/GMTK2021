using Godot;
using System;

public class UI : CanvasLayer
{
    public static UI instance;
    public override void _ExitTree()
    {
        instance = null;
    }

    public override void _Ready()
    {
        instance = this;
    }
    public void showWin()
    {
        GetNode<Control>("WinLabel").Visible = true;
    }
    public void next()
    {
        if(LevelController.levelNum == -1)
        {
            GetTree().ChangeScene("res://Level/LevelMake.tscn");
        }
        else
        {
            GetTree().ChangeScene("res://Level/Levels/" + (LevelController.levelNum+1) + ".tscn");
        }
    }

    public void back()
    {
        if(LevelController.levelNum == -1)
        {
            GetTree().ChangeScene("res://Level/LevelMake.tscn");
        }
        else
        {
            GetTree().ChangeScene("res://Level/LevelSelect.tscn");
        }
    }
}
