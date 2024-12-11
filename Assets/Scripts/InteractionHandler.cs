using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public LayerMask interactionLayer;
    public bool inInteraction = false;
    public float checkRadius = .25f;
    public float checkDistance = 3f;
    public float cooldown = 1;

    Interaction currentInteraction;
    bool inCooldown = false;
    float timer;

    public static InteractionHandler instance;

    public void Awake() {
        if (instance != this) Destroy(instance);
        instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && !inInteraction && !inCooldown) {
            CheckForInteraction();
        }

        if (inCooldown) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                inCooldown = false;
                timer = 0;
            }
        }

        if (currentInteraction == null && inInteraction) ResetInteraction();
    }

    public void CheckForInteraction() {
        Transform hitItem = Physics2D.CircleCast(transform.position, checkRadius, TopDownMovement.GetXYInt(), checkDistance, interactionLayer).transform;
        if (hitItem == null || hitItem.tag== "Wall") return;

        StartNewInteraction(hitItem.GetComponent<Interaction>().GetInteraction());
    }

    public void StartNewInteraction(Interaction toAdd) {
        if (inInteraction) return;

        if (toAdd.LockInPlayer()) {
            inInteraction = true;
            currentInteraction = toAdd;
            currentInteraction.TriggerInteraction();
        } else {
            toAdd.TriggerInteraction();
        }

        timer = cooldown;
        inCooldown = true;

    }

    public void ResetInteraction() {
        currentInteraction = null;
        inInteraction = false;
        inCooldown = true;
        timer = cooldown;
    }

    public bool CompareInteractions(Interaction toCheck) {
        return toCheck == currentInteraction;
    }
}
