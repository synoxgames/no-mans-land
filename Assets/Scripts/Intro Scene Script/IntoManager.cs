using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoManager : MonoBehaviour
{

    [Header("Into End Time")]
    public float time = 10f;
    private float currentTime = 0f;

    [Header("Scene To Load")]
    public string sceneName = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > time) {
            SceneManager.LoadScene(sceneName);
        } else {
            currentTime += Time.deltaTime;
        }
    }
}
