using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject levelObject;
    public float moveControlSpeed = 1;

    private bool moveControlActive;
    private Vector3 moveControlLastPos;
    bool GetTrigger()
    {
        //
        float triggerDepth = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
        //Debug.Log("triggerDepth: " + triggerDepth);
        
        bool triggerDown = (triggerDepth > 0.2) ? true : false;

        return triggerDown;
    }

    void StartLevelMovement()
    {
        levelObject.transform.SetParent( transform, true);
    }

    void StopLevelMovement()
    {
        levelObject.transform.SetParent( null, true);
    }

    void MoveControl()
    {
        if (moveControlActive)
        {
            // Update levelObject's position, but not rotation, according to left controller's movement
            levelObject.transform.Translate(transform.position - moveControlLastPos, Space.World);
        }
        
        if (GetTrigger() && !moveControlActive)
        {
            //StartLevelMovement();
            moveControlActive = true;
        }
        else if (!GetTrigger() && moveControlActive)
        {
            //StopLevelMovement();
            moveControlActive = false;
        }

        moveControlLastPos = transform.position;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        moveControlActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            levelObject.GetComponent<MainScript>().LaunchUIButtonRelease();
        }        
    }
}
