using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSwitcher : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isControlEnabled = false;
    public bool isStuckToCeiling = true;

    public GameObject bodyObject; // Reference to the object with Body control script
    public GameObject tailTipObject; // Reference to the object with TailTip control script
    private HingeJoint2D tailTipHingeJoint; // Reference to the Hinge Joint 2D component on TailTip object

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyObject = GameObject.Find("Body");
        tailTipObject = GameObject.Find("TailTip");
        tailTipHingeJoint = tailTipObject.GetComponent<HingeJoint2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isStuckToCeiling = !isStuckToCeiling;
            ToggleFunctionality();
        }
    }

    private void ToggleFunctionality()
    {
        if (isStuckToCeiling)
        {
            EnableBodyControl();
            DisableTailTipControl();
            DisableTailTipHingeJoint();
        }
        else
        {
            DisableBodyControl();
            EnableTailTipControl();
            EnableTailTipHingeJoint();
        }
    }

    private void EnableBodyControl()
    {
        PlayerControl bodyControl = bodyObject.GetComponent<PlayerControl>();
        if (bodyControl != null)
        {
            bodyControl.enabled = true;
        }
    }

    private void DisableBodyControl()
    {
        PlayerControl bodyControl = bodyObject.GetComponent<PlayerControl>();
        if (bodyControl != null)
        {
            bodyControl.enabled = false;
        }
    }

    private void EnableTailTipControl()
    {
        PlayerControl tailTipControl = tailTipObject.GetComponent<PlayerControl>();
        if (tailTipControl != null)
        {
            tailTipControl.enabled = true;
        }
    }

    private void DisableTailTipControl()
    {
        PlayerControl tailTipControl = tailTipObject.GetComponent<PlayerControl>();
        if (tailTipControl != null)
        {
            tailTipControl.enabled = false;
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
}
