using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawn : MonoBehaviour
{
    public GameObject[] allTrees;
    public LayerMask tree;
    public Vector2 xPoint;
    public Vector2 yPoint;
    public int toSpawn;
    Vector2 offset;

    public void Start() {

        offset = transform.position;

        for (int i = 0; i <= toSpawn; i++) {
            Vector3 toSpawn = new Vector3(Random.Range(xPoint.x, xPoint.y), Random.Range(yPoint.x, yPoint.y), 1) + (Vector3)offset;
            bool hit = Physics2D.CircleCast(toSpawn, .005f, transform.up, 0.01f, tree).transform != null;
            if (!hit) Instantiate(allTrees[Random.Range(0, allTrees.Length)], toSpawn, Quaternion.identity, transform);
            else if (hit)
            continue;

            
        }
    }
}
