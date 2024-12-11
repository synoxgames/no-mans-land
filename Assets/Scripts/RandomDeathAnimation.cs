using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeathAnimation : MonoBehaviour
{
    public int deathCount = 2;
    public Animator anim;

    public void Start() {
        GetRandomDeath();
    }

    public void GetRandomDeath() {
        float val = Random.Range(0, deathCount);
        anim.SetFloat("deathType", val);
    }
}
