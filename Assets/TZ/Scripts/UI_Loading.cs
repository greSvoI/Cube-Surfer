using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubeSurfer
{
	public class UI_Loading : MonoBehaviour
	{

	    [SerializeField] private GameObject progressBar;
		[SerializeField] private Image progressBarImage;
		[SerializeField] private TextMeshProUGUI progressText;
		[SerializeField] private TextMeshProUGUI highScoreText;
		[SerializeField] private GameObject buttonPressTap;
		[SerializeField] private GameObject buttonPressRestart;
		[SerializeField] private GameObject showGameUI;
		[SerializeField] private GameObject showMenuUI;

		AsyncOperation asyncSceneLoad;
		private void Start()
		{
			if (SceneManager.GetActiveScene().name == "Menu") StartCoroutine("AsyncLoadScene", PlayerPrefs.GetString("current_scene"));
			EventManager.EventGameOver += OnEventGameOver;
			highScoreText.text = PlayerPrefs.GetInt("_highScore").ToString();
		}

		private void OnEventGameOver()
		{
			buttonPressRestart.SetActive(true);
		}

		private IEnumerator AsyncLoadScene()
		{
			float progress;

			asyncSceneLoad = SceneManager.LoadSceneAsync(1);
			progressBar.SetActive(true);

			asyncSceneLoad.allowSceneActivation = false;
			while (asyncSceneLoad.progress < 0.9f)
			{
				progress = Mathf.Clamp01(asyncSceneLoad.progress / 0.9f);

				progressBarImage.fillAmount = progress;
				yield return null;
			}
			progressBar.SetActive(false);
			buttonPressTap.SetActive(true);
		}
		public void PressButtonRestart()
		{
			SceneManager.LoadScene(0);
		}
		public void PressButtonStart()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(1);
		}
		public void PressButtonMenu() {

			Time.timeScale = 0f;
			showGameUI.SetActive(false);
			showMenuUI.SetActive(true);
		}
		public void PressButtonSave()
		{
			showGameUI.SetActive(true);
			showMenuUI.SetActive(false);
			Time.timeScale = 1f;
		}
		private void OnDestroy()
		{
			EventManager.EventGameOver -= OnEventGameOver;
		}

	}
}
