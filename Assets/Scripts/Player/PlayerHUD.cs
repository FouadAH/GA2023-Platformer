using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PlayerHUD : MonoBehaviour
{
    public PlayerEventChannel playerEventChannel;
    public GameObject heartPrefab;
    public Transform heartsParent;
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;
    float currentFadeTime;

    void Awake()
    {
        playerEventChannel.OnTakeDamage += HandleTakeDamage;
        playerEventChannel.OnSetPlayerHealth += InitializeHealth;
        playerEventChannel.OnHeal += HandleHeal;
        playerEventChannel.OnDeath += OnRespawnPlayer;
    }

    private void OnDestroy()
    {
        playerEventChannel.OnTakeDamage -= HandleTakeDamage;
        playerEventChannel.OnSetPlayerHealth -= InitializeHealth;
        playerEventChannel.OnHeal -= HandleHeal;
        playerEventChannel.OnDeath -= OnRespawnPlayer;
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

    void OnRespawnPlayer()
    {
        //StartCoroutine(Fade());

        fadeCanvas.DOFade(1, fadeDuration).OnComplete(() =>
        {
            playerEventChannel.RaiseOnPlayerRespawn();
            fadeCanvas.DOFade(0, fadeDuration);
        });
    }

    IEnumerator Fade()
    {
        while (currentFadeTime < 1.0f)
        {
            fadeCanvas.alpha = Mathf.Lerp(fadeCanvas.alpha, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, currentFadeTime));
            currentFadeTime += Time.deltaTime / fadeDuration;
            yield return null;
        }

        currentFadeTime = 0;
        fadeCanvas.alpha = 1;

        yield return new WaitForSeconds(1f);

        while (currentFadeTime < 1.0f)
        {
            fadeCanvas.alpha = Mathf.Lerp(fadeCanvas.alpha, 0, Mathf.SmoothStep(0.0f, 1.0f, currentFadeTime));
            currentFadeTime += Time.deltaTime / fadeDuration;
            yield return null;
        }

        fadeCanvas.alpha = 0;
    }
}
