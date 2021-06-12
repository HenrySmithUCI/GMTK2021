using Godot;
using System;

public class LevelSelect : Control
{
    public override void _Ready()
    {
        GetNode<GridContainer>("GridContainer").GetNode<Button>("Button1").GrabFocus();
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("menu"))
        {
            GetTree().ChangeScene("res://Level/Main.tscn");
        }
    }

    public void changeLevel(int num)
    {
        switch(num)
        {
            case -1:
                GetTree().ChangeScene("res://Level/LevelMake.tscn");
                break;
            case 0:
                GetTree().ChangeScene("res://Level/Main.tscn");
                break;
            case 1:
                GetTree().ChangeScene("res://Level/Levels/1.tscn");
                break;
            case 2:
                GetTree().ChangeScene("res://Level/Levels/2.tscn");
                break;
            case 3:
                GetTree().ChangeScene("res://Level/Levels/3.tscn");
                break;
            case 4:
                GetTree().ChangeScene("res://Level/Levels/4.tscn");
                break;
            case 5:
                GetTree().ChangeScene("res://Level/Levels/5.tscn");
                break;
            case 6:
                GetTree().ChangeScene("res://Level/Levels/6.tscn");
                break;
            case 7:
                GetTree().ChangeScene("res://Level/Levels/7.tscn");
                break;
            case 8:
                GetTree().ChangeScene("res://Level/Levels/8.tscn");
                break;
            case 9:
                GetTree().ChangeScene("res://Level/Levels/9.tscn");
                break;
            case 10:
                GetTree().ChangeScene("res://Level/Levels/10.tscn");
                break;
        }
    }
}
