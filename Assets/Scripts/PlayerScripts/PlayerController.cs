using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("ÒÆ¶¯ÓëÐý×ª")]
    public Transform cam;
    public float moveSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    public float rotateSpeed = 4f;
    float turnSmoothVelocity = 0, angle = 0;

    [HideInInspector]
    public Vector3 moveDir;
    [HideInInspector]
    public float targetAngle;
    [HideInInspector]
    public Vector3 direction;

    Vector3 moveAmount;
    [HideInInspector]
    public Rigidbody rb;

    [Header("ÌøÔ¾")]
    public float jumpSpeed = 1f;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    private bool isGround, jumpPressed = false;
    int jumpCount = 0;

    [Header("Ð±ÆÂ¼ì²â")]
    public float rayLength = 1f;
    public float maxSlopeAngle = 45f;
    Vector3 hitNormal;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Jump();
    }

    private void FixedUpdate()
    {
        //PlayerMovement();
        if (direction.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + moveAmount);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        if (isGround)
        {
            jumpCount = 1;
        }
        if(jumpPressed && isGround)
        {
            rb.velocity += new Vector3(0, jumpSpeed, 0);
            jumpCount--;
            jumpPressed = false;
        }

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void PlayerMovement()
    {
        //Î»ÒÆ
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        if (OnSlope())
        {
            moveDir = (Vector3.ProjectOnPlane(moveDir, hitNormal)).normalized;
        }

        moveAmount = moveDir.normalized * moveSpeed * 0.01f;

        //Ðý×ª
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
    }

    private void Jump()
    {
        //ÂäµØÅÐ¶Ï
        isGround = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
        //ÅÐ¶ÏÌøÔ¾
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }

    bool OnSlope()
    {
        //ÉäÏß¼ì²âÅÐ¶ÏÐ±ÆÂ
        Ray ray = new Ray(groundCheck.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, rayLength))
        {
            hitNormal = hit.normal;
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if(slopeAngle < maxSlopeAngle)
            {
                return true;
            }
        }
        return false;
    }
}
