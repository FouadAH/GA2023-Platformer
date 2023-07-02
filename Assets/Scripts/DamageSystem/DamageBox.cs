using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class DamageBox : MonoBehaviour
{
    public LayerMask damageTargetMask;
    public float damageAmount = 1f; 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((damageTargetMask & (1 << collision.gameObject.layer)) != 0)
        { 
            collision.GetComponent<IDamagable>().TakeDamage(damageAmount);
        }
    }
}
