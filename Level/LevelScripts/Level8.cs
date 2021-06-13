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
public class Level8 : LevelController
{
    public override void _Ready()
    {
        base._Ready();

        levelNum = 8;

        currentLevel = 
"00000000000000000000" +
"00000ff000000      0" +
"0          00      0" +
"0  w       ii      0" +
"0   w          vi  0" +
"0              Vv  0" +
"0          ii      0" +
"0 000000   00      0" +
"0     i0   00      0" +
"0 Sw   0   00      0" +
"00000000000000000000";

        setUpLevel(currentLevel);
    }
}
