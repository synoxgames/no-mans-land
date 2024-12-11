using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ThoughtDialogue : Interaction
{
    [Header("Dialogue Info")]
    public string[] allDialogue;
    public float textSpeed = 0.025f;
    public float clearDelay = 5f;
    public AudioClip talkSFX;
    public float talkSpeed = 0.1f;

    float timer;

    [Header("UI Info")]
    public Text dialogueArea;
    public Image expressionImage;
    public bool forceSceneChange = false;
    public int sceneIndex;
    AudioSource audSource;


    bool isPrinting;
    bool playSound;

    int dialogueCount = 0;

    private void Awake() {
        audSource = dialogueArea.GetComponent<AudioSource>();
    }

    private void Start() {
        audSource.clip = talkSFX;
    }

    private void Update() {
        if (playSound && Time.time > timer && audSource.clip != null) {
            audSource.PlayOneShot(talkSFX);
            timer = Time.time + talkSpeed;
        }
    }

    public override void ResetInteraction() {
        InteractionHandler.instance.ResetInteraction();
    }

    public override void TriggerInteraction() {
        if (isPrinting || dialogueArea.text.Length > 0) return;
        if (dialogueCount >= allDialogue.Length) {

            if (forceSceneChange) SceneManager.LoadScene(sceneIndex);
            dialogueCount = 0; 
        }
        CancelInvoke();
        string dialogue = allDialogue[dialogueCount];
        dialogueCount++;
        StartCoroutine(PrintText(dialogue));
    }

    public IEnumerator PrintText(string toPrint) {
        isPrinting = true;
        playSound = true;
        foreach (char c in toPrint) {
            dialogueArea.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        playSound = false;
        StartCoroutine(ClearText(toPrint));
    }

    public IEnumerator ClearText(string sentence) {
        yield return new WaitForSeconds(clearDelay);
        for (int i = 0; i < sentence.Length; i++) {
            dialogueArea.text = dialogueArea.text.Substring(0, dialogueArea.text.Length-1);
            yield return new WaitForSeconds(textSpeed / 2);
        }
        isPrinting = false;
        ResetInteraction();
    }

    public override bool LockInPlayer() {
        return false;
    }
}

