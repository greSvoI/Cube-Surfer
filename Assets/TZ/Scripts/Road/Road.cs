using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace CubeSurfer
{
	public class Road : MonoBehaviour
	{
		private int _spawnPosition;
		public int SpawnPosition { get => _spawnPosition; set { _spawnPosition = value; } }

		public void Rebuild()
		{
			transform.position = transform.forward * _spawnPosition;
		}
	}
}
