using Godot;
using System;
using System.Text;

public class LevelMake : Control
{
    public static string levelString = "";
    public ErrorString es;

    public override void _Ready()
    {
        es = GetNode<ErrorString>("ErrorString");
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
        string valid = validateLevelString(levelString);
        if(valid == "")
            GetTree().ChangeScene("res://Level/Levels/LevelMakeLevel.tscn");
        else
            es.show(valid);
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

    public string validateLevelString(string level)
    {
        if(level.Length < 220)
        {
            return "Level text too short!";
        }
        if(level.Length > 220)
        {
            return "Level text too long!";
        }
        foreach(char c in level)
        {
            if(c == '0' || c == ' ' || c == 'F' || c == 'W' || c == 'G' || c == 'S' || c == 'f' || c == 'w' || c == 'g' || c == 's' || c == 'b' || c == 'i' || c == 'v' || c == 'V')
            {
                continue;
            }
            return "Invalid character \'" + c + "\'!";
        }
        return "";
    }

    public void paste()
    {
        string temp = OS.Clipboard;
        string valid = validateLevelString(temp);
        if(valid == "")
        {
            levelString = temp;
            populateText();
        }
        else
        {
            es.show(valid);
        }
    }
}
