using System;
using UnityEngine;

public class Grid<T> where T : IGridObject
{
    public event EventHandler<OnGridChangeEventArgs> OnGridChanged;

    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private T[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<T>, int, int ,T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new T[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = createGridObject(this, i, j);
            }
        }

        bool showDebug = true;

        if (showDebug)
        {
            debugTextArray = new TextMesh[width, height];

            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    debugTextArray[i, j] = CreateWorldText(gridArray[i, j].GetDebugDisplay(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, null, TextAnchor.MiddleCenter);

                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridChanged += Grid_OnGridChanged;
        }
    }

    private void Grid_OnGridChanged(object sender, OnGridChangeEventArgs e)
    {
        debugTextArray[e.x, e.y].text = gridArray[e.x,e.y]?.GetDebugDisplay();
    }

    public void GridObjectHasChanged(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        GridObjectHasChanged(x, y);
    }

    public void GridObjectHasChanged(int x, int y)
    {
        OnGridChanged?.Invoke(this, new OnGridChangeEventArgs { x = x, y = y });
    }

    public IGridObject GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }

    public T GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }

        return default;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    #region Helpers

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (!color.HasValue)
        {
            color = Color.white;
        }
        return CreateWorldText(parent, text, localPosition, fontSize, color.Value, textAnchor, textAlignment, sortingOrder);
    }

    private static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment alignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.text = text;
        textMesh.alignment = alignment;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;
    }

    public class OnGridChangeEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    #endregion
}

public interface IGridObject
{
    public string GetDebugDisplay();
}
