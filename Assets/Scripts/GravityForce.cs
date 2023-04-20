using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GravityForce : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    private Vector3 total_force;
    private GameObject obj;
    private Transform massParent;
    private List<GameObject> massiveObjects = new List<GameObject>();

    public float gravitationalConstant = 1;
    public GameObject massiveBodies;  // parent gameobject with children that have mass

    public Vector3 initialVelocity;

    public float mass;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.velocity = initialVelocity;
        
        obj = transform.parent.gameObject;
        massParent = obj.transform.Find("MassiveBodies");
        foreach (Transform child in massParent)
        {
            // check if child has mass (by checking if it has the mass component)
            if(child.gameObject.GetComponent<GravityProperties>() != null)
            {
                massiveObjects.Add(child.gameObject);
            }
            Debug.Log("Found massive body");
            // do any additional checks here as well
            // if it does, add it to massiveObjects
        }

        mass = m_Rigidbody.mass;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // float playermass = GetComponent<GravityProperties>().mass;
        total_force = new Vector3(0, 0, 0);
        /*
         *  for massiveobject in massiveObjects
         *      calculate direction vector and normalize it
         *      calculate force magnitude
         *      multiply them together and add it to total_force
         *  apply total force
         */
        foreach (GameObject massiveObject in massiveObjects)
        {
            Vector3 temp = massiveObject.transform.position - transform.position;
            float dist = temp.magnitude;
            total_force +=
                (gravitationalConstant * mass * massiveObject.GetComponent<GravityProperties>().mass / (dist * dist)) *
                temp.normalized;

        }
        Debug.Log("Total force: " + total_force);

        m_Rigidbody.AddForce(total_force, ForceMode.Force);
    }
}
