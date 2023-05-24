using System;
using UnityEngine;

// Token: 0x0200185D RID: 6237
public class AkAuxSendValue : IDisposable
{
	// Token: 0x06009384 RID: 37764 RVA: 0x003E334C File Offset: 0x003E154C
	internal AkAuxSendValue(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009385 RID: 37765 RVA: 0x003E3364 File Offset: 0x003E1564
	internal static IntPtr getCPtr(AkAuxSendValue obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009386 RID: 37766 RVA: 0x003E337C File Offset: 0x003E157C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009387 RID: 37767 RVA: 0x003E338C File Offset: 0x003E158C
	~AkAuxSendValue()
	{
		this.Dispose();
	}

	// Token: 0x06009388 RID: 37768 RVA: 0x003E33BC File Offset: 0x003E15BC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkAuxSendValue(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170015F6 RID: 5622
	// (get) Token: 0x0600938A RID: 37770 RVA: 0x003E3440 File Offset: 0x003E1640
	// (set) Token: 0x06009389 RID: 37769 RVA: 0x003E3430 File Offset: 0x003E1630
	public ulong listenerID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_listenerID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_listenerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015F7 RID: 5623
	// (get) Token: 0x0600938C RID: 37772 RVA: 0x003E3460 File Offset: 0x003E1660
	// (set) Token: 0x0600938B RID: 37771 RVA: 0x003E3450 File Offset: 0x003E1650
	public uint auxBusID
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_auxBusID_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_auxBusID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170015F8 RID: 5624
	// (get) Token: 0x0600938E RID: 37774 RVA: 0x003E3480 File Offset: 0x003E1680
	// (set) Token: 0x0600938D RID: 37773 RVA: 0x003E3470 File Offset: 0x003E1670
	public float fControlValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_fControlValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_fControlValue_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600938F RID: 37775 RVA: 0x003E3490 File Offset: 0x003E1690
	public void Set(GameObject listener, uint id, float value)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(listener);
		AkSoundEngine.PreGameObjectAPICall(listener, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(this.swigCPtr, akGameObjectID, id, value);
	}

	// Token: 0x06009390 RID: 37776 RVA: 0x003E34BC File Offset: 0x003E16BC
	public bool IsSame(GameObject listener, uint id)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(listener);
		AkSoundEngine.PreGameObjectAPICall(listener, akGameObjectID);
		return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(this.swigCPtr, akGameObjectID, id);
	}

	// Token: 0x06009391 RID: 37777 RVA: 0x003E34E4 File Offset: 0x003E16E4
	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetSizeOf();
	}

	// Token: 0x06009392 RID: 37778 RVA: 0x003E34EC File Offset: 0x003E16EC
	public AKRESULT SetGameObjectAuxSendValues(GameObject in_gameObjectID, uint in_uNumSendValues)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_SetGameObjectAuxSendValues(this.swigCPtr, akGameObjectID, in_uNumSendValues);
	}

	// Token: 0x06009393 RID: 37779 RVA: 0x003E3514 File Offset: 0x003E1714
	public AKRESULT GetGameObjectAuxSendValues(GameObject in_gameObjectID, ref uint io_ruNumSendValues)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetGameObjectAuxSendValues(this.swigCPtr, akGameObjectID, ref io_ruNumSendValues);
	}

	// Token: 0x04009AE3 RID: 39651
	private IntPtr swigCPtr;

	// Token: 0x04009AE4 RID: 39652
	protected bool swigCMemOwn;
}
