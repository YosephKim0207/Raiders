using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001F3 RID: 499
[InspectorDropdownName("Bosses/Infinilich/CarnageSpin1")]
public class InfinilichCarnageSpin1 : Script
{
	// Token: 0x0600076D RID: 1901 RVA: 0x00023A24 File Offset: 0x00021C24
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		InfinilichCarnageSpin1.SpinDirection = BraveUtility.RandomSign();
		base.Fire(new Offset("limb 1"), new Direction(45f, DirectionType.Absolute, -1f), new Speed(32f, SpeedType.Absolute), new InfinilichCarnageSpin1.TipBullet(this));
		base.Fire(new Offset("limb 2"), new Direction(-45f, DirectionType.Absolute, -1f), new Speed(32f, SpeedType.Absolute), new InfinilichCarnageSpin1.TipBullet(this));
		base.Fire(new Offset("limb 3"), new Direction(-135f, DirectionType.Absolute, -1f), new Speed(32f, SpeedType.Absolute), new InfinilichCarnageSpin1.TipBullet(this));
		base.Fire(new Offset("limb 4"), new Direction(135f, DirectionType.Absolute, -1f), new Speed(32f, SpeedType.Absolute), new InfinilichCarnageSpin1.TipBullet(this));
		yield return base.Wait(60);
		for (int i = 0; i < 6; i++)
		{
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed(16f, SpeedType.Absolute), new InfinilichCarnageSpin1.TipBullet(this));
			yield return base.Wait(30);
		}
		yield return base.Wait(90);
		this.Spin = true;
		yield return base.Wait(420);
		yield break;
	}

	// Token: 0x0400073F RID: 1855
	private const int SpinTime = 420;

	// Token: 0x04000740 RID: 1856
	private static float SpinDirection;

	// Token: 0x04000741 RID: 1857
	public bool Spin;

	// Token: 0x020001F4 RID: 500
	public class TipBullet : Bullet
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x00023A40 File Offset: 0x00021C40
		public TipBullet(InfinilichCarnageSpin1 parentScript)
			: base("carnageTip", false, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00023A58 File Offset: 0x00021C58
		protected override IEnumerator Top()
		{
			float spinSpeed = InfinilichCarnageSpin1.SpinDirection * UnityEngine.Random.Range(0.5f, 0.8f);
			for (int i = 0; i < 60; i++)
			{
				base.Fire(new Direction(0f, DirectionType.Relative, -1f), new Speed(0f, SpeedType.Absolute), new InfinilichCarnageSpin1.ChainBullet(this.m_parentScript, i, this.Speed, spinSpeed));
				yield return base.Wait((this.Speed <= 20f) ? 2 : 1);
			}
			while (!this.m_parentScript.Spin)
			{
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000742 RID: 1858
		private InfinilichCarnageSpin1 m_parentScript;
	}

	// Token: 0x020001F6 RID: 502
	public class ChainBullet : Bullet
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x00023C08 File Offset: 0x00021E08
		public ChainBullet(InfinilichCarnageSpin1 parentScript, int spawnDelay, float tipSpeed, float spinSpeed)
			: base(null, false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_spawnDelay = spawnDelay;
			this.m_tipSpeed = tipSpeed;
			this.m_spinSpeed = spinSpeed;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00023C34 File Offset: 0x00021E34
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			float wigglePeriod = 0.333f * this.m_tipSpeed;
			float currentOffset = 0f;
			for (int i = 0; i < 45 - this.m_spawnDelay; i++)
			{
				float magnitude = 0.75f;
				magnitude = Mathf.Min(magnitude, Mathf.Lerp(0f, 0.75f, (float)i / 20f));
				magnitude = Mathf.Min(magnitude, Mathf.Lerp(0f, 0.75f, (float)this.m_spawnDelay / 10f));
				currentOffset = Mathf.SmoothStep(-magnitude, magnitude, Mathf.PingPong(0.5f + (float)i / 60f * wigglePeriod, 1f));
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, currentOffset);
				yield return base.Wait(1);
			}
			float lastOffset = currentOffset;
			for (int j = 0; j < 3; j++)
			{
				currentOffset = Mathf.Lerp(lastOffset, 0f, (float)j / 2f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, currentOffset);
				yield return base.Wait(1);
			}
			while (!this.m_parentScript.Spin)
			{
				yield return base.Wait(1);
			}
			float angle = (base.Position - this.m_parentScript.Position).ToAngle();
			float radius = (base.Position - this.m_parentScript.Position).magnitude;
			for (int k = 0; k < 420; k++)
			{
				float deltaAngle = Mathf.Lerp(0f, this.m_spinSpeed, (float)k / 60f);
				angle += deltaAngle;
				base.Position = this.m_parentScript.Position + BraveMathCollege.DegreesToVector(angle, radius);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000749 RID: 1865
		private const float WiggleMagnitude = 0.75f;

		// Token: 0x0400074A RID: 1866
		private const float WigglePeriodMultiplier = 0.333f;

		// Token: 0x0400074B RID: 1867
		private InfinilichCarnageSpin1 m_parentScript;

		// Token: 0x0400074C RID: 1868
		private int m_spawnDelay;

		// Token: 0x0400074D RID: 1869
		private float m_tipSpeed;

		// Token: 0x0400074E RID: 1870
		private float m_spinSpeed;
	}
}
