using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{

    public float waitTime = 9f;
    private float currentTime = 0f;
    public string sceneToLoad = "";

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= waitTime)
        {
            SceneManager.LoadScene(sceneToLoad);
        } else
        {
            currentTime += Time.deltaTime;
        }
    }
}
