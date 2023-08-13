using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeSurfer
{
	public class AudioController : MonoBehaviour
	{

		[Header("Audio")]
		[SerializeField] private AudioSource audioSourceMusic;
		[SerializeField] private AudioSource audioSourceFX;
		[SerializeField] private float _musicVolume;
		[SerializeField] private float _effectVolume;

		[SerializeField] private AudioClip takeClip;
		[SerializeField] private AudioClip lostClip;

		private void Start()
		{
			EventManager.EventTakeCube += OnEventTakeCube;
			EventManager.EventLostCube += OnEventLostCube;
			EventManager.EventGameOver += OnEventGameOver;
		}

		private void OnEventTakeCube(Cube cube)
		{
			audioSourceFX.PlayOneShot(takeClip);
		}
		private void OnEventGameOver()
		{
			audioSourceMusic.Stop();
			audioSourceFX.PlayOneShot(lostClip);
		}

		private void OnEventLostCube(Cube cube)
		{
			audioSourceFX.PlayOneShot(lostClip);
		}

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
		private void OnDestroy()
		{
			EventManager.EventTakeCube -= OnEventLostCube;
			EventManager.EventLostCube -= OnEventLostCube;
			EventManager.EventGameOver -= OnEventGameOver;
		}
	}
}
