using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TinAungKhant.UIManagement
{
	public class IntegerGrid : MonoBehaviour
	{
		public int Value;
		public int MaxValue=0;

		[SerializeField] private float DelayThreshold = 0.3f;
		[SerializeField] private float threshold=0.1f;
		private float timer=0f;
		private float LastInput=0;

		List<IntegerCell> selectableChilds = new List<IntegerCell>();

		public UnityEvent<int> OnValueChanged;

		public void Awake()
		{
			FreshSelectableChilds();
		}

		public void FreshSelectableChilds()
		{
			selectableChilds = new List<IntegerCell>();
			IntegerCell[] selectables = transform.GetComponentsInChildren<IntegerCell>();
			if (selectables.Length > 0)
			{
				for (int i = 0; i < selectables.Length; i++)
				{
					selectableChilds.Add(selectables[i]);
					int Pow = (selectables.Length - i) - 1;
					selectables[i].increment = Mathf.RoundToInt(Mathf.Pow(10,Pow));
				}
			}
			MaxValue = Mathf.RoundToInt(Mathf.Pow(10,selectableChilds.Count)-1);
		}

		public void Increase()
		{
			ChangeValue(1);
		}
		public void Decrease()
		{
			ChangeValue(-1);
		}
		public void ChangeValue(int factor)
		{
			if (UIManager.Instance.CacheSelectable == null)
			{
				return;
			}
			IntegerCell cell = UIManager.Instance.CacheSelectable.GetComponent<IntegerCell>();
			if (cell != null)
			{
				int index = selectableChilds.IndexOf(cell);
				if (index == -1)
				{
					return;
				}

				float lastValue = Value;
				Value += (cell.increment * factor);
				Value = Mathf.Clamp(Value,0,MaxValue);

				if (lastValue != Value)
				{
					OnValueChanged.Invoke(Value);
					UpdateGraphic();
				}	
			}
		}
		public void Update()
		{
			float Y=Input.GetAxisRaw(UIManager.Instance.m_InputModule.verticalAxis);
			if (Y > 0)
			{
				Y = 1;
			}
			else if (Y < 0)
			{
				Y = -1;
			}
			if (Y != 0)
			{
				if (timer <= 0)
				{
					if (LastInput == 0)
					{
						LastInput = Y;
						timer = DelayThreshold;
					}
					else if (LastInput == Y)
					{
						LastInput = Y;
						timer = threshold;
					}
					else
					{
						LastInput = Y;
						timer = DelayThreshold;
						return;
					}
					ChangeValue((int)Y);
				}
				else
				{
					timer -= Time.deltaTime;
				}
			}
			else
			{
				LastInput = 0;
			}
		}
		public void UpdateGraphic()
		{
			for (int i = 0; i < selectableChilds.Count; i++)
			{
				int value = Value / selectableChilds[i].increment;
				value = value % 10;
				selectableChilds[i].UpdateText(value);
			}
		}
	}
}

