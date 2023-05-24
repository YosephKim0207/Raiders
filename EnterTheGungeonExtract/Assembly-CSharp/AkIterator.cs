using System;

// Token: 0x02001877 RID: 6263
public class AkIterator : IDisposable
{
	// Token: 0x06009473 RID: 38003 RVA: 0x003E4C68 File Offset: 0x003E2E68
	internal AkIterator(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009474 RID: 38004 RVA: 0x003E4C80 File Offset: 0x003E2E80
	public AkIterator()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkIterator(), true)
	{
	}

	// Token: 0x06009475 RID: 38005 RVA: 0x003E4C90 File Offset: 0x003E2E90
	internal static IntPtr getCPtr(AkIterator obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009476 RID: 38006 RVA: 0x003E4CA8 File Offset: 0x003E2EA8
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009477 RID: 38007 RVA: 0x003E4CB8 File Offset: 0x003E2EB8
	~AkIterator()
	{
		this.Dispose();
	}

	// Token: 0x06009478 RID: 38008 RVA: 0x003E4CE8 File Offset: 0x003E2EE8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkIterator(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700163A RID: 5690
	// (get) Token: 0x0600947A RID: 38010 RVA: 0x003E4D70 File Offset: 0x003E2F70
	// (set) Token: 0x06009479 RID: 38009 RVA: 0x003E4D5C File Offset: 0x003E2F5C
	public AkPlaylistItem pItem
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkIterator_pItem_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkPlaylistItem(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkIterator_pItem_set(this.swigCPtr, AkPlaylistItem.getCPtr(value));
		}
	}

	// Token: 0x0600947B RID: 38011 RVA: 0x003E4DA8 File Offset: 0x003E2FA8
	public AkIterator NextIter()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkIterator_NextIter(this.swigCPtr), false);
	}

	// Token: 0x0600947C RID: 38012 RVA: 0x003E4DC8 File Offset: 0x003E2FC8
	public AkIterator PrevIter()
	{
		return new AkIterator(AkSoundEnginePINVOKE.CSharp_AkIterator_PrevIter(this.swigCPtr), false);
	}

	// Token: 0x0600947D RID: 38013 RVA: 0x003E4DE8 File Offset: 0x003E2FE8
	public AkPlaylistItem GetItem()
	{
		return new AkPlaylistItem(AkSoundEnginePINVOKE.CSharp_AkIterator_GetItem(this.swigCPtr), false);
	}

	// Token: 0x0600947E RID: 38014 RVA: 0x003E4E08 File Offset: 0x003E3008
	public bool IsEqualTo(AkIterator in_rOp)
	{
		return AkSoundEnginePINVOKE.CSharp_AkIterator_IsEqualTo(this.swigCPtr, AkIterator.getCPtr(in_rOp));
	}

	// Token: 0x0600947F RID: 38015 RVA: 0x003E4E1C File Offset: 0x003E301C
	public bool IsDifferentFrom(AkIterator in_rOp)
	{
		return AkSoundEnginePINVOKE.CSharp_AkIterator_IsDifferentFrom(this.swigCPtr, AkIterator.getCPtr(in_rOp));
	}

	// Token: 0x04009B4C RID: 39756
	private IntPtr swigCPtr;

	// Token: 0x04009B4D RID: 39757
	protected bool swigCMemOwn;
}
