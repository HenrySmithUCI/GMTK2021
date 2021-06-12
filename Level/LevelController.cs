using Godot;
using System;
using System.Collections.Generic;

public class LevelController : TileMap
{
    [Export]
    public PackedScene playerScene = null;
    [Export]
    public PackedScene blobScene = null;

    public Tile[,] tiles;
    public List<EntityData> entities;
    public Dictionary<EntityData, Node2D> entityNodes;
    public PlayerData player;
    public int turnNumber = 0;

    // player is always entities[0]
    //public BoxData[] entities;

    // entities that are under control
    //public List<BoxData> active = new List<BoxData>();

    //public static int gridSize = 16;

    public int width;
    public int height;

    protected void setUpLevel(int[,] walls)
    {
        height = walls.GetLength(0);
        width = walls.GetLength(1);
        tiles = new Tile[width, height];
        entityNodes = new Dictionary<EntityData, Node2D>();

        // turn info from walls into auto tile map
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (walls[j, i] == 1)
                {
                    tiles[i,j] = new Tile(true);
                    SetCell(i, j, 1);
                }
                if(walls[j,i] == 0)
                {
                    tiles[i,j] = new Tile(false);
                    SetCell(i,j, 0);
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
                if(entity is PlayerData p)
                {
                    player = p;
                    blob = (Blob)playerScene.Instance();
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

        /*
        // turn info from entities into sprites on map
        foreach (var b in entities)
        {
            Box box = (Box)boxScene.Instance();
            box.setTexture(b.id);
            box.GlobalPosition = (b.position * gridSize).Snapped(new Vector2(gridSize, gridSize));
            AddChild(box);
            b.box = box;
        }

        active.Add(entities[0]);
        */
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
        if (player != null)
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

    private HashSet<BlobData> getActiveBlobs()
    {
        HashSet<BlobData> connected = new HashSet<BlobData>() {};
        List<BlobData> fronteir = new List<BlobData>() {player};
        HashSet<BlobData> seen = new HashSet<BlobData>() {player};
        while(fronteir.Count > 0)
        {
            BlobData data = fronteir[0];
            fronteir.RemoveAt(0);
            connected.Add(data);
            //GD.Print(data.element);
            //GD.Print(data.countConnections());
            foreach(BlobData check in data.connected)
            {
                if(check != null && seen.Contains(check) == false && check.deathFlag == false)
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

        bool canMove = true;
        HashSet<BlobData> moving = getActiveBlobs();

        foreach (BlobData b in moving)
        {
            if (b.element != BlobElement.ICE)
            {
                Vector2 nextPos = b.position + dir;


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
                if (b.element != BlobElement.ICE)
                {
                    Tile tile = getTile(b.position);
                    if (tile.entity == b)
                        tile.entity = null;
                    Vector2 nextPos = b.position + dir;
                    b.position = nextPos;
                    getTile(b.position).entity = b;
                    ((Blob)entityNodes[b]).Move(dir);
                }

                //b.move(nextPos);

                //attachNeighbors(nextPos);
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

                if(entity == player)
                {
                    player = null;
                }
            }
        }
    }

    // checks if a specific tile is empty
    // returns true if NO wall or entity is in that tile
    // returns false if a wall or entity IS in the tile
    private bool checkFree(Vector2 pos, HashSet<BlobData> active)
    {
        // if move outside of bounds of level
        //if(pos.x >= walls.GetLength(0) || pos.y >= walls.GetLength(1) ||
        //   pos.x < 0 || pos.y < 0)
        //{
        //    return false;
        //}

        Tile tile = getTile(pos);
        if(tile == null || tile.isBlock)
            return false;
        if(tile.entity != null)
        {
            if(tile.entity is BlobData blob)
            {
                return active.Contains(blob) && blob.element != BlobElement.ICE;
            }
        }
        return true;

        // if there is an entity in the way
        //for(int i = 0; i < entities.Length; i++)
        //{
        //    if(entities[i].position == pos && !active.Contains(entities[i]))
        //    {
        //        return false;
        //    }
        //}
    }

    // attach any neighbors to active
    private void attachNeighbors(Vector2 pos)
    {
        //foreach(BoxData b in entities)
        //{
        //    if(b.position.DistanceTo(pos) < 1.1f && !active.Contains(b))
        //    {
        //        active.Add(b);
        //    }
        //}
    }
}
