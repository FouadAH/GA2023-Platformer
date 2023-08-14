using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerEventChannel : ScriptableObject
{
    public UnityAction OnTakeDamage;
    public UnityAction OnDeath;

    public UnityAction<int> OnHeal;
    public UnityAction<int> OnSetPlayerHealth;

    public UnityAction<int> OnReceivePoints;
    public UnityAction<int> OnUpdatePointUI;

    public UnityAction OnRespawn;

    public Transform playerTransform;
    public void RaiseOnTakeDamage()
    {
        OnTakeDamage?.Invoke();
    }

    public void RaiseOnHeal(int amountHealed)
    {
        OnHeal?.Invoke(amountHealed);
    }

    public void RaiseOnSetPlayerHealth(int maxHealth)
    {
        OnSetPlayerHealth?.Invoke(maxHealth);
    }


    public void RaiseOnPlayerRespawn()
    {
        OnRespawn?.Invoke();
    }

    public void RaiseOnPlayerDeath()
    {
        OnDeath?.Invoke();
    }

    public void RaiseOnReceivePoints(int pointsAmount)
    {
        OnReceivePoints?.Invoke(pointsAmount);
    }

    public void RaiseOnUpdatePointUI(int pointsAmount)
    {
        OnUpdatePointUI?.Invoke(pointsAmount);
    }

}
