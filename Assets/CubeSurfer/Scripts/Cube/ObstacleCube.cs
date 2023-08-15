using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeSurfer
{
	[RequireComponent(typeof(Rigidbody))]
	public class ObstacleCube : MonoBehaviour
	{
		private bool _isActive = false;
		public int Step;
		public bool isActive { get => _isActive; set => _isActive = value; }

		public void Spawn(float position, bool active)
		{
			_isActive = active;
			gameObject.SetActive(active);
			transform.position = new Vector3(0f, 0f, position);
		}
		public void SetActive(bool active)
		{
			_isActive = active;
			gameObject.SetActive(active);
		}
	}

}
