using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    delegate void MyDelegate();
    MyDelegate attack;

    void Start()
    {
        attack = PrimaryAttack;
        attack();
    }

    void PrimaryAttack()
    {
        Debug.Log("Primary attack called!");
    }

    void SecondaryAttack()
    {
        Debug.Log("Secondary attack called!");
    }

    public void ExampleSingletonMethod()
    {

    }
}
