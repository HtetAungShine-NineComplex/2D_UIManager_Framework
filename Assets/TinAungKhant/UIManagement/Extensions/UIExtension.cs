using UnityEngine;

namespace TinAungKhant.UIManagement
{
	public static class UIExtension
	{
		public static void CleanUpChilds(this Transform t)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				Object.Destroy(t.GetChild(i).gameObject);
			}
		}
	}
}