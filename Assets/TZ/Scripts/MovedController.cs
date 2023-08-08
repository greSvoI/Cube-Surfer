using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeSurfer
{
	public class MovedController : MonoBehaviour
	{
		private Rigidbody rigidBody;
		private CharacterController characterController;

		[SerializeField] private float _speed;
		private void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
			characterController = GetComponent<CharacterController>();
		}
		private void Update()
		{
			//characterController.Move(transform.forward * _speed * Time.deltaTime);
			//rigidBody.AddForce(transform.forward * _speed * Time.deltaTime);

			characterController.Move(transform.forward * _speed * Time.deltaTime);	
		}
	}
}
