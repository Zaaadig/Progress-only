using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CharaControl : MonoBehaviour
{
    private InputHandler _input;
    public Rigidbody m_rb;
    public CinemachineVirtualCamera cam;
    //public Animator m_animator;

    [Header("PlayerMouvement")]
    public bool isGrounded;
    public bool isMoving;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("WallCheck")]
    public float rayDistance;
    [SerializeField] private Vector3 raycastCheckDir;

    [Header("GroundCheck")]
    public float rayGroundDistance;
    [SerializeField] private Vector3 raycastCheckGroundDir;

    private void Awake()
    {
        m_rb.drag = 0f;
        isGrounded = true;
        isMoving = true;
        _input = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WallCheck();
        GroundCheck();
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var targetDirection = targetVector.x * cam.transform.right + targetVector.z * cam.transform.forward;
        targetDirection.y = 0;
        var mouvementVector = Walking(targetDirection);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            lookDirection(mouvementVector);
        }

        //if (targetDirection.x != 0 || targetDirection.z != 0)
        //{
        //    m_animator.SetBool("isWalking", true);
        //}
        //else m_animator.SetBool("isWalking", false);
    }

    public void lookDirection(Vector3 mouvementVector)
    {
        if (mouvementVector.magnitude == 0)
        {
            return;
        }
        var rotation = Quaternion.LookRotation(mouvementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }
    public Vector3 Walking(Vector3 targetDirection)
    {
        if (isMoving == true && isGrounded == true)
        {
            var speed = moveSpeed * Time.fixedDeltaTime;


            var targetPosition = transform.position + targetDirection * speed;
            transform.position = targetPosition;
        }
        return targetDirection;
    }

    private void WallCheck()
    {
        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(this.transform.position, this.transform.TransformDirection(direction * rayDistance));
        Debug.DrawRay(this.transform.position, this.transform.TransformDirection(direction * rayDistance));

        if (Physics.Raycast(theRay, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.tag == "Wall")
            {
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
        }
        else
        {
            isMoving = true;
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
}
