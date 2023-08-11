using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;

	private Vector3 newPosition;
	private Vector3 offset;

	[SerializeField] private float lerpValue;



	void Start()
	{
		offset = transform.position - playerTransform.position;
	}

	void LateUpdate()
	{
		SetCameraFollow();
	}

	private void SetCameraFollow()
	{
		newPosition = Vector3.Lerp(transform.position, new Vector3(0f, playerTransform.position.y, playerTransform.position.z) + offset, lerpValue * Time.deltaTime);
		transform.position = newPosition;
	}
}
