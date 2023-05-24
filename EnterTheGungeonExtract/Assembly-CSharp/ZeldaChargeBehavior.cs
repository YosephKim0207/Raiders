using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D72 RID: 3442
public class ZeldaChargeBehavior : BasicAttackBehavior
{
	// Token: 0x060048B6 RID: 18614 RVA: 0x00183224 File Offset: 0x00181424
	public override void Start()
	{
		base.Start();
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		this.m_cachedKnockback = this.m_aiActor.CollisionKnockbackStrength;
		this.m_cachedDamage = this.m_aiActor.CollisionDamage;
		this.m_cachedVfx = this.m_aiActor.CollisionVFX;
		this.m_cachedPathableTiles = this.m_aiActor.PathableTiles;
		this.m_cachedDoDustUps = this.m_aiActor.DoDustUps;
		if (this.switchCollidersOnCharge)
		{
			for (int i = 0; i < this.m_aiActor.specRigidbody.PixelColliders.Count; i++)
			{
				PixelCollider pixelCollider = this.m_aiActor.specRigidbody.PixelColliders[i];
				if (pixelCollider.CollisionLayer == CollisionLayer.EnemyCollider)
				{
					this.m_enemyCollider = pixelCollider;
				}
				if (pixelCollider.CollisionLayer == CollisionLayer.EnemyHitBox)
				{
					this.m_enemyHitbox = pixelCollider;
				}
				if (!pixelCollider.Enabled && pixelCollider.CollisionLayer == CollisionLayer.Projectile)
				{
					this.m_projectileCollider = pixelCollider;
					this.m_projectileCollider.CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
				}
			}
		}
		if (this.stopAtPits)
		{
			SpeculativeRigidbody specRigidbody2 = this.m_aiActor.specRigidbody;
			specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PitMovementRestrictor));
		}
		if (!string.IsNullOrEmpty(this.primeAnim))
		{
			this.m_primeAnimTime = this.m_aiAnimator.GetDirectionalAnimationLength(this.primeAnim);
		}
		this.m_aiActor.OverrideHitEnemies = true;
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x001833C8 File Offset: 0x001815C8
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x001833D0 File Offset: 0x001815D0
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && !playerController.healthHaver.IsDead && !playerController.IsFalling)
			{
				if (this.ShouldChargePlayer(GameManager.Instance.AllPlayers[i]))
				{
					this.State = ZeldaChargeBehavior.FireState.Priming;
					this.m_updateEveryFrame = true;
					return BehaviorResult.RunContinuous;
				}
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x060048B9 RID: 18617 RVA: 0x00183480 File Offset: 0x00181680
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.State == ZeldaChargeBehavior.FireState.Priming)
		{
			if (!this.m_aiAnimator.IsPlaying(this.primeAnim))
			{
				if (!this.m_aiActor.TargetRigidbody)
				{
					return ContinuousBehaviorResult.Finished;
				}
				this.State = ZeldaChargeBehavior.FireState.Charging;
			}
		}
		else if (this.State == ZeldaChargeBehavior.FireState.Charging)
		{
			if (this.endWhenChargeAnimFinishes && !this.m_aiAnimator.IsPlaying(this.chargeAnim))
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.State == ZeldaChargeBehavior.FireState.Bouncing && !this.m_aiAnimator.IsPlaying(this.hitAnim) && !this.m_aiAnimator.IsPlaying(this.hitPlayerAnim))
		{
			if (this.delayWallRecoil && this.m_storedCollisionNormal != null)
			{
				this.m_aiActor.knockbackDoer.ApplyKnockback(this.m_storedCollisionNormal.Value, this.wallRecoilForce, false);
				this.m_storedCollisionNormal = null;
			}
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x00183590 File Offset: 0x00181790
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.State = ZeldaChargeBehavior.FireState.Idle;
		this.UpdateCooldowns();
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x001835AC File Offset: 0x001817AC
	public override void Destroy()
	{
		base.Destroy();
		if (this.stopAtPits)
		{
			SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
			specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PitMovementRestrictor));
		}
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x001835EC File Offset: 0x001817EC
	private void OnCollision(CollisionData collisionData)
	{
		if (this.State == ZeldaChargeBehavior.FireState.Charging)
		{
			if (collisionData.OtherRigidbody)
			{
				SpeculativeRigidbody otherRigidbody = collisionData.OtherRigidbody;
				if (otherRigidbody.projectile)
				{
					return;
				}
				if (otherRigidbody.aiActor)
				{
					if (!otherRigidbody.aiActor.OverrideHitEnemies)
					{
						collisionData.OtherRigidbody.RegisterTemporaryCollisionException(collisionData.MyRigidbody, 0.1f, null);
						collisionData.MyRigidbody.RegisterTemporaryCollisionException(collisionData.OtherRigidbody, 0.1f, null);
						return;
					}
					float num = collisionData.MyRigidbody.Velocity.ToAngle();
					float num2 = collisionData.Normal.ToAngle();
					if (Mathf.Abs(BraveMathCollege.ClampAngle180(num - num2)) <= 91f)
					{
						return;
					}
					float magnitude = collisionData.MyRigidbody.Velocity.magnitude;
					float magnitude2 = otherRigidbody.Velocity.magnitude;
					float num3 = otherRigidbody.Velocity.ToAngle();
					if (Mathf.Abs(BraveMathCollege.ClampAngle180(num - num3)) < 45f && magnitude < magnitude2 * 1.25f)
					{
						return;
					}
				}
			}
			this.m_hitPlayer = collisionData.OtherRigidbody && collisionData.OtherRigidbody.GetComponent<PlayerController>();
			this.m_hitWall = collisionData.collisionType == CollisionData.CollisionType.TileMap;
			this.State = ZeldaChargeBehavior.FireState.Bouncing;
			if (!collisionData.OtherRigidbody || !collisionData.OtherRigidbody.knockbackDoer)
			{
				if (this.delayWallRecoil)
				{
					this.m_storedCollisionNormal = new Vector2?(collisionData.Normal);
					if (collisionData.Normal == Vector2.zero)
					{
						Vector2? chargeDir = this.m_chargeDir;
						this.m_storedCollisionNormal = ((chargeDir == null) ? null : new Vector2?(-chargeDir.Value));
					}
				}
				else
				{
					this.m_storedCollisionNormal = null;
					this.m_aiActor.knockbackDoer.ApplyKnockback(collisionData.Normal, this.wallRecoilForce, false);
				}
			}
			else
			{
				this.m_storedCollisionNormal = null;
			}
			if (!collisionData.OtherRigidbody && !string.IsNullOrEmpty(this.hitWallVfxString))
			{
				string text;
				if (this.m_storedCollisionNormal.Value.x < -0.75f)
				{
					text = "right";
				}
				else if (this.m_storedCollisionNormal.Value.x > 0.75f)
				{
					text = "left";
				}
				else if (this.m_storedCollisionNormal.Value.y < -0.75f)
				{
					text = "up";
				}
				else
				{
					text = "down";
				}
				this.m_aiAnimator.PlayVfx(string.Format(this.hitWallVfxString, text), null, null, null);
			}
		}
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x0018390C File Offset: 0x00181B0C
	private void PitMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		Func<IntVector2, bool> func = delegate(IntVector2 pixel)
		{
			Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(pixel);
			if (!GameManager.Instance.Dungeon.CellSupportsFalling(vector))
			{
				return false;
			}
			List<SpeculativeRigidbody> platformsAt = GameManager.Instance.Dungeon.GetPlatformsAt(vector);
			if (platformsAt != null)
			{
				for (int i = 0; i < platformsAt.Count; i++)
				{
					if (platformsAt[i].PrimaryPixelCollider.ContainsPixel(pixel))
					{
						return false;
					}
				}
			}
			return true;
		};
		PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
		if (primaryPixelCollider != null)
		{
			IntVector2 intVector = pixelOffset - prevPixelOffset;
			if (intVector == IntVector2.Down && func(primaryPixelCollider.LowerLeft + pixelOffset) && func(primaryPixelCollider.LowerRight + pixelOffset) && (!func(primaryPixelCollider.UpperRight + prevPixelOffset) || !func(primaryPixelCollider.UpperLeft + prevPixelOffset)))
			{
				validLocation = false;
				return;
			}
			if (intVector == IntVector2.Right && func(primaryPixelCollider.LowerRight + pixelOffset) && func(primaryPixelCollider.UpperRight + pixelOffset) && (!func(primaryPixelCollider.UpperLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerLeft + prevPixelOffset)))
			{
				validLocation = false;
				return;
			}
			if (intVector == IntVector2.Up && func(primaryPixelCollider.UpperRight + pixelOffset) && func(primaryPixelCollider.UpperLeft + pixelOffset) && (!func(primaryPixelCollider.LowerLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerRight + prevPixelOffset)))
			{
				validLocation = false;
				return;
			}
			if (intVector == IntVector2.Left && func(primaryPixelCollider.UpperLeft + pixelOffset) && func(primaryPixelCollider.LowerLeft + pixelOffset) && (!func(primaryPixelCollider.LowerRight + prevPixelOffset) || !func(primaryPixelCollider.UpperRight + prevPixelOffset)))
			{
				validLocation = false;
				return;
			}
		}
	}

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x060048BE RID: 18622 RVA: 0x00183B1C File Offset: 0x00181D1C
	// (set) Token: 0x060048BF RID: 18623 RVA: 0x00183B24 File Offset: 0x00181D24
	private ZeldaChargeBehavior.FireState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x060048C0 RID: 18624 RVA: 0x00183B54 File Offset: 0x00181D54
	private void BeginState(ZeldaChargeBehavior.FireState state)
	{
		if (state == ZeldaChargeBehavior.FireState.Idle)
		{
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_aiAnimator.LockFacingDirection = false;
		}
		else if (state == ZeldaChargeBehavior.FireState.Priming)
		{
			this.m_aiAnimator.PlayUntilFinished(this.primeAnim, true, null, -1f, false);
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
		}
		else if (state == ZeldaChargeBehavior.FireState.Charging)
		{
			AkSoundEngine.PostEvent("Play_ENM_cube_dash_01", GameManager.Instance.PrimaryPlayer.gameObject);
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = this.m_chargeDir.Value.normalized * this.chargeSpeed;
			float num = this.m_aiActor.BehaviorVelocity.ToAngle();
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.FacingDirection = num;
			this.m_aiActor.CollisionKnockbackStrength = this.chargeKnockback;
			this.m_aiActor.CollisionDamage = this.chargeDamage;
			if (this.hitVfx)
			{
				VFXObject vfxobject = new VFXObject();
				vfxobject.effect = this.hitVfx;
				VFXComplex vfxcomplex = new VFXComplex();
				vfxcomplex.effects = new VFXObject[] { vfxobject };
				VFXPool vfxpool = new VFXPool();
				vfxpool.type = VFXPoolType.Single;
				vfxpool.effects = new VFXComplex[] { vfxcomplex };
				this.m_aiActor.CollisionVFX = vfxpool;
			}
			this.m_aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
			if (this.switchCollidersOnCharge)
			{
				this.m_enemyCollider.CollisionLayer = CollisionLayer.TileBlocker;
				this.m_enemyHitbox.Enabled = false;
				this.m_projectileCollider.Enabled = true;
			}
			this.m_aiActor.DoDustUps = false;
			this.m_aiAnimator.PlayUntilFinished(this.chargeAnim, true, null, -1f, false);
			if (this.launchVfx)
			{
				SpawnManager.SpawnVFX(this.launchVfx, this.m_aiActor.specRigidbody.UnitCenter, Quaternion.identity);
			}
			if (this.trailVfx)
			{
				this.m_trailVfx = SpawnManager.SpawnParticleSystem(this.trailVfx, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, num));
				if (this.trailVfxParent)
				{
					this.m_trailVfx.transform.parent = this.trailVfxParent;
				}
				else
				{
					this.m_trailVfx.transform.parent = this.m_aiActor.transform;
				}
				ParticleKiller component = this.m_trailVfx.GetComponent<ParticleKiller>();
				if (component != null)
				{
					component.Awake();
				}
			}
			if (!string.IsNullOrEmpty(this.trailVfxString))
			{
				Vector2 normalized = this.m_aiActor.BehaviorVelocity.normalized;
				string text;
				if (normalized.x > 0.75f)
				{
					text = "right";
				}
				else if (normalized.x < -0.75f)
				{
					text = "left";
				}
				else if (normalized.y > 0.75f)
				{
					text = "up";
				}
				else
				{
					text = "down";
				}
				this.m_cachedTrailString = string.Format(this.trailVfxString, text);
				AIAnimator aiAnimator = this.m_aiAnimator;
				string cachedTrailString = this.m_cachedTrailString;
				Vector2? vector = new Vector2?(normalized);
				aiAnimator.PlayVfx(cachedTrailString, null, vector, null);
			}
			else
			{
				this.m_cachedTrailString = null;
			}
			this.m_aiActor.specRigidbody.ForceRegenerate(null, null);
		}
		else if (state == ZeldaChargeBehavior.FireState.Bouncing)
		{
			if (!string.IsNullOrEmpty(this.hitPlayerAnim) && this.m_hitPlayer)
			{
				this.m_aiAnimator.PlayUntilFinished(this.hitPlayerAnim, true, null, -1f, false);
				if (this.m_aiAnimator.spriteAnimator.CurrentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop)
				{
					this.m_aiAnimator.PlayForDuration(this.hitPlayerAnim, 1f, true, null, -1f, false);
				}
			}
			else
			{
				this.m_aiAnimator.PlayUntilFinished(this.hitAnim, true, null, -1f, false);
			}
			if (this.impactBulletScript != null && !this.impactBulletScript.IsNull && this.m_hitWall)
			{
				if (!this.m_bulletSource)
				{
					this.m_bulletSource = this.shootPoint.GetOrAddComponent<BulletScriptSource>();
				}
				this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
				this.m_bulletSource.BulletScript = this.impactBulletScript;
				this.m_bulletSource.Initialize();
			}
		}
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x00184030 File Offset: 0x00182230
	private void EndState(ZeldaChargeBehavior.FireState state)
	{
		if (state == ZeldaChargeBehavior.FireState.Charging)
		{
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiActor.CollisionKnockbackStrength = this.m_cachedKnockback;
			this.m_aiActor.CollisionDamage = this.m_cachedDamage;
			this.m_aiActor.CollisionVFX = this.m_cachedVfx;
			if (this.m_trailVfx)
			{
				ParticleKiller component = this.m_trailVfx.GetComponent<ParticleKiller>();
				if (component)
				{
					component.StopEmitting();
				}
				else
				{
					SpawnManager.Despawn(this.m_trailVfx);
				}
				this.m_trailVfx = null;
			}
			if (!string.IsNullOrEmpty(this.m_cachedTrailString))
			{
				this.m_aiAnimator.StopVfx(this.m_cachedTrailString);
				this.m_cachedTrailString = null;
			}
			this.m_aiActor.DoDustUps = this.m_cachedDoDustUps;
			this.m_aiActor.PathableTiles = this.m_cachedPathableTiles;
			if (this.switchCollidersOnCharge)
			{
				this.m_enemyCollider.CollisionLayer = CollisionLayer.EnemyCollider;
				this.m_enemyHitbox.Enabled = true;
				this.m_projectileCollider.Enabled = false;
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_aiActor.specRigidbody, null, false);
			}
		}
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x00184164 File Offset: 0x00182364
	private bool ShouldChargePlayer(PlayerController player)
	{
		Vector2 vector = player.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (this.leadAmount > 0f)
		{
			Vector2 vector2 = vector + player.specRigidbody.Velocity * this.m_primeAnimTime;
			vector = Vector2.Lerp(vector, vector2, this.leadAmount);
		}
		Vector2 unitBottomLeft = this.m_aiActor.specRigidbody.UnitBottomLeft;
		Vector2 unitTopRight = this.m_aiActor.specRigidbody.UnitTopRight;
		this.m_chargeDir = null;
		if (BraveMathCollege.AABBContains(new Vector2(unitBottomLeft.x - this.chargeRange, unitBottomLeft.y), unitTopRight, vector))
		{
			this.m_chargeDir = new Vector2?(-Vector2.right);
		}
		else if (BraveMathCollege.AABBContains(unitBottomLeft, new Vector2(unitTopRight.x + this.chargeRange, unitTopRight.y), vector))
		{
			this.m_chargeDir = new Vector2?(Vector2.right);
		}
		else if (BraveMathCollege.AABBContains(new Vector2(unitBottomLeft.x, unitBottomLeft.y - this.chargeRange), unitTopRight, vector))
		{
			this.m_chargeDir = new Vector2?(-Vector2.up);
		}
		else if (BraveMathCollege.AABBContains(unitBottomLeft, new Vector2(unitTopRight.x, unitTopRight.y + this.chargeRange), vector))
		{
			this.m_chargeDir = new Vector2?(Vector2.up);
		}
		return this.m_chargeDir != null;
	}

	// Token: 0x04003CBD RID: 15549
	public string primeAnim;

	// Token: 0x04003CBE RID: 15550
	public string chargeAnim;

	// Token: 0x04003CBF RID: 15551
	public bool endWhenChargeAnimFinishes;

	// Token: 0x04003CC0 RID: 15552
	public bool switchCollidersOnCharge;

	// Token: 0x04003CC1 RID: 15553
	public string hitAnim;

	// Token: 0x04003CC2 RID: 15554
	public string hitPlayerAnim;

	// Token: 0x04003CC3 RID: 15555
	public float leadAmount;

	// Token: 0x04003CC4 RID: 15556
	public float chargeRange = 15f;

	// Token: 0x04003CC5 RID: 15557
	public float chargeSpeed;

	// Token: 0x04003CC6 RID: 15558
	public float chargeKnockback = 50f;

	// Token: 0x04003CC7 RID: 15559
	public float chargeDamage = 0.5f;

	// Token: 0x04003CC8 RID: 15560
	public bool delayWallRecoil;

	// Token: 0x04003CC9 RID: 15561
	public float wallRecoilForce = 10f;

	// Token: 0x04003CCA RID: 15562
	public bool stopAtPits = true;

	// Token: 0x04003CCB RID: 15563
	public GameObject launchVfx;

	// Token: 0x04003CCC RID: 15564
	public GameObject trailVfx;

	// Token: 0x04003CCD RID: 15565
	public Transform trailVfxParent;

	// Token: 0x04003CCE RID: 15566
	public GameObject hitVfx;

	// Token: 0x04003CCF RID: 15567
	public string trailVfxString;

	// Token: 0x04003CD0 RID: 15568
	public string hitWallVfxString;

	// Token: 0x04003CD1 RID: 15569
	[InspectorHeader("Impact BulletScript")]
	public GameObject shootPoint;

	// Token: 0x04003CD2 RID: 15570
	public BulletScriptSelector impactBulletScript;

	// Token: 0x04003CD3 RID: 15571
	private ZeldaChargeBehavior.FireState m_state;

	// Token: 0x04003CD4 RID: 15572
	private float m_primeAnimTime;

	// Token: 0x04003CD5 RID: 15573
	private Vector2? m_chargeDir;

	// Token: 0x04003CD6 RID: 15574
	private Vector2? m_storedCollisionNormal;

	// Token: 0x04003CD7 RID: 15575
	private bool m_hitPlayer;

	// Token: 0x04003CD8 RID: 15576
	private bool m_hitWall;

	// Token: 0x04003CD9 RID: 15577
	private float m_cachedKnockback;

	// Token: 0x04003CDA RID: 15578
	private float m_cachedDamage;

	// Token: 0x04003CDB RID: 15579
	private VFXPool m_cachedVfx;

	// Token: 0x04003CDC RID: 15580
	private CellTypes m_cachedPathableTiles;

	// Token: 0x04003CDD RID: 15581
	private bool m_cachedDoDustUps;

	// Token: 0x04003CDE RID: 15582
	private PixelCollider m_enemyCollider;

	// Token: 0x04003CDF RID: 15583
	private PixelCollider m_enemyHitbox;

	// Token: 0x04003CE0 RID: 15584
	private PixelCollider m_projectileCollider;

	// Token: 0x04003CE1 RID: 15585
	private GameObject m_trailVfx;

	// Token: 0x04003CE2 RID: 15586
	private string m_cachedTrailString;

	// Token: 0x04003CE3 RID: 15587
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000D73 RID: 3443
	private enum FireState
	{
		// Token: 0x04003CE6 RID: 15590
		Idle,
		// Token: 0x04003CE7 RID: 15591
		Priming,
		// Token: 0x04003CE8 RID: 15592
		Charging,
		// Token: 0x04003CE9 RID: 15593
		Bouncing
	}
}
