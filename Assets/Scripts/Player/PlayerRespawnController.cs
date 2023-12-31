using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnController : MonoBehaviour
{
    public PlayerEventChannel playerEventChannel;
    public Transform respawnPosition;

    private void Start()
    {
        playerEventChannel.OnRespawn += OnRespawn;
    }

    void OnRespawn(Transform player)
    {
        player.position = respawnPosition.position;
    }

}
