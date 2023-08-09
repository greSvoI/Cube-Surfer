using System;
using UnityEngine;
using UnityEngine.Events;

namespace CubeSurfer
{
	public class EventManager : MonoBehaviour
	{
		public static EventManager instance;
		private void Awake()
		{
			if(instance == null)
				instance = this;
		}
		public static Action<GameObject> EventAddCube { get; set; }
		public static Action<GameObject> EventDestroyCube {  get;  set; }

	}
}
