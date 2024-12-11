 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interaction
{
    public string itemName;
    public string toPrint;
        
    public override bool LockInPlayer() {
        return false;
    }

    public override void TriggerInteraction() {
        TopDownMovement.instance.AddItem(itemName);
        StartCoroutine(FindObjectOfType<ThoughtDialogue>().PrintText(toPrint));
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 7);
    }

    public override void ResetInteraction() {
        throw new System.NotImplementedException();
    }
}
