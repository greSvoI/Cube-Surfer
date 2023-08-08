using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace CubeSurfer
{
	public class Road : MonoBehaviour
	{
		private GameObject cubeParent;
		private GameObject obstacleParent;

		[SerializeField] private Cube cube;
		[SerializeField] private ObstacleCube obstacle;

		[SerializeField] private int _spawnPosition;

		[SerializeField] private int _rangeCubeMin;
		[SerializeField] private int _rangeCubeMax;
		[SerializeField] private int _heightObstacleMax;

		[SerializeField] private int _roadlenght;

		public int SpawnPosition { get => _spawnPosition; set { _spawnPosition = value; } }

		public void Build()
		{
			cubeParent = new GameObject("Cube");
			cubeParent.transform.parent = transform;

			obstacleParent = new GameObject("Obstacle");
			obstacleParent.transform.parent = transform;

			int lenght = _spawnPosition + _roadlenght;
			int i = _spawnPosition;
			int spawCube = i  + Random.Range(_rangeCubeMin,_rangeCubeMax);
			for (; i < lenght; i++)
			{
				if(i == spawCube)
				{
					Instantiate(cube, new Vector3(Random.Range(-2, 2), (cube.GetComponent<Renderer>().bounds.size.y + 0.1f), i), Quaternion.identity, cubeParent.transform);
					spawCube += Random.Range(_rangeCubeMin, _rangeCubeMax);
				}
			}
		}
		public void Rebuild() 
		{
			transform.position = transform.forward * SpawnPosition;




		}
	}
}
