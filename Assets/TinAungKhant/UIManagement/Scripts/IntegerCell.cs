using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinAungKhant.UIManagement
{
	public class IntegerCell : MonoBehaviour
	{
		public int increment;
		private Text m_Text;

		public void Awake()
		{
			m_Text = GetComponentInChildren<Text>();
		}

		public void UpdateText(int value)
		{
			m_Text.text = value.ToString();
		}
	}
}

