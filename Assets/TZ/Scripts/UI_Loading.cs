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

		[SerializeField] private Slider musicVolume;

		private bool _vibration = true;

		AsyncOperation asyncSceneLoad;
		private void Start()
		{
			if (SceneManager.GetActiveScene().name == "Menu") StartCoroutine("AsyncLoadScene", PlayerPrefs.GetString("current_scene"));

			EventManager.EventGameOver += OnEventGameOver;
			EventManager.EventLostCube += OnEventLostCube;
			highScoreText.text = PlayerPrefs.GetInt("_highScore").ToString();
		}

		private void OnEventLostCube(Cube cube)
		{
			if(_vibration)
			{
				Handheld.Vibrate();
			}
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
			progressBarImage.fillAmount = 1f;
			progressBar.SetActive(false);
			buttonPressTap.SetActive(true);
		}
		public void TriggerVibration()
		{
			_vibration = !_vibration;
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
		public void PressButtonQuit()
		{
			Application.Quit();
		}
		private void OnDestroy()
		{
			EventManager.EventGameOver -= OnEventGameOver;
			EventManager.EventLostCube -= OnEventLostCube;
		}

	}
}
