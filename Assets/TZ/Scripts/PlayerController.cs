using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubeSurfer
{
	public class PlayerController : MonoBehaviour
	{
		private List<Cube> cubeCollection = new List<Cube>();
		private List<Rigidbody> mouseRagdoll = new List<Rigidbody>();


		private bool _isLive = true;

		[Header("UI")]
		[SerializeField] private GameObject takeCubeUI;
		[SerializeField] private TextMeshProUGUI scoreUI;
		[SerializeField] private TextMeshProUGUI highScoreUI;
		[SerializeField] private float _timeUI;


		[Header("Speed")]
	    [SerializeField]	private float _rangeTimeSpeed= 10;
		[SerializeField]	private float _rangeTime = 10f;
	    [SerializeField]	private float _horizontalSpeed = 5;
     	[SerializeField]	private float _speed = 15;
		private float _time = 0;

		[SerializeField] private float _positionX;
		[Header("Component")]
		[SerializeField] private GameObject magicCircle;
		[SerializeField] private GameObject mouse;
		[SerializeField] private ParticleSystem takeEffect;
		private ParticleSystem[] takeEffects;

		[Header("Seting cube")]
		[SerializeField] private float _rotationSpeed = 5f;
		[SerializeField] private float _horizontalLimit = 2f;
		[SerializeField] private float _heightCube = 1f;
		[SerializeField] private float _lineTower;

		[Header("Force mouse")]
		[SerializeField] private float _force = 10f;

		[Header("Audio")]
		[SerializeField] private AudioSource audioSourceMusic;
		[SerializeField] private AudioSource audioSourceFX;

		[SerializeField] private AudioClip takeClip;
		[SerializeField] private AudioClip lostClip;

		private float _musicVolume = 1;
		private float _effectVolume = 1;

		private int _score = 0;
		private int _highScore = 0;
		private void Awake()
		{
			foreach(Rigidbody rb in mouse.GetComponentsInChildren<Rigidbody>())
			{
				rb.isKinematic = true;
				mouseRagdoll.Add(rb);
			}

			takeEffects = takeEffect.GetComponentsInChildren<ParticleSystem>();
		}
		private void Start()
		{
			if (PlayerPrefs.GetInt("_highScore") <= _highScore)
				PlayerPrefs.SetInt("_highScore", _highScore);

			_highScore = PlayerPrefs.GetInt("_highScore");

			highScoreUI.text = $"Highscore : " + _highScore.ToString();

			EventManager.EventTakeCube += OnEventTakeCube;
			EventManager.EventLostCube += OnEventLostCube;
			EventManager.EventInput += OnEventInput;

			audioSourceMusic.volume = _musicVolume;
			audioSourceFX.volume = _effectVolume;
		}

		private void Update()
		{

			_time += Time.deltaTime;
			if (_rangeTimeSpeed < _time)
			{
				_speed++;
				_rangeTimeSpeed += _rangeTime;
			}

			if (_isLive)
			{
				MoveTransform();
			}
		}

		private void MoveTransform()
		{
			foreach (var cube in cubeCollection)
			{
				cube.transform.position = Vector3.Lerp(cube.transform.position, new Vector3(transform.position.x, cube.transform.position.y, transform.position.z), _rotationSpeed * Time.deltaTime);
				cube.transform.rotation = Quaternion.Lerp(cube.transform.rotation, Quaternion.identity, _rotationSpeed * Time.deltaTime);
			}
			float positionX = transform.position.x + _positionX * _horizontalSpeed * Time.deltaTime;
			positionX = Mathf.Clamp(positionX, -_horizontalLimit, _horizontalLimit);
			transform.position = new Vector3(positionX, transform.position.y, transform.position.z);

			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			magicCircle.transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y < cubeCollection.Count - 1)
			{
				GameOver();
			}
		}

		private void OnEventInput(Vector2 vector)
		{
			_positionX = vector.x;
		}

		private void OnEventLostCube(Cube cube)
		{
			cubeCollection.Remove(cube);
			takeEffect.transform.position = cube.transform.position;
			takeEffect.Emit(1);
			foreach (ParticleSystem partical in takeEffects)
			{
				partical.Emit(3);
			}
			audioSourceFX.PlayOneShot(lostClip);
		}

		private void OnEventTakeCube(Cube cube)
		{
			
			//Take cube and start time ui
			takeCubeUI.SetActive(true);
			StartCoroutine(ShowUI(_timeUI));

			_score++;
			scoreUI.text = $"Score : " + _score.ToString();
			
			transform.position = new Vector3(transform.position.x,transform.position.y + _heightCube,transform.position.z);
			cube.transform.position = new Vector3(transform.position.x,_heightCube / 2, transform.position.z);
			cube.transform.SetParent(transform);

			takeEffect.transform.position = cube.transform.position;
			takeEffect.Emit(1);
			foreach (ParticleSystem partical in takeEffects)
			{
				partical.Emit(1);
			}
			cubeCollection.Add(cube);
			audioSourceFX.PlayOneShot(takeClip);

		}
		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Obstacle")
			{
				audioSourceFX.PlayOneShot(takeClip);
				GameOver();
			}
		}
		private IEnumerator ShowUI(float timeUI)
		{
			yield return new WaitForSeconds(timeUI);
			takeCubeUI.SetActive(false);
		}
		private void GameOver()
		{
			if (_score > _highScore)
				PlayerPrefs.SetInt("_highScore",_score);

			EventManager.EventGameOver?.Invoke();
			_isLive = false;
			mouse.transform.parent = null;
			
			mouse.GetComponent<Animator>().enabled = false;

			foreach (Rigidbody rb in mouseRagdoll)
			{
				rb.isKinematic = false;
			}
			mouse.transform.position += Vector3.up;
			mouse.GetComponent<Rigidbody>().AddForce(Vector3.forward * _force, ForceMode.Impulse);
		}
		private void OnDestroy()
		{
			EventManager.EventTakeCube -= OnEventTakeCube;
			EventManager.EventLostCube -= OnEventLostCube;
			EventManager.EventInput -= OnEventInput;
		}
		//Need volume slider
		public void MusicVolume(float volume)
		{
			_musicVolume = volume;
			audioSourceMusic.volume = _musicVolume;
		}
		public void EffectVolume(float volume)
		{
			_effectVolume = volume;
			audioSourceFX.volume = _effectVolume;
		}
	}
}
