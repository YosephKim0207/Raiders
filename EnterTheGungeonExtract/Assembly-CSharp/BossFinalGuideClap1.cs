using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x0200007F RID: 127
[InspectorDropdownName("Bosses/BossFinalGuide/Clap1")]
public class BossFinalGuideClap1 : Script
{
	// Token: 0x060001EB RID: 491 RVA: 0x00009708 File Offset: 0x00007908
	protected override IEnumerator Top()
	{
		AIActor aiActor = base.BulletBank.aiActor;
		RoomHandler parentRoom = aiActor.ParentRoom;
		CellArea area = parentRoom.area;
		this.m_roomMin = area.basePosition.ToVector2();
		this.m_roomMax = (area.basePosition + area.dimensions).ToVector2();
		this.m_roomMin.x = this.m_roomMin.x + 8f;
		this.m_roomMax.x = this.m_roomMax.x - 8f;
		this.m_roomMax.y = this.m_roomMax.y - 9f;
		for (int i = 0; i < 25; i++)
		{
			base.StartTask(this.FireBolt());
			yield return base.Wait(15);
		}
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00009724 File Offset: 0x00007924
	private IEnumerator FireBolt()
	{
		float width = this.m_roomMax.x - this.m_roomMin.x;
		float quarterWidth = width / 4f;
		if (this.m_quarterIndex >= 4)
		{
			this.m_quarterIndex = 0;
			BraveUtility.RandomizeArray<int>(this.m_quarters, 0, -1);
		}
		int quarter = this.m_quarters[this.m_quarterIndex];
		Vector2 firePos = new Vector2(UnityEngine.Random.Range(this.m_roomMin.x + (float)quarter * quarterWidth, this.m_roomMin.x + (float)(quarter + 1) * quarterWidth), this.m_roomMax.y + 10f);
		this.m_quarterIndex++;
		for (int i = 0; i < 11; i++)
		{
			if (i < 6)
			{
				base.Fire(Offset.OverridePosition(firePos + new Vector2((float)i * 0.2f, 0f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(20f, SpeedType.Absolute), new BossFinalGuideClap1.LightningBullet());
			}
			if (i == 5)
			{
				base.Fire(Offset.OverridePosition(firePos + new Vector2(0.5f, -0.1f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(20f, SpeedType.Absolute), new BossFinalGuideClap1.LightningBullet());
			}
			if (i >= 5)
			{
				base.Fire(Offset.OverridePosition(firePos + new Vector2((float)(i - 5) * 0.2f, -0.2f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(20f, SpeedType.Absolute), new BossFinalGuideClap1.LightningBullet());
			}
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x040001FD RID: 509
	private const int NumBolts = 25;

	// Token: 0x040001FE RID: 510
	private const int BoltSpeed = 20;

	// Token: 0x040001FF RID: 511
	private Vector2 m_roomMin;

	// Token: 0x04000200 RID: 512
	private Vector2 m_roomMax;

	// Token: 0x04000201 RID: 513
	private int[] m_quarters = new int[] { 0, 1, 2, 3 };

	// Token: 0x04000202 RID: 514
	private int m_quarterIndex = 4;

	// Token: 0x02000080 RID: 128
	private class LightningBullet : Bullet
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00009740 File Offset: 0x00007940
		public LightningBullet()
			: base("lightning", false, false, false)
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009750 File Offset: 0x00007950
		protected override IEnumerator Top()
		{
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle));
			yield return base.Wait(65f / this.Speed * 60f);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000976C File Offset: 0x0000796C
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile && this.Projectile.specRigidbody)
			{
				this.Projectile.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle));
			}
		}
	}
}
