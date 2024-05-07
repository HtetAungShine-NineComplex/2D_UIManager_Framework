using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace TinAungKhant.UIManagement
{
	public class PressTriggerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public bool OnPressing = false;
		private bool ContinousPress = false;
		[SerializeField] private float RepeatDelay = 0.3f;
		[SerializeField] private float Threshold = 0.1f;
		[SerializeField] private float CurrentTimer = 0.0f;

		public UnityEvent Event;

		public void OnPointerDown(PointerEventData pointerEventData)
		{
			OnPressing = true;
			ContinousPress = false;
			CurrentTimer = 0f;
			Event.Invoke();
		}

		public void OnPointerUp(PointerEventData pointerEventData)
		{
			OnPressing = false;
			ContinousPress = false;
			CurrentTimer = 0;
		}

		void Update()
		{
			if (OnPressing && ContinousPress)
			{
				CurrentTimer += Time.deltaTime;
				if (CurrentTimer >= Threshold)
				{
					CurrentTimer = 0;
					Event.Invoke();
				}
			}
			else if (OnPressing && !ContinousPress)
			{
				CurrentTimer += Time.deltaTime;
				if (CurrentTimer >= RepeatDelay)
				{
					CurrentTimer = 0;
					ContinousPress = true;
					Event.Invoke();
				}
			}
		}
	}
}
