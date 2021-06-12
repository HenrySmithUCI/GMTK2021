/*
using Godot;
using System;

public class BoxData
{
    public int id;
    public Vector2 position;
    public int size;
    public Box box;

    public BoxData(int setId, Vector2 pos, int setSize)
    {
        id = setId;
        position = pos;
        size = setSize;
    }

    public void move(Vector2 nextPos)
    {
        position = nextPos;

        if(box != null)
        {
            box.GlobalPosition = (nextPos * size).Snapped(new Vector2(size, size));
        }
    }
}
*/