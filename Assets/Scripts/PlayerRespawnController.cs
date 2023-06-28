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
        playerEventChannel.OnSetRespawn += OnSetRespawn;
    }

    void OnRespawn(Transform player)
    {
        player.position = respawnPosition.position;
    }

    void OnSetRespawn(Vector2 respawnPoint)
    {
        respawnPosition.position = respawnPoint;
    }
}
