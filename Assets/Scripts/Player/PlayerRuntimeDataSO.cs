using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerRuntimeData", menuName = "Data/Player Data/Player Runtime Data")]
public class PlayerRuntimeDataSO : ScriptableObject
{
    public Vector2 playerPosition;
}
