using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTrigger : MonoBehaviour
{
    public List<TopDownMovement.AutomaticMovement> movements;
    public ThoughtDialogue reasoning;
    public bool destroyOnTouch = false;
    public bool checkForItem;
    public GameObject itemToCheckFor;
    public Interaction interactionTrigger;
    bool hasDone = false;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (hasDone && destroyOnTouch) return;

        if (collision.transform.tag == "Player") {

            if (checkForItem) {
                if (collision.transform.GetComponent<DragItem>().holdingItem.Equals(itemToCheckFor)) {
                    interactionTrigger.TriggerInteraction();
                    collision.transform.GetComponent<DragItem>().DestoryItem();
                    Destroy(reasoning);
                    Destroy(this);
                    return;
                }
            }


            if (destroyOnTouch) hasDone = true;
            TopDownMovement.instance.StartAutomaticMovement(movements);
            InteractionHandler.instance.StartNewInteraction(reasoning);
            if (destroyOnTouch) Destroy(gameObject, 10f);
        }
    }
}
