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
"00000000000000000000" +
"00                00" +
"00   s   00    s  00" +
"00       00   v   00" +
"00       00  Vv   00" +
"00000 00000       00" +
"000     000       00" +
"000  S  000000000000" +
"000     000000000000" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
