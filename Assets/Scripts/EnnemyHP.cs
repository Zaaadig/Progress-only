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
            ennemy.GetComponent<CapsuleCollider>().enabled = false;
            // lancer les anims de mort à ce moment et destroy à la fin
            StartCoroutine(EnnemyDestroy());
        }
    }

    public IEnumerator EnnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f); // Idéalement il faudrait lui dire d'attendre jusque la fin des anims de mort au lieu d'une variable de temps fixe
        
        Destroy(ennemy);
    }
}
