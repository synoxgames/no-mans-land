using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{   
    public Boss boss;
    public Image fillImage;

    private void Awake() {
        if (boss == null) {
            boss = FindObjectOfType<Boss>();
        }
    }

    private void Start() {
        boss.currentHealth = boss.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        fillImage.fillAmount = boss.currentHealth / boss.maxHealth;
        // Debug.Log(fillImage.fillAmount);
    }
}
