using CubeSurfer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform mouseTransform;
	[SerializeField] private Transform targetTransform;

	private Vector3 newPosition;
	private Vector3 targetOffset;

	[SerializeField] private Vector3 offsetMouse;
	[SerializeField] private Vector3 offsetPlayer;
	[SerializeField] private float _lerpValue;

	private bool _switch = true;

	void Start()
	{
		EventManager.EventGameOver += OnGameOver;
		targetOffset = offsetPlayer;
	}

	private void OnGameOver()
	{
		//_switch = false;
		targetOffset = offsetMouse;
	}
	private void Update()
	{
		targetTransform = playerTransform;
	}
	void LateUpdate()
	{
		SetCameraFollow();
	}

	private void SetCameraFollow()
	{
		newPosition = Vector3.Lerp(transform.position, new Vector3(0f, targetTransform.position.y, targetTransform.position.z) + targetOffset, _lerpValue * Time.deltaTime);
		transform.position = newPosition;
	}
	private void OnDestroy()
	{

		EventManager.EventGameOver -= OnGameOver;
	}
}
