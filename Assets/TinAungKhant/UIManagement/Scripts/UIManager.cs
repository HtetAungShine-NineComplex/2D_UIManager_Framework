using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TinAungKhant.UIManagement
{
	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance;

		public event System.Action EventSelectableChanged;

		[SerializeField] private Transform UIRoot;
		public EventSystem m_EventSystem;
		public StandaloneInputModule m_InputModule;
		public GameObject CacheSelectable;

		private static Dictionary<string,UIBase> m_UIMap = new Dictionary<string,UIBase>();
		private static List<UIBase> m_OpeningUI = new List<UIBase>();
		private static UIBase m_CatchUI;

		public void Awake()
		{
			if (Instance != null)
			{
				Destroy(this.gameObject);
				return;
			}
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		private void SortUI()
		{
			if (m_OpeningUI == null)
				return;

			int Count = -1;
			for (int i = m_OpeningUI.Count - 1; i >= 0; i--)
			{
				m_OpeningUI[i].SortOrder = Count;
				Count--;
			}
			if (m_OpeningUI.Count > 0)
			{
				m_OpeningUI[m_OpeningUI.Count - 1].Focus();
			}		
		}

		public void ShowUI(string UIName,UIBaseData Data = null)
		{
			if (!m_UIMap.ContainsKey(UIName))
			{
				UIBase UIAsset = Resources.Load<UIBase>(string.Format(GLOBALCONST.FORMAT_UIPREFAB_PATH,UIName));
				if (UIAsset == null)
				{
					Debug.LogError(string.Format("load UI failed :{0}",UIName));
					return;
				}
				UIBase UI = Object.Instantiate(UIAsset) as UIBase;
				UI.gameObject.name = UIName;
				if (UI.IsCamUI)
				{
					Object.Destroy(UI.gameObject);
					return;
				}
				else
				{
					UI.transform.SetParent(UIRoot);
				}
				UI.Init();
				UI.gameObject.SetActive(false);
				m_UIMap.Add(UIName,UI);
			}

			if (m_UIMap[UIName] == null)
			{
				Debug.Log(string.Format("UI not in UIMap :{0}",UIName));
				return;
			}

			if (m_OpeningUI.Count > 0)
				m_OpeningUI[m_OpeningUI.Count - 1].Defocus();

			if (m_OpeningUI.Contains(m_UIMap[UIName]))
				m_OpeningUI.Remove(m_UIMap[UIName]);

			m_OpeningUI.Add(m_UIMap[UIName]);
			m_UIMap[UIName].Show(Data);

			SortUI();
		}

		public void CloseUI(string UIName)
		{
			if (!m_UIMap.ContainsKey(UIName))
				return;

			if (m_UIMap[UIName] == null)
				return;

			if (m_OpeningUI.Contains(m_UIMap[UIName]))
				m_OpeningUI.Remove(m_UIMap[UIName]);

			m_UIMap[UIName].Close();

			SortUI();
		}

		public void CloseAllOpeningUIs()
		{
			for (int i = 0; i < m_OpeningUI.Count; i++)
			{
				m_OpeningUI[i].Close();
			}
			m_OpeningUI.Clear();
		}

		public bool IsOpened(string UIName)
		{
			if (!m_UIMap.ContainsKey(UIName))
				return false;
			return m_UIMap[UIName].Opened;
		}

		public static T UIInstance<T>() where T : UIBase
		{
			if (m_CatchUI != null && m_CatchUI is T)
				return m_CatchUI as T;

			for (int i = 0; i < m_OpeningUI.Count; i++)
			{
				if (m_OpeningUI[i] is T)
				{
					m_CatchUI = m_OpeningUI[i];
					return m_OpeningUI[i] as T;
				}
			}
			return null;
		}

		public UIBase GetUIBase(string uiName)
		{
			if (!m_UIMap.ContainsKey(uiName))
				return null;
			return m_UIMap[uiName];
		}

		public bool IsUIActive(string uiName)
		{
			UIBase ui = GetUIBase(uiName);
			if (ui != null)
			{
				if (ui.isActiveAndEnabled)
				{
					return true;
				}
			}
			return false;
		}

		public void Update()
		{
			/*if (m_EventSystem != null)
			{
				if (CacheSelectable != m_EventSystem.currentSelectedGameObject)
				{
					if (m_EventSystem.currentSelectedGameObject == null)
					{
						m_EventSystem.SetSelectedGameObject(CacheSelectable);
						return;
					}
					CacheSelectable = m_EventSystem.currentSelectedGameObject;
					EventSelectableChanged?.Invoke();
				}
			}*/
		}
	}
}
