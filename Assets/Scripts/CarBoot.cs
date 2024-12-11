using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBoot : Interaction
{
    public string itemToGet = "";
    public string toPrint = "";
    bool isOpen = false;
    bool pickedUpItem = false;
    Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    public override void ResetInteraction() {
        InteractionHandler.instance.ResetInteraction();
    }

    public override void TriggerInteraction() {
        isOpen = !isOpen;
        anim.SetBool("isOpen", isOpen);

        if (isOpen && !pickedUpItem) {
            pickedUpItem = true;
            TopDownMovement.instance.AddItem(itemToGet);
            StartCoroutine(GetComponent<ThoughtDialogue>().PrintText(toPrint));
        }

        ResetInteraction();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool LockInPlayer() {
        return false;
    }
}
