using Godot;
using System;

public class Tile
{
    public EntityData entity;
    public TileType type;

    public Tile(TileType t)
    {
        type = t;
    }
}

public enum TileType {NONE,BLOCK,VICTORY,VICTORY_PLAYER,CONVERT_FIRE,CONVERT_WATER,CONVERT_GRASS}
