using Godot;
using System;
using System.Collections.Generic;

public class LevelMakeLevel : LevelController
{
    public override void _Ready()
    {
        base._Ready();
        backScene = "res://Level/LevelMake.tscn";
        levelNum = -1;
        currentLevel = LevelMake.levelString;
        setUpLevel(currentLevel);
    }
}
