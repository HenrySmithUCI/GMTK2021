using Godot;
using System;
using System.Collections.Generic;

public class LevelController : TileMap
{
    [Export]
    public PackedScene boxScene = null;

    public int[,] walls;

    // player is always entities[0]
    public BoxData[] entities;

    // entities that are under control
    public List<BoxData> active = new List<BoxData>();

    public static int gridSize = 16;

    protected void setUpLevel()
    {
        // turn info from walls into auto tile map
        for(int i = 0; i < walls.GetLength(0); i++)
        {
            for(int j = 0; j < walls.GetLength(1); j++)
            {
                if (walls[i, j] == 1)
                {
                    SetCell(j, i, 0);
                }
            }
        }

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
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("right"))
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

    // should handle combining, only handles player movement atm
    private void move(Vector2 dir)
    {
        bool canMove = true;
        List<BoxData> moving = new List<BoxData>(active);

        foreach (BoxData b in moving)
        {
            Vector2 nextPos = b.position + dir;

            if (!checkFree(nextPos))
            {
                canMove = false;
            }
        }

        if (canMove)
        {
            foreach (BoxData b in moving)
            {
                Vector2 nextPos = b.position + dir;

                b.move(nextPos);

                attachNeighbors(nextPos);
            }
        }
    }

    // checks if a specific tile is empty
    // returns true if NO wall or entity is in that tile
    // returns false if a wall or entity IS in the tile
    private bool checkFree(Vector2 pos)
    {
        // if move outside of bounds of level
        if(pos.x >= walls.GetLength(0) || pos.y >= walls.GetLength(1) ||
           pos.x < 0 || pos.y < 0)
        {
            return false;
        }

        // if there is a wall in the way
        // reversed to match the array position visually
        if(walls[(int)pos.y, (int)pos.x] == 1)
        {
            return false;
        }

        // if there is an entity in the way
        for(int i = 0; i < entities.Length; i++)
        {
            if(entities[i].position == pos && !active.Contains(entities[i]))
            {
                return false;
            }
        }

        return true;
    }

    // attach any neighbors to active
    private void attachNeighbors(Vector2 pos)
    {
        foreach(BoxData b in entities)
        {
            if(b.position.DistanceTo(pos) < 1.1f && !active.Contains(b))
            {
                active.Add(b);
            }
        }
    }
}
