using System;
using UnityEngine;

// Token: 0x02001886 RID: 6278
public class AkMIDIPost : AkMIDIEvent
{
	// Token: 0x06009515 RID: 38165 RVA: 0x003E5F34 File Offset: 0x003E4134
	internal AkMIDIPost(IntPtr cPtr, bool cMemoryOwn)
		: base(AkSoundEnginePINVOKE.CSharp_AkMIDIPost_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009516 RID: 38166 RVA: 0x003E5F4C File Offset: 0x003E414C
	public AkMIDIPost()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIPost(), true)
	{
	}

	// Token: 0x06009517 RID: 38167 RVA: 0x003E5F5C File Offset: 0x003E415C
	internal static IntPtr getCPtr(AkMIDIPost obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009518 RID: 38168 RVA: 0x003E5F74 File Offset: 0x003E4174
	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMIDIPost_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009519 RID: 38169 RVA: 0x003E5F8C File Offset: 0x003E418C
	~AkMIDIPost()
	{
		this.Dispose();
	}

	// Token: 0x0600951A RID: 38170 RVA: 0x003E5FBC File Offset: 0x003E41BC
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMIDIPost(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700166D RID: 5741
	// (get) Token: 0x0600951C RID: 38172 RVA: 0x003E6048 File Offset: 0x003E4248
	// (set) Token: 0x0600951B RID: 38171 RVA: 0x003E6038 File Offset: 0x003E4238
	public uint uOffset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIPost_uOffset_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIPost_uOffset_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600951D RID: 38173 RVA: 0x003E6058 File Offset: 0x003E4258
	public AKRESULT PostOnEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uNumPosts)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkMIDIPost_PostOnEvent(this.swigCPtr, in_eventID, akGameObjectID, in_uNumPosts);
	}

	// Token: 0x0600951E RID: 38174 RVA: 0x003E6084 File Offset: 0x003E4284
	public void Clone(AkMIDIPost other)
	{
		AkSoundEnginePINVOKE.CSharp_AkMIDIPost_Clone(this.swigCPtr, AkMIDIPost.getCPtr(other));
	}

	// Token: 0x0600951F RID: 38175 RVA: 0x003E6098 File Offset: 0x003E4298
	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkMIDIPost_GetSizeOf();
	}

	// Token: 0x04009BD5 RID: 39893
	private IntPtr swigCPtr;
}
