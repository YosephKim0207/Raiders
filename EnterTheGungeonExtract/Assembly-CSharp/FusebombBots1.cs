using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200019E RID: 414
[InspectorDropdownName("Bosses/Fusebomb/Bots1")]
public class FusebombBots1 : Script
{
	// Token: 0x06000631 RID: 1585 RVA: 0x0001DC10 File Offset: 0x0001BE10
	protected override IEnumerator Top()
	{
		int num = 4;
		List<AIActor> list = new List<AIActor>();
		Vector2 vector = base.BulletBank.aiActor.ParentRoom.area.UnitBottomLeft + new Vector2(1f, 5.5f);
		Vector2 vector2 = new Vector2((float)(base.BulletBank.aiActor.ParentRoom.area.dimensions.x - 2), 4.75f);
		float num2 = vector2.x / (float)num;
		for (int i = 0; i < num; i++)
		{
			Vector2 vector3 = BraveUtility.RandomVector2(Vector2.zero, vector2);
			vector3.x = vector3.x % num2 + num2 * (float)i;
			Vector2 vector4 = vector + vector3;
			float num3 = (vector4 - base.Position).ToAngle();
			base.Fire(new Direction(num3, DirectionType.Absolute, -1f), new FusebombBots1.BotBullet(vector4, list, 60 + 10 * i));
		}
		return null;
	}

	// Token: 0x040005FE RID: 1534
	private const string EnemyGuid = "4538456236f64ea79f483784370bc62f";

	// Token: 0x0200019F RID: 415
	private class BotBullet : Bullet
	{
		// Token: 0x06000632 RID: 1586 RVA: 0x0001DD0C File Offset: 0x0001BF0C
		public BotBullet(Vector2 goalPos, List<AIActor> spawnedActors, int flightTime)
			: base("bot", false, false, false)
		{
			this.m_goalPos = goalPos;
			this.m_flightTime = flightTime;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001DD2C File Offset: 0x0001BF2C
		protected override IEnumerator Top()
		{
			this.Projectile.IgnoreTileCollisionsFor((float)(this.m_flightTime - 5) / 60f);
			base.ManualControl = true;
			this.Direction = (this.m_goalPos - base.Position).ToAngle();
			this.Speed = Vector2.Distance(this.m_goalPos, base.Position) / ((float)this.m_flightTime / 60f);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < this.m_flightTime; i++)
			{
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + new Vector2(0f, Mathf.Sin((float)i / (float)this.m_flightTime * 3.1415927f) * 5f);
				if (this.m_flightTime - i == 60 && base.BulletBank && base.BulletBank.aiAnimator)
				{
					AIAnimator aiAnimator = base.BulletBank.aiAnimator;
					string text = "remote_spawn";
					Vector2? vector = new Vector2?(this.m_goalPos);
					aiAnimator.PlayVfx(text, null, null, vector);
				}
				yield return base.Wait(1);
			}
			Vector2 spawnPos = this.Projectile.specRigidbody.UnitBottomCenter + new Vector2(0f, 0.125f);
			AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid("4538456236f64ea79f483784370bc62f"), spawnPos, base.BulletBank.aiActor.ParentRoom, true, AIActor.AwakenAnimationType.Awaken, true);
			base.Vanish(true);
			yield break;
		}

		// Token: 0x040005FF RID: 1535
		private Vector2 m_goalPos;

		// Token: 0x04000600 RID: 1536
		private int m_flightTime;
	}
}
