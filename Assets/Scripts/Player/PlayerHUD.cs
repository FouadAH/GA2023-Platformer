using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public PlayerEventChannel playerEventChannel;
    public GameObject heartPrefab;
    public Transform heartsParent;

    void Awake()
    {
        playerEventChannel.OnTakeDamage += HandleTakeDamage;
        playerEventChannel.OnSetPlayerHealth += InitializeHealth;
        playerEventChannel.OnHeal += HandleHeal;
    }

    private void OnDestroy()
    {
        playerEventChannel.OnTakeDamage -= HandleTakeDamage;
        playerEventChannel.OnSetPlayerHealth -= InitializeHealth;
        playerEventChannel.OnHeal -= HandleHeal;
    }

    void InitializeHealth(int maxHealth)
    {
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxHealth; i++)
        {
            Instantiate(heartPrefab, heartsParent);
        }
    }

    void HandleTakeDamage()
    {
        if(heartsParent.childCount > 0)
        {
            Destroy(heartsParent.GetChild(0).gameObject);
        }
    }

    void HandleHeal(int amountHealed)
    {
        for (int i = 0; i < amountHealed; i++)
        {
            Instantiate(heartPrefab, heartsParent);
        }
    }
}
