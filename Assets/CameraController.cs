using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public PlayerEventChannel PlayerEventChannel;
    public CinemachineVirtualCamera virtualCamera;

    public float vignetteLerpDuration = 1f;
    float currentLerpTime;
    bool isLerping;

    private Vignette vignette;


    void Start()
    {
        PlayerEventChannel.OnTakeDamage += DamageEffect;
        virtualCamera.GetComponent<CinemachineVolumeSettings>().m_Profile.TryGet(out vignette);
    }

    private void OnDestroy()
    {
        PlayerEventChannel.OnTakeDamage -= DamageEffect;
    }

    void DamageEffect()
    {
        Debug.Log("DamageEffect " + isLerping);
        if (!isLerping)
        {
            StartCoroutine(VignetteEffect());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(VignetteEffect());
        }
    }

    IEnumerator VignetteEffect()
    {
        isLerping = true;

        currentLerpTime = 0;
        while (currentLerpTime < 1.0f)
        {
            vignette.color.value = Color.Lerp(vignette.color.value, Color.red, Mathf.SmoothStep(0.0f, 1.0f, currentLerpTime));
            currentLerpTime += Time.deltaTime / vignetteLerpDuration;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        currentLerpTime = 0;
        while (currentLerpTime < 1.0f)
        {
            vignette.color.value = Color.Lerp(vignette.color.value, Color.white, Mathf.SmoothStep(0.0f, 1.0f, currentLerpTime));
            currentLerpTime += Time.deltaTime / vignetteLerpDuration;
            yield return null;
        }

        isLerping =false;
    }
}
