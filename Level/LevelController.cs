using Godot;
using System;
using System.Collections.Generic;

public class LevelController : TileMap
{
    public static int levelNum;

    [Export]
    public PackedScene blobScene = null;

    public Tile[,] tiles;
    public List<EntityData> entities;
    public Dictionary<EntityData, Node2D> entityNodes;
    public List<BlobData> players;
    public int turnNumber = 0;
    public List<string> undoList = new List<string>();
    public string currentLevel;
    public float undoTimer = 0.5f;
    public bool victory = false;

    protected AudioStreamPlayer moveSound;

    public int width = 20;
    public int height = 11;

    public override void _Ready()
    {
        moveSound = GetNode<AudioStreamPlayer>("MoveSound");
    }

    public void destroyLevel()
    {
        foreach(var key in entityNodes)
        {
            key.Value.QueueFree();
        }
    }

    public void undo()
    {
        if(turnNumber == 0)
            return;
        turnNumber -= 1;
        destroyLevel();
        string temp = undoList[0].Replace('w','*');
        setUpLevel(temp, false);
        undoList.RemoveAt(0);
    }

    public string levelToString()
    {
        string ret = "";
        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                Tile t = getTile(new Vector2(x,y));
                if(t.type != TileType.NONE)
                {
                    switch(t.type)
                    {
                        case TileType.BLOCK:
                            ret += "0";
                            break;
                        case TileType.VICTORY:
                            ret += "v";
                            break;
                        case TileType.VICTORY_PLAYER:
                            ret += "V";
                            break;
                        case TileType.CONVERT_FIRE:
                            ret += "1";
                            break;
                        case TileType.CONVERT_GRASS:
                            ret += "3";
                            break;
                        case TileType.CONVERT_WATER:
                            ret += "2";
                            break;
                    }
                }
                else if(t.entity == null)
                    ret += " ";
                else
                {
                    BlobData blob = (BlobData)t.entity;
                    char c;
                    switch(blob.element)
                    {
                        case BlobElement.BOX:
                            c = 'b';
                            break;
                        case BlobElement.FIRE:
                            c = 'f';
                            break;
                        case BlobElement.WATER:
                            c = 'w';
                            break;
                        case BlobElement.PRE_WATER:
                            c = '*';
                            break;
                        case BlobElement.STONE:
                            c = 's';
                            break;
                        case BlobElement.GRASS:
                            c = 'g';
                            break;
                        case BlobElement.ICE:
                            c = 'i';
                            break;
                        case BlobElement.BOX_BURNING:
                            c = ']';
                            break;
                        case BlobElement.GRASS_BURNING:
                            c ='[';
                            break;
                        case BlobElement.NEW_ICE:
                            c ='\\';
                            break;
                        case BlobElement.BOX_BURNING_INIT:
                            c='~';
                            break;
                        case BlobElement.GRASS_BURNING_INIT:
                            c='`';
                            break;
                        default:
                            c = 's';
                            break;
                        
                    }
                    if(t.entity.isPlayer)
                    {
                        c = char.ToUpper(c);
                    }
                    ret += c;
                }
            }
        }
        return ret;
    }

    protected void setUpLevel(string levelString, bool newLevel = true)
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
                        tiles[x,y] = new Tile(TileType.NONE);
                        if(newLevel)
                            SetCell(x, y, 0);
                    break;
                    case '0':
                        tiles[x,y] = new Tile(TileType.BLOCK);
                        if(newLevel)
                            SetCell(x, y, 1);
                        break;
                    case 'v':
                        tiles[x,y] = new Tile(TileType.VICTORY);
                        if(newLevel)
                            SetCell(x, y, 2);
                        break;
                    case 'V':
                        tiles[x,y] = new Tile(TileType.VICTORY_PLAYER);
                        if(newLevel)
                            SetCell(x, y, 3);
                        break;
                    case '1':
                        tiles[x,y] = new Tile(TileType.CONVERT_FIRE);
                        if(newLevel)
                            SetCell(x, y, 6);
                        break;
                    case '2':
                        tiles[x,y] = new Tile(TileType.CONVERT_WATER);
                        if(newLevel)
                            SetCell(x, y, 5);
                        break;
                    case '3':
                        tiles[x,y] = new Tile(TileType.CONVERT_GRASS);
                        if(newLevel)
                            SetCell(x, y, 4);
                        break;
                    case 'g':
                    case 'w':
                    case 'f':
                    case 's':
                    case 'i':
                    case '*':
                    case 'b':
                    case ']':
                    case '[':
                    case '\\':
                    case '`':
                    case '~':
                        entities.Add(new BlobData(new Vector2(x,y), charToElement(c)));
                        tiles[x,y] = new Tile(TileType.NONE);
                        if(newLevel)
                            SetCell(x, y, 0);
                        break;
                    case 'G':
                    case 'W':
                    case 'F':
                    case 'S':
                        entities.Add(new BlobData(new Vector2(x,y), charToElement(char.ToLower(c)), true));
                        tiles[x,y] = new Tile(TileType.NONE);
                        if(newLevel)
                            SetCell(x, y, 0);
                        break;
                }
            }
        }
        if(newLevel)
        {
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
        }
        if(newLevel)
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

        updateEntities();
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
            case ']':
                return BlobElement.BOX_BURNING;
            case '[':
                return BlobElement.GRASS_BURNING;
            case '\\':
                return BlobElement.NEW_ICE;
            case '~':
                return BlobElement.BOX_BURNING_INIT;
            case '`':
                return BlobElement.GRASS_BURNING_INIT;
            case '*':
                return BlobElement.PRE_WATER;
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
        if(victory)
            return;
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
            else if (Input.IsActionJustPressed("step"))
            {
                move(new Vector2(0, 0));
            }
        }

        if(Input.IsActionJustPressed("undo"))
        {
            undo();
        }
        if(Input.IsActionPressed("undo"))
        {
            undoTimer -= delta;
            if(undoTimer <= 0)
            {
                undo();
                undoTimer += 0.1f;
            }
        }
        else
        {
            undoTimer = 0.5f;
        }
        if(Input.IsActionJustPressed("restart"))
        {
            turnNumber = 0;
            destroyLevel();
            setUpLevel(currentLevel, false);
            undoList = new List<string>();
        }
        if(Input.IsActionJustPressed("menu"))
        {
            GetTree().ChangeScene("res://Level/LevelSelect.tscn");
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
                   && check.element != BlobElement.BOX_BURNING
                   && check.element != BlobElement.BOX_BURNING_INIT)
                {
                    fronteir.Add(check);
                    seen.Add(check);
                }
            }
        }
        return connected;
    }

    public void updateEntities()
    {
        foreach(EntityData entity in entities)
        {
            entity.PreUpdate();
        }

        foreach(EntityData entity in entities)
        {
            entity.UpdateTurn();
        }

        foreach(EntityData entity in entities)
        {
            entity.PostUpdate();
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

    // should handle combining, only handles player movement atm
    protected void move(Vector2 dir)
    {
        turnNumber += 1;
        moveSound.Playing = true;

		undoList.Insert(0, levelToString());
        HashSet<BlobData> seen = new HashSet<BlobData>();

        for(int i = 0; i < players.Count; ++i)
        {
            if(seen.Contains(players[i]))
                continue;
            
            bool canMove = true;
            HashSet<BlobData> moving = getActiveBlobs(i);

            foreach (BlobData b in moving)
            {
                if (b.element != BlobElement.ICE && b.element != BlobElement.NEW_ICE && b.element != BlobElement.BOX_BURNING && b.element != BlobElement.BOX_BURNING_INIT)
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
                    if (b.element != BlobElement.ICE && b.element != BlobElement.NEW_ICE && b.element != BlobElement.BOX_BURNING && b.element != BlobElement.BOX_BURNING_INIT)
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

        updateEntities();

        if(checkVictory())
        {
            victory = true;
            UI.instance.showWin();
        }
    }

    public bool checkVictory()
    {
        bool victory = true;
        bool tilesExist = false;
        foreach(Vector2 vec in GetUsedCellsById((int)TileType.VICTORY))
        {
            tilesExist = true;
            if(getTile(vec).entity == null || getTile(vec).entity.isPlayer)
            {
                victory = false;
                break;
            }
        }
        if(victory)
            foreach(Vector2 vec in GetUsedCellsById((int)TileType.VICTORY_PLAYER))
            {
                tilesExist = true;
                if(getTile(vec).entity == null || getTile(vec).entity.isPlayer == false)
                {
                    victory = false;
                    break;
                }
            }
        return victory && tilesExist;
    }

    // checks if a specific tile is empty
    // returns true if NO wall or entity is in that tile
    // returns false if a wall or entity IS in the tile
    private bool checkFree(Vector2 pos, HashSet<BlobData> active)
    {
        Tile tile = getTile(pos);
        if(tile == null || tile.type == TileType.BLOCK)
            return false;
        if(tile.entity != null)
        {
            if(tile.entity is BlobData blob)
            {
                return active.Contains(blob) && blob.element != BlobElement.ICE && blob.element != BlobElement.NEW_ICE && blob.element != BlobElement.BOX_BURNING && blob.element != BlobElement.BOX_BURNING_INIT;
            }
        }

        return true;
    }
}
