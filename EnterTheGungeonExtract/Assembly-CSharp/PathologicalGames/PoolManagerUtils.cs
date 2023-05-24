using System;
using UnityEngine;

namespace PathologicalGames
{
	// Token: 0x0200083E RID: 2110
	public static class PoolManagerUtils
	{
		// Token: 0x06002DF9 RID: 11769 RVA: 0x000EF3E4 File Offset: 0x000ED5E4
		internal static void SetActive(GameObject obj, bool state)
		{
			obj.SetActive(state);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000EF3F0 File Offset: 0x000ED5F0
		internal static bool activeInHierarchy(GameObject obj)
		{
			return obj.activeInHierarchy;
		}
	}
}
