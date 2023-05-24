using System;
using System.Collections.Generic;
using DaikonForge.Editor;
using DaikonForge.Tween.Interpolation;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004EA RID: 1258
	[InspectorGroupOrder(new string[] { "General", "Animation", "Looping", "Property", "Values" })]
	public class TweenPropertyComponent<T> : TweenComponent<T>, ITweenPropertyBase
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x00089DF4 File Offset: 0x00087FF4
		// (set) Token: 0x06001E1C RID: 7708 RVA: 0x00089DFC File Offset: 0x00087FFC
		public GameObject Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value != this.target)
				{
					this.target = value;
					if (this.State != TweenState.Stopped)
					{
						this.Stop();
						this.Play();
					}
				}
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001E1D RID: 7709 RVA: 0x00089E30 File Offset: 0x00088030
		// (set) Token: 0x06001E1E RID: 7710 RVA: 0x00089E38 File Offset: 0x00088038
		public string ComponentType
		{
			get
			{
				return this.componentType;
			}
			set
			{
				if (value != this.componentType)
				{
					this.componentType = value;
					if (this.State != TweenState.Stopped)
					{
						this.Stop();
						this.Play();
					}
				}
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x00089E6C File Offset: 0x0008806C
		// (set) Token: 0x06001E20 RID: 7712 RVA: 0x00089E74 File Offset: 0x00088074
		public string MemberName
		{
			get
			{
				return this.memberName;
			}
			set
			{
				if (value != this.memberName)
				{
					this.memberName = value;
					if (this.State != TweenState.Stopped)
					{
						this.Stop();
						this.Play();
					}
				}
			}
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x00089EA8 File Offset: 0x000880A8
		public override void OnEnable()
		{
			if (this.target == null)
			{
				this.target = base.gameObject;
			}
			base.OnEnable();
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00089ED0 File Offset: 0x000880D0
		protected override void validateTweenConfiguration()
		{
			base.validateTweenConfiguration();
			if (this.target == null || string.IsNullOrEmpty(this.componentType) || string.IsNullOrEmpty(this.memberName))
			{
				return;
			}
			Component component = this.target.GetComponent(this.componentType);
			if (component == null)
			{
				throw new NullReferenceException(string.Format("Object {0} does not contain a {1} component", this.target.name, this.componentType));
			}
			if (Interpolators.Get<T>() == null)
			{
				throw new KeyNotFoundException(string.Format("There is no default interpolator defined for type '{0}'", typeof(T).Name));
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x00089F80 File Offset: 0x00088180
		protected override void configureTween()
		{
			if (this.tween == null)
			{
				if (this.target == null || string.IsNullOrEmpty(this.componentType) || string.IsNullOrEmpty(this.memberName))
				{
					return;
				}
				this.component = this.target.GetComponent(this.componentType);
				if (this.component == null)
				{
					return;
				}
				this.easingFunc = TweenEasingFunctions.GetFunction(this.easingType);
				this.tween = (Tween<T>)this.component.TweenProperty(this.memberName).SetEasing(new TweenEasingCallback(this.modifyEasing)).OnStarted(delegate(TweenBase x)
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
			T currentValue = TweenNamedProperty<T>.GetCurrentValue(this.component, this.memberName);
			Interpolator<T> interpolator = Interpolators.Get<T>();
			T t = this.startValue;
			if (this.startValueType == TweenStartValueType.SyncOnRun)
			{
				t = currentValue;
			}
			T t2 = this.endValue;
			if (this.endValueType == TweenEndValueType.SyncOnRun)
			{
				t2 = currentValue;
			}
			else if (this.endValueType == TweenEndValueType.Relative)
			{
				t2 = interpolator.Add(t2, t);
			}
			this.tween.SetStartValue(t).SetEndValue(t2).SetDelay(this.startDelay, this.assignStartValueBeforeDelay)
				.SetDuration(this.duration)
				.SetLoopType(base.LoopType)
				.SetLoopCount(this.loopCount)
				.SetPlayDirection(this.playDirection);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0008A144 File Offset: 0x00088344
		private float modifyEasing(float time)
		{
			if (this.animCurve != null)
			{
				time = this.animCurve.Evaluate(time);
			}
			return this.easingFunc(time);
		}

		// Token: 0x040016A3 RID: 5795
		[Inspector("Property", Label = "Target", Order = 0)]
		[SerializeField]
		protected GameObject target;

		// Token: 0x040016A4 RID: 5796
		[SerializeField]
		protected string componentType;

		// Token: 0x040016A5 RID: 5797
		[Inspector("Property", Label = "Field", Order = 1)]
		[SerializeField]
		protected string memberName;

		// Token: 0x040016A6 RID: 5798
		private Component component;

		// Token: 0x040016A7 RID: 5799
		private TweenEasingCallback easingFunc;
	}
}
