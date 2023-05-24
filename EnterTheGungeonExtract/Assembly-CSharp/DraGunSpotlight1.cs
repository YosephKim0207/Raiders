using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200015D RID: 349
[InspectorDropdownName("Bosses/DraGun/Spotlight1")]
public class DraGunSpotlight1 : Script
{
	// Token: 0x06000539 RID: 1337 RVA: 0x00019334 File Offset: 0x00017534
	protected override IEnumerator Top()
	{
		GameManager.Instance.Dungeon.PreventPlayerLightInDarkTerrifyingRooms = true;
		DraGunController dragunController = base.BulletBank.GetComponent<DraGunController>();
		dragunController.aiActor.ParentRoom.BecomeTerrifyingDarkRoom(0.5f, 0.1f, 1f, "Play_ENM_darken_world_01");
		dragunController.HandleDarkRoomEffects(true, 3f);
		yield return base.Wait(30);
		dragunController.SpotlightPos = base.BulletBank.aiActor.transform.position + new Vector3(4f, 1f);
		dragunController.SpotlightSpeed = 8f;
		dragunController.SpotlightSmoothTime = 0.2f;
		dragunController.SpotlightVelocity = Vector2.zero;
		dragunController.SpotlightEnabled = true;
		base.StartTask(this.UpdateSpotlightShrink());
		while (base.Tick < 480)
		{
			float dist = Vector2.Distance(this.BulletManager.PlayerPosition(), dragunController.SpotlightPos);
			dragunController.SpotlightSpeed = Mathf.Lerp(6f, 14f, Mathf.InverseLerp(3f, 10f, dist));
			if (dist <= dragunController.SpotlightRadius)
			{
				float t = UnityEngine.Random.value;
				float speed = Mathf.Lerp(8f, 14f, t);
				Vector2 target = ((!BraveUtility.RandomBool()) ? base.GetPredictedTargetPositionExact(1f, speed) : this.BulletManager.PlayerPosition());
				base.Fire(new Direction((target - base.Position).ToAngle(), DirectionType.Absolute, -1f), new Speed(speed, SpeedType.Absolute), new DraGunSpotlight1.ArcBullet(target, t));
				yield return base.Wait(3);
			}
			yield return base.Wait(1);
		}
		dragunController.SpotlightEnabled = false;
		dragunController.aiActor.ParentRoom.EndTerrifyingDarkRoom(0.5f, 0.1f, 1f, "Play_ENM_lighten_world_01");
		dragunController.HandleDarkRoomEffects(false, 3f);
		yield return base.Wait(30);
		GameManager.Instance.Dungeon.PreventPlayerLightInDarkTerrifyingRooms = false;
		yield break;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00019350 File Offset: 0x00017550
	private IEnumerator UpdateSpotlightShrink()
	{
		DraGunController dragunController = base.BulletBank.GetComponent<DraGunController>();
		int startTick = base.Tick;
		while (base.Tick < 480)
		{
			if (base.Tick - startTick < 10)
			{
				dragunController.SpotlightShrink = (float)(base.Tick - startTick) / 9f;
			}
			else if (base.Tick > 470)
			{
				int num = 480 - base.Tick - 1;
				dragunController.SpotlightShrink = (float)num / 9f;
			}
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0001936C File Offset: 0x0001756C
	public override void OnForceEnded()
	{
		DraGunController component = base.BulletBank.GetComponent<DraGunController>();
		component.SpotlightEnabled = false;
		component.aiActor.ParentRoom.EndTerrifyingDarkRoom(0.5f, 0.1f, 1f, "Play_ENM_lighten_world_01");
		component.HandleDarkRoomEffects(false, 3f);
	}

	// Token: 0x04000514 RID: 1300
	public const int ChaseTime = 480;

	// Token: 0x0200015E RID: 350
	public class ArcBullet : Bullet
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x000193BC File Offset: 0x000175BC
		public ArcBullet(Vector2 target, float t)
			: base("triangle", false, false, false)
		{
			this.m_target = target;
			this.m_t = t;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000193DC File Offset: 0x000175DC
		protected override IEnumerator Top()
		{
			Vector2 toTarget = this.m_target - base.Position;
			float travelTime = toTarget.magnitude / this.Speed * 60f - 1f;
			float magnitude = BraveUtility.RandomSign() * (1f - this.m_t) * 8f;
			Vector2 offset = magnitude * toTarget.Rotate(90f).normalized;
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			Vector2 lastPosition = base.Position;
			int i = 0;
			while ((float)i < travelTime)
			{
				base.UpdateVelocity();
				truePosition += this.Velocity / 60f;
				lastPosition = base.Position;
				base.Position = truePosition + offset * Mathf.Sin((float)base.Tick / travelTime * 3.1415927f);
				yield return base.Wait(1);
				i++;
			}
			Vector2 v = (base.Position - lastPosition) * 60f;
			this.Speed = v.magnitude;
			this.Direction = v.ToAngle();
			base.ManualControl = false;
			yield break;
		}

		// Token: 0x04000515 RID: 1301
		private Vector2 m_target;

		// Token: 0x04000516 RID: 1302
		private float m_t;
	}
}
