using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCubes : MonoBehaviour
{
    public GameObject CubesPrefab;
    GameObject[] cubes = new GameObject[256];
    public float maxScale;
    bool isInMainMenu = true;

    void Start()
    {
        CreateCubesInScene();   
    }

    void CreateCubesInScene()
    {
        for (int i = 0; i < 256; i++)
        {
            GameObject instaceCube = (GameObject)Instantiate(CubesPrefab);
            instaceCube.transform.position = this.transform.position;
            instaceCube.transform.parent = this.transform;
            instaceCube.name = "cube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instaceCube.transform.position = Vector3.forward * 100;
            cubes[i] = instaceCube;
        }
    }

    void Update()
    {
        for(int i = 0; i< 256; i++)
        {
            if(cubes != null) 
            {
                cubes[i].transform.localScale = new Vector3(10, (MainMenuAudio.samples[i]* maxScale) + 2, 10);
}
        }
    }

    public void EnterOptionsMenu()
    {
        isInMainMenu = false;
        SetCubesActive(false);
    }

    public void ReturnToMainMenu()
    {
        isInMainMenu = true;
        SetCubesActive(true);
    }

    void SetCubesActive(bool isActive)
    {
        foreach (var cube in cubes)
        {
            if (cube != null)
            {
                cube.SetActive(isActive);
            }
        }
    }
    
    
}
