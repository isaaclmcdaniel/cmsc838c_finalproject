using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    public GameObject scorecardobject;
    public TextMeshProUGUI scorecard;
    public int score;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reacting to collision");
        score += 1;
        scorecard.text = "Score: " + score;   
        if (other.gameObject.CompareTag("Destination"))
        {
            transform.parent.gameObject.GetComponent<MainScript>().puckAtDest = true;
            transform.parent.gameObject.GetComponent<MainScript>().launchObject = other.gameObject;
            
            // display final score and add option to reset
            scorecard.text = "Score: " + score + "\nPress X to continue";  
        }
        
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
        score = 0;
        scorecard = scorecardobject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // scorecard.text = "Score: " + score + "\nNewline!!!";
        // score += 1;
    }
}
