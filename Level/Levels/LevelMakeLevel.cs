using Godot;
using System;
using System.Collections.Generic;

public class LevelMakeLevel : LevelController
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        levelNum = -1;
        currentLevel = LevelMake.levelString;
        setUpLevel(currentLevel);
    }

    public override void _Process(float delta)
    {
        if (players.Count > 0)
        {
            if (Input.IsActionJustPressed("right"))
            {
                move(new Vector2(1, 0));
            }
            else if (Input.IsActionJustPressed("left"))
            {
                move(new Vector2(-1, 0));
            }
            if (Input.IsActionJustPressed("up"))
            {
                move(new Vector2(0, -1));
            }
            else if (Input.IsActionJustPressed("down"))
            {
                move(new Vector2(0, 1));
            }
            else if (Input.IsActionJustPressed("step"))
            {
                move(new Vector2(0, 0));
            }
        }

        if(Input.IsActionJustPressed("undo"))
        {
            undo();
        }
        if(Input.IsActionPressed("undo"))
        {
            undoTimer -= delta;
            if(undoTimer <= 0)
            {
                undo();
                undoTimer += 0.1f;
            }
        }
        else
        {
            undoTimer = 0.5f;
        }
        if(Input.IsActionJustPressed("restart"))
        {
            turnNumber = 0;
            destroyLevel();
            setUpLevel(currentLevel, false);
            undoList = new List<string>();
        }
        if(Input.IsActionJustPressed("menu"))
        {
            GetTree().ChangeScene("res://Level/LevelMake.tscn");
        }
    }
}
