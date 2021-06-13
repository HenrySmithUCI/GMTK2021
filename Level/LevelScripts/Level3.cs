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
"0gf0       000    00" +
"00gw        0      0" +
"00       w         0" +
"00G           v v  0" +
"000      f    vvVv 0" +
"000f 00            0" +
"0000 f0  g  0      0" +
"0000fv0     0      0" +
"0000v00  s  00    00" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
