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
		[SerializeField] private ObstacleData obstacleData;
		[SerializeField] private List<ObstacleCube> listObstacle;

		[SerializeField] private List<Road> listRoad;
		[SerializeField] private List<Cube> listCube;

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


		//������� ����� �� ����� � � ������
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

			EventManager.EventTakeCube += OnEventTakeCube => _playerCube++;
			EventManager.EventLostCube += OnEventLostCube => _playerCube--;

			LoadObstacle();
			LoadScene();
		}
		private void LoadObstacle()
		{
			obstacleData.LoadObstacle();

			foreach (ObstacleCube item in obstacleData.ObstacleCubes)
			{
				GameObject obstacle = Instantiate(item.gameObject, obstacleParent.transform);
				obstacle.transform.position = Vector3.zero;
				obstacle.SetActive(false);
				listObstacle.Add(obstacle.GetComponent<ObstacleCube>());
			}
		}
		private void LoadScene()
		{
			
			for (int i = 0; i < _startCountRoad; i++)
			{
				Road road = Instantiate(prefabRoad, transform.forward * _spawnRoadPosition, transform.rotation, transform);
				road.SpawnPosition = _spawnRoadPosition;
				listRoad.Add(road);
				_spawnRoadPosition += _roadLenght;
			}
			for (int i = 0; i < _maxCubeScene; i++)
			{
				GameObject cube = Instantiate(prefabCube.gameObject, cubeParent.transform);
				cube.SetActive(false);
				listCube.Add(cube.GetComponent<Cube>());
			}
			for (int i = 0; i < circleTransform.position.z; i++)
			{
				if (i == _spawnCubePosition)
				{
					_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

					if(_sceneCube == _rangeObstacle)
					{
						foreach (ObstacleCube obstacle in listObstacle)
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
						listCube[_indexCubeSpawn].Spawn(_spawnCubePosition, true);
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
			//if (circleTransform.position.z / 10 == 0)
			//{
			//	SpawnObstacle(listObstacle_1);
			//	return;
			//}

			if (playerTransform.position.z   > _spawnRoadPosition - _roadLenght * (_startCountRoad - 1)) SpawnRoad();

			if (circleTransform.position.z > _spawnCubePosition) Spawn();
		}
		private void SpawnRoad()
		{
			if (_currentRoad == _startCountRoad)
				_currentRoad = 0;

			listRoad[_currentRoad].SpawnPosition = _spawnRoadPosition;

			foreach(Cube cube in cubeParent.GetComponentsInChildren<Cube>())
			{
				if(playerTransform.transform.position.z > cube.transform.position.z)
				{
					cube.SetActive(false);
				}
			}
			foreach (ObstacleCube obstacle in listObstacle)
			{
				if (playerTransform.transform.position.z > obstacle.transform.position.z)
				{
					obstacle.SetActive(false);
				}
			}

			listRoad[_currentRoad].Rebuild();
			_spawnRoadPosition += _roadLenght;
			_currentRoad++;
		}
		private void Spawn()
		{
			if(_sceneCube == _rangeObstacle)
			{
				_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);
				if (_playerCube > 2)
				{
					foreach (ObstacleCube item in listObstacle)
					{
						if(item.Step > 2 && !item.isActive)
						{
							item.Spawn(_spawnCubePosition, true);
							break;
						}
					}
				}
				else
				{
					foreach (ObstacleCube item in listObstacle)
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

			if(_indexCubeSpawn == _maxCubeScene) _indexCubeSpawn = 0;

			_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

			if (listCube[_indexCubeSpawn].IsCollection) _indexCubeSpawn++;

			listCube[_indexCubeSpawn].Spawn(_spawnCubePosition,true);

			_indexCubeSpawn++;
			_sceneCube++;
		}
	}
}
