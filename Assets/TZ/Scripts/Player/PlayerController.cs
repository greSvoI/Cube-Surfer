using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace CubeSurfer
{
	public class PlayerController : MonoBehaviour
	{
		private Rigidbody rigidBody;


		[SerializeField] private float _speed;
		[SerializeField] private GameObject magicCircle;
		[SerializeField] private GameObject mouse;
		[SerializeField] private GameObject lastCube;
		[SerializeField] private List<GameObject> cubeCollection = new List<GameObject>();
		[SerializeField] private float _heightCube = 1f;

		private void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
			cubeCollection.Add(lastCube);
		}
		private void Start()
		{
			EventManager.EventAddCube += OnEventAddCube;
			EventManager.EventDestroyCube += OnEventDestroy;
		}

		private void OnEventDestroy(GameObject obj)
		{
			obj.transform.parent = null;
			cubeCollection.Remove(obj);
			Destroy(obj,1f);
		}

		private void OnEventAddCube(GameObject obj)
		{
			transform.position = new Vector3(transform.position.x,transform.position.y + 1f, transform.position.z);
			obj.transform.position = new Vector3(transform.position.x,0.5f, transform.position.z);
			obj.transform.SetParent(transform);
			cubeCollection.Add(obj);
		}

		private void Update()
		{
			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			magicCircle.transform.Translate(Vector3.down * _speed * Time.deltaTime);

			//foreach (var cube in transform.GetComponentsInChildren<Transform>()) 
			//{
			//	cube.position = new Vector3(transform.position.x,cube.position.y, transform.position.z);
			//}
		}

	}
}
