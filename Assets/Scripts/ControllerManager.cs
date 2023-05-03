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
    public GameObject springUILine;
    public GameObject springSelectObject;

    private bool moveControlActive;
    private Vector3 moveControlLastPos;
    private bool zoomControlActive;
    private float zoomControlLastDist;
    private bool puckLanded;
    private bool launchControlActive;
    private bool launchTriggerLast;
    private LineRenderer springUILineRender;
    
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
    
    bool GetLaunchTrigger()
    {
        //
        float triggerDepth = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
        //Debug.Log("triggerDepth: " + triggerDepth);
        
        bool triggerDown = (triggerDepth > 0.2) ? true : false;

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
            float anchorOffset = Vector3.Distance(transform.position, zoomAnchor.transform.position);
            
            Vector3 levelOffset = transform.position - levelObject.transform.position;
            float levelOffsetNorm = levelOffset.magnitude;
            
            // Scale levelObject according to change in distance between controllers
            levelObject.transform.localScale = zoomObject.transform.localScale * anchorOffset / zoomControlLastDist;
            levelObject.transform.position = transform.position - levelOffset * anchorOffset / zoomControlLastDist;
            
            // Scale velocty of gravity-driven object according to change in distance between controllers
            gravityDrivenObject.GetComponent<Rigidbody>().velocity = gravityDrivenObject.GetComponent<Rigidbody>().velocity * anchorOffset / zoomControlLastDist;
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

    void LaunchControl()
    {
        if (puckLanded)
        {
            // Activate launch UI when the right trigger is pressed close to the start planet
            if (!launchControlActive && !launchTriggerLast && GetLaunchTrigger() && (Vector3.Distance(springSelectObject.transform.position,
                    levelObject.GetComponent<MainScript>().startObject.transform.position) < 0.2) )
            {
                launchControlActive = true;
                springUILine.SetActive(true);
            }
        }
        
        if (launchControlActive)
        {
            // Draw springUILine each frame
            var points = new Vector3[2];
            points[0] = springSelectObject.transform.position;
            points[1] = levelObject.GetComponent<MainScript>().startObject.transform.position;
            springUILineRender.SetPositions(points);

            if (!GetLaunchTrigger())
            {
                launchControlActive = false;
                springUILine.SetActive(false);
                puckLanded = false;
                
                // Launch spacePuck
                Vector3 launchControlVector = points[1] - points[0];
            }
        }

        launchTriggerLast = GetLaunchTrigger();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        moveControlActive = false;
        zoomControlActive = false;
        puckLanded = true;
        launchControlActive = false;
        launchTriggerLast = false;
        
        springUILineRender = springUILine.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.GetUp(OVRInput.Button.One))
        //{
            // levelObject.GetComponent<MainScript>().LaunchUIButtonRelease();
        //}

        LaunchControl();
    }

    void LateUpdate()
    {
        MoveControl();
        ZoomControl();
    }
}
