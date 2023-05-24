using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200167E RID: 5758
public class SpawningProjectile : Projectile
{
	// Token: 0x06008653 RID: 34387 RVA: 0x00378FE4 File Offset: 0x003771E4
	public override void Start()
	{
		base.Start();
		this.m_current3DVelocity = (this.m_currentDirection * this.m_currentSpeed).ToVector3ZUp(0f);
	}

	// Token: 0x06008654 RID: 34388 RVA: 0x00379010 File Offset: 0x00377210
	protected override void Move()
	{
		this.m_kinematicTimer += BraveTime.DeltaTime;
		this.m_current3DVelocity.x = this.m_currentDirection.x;
		this.m_current3DVelocity.y = this.m_currentDirection.y;
		this.m_current3DVelocity.z = this.gravity * this.m_kinematicTimer;
		float num = this.startingHeight + 0.5f * this.gravity * this.m_kinematicTimer * this.m_kinematicTimer;
		if (num < 0f)
		{
			Vector2 unitCenter = base.specRigidbody.UnitCenter;
			IntVector2 intVector = unitCenter.ToIntVector2(VectorConversions.Floor);
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.enemyGuid);
			AIActor aiactor = AIActor.Spawn(orLoadByGuid, intVector, roomFromPosition, true, AIActor.AwakenAnimationType.Default, true);
			if (aiactor && aiactor.aiAnimator)
			{
				aiactor.aiAnimator.PlayDefaultSpawnState();
				this.hitEffects.HandleEnemyImpact(unitCenter, 0f, null, Vector2.zero, Vector2.zero, true, false);
			}
			if (this.IsBlackBullet && aiactor)
			{
				aiactor.ForceBlackPhantom = true;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.m_currentDirection = this.m_current3DVelocity.XY();
		Vector2 vector = this.m_current3DVelocity.XY().normalized * this.m_currentSpeed;
		base.specRigidbody.Velocity = new Vector2(vector.x, vector.y + this.m_current3DVelocity.z);
		base.LastVelocity = this.m_current3DVelocity.XY();
	}

	// Token: 0x06008655 RID: 34389 RVA: 0x003791C8 File Offset: 0x003773C8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008B2B RID: 35627
	public float startingHeight = 1f;

	// Token: 0x04008B2C RID: 35628
	public float gravity = -10f;

	// Token: 0x04008B2D RID: 35629
	[EnemyIdentifier]
	public string enemyGuid;

	// Token: 0x04008B2E RID: 35630
	private Vector3 m_current3DVelocity;

	// Token: 0x04008B2F RID: 35631
	private float m_kinematicTimer;
}
