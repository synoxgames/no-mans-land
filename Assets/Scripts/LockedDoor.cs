using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Interaction
{
    public GameObject lockpickObject;
    public bool hasUnlocked = false;
    public string keyName = "";
    public bool hasKey = false;
    public Animator roof;
    LockpickController lockPick;
    ThoughtDialogue reason;
    Animator doorAnim;

    bool isOpen;

    private void Start() {
        lockPick = lockpickObject.GetComponent<LockpickController>();
        doorAnim = GetComponent<Animator>();
        hasUnlocked = false;
        reason = GetComponent<ThoughtDialogue>();
    }

    public override void ResetInteraction() {
        TopDownMovement.instance.canWalk = true;
        lockpickObject.SetActive(false);
        InteractionHandler.instance.ResetInteraction();
    }

    private void Update() {
        if (!InteractionHandler.instance.CompareInteractions(this)) return;

        if (lockPick.hasSolved && !hasUnlocked) {
            hasUnlocked = true;
            isOpen = true;
            doorAnim.SetBool("isOpen", isOpen);
            if (roof != null) roof.SetBool("hasDone", true);
            ResetInteraction();
        }
    }

    public override void TriggerInteraction() {
        if (!TopDownMovement.instance.CheckForItem("lockpick")) {
            reason.TriggerInteraction();
            return;
        }

        if (hasKey && PlayerHasKey() && !hasUnlocked) {
            hasUnlocked = true;
            isOpen = true;
            doorAnim.SetBool("isOpen", isOpen);
            ResetInteraction();
            if (roof != null) roof.SetBool("hasDone", true);
            return;
        }

        if (!hasUnlocked) {
            lockpickObject.SetActive(true);
            TopDownMovement.instance.canWalk = false;
            lockPick.ResetLockpick(true);
        } else if (hasUnlocked){
            isOpen = !isOpen;
            doorAnim.SetBool("isOpen", isOpen);
        }
    }

    public override bool LockInPlayer() {
        return !hasUnlocked;
    }

    public bool PlayerHasKey() {
        return TopDownMovement.instance.CheckForItem(keyName);
    }
}
