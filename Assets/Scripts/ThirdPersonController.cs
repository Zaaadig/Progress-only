using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ThirdPersonController : MonoBehaviour
{

    [SerializeField] private Rigidbody m_rb;
    public Rigidbody Rb { get => m_rb; set => m_rb = value; }

    public Transform cam;
    private Vector3 m_input;
    private Vector3 moveDirection;


    [Header("PlayerMouvement")]
    public bool isGrounded;
    public bool canMove;
    public float moveSpeed;
    public float maxSpeed;
    public float rotateSpeed;

    [Header("DashSystem")]
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;
    public bool canDash;
    public bool isDashing;

    [Header("FallSystem")]
    public bool isFalling = false;
    [SerializeField] private float gravityScale;
    [SerializeField] private float mouvementDrag;
    [SerializeField] private float defaultDrag;

    [Header("GroundCheck")]
    public float rayGroundDistance;
    [SerializeField] private Vector3 raycastCheckGroundDir;

    [Header("WallCheck")]
    public float rayDistance;
    [SerializeField] private Vector3 raycastCheckDir;

    private void Awake()
    {
        isGrounded = true;
        canMove = true;
    }

    private void Update()
    {
        SetGravity();
        SetDrag();
        GroundCheck();
        WallCheck();

        m_input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        var targetDirection = m_input;
        LookRotation(targetDirection);

        if (Input.GetKeyDown(KeyCode.Space) && canDash == true && isGrounded == true)
        {
            StartCoroutine(DashFrame());
            StartCoroutine(DashCooldown());
        }

    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LookRotation(Vector3 targetDirection)
    {

        if ( targetDirection.magnitude == 0)
        {
            return;
        }

        targetDirection = cam.forward * m_input.z;
        targetDirection += cam.right * m_input.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    private void Move()
    {
        moveDirection = cam.forward * m_input.z;
        moveDirection += cam.right * m_input.x;
        moveDirection.Normalize();
        moveDirection.y = 0;
       
        if (isGrounded == false) 
        {
            m_rb.AddForce(moveDirection * (moveSpeed / 10) * Time.fixedDeltaTime, ForceMode.Force);
            m_rb.velocity = Vector3.ClampMagnitude(m_rb.velocity, maxSpeed);
        }

        if (canMove == true)
        {
            m_rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode.Force);
            m_rb.velocity = Vector3.ClampMagnitude(m_rb.velocity, maxSpeed);
        }
    }
    private void SetGravity()
    {
        if (isGrounded == false)
        {
            m_rb.AddForce(Physics.gravity * gravityScale); // On va set une force de gravité par rapport a la gravité du projet et un multiplicateur (le drag influe)
        }

    }

    private void SetDrag()
    {
        // on set le drag pour ne pas glisser
        if (isGrounded == true)
        {
            if (canMove == true)
            {
                m_rb.drag = mouvementDrag;
            }
            else m_rb.drag = defaultDrag;
        }
        else m_rb.drag = defaultDrag;
    }

    private void WallCheck()
    {
        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(this.transform.position, this.transform.TransformDirection(direction * rayDistance));
        Debug.DrawRay(this.transform.position, this.transform.TransformDirection(direction * rayDistance));

        if (Physics.Raycast(theRay, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground")
            {
                canMove = false;
            }
            else
            {
                canMove = true;
            }
        }
        else
        {
            canMove = true;
        }
    }
        private void GroundCheck()
    {
        Vector3 groundDirection = Vector3.down;
        Ray groundRay = new Ray(this.transform.position, this.transform.TransformDirection(groundDirection * rayGroundDistance));
        Debug.DrawRay(this.transform.position, this.transform.TransformDirection(groundDirection * rayGroundDistance));

        if (Physics.Raycast(groundRay, out RaycastHit hit, rayGroundDistance))
        {
            if (hit.collider.tag == "Ground")
            {
                isGrounded = true;

            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private IEnumerator DashFrame()
    {
        isDashing = true;
        canMove = false;
        Vector3 dashDirection = m_rb.velocity.normalized;
        m_rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(dashTime);
        canMove = true;
        isDashing = false;
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
    