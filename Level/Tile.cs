using Godot;
using System;

public class Tile
{
    public bool isBlock;
    public EntityData entity;

    public Tile(bool isBlock)
    {
        this.isBlock = isBlock;
    }
}
