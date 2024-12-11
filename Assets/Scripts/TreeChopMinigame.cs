using UnityEngine;

public class TreeChopMinigame : MonoBehaviour
{
    [Header("Settings")]
    public int numberOfChops = 4;
    public float keySpeedMin = 2, keySpeedMax = 3, threshhold = 0.05f, indicatorThreshhold, delayMin = 1.25f, delayMax = 3.25f;
    public int chops, fails;

    [Header("Booleans")]
    public bool movingKey;
    public bool inDelay;
    public bool hasFinished;
    public bool playedInbound;

    [Header("Transforms")]
    public SpriteRenderer treeSprite;
    public Transform startPoint = null, endPoint = null, checkPoint = null, key = null;
    public Sprite[] treeSprites;
    public GameObject brokenTree;
    public GameObject stump;
    public Transform bubbleEffect;

    [Header("Audio")]
    public AudioSource mainAudioSource;
    public AudioSource backUp;
    public AudioClip inBound, successfulHit, missHit, treeCut, treeFall;

    float moveTimer = 0;
    float moveSpeed;
    Animator keyAnim;
    Animator bubbleAnim;
    Vector2 desPos;

    private void Start() {
        inDelay = true;
        moveSpeed = Random.Range(delayMin, delayMax);
        chops = 0;
        fails = 0;
        hasFinished = false;
        keyAnim = key.GetComponent<Animator>();
        bubbleAnim = bubbleEffect.GetComponent<Animator>();
        TopDownMovement.instance.canWalk = false;
        TopDownMovement.instance.ChangeToChopState(true);
    }

    private void Update() {
        if (movingKey) {

            if (moveTimer < moveSpeed)
                key.localPosition = Vector3.Lerp(startPoint.localPosition, endPoint.localPosition, moveTimer / moveSpeed);
            else key.localPosition = endPoint.localPosition;

            if (WithinDistance(indicatorThreshhold) && !playedInbound) {
                playedInbound = true;
                PlaySound(inBound);
            }

            if (Input.GetKeyDown(KeyCode.E) || moveTimer >= moveSpeed) {
                desPos = key.localPosition;
                ChopTree();
            }
           
            moveTimer += Time.deltaTime;
        } else if (!movingKey && inDelay) {
            if (moveTimer >= moveSpeed) {
                StartNextKey();
            }

            moveTimer += Time.deltaTime;
        }

        if (chops >= numberOfChops) print("Done");
    }

    public void StartNextKey() {
        if (movingKey) return;

        moveTimer = 0;
        inDelay = false;
        playedInbound = false;
        movingKey = true;
        key.localPosition = startPoint.localPosition;
        keyAnim.SetBool("success", false);
        moveSpeed = Random.Range(keySpeedMin, keySpeedMax);
    }

    public void ChopTree() {
        movingKey = false;
        inDelay = true;
        moveTimer = 0;
        bool success = WithinDistance();
        key.localPosition = desPos;
        bubbleEffect.localPosition = desPos;
        bubbleAnim.SetTrigger(success ? "Chop" : "Fail");
        _ = success ? chops++ : fails++;
        PlaySound(success ? successfulHit : missHit);
        keyAnim.SetBool("success", true);
        TopDownMovement.instance.TriggerChopState();
        moveSpeed = Random.Range(delayMin, delayMax);
        treeSprite.sprite = treeSprites[Mathf.Abs((treeSprites.Length - 1) * chops / numberOfChops)];

        if (chops >= numberOfChops) {
            hasFinished = true;
            stump.SetActive(true);
            brokenTree.SetActive(true);
            PlaySound(treeFall);
            TopDownMovement.instance.canWalk = true;
            TopDownMovement.instance.ChangeToChopState(false);
            Destroy(treeSprite.gameObject);
            gameObject.SetActive(false);
        }
    }

    public void PlaySound(AudioClip toPlay) {
        AudioSource toUse = mainAudioSource.isPlaying ? backUp : mainAudioSource;
        if (toUse.isPlaying) toUse.Stop();
        toUse.PlayOneShot(toPlay);
    }

    public bool WithinDistance() {
        return Vector3.Distance(key.position, checkPoint.position) <= threshhold;
    }

    public bool WithinDistance(float overrideThreshhold) {
        return Vector3.Distance(key.position, checkPoint.position) <= overrideThreshhold;
    }
}
