using Godot;
using System;

public class LevelTemplate : LevelController
{
    public override void _Ready()
    {
        walls = new int[,] { { 1, 1, 1, 1, 1},
                              { 1, 0, 0, 0, 1},
                              { 1, 0, 0, 0, 1},
                              { 1, 0, 0, 0, 1},
                              { 1, 1, 1, 1, 1} };

        entities = new BoxData[]{ new BoxData(0, new Vector2(1, 1), gridSize)};

        setUpLevel();
    }
}
