using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeSurfer
{
	public class AudioController : MonoBehaviour
	{
		private AudioSource audioSource;
		
		[Header("Audio")]
		[SerializeField] private AudioClip _runnerAudio;

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.PlayOneShot(_runnerAudio);
			//audioSource.Play();
		}
		public void Volume(float volume)
		{
			audioSource.volume = volume;	
		}

	}
}
