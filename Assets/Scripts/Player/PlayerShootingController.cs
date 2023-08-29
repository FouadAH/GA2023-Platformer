using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerShootingController : MonoBehaviour
{
    PlayerMovementController movementController;

    public GameObject bulletPrefab;
    public Transform spawnPoint;

    public Transform gunSpriteTransform;
    public Transform gunTransform;

    public Transform gunRecoilPosition;
    public Transform gunOriginalPosition;

    public float recoilForceAmount = 5f;
    public float fireCooldown;
    float lastFiredTime;

    public CinemachineImpulseSource impulseSource;
    public ParticleSystem fireVFX;
    public AudioSource shootSFX;

    bool isRecoiling;

    float currentLerpTime;
    float recoilDuration = 0.05f;


    public float maxGunHeat = 10f;
    public float fireGunHeatAmount = 2.3f;
    public float gunHeatCooldownRate = 0.5f;
    public float gunHeat;

    bool isOverHeated;

    SpriteRenderer gunSpriteRenderer;

    PlayerInputMaster inputActions;
    Vector2 mousePos;

    private void Start()
    {
        movementController= GetComponent<PlayerMovementController>(); 
        inputActions = GetComponent<PlayerMovementController>().playerInputs;
        inputActions.Player.Fire.started += Fire_started;
        gunSpriteRenderer = gunSpriteTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gunTransform.transform.up = (mousePos - (Vector2)transform.position).normalized;
        gunTransform.Rotate(new Vector3(0, 0, -90));
        gunSpriteRenderer.flipY = (gunTransform.right.x > -1 && gunTransform.right.x < 0);

        gunHeat = Mathf.Clamp(Mathf.Lerp(gunHeat, 0, gunHeatCooldownRate), 0, 100f);
        float remapedGunHeat = math.remap(0, maxGunHeat, 0, 1, gunHeat);

        gunSpriteRenderer.color = Color.Lerp(Color.white, Color.red, remapedGunHeat);

        if(remapedGunHeat <= 0.1f)
        {
            isOverHeated = false;
        }
    }

    private void Fire_started(InputAction.CallbackContext obj)
    {
        OnFire(); 
    }

    public void FireBullet()
    {
        if (isOverHeated)
            return;

        Bullet bullet =  Instantiate(bulletPrefab, spawnPoint.position, gunTransform.rotation).GetComponent<Bullet>();
        bullet.direction = -spawnPoint.right;

        Instantiate(fireVFX, spawnPoint.position, gunTransform.rotation).Play();
        Instantiate(shootSFX).Play();

        if (isRecoiling == false)
        {
            currentLerpTime = 0;
            StartCoroutine(Recoil());
        }

        impulseSource.GenerateImpulse(0.5f);

        movementController.PushInDirection(recoilForceAmount, spawnPoint.right);

        gunHeat += fireGunHeatAmount;

        if(gunHeat > maxGunHeat)
        {
            isOverHeated= true;
        }
    }
    
    public void OnFire()
    {
        if(Time.time > lastFiredTime + fireCooldown)
        {
            if (gunHeat < maxGunHeat)
            {
                lastFiredTime = Time.time;
                FireBullet();
            }
        }
    }

    IEnumerator Recoil()
    {
        isRecoiling = true;

        while (currentLerpTime < 1.0f)
        {
            gunSpriteTransform.position = Vector2.Lerp(gunSpriteTransform.position, gunRecoilPosition.position, Mathf.SmoothStep(0.0f, 1.0f, currentLerpTime));
            currentLerpTime += Time.deltaTime / recoilDuration;
            yield return null;
        }

        currentLerpTime = 0;
        yield return new WaitForSeconds(0.02f);

        while (currentLerpTime < 1.0f)
        {
            gunSpriteTransform.position = Vector2.Lerp(gunSpriteTransform.position, gunOriginalPosition.position, Mathf.SmoothStep(0.0f, 1.0f, currentLerpTime));
            currentLerpTime += Time.deltaTime / recoilDuration;
            yield return null;
        }

        isRecoiling = false;
    }
}
