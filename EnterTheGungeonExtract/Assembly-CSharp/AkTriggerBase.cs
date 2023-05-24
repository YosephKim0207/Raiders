using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200190D RID: 6413
public abstract class AkTriggerBase : MonoBehaviour
{
	// Token: 0x06009E06 RID: 40454 RVA: 0x003F2164 File Offset: 0x003F0364
	public static Dictionary<uint, string> GetAllDerivedTypes()
	{
		Dictionary<uint, string> dictionary = new Dictionary<uint, string>();
		Type typeFromHandle = typeof(AkTriggerBase);
		Type[] types = typeFromHandle.Assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			if (types[i].IsClass && (types[i].IsSubclassOf(typeFromHandle) || (typeFromHandle.IsAssignableFrom(types[i]) && typeFromHandle != types[i])))
			{
				string name = types[i].Name;
				dictionary.Add(AkUtilities.ShortIDGenerator.Compute(name), name);
			}
		}
		dictionary.Add(AkUtilities.ShortIDGenerator.Compute("Awake"), "Awake");
		dictionary.Add(AkUtilities.ShortIDGenerator.Compute("Start"), "Start");
		dictionary.Add(AkUtilities.ShortIDGenerator.Compute("Destroy"), "Destroy");
		return dictionary;
	}

	// Token: 0x04009F60 RID: 40800
	public AkTriggerBase.Trigger triggerDelegate;

	// Token: 0x0200190E RID: 6414
	// (Invoke) Token: 0x06009E08 RID: 40456
	public delegate void Trigger(GameObject in_gameObject);
}
