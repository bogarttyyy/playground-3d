using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Assets.Scripts.Grid_System
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public static Pathfinding Instance { get; private set; }

        private Grid<PathNode> grid;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        public Pathfinding(int width, int height, Vector3 position)
        {
            Instance = this;
            grid = new Grid<PathNode>(width, height, 10f, position,
                (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y)
            );
        }

        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            grid.GetXY(startWorldPosition, out int startX, out int startY);
            grid.GetXY(endWorldPosition, out int endX, out int endY);

            List<PathNode> path = FindPath(startX, startY, endX, endY);
            
            if (path == null)
            {
                return null;
            }

            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * 5f);
            }

            return vectorPath;
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetGridObject(startX, startY);
            PathNode endNode = grid.GetGridObject(endX, endY);

            openList = new List<PathNode>() { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);

                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.prevNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode))
                    {
                        continue;
                    }

                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int calculatedDistance = CalculateDistanceCost(currentNode, neighbourNode);
                    int tentativeGCost = currentNode.gCost + calculatedDistance;
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.prevNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = calculatedDistance;
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new();

            if (currentNode.x - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
                if (currentNode.y - 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                }
                if (currentNode.y + 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                }
            }

            if (currentNode.x + 1 < grid.GetWidth())
            {

                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
                if (currentNode.y - 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                }
                if (currentNode.y + 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                }
            }

            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
            }

            if (currentNode.y + 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
            }

            return neighbourList;
        }

        public PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.prevNode != null)
            {
                path.Add(currentNode.prevNode);
                currentNode = currentNode.prevNode;
            }

            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);

            int remaining = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];

            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }

        public Grid<PathNode> GetGrid()
        {
            return grid;
        }
    }
}