using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutScript : Interaction
{
    public GameObject cuttingMinigame;
    public bool inInteraction = false;
    ThoughtDialogue reason;

    private void Awake() {
        reason = GetComponent<ThoughtDialogue>();
    }

    public override bool LockInPlayer() {
        return TopDownMovement.instance.CheckForItem("axe");
    }

    public override void TriggerInteraction() {
       if (!TopDownMovement.instance.CheckForItem("axe")) {
            reason.TriggerInteraction();
        } else {
            cuttingMinigame.SetActive(true);
        }
    }

    public void Update() {
        if (InteractionHandler.instance.CompareInteractions(this) && cuttingMinigame.GetComponent<TreeChopMinigame>().hasFinished) {
            ResetInteraction();
        }
    }

    public override void ResetInteraction() {
        InteractionHandler.instance.ResetInteraction();
    }
}
