using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public PlayerEventChannel playerEventChannel;
    public GameObject heartPrefab;
    public Transform heartsParent;

    void Start()
    {
        playerEventChannel.OnTakeDamage += HandleTakeDamage;
        playerEventChannel.OnHeal += HandleHeal;
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
