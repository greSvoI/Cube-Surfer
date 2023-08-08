using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CubeSurfer
{
	public class Cube : MonoBehaviour
	{
		private Rigidbody rb;

		[SerializeField] private float force;
		private void Start()
		{
			rb = GetComponent<Rigidbody>();
		}
		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Player")
			{
				this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.right * force, ForceMode.Force);
			}
		}
	}
}
