using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPromptTrigger : MonoBehaviour
{
    public float fadeTime = 1.5f;
    public Color finishColour = new Color(1, 1, 1, .7f);
    public bool destroyOnInteract;

    SpriteRenderer img;
    bool inRange;

    private void Start() {
        img = GetComponent<SpriteRenderer>();
        img.color = Color.clear;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") inRange = false;
    }

    private void Update() {
        // If the player isn't near by, check to see if the image is invisible. If it isn't set it to invisible, then return out. Else just return out straight away
        if (!inRange) {
            if (img.color != Color.clear) img.color = Color.Lerp(img.color, Color.clear, fadeTime * Time.deltaTime);
            return;
        }

        if (img.color != finishColour) {
            img.color = Color.Lerp(img.color, finishColour, fadeTime * Time.deltaTime);
        } else return;

        if (inRange && Input.GetKeyDown(KeyCode.E) && destroyOnInteract) Destroy(gameObject);
    }

}
