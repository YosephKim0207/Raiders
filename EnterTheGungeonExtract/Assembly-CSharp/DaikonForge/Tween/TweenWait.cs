using System;
using System.Collections.Generic;

namespace DaikonForge.Tween
{
	// Token: 0x02000522 RID: 1314
	public class TweenWait : TweenBase
	{
		// Token: 0x06001F97 RID: 8087 RVA: 0x0008DE54 File Offset: 0x0008C054
		public TweenWait(float seconds)
		{
			this.Delay = seconds;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0008DE64 File Offset: 0x0008C064
		public static TweenWait Obtain(float seconds)
		{
			if (TweenWait.pool.Count > 0)
			{
				TweenWait tweenWait = TweenWait.pool[TweenWait.pool.Count - 1];
				TweenWait.pool.RemoveAt(TweenWait.pool.Count - 1);
				tweenWait.Delay = seconds;
				return tweenWait;
			}
			return new TweenWait(seconds)
			{
				AutoCleanup = true
			};
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0008DEC8 File Offset: 0x0008C0C8
		public void Release()
		{
			if (TweenWait.pool.Contains(this))
			{
				return;
			}
			this.Reset();
			TweenWait.pool.Add(this);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0008DEEC File Offset: 0x0008C0EC
		public override TweenBase Rewind()
		{
			this.elapsed = 0f;
			return base.Rewind();
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0008DF00 File Offset: 0x0008C100
		public override TweenBase FastForward()
		{
			this.elapsed = this.Delay;
			return base.FastForward();
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0008DF14 File Offset: 0x0008C114
		public override void Update()
		{
			if (base.State != TweenState.Playing && base.State != TweenState.Started)
			{
				return;
			}
			if (base.State == TweenState.Started)
			{
				this.elapsed = 0f;
				this.startTime = base.getCurrentTime();
				base.State = TweenState.Playing;
				return;
			}
			this.elapsed += base.getDeltaTime();
			if (this.elapsed >= this.Delay)
			{
				this.Stop();
				this.raiseCompleted();
			}
		}

		// Token: 0x04001751 RID: 5969
		private static List<TweenWait> pool = new List<TweenWait>();

		// Token: 0x04001752 RID: 5970
		private float elapsed;
	}
}
