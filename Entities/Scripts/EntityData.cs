using Godot;
using System;
using System.Collections.Generic;

public class EntityData
{
    public LevelController level;
    public Vector2 position;
    public virtual void UpdateTurn() {}
}

public class BlobData : EntityData
{
    public BlobElement element;
    public BlobData[] connected;
    public BlobData(Vector2 pos, BlobElement element)
    {
        connected = new BlobData[4];
        position = pos;
        this.element = element;
    }
    public override void UpdateTurn()
    {
        Vector2[] dirs = new Vector2[4] {new Vector2(0,1), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0)};
        for(int i = 0; i < 4; ++i)
        {
            Tile tile = level.getTile(position + dirs[i]);
            if(tile != null && tile.entity != null && tile.entity is BlobData blob)
                connected[i] = blob;
            else
                connected[i] = null;
        }
    }
}

public enum BlobElement {STONE, FIRE, WATER, GRASS}
public enum Direction {UP, RIGHT, DOWN, LEFT}

public class PlayerData : BlobData
{
    public PlayerData(Vector2 pos, BlobElement element) : base (pos, element)
    {
    }
}

public class BoxData : EntityData
{

}


