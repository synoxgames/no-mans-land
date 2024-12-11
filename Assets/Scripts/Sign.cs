using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interaction
{
    public Animator signAnim;
    public float waitAmount = 0.25f;    // Amount of seconds before the player can exit from the sign
    public Text signText;
    [TextArea(2,4)]
    public string toSay = "";

    bool isOpen = false;
    bool ready = false;                 // Whether or not the it is ready to be skipped
    float timer;

    private void Start() {
        signText = signAnim.GetComponentInChildren<Text>();

    }

    public void Update() {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.E) && isOpen) {
            if (ready) ResetInteraction();
        }

        if (timer < waitAmount) timer += Time.deltaTime;
        else ready = true;
    }


    public override void ResetInteraction() {
        signAnim.SetBool("isOpen", false);
        TopDownMovement.instance.canWalk = true;
        isOpen = false;
        timer = 0;
        ready = false;
        InteractionHandler.instance.ResetInteraction();
    }

    public override void TriggerInteraction() {
        signAnim.SetBool("isOpen", true);
        signText.text = toSay;
        isOpen = true;
        TopDownMovement.instance.canWalk = false;
    }

    public override bool LockInPlayer() {
        return true;
    }
}
