using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public abstract class BossFinalRogueLaunchShips1 : Script
{
	// Token: 0x06000283 RID: 643 RVA: 0x0000CF7C File Offset: 0x0000B17C
	protected override IEnumerator Top()
	{
		for (int y = 0; y < 3; y++)
		{
			for (int x = 0; x < 4; x++)
			{
				float dx = Mathf.Lerp(this.ShipPosMax.x, this.ShipPosMin.x, (float)x / 3f);
				float dy = Mathf.Lerp(this.ShipPosMin.y, this.ShipPosMax.y, (float)y / 2f);
				if (y % 2 == 1)
				{
					dx += 0.5f * (this.ShipPosMax.x - this.ShipPosMin.x) / 3f;
				}
				if (this is BossFinalRogueLaunchShipsLeft1)
				{
					dx *= -1f;
				}
				base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(4f, SpeedType.Absolute), new BossFinalRogueLaunchShips1.Ship(dx, dy, (y * 4 + x) * 15));
				yield return base.Wait(15);
			}
		}
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x040002B6 RID: 694
	private const int NumShipColumns = 4;

	// Token: 0x040002B7 RID: 695
	private const int NumShipRows = 3;

	// Token: 0x040002B8 RID: 696
	private const int LifeTime = 900;

	// Token: 0x040002B9 RID: 697
	private const int MinShootInterval = 120;

	// Token: 0x040002BA RID: 698
	private const int MaxShootInterval = 240;

	// Token: 0x040002BB RID: 699
	private Vector2 ShipPosMin = new Vector2(6f, -2f);

	// Token: 0x040002BC RID: 700
	private Vector2 ShipPosMax = new Vector2(16f, 2f);

	// Token: 0x020000A4 RID: 164
	public class Ship : Bullet
	{
		// Token: 0x06000284 RID: 644 RVA: 0x0000CF98 File Offset: 0x0000B198
		public Ship(float xOffset, float yOffset, int spawnDelay)
			: base("anActualSpaceship", false, false, false)
		{
			this.m_desiredXOffset = xOffset;
			this.m_desiredYOffset = yOffset;
			this.m_spawnTime = spawnDelay;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		protected override IEnumerator Top()
		{
			SpeculativeRigidbody ownerRigidbody = this.Projectile.Owner.specRigidbody;
			GameObject exhaust = this.Projectile.transform.Find("Sprite/trail").gameObject;
			Vector2 ownerCenter = ownerRigidbody.UnitCenter;
			Vector2 startingOffset = base.Position - ownerCenter;
			this.Projectile.ImmuneToBlanks = true;
			this.Speed = 4f;
			yield return base.Wait(10);
			base.ChangeDirection(new Direction((float)((startingOffset.x <= 0f) ? 180 : 0), DirectionType.Absolute, -1f), 10);
			yield return base.Wait(10);
			exhaust.SetActive(true);
			ownerCenter = ownerRigidbody.UnitCenter;
			Vector2 lerpStartOffset = base.Position - ownerCenter;
			Vector2 desiredOffset = startingOffset + new Vector2(this.m_desiredXOffset, this.m_desiredYOffset);
			base.ManualControl = true;
			for (int i = 0; i < 60; i++)
			{
				ownerCenter = ownerRigidbody.UnitCenter;
				base.Position = ownerCenter + Vector2.Lerp(lerpStartOffset, desiredOffset, (float)i / 60f);
				yield return base.Wait(1);
			}
			base.ChangeDirection(new Direction(-90f, DirectionType.Absolute, -1f), 10);
			for (int j = 0; j < 10; j++)
			{
				ownerCenter = ownerRigidbody.UnitCenter;
				base.Position = ownerCenter + desiredOffset;
				yield return base.Wait(1);
			}
			this.Direction = -90f;
			ownerRigidbody.CanCarry = true;
			this.Projectile.specRigidbody.CanBeCarried = true;
			ownerRigidbody.RegisterCarriedRigidbody(this.Projectile.specRigidbody);
			base.DisableMotion = true;
			int shootInterval = UnityEngine.Random.Range(120, 241);
			for (int k = this.m_spawnTime; k < 900; k++)
			{
				base.Position = this.Projectile.specRigidbody.Position.UnitPosition;
				shootInterval--;
				if (shootInterval <= 0)
				{
					float aimDirection = base.GetAimDirection(UnityEngine.Random.value, 12f);
					base.Fire(new Offset(-0.25f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("laserBlast", false, false, false));
					base.ChangeDirection(new Direction(aimDirection, DirectionType.Absolute, -1f), 5);
					shootInterval = UnityEngine.Random.Range(120, 241);
				}
				yield return base.Wait(1);
			}
			base.ChangeDirection(new Direction(-90f, DirectionType.Absolute, -1f), 5);
			yield return base.Wait(5);
			ownerRigidbody.DeregisterCarriedRigidbody(this.Projectile.specRigidbody);
			base.ManualControl = false;
			base.DisableMotion = false;
			this.Direction = -90f;
			this.Speed = 0f;
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 30);
			yield return base.Wait(180);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040002BD RID: 701
		private float m_desiredXOffset;

		// Token: 0x040002BE RID: 702
		private float m_desiredYOffset;

		// Token: 0x040002BF RID: 703
		private int m_spawnTime;
	}
}
