using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{

    public GameObject heart1, heart2, heart3;
    public GameObject gameOverScreen;
    public Transform player;
    public Transform boss;
    public int winLoadScreen = 8;
    public bool killedPlayer = false;
    public static BattleController instance;

    private void Awake() {
        if (instance != this) Destroy(instance);
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // player.GetComponent<PlatformerMovement>().lives = 3;
        // heart1.gameObject.SetActive(true);
        // heart2.gameObject.SetActive(true);
        // heart3.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (player.GetComponent<PlatformerMovement>().lives == 3) {
            heart1.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart3.gameObject.SetActive(true);
        }
        if (player.GetComponent<PlatformerMovement>().lives == 2) {
            heart1.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart3.gameObject.SetActive(false);
        }
        if (player.GetComponent<PlatformerMovement>().lives == 1) {
            heart1.gameObject.SetActive(true);
            heart2.gameObject.SetActive(false);
            heart3.gameObject.SetActive(false);
        }
        if (player.GetComponent<PlatformerMovement>().lives == 0) {
            /* Player Died */
            heart1.gameObject.SetActive(false);
            heart2.gameObject.SetActive(false);
            heart3.gameObject.SetActive(false);


            if (!killedPlayer) {
                gameOverScreen.SetActive(true);
                Destroy(FindObjectOfType<Boss>().gameObject);
                killedPlayer = true;
            }
        }

        if (!killedPlayer) {
            if (boss.GetComponent<Boss>().currentHealth <= 0) {
                /* Player Survives */
                SceneManager.LoadScene(winLoadScreen);
            }
        }
        // Debug.Log("Current Lives: ", player.GetComponent<PlatformerMovement>().lives);
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
