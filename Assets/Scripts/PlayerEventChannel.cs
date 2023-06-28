using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerEventChannel : ScriptableObject
{
    public UnityAction OnTakeDamage;
    public UnityAction<int> OnHeal;

    public UnityAction<Transform> OnRespawn;
    public UnityAction<Vector2> OnSetRespawn;
    public UnityAction<Transform> OnPlayerDeath;

    public void RaiseOnTakeDamage()
    {
        OnTakeDamage?.Invoke();
    }

    public void RaiseOnHeal(int amountHealed)
    {
        OnHeal?.Invoke(amountHealed);
    }

    public void RaiseOnPlayerDeath(Transform player)
    {
        OnPlayerDeath?.Invoke(player);
    }

    public void RaiseOnPlayerRespawn(Transform player)
    {
        OnRespawn?.Invoke(player);
    }

    public void RaiseOnSetPlayerRespawnPoint(Vector2 respawnPoint)
    {
        OnSetRespawn?.Invoke(respawnPoint);
    }
}
