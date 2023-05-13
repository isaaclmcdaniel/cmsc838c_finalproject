using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerManager : MonoBehaviour
{
    public bool isTutorial;
    
    public float moveControlSpeed;
    public float omsThrustScaling = 1;
    
    public GameObject levelObject;
    public GameObject zoomAnchor;
    public GameObject zoomObject;
    public GameObject gravityDrivenObject;
    public GameObject springUILine;
    public GameObject springSelectObject;
    public GameObject puckObject;

    private bool moveControlActive;
    private Vector3 moveControlLastPos;
    private bool zoomControlActive;
    private float zoomControlLastDist;
    private bool launchControlActive;
    private bool launchTriggerLast;
    private LineRenderer springUILineRender;
    private MainScript levelpuckscript;
    private Rigidbody puckRigidbody;

    bool GetContinueButton()
    {
        return OVRInput.GetUp(OVRInput.Button.Three);
    }

    bool GetRelaunchButton()
    {
        return OVRInput.GetUp(OVRInput.Button.One);
    }

    bool GetResetButton()
    {
        return OVRInput.GetUp(OVRInput.Button.Two);
    }
    
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
            levelObject.transform.Translate((transform.position - moveControlLastPos) * moveControlSpeed, Space.World);
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
        if (!levelpuckscript.puckFlying)
        {
            // Activate launch UI when the right trigger is pressed close to the start planet
            if (!launchControlActive && !launchTriggerLast && GetLaunchTrigger() && (Vector3.Distance(springSelectObject.transform.position,
                    levelpuckscript.launchObject.transform.position) < 0.2) )
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
            points[1] = levelpuckscript.launchObject.transform.position;
            springUILineRender.SetPositions(points);

            if (!GetLaunchTrigger())
            {
                launchControlActive = false;
                springUILine.SetActive(false);
                
                // Launch spacePuck
                Vector3 launchControlVector = points[1] - points[0];
                levelpuckscript.LaunchPuck(launchControlVector);
            }
        }

        launchTriggerLast = GetLaunchTrigger();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        moveControlActive = false;
        zoomControlActive = false;
        launchControlActive = false;
        launchTriggerLast = false;
        
        springUILineRender = springUILine.GetComponent<LineRenderer>();

        levelpuckscript = levelObject.GetComponent<MainScript>();
        puckRigidbody = puckObject.GetComponent<Rigidbody>();
    }

    void ApplyCourseCorrection()
    {
        // thumstick control: forward to thrust in prograde direction, backwards for retrograde
        var direction = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Touch);
        // just care about y direction for now (prograde/retrograde)
        // in the future maybe use x direction and controller orientation for roll/yaw stuff

        var totalForce = new Vector3(0, 0, 0);
        puckRigidbody.AddForce(puckRigidbody.velocity.normalized * (omsThrustScaling * direction.y));
    }

    private void FixedUpdate()
    {
        if (levelpuckscript.puckFlying)
        {
            ApplyCourseCorrection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LaunchControl();

        if (levelpuckscript.puckFlying)
        {
            if (GetRelaunchButton())
            {
                // call reset function in MainScript
                levelpuckscript.PuckRelaunch();
            }

            if (GetResetButton())
            {
                // call reset function in MainScript
                levelpuckscript.PuckReset();
            }
        }
    }

    void LateUpdate()
    {
        MoveControl();
        ZoomControl();

        if (levelObject.GetComponent<MainScript>().puckAtDest && GetContinueButton())
        {
            // Change scene
            if (isTutorial)
            {
                SceneManager.LoadScene("Scenes/LevelScene");
                isTutorial = false;
            }
            else
            {
                {
                    SceneManager.LoadScene("Scenes/TutorialScene");
                    isTutorial = true;
                }
            }
        }
    }
}
