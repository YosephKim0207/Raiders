using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x020004CC RID: 1228
[AddComponentMenu("Daikon Forge/Tweens/Group")]
[Serializable]
public class dfTweenGroup : dfTweenPlayableBase
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06001CEC RID: 7404 RVA: 0x00087630 File Offset: 0x00085830
	// (remove) Token: 0x06001CED RID: 7405 RVA: 0x00087668 File Offset: 0x00085868
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenStarted;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06001CEE RID: 7406 RVA: 0x000876A0 File Offset: 0x000858A0
	// (remove) Token: 0x06001CEF RID: 7407 RVA: 0x000876D8 File Offset: 0x000858D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenStopped;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06001CF0 RID: 7408 RVA: 0x00087710 File Offset: 0x00085910
	// (remove) Token: 0x06001CF1 RID: 7409 RVA: 0x00087748 File Offset: 0x00085948
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenReset;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06001CF2 RID: 7410 RVA: 0x00087780 File Offset: 0x00085980
	// (remove) Token: 0x06001CF3 RID: 7411 RVA: 0x000877B8 File Offset: 0x000859B8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenCompleted;

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000877F0 File Offset: 0x000859F0
	// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x000877F8 File Offset: 0x000859F8
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

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x00087804 File Offset: 0x00085A04
	// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x0008780C File Offset: 0x00085A0C
	public bool AutoStart
	{
		get
		{
			return this.autoStart;
		}
		set
		{
			this.autoStart = value;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x00087818 File Offset: 0x00085A18
	// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x00087820 File Offset: 0x00085A20
	public override string TweenName
	{
		get
		{
			return this.groupName;
		}
		set
		{
			this.groupName = value;
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0008782C File Offset: 0x00085A2C
	public override bool IsPlaying
	{
		get
		{
			for (int i = 0; i < this.Tweens.Count; i++)
			{
				if (!(this.Tweens[i] == null) && this.Tweens[i].enabled)
				{
					if (this.Tweens[i].IsPlaying)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000878A0 File Offset: 0x00085AA0
	public void Start()
	{
		if (this.AutoStart && !this.IsPlaying)
		{
			this.Play();
		}
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x000878C0 File Offset: 0x00085AC0
	public void EnableTween(string TweenName)
	{
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (!(this.Tweens[i] == null))
			{
				if (this.Tweens[i].TweenName == TweenName)
				{
					this.Tweens[i].enabled = true;
					break;
				}
			}
		}
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x00087938 File Offset: 0x00085B38
	public void DisableTween(string TweenName)
	{
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (!(this.Tweens[i] == null))
			{
				if (this.Tweens[i].name == TweenName)
				{
					this.Tweens[i].enabled = false;
					break;
				}
			}
		}
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000879B0 File Offset: 0x00085BB0
	public override void Play()
	{
		if (this.IsPlaying)
		{
			this.Stop();
		}
		this.onStarted();
		if (this.Mode == dfTweenGroup.TweenGroupMode.Concurrent)
		{
			base.StartCoroutine(this.runConcurrent());
		}
		else
		{
			base.StartCoroutine(this.runSequence());
		}
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x00087A00 File Offset: 0x00085C00
	public override void Stop()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		base.StopAllCoroutines();
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (!(this.Tweens[i] == null))
			{
				this.Tweens[i].Stop();
			}
		}
		this.onStopped();
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x00087A70 File Offset: 0x00085C70
	public override void Reset()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		base.StopAllCoroutines();
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (!(this.Tweens[i] == null))
			{
				this.Tweens[i].Reset();
			}
		}
		this.onReset();
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x00087AE0 File Offset: 0x00085CE0
	[HideInInspector]
	private IEnumerator runSequence()
	{
		if (this.delayBeforeStarting > 0f)
		{
			float timeout = Time.realtimeSinceStartup + this.delayBeforeStarting;
			while (Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}
		}
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (!(this.Tweens[i] == null) && this.Tweens[i].enabled)
			{
				dfTweenPlayableBase tween = this.Tweens[i];
				tween.Play();
				while (tween.IsPlaying)
				{
					yield return null;
				}
			}
		}
		this.onCompleted();
		yield break;
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x00087AFC File Offset: 0x00085CFC
	[HideInInspector]
	private IEnumerator runConcurrent()
	{
		if (this.delayBeforeStarting > 0f)
		{
			float timeout = Time.realtimeSinceStartup + this.delayBeforeStarting;
			while (Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}
		}
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (!(this.Tweens[i] == null) && this.Tweens[i].enabled)
			{
				this.Tweens[i].Play();
			}
		}
		do
		{
			yield return null;
		}
		while (this.Tweens.Any((dfTweenPlayableBase tween) => tween != null && tween.IsPlaying));
		this.onCompleted();
		yield break;
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x00087B18 File Offset: 0x00085D18
	protected internal void onStarted()
	{
		base.SendMessage("TweenStarted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStarted != null)
		{
			this.TweenStarted(this);
		}
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x00087B40 File Offset: 0x00085D40
	protected internal void onStopped()
	{
		base.SendMessage("TweenStopped", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStopped != null)
		{
			this.TweenStopped(this);
		}
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x00087B68 File Offset: 0x00085D68
	protected internal void onReset()
	{
		base.SendMessage("TweenReset", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenReset != null)
		{
			this.TweenReset(this);
		}
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x00087B90 File Offset: 0x00085D90
	protected internal void onCompleted()
	{
		base.SendMessage("TweenCompleted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenCompleted != null)
		{
			this.TweenCompleted(this);
		}
	}

	// Token: 0x04001651 RID: 5713
	[SerializeField]
	protected string groupName = string.Empty;

	// Token: 0x04001652 RID: 5714
	[SerializeField]
	protected bool autoStart;

	// Token: 0x04001653 RID: 5715
	[SerializeField]
	protected float delayBeforeStarting;

	// Token: 0x04001654 RID: 5716
	public List<dfTweenPlayableBase> Tweens = new List<dfTweenPlayableBase>();

	// Token: 0x04001655 RID: 5717
	public dfTweenGroup.TweenGroupMode Mode;

	// Token: 0x020004CD RID: 1229
	public enum TweenGroupMode
	{
		// Token: 0x04001657 RID: 5719
		Concurrent,
		// Token: 0x04001658 RID: 5720
		Sequence
	}
}
