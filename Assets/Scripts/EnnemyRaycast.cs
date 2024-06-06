using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyRaycast : MonoBehaviour
{
    [Header("References")]
    public float raycastDistance = 100f;
    public LayerMask hitReturn;
    public KeyCode scan = KeyCode.E;

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
            Debug.Log(hit.collider.gameObject.name + "was hit!");
        }
    }
}