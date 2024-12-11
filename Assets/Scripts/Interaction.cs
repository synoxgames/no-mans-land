using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Interaction : MonoBehaviour
{
 
    // This will be used to check whether or not the player should be locked in-place
    public abstract bool LockInPlayer();

    // This will be used to trigger an action when interacted with
    public abstract void TriggerInteraction();

    // This will be used to unlock the player and reset the interation handler
    public abstract void ResetInteraction();

    // This returns the interaction script currently used
    public Interaction GetInteraction() { return this; }

}
