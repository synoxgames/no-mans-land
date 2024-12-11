using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header("Floats")]
    public float walkSpeed = 1.5f;

    [Header("Booleans")]
    public bool canWalk = true;
    private bool isWalking = false;
    public bool yLock = false;
    public Vector2 yLockConstraints;

    [Header("Audio")]
    public StepType[] allSteps;
    public LayerMask groundLayer;
    private AudioClip[] currentClips;
    public float stepDelay = 0.3f;
    private AudioSource audSource;
    private float nextStep = 0.0f;
    private int audIndex;
    Ground.GroundType currentGroundType;

    [Header("Automated Movement")]
    public bool inAutomatedMovement = false;
    public List<AutomaticMovement> testList = new List<AutomaticMovement>();
    private Queue<AutomaticMovement> movementList = new Queue<AutomaticMovement>();
    private AutomaticMovement currentMovement;
    Vector2 startPoint;
    private bool inMovement;
    float autoTimer;

    [Header("Items")]
    public List<string> allItems = new List<string>();  // This list is temporary, i will set up an actual inventory soon just using strings atm

    Animator anim;
    static float x, y;
    public static TopDownMovement instance;

    private void Awake() {
        anim = GetComponent<Animator>();

        if (instance != this) {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start() {
        audSource = GetComponent<AudioSource>();
        currentClips = allSteps[0].walkClips;
        nextStep = 0.0f;
        audIndex = 0;
    }

    private void Update() {
        if (!canWalk && !inAutomatedMovement) return;

        if (!inAutomatedMovement) {
            isWalking = Input.GetButton("Horizontal") || Input.GetButton("Vertical");
            if (isWalking) Move();

            anim.SetBool("isWalking", isWalking || inAutomatedMovement);

            if (isWalking && !canWalk) {
                isWalking = false;
            }
        }

        if (inAutomatedMovement) {
            MovePlayer();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            StartAutomaticMovement(testList);
        }
    }

    // Used to move the player
    public void Move() {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("X", x);
        anim.SetFloat("Y", y);

        Vector2 movement = new Vector2(x, yLock ? 0 : y).normalized;
        transform.Translate(movement * walkSpeed * Time.deltaTime);

        if (nextStep < Time.time) {
            CheckGround();
            PlayStepSound();
        }
    }

    public static Vector2 GetXY() {
        return new Vector2(x, y);
    }

    public static Vector2 GetXYInt() {
        return new Vector2(x > 0 ? Mathf.CeilToInt(x) : Mathf.FloorToInt(x), y > 0 ? Mathf.CeilToInt(y) : Mathf.FloorToInt(y));
    }

    public void PlayStepSound() {
        audSource.PlayOneShot(GetStep());
        nextStep = Time.time + stepDelay;
    }

    public AudioClip GetStep() {
        if (audIndex > currentClips.Length - 1) audIndex = 0;
        return currentClips[audIndex++];
    }

    public void StartAutomaticMovement(List<AutomaticMovement> movements) {
        foreach (AutomaticMovement m in movements) {
            movementList.Enqueue(m);
        }

        inAutomatedMovement = true;
        canWalk = false;
        PlayNextMovement();
    }

    public void MovePlayer() {
        autoTimer -= Time.deltaTime;
        transform.position = Vector3.Lerp(startPoint, (startPoint + currentMovement.moveDirection * currentMovement.moveDistance), 1 - (autoTimer / currentMovement.moveSpeed));
        anim.SetBool("isWalking", true);
        

        if (Time.time > nextStep) PlayStepSound();

        if (autoTimer <= 0) {
            PlayNextMovement();
        }
    }

    public void PlayNextMovement() {
        if (movementList.Count <= 0) {
            ResetAutomatedMovement();
            return;
        }
        currentMovement = movementList.Dequeue();
        autoTimer = currentMovement.moveSpeed;
        anim.SetFloat("X", currentMovement.moveDirection.x);
        anim.SetFloat("Y", currentMovement.moveDirection.y);
        startPoint = transform.position;

        if (currentMovement.hasDelay) {
            // Do delay stuff
        }
    }

    public void ResetAutomatedMovement() {
        inAutomatedMovement = false;
        inMovement = false;
        canWalk = true;
    }

    public void AddItem(string toAdd) {
        allItems.Add(toAdd);
    }

    public bool CheckForItem(string toCheck) {
        return allItems.Contains(toCheck);
    }
    public void CheckGround() {
        RaycastHit2D ground = Physics2D.CircleCast(transform.position, .01f, transform.up, .01f ,groundLayer);

        if (ground.transform == null || ground.transform.GetComponent<Ground>() == null) return;
        if (ground.transform.GetComponent<Ground>().groundType == currentGroundType) return;

        currentGroundType = ground.transform.GetComponent<Ground>().groundType;
        currentClips = GetClipFromType(currentGroundType);
    }

    public AudioClip[] GetClipFromType(Ground.GroundType type) {
        StepType gotten = Array.Find(allSteps, step => step.type == type);

        if (gotten == null) return null;

        return gotten.walkClips;
    }

    public void ChangeToChopState(bool state) {
        anim.SetLayerWeight(1, state ? 1 : 0);
    }

    public void TriggerChopState() {
        anim.SetTrigger("Chop");
    }

    [System.Serializable]
    public class AutomaticMovement {
        public float moveSpeed = 1.5f;  // How long it takes in seconds to move the distance
        public float moveDistance;      // The number of units to move
        public Vector2 moveDirection;   // The direction to move

        // TO-DO
        public bool hasDelay;
        public float delayLength;

        public AutomaticMovement(Vector2 dir, float dis, float speed, bool delay, float delayL = 0) {
            moveDirection = dir;
            moveDistance = dis;
            moveSpeed = speed;
            hasDelay = delay;

            if (delay) delayLength = delayL;
        }
    }

    [System.Serializable]
    public class StepType {
        public Ground.GroundType type;
        public AudioClip[] walkClips;
    }
}
