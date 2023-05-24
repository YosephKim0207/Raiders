using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000084 RID: 132
[InspectorDropdownName("Bosses/BossFinalGuide/Clap2")]
public class BossFinalGuideClap2 : Script
{
	// Token: 0x06000203 RID: 515 RVA: 0x00009D40 File Offset: 0x00007F40
	protected override IEnumerator Top()
	{
		AkSoundEngine.PostEvent("Play_BOSS_cyborg_eagle_01", GameManager.Instance.gameObject);
		base.EndOnBlank = true;
		Vector2 leftOrigin = new Vector2(0f, 0f);
		Vector2 rightOrigin = new Vector2(0f, 0f);
		this.FireLine(leftOrigin, new Vector2(-3.5f, 2.5f), new Vector2(-12.5f, 2.5f), 16, 180f, 1f, true);
		this.FireLine(leftOrigin, new Vector2(-7f, 2f), new Vector2(-12f, 2f), 8, 190f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-6.75f, 1.5f), new Vector2(-11.5f, 1.5f), 7, 200f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-6.5f, 1f), new Vector2(-11f, 1f), 6, 210f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-6.25f, 0.5f), new Vector2(-10f, 0.5f), 6, 220f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-6f, 0f), new Vector2(-9f, 0f), 5, 230f, 1f, false);
		Vector2 center = new Vector2(-1f, 2f);
		for (int i = 0; i < 10; i++)
		{
			float num = -180f + (float)i * 10f;
			float num2 = 4f - (float)Mathf.Min(i, 5) * 0.1f;
			float num3 = 5f + (float)Mathf.Max(0, 3 - i) * 0.125f;
			Vector2 vector = center + BraveMathCollege.DegreesToVector(num, num2);
			Vector2 vector2 = center + BraveMathCollege.DegreesToVector(num - (float)i * 0.5f, num3);
			this.FireLine(leftOrigin, vector, vector2, 4, (vector2 - vector).ToAngle(), 1f, true);
		}
		this.FireLine(rightOrigin, new Vector2(3.5f, 2.5f), new Vector2(12.5f, 2.5f), 16, 0f, 1f, true);
		this.FireLine(rightOrigin, new Vector2(7f, 2f), new Vector2(12f, 2f), 8, -10f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(6.75f, 1.5f), new Vector2(11.5f, 1.5f), 7, -20f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(6.5f, 1f), new Vector2(11f, 1f), 6, -30f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(6.25f, 0.5f), new Vector2(10f, 0.5f), 6, -40f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(6f, 0f), new Vector2(9f, 0f), 5, -50f, 1f, false);
		center = new Vector2(1f, 2f);
		for (int j = 0; j < 10; j++)
		{
			float num4 = (float)j * -10f;
			float num5 = 4f - (float)Mathf.Min(j, 5) * 0.1f;
			float num6 = 5f + (float)Mathf.Max(0, 3 - j) * 0.125f;
			Vector2 vector3 = center + BraveMathCollege.DegreesToVector(num4, num5);
			Vector2 vector4 = center + BraveMathCollege.DegreesToVector(num4 + (float)j * 0.5f, num6);
			this.FireLine(rightOrigin, vector3, vector4, 4, (vector4 - vector3).ToAngle(), 1f, true);
		}
		this.FireLine(leftOrigin, new Vector2(-0.5f, -3.8f) * 1.5f, 1, -100f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-0.3f, -3.9f) * 1.5f, 1, -100f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-0.5f, -4f) * 1.5f, 1, -100f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-0.7f, -4f) * 1.5f, 1, -100f, 1f, false);
		this.FireLine(leftOrigin, new Vector2(-0.45f, -4.2f) * 1.5f, 1, -100f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(0.5f, -3.8f) * 1.5f, 1, -80f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(0.3f, -3.9f) * 1.5f, 1, -80f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(0.5f, -4f) * 1.5f, 1, -80f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(0.7f, -4f) * 1.5f, 1, -80f, 1f, false);
		this.FireLine(rightOrigin, new Vector2(0.45f, -4.2f) * 1.5f, 1, -80f, 1f, false);
		float angle = 180f;
		float delta = -16.363636f;
		this.FireLine(leftOrigin, new Vector2(-0.9f, 3.5f), angle);
		angle += delta;
		this.FireLine(leftOrigin, new Vector2(-0.7f, 4.1f), angle);
		angle += delta;
		this.FireLine(leftOrigin, new Vector2(-0.5f, 4.7f), angle);
		angle += delta;
		this.FireLine(leftOrigin, new Vector2(-0.4f, 5.3f), angle);
		angle += delta;
		this.FireLine(leftOrigin, new Vector2(-0.1f, 5.5f), angle);
		angle += delta;
		this.FireLine(leftOrigin, new Vector2(0.3f, 5.5f), angle);
		angle += delta;
		this.FireLine(rightOrigin, new Vector2(0.5f, 5.3f), angle);
		angle += delta;
		this.FireLine(rightOrigin, new Vector2(0.9f, 5.3f), angle);
		angle += delta;
		this.FireLine(rightOrigin, new Vector2(1f, 4.9f), angle);
		angle += delta;
		this.FireLine(rightOrigin, new Vector2(0.7f, 4.8f), angle);
		angle += delta;
		this.FireLine(rightOrigin, new Vector2(0.6f, 4.4f), angle);
		angle += delta;
		this.FireLine(rightOrigin, new Vector2(0.7f, 3.8f), angle);
		angle += delta;
		yield return base.Wait(310);
		yield break;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00009D5C File Offset: 0x00007F5C
	private void FireLine(Vector2 spawn, Vector2 start, float direction)
	{
		this.FireLine(spawn, start, start, 1, direction, 1f, false);
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00009D70 File Offset: 0x00007F70
	private void FireLine(Vector2 start, Vector2 end, int numBullets, float direction, float timeMultiplier = 1f, bool lerpSpeed = false)
	{
		this.FireLine(start, start, end, numBullets, direction, timeMultiplier, lerpSpeed);
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00009D84 File Offset: 0x00007F84
	private void FireLine(Vector2 spawnPoint, Vector2 start, Vector2 end, int numBullets, float direction, float timeMultiplier = 1f, bool lerpSpeed = false)
	{
		Vector2 vector = (end - start) / (float)Mathf.Max(1, numBullets - 1);
		float num = 0.6666667f * timeMultiplier;
		for (int i = 0; i < numBullets; i++)
		{
			Vector2 vector2 = ((numBullets != 1) ? (start + vector * (float)i) : end);
			float num2 = Vector2.Distance(vector2, spawnPoint) / num;
			base.Fire(new Offset(spawnPoint, 0f, string.Empty, DirectionType.Absolute), new Direction((vector2 - spawnPoint).ToAngle(), DirectionType.Absolute, -1f), new Speed(num2, SpeedType.Absolute), new BossFinalGuideClap2.WingBullet(direction, (!lerpSpeed) ? 1f : ((float)i / (float)numBullets), timeMultiplier));
		}
	}

	// Token: 0x04000218 RID: 536
	private const int SetupTime = 40;

	// Token: 0x04000219 RID: 537
	private const int HoldTime = 90;

	// Token: 0x0400021A RID: 538
	private const float FireSpeed = 8f;

	// Token: 0x02000085 RID: 133
	public class WingBullet : Bullet
	{
		// Token: 0x06000207 RID: 519 RVA: 0x00009E44 File Offset: 0x00008044
		public WingBullet(float direction, float speedT, float timeMultiplier)
			: base(null, false, false, false)
		{
			this.m_direction = direction;
			this.m_speedT = speedT;
			this.m_timeMultiplier = timeMultiplier;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009E68 File Offset: 0x00008068
		protected override IEnumerator Top()
		{
			yield return base.Wait(40f * this.m_timeMultiplier);
			this.Speed = 0f;
			yield return base.Wait(90f + 40f * (1f - this.m_timeMultiplier));
			this.Speed = 8f * Mathf.Lerp(0.1f, 1f, this.m_speedT);
			this.Direction = this.m_direction;
			yield return base.Wait(180);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400021B RID: 539
		private float m_direction;

		// Token: 0x0400021C RID: 540
		private float m_speedT;

		// Token: 0x0400021D RID: 541
		private float m_timeMultiplier;
	}
}
