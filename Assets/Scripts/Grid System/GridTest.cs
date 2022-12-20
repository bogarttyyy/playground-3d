using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    private Grid<SpecialGridObject> grid;

    void Start()
    {
        grid = new(8, 8, 10f, Vector3.zero, (grid, x, y) => new SpecialGridObject(grid, x, y));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SpecialGridObject gridObject = grid.GetGridObject(mousePos) as SpecialGridObject;
            if (gridObject != null)
            {
                Debug.Log(gridObject.GetDebugDisplay());
                gridObject.AddValue();
                grid.GridObjectHasChanged(mousePos);
            }
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.Log(grid.GetValue(mousePos));
        //}
    }
}

public class SpecialGridObject : IGridObject
{
    private Grid<SpecialGridObject> grid;
    private int x;
    private int y;
    private int value = 0;

    public SpecialGridObject(Grid<SpecialGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        value = 0;
    }

    public void AddValue()
    {
        value += 1;
        grid.GridObjectHasChanged(x, y);
    }

    public string GetDebugDisplay()
    {
        return value.ToString();
    }
}