using System;
using System.Collections.Generic;
using DaikonForge.Tween.Interpolation;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x020004FB RID: 1275
	public class Tween<T> : TweenBase, IPoolableObject
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x0008B62C File Offset: 0x0008982C
		// (set) Token: 0x06001E8F RID: 7823 RVA: 0x0008B634 File Offset: 0x00089834
		public T CurrentValue { get; protected set; }

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x0008B640 File Offset: 0x00089840
		// (set) Token: 0x06001E91 RID: 7825 RVA: 0x0008B648 File Offset: 0x00089848
		public bool EndIsOffset { get; protected set; }

		// Token: 0x06001E92 RID: 7826 RVA: 0x0008B654 File Offset: 0x00089854
		public static Tween<T> Obtain()
		{
			if (Tween<T>.pool.Count > 0)
			{
				Tween<T> tween = (Tween<T>)Tween<T>.pool[Tween<T>.pool.Count - 1];
				Tween<T>.pool.RemoveAt(Tween<T>.pool.Count - 1);
				return tween;
			}
			return new Tween<T>();
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0008B6AC File Offset: 0x000898AC
		public void Release()
		{
			this.Stop();
			this.Reset();
			Tween<T>.pool.Add(this);
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0008B6C8 File Offset: 0x000898C8
		public Tween<T> SetEndRelative(bool relative)
		{
			this.EndIsOffset = relative;
			return this;
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0008B6D4 File Offset: 0x000898D4
		public Tween<T> SetAutoCleanup(bool autoCleanup)
		{
			this.AutoCleanup = autoCleanup;
			return this;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0008B6E0 File Offset: 0x000898E0
		public Tween<T> SetPlayDirection(TweenDirection direction)
		{
			this.PlayDirection = direction;
			return this;
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0008B6EC File Offset: 0x000898EC
		public Tween<T> SetEasing(TweenEasingCallback easingFunction)
		{
			this.Easing = easingFunction;
			return this;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0008B6F8 File Offset: 0x000898F8
		public Tween<T> SetDuration(float duration)
		{
			this.Duration = duration;
			return this;
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0008B704 File Offset: 0x00089904
		public Tween<T> SetEndValue(T value)
		{
			this.EndValue = value;
			return this;
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0008B710 File Offset: 0x00089910
		public Tween<T> SetStartValue(T value)
		{
			this.CurrentValue = value;
			this.StartValue = value;
			return this;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0008B730 File Offset: 0x00089930
		public Tween<T> SetDelay(float seconds)
		{
			return this.SetDelay(seconds, this.assignStartValueBeforeDelay);
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0008B740 File Offset: 0x00089940
		public Tween<T> SetDelay(float seconds, bool assignStartValueBeforeDelay)
		{
			this.Delay = seconds;
			this.assignStartValueBeforeDelay = assignStartValueBeforeDelay;
			return this;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0008B754 File Offset: 0x00089954
		public Tween<T> SetLoopType(TweenLoopType loopType)
		{
			this.LoopType = loopType;
			return this;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0008B760 File Offset: 0x00089960
		public Tween<T> SetLoopCount(int loopCount)
		{
			this.LoopCount = loopCount;
			return this;
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0008B76C File Offset: 0x0008996C
		public Tween<T> SetTimeScaleIndependent(bool timeScaleIndependent)
		{
			this.IsTimeScaleIndependent = timeScaleIndependent;
			return this;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0008B778 File Offset: 0x00089978
		public override TweenBase Play()
		{
			if (this.TweenSyncStartValue != null)
			{
				this.StartValue = this.TweenSyncStartValue();
			}
			if (this.TweenSyncEndValue != null)
			{
				this.EndValue = this.TweenSyncEndValue();
			}
			base.Play();
			this.ensureInterpolator();
			if (this.assignStartValueBeforeDelay)
			{
				this.evaluateAtTime(this.CurrentTime);
			}
			return this;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0008B7E4 File Offset: 0x000899E4
		public override TweenBase Rewind()
		{
			base.Rewind();
			this.ensureInterpolator();
			this.evaluateAtTime(this.CurrentTime);
			return this;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0008B800 File Offset: 0x00089A00
		public override TweenBase FastForward()
		{
			base.FastForward();
			this.ensureInterpolator();
			this.evaluateAtTime(this.CurrentTime);
			return this;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0008B81C File Offset: 0x00089A1C
		public virtual TweenBase Seek(float time)
		{
			this.CurrentTime = Mathf.Clamp01(time);
			this.evaluateAtTime(this.CurrentTime);
			return this;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0008B838 File Offset: 0x00089A38
		public virtual TweenBase ReversePlayDirection()
		{
			this.PlayDirection = ((this.PlayDirection != TweenDirection.Forward) ? TweenDirection.Forward : TweenDirection.Reverse);
			return this;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0008B854 File Offset: 0x00089A54
		public Tween<T> SetInterpolator(Interpolator<T> interpolator)
		{
			this.Interpolator = interpolator;
			return this;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0008B860 File Offset: 0x00089A60
		public Tween<T> OnExecute(TweenAssignmentCallback<T> function)
		{
			this.Execute = function;
			return this;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0008B86C File Offset: 0x00089A6C
		public Tween<T> OnSyncStartValue(TweenSyncCallback<T> function)
		{
			this.TweenSyncStartValue = function;
			return this;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0008B878 File Offset: 0x00089A78
		public Tween<T> OnSyncEndValue(TweenSyncCallback<T> function)
		{
			this.TweenSyncEndValue = function;
			return this;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0008B884 File Offset: 0x00089A84
		public override void Update()
		{
			if (base.State == TweenState.Started)
			{
				float currentTime = base.getCurrentTime();
				if (currentTime < this.startTime + this.Delay)
				{
					return;
				}
				this.startTime = currentTime;
				this.CurrentTime = 0f;
				base.State = TweenState.Playing;
			}
			else if (base.State != TweenState.Playing)
			{
				return;
			}
			float deltaTime = base.getDeltaTime();
			this.CurrentTime = Mathf.MoveTowards(this.CurrentTime, 1f, deltaTime / this.Duration);
			float num = this.CurrentTime;
			if (this.Easing != null)
			{
				num = this.Easing(this.CurrentTime);
			}
			this.evaluateAtTime(num);
			if (this.CurrentTime >= 1f)
			{
				if (this.LoopType == TweenLoopType.Loop && --this.LoopCount != 0)
				{
					if (this.TweenLoopCompleted != null)
					{
						this.TweenLoopCompleted(this);
					}
					if (this.EndIsOffset)
					{
						this.StartValue = this.CurrentValue;
					}
					this.Rewind();
					this.Play();
				}
				else if (this.LoopType == TweenLoopType.Pingpong && --this.LoopCount != 0)
				{
					if (this.TweenLoopCompleted != null)
					{
						this.TweenLoopCompleted(this);
					}
					this.ReversePlayDirection();
					this.Play();
				}
				else
				{
					this.Stop();
					this.raiseCompleted();
					if (this.AutoCleanup)
					{
						this.Release();
					}
				}
			}
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0008BA10 File Offset: 0x00089C10
		private void ensureInterpolator()
		{
			if (this.Interpolator == null)
			{
				this.Interpolator = Interpolators.Get<T>();
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0008BA28 File Offset: 0x00089C28
		protected override void Reset()
		{
			base.Reset();
			this.StartValue = default(T);
			this.EndValue = default(T);
			this.CurrentValue = default(T);
			this.Duration = 1f;
			this.EndIsOffset = false;
			this.PlayDirection = TweenDirection.Forward;
			this.LoopCount = -1;
			this.assignStartValueBeforeDelay = true;
			this.Interpolator = null;
			this.Execute = null;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0008BAA0 File Offset: 0x00089CA0
		private void evaluateAtTime(float time)
		{
			if (this.Interpolator == null)
			{
				throw new InvalidOperationException(string.Format("No interpolator for type '{0}' has been assigned", typeof(T).Name));
			}
			T t = this.EndValue;
			if (this.EndIsOffset)
			{
				t = this.Interpolator.Add(this.StartValue, this.EndValue);
			}
			if (this.PlayDirection == TweenDirection.Reverse)
			{
				this.CurrentValue = this.Interpolator.Interpolate(t, this.StartValue, time);
			}
			else
			{
				this.CurrentValue = this.Interpolator.Interpolate(this.StartValue, t, time);
			}
			if (this.Execute != null)
			{
				this.Execute(this.CurrentValue);
			}
		}

		// Token: 0x040016D7 RID: 5847
		public T StartValue;

		// Token: 0x040016D8 RID: 5848
		public T EndValue;

		// Token: 0x040016D9 RID: 5849
		public Interpolator<T> Interpolator;

		// Token: 0x040016DA RID: 5850
		public TweenAssignmentCallback<T> Execute;

		// Token: 0x040016DD RID: 5853
		public TweenDirection PlayDirection;

		// Token: 0x040016DE RID: 5854
		public TweenSyncCallback<T> TweenSyncStartValue;

		// Token: 0x040016DF RID: 5855
		public TweenSyncCallback<T> TweenSyncEndValue;

		// Token: 0x040016E0 RID: 5856
		protected bool assignStartValueBeforeDelay = true;

		// Token: 0x040016E1 RID: 5857
		private static List<object> pool = new List<object>();
	}
}
