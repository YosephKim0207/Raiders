using System;

namespace AK.Wwise
{
	// Token: 0x020018DB RID: 6363
	[Serializable]
	public class Bank : BaseType
	{
		// Token: 0x06009CF2 RID: 40178 RVA: 0x003EDBCC File Offset: 0x003EBDCC
		public void Load(bool decodeBank = false, bool saveDecodedBank = false)
		{
			if (this.IsValid())
			{
				AkBankManager.LoadBank(this.name, decodeBank, saveDecodedBank);
			}
		}

		// Token: 0x06009CF3 RID: 40179 RVA: 0x003EDBE8 File Offset: 0x003EBDE8
		public void LoadAsync(AkCallbackManager.BankCallback callback = null)
		{
			if (this.IsValid())
			{
				AkBankManager.LoadBankAsync(this.name, callback);
			}
		}

		// Token: 0x06009CF4 RID: 40180 RVA: 0x003EDC04 File Offset: 0x003EBE04
		public void Unload()
		{
			if (this.IsValid())
			{
				AkBankManager.UnloadBank(this.name);
			}
		}

		// Token: 0x06009CF5 RID: 40181 RVA: 0x003EDC1C File Offset: 0x003EBE1C
		public override bool IsValid()
		{
			return this.name.Length != 0 || base.IsValid();
		}

		// Token: 0x04009E82 RID: 40578
		public string name;
	}
}
