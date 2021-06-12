using Godot;
using System;
using System.Collections.Generic;

public abstract class EntityData
{
    public LevelController level;
    public Vector2 position;
    public virtual void UpdateTurn() {}
    public bool deathFlag = false;
    public int deathTimer = -1;
    public bool isPlayer = false;
}

public class BlobData : EntityData
{
    public BlobElement element;
    public BlobData[] connected;
    
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

    public override void UpdateTurn()
    {
        Vector2[] dirs = new Vector2[4] {new Vector2(0,1), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0)};
        for(int i = 0; i < 4; ++i)
        {
            Tile tile = level.getTile(position + dirs[i]);
            if(tile != null && tile.entity != null && tile.entity is BlobData blob 
               && ((BlobData)tile.entity).element != BlobElement.BOX)
                connected[i] = blob;
            else
                connected[i] = null;
        }
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

        // countdown death timer if it above 0
        if(deathTimer > 0)
        {
            deathTimer--;
        }

        switch(element)
        {
            case BlobElement.STONE:
                break;
            case BlobElement.FIRE:
                if(neighborElements.Contains(BlobElement.WATER))
                {
                    deathTimer = 0;
                }
                break;
            case BlobElement.WATER:
                if(neighborElements.Contains(BlobElement.ICE))
                {
                    level.entityNodes[this].Call("ChangeElement", BlobElement.ICE);

                    if(level.players.Contains(this))
                    {
                        level.players.Remove(this);
                    }
                }
                break;
            case BlobElement.GRASS:
                if(neighborElements.Contains(BlobElement.FIRE))
                {
                    if(deathTimer < 0)
                    {
                        deathTimer = 2;
                        level.entityNodes[this].Call("SetParticles", BlobElement.FIRE);
                    }
                }
                if (neighborElements.Contains(BlobElement.WATER))
                {
                    deathTimer = -1;
                    level.entityNodes[this].Call("SetParticles", BlobElement.GRASS);
                }
                break;
            case BlobElement.BOX:
                if (neighborElements.Contains(BlobElement.FIRE) || neighborElements.Contains(BlobElement.BOX_BURNING))
                {
                    if (deathTimer < 0)
                    {
                        deathTimer = 2;
                        level.entityNodes[this].Call("ChangeElement", BlobElement.BOX_BURNING);
                    }
                }

                if (neighborElements.Contains(BlobElement.WATER))
                {
                    deathTimer = -1;
                    level.entityNodes[this].Call("SetParticles", BlobElement.BOX);
                }
                break;
            case BlobElement.ICE:
                if (neighborElements.Contains(BlobElement.FIRE))
                {
                    level.entityNodes[this].Call("ChangeElement", BlobElement.WATER);
                }
                break;
            case BlobElement.BOX_BURNING:
                if (neighborElements.Contains(BlobElement.WATER))
                {
                    deathTimer = -1;
                    level.entityNodes[this].Call("ChangeElement", BlobElement.BOX);
                }
                break;
        }

        if(deathTimer == 0)
        {
            deathFlag = true;
        }
    }
}

public enum BlobElement {STONE, FIRE, WATER, GRASS, BOX, ICE, BOX_BURNING}
public enum Direction {UP, RIGHT, DOWN, LEFT}


