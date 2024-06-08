using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyRaycast : MonoBehaviour
{
    [Header("References")]
    public float raycastDistance = 100f;
    public LayerMask hitReturn;
    public KeyCode scan = KeyCode.E;
    public EnnemyHP ennemyHP;

    private Ray ray;

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
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, hitReturn, QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.tag == "Ennemy")
            {
                print("It's an ennemy");
                ennemyHP.TakeDamage();
            }

            if(hit.collider.tag == "Wall")
            {
                print("It's a wall");
            }
        }
    }
}