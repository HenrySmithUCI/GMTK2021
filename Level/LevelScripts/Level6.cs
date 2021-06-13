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
public class Level6 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 6;

        currentLevel = 
"00000000000000000000" +
"0wbbbbb0  s000000000" +
"0bbbbbbbbbb000000000" +
"00000b00     0     0" +
"0   0        0     0" +
"0 F b     0  0    b0" +
"0   0  00 0  bvv bb0" +
"000000b0b 0  bVvgbb0" +
"0sbbbbb0b 0  b  00b0" +
"0gbbbbb0bbbbbbbbbbb0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
