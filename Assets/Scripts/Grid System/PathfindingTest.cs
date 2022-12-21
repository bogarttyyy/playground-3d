using Assets.Scripts.Grid_System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PathfindingTest : MonoBehaviour
{
    private Pathfinding pathfinding;

    [SerializeField]
    private PathfindingController characterPathfinding;

    private void Start()
    {
        pathfinding = new(10, 10, transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = CommonHelper.GetMouseWorldPosition();
            var grid = pathfinding.GetGrid();
            grid.GetXY(mouseWorldPosition, out int x, out int y);

            //Debug.Log($"{x} - {y}");

            List<PathNode> path = pathfinding.FindPath(0,0, x,y);

            //foreach (PathNode node in path)
            //{
            //    Debug.Log($"{node.x} - {node.y}");
            //}

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 factor = Vector3.one * 5f;
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + factor, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + factor, Color.green, 5f);
                }
            }

            characterPathfinding.SetTargetPosition(CommonHelper.GetMouseWorldPosition());
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = CommonHelper.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).ToggleIsWalkable();
        }
    }
}
