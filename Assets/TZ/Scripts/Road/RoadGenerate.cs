using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace CubeSurfer
{
	public class RoadGenerate : MonoBehaviour
	{
		private GameObject cubeParent;
		private GameObject obstacleParent;

		[Header("Prefabs")]
		[SerializeField] private Road prefabRoad;
		[SerializeField] private Cube prefabCube;
		[SerializeField] private ObstacleData dataObstacle;
		[SerializeField] private List<ObstacleCube> obstacleCollection;

		[SerializeField] private List<Road> roadCollection;
		[SerializeField] private List<Cube> cubeCollection;

		[Header("Position player and circle")]
		[SerializeField] private Transform playerTransform;
		[SerializeField] private Transform circleTransform;

		[SerializeField] private int _spawnRoadPosition = 0;
		[SerializeField] private int _startCountRoad = 3;

		private int _currentRoad = 0;
		[SerializeField] private int _indexCubeSpawn = 0;
	   	[SerializeField] private float _spawnCubePosition = 0;

		[Header("Settings Spawn")]
		[SerializeField] private int _rangeObstacle;
		[SerializeField] private int _distanceCubeMin;
		[SerializeField] private int _distanceCubeMax;
		[SerializeField] private int _maxCubeScene;


		//Сколько кубов на сцене и у игрока
		[SerializeField] private int _playerCube = 0;
		[SerializeField] private int _sceneCube = 0;

		private int _roadLenght = 100;

		private void Start()
		{
			if (cubeParent == null && obstacleParent == null)
			{
				cubeParent = new GameObject("Cube");
				cubeParent.transform.parent = transform;

				obstacleParent = new GameObject("Obstacle");
				obstacleParent.transform.parent = transform;
			}

			EventManager.EventTakeCube += OnEventTakeCube;
			EventManager.EventLostCube += OnEventLostCube;

			LoadObstacle();
			LoadScene();
		}

		private void OnEventLostCube(Cube cube)
		{
			_playerCube--;
		}

		private void OnEventTakeCube(Cube cube)
		{
			_playerCube++;
		}

		private void LoadObstacle()
		{
			dataObstacle.LoadObstacle();

			foreach (ObstacleCube item in dataObstacle.ObstacleCubes)
			{
				GameObject obstacle = Instantiate(item.gameObject, obstacleParent.transform);
				obstacle.transform.position = Vector3.zero;
				obstacle.SetActive(false);
				obstacleCollection.Add(obstacle.GetComponent<ObstacleCube>());
			}
		}
		private void LoadScene()
		{
			
			for (int i = 0; i < _startCountRoad; i++)
			{
				Road road = Instantiate(prefabRoad, transform.forward * _spawnRoadPosition, transform.rotation, transform);
				road.SpawnPosition = _spawnRoadPosition;
				roadCollection.Add(road);
				_spawnRoadPosition += _roadLenght;
			}
			for (int i = 0; i < _maxCubeScene; i++)
			{
				GameObject cube = Instantiate(prefabCube.gameObject, cubeParent.transform);
				cube.SetActive(false);
				cubeCollection.Add(cube.GetComponent<Cube>());
			}
			for (int i = 0; i < circleTransform.position.z; i++)
			{
				if (i == _spawnCubePosition)
				{
					_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

					if(_sceneCube == _rangeObstacle)
					{
						foreach (ObstacleCube obstacle in obstacleCollection)
						{
							if(obstacle.Step == 1)
							{
								obstacle.isActive = true;
								obstacle.Spawn(_spawnCubePosition, true);
								break;
							}
						}
						_sceneCube = 0;
					}
					else
					{
						cubeCollection[_indexCubeSpawn].Spawn(_spawnCubePosition, true);
					}

					_indexCubeSpawn++;
					_sceneCube++;
				}
			}
			_sceneCube = 0;
			_spawnCubePosition = circleTransform.position.z;		
		}

		private void Update()
		{
			if (playerTransform.position.z   > _spawnRoadPosition - _roadLenght * (_startCountRoad - 1)) SpawnRoad();

			if (circleTransform.position.z > _spawnCubePosition) Spawn();
		}
		private void SpawnRoad()
		{
			if (_currentRoad == _startCountRoad)
				_currentRoad = 0;

			roadCollection[_currentRoad].SpawnPosition = _spawnRoadPosition;

			foreach(Cube cube in cubeCollection)
			{
				if(playerTransform.transform.position.z > cube.transform.position.z)
				{
					if(!cube.IsCollection)
					{
						cube.transform.SetParent(cubeParent.transform);
						cube.transform.rotation = Quaternion.Euler(0f,Random.Range(0,360),0f);
						cube.SetActive(false);
					}
				}
			}
			foreach (ObstacleCube obstacle in obstacleCollection)
			{
				if (playerTransform.transform.position.z > obstacle.transform.position.z)
				{ 
					obstacle.SetActive(false);
				}
			}

			roadCollection[_currentRoad].Rebuild();
			_spawnRoadPosition += _roadLenght;
			_currentRoad++;
		}
		private void Spawn()
		{
			if(_sceneCube == _rangeObstacle)
			{
				_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);
				if (_playerCube > 3)
				{
					foreach (ObstacleCube obstacle in obstacleCollection)
					{
						if(obstacle.Step >= 3 && !obstacle.isActive)
						{
							obstacle.Spawn(_spawnCubePosition, true);
						
							break;
						}
						if (obstacle.transform.position.z < playerTransform.transform.position.z)
						{
							obstacle.SetActive(false);
						}
					}
				}
				else
				{
					foreach (ObstacleCube item in obstacleCollection)
					{
						if (item.Step >= 1 && !item.isActive)
						{
					
							item.Spawn(_spawnCubePosition, true);
							break;
						}
					}
				}

				_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);
				_sceneCube = 0;
				return;
			}

			try
			{

				if (_indexCubeSpawn == cubeCollection.Count - 1) _indexCubeSpawn = 0;

				_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

				while (cubeCollection[_indexCubeSpawn].IsCollection)
				{
					if (_indexCubeSpawn == cubeCollection.Count - 1)
						_indexCubeSpawn = 0;
					_indexCubeSpawn++;
				}

				cubeCollection[_indexCubeSpawn].Spawn(_spawnCubePosition, true);


				_indexCubeSpawn++;
				_sceneCube++;
			}
			catch (System.Exception ex)
			{

				Debug.Log(ex.Message);
			}
		}
		private void OnDestroy()
		{
			EventManager.EventTakeCube -= OnEventTakeCube;
			EventManager.EventLostCube -= OnEventLostCube;
		}
	}
}
