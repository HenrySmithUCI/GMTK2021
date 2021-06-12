using Godot;
using System;
using System.Collections.Generic;

public class LevelController : TileMap
{
    [Export]
    public PackedScene blobScene = null;

    public Tile[,] tiles;
    public List<EntityData> entities;
    public Dictionary<EntityData, Node2D> entityNodes;
    public List<BlobData> players;
    public int turnNumber = 0;

    // player is always entities[0]
    //public BoxData[] entities;

    // entities that are under control
    //public List<BoxData> active = new List<BoxData>();

    //public static int gridSize = 16;

    public int width = 20;
    public int height = 11;

    protected void setUpLevel(string levelString)
    {
        tiles = new Tile[width, height];
        entityNodes = new Dictionary<EntityData, Node2D>();
        entities = new List<EntityData>();
        players = new List<BlobData>();

        // turn info from walls into auto tile map
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                char c = levelString[y*width+x];
                switch(c)
                {
                    case ' ':
                        tiles[x,y] = new Tile(false);
                        SetCell(x, y, 0);
                    break;
                    case '0':
                        tiles[x,y] = new Tile(true);
                        SetCell(x, y, 1);
                        break;
                    case 'g':
                    case 'w':
                    case 'f':
                    case 's':
                    case 'i':
                    case 'b':
                        entities.Add(new BlobData(new Vector2(x,y), charToElement(c)));
                        tiles[x,y] = new Tile(false);
                        SetCell(x, y, 0);
                        break;
                    case 'G':
                    case 'W':
                    case 'F':
                    case 'S':
                        entities.Add(new BlobData(new Vector2(x,y), charToElement(char.ToLower(c)), true));
                        tiles[x,y] = new Tile(false);
                        SetCell(x, y, 0);
                        break;
                }
            }
        }
        for(int i = -1; i <= width; ++i)
        {
            SetCell(i, -1, 1);
            SetCell(i, height,1);
        }
        for(int j = -1; j <= height; ++j)
        {
            SetCell(-1, j,1);
            SetCell(width, j,1);
        }
        UpdateBitmaskRegion();

        foreach(var entity in entities)
        {
            entity.level = this;
            if(entity is BlobData bl)
            {
                Blob blob;
                if(bl.isPlayer)
                {
                    players.Add(bl);
                    blob = (Blob)blobScene.Instance();
                }
                else
                {
                    blob = (Blob)blobScene.Instance();
                }
                blob.data = bl;
                AddChild(blob);
                entityNodes[entity] = blob;
            }
            getTile(entity.position).entity = entity;
        }
    }

    public BlobElement charToElement(char type)
    {
        switch(type)
        {
            case 'f':
                return BlobElement.FIRE;
            case 'w':
                return BlobElement.WATER;
            case 'g':
                return BlobElement.GRASS;
            case 's':
                return BlobElement.STONE;
            case 'i':
                return BlobElement.ICE;
            case 'b':
                return BlobElement.BOX;
            default:
                return BlobElement.STONE;
        }
    }

    public Tile getTile(Vector2 tilePos)
    {
        if(tilePos.x < 0 || tilePos.x >= width || tilePos.y < 0 || tilePos.y >= height)
        {
            return null;
        }
        return tiles[(int)tilePos.x, (int)tilePos.y];
    }

    public override void _Process(float delta)
    {
        if (players.Count > 0)
        {
            if (Input.IsActionJustPressed("right"))
            {
                move(new Vector2(1, 0));
            }
            else if (Input.IsActionJustPressed("left"))
            {
                move(new Vector2(-1, 0));
            }

            if (Input.IsActionJustPressed("up"))
            {
                move(new Vector2(0, -1));
            }
            else if (Input.IsActionJustPressed("down"))
            {
                move(new Vector2(0, 1));
            }
        }
    }

    private HashSet<BlobData> getActiveBlobs(int i)
    {
        HashSet<BlobData> connected = new HashSet<BlobData>() {};
        List<BlobData> fronteir = new List<BlobData>() {players[i]};
        HashSet<BlobData> seen = new HashSet<BlobData>() {players[i]};
        while(fronteir.Count > 0)
        {
            BlobData data = fronteir[0];
            fronteir.RemoveAt(0);
            connected.Add(data);
            foreach(BlobData check in data.connected)
            {
                if(check != null && seen.Contains(check) == false && check.deathFlag == false 
                   && check.element != BlobElement.ICE
                   && check.element != BlobElement.NEW_ICE
                   && check.element != BlobElement.BOX_BURNING)
                {
                    fronteir.Add(check);
                    seen.Add(check);
                }
            }
        }
        return connected;
    }

    // should handle combining, only handles player movement atm
    private void move(Vector2 dir)
    {
        turnNumber += 1;

        HashSet<BlobData> seen = new HashSet<BlobData>();

        for(int i = 0; i < players.Count; ++i)
        {
            if(seen.Contains(players[i]))
                continue;
            
            bool canMove = true;
            HashSet<BlobData> moving = getActiveBlobs(i);

            foreach (BlobData b in moving)
            {
                if (b.element != BlobElement.ICE && b.element != BlobElement.NEW_ICE && b.element != BlobElement.BOX_BURNING)
                {
                    Vector2 nextPos = b.position + dir;

                    if (b.isPlayer)
                    {
                        seen.Add(b);
                    }

                    if (checkFree(nextPos, moving) == false)
                    {
                        canMove = false;
                        break;
                    }
                }
            }

            if (canMove)
            {
                foreach(BlobData b in moving)
                {
                    if (b.element != BlobElement.ICE && b.element != BlobElement.NEW_ICE && b.element != BlobElement.BOX_BURNING)
                    {
                        Tile tile = getTile(b.position);
                        if (tile.entity == b)
                            tile.entity = null;
                        Vector2 nextPos = b.position + dir;
                        b.position = nextPos;
                        getTile(b.position).entity = b;
                        ((Blob)entityNodes[b]).Move(dir);
                    }
                }
            }
        }

        foreach(EntityData entity in entities)
        {
            entity.UpdateTurn();
        }

        List<EntityData> dupe = new List<EntityData>(entities);
        foreach(EntityData entity in dupe)
        {
            if (entity.deathFlag == true)
            {
                getTile(entity.position).entity = null;
                entityNodes[entity].Call("die");
                entityNodes.Remove(entity);
                entities.Remove(entity);

                if(entity.isPlayer && players.Contains((BlobData)entity))
                {
                    players.Remove((BlobData)entity);
                }
            }
        }
    }

    // checks if a specific tile is empty
    // returns true if NO wall or entity is in that tile
    // returns false if a wall or entity IS in the tile
    private bool checkFree(Vector2 pos, HashSet<BlobData> active)
    {
        Tile tile = getTile(pos);
        if(tile == null || tile.isBlock)
            return false;
        if(tile.entity != null)
        {
            if(tile.entity is BlobData blob)
            {
                return active.Contains(blob) && blob.element != BlobElement.ICE && blob.element != BlobElement.NEW_ICE && blob.element != BlobElement.BOX_BURNING;
            }
        }
        return true;
    }
}
