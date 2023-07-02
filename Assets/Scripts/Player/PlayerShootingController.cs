using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireCooldown;

    float lastFiredTime;

    public void FireBullet()
    {
        Bullet bullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        float bulletXDiretion = transform.localScale.x * -1;
        bullet.direction = new Vector2(bulletXDiretion, 0);
    }

    public void OnFire()
    {
        if(Time.time > lastFiredTime + fireCooldown)
        {
            FireBullet();
        }
    }
}
