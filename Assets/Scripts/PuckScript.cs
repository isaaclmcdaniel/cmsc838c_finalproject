using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reacting to collision");
        
        // Set new start object
        if (other.gameObject.CompareTag("Launchable"))
        {
            transform.parent.gameObject.GetComponent<MainScript>().launchObject = other.gameObject;
        }
        
        // Call ReadyLaunch()
        transform.parent.gameObject.GetComponent<MainScript>().ReadyLaunch();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
