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

        GetNode<Button>("Play").GrabFocus();
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("menu"))
        {
            GetTree().ChangeScene("res://Level/LevelSelect.tscn");
        }
    }

    public void back()
    {
        GetTree().ChangeScene("res://Level/LevelSelect.tscn");
    }

    public void play()
    {
        GetTree().ChangeScene("res://Level/Levels/LevelMakeLevel.tscn");
    }

    public void textChange(string text, int line)
    {
        string temp = levelString.Substr(0, line*20);
        while(text.Length < 20)
            text += " ";
        temp += text;
        temp += levelString.Substr((line+1)*20,levelString.Length - ((line+1)*20));
        levelString = temp;
    }

    public void populateText()
    {
        for(int i = 0; i < 11; ++i)
        {
            GetNode<LineEdit>("PanelContainer/VBoxContainer/"+i).Text = levelString.Substr(i*20, 20);
        }
    }

    public void copy()
    {
        OS.Clipboard = levelString;
    }

    public void paste()
    {
        levelString = OS.Clipboard;
        populateText();
    }
}
