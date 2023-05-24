using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x020018BB RID: 6331
public static class AkBankManager
{
	// Token: 0x06009C56 RID: 40022 RVA: 0x003EBD5C File Offset: 0x003E9F5C
	internal static void DoUnloadBanks()
	{
		int count = AkBankManager.BanksToUnload.Count;
		for (int i = 0; i < count; i++)
		{
			AkBankManager.BanksToUnload[i].UnloadBank();
		}
		AkBankManager.BanksToUnload.Clear();
	}

	// Token: 0x06009C57 RID: 40023 RVA: 0x003EBDA0 File Offset: 0x003E9FA0
	internal static void Reset()
	{
		AkBankManager.m_BankHandles.Clear();
		AkBankManager.BanksToUnload.Clear();
	}

	// Token: 0x06009C58 RID: 40024 RVA: 0x003EBDB8 File Offset: 0x003E9FB8
	public static void LoadBank(string name, bool decodeBank, bool saveDecodedBank)
	{
		AkBankManager.m_Mutex.WaitOne();
		AkBankManager.BankHandle bankHandle = null;
		if (!AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
		{
			bankHandle = ((!decodeBank) ? new AkBankManager.BankHandle(name) : new AkBankManager.DecodableBankHandle(name, saveDecodedBank));
			AkBankManager.m_BankHandles.Add(name, bankHandle);
			AkBankManager.m_Mutex.ReleaseMutex();
			bankHandle.LoadBank();
		}
		else
		{
			bankHandle.IncRef();
			AkBankManager.m_Mutex.ReleaseMutex();
		}
	}

	// Token: 0x06009C59 RID: 40025 RVA: 0x003EBE30 File Offset: 0x003EA030
	public static void LoadBankAsync(string name, AkCallbackManager.BankCallback callback = null)
	{
		AkBankManager.m_Mutex.WaitOne();
		AkBankManager.BankHandle bankHandle = null;
		if (!AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
		{
			AkBankManager.AsyncBankHandle asyncBankHandle = new AkBankManager.AsyncBankHandle(name, callback);
			AkBankManager.m_BankHandles.Add(name, asyncBankHandle);
			AkBankManager.m_Mutex.ReleaseMutex();
			asyncBankHandle.LoadBank();
		}
		else
		{
			bankHandle.IncRef();
			AkBankManager.m_Mutex.ReleaseMutex();
		}
	}

	// Token: 0x06009C5A RID: 40026 RVA: 0x003EBE98 File Offset: 0x003EA098
	public static void UnloadBank(string name)
	{
		AkBankManager.m_Mutex.WaitOne();
		AkBankManager.BankHandle bankHandle = null;
		if (AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
		{
			bankHandle.DecRef();
			if (bankHandle.RefCount == 0)
			{
				AkBankManager.m_BankHandles.Remove(name);
			}
		}
		AkBankManager.m_Mutex.ReleaseMutex();
	}

	// Token: 0x04009E41 RID: 40513
	private static readonly Dictionary<string, AkBankManager.BankHandle> m_BankHandles = new Dictionary<string, AkBankManager.BankHandle>();

	// Token: 0x04009E42 RID: 40514
	private static readonly List<AkBankManager.BankHandle> BanksToUnload = new List<AkBankManager.BankHandle>();

	// Token: 0x04009E43 RID: 40515
	private static readonly Mutex m_Mutex = new Mutex();

	// Token: 0x020018BC RID: 6332
	private class BankHandle
	{
		// Token: 0x06009C5C RID: 40028 RVA: 0x003EBF0C File Offset: 0x003EA10C
		public BankHandle(string name)
		{
			this.bankName = name;
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x06009C5D RID: 40029 RVA: 0x003EBF1C File Offset: 0x003EA11C
		// (set) Token: 0x06009C5E RID: 40030 RVA: 0x003EBF24 File Offset: 0x003EA124
		public int RefCount { get; private set; }

		// Token: 0x06009C5F RID: 40031 RVA: 0x003EBF30 File Offset: 0x003EA130
		public virtual AKRESULT DoLoadBank()
		{
			return AkSoundEngine.LoadBank(this.bankName, -1, out this.m_BankID);
		}

		// Token: 0x06009C60 RID: 40032 RVA: 0x003EBF44 File Offset: 0x003EA144
		public void LoadBank()
		{
			if (this.RefCount == 0)
			{
				if (AkBankManager.BanksToUnload.Contains(this))
				{
					AkBankManager.BanksToUnload.Remove(this);
				}
				else
				{
					AKRESULT akresult = this.DoLoadBank();
					this.LogLoadResult(akresult);
				}
			}
			this.IncRef();
		}

		// Token: 0x06009C61 RID: 40033 RVA: 0x003EBF94 File Offset: 0x003EA194
		public virtual void UnloadBank()
		{
			AkSoundEngine.UnloadBank(this.m_BankID, IntPtr.Zero, null, null);
		}

		// Token: 0x06009C62 RID: 40034 RVA: 0x003EBFAC File Offset: 0x003EA1AC
		public void IncRef()
		{
			this.RefCount++;
		}

		// Token: 0x06009C63 RID: 40035 RVA: 0x003EBFBC File Offset: 0x003EA1BC
		public void DecRef()
		{
			this.RefCount--;
			if (this.RefCount == 0)
			{
				AkBankManager.BanksToUnload.Add(this);
			}
		}

		// Token: 0x06009C64 RID: 40036 RVA: 0x003EBFE4 File Offset: 0x003EA1E4
		protected void LogLoadResult(AKRESULT result)
		{
			if (result != AKRESULT.AK_Success && AkSoundEngine.IsInitialized())
			{
				Debug.LogWarning(string.Concat(new object[] { "WwiseUnity: Bank ", this.bankName, " failed to load (", result, ")" }));
			}
		}

		// Token: 0x04009E44 RID: 40516
		protected readonly string bankName;

		// Token: 0x04009E45 RID: 40517
		protected uint m_BankID;
	}

	// Token: 0x020018BD RID: 6333
	private class AsyncBankHandle : AkBankManager.BankHandle
	{
		// Token: 0x06009C65 RID: 40037 RVA: 0x003EC03C File Offset: 0x003EA23C
		public AsyncBankHandle(string name, AkCallbackManager.BankCallback callback)
			: base(name)
		{
			this.bankCallback = callback;
		}

		// Token: 0x06009C66 RID: 40038 RVA: 0x003EC04C File Offset: 0x003EA24C
		private static void GlobalBankCallback(uint in_bankID, IntPtr in_pInMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie)
		{
			AkBankManager.m_Mutex.WaitOne();
			AkBankManager.AsyncBankHandle asyncBankHandle = (AkBankManager.AsyncBankHandle)in_Cookie;
			AkCallbackManager.BankCallback bankCallback = asyncBankHandle.bankCallback;
			if (in_eLoadResult != AKRESULT.AK_Success)
			{
				asyncBankHandle.LogLoadResult(in_eLoadResult);
				AkBankManager.m_BankHandles.Remove(asyncBankHandle.bankName);
			}
			AkBankManager.m_Mutex.ReleaseMutex();
			if (bankCallback != null)
			{
				bankCallback(in_bankID, in_pInMemoryBankPtr, in_eLoadResult, in_memPoolId, null);
			}
		}

		// Token: 0x06009C67 RID: 40039 RVA: 0x003EC0B0 File Offset: 0x003EA2B0
		public override AKRESULT DoLoadBank()
		{
			return AkSoundEngine.LoadBank(this.bankName, new AkCallbackManager.BankCallback(AkBankManager.AsyncBankHandle.GlobalBankCallback), this, -1, out this.m_BankID);
		}

		// Token: 0x04009E47 RID: 40519
		private readonly AkCallbackManager.BankCallback bankCallback;
	}

	// Token: 0x020018BE RID: 6334
	private class DecodableBankHandle : AkBankManager.BankHandle
	{
		// Token: 0x06009C68 RID: 40040 RVA: 0x003EC0E4 File Offset: 0x003EA2E4
		public DecodableBankHandle(string name, bool save)
			: base(name)
		{
			this.saveDecodedBank = save;
			string text = this.bankName + ".bnk";
			string currentLanguage = AkInitializer.GetCurrentLanguage();
			this.decodedBankPath = Path.Combine(AkSoundEngineController.GetDecodedBankFullPath(), currentLanguage);
			string text2 = Path.Combine(this.decodedBankPath, text);
			bool flag = File.Exists(text2);
			if (!flag)
			{
				this.decodedBankPath = AkSoundEngineController.GetDecodedBankFullPath();
				text2 = Path.Combine(this.decodedBankPath, text);
				flag = File.Exists(text2);
			}
			if (flag)
			{
				try
				{
					DateTime lastWriteTime = File.GetLastWriteTime(text2);
					string soundbankBasePath = AkBasePathGetter.GetSoundbankBasePath();
					string text3 = Path.Combine(soundbankBasePath, text);
					DateTime lastWriteTime2 = File.GetLastWriteTime(text3);
					this.decodeBank = lastWriteTime <= lastWriteTime2;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06009C69 RID: 40041 RVA: 0x003EC1BC File Offset: 0x003EA3BC
		public override AKRESULT DoLoadBank()
		{
			if (this.decodeBank)
			{
				return AkSoundEngine.LoadAndDecodeBank(this.bankName, this.saveDecodedBank, out this.m_BankID);
			}
			AKRESULT akresult = AKRESULT.AK_Success;
			if (!string.IsNullOrEmpty(this.decodedBankPath))
			{
				akresult = AkSoundEngine.SetBasePath(this.decodedBankPath);
			}
			if (akresult == AKRESULT.AK_Success)
			{
				akresult = AkSoundEngine.LoadBank(this.bankName, -1, out this.m_BankID);
				if (!string.IsNullOrEmpty(this.decodedBankPath))
				{
					AkSoundEngine.SetBasePath(AkBasePathGetter.GetSoundbankBasePath());
				}
			}
			return akresult;
		}

		// Token: 0x06009C6A RID: 40042 RVA: 0x003EC240 File Offset: 0x003EA440
		public override void UnloadBank()
		{
			if (this.decodeBank && !this.saveDecodedBank)
			{
				AkSoundEngine.PrepareBank(AkPreparationType.Preparation_Unload, this.m_BankID);
			}
			else
			{
				base.UnloadBank();
			}
		}

		// Token: 0x04009E49 RID: 40521
		private readonly bool decodeBank = true;

		// Token: 0x04009E4A RID: 40522
		private readonly string decodedBankPath;

		// Token: 0x04009E4B RID: 40523
		private readonly bool saveDecodedBank;
	}
}
