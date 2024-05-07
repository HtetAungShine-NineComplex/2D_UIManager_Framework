using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinAungKhant.UIManagement
{
	[RequireComponent(typeof(GridLayoutGroup))]
	public class VerticleGridLayoutSnapScroll : MonoBehaviour
	{
		public int MaxRow = 3;
		private Navigation cullNav=new Navigation();
		private Navigation normalNav=new Navigation();

		private RectTransform rectTransform;
		List<Button> selectableChilds = new List<Button>();
		private GridLayoutGroup layoutgroup;
		private float DefaultOffset;
		private float InsSpace;

		private int Page = 0;

		public void Awake()
		{
			FreshSelectableChilds();
			rectTransform = GetComponent<RectTransform>();
			layoutgroup = GetComponent<GridLayoutGroup>();
			
			DefaultOffset = layoutgroup.cellSize.y / 2;
			InsSpace = layoutgroup.cellSize.y + layoutgroup.spacing.y;

			cullNav.mode = Navigation.Mode.None;
			normalNav.mode = Navigation.Mode.Automatic;
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
			if (selectableChilds.Contains(UIManager.Instance.CacheSelectable.GetComponent<Button>()))
			{
				float Y = UIManager.Instance.CacheSelectable.GetComponent<RectTransform>().anchoredPosition.y;
				Y += DefaultOffset;
				Y = Mathf.Abs(Y);
				Page = Mathf.FloorToInt(Y / (InsSpace * MaxRow));
				ChangeLayout();
			}
			else
			{
				ChangeLayout();
			}
		}

		public void PageUp()
		{
			if (Page == 0)
			{
				return;
			}
			Page -= 1;
			if (Page <= 0)
			{
				Page = 0;
			}
			ChangeLayout(-1);
		}
		public void PageDown()
		{
			int maxPage =Mathf.FloorToInt(selectableChilds.Count / (MaxRow * layoutgroup.constraintCount));
			if (Page >= maxPage - 1)
			{
				return;
			}
			Page += 1;
			if (Page >= maxPage-1)
			{
				Page = maxPage-1;
			}
			ChangeLayout(1);
		}
		public void ChangeLayout(int skipfactor=0)
		{		
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,Page * InsSpace * MaxRow);

			if (UIManager.Instance.CacheSelectable == null)
			{
				DisableCullObjects();
				return;
			}
			else if (selectableChilds.Contains(UIManager.Instance.CacheSelectable.GetComponent<Button>()))
			{
				EnableAllObjects();
				int index=selectableChilds.IndexOf(UIManager.Instance.CacheSelectable.GetComponent<Button>());
				index += MaxRow * layoutgroup.constraintCount * skipfactor;
				index = Mathf.Clamp(index,0,selectableChilds.Count-1);
				selectableChilds[index].Select();
			}
			else
			{
				DisableCullObjects();
			}
		}
		public void DisableCullObjects()
		{
			int startIndex = Page * MaxRow * layoutgroup.constraintCount;
			int endIndex = (Page + 1) * MaxRow * layoutgroup.constraintCount;
			for (int i = 0; i < selectableChilds.Count; i++)
			{
				if (i < startIndex)
				{
					selectableChilds[i].navigation=cullNav;
				}
				else if (i >= endIndex)
				{
					selectableChilds[i].navigation=cullNav;
				}
				else
				{
					selectableChilds[i].navigation=normalNav;
				}
			}
		}
		public void EnableAllObjects()
		{
			for (int i = 0; i < selectableChilds.Count; i++)
			{
				selectableChilds[i].navigation = normalNav;
			}
		}
	}
}

