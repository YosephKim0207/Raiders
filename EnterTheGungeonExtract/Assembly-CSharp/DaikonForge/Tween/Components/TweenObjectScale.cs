using System;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E8 RID: 1256
	[AddComponentMenu("Daikon Forge/Tween/Object Scale")]
	public class TweenObjectScale : TweenComponent<Vector3>
	{
		// Token: 0x06001E0C RID: 7692 RVA: 0x00089C3C File Offset: 0x00087E3C
		protected override void configureTween()
		{
			if (this.tween == null)
			{
				this.easingFunc = TweenEasingFunctions.GetFunction(this.easingType);
				this.tween = (Tween<Vector3>)base.transform.TweenScale().SetEasing(new TweenEasingCallback(this.modifyEasing)).OnStarted(delegate(TweenBase x)
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
			Vector3 localScale = base.transform.localScale;
			Vector3 vector = this.startValue;
			if (this.startValueType == TweenStartValueType.SyncOnRun)
			{
				vector = localScale;
			}
			Vector3 vector2 = this.endValue;
			if (this.endValueType == TweenEndValueType.SyncOnRun)
			{
				vector2 = localScale;
			}
			else if (this.endValueType == TweenEndValueType.Relative)
			{
				vector2 += vector;
			}
			this.tween.SetStartValue(vector).SetEndValue(vector2).SetDelay(this.startDelay, this.assignStartValueBeforeDelay)
				.SetDuration(this.duration)
				.SetLoopType(base.LoopType)
				.SetLoopCount(this.loopCount)
				.SetPlayDirection(this.playDirection);
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x00089D94 File Offset: 0x00087F94
		private float modifyEasing(float time)
		{
			if (this.animCurve != null)
			{
				time = this.animCurve.Evaluate(time);
			}
			return this.easingFunc(time);
		}

		// Token: 0x040016A2 RID: 5794
		private TweenEasingCallback easingFunc;
	}
}
