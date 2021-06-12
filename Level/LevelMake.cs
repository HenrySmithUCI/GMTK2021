using Godot;
using System;
using System.Text;

public class LevelMake : Control
{
    public static string levelString = "";

    public override void _Ready()
    {
        if(levelString == "")
        {
            levelString =
"00000000000000000000" +
"0                  0" +
"0                  0" +
"0                  0" +
"0                  0" +
"0                  0" +
"0                  0" +
"0                  0" +
"0                  0" +
"0                  0" +
"00000000000000000000";
        }
        populateText();
    }

    public void back()
    {
        GetTree().ChangeScene("res://Level/LevelSelect.tscn");
    }

    public void play()
    {
        GetTree().ChangeScene("res://Level/Levels/LevelMakeLevel.tscn");
    }

    public void textChanged(int line)
    {

    }

    public void populateText()
    {

    }
}
