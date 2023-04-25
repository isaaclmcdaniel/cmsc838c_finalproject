using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
	public GameObject startObject;
	public GameObject destObject;
	public Material startObjectUIMaterial;
	public Material destObjectUIMaterial;

	private int gameState;
	private Material startObjectOrigMaterial;
	private Material destObjectOrigMaterial;

	void PaintLaunchUITextures()
	{
		startObjectOrigMaterial = startObject.GetComponent<MeshRenderer> ().material;
		destObjectOrigMaterial = destObject.GetComponent<MeshRenderer> ().material;

		startObject.GetComponent<MeshRenderer> ().material = startObjectUIMaterial;
		destObject.GetComponent<MeshRenderer> ().material = destObjectUIMaterial;
	}

	void RemoveLaunchUITextures()
	{
		startObject.GetComponent<MeshRenderer> ().material = startObjectOrigMaterial;
		destObject.GetComponent<MeshRenderer> ().material = destObjectOrigMaterial;
	}

	public void LaunchUIButtonRelease()
	{
		if(gameState == 1)
		{
			RemoveLaunchUITextures();
			gameState = 2;
		}
	}

	bool PackageLaunched()
	{
		return false;
	}

	void GameFSM()
	{
		switch(gameState)
		{	
		case 0:
			PaintLaunchUITextures();
			gameState = 1;
			break;
		case 1:
			// if launched, remove launch UI textures
			if(PackageLaunched())
			{
				RemoveLaunchUITextures();
				gameState = 2;
			}
			break;
		case 2:
			break;
			
		default:
			break;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        gameState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameFSM();
    }
}
