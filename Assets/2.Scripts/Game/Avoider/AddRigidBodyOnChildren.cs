using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidBodyOnChildren : MonoBehaviour
{
    public int num_of_EdgePoints = 10;
    List<Transform> oneStageChildren;
    private void Start()
    {
        NewEdge.AlwaysUpdateLine = true;
        oneStageChildren = new List<Transform>();
        var children = gameObject.GetComponentsInChildren<Transform>();
        foreach(var child in children)
        {
            if(child.parent == transform)
            {
                oneStageChildren.Add(child);
                Rigidbody2D rigid2D = child.GetComponent<Rigidbody2D>();
                Collider2D coll2D = child.GetComponent<Collider2D>();
                if (rigid2D == null)
                    rigid2D = child.gameObject.AddComponent<Rigidbody2D>();
                rigid2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                if (coll2D == null)
                    coll2D = child.gameObject.AddComponent<Collider2D>();
                coll2D.isTrigger = false;
            }
        }
        var edges = FindObjectsOfType<NewEdge>();
        foreach(var edge in edges)
        {
            edge.UpdatePointSize(num_of_EdgePoints);
        }
    }
    public float resetNodeYThreshold = 10;
    private void Update()
    {
        foreach(var child in oneStageChildren){
            if(child.position.y < resetNodeYThreshold)
            {
                child.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                child.position = transform.position;
            }
        }
    }
}
