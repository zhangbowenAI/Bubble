
using UnityEngine;

namespace LightGive
{
	[CreateAssetMenu(menuName = "LightGive/Create ManagerCreator", fileName = "ManagerCreator")]
	public class ManagerCreator : ScriptableObject
	{
		public const string CreatorName = "ManagerCreator";

		public const string CreatorPath = "LightGive/Create ManagerCreator";

		[SerializeField]
		[Tooltip("Whether to issue a log when generated")]
		private bool m_isCheckLog = true;

		[SerializeField]
		private GameObject[] m_createManagers;

		public GameObject[] createManagers => m_createManagers;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeBeforeSceneLoad()
		{
			ManagerCreator managerCreator = Resources.Load<ManagerCreator>("ManagerCreator");
			if (managerCreator == null)
			{
				UnityEngine.Debug.Log("Manager Creator does not exist.\nIn project view Create/LightGive/Create ManagerCreator from generate.");
				return;
			}
			string text = string.Empty;
			for (int i = 0; i < managerCreator.createManagers.Length; i++)
			{
				if (!(managerCreator.createManagers[i] == null))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(managerCreator.createManagers[i]);
					string text2 = text;
					text = text2 + "\n" + (i + 1).ToString("0") + "." + gameObject.name + ",";
				}
			}
			if (managerCreator.m_isCheckLog)
			{
				UnityEngine.Debug.Log("Create manager class complete." + text);
			}
		}
	}
}
