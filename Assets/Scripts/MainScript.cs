using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
	public GameObject startObject;
	public GameObject destObject;
	public Material startObjectUIMaterial;
	public Material destObjectUIMaterial;

    public GameObject spacePuck;

	private int gameState;
	private Material startObjectOrigMaterial;
	private Material destObjectOrigMaterial;

	void PaintLaunchUIPlanetTextures()
	{
		startObjectOrigMaterial = startObject.GetComponent<MeshRenderer> ().material;
		destObjectOrigMaterial = destObject.GetComponent<MeshRenderer> ().material;

		startObject.GetComponent<MeshRenderer> ().material = startObjectUIMaterial;
		destObject.GetComponent<MeshRenderer> ().material = destObjectUIMaterial;
	}

	void RemoveLaunchUIPlanetTextures()
	{
		startObject.GetComponent<MeshRenderer> ().material = startObjectOrigMaterial;
		destObject.GetComponent<MeshRenderer> ().material = destObjectOrigMaterial;
	}

    private void LaunchUIButtonRelease()
    {
        if(gameState == 1)
        {
            RemoveLaunchUIPlanetTextures();
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
			PaintLaunchUIPlanetTextures();
			gameState = 1;
			break;
		case 1:
			// if launched, remove launch UI textures
			if(PackageLaunched())
			{
				RemoveLaunchUIPlanetTextures();
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

        spacePuck.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameFSM();
    }
}
