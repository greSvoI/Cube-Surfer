using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubeSurfer
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameObject startUI;
		[SerializeField] private GameObject gameUI;
		[SerializeField] private GameObject restartUI;

		public Button startButton;
		public Button restartButton;

		private void Awake()
		{
			gameUI.SetActive(false);
			restartUI.SetActive(false);

			Time.timeScale = 0f;
			EventManager.EventGameOver += OnEventGameOver;
		}

		private void OnEventGameOver()
		{
			StartCoroutine(Restart(3f));
		}
		private IEnumerator Restart(float v)
		{
			yield return new WaitForSeconds(v);
			Time.timeScale = 0f;
			gameUI.SetActive(false);
			restartUI.SetActive(true);
		}

		public void StartGame()
		{
			Time.timeScale = 1f;
			startUI.SetActive(false);
			gameUI.SetActive(true);
		}
		public void RestartLevel()
		{
			restartUI.SetActive(false);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
