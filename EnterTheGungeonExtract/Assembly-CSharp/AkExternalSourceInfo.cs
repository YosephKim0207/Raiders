using System;

// Token: 0x02001871 RID: 6257
public class AkExternalSourceInfo : IDisposable
{
	// Token: 0x06009420 RID: 37920 RVA: 0x003E445C File Offset: 0x003E265C
	internal AkExternalSourceInfo(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009421 RID: 37921 RVA: 0x003E4474 File Offset: 0x003E2674
	public AkExternalSourceInfo()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_0(), true)
	{
	}

	// Token: 0x06009422 RID: 37922 RVA: 0x003E4484 File Offset: 0x003E2684
	public AkExternalSourceInfo(IntPtr in_pInMemory, uint in_uiMemorySize, uint in_iExternalSrcCookie, uint in_idCodec)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_1(in_pInMemory, in_uiMemorySize, in_iExternalSrcCookie, in_idCodec), true)
	{
	}

	// Token: 0x06009423 RID: 37923 RVA: 0x003E4498 File Offset: 0x003E2698
	public AkExternalSourceInfo(string in_pszFileName, uint in_iExternalSrcCookie, uint in_idCodec)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_2(in_pszFileName, in_iExternalSrcCookie, in_idCodec), true)
	{
	}

	// Token: 0x06009424 RID: 37924 RVA: 0x003E44AC File Offset: 0x003E26AC
	public AkExternalSourceInfo(uint in_idFile, uint in_iExternalSrcCookie, uint in_idCodec)
		: this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_3(in_idFile, in_iExternalSrcCookie, in_idCodec), true)
	{
	}

	// Token: 0x06009425 RID: 37925 RVA: 0x003E44C0 File Offset: 0x003E26C0
	internal static IntPtr getCPtr(AkExternalSourceInfo obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009426 RID: 37926 RVA: 0x003E44D8 File Offset: 0x003E26D8
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009427 RID: 37927 RVA: 0x003E44E8 File Offset: 0x003E26E8
	~AkExternalSourceInfo()
	{
		this.Dispose();
	}

	// Token: 0x06009428 RID: 37928 RVA: 0x003E4518 File Offset: 0x003E2718
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkExternalSourceInfo(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001620 RID: 5664
	// (get) Token: 0x0600942A RID: 37930 RVA: 0x003E459C File Offset: 0x003E279C
	// (set) Token: 0x06009429 RID: 37929 RVA: 0x003E458C File Offset: 0x003E278C
	public uint iExternalSrcCookie
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_iExternalSrcCookie_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_iExternalSrcCookie_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001621 RID: 5665
	// (get) Token: 0x0600942C RID: 37932 RVA: 0x003E45BC File Offset: 0x003E27BC
	// (set) Token: 0x0600942B RID: 37931 RVA: 0x003E45AC File Offset: 0x003E27AC
	public uint idCodec
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idCodec_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idCodec_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001622 RID: 5666
	// (get) Token: 0x0600942E RID: 37934 RVA: 0x003E45DC File Offset: 0x003E27DC
	// (set) Token: 0x0600942D RID: 37933 RVA: 0x003E45CC File Offset: 0x003E27CC
	public string szFile
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_szFile_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_szFile_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001623 RID: 5667
	// (get) Token: 0x06009430 RID: 37936 RVA: 0x003E45FC File Offset: 0x003E27FC
	// (set) Token: 0x0600942F RID: 37935 RVA: 0x003E45EC File Offset: 0x003E27EC
	public IntPtr pInMemory
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_pInMemory_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_pInMemory_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001624 RID: 5668
	// (get) Token: 0x06009432 RID: 37938 RVA: 0x003E461C File Offset: 0x003E281C
	// (set) Token: 0x06009431 RID: 37937 RVA: 0x003E460C File Offset: 0x003E280C
	public uint uiMemorySize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_uiMemorySize_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_uiMemorySize_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001625 RID: 5669
	// (get) Token: 0x06009434 RID: 37940 RVA: 0x003E463C File Offset: 0x003E283C
	// (set) Token: 0x06009433 RID: 37939 RVA: 0x003E462C File Offset: 0x003E282C
	public uint idFile
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idFile_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idFile_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009B36 RID: 39734
	private IntPtr swigCPtr;

	// Token: 0x04009B37 RID: 39735
	protected bool swigCMemOwn;
}
