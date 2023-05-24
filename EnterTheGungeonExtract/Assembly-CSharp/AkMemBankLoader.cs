using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018F8 RID: 6392
public class AkMemBankLoader : MonoBehaviour
{
	// Token: 0x06009D8B RID: 40331 RVA: 0x003F026C File Offset: 0x003EE46C
	private void Start()
	{
		if (this.isLocalizedBank)
		{
			this.LoadLocalizedBank(this.bankName);
		}
		else
		{
			this.LoadNonLocalizedBank(this.bankName);
		}
	}

	// Token: 0x06009D8C RID: 40332 RVA: 0x003F0298 File Offset: 0x003EE498
	public void LoadNonLocalizedBank(string in_bankFilename)
	{
		string text = "file://" + Path.Combine(AkBasePathGetter.GetPlatformBasePath(), in_bankFilename);
		this.DoLoadBank(text);
	}

	// Token: 0x06009D8D RID: 40333 RVA: 0x003F02C4 File Offset: 0x003EE4C4
	public void LoadLocalizedBank(string in_bankFilename)
	{
		string text = "file://" + Path.Combine(Path.Combine(AkBasePathGetter.GetPlatformBasePath(), AkInitializer.GetCurrentLanguage()), in_bankFilename);
		this.DoLoadBank(text);
	}

	// Token: 0x06009D8E RID: 40334 RVA: 0x003F02F8 File Offset: 0x003EE4F8
	private IEnumerator LoadFile()
	{
		this.ms_www = new WWW(this.m_bankPath);
		yield return this.ms_www;
		uint in_uInMemoryBankSize = 0U;
		try
		{
			this.ms_pinnedArray = GCHandle.Alloc(this.ms_www.bytes, GCHandleType.Pinned);
			this.ms_pInMemoryBankPtr = this.ms_pinnedArray.AddrOfPinnedObject();
			in_uInMemoryBankSize = (uint)this.ms_www.bytes.Length;
			if ((this.ms_pInMemoryBankPtr.ToInt64() & 15L) != 0L)
			{
				byte[] array = new byte[(long)this.ms_www.bytes.Length + 16L];
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				int num = 0;
				if ((intPtr.ToInt64() & 15L) != 0L)
				{
					long num2 = (intPtr.ToInt64() + 15L) & -16L;
					num = (int)(num2 - intPtr.ToInt64());
					intPtr = new IntPtr(num2);
				}
				Array.Copy(this.ms_www.bytes, 0, array, num, this.ms_www.bytes.Length);
				this.ms_pInMemoryBankPtr = intPtr;
				this.ms_pinnedArray.Free();
				this.ms_pinnedArray = gchandle;
			}
		}
		catch
		{
			yield break;
		}
		AKRESULT result = AkSoundEngine.LoadBank(this.ms_pInMemoryBankPtr, in_uInMemoryBankSize, out this.ms_bankID);
		if (result != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: AkMemBankLoader: bank loading failed with result " + result);
		}
		yield break;
	}

	// Token: 0x06009D8F RID: 40335 RVA: 0x003F0314 File Offset: 0x003EE514
	private void DoLoadBank(string in_bankPath)
	{
		this.m_bankPath = in_bankPath;
		base.StartCoroutine(this.LoadFile());
	}

	// Token: 0x06009D90 RID: 40336 RVA: 0x003F032C File Offset: 0x003EE52C
	private void OnDestroy()
	{
		if (this.ms_pInMemoryBankPtr != IntPtr.Zero)
		{
			AKRESULT akresult = AkSoundEngine.UnloadBank(this.ms_bankID, this.ms_pInMemoryBankPtr);
			if (akresult == AKRESULT.AK_Success)
			{
				this.ms_pinnedArray.Free();
			}
		}
	}

	// Token: 0x04009F0A RID: 40714
	private const int WaitMs = 50;

	// Token: 0x04009F0B RID: 40715
	private const long AK_BANK_PLATFORM_DATA_ALIGNMENT = 16L;

	// Token: 0x04009F0C RID: 40716
	private const long AK_BANK_PLATFORM_DATA_ALIGNMENT_MASK = 15L;

	// Token: 0x04009F0D RID: 40717
	public string bankName = string.Empty;

	// Token: 0x04009F0E RID: 40718
	public bool isLocalizedBank;

	// Token: 0x04009F0F RID: 40719
	private string m_bankPath;

	// Token: 0x04009F10 RID: 40720
	[HideInInspector]
	public uint ms_bankID;

	// Token: 0x04009F11 RID: 40721
	private IntPtr ms_pInMemoryBankPtr = IntPtr.Zero;

	// Token: 0x04009F12 RID: 40722
	private GCHandle ms_pinnedArray;

	// Token: 0x04009F13 RID: 40723
	private WWW ms_www;
}
