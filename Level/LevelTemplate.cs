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
public class LevelTemplate : LevelController
{
    public override void _Ready()
    {
        currentLevel = 
"00000000000000000000" +
"0      bbb         0" +
"0      ggg         0" +
"0    g      00     0" +
"0           0      0" +
"0   W  w       F   0" +
"0                  0" +
"0   s  f    w      0" +
"0                  0" +
"0                  0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
