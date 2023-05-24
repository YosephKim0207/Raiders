using System;

// Token: 0x0200187C RID: 6268
public class AkMIDIEvent : IDisposable
{
	// Token: 0x06009493 RID: 38035 RVA: 0x003E509C File Offset: 0x003E329C
	internal AkMIDIEvent(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009494 RID: 38036 RVA: 0x003E50B4 File Offset: 0x003E32B4
	public AkMIDIEvent()
		: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent(), true)
	{
	}

	// Token: 0x06009495 RID: 38037 RVA: 0x003E50C4 File Offset: 0x003E32C4
	internal static IntPtr getCPtr(AkMIDIEvent obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	// Token: 0x06009496 RID: 38038 RVA: 0x003E50DC File Offset: 0x003E32DC
	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	// Token: 0x06009497 RID: 38039 RVA: 0x003E50EC File Offset: 0x003E32EC
	~AkMIDIEvent()
	{
		this.Dispose();
	}

	// Token: 0x06009498 RID: 38040 RVA: 0x003E511C File Offset: 0x003E331C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17001640 RID: 5696
	// (get) Token: 0x0600949A RID: 38042 RVA: 0x003E51A0 File Offset: 0x003E33A0
	// (set) Token: 0x06009499 RID: 38041 RVA: 0x003E5190 File Offset: 0x003E3390
	public byte byChan
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChan_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChan_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001641 RID: 5697
	// (get) Token: 0x0600949C RID: 38044 RVA: 0x003E51C4 File Offset: 0x003E33C4
	// (set) Token: 0x0600949B RID: 38043 RVA: 0x003E51B0 File Offset: 0x003E33B0
	public AkMIDIEvent.tGen Gen
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Gen_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tGen(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Gen_set(this.swigCPtr, AkMIDIEvent.tGen.getCPtr(value));
		}
	}

