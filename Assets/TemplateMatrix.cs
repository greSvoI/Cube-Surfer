using CubeSurfer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMatrix : MonoBehaviour
{
    public ObstacleCube cube;
	public int totalCube;
	private void Start()
	{
		for (int height = 0; height < 5; height++)
		{
			if(height==0)
			{
				for(int width = -2; width < 3; width++)
				{
					Instantiate(cube, new Vector3(width,0, 0f), Quaternion.identity,transform);
				}
			}
			if(height==3)
			{
				for (int width = -2; width < 3; width++)
				{
					Instantiate(cube, new Vector3(width,Vector3.up.y * height, 0f), Quaternion.identity, transform);
				}
			}
		}
	}

}
