using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CubeSurfer
{
	public class Cube : MonoBehaviour
	{
		private bool isCollection = false;
		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Obstacle")
			{
				if(isCollection)
				{
					EventManager.EventDestroyCube?.Invoke(this.gameObject);
				}
			}
			if(other.tag == "Cube")
			{
				if(!isCollection)
				{
					EventManager.EventAddCube?.Invoke(this.gameObject);
					isCollection = true;
				}
				
			}
		}
	}
}
