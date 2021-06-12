using Godot;
using System;

public class LevelSelect : Control
{
    public void changeLevel(int num)
    {
        switch(num)
        {
            case 0:
                GetTree().ChangeScene("res://Ian/Main.tscn");
                break;
            case 1:
                GetTree().ChangeScene("res://Ian/1.tscn");
                break;
            case 2:
                GetTree().ChangeScene("res://Ian/2.tscn");
                break;
            case 3:
                GetTree().ChangeScene("res://Ian/3.tscn");
                break;
            case 4:
                GetTree().ChangeScene("res://Ian/4.tscn");
                break;
            case 5:
                GetTree().ChangeScene("res://Ian/5.tscn");
                break;
            case 6:
                GetTree().ChangeScene("res://Ian/6.tscn");
                break;
            case 7:
                GetTree().ChangeScene("res://Ian/7.tscn");
                break;
            case 8:
                GetTree().ChangeScene("res://Ian/8.tscn");
                break;
            case 9:
                GetTree().ChangeScene("res://Ian/9.tscn");
                break;
            case 10:
                GetTree().ChangeScene("res://Ian/10.tscn");
                break;
        }
    }
}
