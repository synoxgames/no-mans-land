using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GroundType groundType;
    public enum GroundType {
        Default,
        Dirt,
        Wood
    }
}
