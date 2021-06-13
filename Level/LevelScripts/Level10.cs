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
public class Level10 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 10;

        currentLevel =
"00000000000000000000" +
"0Sb    V0    0   Vi0" +
"00i     0    0   vv0" +
"0   0   0    0   vv0" +
"0f  0   i    0   vv0" +
"00000g0      0     0" +
"0 f   i      b     0" +
"0     0fff   b     0" +
"0     0    ggb     0" +
"0F   f0 iiigwb     0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
