using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickController : MonoBehaviour
{
    [Header("General")]
    public float turnSpeed = 1f;
    public float pickSpeed = 50f;
    public float wiggleRange = .05f;
    public float threshhold = 0.1f;
    public float lockPosition = 0;
    public bool isUnlocked = true;
    public float wiggleStrength = 1.25f;
    public float wiggleFreq = 3;
    public float closeDelay = 1.25f;
    public bool hasSolved = false;      // This is for when interacting, it will allow for a second pause between solving it and closing the menu
    bool isTurning = false;
    bool canTurn = false;
    bool isWiggling = false;
    public bool inWiggleZone = false;

    [Header("Audio")]
    public float audioSpeed = 0.25f;
    public AudioClip unlockClip;
    public AudioClip stuckClip;
    public AudioClip turningClip;
    public AudioClip lockTurnClip;
    float audTimer = 0.0f;
    bool playingAudio = false;

    [Header("Lock")]
    public int lockMaxRot = 90;
    float lockCurrentRot = 360f;
    Vector3 lockRot;
    public RectTransform lockTransform;
    public float rotMaxTemp = 0;

    [Header("Lock Pick")] 
    public int pickMaxRot = 360;
    public bool canMovePick = true;
    Vector3 pickRot;
    float pickCurrentRot = 360f;
    float pickWiggle = 0;
    public RectTransform lockPickTransform;

    float stuckTime = 0.0f;
    float dist = 0;
    AudioSource audSource;

    private void Start() {
        // Set up lock and lockpick positions
        pickCurrentRot = 0;
        lockCurrentRot = 0;
        pickRot = lockPickTransform.eulerAngles;
        lockRot = lockTransform.eulerAngles;
        
        lockPosition = Random.Range(10.000f, 340.000f);
        isUnlocked = false;

        audSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (isUnlocked) return;

        if (Input.GetKeyDown(KeyCode.Q)) {
            ResetLockpick(false);
            gameObject.SetActive(false);
            InteractionHandler.instance.ResetInteraction();
            TopDownMovement.instance.canWalk = true;
        }

        // Set lock and lockpick rotations
        pickRot.z = -pickCurrentRot;
        lockRot.z = isWiggling ? -pickWiggle + -lockCurrentRot : -lockCurrentRot;

        // Get the wiggle offset
        pickWiggle = Mathf.Cos(wiggleFreq * stuckTime) * wiggleStrength;

        // Set up lock and lockpick euler angles
        lockTransform.localEulerAngles = lockRot;
        lockPickTransform.localEulerAngles = pickRot;

        // Make sure the number never goes past the maximum it's allowed to rotate
        pickCurrentRot = pickCurrentRot % pickMaxRot;
        lockCurrentRot = lockCurrentRot % lockMaxRot;

        // Gets the distance between the lock position and the lockpick
        dist = Mathf.Abs(lockPosition - pickCurrentRot) / pickMaxRot;

        // Check to see if turning
        isTurning = (Input.GetKey(KeyCode.D));
        canMovePick = !isTurning;


        if (canMovePick && !isWiggling) {
            // Move lockpick
            pickCurrentRot += Input.GetAxisRaw("Vertical") * Time.deltaTime * pickSpeed;


            if (Input.GetButton("Vertical") && !playingAudio) StartAudio(turningClip);
            else if (Input.GetButtonUp("Vertical")) StopAudio();

        }
        

        if (playingAudio && Time.time > audTimer) {
            audSource.Play();
            audTimer = Time.time + audioSpeed;
        }



        // If turning
        if (isTurning) {


            // Turn
            if (!isWiggling) {
                lockCurrentRot += turnSpeed * Time.deltaTime;
                StartAudio(lockTurnClip);
            }
            // If it's reached the wiggle room rotation
            if (lockCurrentRot >= rotMaxTemp && !canTurn) {
                stuckTime += Time.deltaTime;

                if (!isWiggling) {
                    StopAudio();
                    StartAudio(stuckClip);
                    pickWiggle = lockCurrentRot;
                    isWiggling = true;
                }
            } else if (lockCurrentRot >= rotMaxTemp && canTurn) {
                isUnlocked = true;
                Invoke("Finished", closeDelay);
                PlayAudio(unlockClip);
                return;
            }
        } else if (lockCurrentRot > 0 && !isUnlocked) {
            StopAudio();
            StartAudio(lockTurnClip, 0.2f);
            lockCurrentRot -= Time.deltaTime * 150;
        } else if (lockCurrentRot < 0) {
            lockCurrentRot = 0;
            StopAudio();
        }
        
        if (!isTurning && isWiggling) {
                StopAudio();
                isWiggling = false;
                pickWiggle = 0;
        }

        if (pickCurrentRot <= 0) pickCurrentRot = 359.99f;

        canTurn = dist <= threshhold;
        inWiggleZone = dist <= wiggleRange && !canTurn;

        if (inWiggleZone && !canTurn) {
            rotMaxTemp = lockMaxRot * (1f - ((dist) / wiggleRange));
        } else if (canTurn) {
            rotMaxTemp = lockMaxRot;
        } else rotMaxTemp = 0;
    }

    /*private void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), ("Prox: " + dist.ToString("#.###")));
        GUI.Label(new Rect(10, 25, 100, 20), (1 - dist) + "%");
        GUI.Label(new Rect(10, 40, 100, 20), "Can Turn: " + canTurn.ToString());
        GUI.Label(new Rect(10, 55, 250, 20), "Can Wiggle: " + inWiggleZone.ToString());
        GUI.Label(new Rect(10, 70, 250, 20), "Max Move Lock: " + rotMaxTemp.ToString());
        GUI.Label(new Rect(10, 85, 250, 20), "Pick Wiggle Wave: " + pickWiggle.ToString());
    }*/


    public void StartAudio(AudioClip toPlay) {
        playingAudio = true;
        audSource.clip = toPlay;
    }

    public void StartAudio(AudioClip toPlay, float pitch) {
        playingAudio = true;
        audSource.clip = toPlay;
        audSource.pitch = pitch;
    }

    public void PlayAudio(AudioClip toPlay) {
        audSource.Stop();
        audSource.PlayOneShot(toPlay);
    }
    public void StopAudio() {
        playingAudio = false;
        audSource.pitch = 1;
        audSource.Stop();
    }

    public void Finished() {
        hasSolved = true;
    }

    public void ResetLockpick(bool newPosition) {
        pickCurrentRot = 0;
        lockCurrentRot = 0;
        stuckTime = 0;
        lockPickTransform.eulerAngles = Vector3.zero;
        lockPickTransform.eulerAngles = Vector3.zero;
        pickRot = lockPickTransform.eulerAngles;
        lockRot = lockTransform.eulerAngles;
        hasSolved = false;
        isUnlocked = false;
        playingAudio = false;
        if (newPosition || lockPosition == 0) {
            lockPosition = Random.Range(10.000f, 340.000f);
        }
        
        isUnlocked = false;
    }
}

