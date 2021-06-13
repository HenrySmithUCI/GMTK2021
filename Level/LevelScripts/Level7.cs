using Godot;
using System;
using System.Collections.Generic;

//Key:
//Space: empty
//0: wall
//f,w,g,s: non player blob of fire, water, grass, or stone
//F,W,G,S: player blob
//b: burnable box
//i: ice
public class Level7 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 7;

        currentLevel = 
"00000000000000000000" +
"00000          V0000" +
"00000           0000" +
"000000000w0000000000" +
"0W              0000" +
"0bbbbbb00b000  V0000" +
"0F              0000" +
"0bbbbbbbbbbbbbbb0000" +
"0               0000" +
"00000000000000000000" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
