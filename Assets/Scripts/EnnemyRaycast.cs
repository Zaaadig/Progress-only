using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyRaycast : MonoBehaviour
{
    [Header("References")]
    public float raycastDistance = 100f;
    public float coneAngle = 180f;
    public int numRays = 50;
    public LayerMask hitLayer;
    public KeyCode scan = KeyCode.E;
    public EnnemyHP ennemyHP;

    //private Transform rotation;
    private float stepAngle;
    private float currentAngle;
    private Vector3 rayDirection;
    private RaycastHit hit;
    private void Start()
    {
        //rotation = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y +45f, gameObject.transform.eulerAngles.z);
    }
    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(scan))
        {
            RaycastShoot();
        }
    }
    private void RaycastShoot()
    {
        stepAngle = coneAngle / numRays; // calculer l'espace entre les rayons de manière intelligente (toujours avoir le meme ecartement entre les rays peut importe les paramètres)

        for (int i = 0; i < numRays; i++) // La boucle for sert a repeter le raycast pour qu'il fasse toute la largeur du cone 
        {
            currentAngle = -coneAngle / 2 + stepAngle * i; // calculer à quel raycast on est (comme ca au suivant vu que i augmente alors l'angle changera)
            rayDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward; // calculer la direction initial pour le raycast avec en mémoire le décalage pour le raycast en cours

            if (Physics.Raycast(transform.position, rayDirection, out hit, raycastDistance, hitLayer))
            {
                if (hit.collider.tag == "Ennemy")
                {
                    print("It's an ennemy");
                    ennemyHP.TakeDamage();
                }

                if (hit.collider.tag == "Wall")
                {
                    print("It's a wall");
                }
            }
            Debug.DrawRay(transform.position, rayDirection * raycastDistance, Color.red, 1f);
        }

    //    ray1 = new Ray(transform.position, transform.forward);
    //    if (Physics.Raycast(ray1, out RaycastHit hit1, raycastDistance, hitLayer))
    //    {
    //        if (hit1.collider.tag == "Ennemy")
    //        {
    //            print("It's an ennemy");
    //            ennemyHP.TakeDamage();
    //        }

    //        if (hit1.collider.tag == "Wall")
    //        {
    //            print("It's a wall");
    //        }
    //    }
    }
}