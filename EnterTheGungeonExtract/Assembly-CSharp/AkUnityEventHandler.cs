using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001919 RID: 6425
public abstract class AkUnityEventHandler : MonoBehaviour
{
	// Token: 0x06009E21 RID: 40481
	public abstract void HandleEvent(GameObject in_gameObject);

	// Token: 0x06009E22 RID: 40482 RVA: 0x003F24E4 File Offset: 0x003F06E4
	protected virtual void Awake()
	{
		this.RegisterTriggers(this.triggerList, new AkTriggerBase.Trigger(this.HandleEvent));
		if (this.triggerList.Contains(1151176110))
		{
			this.HandleEvent(null);
		}
	}

	// Token: 0x06009E23 RID: 40483 RVA: 0x003F251C File Offset: 0x003F071C
	protected virtual void Start()
	{
		if (this.triggerList.Contains(1281810935))
		{
			this.HandleEvent(null);
		}
	}

	// Token: 0x06009E24 RID: 40484 RVA: 0x003F253C File Offset: 0x003F073C
	protected virtual void OnDestroy()
	{
		if (!this.didDestroy)
		{
			this.DoDestroy();
		}
	}

	// Token: 0x06009E25 RID: 40485 RVA: 0x003F2550 File Offset: 0x003F0750
	public void DoDestroy()
	{
		this.UnregisterTriggers(this.triggerList, new AkTriggerBase.Trigger(this.HandleEvent));
		this.didDestroy = true;
		if (this.triggerList.Contains(-358577003))
		{
			this.HandleEvent(null);
		}
	}

	// Token: 0x06009E26 RID: 40486 RVA: 0x003F2590 File Offset: 0x003F0790
	protected void RegisterTriggers(List<int> in_triggerList, AkTriggerBase.Trigger in_delegate)
	{
		using (List<int>.Enumerator enumerator = in_triggerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				uint num = (uint)enumerator.Current;
				string empty = string.Empty;
				if (AkUnityEventHandler.triggerTypes.TryGetValue(num, out empty))
				{
					if (!(empty == "Awake") && !(empty == "Start") && !(empty == "Destroy"))
					{
						AkTriggerBase akTriggerBase = (AkTriggerBase)base.GetComponent(Type.GetType(empty));
						if (akTriggerBase == null)
						{
							akTriggerBase = (AkTriggerBase)base.gameObject.AddComponent(Type.GetType(empty));
						}
						AkTriggerBase akTriggerBase2 = akTriggerBase;
						akTriggerBase2.triggerDelegate = (AkTriggerBase.Trigger)Delegate.Combine(akTriggerBase2.triggerDelegate, in_delegate);
					}
				}
			}
		}
	}

	// Token: 0x06009E27 RID: 40487 RVA: 0x003F267C File Offset: 0x003F087C
	protected void UnregisterTriggers(List<int> in_triggerList, AkTriggerBase.Trigger in_delegate)
	{
		using (List<int>.Enumerator enumerator = in_triggerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				uint num = (uint)enumerator.Current;
				string empty = string.Empty;
				if (AkUnityEventHandler.triggerTypes.TryGetValue(num, out empty))
				{
					if (!(empty == "Awake") && !(empty == "Start") && !(empty == "Destroy"))
					{
						AkTriggerBase akTriggerBase = (AkTriggerBase)base.GetComponent(Type.GetType(empty));
						if (akTriggerBase != null)
						{
							AkTriggerBase akTriggerBase2 = akTriggerBase;
							akTriggerBase2.triggerDelegate = (AkTriggerBase.Trigger)Delegate.Remove(akTriggerBase2.triggerDelegate, in_delegate);
							if (akTriggerBase.triggerDelegate == null)
							{
								UnityEngine.Object.Destroy(akTriggerBase);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x04009F65 RID: 40805
	public const int AWAKE_TRIGGER_ID = 1151176110;

	// Token: 0x04009F66 RID: 40806
	public const int START_TRIGGER_ID = 1281810935;

	// Token: 0x04009F67 RID: 40807
	public const int DESTROY_TRIGGER_ID = -358577003;

	// Token: 0x04009F68 RID: 40808
	public const int MAX_NB_TRIGGERS = 32;

	// Token: 0x04009F69 RID: 40809
	public static Dictionary<uint, string> triggerTypes = AkTriggerBase.GetAllDerivedTypes();

	// Token: 0x04009F6A RID: 40810
	private bool didDestroy;

	// Token: 0x04009F6B RID: 40811
	public List<int> triggerList = new List<int> { 1281810935 };

	// Token: 0x04009F6C RID: 40812
	public bool useOtherObject;
}
