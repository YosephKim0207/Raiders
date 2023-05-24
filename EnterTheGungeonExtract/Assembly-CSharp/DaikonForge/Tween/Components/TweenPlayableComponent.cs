using System;
using System.Collections;
using System.Diagnostics;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004D9 RID: 1241
	public abstract class TweenPlayableComponent : MonoBehaviour
	{
		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06001D49 RID: 7497 RVA: 0x0008842C File Offset: 0x0008662C
		// (remove) Token: 0x06001D4A RID: 7498 RVA: 0x00088464 File Offset: 0x00086664
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TweenComponentNotification TweenStarted;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06001D4B RID: 7499 RVA: 0x0008849C File Offset: 0x0008669C
		// (remove) Token: 0x06001D4C RID: 7500 RVA: 0x000884D4 File Offset: 0x000866D4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TweenComponentNotification TweenStopped;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06001D4D RID: 7501 RVA: 0x0008850C File Offset: 0x0008670C
		// (remove) Token: 0x06001D4E RID: 7502 RVA: 0x00088544 File Offset: 0x00086744
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TweenComponentNotification TweenPaused;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06001D4F RID: 7503 RVA: 0x0008857C File Offset: 0x0008677C
		// (remove) Token: 0x06001D50 RID: 7504 RVA: 0x000885B4 File Offset: 0x000867B4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TweenComponentNotification TweenResumed;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06001D51 RID: 7505 RVA: 0x000885EC File Offset: 0x000867EC
		// (remove) Token: 0x06001D52 RID: 7506 RVA: 0x00088624 File Offset: 0x00086824
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TweenComponentNotification TweenLoopCompleted;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06001D53 RID: 7507 RVA: 0x0008865C File Offset: 0x0008685C
		// (remove) Token: 0x06001D54 RID: 7508 RVA: 0x00088694 File Offset: 0x00086894
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TweenComponentNotification TweenCompleted;

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x000886CC File Offset: 0x000868CC
		// (set) Token: 0x06001D56 RID: 7510 RVA: 0x000886D4 File Offset: 0x000868D4
		public virtual string TweenName { get; set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001D57 RID: 7511
		public abstract TweenState State { get; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001D58 RID: 7512
		public abstract TweenBase BaseTween { get; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x000886E0 File Offset: 0x000868E0
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x000886E8 File Offset: 0x000868E8
		[Inspector("General", 1, BackingField = "autoRun", Tooltip = "If set to TRUE, this Tween will automatically play when the scene starts")]
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

		// Token: 0x06001D5B RID: 7515
		public abstract void Play();

		// Token: 0x06001D5C RID: 7516
		public abstract void Stop();

		// Token: 0x06001D5D RID: 7517
		public abstract void Rewind();

		// Token: 0x06001D5E RID: 7518
		public abstract void FastForward();

		// Token: 0x06001D5F RID: 7519
		public abstract void Pause();

		// Token: 0x06001D60 RID: 7520
		public abstract void Resume();

		// Token: 0x06001D61 RID: 7521 RVA: 0x000886F4 File Offset: 0x000868F4
		public virtual void Awake()
		{
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000886F8 File Offset: 0x000868F8
		public virtual void Start()
		{
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000886FC File Offset: 0x000868FC
		public virtual void OnEnable()
		{
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x00088700 File Offset: 0x00086900
		public virtual void OnDisable()
		{
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x00088704 File Offset: 0x00086904
		public virtual void OnDestroy()
		{
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00088708 File Offset: 0x00086908
		public virtual void Enable()
		{
			base.enabled = true;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x00088714 File Offset: 0x00086914
		public virtual void Disable()
		{
			base.enabled = false;
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x00088720 File Offset: 0x00086920
		public virtual IEnumerator WaitForCompletion()
		{
			while (this.State != TweenState.Stopped)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0008873C File Offset: 0x0008693C
		protected virtual void onPaused()
		{
			if (this.TweenPaused != null)
			{
				this.TweenPaused(this);
			}
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x00088758 File Offset: 0x00086958
		protected virtual void onResumed()
		{
			if (this.TweenResumed != null)
			{
				this.TweenResumed(this);
			}
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x00088774 File Offset: 0x00086974
		protected virtual void onStarted()
		{
			if (this.TweenStarted != null)
			{
				this.TweenStarted(this);
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x00088790 File Offset: 0x00086990
		protected virtual void onStopped()
		{
			if (this.TweenStopped != null)
			{
				this.TweenStopped(this);
			}
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000887AC File Offset: 0x000869AC
		protected virtual void onLoopCompleted()
		{
			if (this.TweenLoopCompleted != null)
			{
				this.TweenLoopCompleted(this);
			}
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000887C8 File Offset: 0x000869C8
		protected virtual void onCompleted()
		{
			if (this.TweenCompleted != null)
			{
				this.TweenCompleted(this);
			}
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000887E4 File Offset: 0x000869E4
		public override string ToString()
		{
			return this.TweenName + " - " + base.ToString();
		}

		// Token: 0x04001675 RID: 5749
		[SerializeField]
		protected bool autoRun;
	}
}
