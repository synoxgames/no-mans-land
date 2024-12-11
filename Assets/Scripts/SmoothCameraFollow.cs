using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmoothCameraFollow : MonoBehaviour
{
    public float smoothSpeed = 10;
    public float dampen;
    public Vector3 offset;
    Transform target;

    private void Awake() {
        if (!GameObject.FindWithTag("Player")) return;
        target = GameObject.FindWithTag("Player").transform;
    }

    public void LateUpdate() {
        if (!target) return;
        Vector2 toMove = offset + target.position;
        Vector2 smoothedMove = Vector2.Lerp(transform.position, toMove, smoothSpeed * Time.deltaTime / dampen);
        transform.position = (Vector3)smoothedMove + offset;
    }
}
