using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017EF RID: 6127
public static class IntToStringSansGarbage
{
	// Token: 0x0600903D RID: 36925 RVA: 0x003CF4B4 File Offset: 0x003CD6B4
	public static string GetStringForInt(int input)
	{
		if (IntToStringSansGarbage.m_map.ContainsKey(input))
		{
			return IntToStringSansGarbage.m_map[input];
		}
		string text = input.ToString();
		IntToStringSansGarbage.m_map.Add(input, text);
		if (IntToStringSansGarbage.m_map.Count > 25000)
		{
			Debug.LogError("Int To String (sans Garbage) map count greater than 25000!");
			IntToStringSansGarbage.m_map.Clear();
		}
		return text;
	}

	// Token: 0x0400985B RID: 39003
	private static Dictionary<int, string> m_map = new Dictionary<int, string>();
}
