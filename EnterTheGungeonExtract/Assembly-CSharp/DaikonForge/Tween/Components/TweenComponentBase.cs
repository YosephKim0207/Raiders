using System;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004DB RID: 1243
	[Serializable]
	public abstract class TweenComponentBase : TweenPlayableComponent
	{
		// Token: 0x06001D77 RID: 7543 RVA: 0x000888A4 File Offset: 0x00086AA4
		private static bool IsLoopCountVisible(object target)
		{
			return true;
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x000888A8 File Offset: 0x00086AA8
		// (set) Token: 0x06001D79 RID: 7545 RVA: 0x000888B0 File Offset: 0x00086AB0
		public float StartDelay
		{
			get
			{
				return this.startDelay;
			}
			set
			{
				this.startDelay = value;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x000888BC File Offset: 0x00086ABC
		// (set) Token: 0x06001D7B RID: 7547 RVA: 0x000888C4 File Offset: 0x00086AC4
		public bool AssignStartValueBeforeDelay
		{
			get
			{
				return this.assignStartValueBeforeDelay;
			}
			set
			{
				this.assignStartValueBeforeDelay = value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x000888D0 File Offset: 0x00086AD0
		// (set) Token: 0x06001D7D RID: 7549 RVA: 0x000888D8 File Offset: 0x00086AD8
		public TweenLoopType LoopType
		{
			get
			{
				return this.loopType;
			}
			set
			{
				this.loopType = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x000888F8 File Offset: 0x00086AF8
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x00088900 File Offset: 0x00086B00
		public int LoopCount
		{
			get
			{
				return this.loopCount;
			}
			set
			{
				this.loopCount = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x00088920 File Offset: 0x00086B20
		public bool IsPlaying
		{
			get
			{
				return base.enabled && (this.State == TweenState.Started || this.State == TweenState.Playing);
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x00088948 File Offset: 0x00086B48
		public bool IsPaused
		{
			get
			{
				return this.State == TweenState.Paused;
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x00088954 File Offset: 0x00086B54
		public override void Start()
		{
			base.Start();
			if (this.autoRun && !this.wasAutoStarted)
			{
				this.wasAutoStarted = true;
				this.Play();
			}
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x00088980 File Offset: 0x00086B80
		public override void OnEnable()
		{
			base.OnEnable();
			if (this.autoRun && !this.wasAutoStarted)
			{
				this.wasAutoStarted = true;
				this.Play();
			}
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000889AC File Offset: 0x00086BAC
		public override void OnDisable()
		{
			base.OnDisable();
			if (this.IsPlaying)
			{
				this.Stop();
			}
			this.wasAutoStarted = false;
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000889CC File Offset: 0x00086BCC
		public override string ToString()
		{
			return string.Format("{0}.{1} '{2}'", base.gameObject.name, base.GetType().Name, this.tweenName);
		}

		// Token: 0x0400167B RID: 5755
		[Inspector("General", Order = -1, Label = "Name", Tooltip = "For your convenience, you may specify a name for this Tween")]
		[SerializeField]
		protected string tweenName;

		// Token: 0x0400167C RID: 5756
		[SerializeField]
		[Inspector("Animation", Order = 0, Label = "Delay", Tooltip = "The amount of time in seconds to delay before starting the animation")]
		protected float startDelay;

		// Token: 0x0400167D RID: 5757
		[Inspector("Animation", Order = 1, Label = "Assign Start First", Tooltip = "If set, the StartValue will be assigned to the target before the delay (if any) is performed")]
		[SerializeField]
		protected bool assignStartValueBeforeDelay = true;

		// Token: 0x0400167E RID: 5758
		[SerializeField]
		[Inspector("Looping", Order = 1, Label = "Type", Tooltip = "Specify whether the animation will loop at the end")]
		protected TweenLoopType loopType;

		// Token: 0x0400167F RID: 5759
		[Inspector("Looping", Order = 1, Label = "Count", Tooltip = "If set to 0, the animation will loop forever")]
		[SerializeField]
		protected int loopCount;

		// Token: 0x04001680 RID: 5760
		protected bool wasAutoStarted;
	}
}
