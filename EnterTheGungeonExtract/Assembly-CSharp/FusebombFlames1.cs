using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001A1 RID: 417
[InspectorDropdownName("Bosses/Fusebomb/Flames1")]
public class FusebombFlames1 : Script
{
	// Token: 0x0600063B RID: 1595 RVA: 0x0001E020 File Offset: 0x0001C220
	protected override IEnumerator Top()
	{
		List<AIActor> list = new List<AIActor>();
		Vector2 vector = base.BulletBank.aiActor.ParentRoom.area.UnitBottomLeft + new Vector2(1f, 4.8f);
		float num = (float)(base.BulletBank.aiActor.ParentRoom.area.dimensions.x - 2);
		float num2 = num / 4f;
		for (int i = 0; i < 4; i++)
		{
			float num3 = UnityEngine.Random.Range(0f, num);
			num3 = num3 % num2 + num2 * (float)i;
			float num4 = UnityEngine.Random.Range(0f, 0.8f);
			for (int j = 0; j < 6; j++)
			{
				Vector2 vector2 = vector + new Vector2(num3, (float)j + num4);
				float num5 = (vector2 - base.Position).ToAngle();
				base.Fire(new Direction(num5, DirectionType.Absolute, -1f), new FusebombFlames1.FlameBullet(vector2, list, 60 + 10 * i + 10 * j));
			}
		}
		return null;
	}

	// Token: 0x04000608 RID: 1544
	public const int NumFlameRows = 4;

	// Token: 0x04000609 RID: 1545
	public const int NumFlamesPerRow = 6;

	// Token: 0x020001A2 RID: 418
	private class FlameBullet : Bullet
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x0001E138 File Offset: 0x0001C338
		public FlameBullet(Vector2 goalPos, List<AIActor> spawnedActors, int flightTime)
			: base("flame", false, false, false)
		{
			this.m_goalPos = goalPos;
			this.m_flightTime = flightTime;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001E158 File Offset: 0x0001C358
		protected override IEnumerator Top()
		{
			this.Projectile.IgnoreTileCollisionsFor((float)(this.m_flightTime - 5) / 60f);
			float dir = (this.m_goalPos - base.Position).ToAngle();
			tk2dSpriteAnimationClip clip;
			if (BraveMathCollege.AbsAngleBetween(0f, dir) <= 90f)
			{
				clip = this.Projectile.spriteAnimator.GetClipByName("fusebomb_fire_projectile_right");
			}
			else
			{
				clip = this.Projectile.spriteAnimator.GetClipByName("fusebomb_fire_projectile_left");
			}
			this.Projectile.spriteAnimator.Play(clip, UnityEngine.Random.Range(0f, clip.BaseClipLength - 0.1f), clip.fps, false);
			base.ManualControl = true;
			this.Direction = dir;
			this.Speed = Vector2.Distance(this.m_goalPos, base.Position) / ((float)this.m_flightTime / 60f);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < this.m_flightTime; i++)
			{
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + new Vector2(0f, Mathf.Sin((float)i / (float)this.m_flightTime * 3.1415927f) * 5f);
				yield return base.Wait(1);
			}
			yield return base.Wait((4 + UnityEngine.Random.Range(0, 6)) * 60);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400060A RID: 1546
		private Vector2 m_goalPos;

		// Token: 0x0400060B RID: 1547
		private int m_flightTime;
	}
}
