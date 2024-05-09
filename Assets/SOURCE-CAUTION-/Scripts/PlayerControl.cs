using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject bodyObject; // Reference to the object with Body control script
    public GameObject tailTipObject; // Reference to the object with TailTip control script
    private HingeJoint2D tailTipHingeJoint; // Reference to the Hinge Joint 2D component on TailTip object

    public float speed = 20;
    private float xMove;
    private float yMove;

    private bool jumpFlag;
    public float jumpPower = 10;

    private int jumpCount = 0;
    private int maxJumpCount = 10;
    private bool isGrounded = true;

    public bool isControlEnabled = false;
    private bool isStuckToCeiling = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyObject = GameObject.Find("Body");
        tailTipObject = GameObject.Find("TailTip");
        tailTipHingeJoint = tailTipObject.GetComponent<HingeJoint2D>();

        // Initially, enable the Body control script and disable the TailTip HingeJoint2D
        //EnableBodyControl();
        //DisableTailTipHingeJoint();
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpCount < maxJumpCount) // if isGrounded = T or jumped < 2 times
            {
                jumpFlag = true;
                jumpCount++;
                isGrounded = false; // Will be reset to true when the player touches the ground again

                /*if (jumpCount >= maxJumpCount)
                {
                    isGrounded = false; // Wiyll be reset to true when the player touches the ground again
                }*/
            }
        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleStuckToCeiling();
            ToggleFunctionality();
        }

    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(xMove * speed * Time.deltaTime, rb.velocity.y);
        rb.velocity = new Vector2(xMove * speed * Time.deltaTime, yMove * speed * Time.deltaTime);

        if (jumpFlag)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpFlag = false;
        }


    }

    private void ToggleStuckToCeiling()
    {
        isStuckToCeiling = !isStuckToCeiling;
        if (isStuckToCeiling)
        {
            EnableBodyControl();
            DisableTailTipHingeJoint();
        }
        else
        {
            EnableTailTipControl();
            EnableTailTipHingeJoint();
        }
    }
    private void ToggleFunctionality()
    {
        if (isStuckToCeiling)
        {
            DisableBodyControl();
            EnableTailTipControl();
        }
        else
        {
            EnableBodyControl();
            DisableTailTipControl();
        }
    }


    private void EnableBodyControl()
    {
        if (bodyObject != null)
        {
            PlayerControl bodyControl = bodyObject.GetComponent<PlayerControl>();
            if (bodyControl != null)
            {
                bodyControl.enabled = true;
            }
        }
    }

    private void DisableBodyControl()
    {
        if (bodyObject != null)
        {
            PlayerControl bodyControl = bodyObject.GetComponent<PlayerControl>();
            if (bodyControl != null)
            {
                bodyControl.enabled = false;
            }
        }
    }

    private void EnableTailTipControl()
    {
        if (tailTipObject != null)
        {
            PlayerControl tailTipControl = tailTipObject.GetComponent<PlayerControl>();
            if (tailTipControl != null)
            {
                tailTipControl.enabled = true;
            }
        }
    }

    private void DisableTailTipControl()
    {
        if (tailTipObject != null)
        {
            PlayerControl tailTipControl = tailTipObject.GetComponent<PlayerControl>();
            if (tailTipControl != null)
            {
                tailTipControl.enabled = false;
            }
        }
    }

    private void EnableTailTipHingeJoint()
    {
        if (tailTipHingeJoint != null)
        {
            tailTipHingeJoint.enabled = true;
        }
    }

    private void DisableTailTipHingeJoint()
    {
        if (tailTipHingeJoint != null)
        {
            tailTipHingeJoint.enabled = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            jumpCount = 0; // Reset jump count
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }


    }

}



