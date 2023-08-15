using CubeSurfer;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace CubeSurfer
{
	public class TouchController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		private float _screenWidth;
	    public	Vector2 clampedPosition;

		private void Start()
		{
			_screenWidth = Screen.width;
		}
		public void OnDrag(PointerEventData eventData)
		{
			clampedPosition = ClampValuesToMagnitude(eventData.delta);
			OutputEventPosition(clampedPosition);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			clampedPosition.x =	_screenWidth / 2 < eventData.position.x ? 1 : -1;
			OutputEventPosition(clampedPosition);
		}
		public void OnPointerUp(PointerEventData eventData)
		{
			clampedPosition = Vector2.zero;
			OutputEventPosition(clampedPosition);
		}
		private void OutputEventPosition(Vector2 newPosition)
		{
			EventManager.EventInput?.Invoke(newPosition);
		}
		private Vector2 ClampValuesToMagnitude(Vector2 position)
		{
			return Vector2.ClampMagnitude(position, 1);
		}
	}
}
