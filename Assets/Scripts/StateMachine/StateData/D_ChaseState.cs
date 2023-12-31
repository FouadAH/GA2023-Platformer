﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/State Data/Player Detected State")]
public class D_ChaseState : ScriptableObject
{
    public float chaseSpeed = 10f;
    public float chaseTime = 5f;
}
