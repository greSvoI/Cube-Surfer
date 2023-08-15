using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeSurfer
{
	public interface ICube 
	{
		public void Spawn(float position, bool active);
		public void SetActive(bool active);
	}
}
