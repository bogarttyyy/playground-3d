using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IGridObject
{
    public Grid<PathNode> grid;
    public int x;
    public int y;

    public int fCost;
    public int gCost;
    public int hCost;

    public bool isWalkable;
    public PathNode prevNode;

    public PathNode(Grid<PathNode> grid, int width, int height)
    {
        x = width;
        y = height;
        this.grid = grid;

        isWalkable = true;
    }

    public override string ToString()
    {
        return $"{x},{y}";
    }

    public string GetDebugDisplay()
    {
        if (isWalkable)
        {
            return $"{x},{y}";
        }

        return "X";
    }

    internal void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    internal void ToggleIsWalkable()
    {
        isWalkable = !isWalkable;
        grid.GridObjectHasChanged(x, y);
    }

    internal Vector3 GetWorldPosition()
    {
        return new Vector3(x, y);
    }
}
