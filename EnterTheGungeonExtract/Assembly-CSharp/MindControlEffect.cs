using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E2E RID: 3630
public class MindControlEffect : MonoBehaviour
{
	// Token: 0x06004CC6 RID: 19654 RVA: 0x001A3E3C File Offset: 0x001A203C
	private void Start()
	{
		this.m_aiActor = base.GetComponent<AIActor>();
		this.m_behaviorSpeculator = this.m_aiActor.behaviorSpeculator;
		GameObject gameObject = new GameObject("fake target");
		this.m_fakeActor = gameObject.AddComponent<NonActor>();
		this.m_fakeActor.HasShadow = false;
		this.m_fakeTargetRigidbody = gameObject.AddComponent<SpeculativeRigidbody>();
		this.m_fakeTargetRigidbody.PixelColliders = new List<PixelCollider>();
		this.m_fakeTargetRigidbody.CollideWithTileMap = false;
		this.m_fakeTargetRigidbody.CollideWithOthers = false;
		this.m_fakeTargetRigidbody.CanBeCarried = false;
		this.m_fakeTargetRigidbody.CanBePushed = false;
		this.m_fakeTargetRigidbody.CanCarry = false;
		PixelCollider pixelCollider = new PixelCollider();
		pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
		pixelCollider.CollisionLayer = CollisionLayer.TileBlocker;
		pixelCollider.ManualWidth = 4;
		pixelCollider.ManualHeight = 4;
		this.m_fakeTargetRigidbody.PixelColliders.Add(pixelCollider);
		this.m_cable = this.m_aiActor.gameObject.AddComponent<ArbitraryCableDrawer>();
		this.m_cable.Attach1Offset = this.owner.CenterPosition - this.owner.transform.position.XY();
		this.m_cable.Attach2Offset = this.m_aiActor.CenterPosition - this.m_aiActor.transform.position.XY();
		this.m_cable.Initialize(this.owner.transform, this.m_aiActor.transform);
		this.m_overheadVFX = this.m_aiActor.PlayEffectOnActor((GameObject)ResourceCache.Acquire("Global VFX/VFX_Controller_Status"), new Vector3(0f, this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitDimensions.y, 0f), true, false, true);
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x001A3FFC File Offset: 0x001A21FC
	private Vector2 GetPlayerAimPointController(Vector2 aimBase, Vector2 aimDirection)
	{
		Func<SpeculativeRigidbody, bool> func = (SpeculativeRigidbody otherRigidbody) => otherRigidbody.minorBreakable && !otherRigidbody.minorBreakable.stopsBullets;
		Vector2 vector = aimBase + aimDirection * 10f;
		CollisionLayer collisionLayer = CollisionLayer.EnemyHitBox;
		int num = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, collisionLayer, CollisionLayer.BulletBreakable);
		RaycastResult raycastResult;
		if (PhysicsEngine.Instance.Raycast(aimBase, aimDirection, 50f, out raycastResult, true, true, num, null, false, func, null))
		{
			vector = aimBase + aimDirection * raycastResult.Distance;
		}
		RaycastResult.Pool.Free(ref raycastResult);
		return vector;
	}

	// Token: 0x06004CC8 RID: 19656 RVA: 0x001A4090 File Offset: 0x001A2290
	private void UpdateAimTargetPosition()
	{
		PlayerController playerController = this.owner;
		BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(playerController.PlayerIDX);
		GungeonActions activeActions = instanceForPlayer.ActiveActions;
		if (instanceForPlayer.IsKeyboardAndMouse(false))
		{
			this.m_fakeTargetRigidbody.transform.position = playerController.unadjustedAimPoint.XY();
		}
		else
		{
			this.m_fakeTargetRigidbody.transform.position = this.GetPlayerAimPointController(playerController.CenterPosition, activeActions.Aim.Vector);
		}
		this.m_fakeTargetRigidbody.Reinitialize();
	}

