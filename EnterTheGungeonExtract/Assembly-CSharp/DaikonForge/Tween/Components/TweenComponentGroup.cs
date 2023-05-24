using System;
using System.Collections.Generic;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E0 RID: 1248
	[InspectorGroupOrder(new string[] { "General", "Animation", "Looping", "Tweens" })]
	[AddComponentMenu("Daikon Forge/Tween/Group")]
	public class TweenComponentGroup : TweenComponentBase
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x00088CCC File Offset: 0x00086ECC
		public override TweenBase BaseTween
		{
			get
			{
				this.configureTween();
				return this.group;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x00088CDC File Offset: 0x00086EDC
		public List<TweenPlayableComponent> Tweens
		{
			get
			{
				return this.tweens;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x00088CE4 File Offset: 0x00086EE4
		// (set) Token: 0x06001DAA RID: 7594 RVA: 0x00088CEC File Offset: 0x00086EEC
		public TweenGroupMode GroupMode
		{
			get
			{
				return this.groupMode;
			}
			set
			{
				if (value != this.groupMode)
				{
					this.groupMode = value;
					this.Stop();
				}
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x00088D08 File Offset: 0x00086F08
		public override TweenState State
		{
			get
			{
				if (this.group == null)
				{
					return TweenState.Stopped;
				}
				return this.group.State;
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x00088D24 File Offset: 0x00086F24
		public virtual void OnApplicationQuit()
		{
			this.cleanup();
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00088D2C File Offset: 0x00086F2C
		public override void OnDisable()
		{
			base.OnDisable();
			this.cleanup();
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x00088D3C File Offset: 0x00086F3C
		public override void Play()
		{
			if (this.State != TweenState.Stopped)
			{
				this.Stop();
			}
			this.configureTween();
			this.validateTweenConfiguration();
			this.group.Play();
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x00088D68 File Offset: 0x00086F68
		public override void Stop()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.group.Stop();
			}
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x00088D88 File Offset: 0x00086F88
		public override void Pause()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.group.Pause();
			}
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x00088DA8 File Offset: 0x00086FA8
		public override void Resume()
		{
			if (base.IsPaused)
			{
				this.validateTweenConfiguration();
				this.group.Resume();
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00088DC8 File Offset: 0x00086FC8
		public override void Rewind()
		{
			this.validateTweenConfiguration();
			this.group.Rewind();
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x00088DDC File Offset: 0x00086FDC
		public override void FastForward()
		{
			this.validateTweenConfiguration();
			this.group.FastForward();
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00088DF0 File Offset: 0x00086FF0
		protected void cleanup()
		{
			if (this.group != null)
			{
				this.group.Stop();
				this.group.Release();
				this.group = null;
			}
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00088E1C File Offset: 0x0008701C
		protected void validateTweenConfiguration()
		{
			this.loopCount = Mathf.Max(0, this.loopCount);
			if (this.loopType != TweenLoopType.None && this.loopType != TweenLoopType.Loop)
			{
				throw new ArgumentException("LoopType may only be one of the following values: TweenLoopType.None, TweenLoopType.Loop");
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x00088E54 File Offset: 0x00087054
		protected void configureTween()
		{
			if (this.group == null)
			{
				this.group = (TweenGroup)new TweenGroup().OnStarted(delegate(TweenBase x)
				{
					this.onStarted();
				}).OnStopped(delegate(TweenBase x)
				{
					this.onStopped();
				}).OnPaused(delegate(TweenBase x)
				{
					this.onPaused();
				})
					.OnResumed(delegate(TweenBase x)
					{
						this.onResumed();
					})
					.OnLoopCompleted(delegate(TweenBase x)
					{
						this.onLoopCompleted();
					})
					.OnCompleted(delegate(TweenBase x)
					{
						this.onCompleted();
					});
			}
			this.group.ClearTweens().SetMode(this.groupMode).SetDelay(this.startDelay)
				.SetLoopType(this.loopType)
				.SetLoopCount(this.loopCount);
			for (int i = 0; i < this.tweens.Count; i++)
			{
				TweenPlayableComponent tweenPlayableComponent = this.tweens[i];
				if (tweenPlayableComponent != null)
				{
					tweenPlayableComponent.AutoRun = false;
					TweenBase baseTween = tweenPlayableComponent.BaseTween;
					if (baseTween == null)
					{
						Debug.LogError("Base tween not set", tweenPlayableComponent);
					}
					else
					{
						this.group.AppendTween(new TweenBase[] { baseTween });
					}
				}
			}
		}

		// Token: 0x04001691 RID: 5777
		[SerializeField]
		[Inspector("General", 1, Label = "Mode")]
		protected TweenGroupMode groupMode;

		// Token: 0x04001692 RID: 5778
		[Inspector("Tweens", 0, Label = "Tweens")]
		[SerializeField]
		protected List<TweenPlayableComponent> tweens = new List<TweenPlayableComponent>();

		// Token: 0x04001693 RID: 5779
		protected TweenGroup group;
	}
}
