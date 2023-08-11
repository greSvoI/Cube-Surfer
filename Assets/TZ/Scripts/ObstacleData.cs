using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CubeSurfer
{
	[CreateAssetMenu(fileName = "Obstacle data", menuName = "CubeSurfer/Obstacle data", order = 1)]
	public class ObstacleData : ScriptableObject
	{
	    private ObstacleCube[] obstacle;
		private ObstacleCube[] obstacle_1;
		private ObstacleCube[] obstacle_2;
		private ObstacleCube[] obstacle_3;

		public List<ObstacleCube> ObstacleCubes;
		//Not Work
		public void LoadObstacle()
		{
			obstacle = Resources.LoadAll<ObstacleCube>("Obstacle");
			obstacle_1 = Resources.LoadAll<ObstacleCube>("1_Rand");
			obstacle_2 = Resources.LoadAll<ObstacleCube>("2_Rand");
			obstacle_3 = Resources.LoadAll<ObstacleCube>("3_Rand");

			foreach (var obstacle in obstacle) ObstacleCubes.Add(obstacle);
			foreach (var obstacle in obstacle_1) ObstacleCubes.Add(obstacle);
			foreach (var obstacle in obstacle_2) ObstacleCubes.Add(obstacle);
			foreach (var obstacle in obstacle_3) ObstacleCubes.Add(obstacle);
		}
		private void OnEnable()
		{
			ObstacleCubes.Clear();
		}
	}
}
