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
public class Level1 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 1;

        currentLevel = 
"00000000000000000000" +
"0         0        0" +
"0    s        s    0" +
"0                  0" +
"0000  00000  vv    0" +
"0         0  V     0" +
"0         0        0" +
"0    S    0        0" +
"0         0        0" +
"0         0        0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
