using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CommonHelper
{

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (!color.HasValue)
        {
            color = Color.white;
        }
        return CreateWorldText(parent, text, localPosition, fontSize, color.Value, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment alignment, int sortingOrder)
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

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = GetObjectWorldPosition(Input.mousePosition, Camera.main);
        mousePos.z = 0;
        return mousePos;
    }

    private static Vector3 GetMouseWorldPosition(Camera main)
    {
        return GetObjectWorldPosition(Input.mousePosition, main);
    }

    private static Vector3 GetObjectWorldPosition(Vector3 objectPosition, Camera main)
    {
        Vector3 worldPosition = main.ScreenToWorldPoint(objectPosition);
        return worldPosition;
    }
}