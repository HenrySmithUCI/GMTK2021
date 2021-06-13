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
public class Level9 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 9;

        currentLevel = 
"00000000000000000000" +
"0 f     i  00    g 0" +
"0 v  i f         g 0" +
"0vV                0" +
"0 v  i f       i   0" +
"0       i      i   0" +
"0                  0" +
"0000 i i i i i i i 0" +
"0                  0" +
"0 W      f   f   f 0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
