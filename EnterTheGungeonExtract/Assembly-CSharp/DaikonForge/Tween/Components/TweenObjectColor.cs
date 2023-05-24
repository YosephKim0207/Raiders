using System;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E4 RID: 1252
	[AddComponentMenu("Daikon Forge/Tween/Object Color")]
	public class TweenObjectColor : TweenComponent<Color>
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x00089284 File Offset: 0x00087484
		// (set) Token: 0x06001DCD RID: 7629 RVA: 0x0008928C File Offset: 0x0008748C
		public Component Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
				this.Stop();
			}
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0008929C File Offset: 0x0008749C
		protected override void validateTweenConfiguration()
		{
			if (this.target == null)
			{
				throw new InvalidOperationException("The Target cannot be NULL");
			}
			base.validateTweenConfiguration();
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x000892C0 File Offset: 0x000874C0
		protected override void configureTween()
		{
			if (this.target == null)
			{
				this.target = base.gameObject.GetComponent<Renderer>();
				if (this.target == null)
				{
					if (this.tween != null)
					{
						this.tween.Stop();
						this.tween.Release();
						this.tween = null;
					}
					return;
				}
			}
			if (this.tween == null)
			{
				this.easingFunc = TweenEasingFunctions.GetFunction(this.easingType);
				this.tween = (Tween<Color>)this.Target.TweenColor().SetEasing(new TweenEasingCallback(this.modifyEasing)).OnStarted(delegate(TweenBase x)
				{
					this.onStarted();
				})
					.OnStopped(delegate(TweenBase x)
					{
						this.onStopped();
					})
					.OnPaused(delegate(TweenBase x)
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
			Color currentValue = this.tween.CurrentValue;
			Color color = this.startValue;
			if (this.startValueType == TweenStartValueType.SyncOnRun)
			{
				color = currentValue;
			}
			Color color2 = this.endValue;
			if (this.endValueType == TweenEndValueType.SyncOnRun)
			{
				color2 = currentValue;
			}
			else if (this.endValueType == TweenEndValueType.Relative)
			{
				color2 += color;
			}
			this.tween.SetStartValue(color).SetEndValue(color2).SetDelay(this.startDelay, this.assignStartValueBeforeDelay)
				.SetDuration(this.duration)
				.SetLoopType(base.LoopType)
				.SetLoopCount(this.loopCount)
				.SetPlayDirection(this.playDirection);
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00089474 File Offset: 0x00087674
		private float modifyEasing(float time)
		{
			if (this.animCurve != null)
			{
				time = this.animCurve.Evaluate(time);
			}
			return this.easingFunc(time);
		}

		// Token: 0x04001696 RID: 5782
		[SerializeField]
		protected Component target;

		// Token: 0x04001697 RID: 5783
		private TweenEasingCallback easingFunc;
	}
}
