using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class CharaControl : MonoBehaviour
{

    public Rigidbody m_rb;
    public Rigidbody Rb { get => m_rb; set => m_rb = value; }
    public CinemachineVirtualCamera cam;
    //public Animator m_animator;

    [Header("PlayerMouvement")]
    public bool isGrounded;
    public bool isMoving;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float mouvementDrag;
    [SerializeField] private float defaultDrag;

    [Header("FallSystem")]
    public bool isFalling = false;
    [SerializeField] private float gravityScale;

    [Header("WallCheck")]
    public float rayDistance;
    [SerializeField] private Vector3 raycastCheckDir;

    [Header("GroundCheck")]
    public float rayGroundDistance;
    [SerializeField] private Vector3 raycastCheckGroundDir;

    private Vector3 m_currentInput;


    private void Awake()
    {
        isGrounded = true;
        isMoving = true;
    }

    private void Update()
    {
        m_currentInput = Vector3.zero;
        m_currentInput.x = Input.GetAxis("Horizontal");
        m_currentInput.y = Input.GetAxis("Vertical");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(m_rb.velocity);
        WallCheck();
        GroundCheck();
        SetDrag();
        SetGravity();

        var targetVector = new Vector3(m_currentInput.x, 0, m_currentInput.y);

        // Normalizer le vecteur de la camera pour normalizer les déplacement en general
        Vector3 cameraForward  = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // Utiliser le normalize pour bouger en fonction de la cam qui se déplace
        var targetDirection = targetVector.x * cameraRight + targetVector.z * cameraForward;
        targetDirection.y = 0;

        var mouvementVector = Walking(targetDirection);
        lookDirection(mouvementVector);
        

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
        // On clamp la velocité du rigid body si le player est plus au sol
        if (isGrounded == false)
        {
         m_rb.velocity = Vector2.ClampMagnitude(m_rb.velocity, maxSpeed);
        } 

        if (isMoving == true)
        {
            var speed = moveSpeed * Time.fixedDeltaTime;
            var targetPosition = m_rb.velocity + targetDirection * speed; // On calcul la position par rapport a la velocity et la direction qu'on targer (qui a été normalizer)
            m_rb.velocity = targetPosition;


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
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground")
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
            if (isMoving == true)
            {
                m_rb.drag = mouvementDrag;
            }
            else m_rb.drag = defaultDrag;
        }
        else m_rb.drag = defaultDrag;
    }


}
