using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHP : MonoBehaviour
{
    public float baseHP;
    public GameObject ennemy;

    private void Start()
    {
        
    }
    public void TakeDamage()
    {
        if (baseHP > 0)
        {
            baseHP = baseHP - 1f;
        }
        
        if (baseHP <= 0)
        {
            Destroy(ennemy);
        }
    }
}
