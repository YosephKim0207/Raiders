using System;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
[Serializable]
public abstract class dfTweenComponentBase : dfTweenPlayableBase
{
	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x0008702C File Offset: 0x0008522C
	// (set) Token: 0x06001CB5 RID: 7349 RVA: 0x0008704C File Offset: 0x0008524C
	public override string TweenName
	{
		get
		{
			if (this.tweenName == null)
			{
				this.tweenName = base.ToString();
			}
			return this.tweenName;
		}
		set
		{
			this.tweenName = value;
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x00087058 File Offset: 0x00085258
	// (set) Token: 0x06001CB7 RID: 7351 RVA: 0x00087060 File Offset: 0x00085260
	public dfComponentMemberInfo Target
	{
		get
		{
			return this.target;
		}
		set
		{
			this.target = value;
		}
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x0008706C File Offset: 0x0008526C
	// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x00087074 File Offset: 0x00085274
	public AnimationCurve AnimationCurve
	{
		get
		{
			return this.animCurve;
		}
		set
		{
			this.animCurve = value;
		}
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06001CBA RID: 7354 RVA: 0x00087080 File Offset: 0x00085280
	// (set) Token: 0x06001CBB RID: 7355 RVA: 0x00087088 File Offset: 0x00085288
	public float Length
	{
		get
		{
			return this.length;
		}
		set
		{
			this.length = Mathf.Max(0f, value);
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06001CBC RID: 7356 RVA: 0x0008709C File Offset: 0x0008529C
	// (set) Token: 0x06001CBD RID: 7357 RVA: 0x000870A4 File Offset: 0x000852A4
	public float StartDelay
	{
		get
		{
			return this.delayBeforeStarting;
		}
		set
		{
			this.delayBeforeStarting = value;
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06001CBE RID: 7358 RVA: 0x000870B0 File Offset: 0x000852B0
	// (set) Token: 0x06001CBF RID: 7359 RVA: 0x000870B8 File Offset: 0x000852B8
	public dfEasingType Function
	{
		get
		{
			return this.easingType;
		}
		set
		{
			this.easingType = value;
			if (this.state != dfTweenState.Stopped)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x000870D8 File Offset: 0x000852D8
	// (set) Token: 0x06001CC1 RID: 7361 RVA: 0x000870E0 File Offset: 0x000852E0
	public dfTweenLoopType LoopType
	{
		get
		{
			return this.loopType;
		}
		set
		{
			this.loopType = value;
			if (this.state != dfTweenState.Stopped)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x00087100 File Offset: 0x00085300
	// (set) Token: 0x06001CC3 RID: 7363 RVA: 0x00087108 File Offset: 0x00085308
	public bool SyncStartValueWhenRun
	{
		get
		{
			return this.syncStartWhenRun;
		}
		set
		{
			this.syncStartWhenRun = value;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x00087114 File Offset: 0x00085314
	// (set) Token: 0x06001CC5 RID: 7365 RVA: 0x0008711C File Offset: 0x0008531C
	public bool StartValueIsOffset
	{
		get
		{
			return this.startValueIsOffset;
		}
		set
		{
			this.startValueIsOffset = value;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x00087128 File Offset: 0x00085328
	// (set) Token: 0x06001CC7 RID: 7367 RVA: 0x00087130 File Offset: 0x00085330
	public bool SyncEndValueWhenRun
	{
		get
		{
			return this.syncEndWhenRun;
		}
		set
		{
			this.syncEndWhenRun = value;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x0008713C File Offset: 0x0008533C
	// (set) Token: 0x06001CC9 RID: 7369 RVA: 0x00087144 File Offset: 0x00085344
	public bool EndValueIsOffset
	{
		get
		{
			return this.endValueIsOffset;
		}
		set
		{
			this.endValueIsOffset = value;
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06001CCA RID: 7370 RVA: 0x00087150 File Offset: 0x00085350
	// (set) Token: 0x06001CCB RID: 7371 RVA: 0x00087158 File Offset: 0x00085358
	public bool AutoRun
	{
		get
		{
			return this.autoRun;
		}
		set
		{
			this.autoRun = value;
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06001CCC RID: 7372 RVA: 0x00087164 File Offset: 0x00085364
	public override bool IsPlaying
	{
		get
		{
			return base.enabled && this.state != dfTweenState.Stopped;
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06001CCD RID: 7373 RVA: 0x00087180 File Offset: 0x00085380
	// (set) Token: 0x06001CCE RID: 7374 RVA: 0x0008718C File Offset: 0x0008538C
	public bool IsPaused
	{
		get
		{
			return this.state == dfTweenState.Paused;
		}
		set
		{
			bool flag = this.state == dfTweenState.Paused;
			if (value != flag && this.state != dfTweenState.Stopped)
			{
				this.state = ((!value) ? dfTweenState.Playing : dfTweenState.Paused);
				if (value)
				{
					this.onPaused();
				}
				else
				{
					this.onResumed();
				}
			}
		}
	}

	// Token: 0x06001CCF RID: 7375
	protected internal abstract void onPaused();

	// Token: 0x06001CD0 RID: 7376
	protected internal abstract void onResumed();

	// Token: 0x06001CD1 RID: 7377
	protected internal abstract void onStarted();

	// Token: 0x06001CD2 RID: 7378
	protected internal abstract void onStopped();

	// Token: 0x06001CD3 RID: 7379
	protected internal abstract void onReset();

	// Token: 0x06001CD4 RID: 7380
	protected internal abstract void onCompleted();

	// Token: 0x06001CD5 RID: 7381 RVA: 0x000871E0 File Offset: 0x000853E0
	public void Start()
	{
		if (this.autoRun && !this.wasAutoStarted)
		{
			this.wasAutoStarted = true;
			this.Play();
		}
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x00087208 File Offset: 0x00085408
	public void OnDisable()
	{
		this.Stop();
		this.wasAutoStarted = false;
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x00087218 File Offset: 0x00085418
	public override string ToString()
	{
		if (this.Target != null && this.Target.IsValid)
		{
			string name = this.target.Component.name;
			return string.Format("{0} ({1}.{2})", this.TweenName, name, this.target.MemberName);
		}
		return this.TweenName;
	}

	// Token: 0x0400162D RID: 5677
	[SerializeField]
	protected string tweenName = string.Empty;

	// Token: 0x0400162E RID: 5678
	[SerializeField]
	protected dfComponentMemberInfo target;

	// Token: 0x0400162F RID: 5679
	[SerializeField]
	protected dfEasingType easingType;

	// Token: 0x04001630 RID: 5680
	[SerializeField]
	protected AnimationCurve animCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04001631 RID: 5681
	[SerializeField]
	protected float length = 1f;

	// Token: 0x04001632 RID: 5682
	[SerializeField]
	protected bool syncStartWhenRun;

	// Token: 0x04001633 RID: 5683
	[SerializeField]
	protected bool startValueIsOffset;

	// Token: 0x04001634 RID: 5684
	[SerializeField]
	protected bool syncEndWhenRun;

	// Token: 0x04001635 RID: 5685
	[SerializeField]
	protected bool endValueIsOffset;

	// Token: 0x04001636 RID: 5686
	[SerializeField]
	protected dfTweenLoopType loopType;

	// Token: 0x04001637 RID: 5687
	[SerializeField]
	protected bool autoRun;

	// Token: 0x04001638 RID: 5688
	[SerializeField]
	protected bool skipToEndOnStop;

	// Token: 0x04001639 RID: 5689
	[SerializeField]
	protected float delayBeforeStarting;

	// Token: 0x0400163A RID: 5690
	protected dfTweenState state;

	// Token: 0x0400163B RID: 5691
	protected dfEasingFunctions.EasingFunction easingFunction;

	// Token: 0x0400163C RID: 5692
	protected dfObservableProperty boundProperty;

	// Token: 0x0400163D RID: 5693
	protected bool wasAutoStarted;
}
