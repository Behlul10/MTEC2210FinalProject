using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject bodyObject; // Reference to the object with Body control script
    public GameObject tailTipObject; // Reference to the object with TailTip control script
    private HingeJoint2D tailTipHingeJoint; // Reference to the Hinge Joint 2D component on TailTip object

    public float speed = 200;
    private float xMove;
    private float yMove;
    bool quit = false;

    private bool jumpFlag;
    public float jumpPower = 10;

    private int jumpCount = 0;
    private int maxJumpCount = 10;
    private bool isGrounded = true;

    public bool isControlEnabled = false;
    private bool isStuckToCeiling = true;

    private int specialCoinsCount = 0;
    private int timeDashed = 0;
    private int maxTimeDash = 10;
    private int CoinCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyObject = GameObject.Find("Body");
        tailTipObject = GameObject.Find("TailTip");
        tailTipHingeJoint = tailTipObject.GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        quit = Input.GetKey(KeyCode.Escape);

        if(quit)
        {
            Application.Quit();
        }

        //
        //
        //
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleStuckToCeiling();
            ToggleFunctionality();
        }*/
        //
        ///
        //
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xMove * speed * Time.deltaTime, rb.velocity.y);

        //rb.velocity = new Vector2(xMove * speed * Time.deltaTime, yMove * speed * Time.deltaTime);

        if (specialCoinsCount == 3 && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(xMove * speed * Time.deltaTime, yMove * speed * Time.deltaTime);
            speed = 850; 
        }
        else if (Input.GetKey(KeyCode.Space) && CoinCount <= maxTimeDash)
        {
            rb.velocity = new Vector2(xMove * speed * Time.deltaTime, yMove * speed * Time.deltaTime);
            speed = 400;
            timeDashed++;
            CoinCount--;
        }

    }
    /*
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
    */
    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            jumpCount = 0; // Reset jump count
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            CoinCount++;
        }

        if (other.gameObject.CompareTag("SpecialCoin"))
        {
            Destroy(other.gameObject);
            specialCoinsCount++;
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



