using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CubeSurfer
{
	public class RoadGenerate : MonoBehaviour
	{
		private GameObject cubeParent;
		private GameObject obstacleParent;

		[Header("Prefabs")]
		[SerializeField] private Road prefabRoad;
		[SerializeField] private Cube prefabCube;
		
		[SerializeField] private GameObject[] obstacle;
		[SerializeField] private ObstacleCube[] obstacle_1;
		[SerializeField] private ObstacleCube[] obstacle_2;
		[SerializeField] private ObstacleCube[] obstacle_3;

		[SerializeField] private List<ObstacleCube> listObstacle;
		[SerializeField] private List<ObstacleCube> listObstacle_1;
		[SerializeField] private List<ObstacleCube> listObstacle_2;
		[SerializeField] private List<ObstacleCube> listObstacle_3;

		[SerializeField] private List<Road> listRoad  = new List<Road>();
		[SerializeField] private List<Cube> listCube = new List<Cube>();

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
			_spawnCubePosition = circleTransform.position.z;
			

			EventManager.EventTakeCube += OnEventTakeCube => _playerCube++;
			EventManager.EventLostCube += OnEventLostCube => _playerCube--;

			LoadCubeObstacel();

			_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

			for (int i = 0; i < circleTransform.position.z; i++)
			{
				if(i == _spawnCubePosition)
				{
					listCube[_indexCubeSpawn].Spawn(_spawnCubePosition, true);
					_indexCubeSpawn++;
				}
			}
			_spawnCubePosition = circleTransform.position.z;
		}
		private void LoadCubeObstacel()
		{
			//obstacle = Resources.LoadAll<GameObject>("Obstacle");
			//obstacle_1 = Resources.LoadAll<ObstacleCube>("1_Rand");
			//obstacle_2 = Resources.LoadAll<ObstacleCube>("2_Rand");
			//obstacle_3 = Resources.LoadAll<ObstacleCube>("3_Rand");

			foreach (GameObject item in obstacle)
			{
				GameObject obstacle = Instantiate(item, obstacleParent.transform);
				obstacle.SetActive(false);
				listObstacle.Add(item.GetComponent<ObstacleCube>());
			
			}
			foreach(ObstacleCube item in obstacle_1)
			{
				GameObject obstacle = Instantiate(item.gameObject, obstacleParent.transform);
				obstacle.SetActive(false);
				listObstacle_1.Add(item.GetComponent<ObstacleCube>());
			}
			//foreach (ObstacleCube item in obstacle_1)
			//{
			//	GameObject obstacle = Instantiate(item.gameObject, obstacleParent.transform);	
			//	item.SetActive(false);
			//	listObstacle_1.Add(item.GetComponent<ObstacleCube>());
			//}
			//foreach (ObstacleCube item in obstacle_2)
			//{
			//	GameObject obstacle = Instantiate(item.gameObject, obstacleParent.transform);
			//	item.SetActive(false);
			//	listObstacle_2.Add(item);
			//}
			//foreach (ObstacleCube item in obstacle_3)
			//{
			//	GameObject obstacle = Instantiate(item.gameObject, obstacleParent.transform);
			//	item.SetActive(false);
			//	listObstacle_3.Add(item);
			//}
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

			listRoad[_currentRoad].Rebuild();
			_spawnRoadPosition += _roadLenght;
			_currentRoad++;
		}
		private void Spawn()
		{
			if(_sceneCube == _rangeObstacle)
			{

				int index = Random.Range(0, listObstacle_1.Count);
				_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

				while (listObstacle_1[index].isActive)
				{
					index = Random.Range(0, listObstacle_1.Count);
				}
				listObstacle_1[index].Spawn(_spawnCubePosition, true);
				_sceneCube = 0;

				//if (_playerCube >= 2)
				//{
				//	switch (_playerCube)
				//	{
				//		case 2:
				//			SpawnObstacle(listObstacle_1);
				//			break;
				//		case 3:
				//			SpawnObstacle(listObstacle_1);
				//			break;
				//		case 4:
				//			SpawnObstacle(listObstacle_1);
				//			break;
				//	}
				//}
				//else
				//{
				//	SpawnObstacle(listObstacle);
				//}
				SpawnObstacle(listObstacle);
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

		private void SpawnObstacle(List<ObstacleCube>listObstacle)
		{
			int index = Random.Range(0, listObstacle.Count);
			_spawnCubePosition += Random.Range(_distanceCubeMin, _distanceCubeMax);

			while (listObstacle[index].isActive)
			{
				index = Random.Range(0, listObstacle.Count);
			}
			listObstacle[index].Spawn(_spawnCubePosition, true);
			_sceneCube = 0;
		}
	}
}
