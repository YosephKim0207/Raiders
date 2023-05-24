using System;
using UnityEngine;

// Token: 0x02001694 RID: 5780
public class WaftingDebrisObject : DebrisObject
{
	// Token: 0x060086C3 RID: 34499 RVA: 0x0037DAF4 File Offset: 0x0037BCF4
	protected override void UpdateVelocity(float adjustedDeltaTime)
	{
		if (!this.m_initialized)
		{
			this.m_initialized = true;
			this.m_cachedInitialVelocity = this.m_velocity;
			this.m_peakDuration = Mathf.Lerp(this.initialBurstDuration.x, this.initialBurstDuration.y, UnityEngine.Random.value);
		}
		if (this.m_currentPosition.z > 0f)
		{
			if (!this.m_hasHitPeak)
			{
				this.m_peakElapsed += adjustedDeltaTime;
				float num = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, this.m_peakElapsed / this.m_peakDuration);
				this.m_velocity = Vector3.Lerp(Vector3.Scale(this.m_cachedInitialVelocity, new Vector3(2.5f, 2.5f, 4f)), new Vector3(this.m_cachedInitialVelocity.x * 0.5f, this.m_cachedInitialVelocity.y * 0.5f, 0f), num);
				if (this.m_velocity.z <= 0f)
				{
					this.m_hasHitPeak = true;
					this.m_waftPeriod = Mathf.Lerp(this.waftDuration.x, this.waftDuration.y, UnityEngine.Random.value);
					this.m_waftDistance = Mathf.Lerp(this.waftDistance.x, this.waftDistance.y, UnityEngine.Random.value);
					this.m_coplanarSign = ((UnityEngine.Random.value <= 0.5f) ? (-1) : 1);
					if (UnityEngine.Random.value < 0.5f)
					{
						this.m_waftElapsed = this.m_waftPeriod / 2f;
					}
					else
					{
						this.m_waftElapsed = 0f;
					}
				}
			}
			else
			{
				this.m_waftElapsed += adjustedDeltaTime;
				float num2 = this.m_waftElapsed % this.m_waftPeriod;
				float num3 = Mathf.Cos(num2 / this.m_waftPeriod * 2f * 3.1415927f);
				float num4 = Mathf.Sin(num2 / this.m_waftPeriod * 2f * 3.1415927f);
				float num5 = Mathf.Lerp(this.m_velocity.z, this.m_velocity.z + 4f * adjustedDeltaTime, Mathf.Abs(num3));
				num5 += -3f * adjustedDeltaTime;
				this.m_velocity = new Vector3(this.m_waftDistance * num3, this.m_waftDistance / 5f * num4 * (float)this.m_coplanarSign, num5);
				if (!string.IsNullOrEmpty(this.waftAnimationName))
				{
					tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.waftAnimationName);
					if (clipByName != base.spriteAnimator.CurrentClip)
					{
						base.spriteAnimator.Play(this.waftAnimationName);
						base.spriteAnimator.Stop();
					}
					float num6 = (this.m_waftElapsed + 0.5f * this.m_waftPeriod) % this.m_waftPeriod;
					float num7 = Mathf.PingPong(num6 / this.m_waftPeriod * 2f, 1f);
					int num8 = Mathf.Clamp(Mathf.FloorToInt((float)clipByName.frames.Length * num7), 0, clipByName.frames.Length - 1);
					base.spriteAnimator.SetFrame(num8);
				}
			}
		}
	}

	// Token: 0x060086C4 RID: 34500 RVA: 0x0037DE10 File Offset: 0x0037C010
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008BE8 RID: 35816
	[Header("Waft Properties")]
	public string waftAnimationName;

	// Token: 0x04008BE9 RID: 35817
	public Vector2 initialBurstDuration = new Vector2(0.3f, 0.45f);

	// Token: 0x04008BEA RID: 35818
	public Vector2 waftDuration = new Vector2(2f, 4.5f);

	// Token: 0x04008BEB RID: 35819
	public Vector2 waftDistance = new Vector2(1.5f, 3.5f);

	// Token: 0x04008BEC RID: 35820
	private bool m_initialized;

	// Token: 0x04008BED RID: 35821
	private Vector3 m_cachedInitialVelocity;

	// Token: 0x04008BEE RID: 35822
	private float m_peakElapsed;

	// Token: 0x04008BEF RID: 35823
	private float m_peakDuration;

	// Token: 0x04008BF0 RID: 35824
	private bool m_hasHitPeak;

	// Token: 0x04008BF1 RID: 35825
	private float m_waftElapsed;

	// Token: 0x04008BF2 RID: 35826
	private float m_waftPeriod;

	// Token: 0x04008BF3 RID: 35827
	private float m_waftDistance;

	// Token: 0x04008BF4 RID: 35828
	private int m_coplanarSign;
}
