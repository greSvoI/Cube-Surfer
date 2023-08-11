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
		public static Action<Cube> EventTakeCube { get; set; }
		public static Action<Cube> EventLostCube {  get;  set; }
		public static Action<Vector2> EventInput { get;set; }
		public static Action EventGameOver { get; set; }	

	}
}
