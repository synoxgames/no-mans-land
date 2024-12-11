using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public int offset = 0;
    public Transform changePoint;
    float dist;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (TopDownMovement.instance == null) return;
        dist = (changePoint.position - TopDownMovement.instance.transform.position).y;

        if (dist > 10) return;

        if (dist > 0) {
            spriteRenderer.sortingOrder = 2 + offset;
        } else if (dist < 0) {
            spriteRenderer.sortingOrder = 5 + offset;
        }
    }
}
