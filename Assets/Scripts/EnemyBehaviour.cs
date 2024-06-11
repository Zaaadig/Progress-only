using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask ground, whatIsPlayer; // bien mettre le layer du player la où il y a le collider
    public string playerName;
    private Rigidbody m_rb;

    [Header("Patrol")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public float attackDashForce;
    public float timeAttackAnim;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find(playerName).transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patrolling();
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();
    }
    private void Patrolling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Coder l'attaque ici
            StartCoroutine(Attack());
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private IEnumerator Attack()
    {
        //agent.enabled = false;
        alreadyAttacked = true;
        // lancer l'anim de predash
        transform.LookAt(player);
        yield return new WaitForSeconds(timeAttackAnim);
        m_rb.drag = 5;
        m_rb.AddForce(transform.forward * attackDashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        m_rb.drag = 0;
        //agent.enabled = true;
        
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
}
