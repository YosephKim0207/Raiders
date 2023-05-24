using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x02000521 RID: 1313
	public class TweenShake : TweenBase, IPoolableObject
	{
		// Token: 0x06001F88 RID: 8072 RVA: 0x0008DB80 File Offset: 0x0008BD80
		public TweenShake()
		{
			this.ShakeSpeed = 10f;
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0008DB94 File Offset: 0x0008BD94
		public TweenShake(Vector3 StartValue, float ShakeMagnitude, float ShakeDuration, float ShakeSpeed, float StartDelay, bool AutoCleanup, TweenAssignmentCallback<Vector3> OnExecute)
		{
			this.SetStartValue(StartValue).SetShakeMagnitude(ShakeMagnitude).SetDuration(ShakeDuration)
				.SetShakeSpeed(ShakeSpeed)
				.SetDelay(StartDelay)
				.SetAutoCleanup(AutoCleanup)
				.OnExecute(OnExecute);
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0008DBCC File Offset: 0x0008BDCC
		public static TweenShake Obtain()
		{
			if (TweenShake.pool.Count > 0)
			{
				TweenShake tweenShake = TweenShake.pool[TweenShake.pool.Count - 1];
				TweenShake.pool.RemoveAt(TweenShake.pool.Count - 1);
				return tweenShake;
			}
			return new TweenShake();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0008DC20 File Offset: 0x0008BE20
		public void Release()
		{
			this.Stop();
			this.StartValue = Vector3.zero;
			this.currentValue = Vector3.zero;
			this.CurrentTime = 0f;
			this.Delay = 0f;
			this.ShakeCompleted = null;
			this.Execute = null;
			TweenShake.pool.Add(this);
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0008DC7C File Offset: 0x0008BE7C
		public TweenShake SetTimeScaleIndependent(bool timeScaleIndependent)
		{
			this.IsTimeScaleIndependent = timeScaleIndependent;
			return this;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0008DC88 File Offset: 0x0008BE88
		public TweenShake SetAutoCleanup(bool autoCleanup)
		{
			this.AutoCleanup = autoCleanup;
			return this;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0008DC94 File Offset: 0x0008BE94
		public TweenShake SetDuration(float duration)
		{
			this.ShakeDuration = duration;
			return this;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0008DCA0 File Offset: 0x0008BEA0
		public TweenShake SetStartValue(Vector3 value)
		{
			this.StartValue = value;
			return this;
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0008DCAC File Offset: 0x0008BEAC
		public TweenShake SetDelay(float seconds)
		{
			this.Delay = seconds;
			return this;
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x0008DCB8 File Offset: 0x0008BEB8
		public TweenShake SetShakeMagnitude(float magnitude)
		{
			this.ShakeMagnitude = magnitude;
			return this;
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0008DCC4 File Offset: 0x0008BEC4
		public TweenShake SetShakeSpeed(float speed)
		{
			this.ShakeSpeed = speed;
			return this;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0008DCD0 File Offset: 0x0008BED0
		public TweenShake OnExecute(TweenAssignmentCallback<Vector3> Execute)
		{
			this.Execute = Execute;
			return this;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0008DCDC File Offset: 0x0008BEDC
		public TweenShake OnComplete(TweenCallback Complete)
		{
			this.ShakeCompleted = Complete;
			return this;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0008DCE8 File Offset: 0x0008BEE8
		public override void Update()
		{
			float currentTime = base.getCurrentTime();
			if (base.State == TweenState.Started)
			{
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
			this.CurrentTime = Mathf.MoveTowards(this.CurrentTime, 1f, base.getDeltaTime() / this.ShakeDuration);
			float num = 1f - this.CurrentTime;
			num *= this.ShakeMagnitude;
			float num2 = Mathf.PerlinNoise(0.33f, currentTime * this.ShakeSpeed) * 2f - 1f;
			float num3 = Mathf.PerlinNoise(0.66f, currentTime * this.ShakeSpeed) * 2f - 1f;
			float num4 = Mathf.PerlinNoise(1f, currentTime * this.ShakeSpeed) * 2f - 1f;
			this.currentValue = this.StartValue + new Vector3(num2, num3, num4) * num;
			if (this.Execute != null)
			{
				this.Execute(this.currentValue);
			}
			if (this.CurrentTime >= 1f)
			{
				this.Stop();
				this.raiseCompleted();
				if (this.AutoCleanup)
				{
					this.Release();
				}
			}
		}

		// Token: 0x04001749 RID: 5961
		public Vector3 StartValue;

		// Token: 0x0400174A RID: 5962
		public float ShakeMagnitude;

		// Token: 0x0400174B RID: 5963
		public float ShakeDuration;

		// Token: 0x0400174C RID: 5964
		public float ShakeSpeed;

		// Token: 0x0400174D RID: 5965
		public TweenAssignmentCallback<Vector3> Execute;

		// Token: 0x0400174E RID: 5966
		public TweenCallback ShakeCompleted;

		// Token: 0x0400174F RID: 5967
		protected Vector3 currentValue;

		// Token: 0x04001750 RID: 5968
		private static List<TweenShake> pool = new List<TweenShake>();
	}
}
