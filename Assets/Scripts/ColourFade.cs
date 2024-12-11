using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourFade : MonoBehaviour
{
    [Header("Image")]
    public Image toFade;
    [Header("Settings")]
    public float fadeTime = 3f;
    private float currentTime = 0.0f;
    public bool fade = true;

    public Color startColour = Color.black;
    public Color endColour = Color.clear;

    private void Update() {
        if (!toFade) return;

        if (fade) {
            toFade.color = Color.Lerp(startColour, endColour, currentTime / fadeTime);

            if (currentTime >= fadeTime) {
                toFade.color = endColour;
                currentTime = 0;
                fade = false;
            }

            currentTime += Time.deltaTime;
        }
    }

    public void StartFade() {
        fade = true;
    }

    public void StartFade(Color startColour, Color endColour) {
        this.startColour = startColour;
        this.endColour = endColour;
    }
}