	// Token: 0x17001642 RID: 5698
	// (get) Token: 0x0600949E RID: 38046 RVA: 0x003E5210 File Offset: 0x003E3410
	// (set) Token: 0x0600949D RID: 38045 RVA: 0x003E51FC File Offset: 0x003E33FC
	public AkMIDIEvent.tCc Cc
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Cc_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tCc(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Cc_set(this.swigCPtr, AkMIDIEvent.tCc.getCPtr(value));
		}
	}

	// Token: 0x17001643 RID: 5699
	// (get) Token: 0x060094A0 RID: 38048 RVA: 0x003E525C File Offset: 0x003E345C
	// (set) Token: 0x0600949F RID: 38047 RVA: 0x003E5248 File Offset: 0x003E3448
	public AkMIDIEvent.tNoteOnOff NoteOnOff
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteOnOff_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tNoteOnOff(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteOnOff_set(this.swigCPtr, AkMIDIEvent.tNoteOnOff.getCPtr(value));
		}
	}

	// Token: 0x17001644 RID: 5700
	// (get) Token: 0x060094A2 RID: 38050 RVA: 0x003E52A8 File Offset: 0x003E34A8
	// (set) Token: 0x060094A1 RID: 38049 RVA: 0x003E5294 File Offset: 0x003E3494
	public AkMIDIEvent.tPitchBend PitchBend
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_PitchBend_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tPitchBend(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_PitchBend_set(this.swigCPtr, AkMIDIEvent.tPitchBend.getCPtr(value));
		}
	}

	// Token: 0x17001645 RID: 5701
	// (get) Token: 0x060094A4 RID: 38052 RVA: 0x003E52F4 File Offset: 0x003E34F4
	// (set) Token: 0x060094A3 RID: 38051 RVA: 0x003E52E0 File Offset: 0x003E34E0
	public AkMIDIEvent.tNoteAftertouch NoteAftertouch
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteAftertouch_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tNoteAftertouch(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteAftertouch_set(this.swigCPtr, AkMIDIEvent.tNoteAftertouch.getCPtr(value));
		}
	}

	// Token: 0x17001646 RID: 5702
	// (get) Token: 0x060094A6 RID: 38054 RVA: 0x003E5340 File Offset: 0x003E3540
	// (set) Token: 0x060094A5 RID: 38053 RVA: 0x003E532C File Offset: 0x003E352C
	public AkMIDIEvent.tChanAftertouch ChanAftertouch
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ChanAftertouch_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tChanAftertouch(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ChanAftertouch_set(this.swigCPtr, AkMIDIEvent.tChanAftertouch.getCPtr(value));
		}
	}

	// Token: 0x17001647 RID: 5703
	// (get) Token: 0x060094A8 RID: 38056 RVA: 0x003E538C File Offset: 0x003E358C
	// (set) Token: 0x060094A7 RID: 38055 RVA: 0x003E5378 File Offset: 0x003E3578
	public AkMIDIEvent.tProgramChange ProgramChange
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ProgramChange_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tProgramChange(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ProgramChange_set(this.swigCPtr, AkMIDIEvent.tProgramChange.getCPtr(value));
		}
	}

	// Token: 0x17001648 RID: 5704
	// (get) Token: 0x060094AA RID: 38058 RVA: 0x003E53D4 File Offset: 0x003E35D4
	// (set) Token: 0x060094A9 RID: 38057 RVA: 0x003E53C4 File Offset: 0x003E35C4
	public AkMIDIEventTypes byType
	{
		get
		{
			return (AkMIDIEventTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byType_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byType_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x17001649 RID: 5705
	// (get) Token: 0x060094AC RID: 38060 RVA: 0x003E53F4 File Offset: 0x003E35F4
	// (set) Token: 0x060094AB RID: 38059 RVA: 0x003E53E4 File Offset: 0x003E35E4
	public byte byOnOffNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byOnOffNote_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byOnOffNote_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700164A RID: 5706
	// (get) Token: 0x060094AE RID: 38062 RVA: 0x003E5414 File Offset: 0x003E3614
	// (set) Token: 0x060094AD RID: 38061 RVA: 0x003E5404 File Offset: 0x003E3604
	public byte byVelocity
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byVelocity_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byVelocity_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700164B RID: 5707
	// (get) Token: 0x060094B0 RID: 38064 RVA: 0x003E5434 File Offset: 0x003E3634
	// (set) Token: 0x060094AF RID: 38063 RVA: 0x003E5424 File Offset: 0x003E3624
	public AkMIDICcTypes byCc
	{
		get
		{
			return (AkMIDICcTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCc_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCc_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x1700164C RID: 5708
	// (get) Token: 0x060094B2 RID: 38066 RVA: 0x003E5454 File Offset: 0x003E3654
	// (set) Token: 0x060094B1 RID: 38065 RVA: 0x003E5444 File Offset: 0x003E3644
	public byte byCcValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCcValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCcValue_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700164D RID: 5709
	// (get) Token: 0x060094B4 RID: 38068 RVA: 0x003E5474 File Offset: 0x003E3674
	// (set) Token: 0x060094B3 RID: 38067 RVA: 0x003E5464 File Offset: 0x003E3664
	public byte byValueLsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueLsb_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueLsb_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700164E RID: 5710
	// (get) Token: 0x060094B6 RID: 38070 RVA: 0x003E5494 File Offset: 0x003E3694
	// (set) Token: 0x060094B5 RID: 38069 RVA: 0x003E5484 File Offset: 0x003E3684
	public byte byValueMsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueMsb_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueMsb_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700164F RID: 5711
	// (get) Token: 0x060094B8 RID: 38072 RVA: 0x003E54B4 File Offset: 0x003E36B4
	// (set) Token: 0x060094B7 RID: 38071 RVA: 0x003E54A4 File Offset: 0x003E36A4
	public byte byAftertouchNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byAftertouchNote_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byAftertouchNote_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001650 RID: 5712
	// (get) Token: 0x060094BA RID: 38074 RVA: 0x003E54D4 File Offset: 0x003E36D4
	// (set) Token: 0x060094B9 RID: 38073 RVA: 0x003E54C4 File Offset: 0x003E36C4
	public byte byNoteAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byNoteAftertouchValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byNoteAftertouchValue_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001651 RID: 5713
	// (get) Token: 0x060094BC RID: 38076 RVA: 0x003E54F4 File Offset: 0x003E36F4
	// (set) Token: 0x060094BB RID: 38075 RVA: 0x003E54E4 File Offset: 0x003E36E4
	public byte byChanAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChanAftertouchValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChanAftertouchValue_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17001652 RID: 5714
	// (get) Token: 0x060094BE RID: 38078 RVA: 0x003E5514 File Offset: 0x003E3714
	// (set) Token: 0x060094BD RID: 38077 RVA: 0x003E5504 File Offset: 0x003E3704
	public byte byProgramNum
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byProgramNum_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byProgramNum_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04009BB9 RID: 39865
	private IntPtr swigCPtr;

	// Token: 0x04009BBA RID: 39866
	protected bool swigCMemOwn;

	// Token: 0x0200187D RID: 6269
	public class tGen : IDisposable
	{
		// Token: 0x060094BF RID: 38079 RVA: 0x003E5524 File Offset: 0x003E3724
		internal tGen(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094C0 RID: 38080 RVA: 0x003E553C File Offset: 0x003E373C
		public tGen()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tGen(), true)
		{
		}

		// Token: 0x060094C1 RID: 38081 RVA: 0x003E554C File Offset: 0x003E374C
		internal static IntPtr getCPtr(AkMIDIEvent.tGen obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094C2 RID: 38082 RVA: 0x003E5564 File Offset: 0x003E3764
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094C3 RID: 38083 RVA: 0x003E5574 File Offset: 0x003E3774
		~tGen()
		{
			this.Dispose();
		}

		// Token: 0x060094C4 RID: 38084 RVA: 0x003E55A4 File Offset: 0x003E37A4
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tGen(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x060094C6 RID: 38086 RVA: 0x003E5628 File Offset: 0x003E3828
		// (set) Token: 0x060094C5 RID: 38085 RVA: 0x003E5618 File Offset: 0x003E3818
		public byte byParam1
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam1_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam1_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x060094C8 RID: 38088 RVA: 0x003E5648 File Offset: 0x003E3848
		// (set) Token: 0x060094C7 RID: 38087 RVA: 0x003E5638 File Offset: 0x003E3838
		public byte byParam2
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam2_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam2_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BBB RID: 39867
		private IntPtr swigCPtr;

		// Token: 0x04009BBC RID: 39868
		protected bool swigCMemOwn;
	}

	// Token: 0x0200187E RID: 6270
	public class tNoteOnOff : IDisposable
	{
		// Token: 0x060094C9 RID: 38089 RVA: 0x003E5658 File Offset: 0x003E3858
		internal tNoteOnOff(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094CA RID: 38090 RVA: 0x003E5670 File Offset: 0x003E3870
		public tNoteOnOff()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tNoteOnOff(), true)
		{
		}

		// Token: 0x060094CB RID: 38091 RVA: 0x003E5680 File Offset: 0x003E3880
		internal static IntPtr getCPtr(AkMIDIEvent.tNoteOnOff obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094CC RID: 38092 RVA: 0x003E5698 File Offset: 0x003E3898
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094CD RID: 38093 RVA: 0x003E56A8 File Offset: 0x003E38A8
		~tNoteOnOff()
		{
			this.Dispose();
		}

		// Token: 0x060094CE RID: 38094 RVA: 0x003E56D8 File Offset: 0x003E38D8
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tNoteOnOff(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x060094D0 RID: 38096 RVA: 0x003E575C File Offset: 0x003E395C
		// (set) Token: 0x060094CF RID: 38095 RVA: 0x003E574C File Offset: 0x003E394C
		public byte byNote
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byNote_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byNote_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x060094D2 RID: 38098 RVA: 0x003E577C File Offset: 0x003E397C
		// (set) Token: 0x060094D1 RID: 38097 RVA: 0x003E576C File Offset: 0x003E396C
		public byte byVelocity
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byVelocity_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byVelocity_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BBD RID: 39869
		private IntPtr swigCPtr;

		// Token: 0x04009BBE RID: 39870
		protected bool swigCMemOwn;
	}

	// Token: 0x0200187F RID: 6271
	public class tCc : IDisposable
	{
		// Token: 0x060094D3 RID: 38099 RVA: 0x003E578C File Offset: 0x003E398C
		internal tCc(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094D4 RID: 38100 RVA: 0x003E57A4 File Offset: 0x003E39A4
		public tCc()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tCc(), true)
		{
		}

		// Token: 0x060094D5 RID: 38101 RVA: 0x003E57B4 File Offset: 0x003E39B4
		internal static IntPtr getCPtr(AkMIDIEvent.tCc obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094D6 RID: 38102 RVA: 0x003E57CC File Offset: 0x003E39CC
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094D7 RID: 38103 RVA: 0x003E57DC File Offset: 0x003E39DC
		~tCc()
		{
			this.Dispose();
		}

		// Token: 0x060094D8 RID: 38104 RVA: 0x003E580C File Offset: 0x003E3A0C
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tCc(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x060094DA RID: 38106 RVA: 0x003E5890 File Offset: 0x003E3A90
		// (set) Token: 0x060094D9 RID: 38105 RVA: 0x003E5880 File Offset: 0x003E3A80
		public byte byCc
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byCc_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byCc_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x060094DC RID: 38108 RVA: 0x003E58B0 File Offset: 0x003E3AB0
		// (set) Token: 0x060094DB RID: 38107 RVA: 0x003E58A0 File Offset: 0x003E3AA0
		public byte byValue
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byValue_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byValue_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BBF RID: 39871
		private IntPtr swigCPtr;

		// Token: 0x04009BC0 RID: 39872
		protected bool swigCMemOwn;
	}

	// Token: 0x02001880 RID: 6272
	public class tPitchBend : IDisposable
	{
		// Token: 0x060094DD RID: 38109 RVA: 0x003E58C0 File Offset: 0x003E3AC0
		internal tPitchBend(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094DE RID: 38110 RVA: 0x003E58D8 File Offset: 0x003E3AD8
		public tPitchBend()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tPitchBend(), true)
		{
		}

		// Token: 0x060094DF RID: 38111 RVA: 0x003E58E8 File Offset: 0x003E3AE8
		internal static IntPtr getCPtr(AkMIDIEvent.tPitchBend obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094E0 RID: 38112 RVA: 0x003E5900 File Offset: 0x003E3B00
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094E1 RID: 38113 RVA: 0x003E5910 File Offset: 0x003E3B10
		~tPitchBend()
		{
			this.Dispose();
		}

		// Token: 0x060094E2 RID: 38114 RVA: 0x003E5940 File Offset: 0x003E3B40
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tPitchBend(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x060094E4 RID: 38116 RVA: 0x003E59C4 File Offset: 0x003E3BC4
		// (set) Token: 0x060094E3 RID: 38115 RVA: 0x003E59B4 File Offset: 0x003E3BB4
		public byte byValueLsb
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueLsb_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueLsb_set(this.swigCPtr, value);
			}
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x060094E6 RID: 38118 RVA: 0x003E59E4 File Offset: 0x003E3BE4
		// (set) Token: 0x060094E5 RID: 38117 RVA: 0x003E59D4 File Offset: 0x003E3BD4
		public byte byValueMsb
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueMsb_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueMsb_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BC1 RID: 39873
		private IntPtr swigCPtr;

		// Token: 0x04009BC2 RID: 39874
		protected bool swigCMemOwn;
	}

	// Token: 0x02001881 RID: 6273
	public class tNoteAftertouch : IDisposable
	{
		// Token: 0x060094E7 RID: 38119 RVA: 0x003E59F4 File Offset: 0x003E3BF4
		internal tNoteAftertouch(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094E8 RID: 38120 RVA: 0x003E5A0C File Offset: 0x003E3C0C
		public tNoteAftertouch()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tNoteAftertouch(), true)
		{
		}

		// Token: 0x060094E9 RID: 38121 RVA: 0x003E5A1C File Offset: 0x003E3C1C
		internal static IntPtr getCPtr(AkMIDIEvent.tNoteAftertouch obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094EA RID: 38122 RVA: 0x003E5A34 File Offset: 0x003E3C34
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094EB RID: 38123 RVA: 0x003E5A44 File Offset: 0x003E3C44
		~tNoteAftertouch()
		{
			this.Dispose();
		}

		// Token: 0x060094EC RID: 38124 RVA: 0x003E5A74 File Offset: 0x003E3C74
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tNoteAftertouch(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x060094EE RID: 38126 RVA: 0x003E5AF8 File Offset: 0x003E3CF8
		// (set) Token: 0x060094ED RID: 38125 RVA: 0x003E5AE8 File Offset: 0x003E3CE8
		public byte byNote
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byNote_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byNote_set(this.swigCPtr, value);
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x060094F0 RID: 38128 RVA: 0x003E5B18 File Offset: 0x003E3D18
		// (set) Token: 0x060094EF RID: 38127 RVA: 0x003E5B08 File Offset: 0x003E3D08
		public byte byValue
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byValue_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byValue_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BC3 RID: 39875
		private IntPtr swigCPtr;

		// Token: 0x04009BC4 RID: 39876
		protected bool swigCMemOwn;
	}

	// Token: 0x02001882 RID: 6274
	public class tChanAftertouch : IDisposable
	{
		// Token: 0x060094F1 RID: 38129 RVA: 0x003E5B28 File Offset: 0x003E3D28
		internal tChanAftertouch(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094F2 RID: 38130 RVA: 0x003E5B40 File Offset: 0x003E3D40
		public tChanAftertouch()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tChanAftertouch(), true)
		{
		}

		// Token: 0x060094F3 RID: 38131 RVA: 0x003E5B50 File Offset: 0x003E3D50
		internal static IntPtr getCPtr(AkMIDIEvent.tChanAftertouch obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094F4 RID: 38132 RVA: 0x003E5B68 File Offset: 0x003E3D68
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094F5 RID: 38133 RVA: 0x003E5B78 File Offset: 0x003E3D78
		~tChanAftertouch()
		{
			this.Dispose();
		}

		// Token: 0x060094F6 RID: 38134 RVA: 0x003E5BA8 File Offset: 0x003E3DA8
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tChanAftertouch(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x060094F8 RID: 38136 RVA: 0x003E5C2C File Offset: 0x003E3E2C
		// (set) Token: 0x060094F7 RID: 38135 RVA: 0x003E5C1C File Offset: 0x003E3E1C
		public byte byValue
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tChanAftertouch_byValue_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tChanAftertouch_byValue_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BC5 RID: 39877
		private IntPtr swigCPtr;

		// Token: 0x04009BC6 RID: 39878
		protected bool swigCMemOwn;
	}

	// Token: 0x02001883 RID: 6275
	public class tProgramChange : IDisposable
	{
		// Token: 0x060094F9 RID: 38137 RVA: 0x003E5C3C File Offset: 0x003E3E3C
		internal tProgramChange(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094FA RID: 38138 RVA: 0x003E5C54 File Offset: 0x003E3E54
		public tProgramChange()
			: this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tProgramChange(), true)
		{
		}

		// Token: 0x060094FB RID: 38139 RVA: 0x003E5C64 File Offset: 0x003E3E64
		internal static IntPtr getCPtr(AkMIDIEvent.tProgramChange obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		// Token: 0x060094FC RID: 38140 RVA: 0x003E5C7C File Offset: 0x003E3E7C
		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		// Token: 0x060094FD RID: 38141 RVA: 0x003E5C8C File Offset: 0x003E3E8C
		~tProgramChange()
		{
			this.Dispose();
		}

		// Token: 0x060094FE RID: 38142 RVA: 0x003E5CBC File Offset: 0x003E3EBC
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tProgramChange(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06009500 RID: 38144 RVA: 0x003E5D40 File Offset: 0x003E3F40
		// (set) Token: 0x060094FF RID: 38143 RVA: 0x003E5D30 File Offset: 0x003E3F30
		public byte byProgramNum
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tProgramChange_byProgramNum_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tProgramChange_byProgramNum_set(this.swigCPtr, value);
			}
		}

		// Token: 0x04009BC7 RID: 39879
		private IntPtr swigCPtr;

		// Token: 0x04009BC8 RID: 39880
		protected bool swigCMemOwn;
	}
}
