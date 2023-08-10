using System.Collections;
using UnityEngine;

namespace CubeSurfer
{
	public class Cube : MonoBehaviour, ICube
	{
		private bool _isCollection = false;
		[SerializeField] private GameObject ui;

		public bool IsCollection { get => _isCollection; set => _isCollection = value; }
		public void SetActive(bool active)
		{
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
				}
			}
			if (other.tag == "Cube")
			{
				if(!_isCollection)
				{
					EventManager.EventTakeCube?.Invoke(this);
					_isCollection = true;
					ui.SetActive(true);
					StartCoroutine(ShowUI());
				}
				
			}
		}

		private IEnumerator ShowUI()
		{
			yield return new WaitForSeconds(1f);
			ui.SetActive(false);
		}
	}
}
