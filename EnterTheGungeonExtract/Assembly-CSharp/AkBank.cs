using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018E3 RID: 6371
[AddComponentMenu("Wwise/AkBank")]
[ExecuteInEditMode]
public class AkBank : AkUnityEventHandler
{
	// Token: 0x06009D18 RID: 40216 RVA: 0x003EE3F0 File Offset: 0x003EC5F0
	protected override void Awake()
	{
		base.Awake();
		base.RegisterTriggers(this.unloadTriggerList, new AkTriggerBase.Trigger(this.UnloadBank));
		if (this.unloadTriggerList.Contains(1151176110))
		{
			this.UnloadBank(null);
		}
	}

	// Token: 0x06009D19 RID: 40217 RVA: 0x003EE42C File Offset: 0x003EC62C
	protected override void Start()
	{
		base.Start();
		if (this.unloadTriggerList.Contains(1281810935))
		{
			this.UnloadBank(null);
		}
	}

	// Token: 0x06009D1A RID: 40218 RVA: 0x003EE450 File Offset: 0x003EC650
	public override void HandleEvent(GameObject in_gameObject)
	{
		if (!this.loadAsynchronous)
		{
			AkBankManager.LoadBank(this.bankName, this.decodeBank, this.saveDecodedBank);
		}
		else
		{
			AkBankManager.LoadBankAsync(this.bankName, null);
		}
	}

	// Token: 0x06009D1B RID: 40219 RVA: 0x003EE488 File Offset: 0x003EC688
	public void UnloadBank(GameObject in_gameObject)
	{
		AkBankManager.UnloadBank(this.bankName);
	}

	// Token: 0x06009D1C RID: 40220 RVA: 0x003EE498 File Offset: 0x003EC698
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.UnregisterTriggers(this.unloadTriggerList, new AkTriggerBase.Trigger(this.UnloadBank));
		if (this.unloadTriggerList.Contains(-358577003))
		{
			this.UnloadBank(null);
		}
	}

	// Token: 0x04009E96 RID: 40598
	public string bankName = string.Empty;

	// Token: 0x04009E97 RID: 40599
	public bool decodeBank;

	// Token: 0x04009E98 RID: 40600
	public bool loadAsynchronous;

	// Token: 0x04009E99 RID: 40601
	public bool saveDecodedBank;

	// Token: 0x04009E9A RID: 40602
	public List<int> unloadTriggerList = new List<int> { -358577003 };
}
