using System;

// Token: 0x02001894 RID: 6292
public class AkPlaylistArray : IDisposable
{
	// Token: 0x06009582 RID: 38274 RVA: 0x003E6BE4 File Offset: 0x003E4DE4
	internal AkPlaylistArray(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009583 RID: 38275 RVA: 0x003E6BFC File Offset: 0x003E4DFC
	public AkPlaylistArray()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkPlaylistArray(), true)
	{
	}

	// Token: 0x06009584 RID: 38276 RVA: 0x003E6C0C File Offset: 0x003E4E0C
	internal static IntPtr getCPtr(AkPlaylistArray obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009585 RID: 38277 RVA: 0x003E6C24 File Offset: 0x003E4E24
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009586 RID: 38278 RVA: 0x003E6C34 File Offset: 0x003E4E34
	~AkPlaylistArray()
	{
		this.Dispose();
	}

	// Token: 0x06009587 RID: 38279 RVA: 0x003E6C64 File Offset: 0x003E4E64
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkPlaylistArray(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06009588 RID: 38280 RVA: 0x003E6CD8 File Offset: 0x003E4ED8
	public AkIterator Begin()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Begin(this.swigCPtr), true);
	}

	// Token: 0x06009589 RID: 38281 RVA: 0x003E6CF8 File Offset: 0x003E4EF8
	public AkIterator End()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_End(this.swigCPtr), true);
	}

	// Token: 0x0600958A RID: 38282 RVA: 0x003E6D18 File Offset: 0x003E4F18
	public AkIterator FindEx(AkPlaylistItem in_Item)
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_FindEx(this.swigCPtr, AkPlaylistItem.getCPtr(in_Item)), true);
	}

	// Token: 0x0600958B RID: 38283 RVA: 0x003E6D40 File Offset: 0x003E4F40
	public AkIterator Erase(AkIterator in_rIter)
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Erase__SWIG_0(this.swigCPtr, AkIterator.getCPtr(in_rIter)), true);
	}

	// Token: 0x0600958C RID: 38284 RVA: 0x003E6D68 File Offset: 0x003E4F68
	public void Erase(uint in_uIndex)
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Erase__SWIG_1(this.swigCPtr, in_uIndex);
	}

	// Token: 0x0600958D RID: 38285 RVA: 0x003E6D78 File Offset: 0x003E4F78
	public AkIterator EraseSwap(AkIterator in_rIter)
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_EraseSwap(this.swigCPtr, AkIterator.getCPtr(in_rIter)), true);
	}

	// Token: 0x0600958E RID: 38286 RVA: 0x003E6DA0 File Offset: 0x003E4FA0
	public AKRESULT Reserve(uint in_ulReserve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Reserve(this.swigCPtr, in_ulReserve);
	}

	// Token: 0x0600958F RID: 38287 RVA: 0x003E6DB0 File Offset: 0x003E4FB0
	public uint Reserved()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Reserved(this.swigCPtr);
	}

	// Token: 0x06009590 RID: 38288 RVA: 0x003E6DC0 File Offset: 0x003E4FC0
	public void Term()
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Term(this.swigCPtr);
	}

	// Token: 0x06009591 RID: 38289 RVA: 0x003E6DD0 File Offset: 0x003E4FD0
	public uint Length()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Length(this.swigCPtr);
	}

	// Token: 0x06009592 RID: 38290 RVA: 0x003E6DE0 File Offset: 0x003E4FE0
	public bool IsEmpty()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_IsEmpty(this.swigCPtr);
	}

	// Token: 0x06009593 RID: 38291 RVA: 0x003E6DF0 File Offset: 0x003E4FF0
	public AkPlaylistItem Exists(AkPlaylistItem in_Item)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Exists(this.swigCPtr, AkPlaylistItem.getCPtr(in_Item));
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	// Token: 0x06009594 RID: 38292 RVA: 0x003E6E30 File Offset: 0x003E5030
	public AkPlaylistItem AddLast()
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_AddLast__SWIG_0(this.swigCPtr);
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	// Token: 0x06009595 RID: 38293 RVA: 0x003E6E68 File Offset: 0x003E5068
	public AkPlaylistItem AddLast(AkPlaylistItem in_rItem)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_AddLast__SWIG_1(this.swigCPtr, AkPlaylistItem.getCPtr(in_rItem));
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	// Token: 0x06009596 RID: 38294 RVA: 0x003E6EA8 File Offset: 0x003E50A8
	public AkPlaylistItem Last()
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Last(this.swigCPtr), false);
	}

	// Token: 0x06009597 RID: 38295 RVA: 0x003E6EC8 File Offset: 0x003E50C8
	public void RemoveLast()
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_RemoveLast(this.swigCPtr);
	}

	// Token: 0x06009598 RID: 38296 RVA: 0x003E6ED8 File Offset: 0x003E50D8
	public AKRESULT Remove(AkPlaylistItem in_rItem)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Remove(this.swigCPtr, AkPlaylistItem.getCPtr(in_rItem));
	}

	// Token: 0x06009599 RID: 38297 RVA: 0x003E6EEC File Offset: 0x003E50EC
	public AKRESULT RemoveSwap(AkPlaylistItem in_rItem)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_RemoveSwap(this.swigCPtr, AkPlaylistItem.getCPtr(in_rItem));
	}

	// Token: 0x0600959A RID: 38298 RVA: 0x003E6F00 File Offset: 0x003E5100
	public void RemoveAll()
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_RemoveAll(this.swigCPtr);
	}

	// Token: 0x0600959B RID: 38299 RVA: 0x003E6F10 File Offset: 0x003E5110
	public AkPlaylistItem ItemAtIndex(uint uiIndex)
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_ItemAtIndex(this.swigCPtr, uiIndex), false);
	}

	// Token: 0x0600959C RID: 38300 RVA: 0x003E6F34 File Offset: 0x003E5134
	public AkPlaylistItem Insert(uint in_uIndex)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Insert(this.swigCPtr, in_uIndex);
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
	}

	// Token: 0x0600959D RID: 38301 RVA: 0x003E6F70 File Offset: 0x003E5170
	public bool GrowArray(uint in_uGrowBy)
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_GrowArray__SWIG_0(this.swigCPtr, in_uGrowBy);
	}

	// Token: 0x0600959E RID: 38302 RVA: 0x003E6F80 File Offset: 0x003E5180
	public bool GrowArray()
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_GrowArray__SWIG_1(this.swigCPtr);
	}

	// Token: 0x0600959F RID: 38303 RVA: 0x003E6F90 File Offset: 0x003E5190
	public bool Resize(uint in_uiSize)
	{
		return AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Resize(this.swigCPtr, in_uiSize);
	}

	// Token: 0x060095A0 RID: 38304 RVA: 0x003E6FA0 File Offset: 0x003E51A0
	public void Transfer(AkPlaylistArray in_rSource)
	{
		AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Transfer(this.swigCPtr, AkPlaylistArray.getCPtr(in_rSource));
	}

	// Token: 0x060095A1 RID: 38305 RVA: 0x003E6FB4 File Offset: 0x003E51B4
	public AKRESULT Copy(AkPlaylistArray in_rSource)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkPlaylistArray_Copy(this.swigCPtr, AkPlaylistArray.getCPtr(in_rSource));
	}

	// Token: 0x04009C49 RID: 40009
	private IntPtr swigCPtr;

	// Token: 0x04009C4A RID: 40010
	protected bool swigCMemOwn;
}
