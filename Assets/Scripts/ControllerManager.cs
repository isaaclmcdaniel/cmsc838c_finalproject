using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject levelObject;
    public float moveControlSpeed = 1;
    public GameObject zoomAnchor;
    public GameObject zoomObject;
    public float zoomControlSpeed = 1;
    public GameObject gravityDrivenObject;

    private bool moveControlActive;
    private bool zoomControlActive;
    private Vector3 moveControlLastPos;
    private float zoomControlLastDist;
    bool GetMoveTrigger()
    {
        //
        float triggerDepth = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
        //Debug.Log("triggerDepth: " + triggerDepth);
        
        bool triggerDown = (triggerDepth > 0.2) ? true : false;

        return triggerDown;
    }
    
    bool GetZoomTrigger()
    {
        //
        float LTriggerDepth = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
        float RTriggerDepth = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
        //Debug.Log("triggerDepth: " + triggerDepth);
        
        bool triggerDown = ((LTriggerDepth > 0.2) && (RTriggerDepth > 0.2)) ? true : false;

        return triggerDown;
    }

    // void StartLevelMovement()
    // {
    //     levelObject.transform.SetParent( transform, true);
    // }

    // void StopLevelMovement()
    // {
    //     levelObject.transform.SetParent( null, true);
    // }

    void MoveControl()
    {
        if (moveControlActive)
        {
            // Update levelObject's position, but not rotation, according to left controller's movement
            levelObject.transform.Translate(transform.position - moveControlLastPos, Space.World);
        }
        
        if (GetMoveTrigger() && !moveControlActive)
        {
            //StartLevelMovement();
            moveControlActive = true;
        }
        else if (!GetMoveTrigger() && moveControlActive)
        {
            //StopLevelMovement();
            moveControlActive = false;
        }

        moveControlLastPos = transform.position;
    }
    
    void ZoomControl()
    {
        if (zoomControlActive)
        {
            // Update levelObject's position, but not rotation, according to left controller's movement
            //levelObject.transform.Translate(transform.position - moveControlLastPos, Space.World);
            float distance = Vector3.Distance(transform.position, zoomAnchor.transform.position);
            
            // Scale levelObject according to change in distance between controllers
            levelObject.transform.localScale = zoomObject.transform.localScale * distance / zoomControlLastDist;
            
            // Scale velocty of gravity-driven object according to change in distance between controllers
            gravityDrivenObject.GetComponent<Rigidbody>().velocity = gravityDrivenObject.GetComponent<Rigidbody>().velocity * distance / zoomControlLastDist;
        }
        
        if (GetZoomTrigger() && !zoomControlActive)
        {
            //StartLevelZoom();
            zoomControlActive = true;
        }
        else if (!GetZoomTrigger() && zoomControlActive)
        {
            //StopLevelZoom();
            zoomControlActive = false;
        }

        zoomControlLastDist = Vector3.Distance(transform.position, zoomAnchor.transform.position);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        moveControlActive = false;
        zoomControlActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            levelObject.GetComponent<MainScript>().LaunchUIButtonRelease();
        }        
    }

    void LateUpdate()
    {
        MoveControl();
        ZoomControl();
    }
}
