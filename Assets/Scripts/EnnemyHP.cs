using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHP : MonoBehaviour
{
    [Header("Ennemy Type")]
    public bool dashEnnemy = false;
    public float dashEnnemyHP;

    [Header("References")]
    public float baseHP;
    public GameObject ennemy;
    private Rigidbody m_rb;

    [Header("Dash Ennemy")]
    public float dashForce;
    private void Start()
    {
        m_rb = GetComponentInParent<Rigidbody>();
        if (dashEnnemy == true)
        {
            baseHP = dashEnnemyHP;
        }
    }
    public void TakeDamage()
    {
        if (baseHP > 0 && dashEnnemy == false)
        {
            baseHP = baseHP - 1f;
        }

        if (baseHP <= 0 && dashEnnemy == false)
        {
            ennemy.GetComponentInChildren<CapsuleCollider>().enabled = false;
            // lancer les anims de mort à ce moment et destroy à la fin
            StartCoroutine(EnnemyDestroy());
        }

        if (baseHP > 0 && dashEnnemy == true)
        {
            baseHP = baseHP - 1f;
            m_rb.AddForce(transform.right *  dashForce, ForceMode.Impulse);
        }

        if (baseHP <= 0 && dashEnnemy == true)
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(EnnemyDestroy());
        }
        
    }

    public IEnumerator EnnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f); // Idéalement il faudrait lui dire d'attendre jusque la fin des anims de mort au lieu d'une variable de temps fixe
        
        Destroy(ennemy);
    }
}
