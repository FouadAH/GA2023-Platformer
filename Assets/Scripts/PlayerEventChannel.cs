using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerEventChannel : ScriptableObject
{
    public UnityAction OnTakeDamage;

    public UnityAction<int> OnHeal;
    public UnityAction<int> OnSetPlayerHealth;

    public UnityAction<Transform> OnRespawn;

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

    public void RaiseOnPlayerRespawn(Transform player)
    {
        OnRespawn?.Invoke(player);
    }

}
