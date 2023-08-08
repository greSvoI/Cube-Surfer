using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeSurfer
{
	public class RoadGenerate : MonoBehaviour
	{

		[SerializeField] private Road prefabRoad;
		[SerializeField] private List<Road> listRoad;
		[SerializeField] private Transform player;

		[SerializeField] private float _startPosPlayer = 25f;
		[SerializeField] private int _spawnPosition = 0;
		[SerializeField] private int _startCountRoad = 5;
		[SerializeField] private int _currentRoad = 0;

		private int _roadLenght = 50;


		private void Start()
		{
			for(int i=0; i < _startCountRoad; i++)
			{
				Road road = Instantiate(prefabRoad, transform.forward * _spawnPosition, transform.rotation, transform);
				road.SpawnPosition = _spawnPosition;
				road.Build();
				listRoad.Add(road);
				_spawnPosition += _roadLenght;
			}
		}

		private void Update()
		{
			if(player.position.z   > _spawnPosition - _roadLenght * (_startCountRoad - 1))
			{
				SpawnRoad();
			}
		}
		private void SpawnRoad()
		{
			if (_currentRoad == _startCountRoad)
				_currentRoad = 0;

			listRoad[_currentRoad].SpawnPosition = _spawnPosition;
			listRoad[_currentRoad].Rebuild();
			_spawnPosition += _roadLenght;
			_currentRoad++;
		}
	}
}
