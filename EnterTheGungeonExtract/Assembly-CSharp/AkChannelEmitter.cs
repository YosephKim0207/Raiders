using System;

// Token: 0x02001866 RID: 6246
public class AkChannelEmitter : IDisposable
{
	// Token: 0x060093CA RID: 37834 RVA: 0x003E3AFC File Offset: 0x003E1CFC
	internal AkChannelEmitter(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093CB RID: 37835 RVA: 0x003E3B14 File Offset: 0x003E1D14
	public AkChannelEmitter()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkChannelEmitter(), true)
	{
	}

	// Token: 0x060093CC RID: 37836 RVA: 0x003E3B24 File Offset: 0x003E1D24
	internal static IntPtr getCPtr(AkChannelEmitter obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x060093CD RID: 37837 RVA: 0x003E3B3C File Offset: 0x003E1D3C
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x060093CE RID: 37838 RVA: 0x003E3B4C File Offset: 0x003E1D4C
	~AkChannelEmitter()
	{
		this.Dispose();
	}

	// Token: 0x060093CF RID: 37839 RVA: 0x003E3B7C File Offset: 0x003E1D7C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkChannelEmitter(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001602 RID: 5634
	// (get) Token: 0x060093D1 RID: 37841 RVA: 0x003E3C04 File Offset: 0x003E1E04
	// (set) Token: 0x060093D0 RID: 37840 RVA: 0x003E3BF0 File Offset: 0x003E1DF0
	public AkTransform position
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_position_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkTransform(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_position_set(this.swigCPtr, AkTransform.getCPtr(value));
		}
	}

	// Token: 0x17001603 RID: 5635
	// (get) Token: 0x060093D3 RID: 37843 RVA: 0x003E3C4C File Offset: 0x003E1E4C
	// (set) Token: 0x060093D2 RID: 37842 RVA: 0x003E3C3C File Offset: 0x003E1E3C
	public uint uInputChannels
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_uInputChannels_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkChannelEmitter_uInputChannels_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B12 RID: 39698
	private IntPtr swigCPtr;

	// Token: 0x04009B13 RID: 39699
	protected bool swigCMemOwn;
}
