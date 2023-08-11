using System.Collections;
using UnityEngine;

namespace CubeSurfer
{
	public class Cube : MonoBehaviour, ICube
	{
		private bool _isCollection = false;
		private Rigidbody _body;

		[SerializeField] private GameObject ui;
		[SerializeField] private float _timeUI = 1f;
		[SerializeField] private float _timeFlight = 2f;
		[SerializeField] private float _force = 10f;
		[SerializeField] private float _radius = 50f;

		public bool IsCollection { get => _isCollection; set => _isCollection = value; }

		private void Start()
		{
			_body = GetComponent<Rigidbody>();
		}
		public void SetActive(bool active)
		{
			_isCollection = active;
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
				if (_isCollection)
				{	
					EventManager.EventLostCube?.Invoke(this);
					transform.parent = null;
					StartCoroutine(FlightCube(_timeFlight));
				}
			}
			if (other.tag == "Cube")
			{
				if(!_isCollection)
				{
					EventManager.EventTakeCube?.Invoke(this);
					_isCollection = true;
					ui.SetActive(true);
					StartCoroutine(ShowUI(_timeUI));
				}
				
			}
		}
		private IEnumerator ShowUI(float timeUI)
		{
			yield return new WaitForSeconds(timeUI);
			ui.SetActive(false);
		}

		private IEnumerator FlightCube(float timeFly)
		{
			Vector3 direction = new Vector3(Random.Range(-1, 2), 0f, Random.Range(-1, 2));
			_body.AddForce(direction * _force , ForceMode.Impulse);
			yield return new WaitForSeconds(timeFly);

			SetActive(false);
		}
	}
}
