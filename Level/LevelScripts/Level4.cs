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
public class Level4 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 4;

        currentLevel = 
"00000000000000000000" +
"0     0ww00     0ww0" +
"0   F  ww00   W  ww0" +
"0       000       00" +
"0        00        0" +
"0        00        0" +
"000g g0000000g f0000" +
"0        00        0" +
"0f     ff00f     ff0" +
"0000000vV00000000vV0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
