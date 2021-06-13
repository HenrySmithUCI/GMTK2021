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
public class Level3 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 3;

        currentLevel = 
"00000000000000000000" +
"0                  0" +
"0     g   w        0" +
"0        g         0" +
"0            vv    0" +
"0            Vv    0" +
"0    F             0" +
"0                  0" +
"0                  0" +
"0                  0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
