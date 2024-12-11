using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour
{
    [Header("Transform")]
    public Transform holdPoint;
    public Transform holdingTransform;
    public Vector2 grabOffset;

    [Header("Floats")]
    [Tooltip("Represented by a 0-100%")]
    public float speedReduction = 25f;
    [Range(0f, 1f)]
    public float holdOffsetStrength = .25f;
    public float checkLength = 1.25f;
    public float moveDelay = 1.25f;

    [Header("Boolean & Layer Mask")]
    public bool holdingItem = false;
    public LayerMask layerMask;

    public void Update() {

        if (holdingItem && holdingTransform == null) {
            DropItem();
        }
        
        if (Input.GetButtonDown("Jump")) {
            CheckForItem();
        } 

        if (Input.GetButtonUp("Jump")) {
            DropItem();
        }

        if (holdingItem) {
            holdingTransform.position = Vector2.Lerp(holdingTransform.position, (Vector2)holdPoint.position + grabOffset, moveDelay * Time.deltaTime);
        }
    }

    public void CheckForItem() {
        if (holdingItem) return;
        Vector2 pos = TopDownMovement.GetXYInt();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, pos, checkLength, layerMask);

        if (hit.transform != null) {
            holdingItem = true;
            grabOffset = TopDownMovement.GetXYInt() * holdOffsetStrength;
            TopDownMovement.instance.walkSpeed /= 1f + (speedReduction / 100f);
            holdingTransform = hit.transform;
            Physics2D.IgnoreCollision(hit.collider, TopDownMovement.instance.GetComponent<Collider2D>(), true);
        }
    }

    public void DropItem() {
        if (!holdingItem) return;

        holdingItem = false;
        TopDownMovement.instance.walkSpeed *= 1f + (speedReduction / 100f);
        Physics2D.IgnoreCollision(holdingTransform.GetComponent<Collider2D>(), TopDownMovement.instance.GetComponent<Collider2D>(), false);
        holdingTransform = null;
        grabOffset = Vector2.zero;
    }

    public void DestoryItem() {
        Destroy(holdingTransform.gameObject);
        DropItem();
    }
}
