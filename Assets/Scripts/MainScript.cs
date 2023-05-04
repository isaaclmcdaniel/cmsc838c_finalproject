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

    private bool puckFlying;
	private int gameState;
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
		
		spacePuck.SetActive(true);
		spacePuck.GetComponent<Rigidbody>().AddForce(launchControlVector, ForceMode.VelocityChange);
	}

	public void ReadyLaunch()
	{
		PaintLaunchUIPlanetTextures();

		spacePuck.SetActive(false);
		spacePuck.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		spacePuck.transform.position = launchObject.transform.position;
		
		Physics.IgnoreCollision(spacePuck.GetComponent<Collider>(), launchObject.GetComponent<Collider>(), true);
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
	    
    }
}
