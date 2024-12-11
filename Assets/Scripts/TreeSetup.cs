using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeSetup : Interaction
{

    public bool treeSet;
    public GameObject setTree;
    public GameObject cutscene;
    public int sceneToLoad;
    public float transitionDelay;

    public override bool LockInPlayer() {
        return treeSet;
    }

    public override void ResetInteraction() {
        throw new System.NotImplementedException();
    }

    public override void TriggerInteraction() {
        if (!treeSet) {
            treeSet = true;
            setTree.SetActive(true);
        } else if (treeSet) {
            cutscene.SetActive(true);
            Invoke("LoadScene", transitionDelay);
            TopDownMovement.instance.gameObject.SetActive(false);
            
        }
    }

    public void LoadScene() {
        SceneManager.LoadScene(sceneToLoad);
    }

}
