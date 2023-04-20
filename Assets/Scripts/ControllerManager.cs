using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject levelObject;

    private bool moveControlActive;
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
        
        if (GetTrigger() && !moveControlActive)
        {
            StartLevelMovement();
            moveControlActive = true;
        }
        else if (!GetTrigger() && moveControlActive)
        {
            StopLevelMovement();
            moveControlActive = false;
        }
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
    }
}
