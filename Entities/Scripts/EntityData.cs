using Godot;
using System;
using System.Collections.Generic;

public abstract class EntityData
{
    public LevelController level;
    public Vector2 position;
    public virtual void UpdateTurn() {}
    public virtual void PreUpdate() {}
    public virtual void PostUpdate() {}
    public bool deathFlag = false;
    public bool isPlayer = false;
}

public class BlobData : EntityData
{
    public BlobElement element;
    public BlobData[] connected;
    public BlobElement buffer = BlobElement.NONE;
    
    public int countConnections()
    {
        int ret = 0;
        foreach(var blob in connected)
            if(blob != null)
                ++ret;
        return ret;
    }
    
    public BlobData(Vector2 pos, BlobElement element, bool bePlayer = false)
    {
        connected = new BlobData[4];
        position = pos;
        this.element = element;
        isPlayer = bePlayer;
    }

    public void getConnections()
    {
        Vector2[] dirs = new Vector2[4] {new Vector2(0,1), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0)};
        for(int i = 0; i < 4; ++i)
        {
            Tile tile = level.getTile(position + dirs[i]);
            if(tile != null && tile.entity != null && tile.entity is BlobData blob 
               && blob.element != BlobElement.BOX && !(element == BlobElement.WATER && blob.element == BlobElement.BOX_BURNING_INIT))
                connected[i] = blob;
            else
                connected[i] = null;
        }
    }

    public override void PreUpdate()
    {
        HashSet<BlobElement> neighborElements = new HashSet<BlobElement>();
        for(int i = 0; i < connected.Length; i++)
        {
            if(connected[i] != null)
                neighborElements.Add(connected[i].element);
        }
        switch(element)
        {
            case BlobElement.NEW_ICE:
                buffer = BlobElement.ICE;
                break;
            case BlobElement.BOX_BURNING_INIT:
                buffer = BlobElement.BOX_BURNING;
                break;
            case BlobElement.GRASS_BURNING:
                deathFlag = true;
                break;
            case BlobElement.BOX_BURNING:
                deathFlag = true;
                break;
            case BlobElement.FIRE:
                if(neighborElements.Contains(BlobElement.WATER))
                {
                    deathFlag = true;
                }
                break;
        }
    }

    public override void PostUpdate()
    {
        if(element == BlobElement.PRE_WATER)
        {
            buffer = BlobElement.WATER;
        }
        if(buffer != BlobElement.NONE)
            level.entityNodes[this].Call("ChangeElement", buffer);
        buffer = BlobElement.NONE;
    }

    public override void UpdateTurn()
    {
        getConnections();
        ProcessSelf();
    }

    public void ProcessSelf()
    {
        HashSet<BlobElement> neighborElements = new HashSet<BlobElement>();
        for(int i = 0; i < connected.Length; i++)
        {
            if(connected[i] != null)
                neighborElements.Add(connected[i].element);
        }

        switch(element)
        {
            case BlobElement.STONE:
                break;
            case BlobElement.FIRE:
                break;
            case BlobElement.WATER:
                if(neighborElements.Contains(BlobElement.ICE))
                {
                    buffer = BlobElement.ICE;

                    if(level.players.Contains(this))
                    {
                        level.players.Remove(this);
                    }
                }
                break;
            case BlobElement.GRASS:
                if((neighborElements.Contains(BlobElement.FIRE)
                || neighborElements.Contains(BlobElement.BOX_BURNING_INIT) || neighborElements.Contains(BlobElement.GRASS_BURNING_INIT)
                || neighborElements.Contains(BlobElement.BOX_BURNING) || neighborElements.Contains(BlobElement.GRASS_BURNING))
                   && !neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.GRASS_BURNING_INIT;
                }
                else if (neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.GRASS;
                }
                break;
            case BlobElement.GRASS_BURNING_INIT:
                if (neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.GRASS;
                }
                else
                {
                    buffer = BlobElement.GRASS_BURNING;
                }
                break;
            case BlobElement.BOX_BURNING_INIT:
                if (neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.BOX;
                }
                else
                {
                    buffer = BlobElement.BOX_BURNING;
                }
                break;
            case BlobElement.GRASS_BURNING:
                if (neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.GRASS;
                    deathFlag = false;
                }
                break;
            case BlobElement.BOX:
                if ((neighborElements.Contains(BlobElement.FIRE)
                || neighborElements.Contains(BlobElement.BOX_BURNING) || neighborElements.Contains(BlobElement.GRASS_BURNING)
                || neighborElements.Contains(BlobElement.BOX_BURNING_INIT) || neighborElements.Contains(BlobElement.GRASS_BURNING_INIT))
                    && !neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.BOX_BURNING_INIT;
                }
                break;
            case BlobElement.ICE:
                if (neighborElements.Contains(BlobElement.FIRE))
                {
                    buffer = BlobElement.WATER;
                }
                break;
            case BlobElement.NEW_ICE:
                if (neighborElements.Contains(BlobElement.FIRE))
                {
                    buffer = BlobElement.WATER;
                }
                else
                {
                    buffer = BlobElement.ICE;
                }
                break;
            case BlobElement.BOX_BURNING:
                if (neighborElements.Contains(BlobElement.WATER))
                {
                    buffer = BlobElement.BOX;
                }
                break;
        }
    }
}

public enum BlobElement {STONE, FIRE, WATER, GRASS, BOX, ICE, BOX_BURNING, GRASS_BURNING, NEW_ICE, BOX_BURNING_INIT, GRASS_BURNING_INIT, PRE_WATER, NONE}
public enum Direction {UP, RIGHT, DOWN, LEFT}