	// Token: 0x06004CC9 RID: 19657 RVA: 0x001A4120 File Offset: 0x001A2320
	private void Update()
	{
		this.m_fakeActor.specRigidbody = this.m_fakeTargetRigidbody;
		if (this.m_aiActor)
		{
			this.m_aiActor.CanTargetEnemies = true;
			this.m_aiActor.CanTargetPlayers = false;
			this.m_aiActor.PlayerTarget = this.m_fakeActor;
			this.m_aiActor.OverrideTarget = null;
			this.UpdateAimTargetPosition();
			if (this.m_aiActor.aiShooter)
			{
				this.m_aiActor.aiShooter.AimAtPoint(this.m_behaviorSpeculator.PlayerTarget.CenterPosition);
			}
		}
		if (this.m_behaviorSpeculator)
		{
			PlayerController playerController = this.owner;
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(playerController.PlayerIDX);
			GungeonActions activeActions = instanceForPlayer.ActiveActions;
			if (this.m_behaviorSpeculator.AttackCooldown <= 0f)
			{
				if (!this.m_attackedThisCycle && this.m_behaviorSpeculator.ActiveContinuousAttackBehavior != null)
				{
					this.m_attackedThisCycle = true;
				}
				if (this.m_attackedThisCycle && this.m_behaviorSpeculator.ActiveContinuousAttackBehavior == null)
				{
					this.m_behaviorSpeculator.AttackCooldown = float.MaxValue;
				}
			}
			else if (activeActions.ShootAction.WasPressed)
			{
				this.m_attackedThisCycle = false;
				this.m_behaviorSpeculator.AttackCooldown = 0f;
			}
			if (this.m_behaviorSpeculator.TargetBehaviors != null && this.m_behaviorSpeculator.TargetBehaviors.Count > 0)
			{
				this.m_behaviorSpeculator.TargetBehaviors.Clear();
			}
			if (this.m_behaviorSpeculator.MovementBehaviors != null && this.m_behaviorSpeculator.MovementBehaviors.Count > 0)
			{
				this.m_behaviorSpeculator.MovementBehaviors.Clear();
			}
			this.m_aiActor.ImpartedVelocity += activeActions.Move.Value * this.m_aiActor.MovementSpeed;
			if (this.m_behaviorSpeculator.AttackBehaviors != null)
			{
				for (int i = 0; i < this.m_behaviorSpeculator.AttackBehaviors.Count; i++)
				{
					AttackBehaviorBase attackBehaviorBase = this.m_behaviorSpeculator.AttackBehaviors[i];
					this.ProcessAttack(attackBehaviorBase);
				}
			}
		}
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x001A4360 File Offset: 0x001A2560
	private void ProcessAttack(AttackBehaviorBase attack)
	{
		if (attack == null)
		{
			return;
		}
		if (attack is BasicAttackBehavior)
		{
			BasicAttackBehavior basicAttackBehavior = attack as BasicAttackBehavior;
			basicAttackBehavior.Cooldown = 0f;
			basicAttackBehavior.RequiresLineOfSight = false;
			basicAttackBehavior.MinRange = -1f;
			basicAttackBehavior.Range = -1f;
			if (attack is TeleportBehavior)
			{
				basicAttackBehavior.RequiresLineOfSight = true;
				basicAttackBehavior.MinRange = 1000f;
				basicAttackBehavior.Range = 0.1f;
			}
			if (basicAttackBehavior is ShootGunBehavior)
			{
				ShootGunBehavior shootGunBehavior = basicAttackBehavior as ShootGunBehavior;
				shootGunBehavior.LineOfSight = false;
				shootGunBehavior.EmptiesClip = false;
				shootGunBehavior.RespectReload = false;
			}
		}
		else if (attack is AttackBehaviorGroup)
		{
			AttackBehaviorGroup attackBehaviorGroup = attack as AttackBehaviorGroup;
			for (int i = 0; i < attackBehaviorGroup.AttackBehaviors.Count; i++)
			{
				this.ProcessAttack(attackBehaviorGroup.AttackBehaviors[i].Behavior);
			}
		}
	}

	// Token: 0x040042DD RID: 17117
	[NonSerialized]
	public PlayerController owner;

	// Token: 0x040042DE RID: 17118
	private AIActor m_aiActor;

	// Token: 0x040042DF RID: 17119
	private BehaviorSpeculator m_behaviorSpeculator;

	// Token: 0x040042E0 RID: 17120
	private bool m_attackedThisCycle = true;

	// Token: 0x040042E1 RID: 17121
	private NonActor m_fakeActor;

	// Token: 0x040042E2 RID: 17122
	private SpeculativeRigidbody m_fakeTargetRigidbody;

	// Token: 0x040042E3 RID: 17123
	private ArbitraryCableDrawer m_cable;

	// Token: 0x040042E4 RID: 17124
	private GameObject m_overheadVFX;
}
