using Godot;
using System;

public class UI : CanvasLayer
{
    AudioStreamPlayer victorySound;

    public static UI instance;
    public override void _ExitTree()
    {
        instance = null;
    }

    public override void _Ready()
    {
        instance = this;
        victorySound = GetNode<AudioStreamPlayer>("VictorySound");
    }

    public void showWin()
    {
        GetNode<Control>("WinLabel").Visible = true;
        GetNode<Control>("WinLabel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Button>("Next").GrabFocus();
        SoundController.instance.play("Victory");
    }

    public void next()
    {
        if(LevelController.levelNum == -1)
        {
            GetTree().ChangeScene("res://Level/LevelMake.tscn");
        }
        else if(LevelController.levelNum == 10)
        {
            GetTree().ChangeScene("res://Level/LevelSelect.tscn");
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
