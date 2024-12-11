using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnInteract : Interaction
{
    public int sceneToLoad = 4;
    public override bool LockInPlayer() {
        return false;
    }

    public override void ResetInteraction() {
        return;
    }

    public override void TriggerInteraction() {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
