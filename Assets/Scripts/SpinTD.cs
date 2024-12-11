using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTD : MonoBehaviour
{
    public float moveSpeed = 2.5f;

    public void Update() {
        transform.Rotate(-transform.forward * moveSpeed * Time.deltaTime);
    }
}
