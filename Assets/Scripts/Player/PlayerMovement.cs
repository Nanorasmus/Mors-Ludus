using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turningSensitivity = 3f;

    [SerializeField] float jumpPower = 5f;
    [SerializeField] int maxJumpCount = 3;

    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float sprintingModifier = 2f;

    [SerializeField] float sidewaysSpeedMultiplier = 0.9f;
    [SerializeField] float backwardsSpeedMultiplier = 0.7f;

    Rigidbody playerRB;
    Animator playerAnimator;

    int jumpCount = 0;
    bool inAir = true;

    public void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = transform.GetChild(0).GetComponent<Animator>();

        Vector3 spawnPosition = new Vector3(Random.Range(25, 75), 10, Random.Range(25, 75));
        transform.position = spawnPosition;
    }

    void Update()
    {
        playerAnimator.SetFloat("X", playerRB.velocity.x);
        playerAnimator.SetFloat("Z", playerRB.velocity.z);

        //Inputs
        if (Input.GetAxisRaw("Mouse X") != 0f)
        {
            transform.Rotate(new Vector3(0, (Input.GetAxisRaw("Mouse X") * turningSensitivity)));
        }
        if (Input.GetAxisRaw("Vertical") != 0f || Input.GetAxisRaw("Horizontal") != 0f || Input.GetKeyDown(KeyCode.Space))
        {
            float x = Input.GetAxisRaw("Horizontal") * walkingSpeed;
            float y = playerRB.velocity.y;
            float z = Input.GetAxisRaw("Vertical") * walkingSpeed;

            if (z < 0) z *= backwardsSpeedMultiplier;
            x *= sidewaysSpeedMultiplier;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                x *= sprintingModifier;
                z *= sprintingModifier;
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
            {
                y = jumpPower;
                jumpCount--;
                inAir = true;
            }

            playerRB.velocity = transform.right * x + transform.up * y + transform.forward * z;
        } else if (!inAir)
        {
            playerRB.velocity = Vector3.zero;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            jumpCount = maxJumpCount;
            inAir = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            inAir = true;
        }
    }
}
