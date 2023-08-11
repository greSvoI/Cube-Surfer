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

		[Header("UI")]
		[SerializeField] private GameObject takeCubeUI;
		[SerializeField] private TextMeshProUGUI textCube;
		[SerializeField] private float _timeUI;


		[Header("Input")]
		[SerializeField] private float _speed;
		[SerializeField] private float _horizontalSpeed;
		[SerializeField] private float _verticalSpeed;

		[SerializeField] private float _positionX;


		[SerializeField] private GameObject magicCircle;
		[SerializeField] private GameObject mouse;

		[SerializeField] private List<Cube> cubeCollection = new List<Cube>();
		[SerializeField] private float _rotationSpeed = 15f;
		[SerializeField] private float _horizontalLimit = 2f;
		[SerializeField] private float _heightCube = 1f;
		[SerializeField] private float _lineTower;

		private int _totalCube = 0;
		private void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
		}
		private void Start()
		{
			EventManager.EventTakeCube += OnEventTakeCube;
			EventManager.EventLostCube += OnEventLostCube;
			EventManager.EventInput += OnEventInput;
		}

		private void OnEventInput(Vector2 vector)
		{
			_positionX = vector.x;
		}

		private void OnEventLostCube(Cube cube)
		{
			
			cubeCollection.Remove(cube);
		}

		private void OnEventTakeCube(Cube cube)
		{
			takeCubeUI.SetActive(true);
			StartCoroutine(ShowUI(_timeUI));
			_totalCube++;
			textCube.text = _totalCube.ToString();
			transform.position = new Vector3(transform.position.x,transform.position.y + _heightCube,transform.position.z);
			cube.transform.position = new Vector3(transform.position.x,_heightCube / 2, transform.position.z);
			cube.transform.SetParent(transform);
			cubeCollection.Add(cube);	
		}

		private void Update()
		{
			foreach (var cube in cubeCollection)
			{
				cube.transform.position = Vector3.Lerp(cube.transform.position,new Vector3(transform.position.x,cube.transform.position.y,transform.position.z),_verticalSpeed * Time.deltaTime);
				cube.transform.rotation = Quaternion.Lerp(cube.transform.rotation,Quaternion.identity, _verticalSpeed *  Time.deltaTime);	
			}
			float positionX = transform.position.x + _positionX * _horizontalSpeed * Time.deltaTime;
			positionX = Mathf.Clamp(positionX, -_horizontalLimit, _horizontalLimit);
			transform.position = new Vector3(positionX, transform.position.y, transform.position.z);

			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			magicCircle.transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if(transform.position.y  < cubeCollection.Count)
			{
				Debug.Log("gameOver Update");
			}
		}
		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Obstacle")
			{
				Debug.Log("gameOver Other");

				mouse.transform.parent = null;
				mouse.GetComponent<Rigidbody>().AddForce(Vector3.forward * 20f , ForceMode.Force);
			}
		}
		private IEnumerator ShowUI(float timeUI)
		{
			yield return new WaitForSeconds(timeUI);
			takeCubeUI.SetActive(false);
		}

	}
}
