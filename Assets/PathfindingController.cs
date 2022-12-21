using Assets.Scripts.Grid_System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using TreeEditor;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PathfindingController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private Vector3 origin;
    [SerializeField]
    private Vector3 nextStop;
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private List<Vector3> nodeCoordinates;

    private bool hasArrived;

    private int pathIndex;

    private void Start()
    {
        hasArrived = transform.position == destination;
        nodeCoordinates = new List<Vector3>();
    }

    private void Update()
    {
        if (nodeCoordinates.Any())
        {
            TraversePath();
        }
    }

    private void TraversePath()
    {

        Vector3 targetPosition = nodeCoordinates[pathIndex];

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            transform.position = transform.position + speed * Time.deltaTime * moveDir;
        }
        else
        {
            transform.position = targetPosition;
            pathIndex++;
            if (pathIndex >= nodeCoordinates.Count)
            {
                nodeCoordinates.Clear();
            }
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        pathIndex = 0;
        nodeCoordinates = Pathfinding.Instance.FindPath(transform.position, targetPosition);

        if (nodeCoordinates != null)
        {
            origin = nodeCoordinates.First();
            destination = nodeCoordinates.Last();
        }
    }
}
