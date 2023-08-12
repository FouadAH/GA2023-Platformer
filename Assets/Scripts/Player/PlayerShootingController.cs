using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;

    public Transform gunTransform;
    public Transform gunRecoilPosition;
    public Transform gunOriginalPosition;

    public float fireCooldown;
    float lastFiredTime;

    public CinemachineImpulseSource impulseSource;
    public ParticleSystem fireVFX;
    public AudioSource shootSFX;

    private void Start()
    {
        PlayerInputMaster inputActions = GetComponent<PlayerMovementController>().playerInputs;
        inputActions.Player.Fire.started += Fire_started;
    }

    private void Fire_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFire();
    }

    public void FireBullet()
    {
        Bullet bullet =  Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
        float bulletXDiretion = transform.localScale.x * -1;
        bullet.direction = new Vector2(bulletXDiretion, 0);

        Instantiate(fireVFX, spawnPoint.position, Quaternion.identity).Play();

        Instantiate(shootSFX).Play();

        if (isRecoiling == false)
        {
            currentLerpTime = 0;
            StartCoroutine(Recoil());
        }

        impulseSource.GenerateImpulse(0.5f);
    }
    bool isRecoiling;
    public void OnFire()
    {
        if(Time.time > lastFiredTime + fireCooldown)
        {
            lastFiredTime = Time.time;
            FireBullet();
        }
    }

    float currentLerpTime;
    float recoilDuration = 0.05f;

    IEnumerator Recoil()
    {
        isRecoiling = true;

        while (currentLerpTime < 1.0f)
        {
            gunTransform.position = Vector2.Lerp(gunTransform.position, gunRecoilPosition.position, Mathf.SmoothStep(0.0f, 1.0f, currentLerpTime));
            currentLerpTime += Time.deltaTime / recoilDuration;
            yield return null;
        }

        currentLerpTime = 0;
        yield return new WaitForSeconds(0.02f);

        while (currentLerpTime < 1.0f)
        {
            gunTransform.position = Vector2.Lerp(gunTransform.position, gunOriginalPosition.position, Mathf.SmoothStep(0.0f, 1.0f, currentLerpTime));
            currentLerpTime += Time.deltaTime / recoilDuration;
            yield return null;
        }

        isRecoiling = false;
    }
}
