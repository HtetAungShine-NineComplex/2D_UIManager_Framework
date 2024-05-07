using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinAungKhant.UIManagement
{
	[RequireComponent(typeof(VerticalLayoutGroup))]
	public class VerticleSnapScroll : MonoBehaviour
	{
		public float MaxView=4;
		public float CellSize;

		private RectTransform rectTransform;
		List<Button> selectableChilds = new List<Button>();
		private VerticalLayoutGroup layoutgroup;
		private float InsSpace;

		public void Awake()
		{
			FreshSelectableChilds();
			rectTransform = GetComponent<RectTransform>();
			layoutgroup = GetComponent<VerticalLayoutGroup>();
			InsSpace = CellSize+layoutgroup.spacing;
		}

		public void Start()
		{
			UIManager.Instance.EventSelectableChanged += OnSelectableChange;
		}

		public void FreshSelectableChilds()
		{
			selectableChilds = new List<Button>();
			Button[] selectables = transform.GetComponentsInChildren<Button>();
			if (selectables.Length > 0)
			{
				for (int i = 0; i < selectables.Length; i++)
				{
					selectableChilds.Add(selectables[i]);
				}
			}
		}

		private void OnSelectableChange()
		{
			if (!this.gameObject.activeInHierarchy)
			{
				return;
			}
			if (UIManager.Instance.CacheSelectable == null)
			{
				return;
			}
			int index = selectableChilds.IndexOf(UIManager.Instance.CacheSelectable.GetComponent<Button>());
			if (index != -1)
			{
				float targetY = InsSpace * index;
				float currentY = rectTransform.anchoredPosition.y;
				float difference = targetY - currentY;
				if (difference > InsSpace * (MaxView - 1))
				{
					float distance = Mathf.Abs(difference) - InsSpace * (MaxView - 1);
					rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,currentY +distance);
				}
				else if (difference <0)
				{
					float distance = Mathf.Abs(difference);
					rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,currentY - distance);
				}
			}
		}

		public void Up()
		{
			if (UIManager.Instance.CacheSelectable == null)
			{
				return;
			}
			int index = selectableChilds.IndexOf(UIManager.Instance.CacheSelectable.GetComponent<Button>());

			if (index != -1)
			{
				index -= 1;
				index = Mathf.Clamp(index,0,selectableChilds.Count-1);
				selectableChilds[index].Select();
			}
		}
		public void Down()
		{
			if (UIManager.Instance.CacheSelectable == null)
			{
				return;
			}
			int index = selectableChilds.IndexOf(UIManager.Instance.CacheSelectable.GetComponent<Button>());

			if (index != -1)
			{
				index += 1;
				index = Mathf.Clamp(index,0,selectableChilds.Count-1);
				selectableChilds[index].Select();
			}
		}
	}
}
