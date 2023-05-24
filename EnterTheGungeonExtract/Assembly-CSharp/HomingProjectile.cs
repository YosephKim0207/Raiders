using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200164D RID: 5709
public class HomingProjectile : Projectile
{
	// Token: 0x0600854F RID: 34127 RVA: 0x003701CC File Offset: 0x0036E3CC
	protected override void Move()
	{
		if (this.stopTrackingIfLeaveRadius)
		{
			this.nearestEnemy = null;
		}
		if (this.nearestEnemy == null)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
			this.nearestEnemy = BraveUtility.GetClosestToPosition<AIActor>(absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), base.transform.position.XY(), new AIActor[0]);
		}
		if (this.nearestEnemy != null)
		{
			Vector3 vector = this.nearestEnemy.transform.position - base.transform.position;
			float num = (Mathf.Atan2(vector.y, vector.x) - Mathf.Atan2(base.specRigidbody.Velocity.y, base.specRigidbody.Velocity.x)) * 57.29578f;
			float num2 = Mathf.Min(Mathf.Abs(num), this.trackingSpeed * BraveTime.DeltaTime) * Mathf.Sign(num);
			base.transform.Rotate(0f, 0f, num2);
		}
		base.specRigidbody.Velocity = base.transform.right * this.baseData.speed;
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008550 RID: 34128 RVA: 0x00370330 File Offset: 0x0036E530
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400897B RID: 35195
	public float detectionRange = 5f;

	// Token: 0x0400897C RID: 35196
	public float trackingSpeed = 5f;

	// Token: 0x0400897D RID: 35197
	public bool stopTrackingIfLeaveRadius;

	// Token: 0x0400897E RID: 35198
	private AIActor nearestEnemy;
}
