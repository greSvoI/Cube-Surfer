using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CubeSurfer
{
	public class PlayerController : MonoBehaviour
	{
		private Rigidbody rigidBody;


		[SerializeField] private float _speed;
		[SerializeField] private TextMeshProUGUI textCube;

		[SerializeField] private GameObject magicCircle;
		[SerializeField] private GameObject mouse;

		[SerializeField] private List<Cube> cubeCollection = new List<Cube>();

		[SerializeField] private float _heightCube = 1f;

		private int _totalCube = 0;
		private void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
		}
		private void Start()
		{
			EventManager.EventTakeCube += OnEventTakeCube;
			EventManager.EventLostCube += OnEventLostCube;
		}

		private void OnEventLostCube(Cube cube)
		{
			cube.transform.parent = null;
			cubeCollection.Remove(cube);
		}

		private void OnEventTakeCube(Cube cube)
		{
			_totalCube++;
			textCube.text = _totalCube.ToString();
			transform.position = new Vector3(transform.position.x,transform.position.y + _heightCube,transform.position.z);
			cube.transform.position = new Vector3(transform.position.x,_heightCube / 2, transform.position.z);
			cube.transform.SetParent(transform);
			cubeCollection.Add(cube);	
		}

		private void Update()
		{
			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			magicCircle.transform.Translate(Vector3.down * _speed * Time.deltaTime);
		}

	}
}
