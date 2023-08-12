using System.Collections;
using UnityEngine;

namespace CubeSurfer
{
	public class Cube : MonoBehaviour, ICube
	{
		private bool _isCollection = false;
		private bool _isFree = true;
		private Rigidbody rigidBody;


		[SerializeField] private float _force = 10f;

		public bool IsCollection { get => _isCollection; set => _isCollection = value; }

		private void Start()
		{
			rigidBody = GetComponent<Rigidbody>();
		}
		public void SetActive(bool active)
		{
			_isCollection = active;
			_isFree = !active;
			this.gameObject.SetActive(active);
		}

		public void Spawn(float position, bool active)
		{
			transform.position = new Vector3(Random.Range(-2, 3), 0.5f, position);
			gameObject.SetActive(active);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Obstacle")
			{
				if (_isCollection && !_isFree)
				{	
					EventManager.EventLostCube?.Invoke(this);
					transform.parent = null;
					_isFree = true;
					Vector3 direction = new Vector3(Random.Range(-1, 2), 0f, -1);
					rigidBody.AddForce(direction * _force, ForceMode.Impulse);
				}
			}
			if (other.tag == "Cube")
			{
				if(!_isCollection && _isFree)
				{
					EventManager.EventTakeCube?.Invoke(this);
					_isFree = false;
					_isCollection = true;
				}
				
			}
		}
	}
}
