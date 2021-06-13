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
public class Level5 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 5;

        currentLevel =
"00000000000000000000" +
"000w          0wwww0" +
"0g0           0000w0" +
"0 000000         0w0" +
"0    S        v  0w0" +
"0    f        v  0w0" +
"0    f        V  0w0" +
"0 000000      v  ww0" +
"0g0                0" +
"000w               0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
