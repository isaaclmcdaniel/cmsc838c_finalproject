using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
	public GameObject startObject;
	public GameObject launchObject;
	public GameObject destObject;
	public Material startObjectUIMaterial;
	public Material destObjectUIMaterial;

    public GameObject spacePuck;
    public bool puckFlying;
    public float launchPositionAdjustment;
    public float launchVelocityMultiplier;

	private Material launchObjectOrigMaterial;
	private Material destObjectOrigMaterial;

	void PaintLaunchUIPlanetTextures()
	{
		launchObjectOrigMaterial = launchObject.GetComponent<MeshRenderer> ().material;
		destObjectOrigMaterial = destObject.GetComponent<MeshRenderer> ().material;

		launchObject.GetComponent<MeshRenderer> ().material = startObjectUIMaterial;
		destObject.GetComponent<MeshRenderer> ().material = destObjectUIMaterial;
	}

	void RemoveLaunchUIPlanetTextures()
	{
		launchObject.GetComponent<MeshRenderer> ().material = launchObjectOrigMaterial;
		destObject.GetComponent<MeshRenderer> ().material = destObjectOrigMaterial;
	}

	public void LaunchPuck(Vector3 launchControlVector)
	{
		RemoveLaunchUIPlanetTextures();
		
		spacePuck.transform.position = launchObject.transform.position + Vector3.Normalize(launchControlVector) * launchPositionAdjustment * transform.localScale.x;
		spacePuck.transform.rotation = Quaternion.LookRotation(launchControlVector, Vector3.up);
		spacePuck.transform.Rotate(90, 0, 0, Space.Self);
		
		spacePuck.SetActive(true);
		spacePuck.GetComponent<Rigidbody>().AddForce(launchControlVector * launchVelocityMultiplier * transform.localScale.x, ForceMode.VelocityChange);
		puckFlying = true;
	}

	public void ReadyLaunch()
	{
		PaintLaunchUIPlanetTextures();

		puckFlying = false;
		spacePuck.SetActive(false);
		spacePuck.GetComponent<Rigidbody>().velocity = Vector3.zero;
		spacePuck.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		spacePuck.transform.position = launchObject.transform.position;
	}

	public void PuckRelaunch()
	{
		ReadyLaunch();
	}

	public void PuckReset()
	{
		launchObject = startObject;
		ReadyLaunch();
	}

    // Start is called before the first frame update
    void Start()
    {
	    puckFlying = false;
	    launchObject = startObject;
	    ReadyLaunch();
    }

    // Update is called once per frame
    void Update()
    {
	    // Keep puck pointing in the direction of motion
	    spacePuck.transform.rotation = Quaternion.LookRotation(spacePuck.GetComponent<Rigidbody>().velocity, Vector3.up);
	    spacePuck.transform.Rotate(90, 0, 0, Space.Self);
    }
}
