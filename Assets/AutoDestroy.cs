using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float waitTime;

    void Start()
    {
        StartCoroutine(AutoDestroyAfterTime());
    }

    IEnumerator AutoDestroyAfterTime()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
