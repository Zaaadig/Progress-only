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
    //public LayerMask obstacleLayer;
    public KeyCode scan = KeyCode.E;
    public float cooldownTime = 0.7f;

    //private Transform rotation;
    private bool cooldown = true;
    private float stepAngle;
    private float currentAngle;
    private Vector3 rayDirection;
    private RaycastHit hit;
    private RaycastHit obstacleHit;
    private List<GameObject> detectedEnnemy = new List<GameObject>();
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
        if (Input.GetKeyDown(scan) && cooldown == true)
        {
            RaycastShoot();
            cooldown = false;
            StartCoroutine(Cooldown());
        }
    }
    private void RaycastShoot()
    {
        detectedEnnemy.Clear();
        HashSet<GameObject> checkEnnemy = new HashSet<GameObject>();

        stepAngle = coneAngle / numRays; // calculer l'espace entre les rays de manière intelligente (toujours avoir le meme ecartement entre les rays peut importe les paramètres)

        for (int i = 0; i < numRays; i++) // La boucle for sert a repeter le raycast pour qu'il fasse toute la largeur du cone 
        {
            currentAngle = -coneAngle / 2 + stepAngle * i; // calculer à quels raycasts on es (comme ca au suivant vu que i augmente alors l'angle changera)
            rayDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward; // calculer la direction initial pour le raycast avec en mémoire le décalage pour le raycast en cours

            if (Physics.Raycast(transform.position, rayDirection, out hit, raycastDistance, hitLayer))
            {
                if (hit.collider.CompareTag("Ennemy") && !checkEnnemy.Contains(hit.collider.gameObject))
                {
                    if (hit.collider.tag == "Ennemy")
                    {
                        print("It's an ennemy " + hit.collider.name);
                        checkEnnemy.Add(hit.collider.gameObject);
                        EnnemyHP ennemyHP = hit.collider.GetComponent<EnnemyHP>();
                        ennemyHP.TakeDamage();
                    }

                    if (hit.collider.tag == "Wall")
                    {
                        print("It's a wall");
                    }

                    //if (!Physics.Raycast(transform.position, (hit.point - transform.position).normalized, out obstacleHit, Vector3.Distance(transform.position, hit.point), obstacleLayer))
                    //{
                    //    detectedEnnemy.Add(hit.collider.gameObject);
                    //    checkEnnemy.Add(hit.collider.gameObject);
                    //    ennemyHP = hit.collider.GetComponent<EnnemyHP>();

                    //    if (ennemyHP != null)
                    //    {
                    //        ennemyHP.TakeDamage();
                    //        print("test" + hit.collider.name);
                    //    }
                    //}
                }

            }
            Debug.DrawRay(transform.position, rayDirection * raycastDistance, Color.red, 5f);
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

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldown = true;
    }
}