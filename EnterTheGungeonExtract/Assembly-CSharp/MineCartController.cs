using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x020011A8 RID: 4520
public class MineCartController : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x060064A5 RID: 25765 RVA: 0x00270360 File Offset: 0x0026E560
	public float MaxSpeedEnemy
	{
		get
		{
			return this.MaxSpeed;
		}
	}

	// Token: 0x17000EE5 RID: 3813
	// (get) Token: 0x060064A6 RID: 25766 RVA: 0x00270368 File Offset: 0x0026E568
	public float MaxSpeedPlayer
	{
		get
		{
			return this.MaxSpeed;
		}
	}

	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x060064A7 RID: 25767 RVA: 0x00270370 File Offset: 0x0026E570
	public GameActor CurrentInhabitant
	{
		get
		{
			return this.m_rider;
		}
	}

	// Token: 0x060064A8 RID: 25768 RVA: 0x00270378 File Offset: 0x0026E578
	private void Awake()
	{
		if (this.carriedCargo != null && this.carriedCargo.specRigidbody != null)
		{
			base.specRigidbody.RegisterSpecificCollisionException(this.carriedCargo.specRigidbody);
			this.carriedCargo.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
			this.m_turret = this.carriedCargo.GetComponent<CartTurretController>();
		}
		this.m_pathMover = base.GetComponent<PathMover>();
		this.m_pathMover.ForceCornerDelayHack = true;
		if (base.majorBreakable)
		{
			MajorBreakable majorBreakable = base.majorBreakable;
			majorBreakable.OnBreak = (Action)Delegate.Combine(majorBreakable.OnBreak, new Action(this.DestroyMineCart));
		}
	}

	// Token: 0x060064A9 RID: 25769 RVA: 0x00270438 File Offset: 0x0026E638
	private IEnumerator Start()
	{
		this.m_room = base.GetAbsoluteParentRoom();
		this.ForceActive |= this.AlwaysMoving;
		this.m_pathMover.nodeOffset = new Vector2(-0.5f, 0f);
		PathMover pathMover = this.m_pathMover;
		pathMover.OnNodeReached = (Action<Vector2, Vector2, bool>)Delegate.Combine(pathMover.OnNodeReached, new Action<Vector2, Vector2, bool>(this.HandleCornerReached));
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreRigidbodyCollision));
		base.specRigidbody.CollideWithTileMap = false;
		base.specRigidbody.ForceCarriesRigidbodies = true;
		yield return null;
		if (this.m_room.GetRoomName().Contains("BulletComponent"))
		{
			RoomHandler room = this.m_room;
			room.OnPlayerReturnedFromPit = (Action<PlayerController>)Delegate.Combine(room.OnPlayerReturnedFromPit, new Action<PlayerController>(this.HandlePlayerPitRespawn));
		}
		this.m_minecartsInRoom = this.m_room.GetComponentsAbsoluteInRoom<MineCartController>();
		base.specRigidbody.PixelColliders[0].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.LowObstacle);
		if (this.carriedCargo != null)
		{
			this.occupation = MineCartController.CartOccupationState.CARGO;
			this.BecomeCargoOccupied();
		}
		yield break;
	}

	// Token: 0x060064AA RID: 25770 RVA: 0x00270454 File Offset: 0x0026E654
	private bool IsOnlyMinecartInRoom()
	{
		return this.m_minecartsInRoom.Count == 1;
	}

	// Token: 0x060064AB RID: 25771 RVA: 0x00270464 File Offset: 0x0026E664
	private bool IsReachableFromPosition(Vector2 targetPoint)
	{
		Path path = new Path();
		Pathfinder.Instance.GetPath(targetPoint.ToIntVector2(VectorConversions.Floor), base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), out path, new IntVector2?(IntVector2.One), CellTypes.FLOOR, null, null, false);
		return path != null && path.WillReachFinalGoal;
	}

	// Token: 0x060064AC RID: 25772 RVA: 0x002704B8 File Offset: 0x0026E6B8
	private void HandlePlayerPitRespawn(PlayerController obj)
	{
		this.m_pathMover.WarpToStart();
	}

	// Token: 0x060064AD RID: 25773 RVA: 0x002704C8 File Offset: 0x0026E6C8
	private void WarpToNearestPointOnPath(Vector2 targetPoint)
	{
		this.m_pathMover.WarpToNearestPoint(targetPoint);
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
	}

	// Token: 0x060064AE RID: 25774 RVA: 0x002704FC File Offset: 0x0026E6FC
	private void HandlePitFall(Vector2 lastVec)
	{
		this.Evacuate(false, true);
		this.EvacuateSecondary(false, true);
		this.m_pathMover.Paused = true;
		IntVector2 intVector = lastVec.ToIntVector2(VectorConversions.Floor).MajorAxis * 2;
		base.StartCoroutine(this.StartFallAnimation(intVector, base.specRigidbody));
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x00270550 File Offset: 0x0026E750
	private IEnumerator StartFallAnimation(IntVector2 dir, SpeculativeRigidbody targetRigidbody)
	{
		targetRigidbody.enabled = false;
		targetRigidbody.sprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
		float duration = 0.5f;
		float rotation = ((dir.x == 0) ? 0f : (-Mathf.Sign((float)dir.x) * 135f));
		Vector3 velocity = dir.ToVector3() * 1.25f / duration;
		Vector3 acceleration = new Vector3(0f, -10f, 0f);
		float timer = 0f;
		while (timer < duration)
		{
			targetRigidbody.transform.position += velocity * BraveTime.DeltaTime;
			targetRigidbody.transform.eulerAngles = targetRigidbody.transform.eulerAngles.WithZ(Mathf.Lerp(0f, rotation, timer / duration));
			targetRigidbody.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, timer / duration);
			yield return null;
			timer += BraveTime.DeltaTime;
			velocity += acceleration * BraveTime.DeltaTime;
		}
		GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(targetRigidbody.transform.position);
		yield return null;
		targetRigidbody.transform.rotation = Quaternion.identity;
		targetRigidbody.transform.localScale = Vector3.one;
		targetRigidbody.enabled = true;
		targetRigidbody.sprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
		this.m_pathMover.Paused = false;
		this.m_pathMover.WarpToNearestPoint(this.m_pathMover.Path.nodes[0].RoomPosition + this.m_pathMover.nodeOffset + this.m_pathMover.RoomHandler.area.basePosition.ToVector2());
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		this.m_pathMover.ForcePathToNextNode();
		yield break;
	}

	// Token: 0x060064B0 RID: 25776 RVA: 0x0027057C File Offset: 0x0026E77C
	protected override void OnDestroy()
	{
		this.StopSound();
		base.OnDestroy();
	}

	// Token: 0x060064B1 RID: 25777 RVA: 0x0027058C File Offset: 0x0026E78C
	private void HandlePreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.minorBreakable && otherRigidbody.minorBreakable.isImpermeableToGameActors)
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (this.occupation == MineCartController.CartOccupationState.EMPTY || this.occupation == MineCartController.CartOccupationState.PLAYER)
		{
			if (otherRigidbody.gameActor is PlayerController)
			{
				PlayerController playerController = otherRigidbody.gameActor as PlayerController;
				if (playerController.IsDodgeRolling && playerController.previousMineCart != null && playerController.previousMineCart != this)
				{
					playerController.ForceStopDodgeRoll();
					playerController.ToggleGunRenderers(true, string.Empty);
					this.m_justRolledInTimer = 0.5f;
					this.BecomeOccupied(playerController);
				}
				else if (this.occupation != MineCartController.CartOccupationState.EMPTY || Mathf.Approximately(this.m_pathMover.PathSpeed, 0f))
				{
				}
			}
			else if (otherRigidbody.gameActor is AIActor)
			{
				AIActor aiactor = otherRigidbody.gameActor as AIActor;
				if (!aiactor.IsNormalEnemy)
				{
					PhysicsEngine.SkipCollision = true;
				}
			}
		}
	}

	// Token: 0x060064B2 RID: 25778 RVA: 0x002706A0 File Offset: 0x0026E8A0
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (!this.m_pathMover.Paused)
		{
			Vector2 vector = BraveMathCollege.VectorToCone(-rigidbodyCollision.Normal, 15f);
			AIActor aiActor = rigidbodyCollision.OtherRigidbody.aiActor;
			if (aiActor && aiActor.healthHaver && aiActor.healthHaver.IsAlive && this.CurrentInhabitant is PlayerController && base.specRigidbody.Velocity.magnitude > 2f)
			{
				aiActor.healthHaver.ApplyDamage(50f, vector, "Minecart Damage", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			if (rigidbodyCollision.OtherRigidbody.knockbackDoer != null)
			{
				if (rigidbodyCollision.OtherRigidbody.gameActor && rigidbodyCollision.OtherRigidbody.gameActor is PlayerController)
				{
					rigidbodyCollision.OtherRigidbody.knockbackDoer.ApplySourcedKnockback(vector, this.KnockbackStrengthPlayer * Mathf.Abs(this.m_pathMover.PathSpeed), base.gameObject, false);
				}
				else
				{
					rigidbodyCollision.OtherRigidbody.knockbackDoer.ApplySourcedKnockback(vector, this.KnockbackStrengthEnemy * Mathf.Abs(this.m_pathMover.PathSpeed), base.gameObject, false);
				}
				if (this.m_rider)
				{
					this.m_rider.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 1f, null);
				}
				if (this.m_secondaryRider)
				{
					this.m_secondaryRider.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 1f, null);
				}
				base.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 1f, null);
			}
		}
	}

	// Token: 0x060064B3 RID: 25779 RVA: 0x00270880 File Offset: 0x0026EA80
	private void Update()
	{
		if (!this.m_pathMover)
		{
			return;
		}
		bool flag = GameManager.Instance.PlayerIsNearRoom(this.m_room);
		bool flag2 = this.m_turret != null && this.m_turret.Inactive;
		this.m_justRolledInTimer -= BraveTime.DeltaTime;
		if (flag2 || (this.occupation == MineCartController.CartOccupationState.EMPTY && !this.ForceActive))
		{
			this.m_elapsedOccupied = 0f;
			this.m_elapsedSecondary = 0f;
			if (this.m_pathMover.PathSpeed != 0f)
			{
				this.m_pathMover.Paused = false;
				if (!this.m_wasPushedThisFrame)
				{
					this.m_pathMover.PathSpeed = Mathf.MoveTowards(this.m_pathMover.PathSpeed, 0f, 4f * BraveTime.DeltaTime);
				}
			}
			else if (!this.m_pathMover.Paused)
			{
				CellData cellData = GameManager.Instance.Dungeon.data[base.transform.position.IntXY(VectorConversions.Round)];
				if (cellData == null || cellData.type == CellType.WALL)
				{
					this.m_pathMover.PathSpeed = Mathf.Sign(this.m_pathMover.PathSpeed) * this.MaxSpeedEnemy;
				}
				this.m_pathMover.Paused = true;
			}
		}
		else if (this.occupation == MineCartController.CartOccupationState.CARGO)
		{
			if (flag)
			{
				if (this.m_pathMover.Paused)
				{
					this.m_pathMover.Paused = false;
				}
				float maxSpeedEnemy = this.MaxSpeedEnemy;
				this.m_pathMover.PathSpeed = BraveMathCollege.SmoothLerp(0f, maxSpeedEnemy, Mathf.Clamp01(this.m_elapsedOccupied / this.TimeToMaxSpeed));
				this.m_elapsedOccupied += BraveTime.DeltaTime;
				if (!this.carriedCargo)
				{
					this.occupation = MineCartController.CartOccupationState.EMPTY;
				}
			}
			else
			{
				this.m_pathMover.Paused = true;
			}
		}
		else
		{
			if (this.m_pathMover.Paused)
			{
				this.m_pathMover.Paused = false;
			}
			if (this.occupation == MineCartController.CartOccupationState.PLAYER)
			{
				this.m_pathMover.PathSpeed = Mathf.Clamp(this.m_pathMover.PathSpeed, -this.MaxSpeedPlayer, this.MaxSpeedPlayer);
			}
			else
			{
				float num = ((this.occupation != MineCartController.CartOccupationState.PLAYER) ? this.MaxSpeedEnemy : this.MaxSpeedPlayer);
				float num2 = Mathf.Max(this.m_elapsedOccupied, this.m_elapsedSecondary);
				this.m_pathMover.PathSpeed = BraveMathCollege.SmoothLerp(0f, num, Mathf.Clamp01(num2 / this.TimeToMaxSpeed));
				if (this.ForceActive)
				{
					this.m_pathMover.PathSpeed = this.MaxSpeedEnemy;
				}
			}
			if (this.m_rider != null)
			{
				this.m_elapsedOccupied += BraveTime.DeltaTime;
			}
			if (this.m_secondaryRider != null)
			{
				this.m_elapsedSecondary += BraveTime.DeltaTime;
			}
			if (!GameManager.Instance.IsPaused)
			{
				if (this.occupation == MineCartController.CartOccupationState.PLAYER)
				{
					this.HandlePlayerRiderInput(this.m_rider, this.m_elapsedOccupied);
					this.HandlePlayerRiderInput(this.m_secondaryRider, this.m_elapsedSecondary);
				}
				if (!this.m_rider || this.m_rider.healthHaver.IsDead)
				{
					this.Evacuate(false, false);
				}
				if (!this.m_secondaryRider || this.m_secondaryRider.healthHaver.IsDead)
				{
					this.EvacuateSecondary(false, false);
				}
			}
		}
		if (this.m_pathMover.AbsPathSpeed > 0f && flag)
		{
			this.StartSound();
		}
		else
		{
			this.StopSound();
		}
		if (this.m_cartSoundActive)
		{
			AkSoundEngine.SetRTPCValue("Pitch_Minecart", this.m_pathMover.AbsPathSpeed / this.MaxSpeed);
		}
		Vector2 vector = PhysicsEngine.PixelToUnit(base.specRigidbody.PathTarget) - base.specRigidbody.Position.UnitPosition;
		if (!this.m_hasHandledCornerAnimation && !this.m_handlingQueuedAnimation && vector.magnitude < 0.5f)
		{
			Vector2 vector2 = this.m_pathMover.GetNextTargetPosition() - PhysicsEngine.PixelToUnit(base.specRigidbody.PathTarget);
			this.m_hasHandledCornerAnimation = true;
			this.HandleTurnAnimation(vector, vector2);
		}
		this.HandlePushCarts();
		this.EnsureRiderPosition();
		this.m_wasPushedThisFrame = false;
		this.m_pusher = null;
		this.UpdateSparksTransforms();
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x00270D28 File Offset: 0x0026EF28
	private void StartSound()
	{
		if (!this.m_cartSoundActive)
		{
			this.m_cartSoundActive = true;
			AkSoundEngine.PostEvent("Play_OBJ_minecart_loop_01", base.gameObject);
		}
	}

	// Token: 0x060064B5 RID: 25781 RVA: 0x00270D50 File Offset: 0x0026EF50
	private void StopSound()
	{
		if (this.m_cartSoundActive)
		{
			this.m_cartSoundActive = false;
			AkSoundEngine.PostEvent("Stop_OBJ_minecart_loop_01", base.gameObject);
		}
	}

	// Token: 0x060064B6 RID: 25782 RVA: 0x00270D78 File Offset: 0x0026EF78
	private void UpdateSparksTransforms()
	{
		if (this.Sparks_A == null)
		{
			return;
		}
		Vector2 velocity = base.specRigidbody.Velocity;
		if (velocity.magnitude < 2f)
		{
			this.Sparks_A.gameObject.SetActive(false);
			this.Sparks_B.gameObject.SetActive(false);
		}
		else
		{
			this.Sparks_A.GetComponent<Renderer>().enabled = true;
			this.Sparks_B.GetComponent<Renderer>().enabled = true;
			this.Sparks_A.gameObject.SetActive(true);
			this.Sparks_B.gameObject.SetActive(true);
			if (velocity.IsHorizontal())
			{
				ParticleSystem componentInChildren = this.Sparks_A.GetComponentInChildren<ParticleSystem>();
				ParticleSystem componentInChildren2 = this.Sparks_B.GetComponentInChildren<ParticleSystem>();
				this.Sparks_A.localPosition = new Vector3(1.4375f, 0.375f, -1.125f);
				componentInChildren.transform.localRotation = Quaternion.Euler(-30f, -125.25f, 55f);
				this.Sparks_B.localPosition = new Vector3(0.5f, 0.375f, -1.125f);
				componentInChildren2.transform.localRotation = Quaternion.Euler(-30f, -125.25f, 55f);
				if (velocity.x < 0f)
				{
					this.Sparks_B.localPosition = new Vector3(1.4375f, 1.0625f, -0.4375f);
					this.Sparks_B.GetComponent<Renderer>().enabled = false;
					if (componentInChildren.simulationSpace == ParticleSystemSimulationSpace.Local)
					{
						componentInChildren.transform.localRotation = Quaternion.Euler(-10f, 90f, 0f);
						componentInChildren2.transform.localRotation = Quaternion.Euler(-10f, 90f, 0f);
					}
				}
				else
				{
					this.Sparks_A.localPosition = new Vector3(0.5f, 1.0625f, -0.4375f);
					this.Sparks_A.GetComponent<Renderer>().enabled = false;
					if (componentInChildren.simulationSpace == ParticleSystemSimulationSpace.Local)
					{
						componentInChildren.transform.localRotation = Quaternion.Euler(-10f, -125.25f, 55f);
						componentInChildren2.transform.localRotation = Quaternion.Euler(-10f, -125.25f, 55f);
					}
				}
			}
			else
			{
				this.Sparks_A.localPosition = new Vector3(0.625f, 0.125f, -1.375f);
				this.Sparks_A.GetComponentInChildren<ParticleSystem>().transform.localRotation = Quaternion.Euler(-45f, 0f, -45f);
				this.Sparks_B.localPosition = new Vector3(1.3125f, 0.125f, -1.375f);
				this.Sparks_B.GetComponentInChildren<ParticleSystem>().transform.localRotation = Quaternion.Euler(-45f, 0f, -45f);
				if (velocity.y <= 0f)
				{
					this.Sparks_A.GetComponent<Renderer>().enabled = false;
					this.Sparks_B.GetComponent<Renderer>().enabled = false;
				}
			}
		}
	}

	// Token: 0x060064B7 RID: 25783 RVA: 0x00271094 File Offset: 0x0026F294
	public void ApplyVelocity(float speed)
	{
		if (this.m_pathMover == null)
		{
			this.m_pathMover = base.GetComponent<PathMover>();
		}
		this.m_pathMover.Paused = false;
		this.m_pathMover.PathSpeed = Mathf.Max(this.MaxSpeedEnemy, this.m_pathMover.PathSpeed + speed);
	}

	// Token: 0x060064B8 RID: 25784 RVA: 0x002710F0 File Offset: 0x0026F2F0
	protected void HandlePushCarts()
	{
		if (this.m_pathMover.PathSpeed == 0f)
		{
			return;
		}
		MineCartController mineCartController = this.CheckWillHitMineCart();
		if (mineCartController != null && this.m_pathMover.AbsPathSpeed / this.MaxSpeed < 0.3f)
		{
			this.m_pathMover.PathSpeed = 0f;
			return;
		}
		float num = Mathf.Min(this.m_pathMover.AbsPathSpeed, this.MaxSpeedPlayer);
		float num2 = num + 1f;
		num2 *= Mathf.Sign(this.m_pathMover.PathSpeed);
		if (mineCartController != null)
		{
			bool flag = Mathf.Abs(num2) > Mathf.Abs(mineCartController.m_pathMover.PathSpeed) || Mathf.Sign(num2) != Mathf.Sign(mineCartController.m_pathMover.PathSpeed);
			if (flag)
			{
				float parametrizedPathPosition = mineCartController.m_pathMover.GetParametrizedPathPosition();
				float parametrizedPathPosition2 = this.m_pathMover.GetParametrizedPathPosition();
				if ((this.m_pathMover.PathSpeed <= 0f || parametrizedPathPosition <= parametrizedPathPosition2) && (parametrizedPathPosition >= 0.25f || parametrizedPathPosition2 <= 0.75f) && (this.m_pathMover.PathSpeed >= 0f || parametrizedPathPosition >= parametrizedPathPosition2) && (parametrizedPathPosition <= 0.75f || parametrizedPathPosition2 >= 0.25f))
				{
					return;
				}
				mineCartController.m_pathMover.Paused = false;
				mineCartController.m_pathMover.PathSpeed = num2;
				mineCartController.m_wasPushedThisFrame = true;
				mineCartController.m_pusher = base.specRigidbody;
				mineCartController.HandlePushCarts();
			}
		}
	}

	// Token: 0x060064B9 RID: 25785 RVA: 0x00271298 File Offset: 0x0026F498
	protected MineCartController CheckWillHitMineCart()
	{
		MineCartController mineCartController = null;
		this.m_cachedCollisionList.Clear();
		IntVector2 intVector = (PhysicsEngine.UnitToPixel((PhysicsEngine.PixelToUnit(base.specRigidbody.PathTarget) - base.specRigidbody.Position.UnitPosition).normalized * base.specRigidbody.PathSpeed).ToVector2() * BraveTime.DeltaTime).ToIntVector2(VectorConversions.Ceil);
		SpeculativeRigidbody speculativeRigidbody = null;
		SpeculativeRigidbody speculativeRigidbody2 = null;
		if (this.occupation == MineCartController.CartOccupationState.CARGO)
		{
			speculativeRigidbody = this.carriedCargo;
		}
		else if (this.occupation != MineCartController.CartOccupationState.EMPTY)
		{
			if (this.m_rider)
			{
				speculativeRigidbody = this.m_rider.specRigidbody;
			}
			if (this.m_secondaryRider)
			{
				speculativeRigidbody2 = this.m_secondaryRider.specRigidbody;
			}
		}
		if (PhysicsEngine.Instance.OverlapCast(base.specRigidbody, this.m_cachedCollisionList, false, true, null, null, false, null, null, new SpeculativeRigidbody[] { speculativeRigidbody, speculativeRigidbody2, this.m_pusher }))
		{
			for (int i = 0; i < this.m_cachedCollisionList.Count; i++)
			{
				if (this.m_cachedCollisionList[i].OtherRigidbody)
				{
					MineCartController component = this.m_cachedCollisionList[i].OtherRigidbody.GetComponent<MineCartController>();
					if (!(component == null))
					{
						float parametrizedPathPosition = component.m_pathMover.GetParametrizedPathPosition();
						float parametrizedPathPosition2 = this.m_pathMover.GetParametrizedPathPosition();
						bool flag = (this.m_pathMover.PathSpeed > 0f && parametrizedPathPosition > parametrizedPathPosition2) || (parametrizedPathPosition < 0.25f && parametrizedPathPosition2 > 0.75f) || (this.m_pathMover.PathSpeed < 0f && parametrizedPathPosition < parametrizedPathPosition2) || (parametrizedPathPosition > 0.75f && parametrizedPathPosition2 < 0.25f);
						if (flag)
						{
							return component;
						}
					}
				}
			}
		}
		CollisionData collisionData;
		if (PhysicsEngine.Instance.RigidbodyCastWithIgnores(base.specRigidbody, intVector, out collisionData, false, true, null, true, new SpeculativeRigidbody[] { speculativeRigidbody, speculativeRigidbody2, this.m_pusher }))
		{
			mineCartController = collisionData.OtherRigidbody.GetComponent<MineCartController>();
			if (mineCartController != null)
			{
				for (int j = 0; j < this.m_cachedCollisionList.Count; j++)
				{
					if (this.m_cachedCollisionList[j].OtherRigidbody == mineCartController.specRigidbody)
					{
						mineCartController = null;
						break;
					}
				}
			}
		}
		CollisionData.Pool.Free(ref collisionData);
		return mineCartController;
	}

	// Token: 0x060064BA RID: 25786 RVA: 0x00271578 File Offset: 0x0026F778
	protected void HandlePlayerRiderInput(GameActor targetRider, float targetElapsed)
	{
		if (targetRider == null)
		{
			return;
		}
		PlayerController playerController = targetRider as PlayerController;
		playerController.ZeroVelocityThisFrame = true;
		if (targetElapsed > BraveTime.DeltaTime)
		{
			GungeonActions activeActions = BraveInput.GetInstanceForPlayer(playerController.PlayerIDX).ActiveActions;
			if (activeActions.InteractAction.WasPressed && this.m_justRolledInTimer <= 0f)
			{
				if (targetRider == this.m_rider)
				{
					this.Evacuate(false, false);
				}
				else if (targetRider == this.m_secondaryRider)
				{
					this.EvacuateSecondary(false, false);
				}
			}
			if (targetRider == this.m_rider || (targetRider == this.m_secondaryRider && this.m_rider == null))
			{
				Vector2 majorAxis = BraveUtility.GetMajorAxis(this.m_pathMover.GetPositionOfNode(this.m_pathMover.CurrentIndex) - base.transform.position.XY());
				float num = Vector2.Dot(majorAxis, activeActions.Move.Vector) * 15f * Mathf.Sign(this.m_pathMover.PathSpeed) * BraveTime.DeltaTime;
				if (this.m_pathMover.AbsPathSpeed / this.MaxSpeed > 0.1f && Mathf.Sign(num) != this.m_lastAccelVector && num != 0f && Mathf.Sign(num) != Mathf.Sign(this.m_pathMover.PathSpeed))
				{
					AkSoundEngine.PostEvent("Play_OBJ_minecart_brake_01", base.gameObject);
					this.m_lastAccelVector = Mathf.Sign(num);
				}
				this.m_pathMover.PathSpeed += num;
				if (num == 0f && this.m_pathMover.AbsPathSpeed / this.MaxSpeedPlayer < 0.3f)
				{
					this.m_pathMover.PathSpeed = Mathf.MoveTowards(this.m_pathMover.PathSpeed, 0f, 4f * BraveTime.DeltaTime);
				}
			}
			if (activeActions.DodgeRollAction.WasPressed && !playerController.WasPausedThisFrame)
			{
				if (activeActions.Move.Vector.magnitude > 0.1f)
				{
					if (targetRider == this.m_rider)
					{
						this.Evacuate(true, false);
					}
					else if (targetRider == this.m_secondaryRider)
					{
						this.EvacuateSecondary(true, false);
					}
				}
				else
				{
					Vector2 normalized = base.specRigidbody.Velocity.normalized;
					string text = string.Empty;
					if (Mathf.Abs(normalized.x) < 0.1f)
					{
						text = ((normalized.y <= 0.1f) ? "dodge" : "dodge_bw") + ((!playerController.ArmorlessAnimations || playerController.healthHaver.Armor != 0f) ? string.Empty : "_armorless");
					}
					else
					{
						text = ((normalized.y <= 0.1f) ? "dodge_left" : "dodge_left_bw") + ((!playerController.ArmorlessAnimations || playerController.healthHaver.Armor != 0f) ? string.Empty : "_armorless");
					}
					playerController.QueueSpecificAnimation(text);
				}
			}
		}
	}

	// Token: 0x060064BB RID: 25787 RVA: 0x002718D8 File Offset: 0x0026FAD8
	protected void SetAnimation(string animationName, float clipFpsFraction)
	{
		if (!string.IsNullOrEmpty(animationName))
		{
			float num = 4f;
			tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(animationName);
			if (!base.spriteAnimator.IsPlaying(clipByName))
			{
				base.spriteAnimator.Play(clipByName);
			}
			base.spriteAnimator.ClipFps = Mathf.Max(num, BraveMathCollege.UnboundedLerp(num, clipByName.fps, clipFpsFraction));
			string text = string.Empty;
			if (this.m_animationMap.ContainsKey(animationName))
			{
				text = this.m_animationMap[animationName];
			}
			else
			{
				text = animationName.Replace("_A", "_B");
				this.m_animationMap.Add(animationName, text);
			}
			tk2dSpriteAnimationClip clipByName2 = this.childAnimator.GetClipByName(text);
			if (!this.childAnimator.IsPlaying(clipByName2))
			{
				this.childAnimator.Play(clipByName2);
			}
			this.childAnimator.ClipFps = Mathf.Max(num, BraveMathCollege.UnboundedLerp(num, clipByName2.fps, clipFpsFraction));
		}
	}

	// Token: 0x060064BC RID: 25788 RVA: 0x002719D0 File Offset: 0x0026FBD0
	public void HandleTurnAnimation(Vector2 directionFromPreviousNode, Vector2 directionToNextNode)
	{
		IntVector2 intMajorAxis = BraveUtility.GetIntMajorAxis(directionFromPreviousNode);
		IntVector2 intMajorAxis2 = BraveUtility.GetIntMajorAxis(directionToNextNode);
		float num = 2f;
		if (intMajorAxis == IntVector2.North)
		{
			if (intMajorAxis2 == IntVector2.East)
			{
				this.SetAnimation("minecart_turn_TL_VH_A", num);
			}
			else if (intMajorAxis2 == IntVector2.West)
			{
				this.SetAnimation("minecart_turn_TR_VH_A", num);
			}
		}
		else if (intMajorAxis == IntVector2.East)
		{
			if (intMajorAxis2 == IntVector2.North)
			{
				this.SetAnimation("minecart_turn_BR_HV_A", num);
			}
			else if (intMajorAxis2 == IntVector2.South)
			{
				this.SetAnimation("minecart_turn_TR_HV_A", num);
			}
		}
		else if (intMajorAxis == IntVector2.South)
		{
			if (intMajorAxis2 == IntVector2.East)
			{
				this.SetAnimation("minecart_turn_BL_VH_A", num);
			}
			else if (intMajorAxis2 == IntVector2.West)
			{
				this.SetAnimation("minecart_turn_BR_VH_A", num);
			}
		}
		else if (intMajorAxis == IntVector2.West)
		{
			if (intMajorAxis2 == IntVector2.North)
			{
				this.SetAnimation("minecart_turn_BL_HV_A", num);
			}
			else if (intMajorAxis2 == IntVector2.South)
			{
				this.SetAnimation("minecart_turn_TL_HV_A", num);
			}
		}
		this.m_handlingQueuedAnimation = true;
	}

	// Token: 0x060064BD RID: 25789 RVA: 0x00271B3C File Offset: 0x0026FD3C
	public void HandleCornerReached(Vector2 directionFromPreviousNode, Vector2 directionToNextNode, bool hasNextNode)
	{
		this.m_pathMover.PathSpeed = Mathf.Sign(this.m_pathMover.PathSpeed) * ((this.m_pathMover.AbsPathSpeed <= this.MaxSpeedEnemy) ? (this.m_pathMover.AbsPathSpeed + 1f) : this.m_pathMover.AbsPathSpeed);
		if (!this.m_hasHandledCornerAnimation)
		{
			this.HandleTurnAnimation(directionFromPreviousNode, directionToNextNode);
		}
		this.m_hasHandledCornerAnimation = false;
		if (!hasNextNode)
		{
			if (GameManager.Instance.Dungeon.CellSupportsFalling(base.specRigidbody.UnitCenter))
			{
				this.HandlePitFall(directionFromPreviousNode);
			}
			else
			{
				base.StartCoroutine(this.DelayedWarpToStart());
			}
		}
	}

	// Token: 0x060064BE RID: 25790 RVA: 0x00271BFC File Offset: 0x0026FDFC
	private IEnumerator DelayedWarpToStart()
	{
		yield return null;
		if (this.m_pathMover.PathSpeed < 0f)
		{
			this.m_pathMover.WarpToNearestPoint(this.m_pathMover.Path.nodes[this.m_pathMover.Path.nodes.Count - 1].RoomPosition + this.m_pathMover.nodeOffset + this.m_pathMover.RoomHandler.area.basePosition.ToVector2());
		}
		else
		{
			this.m_pathMover.WarpToNearestPoint(this.m_pathMover.Path.nodes[0].RoomPosition + this.m_pathMover.nodeOffset + this.m_pathMover.RoomHandler.area.basePosition.ToVector2());
		}
		if (this.occupation == MineCartController.CartOccupationState.CARGO)
		{
			Vector2 vector = this.carriedCargo.transform.position.XY() - this.carriedCargo.sprite.WorldBottomCenter;
			this.carriedCargo.transform.position = this.attachTransform.position + vector.ToVector3ZUp(0f);
			this.carriedCargo.specRigidbody.Reinitialize();
		}
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		this.m_pathMover.ForcePathToNextNode();
		yield break;
	}

	// Token: 0x060064BF RID: 25791 RVA: 0x00271C18 File Offset: 0x0026FE18
	private void UpdateAnimations()
	{
		Vector2 velocity = base.specRigidbody.Velocity;
		float num = ((this.occupation != MineCartController.CartOccupationState.PLAYER) ? this.MaxSpeedEnemy : this.MaxSpeedPlayer);
		float num2 = this.m_pathMover.PathSpeed / num;
		if (this.m_handlingQueuedAnimation)
		{
			if (!base.spriteAnimator.IsPlaying(base.spriteAnimator.CurrentClip))
			{
				this.m_handlingQueuedAnimation = false;
			}
			if (base.spriteAnimator.CurrentClip == null || base.spriteAnimator.ClipFps <= 0f)
			{
				this.m_handlingQueuedAnimation = false;
			}
		}
		if (velocity.x == 0f && velocity.y == 0f)
		{
			base.spriteAnimator.Stop();
			this.childAnimator.Stop();
		}
		else if (Mathf.Abs(velocity.x) < Mathf.Abs(velocity.y))
		{
			if (!this.m_handlingQueuedAnimation)
			{
				this.SetAnimation(this.VerticalAnimationName, num2);
			}
		}
		else if (Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x) && !this.m_handlingQueuedAnimation)
		{
			this.SetAnimation(this.HorizontalAnimationName, num2);
		}
	}

	// Token: 0x060064C0 RID: 25792 RVA: 0x00271D64 File Offset: 0x0026FF64
	private void LateUpdate()
	{
		if (!this.m_pathMover.Paused)
		{
			this.UpdateAnimations();
		}
		if (this.occupation == MineCartController.CartOccupationState.EMPTY)
		{
			if (base.sprite.HeightOffGround != -1f)
			{
				base.sprite.HeightOffGround = -1f;
				base.sprite.UpdateZDepth();
			}
			if (this.childAnimator.sprite.HeightOffGround != 0.125f)
			{
				this.childAnimator.sprite.IsPerpendicular = false;
				this.childAnimator.sprite.HeightOffGround = 0.125f;
				this.childAnimator.sprite.UpdateZDepth();
			}
		}
		else
		{
			if (Mathf.Abs(base.specRigidbody.Velocity.y) > Mathf.Abs(base.specRigidbody.Velocity.x))
			{
				if (base.sprite.HeightOffGround != -1.25f)
				{
					base.sprite.HeightOffGround = -1.25f;
					base.sprite.UpdateZDepth();
				}
			}
			else if (base.sprite.HeightOffGround != -0.6f)
			{
				base.sprite.HeightOffGround = -0.6f;
				base.sprite.UpdateZDepth();
			}
			if (this.childAnimator.sprite.HeightOffGround != -2.5f)
			{
				this.childAnimator.sprite.IsPerpendicular = true;
				this.childAnimator.sprite.HeightOffGround = -2.5f;
				this.childAnimator.sprite.UpdateZDepth();
			}
			this.childAnimator.sprite.UpdateZDepth();
		}
	}

	// Token: 0x060064C1 RID: 25793 RVA: 0x00271F0C File Offset: 0x0027010C
	public void BecomeCargoOccupied()
	{
		if (this.MoveCarriedCargoIntoCart)
		{
			Vector2 vector = this.carriedCargo.transform.position.XY() - this.carriedCargo.sprite.WorldBottomCenter;
			this.carriedCargo.transform.position = this.attachTransform.position + vector.ToVector3ZUp(0f);
			this.carriedCargo.specRigidbody.Reinitialize();
		}
		this.carriedCargo.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
		base.specRigidbody.RegisterSpecificCollisionException(this.carriedCargo.specRigidbody);
		this.carriedCargo.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.LowObstacle));
		if (this.carriedCargo.knockbackDoer)
		{
			this.carriedCargo.knockbackDoer.knockbackMultiplier = 0f;
		}
		if (this.carriedCargo.minorBreakable && this.carriedCargo.minorBreakable.explodesOnBreak)
		{
			MinorBreakable minorBreakable = this.carriedCargo.minorBreakable;
			minorBreakable.OnBreak = (Action)Delegate.Combine(minorBreakable.OnBreak, new Action(delegate
			{
				this.DestroyMineCart();
			}));
		}
		base.specRigidbody.RegisterCarriedRigidbody(this.carriedCargo.specRigidbody);
	}

	// Token: 0x060064C2 RID: 25794 RVA: 0x00272064 File Offset: 0x00270264
	private void DestroyMineCart()
	{
		if (this.carriedCargo && this.carriedCargo.minorBreakable)
		{
			this.carriedCargo.transform.parent = null;
		}
		this.Evacuate(false, false);
		this.EvacuateSecondary(false, false);
		base.GetAbsoluteParentRoom().DeregisterInteractable(this);
		this.m_pathMover.Paused = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060064C3 RID: 25795 RVA: 0x002720E0 File Offset: 0x002702E0
	public void BecomeOccupied(PlayerController player)
	{
		if (this.occupation == MineCartController.CartOccupationState.ENEMY)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.RemoveOutlineFromSprite(this.childAnimator.sprite, false);
		if (this.occupation == MineCartController.CartOccupationState.PLAYER)
		{
			if (player == this.m_rider)
			{
				return;
			}
			player.currentMineCart = this;
			this.m_elapsedSecondary = 0f;
			if (player.IsDodgeRolling)
			{
				player.ForceStopDodgeRoll();
			}
			this.m_secondaryRider = player;
			player.CurrentInputState = PlayerInputState.NoMovement;
			player.ZeroVelocityThisFrame = true;
			this.AttachSecondaryRider();
			StaticReferenceManager.ActiveMineCarts.Add(player, this);
		}
		else if (this.occupation == MineCartController.CartOccupationState.EMPTY)
		{
			this.m_elapsedOccupied = 0f;
			player.currentMineCart = this;
			if (player.IsDodgeRolling)
			{
				player.ForceStopDodgeRoll();
			}
			this.m_rider = player;
			this.occupation = MineCartController.CartOccupationState.PLAYER;
			base.specRigidbody.PixelColliders[0].CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			player.CurrentInputState = PlayerInputState.NoMovement;
			player.ZeroVelocityThisFrame = true;
			this.AttachRider();
			StaticReferenceManager.ActiveMineCarts.Add(player, this);
		}
	}

	// Token: 0x060064C4 RID: 25796 RVA: 0x00272204 File Offset: 0x00270404
	public void BecomeOccupied(AIActor enemy)
	{
		if (this.occupation != MineCartController.CartOccupationState.EMPTY)
		{
			return;
		}
		this.m_elapsedOccupied = 0f;
		this.m_rider = enemy;
		this.occupation = MineCartController.CartOccupationState.ENEMY;
		this.AttachRider();
	}

	// Token: 0x060064C5 RID: 25797 RVA: 0x00272234 File Offset: 0x00270434
	public void EvacuateSpecificPlayer(PlayerController p, bool usePitfallLogic = false)
	{
		if (this.m_rider == p)
		{
			this.Evacuate(false, usePitfallLogic);
		}
		if (this.m_secondaryRider == p)
		{
			this.EvacuateSecondary(false, usePitfallLogic);
		}
	}

	// Token: 0x060064C6 RID: 25798 RVA: 0x00272268 File Offset: 0x00270468
	private void Evacuate(bool doRoll = false, bool isPitfalling = false)
	{
		if (this.occupation != MineCartController.CartOccupationState.EMPTY)
		{
			if (this.occupation == MineCartController.CartOccupationState.CARGO)
			{
				if (this.carriedCargo.minorBreakable)
				{
					this.carriedCargo.minorBreakable.Break();
				}
			}
			else
			{
				if (this.m_rider)
				{
					base.specRigidbody.DeregisterCarriedRigidbody(this.m_rider.specRigidbody);
					if (this.occupation == MineCartController.CartOccupationState.PLAYER)
					{
						GameManager.Instance.MainCameraController.SetManualControl(false, true);
						PlayerController playerController = this.m_rider as PlayerController;
						playerController.currentMineCart = null;
						playerController.CurrentInputState = PlayerInputState.AllInput;
						StaticReferenceManager.ActiveMineCarts.Remove(playerController);
						if (doRoll)
						{
							playerController.ForceStartDodgeRoll();
							playerController.previousMineCart = this;
						}
						else if (isPitfalling)
						{
							playerController.previousMineCart = this;
						}
						else
						{
							Vector2 vector = this.m_pathMover.GetPositionOfNode(this.m_pathMover.CurrentIndex) - this.m_pathMover.transform.position.XY();
							Vector2 vector2 = BraveUtility.GetMajorAxis(vector);
							if ((this.m_pathMover.GetPositionOfNode(this.m_pathMover.PreviousIndex) - this.m_pathMover.transform.position.XY()).magnitude < 1.5f)
							{
								vector2 *= -1f;
							}
							Vector2 vector3 = vector2.normalized * -1f;
							if (this.m_primaryLerpCoroutine != null)
							{
								base.StopCoroutine(this.m_primaryLerpCoroutine);
							}
							this.m_primaryLerpCoroutine = base.StartCoroutine(this.HandleLerpCameraPlayerPosition(playerController, -vector3));
							playerController.transform.position = playerController.transform.position + (vector2.normalized * -1f).ToVector3ZUp(0f);
							playerController.specRigidbody.Reinitialize();
						}
					}
					if (this.m_rider.knockbackDoer)
					{
						this.m_rider.knockbackDoer.knockbackMultiplier = 1f;
					}
					this.m_rider.FallingProhibited = false;
					this.m_rider.specRigidbody.DeregisterSpecificCollisionException(base.specRigidbody);
					base.specRigidbody.DeregisterSpecificCollisionException(this.m_rider.specRigidbody);
					this.m_rider.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.LowObstacle));
					this.m_rider.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
					base.specRigidbody.RegisterTemporaryCollisionException(this.m_rider.specRigidbody, 0.25f, null);
					this.m_rider = null;
				}
				if (this.m_secondaryRider == null)
				{
					this.occupation = MineCartController.CartOccupationState.EMPTY;
					base.specRigidbody.PixelColliders[0].CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
				}
			}
		}
	}

	// Token: 0x060064C7 RID: 25799 RVA: 0x00272550 File Offset: 0x00270750
	private IEnumerator HandleLerpCameraPlayerPosition(PlayerController targetPlayer, Vector2 sourceOffset)
	{
		if (targetPlayer.IsPrimaryPlayer)
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = true;
			GameManager.Instance.MainCameraController.OverridePlayerOnePosition = targetPlayer.CenterPosition;
		}
		else
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = true;
			GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = targetPlayer.CenterPosition;
		}
		yield return null;
		float elapsed = 0f;
		float duration = 0.2f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			Vector2 currentOffset = Vector2.Lerp(sourceOffset, Vector2.zero, elapsed / duration);
			if (targetPlayer.IsPrimaryPlayer)
			{
				GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = true;
				GameManager.Instance.MainCameraController.OverridePlayerOnePosition = targetPlayer.CenterPosition + currentOffset;
			}
			else
			{
				GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = true;
				GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = targetPlayer.CenterPosition + currentOffset;
			}
			yield return null;
		}
		if (targetPlayer.IsPrimaryPlayer)
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = false;
		}
		else
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = false;
		}
		yield break;
	}

	// Token: 0x060064C8 RID: 25800 RVA: 0x00272574 File Offset: 0x00270774
	private void EvacuateSecondary(bool doRoll = false, bool isPitfalling = false)
	{
		if (this.occupation == MineCartController.CartOccupationState.PLAYER && this.m_secondaryRider != null)
		{
			GameManager.Instance.MainCameraController.SetManualControl(false, true);
			base.specRigidbody.DeregisterCarriedRigidbody(this.m_secondaryRider.specRigidbody);
			PlayerController playerController = this.m_secondaryRider as PlayerController;
			playerController.currentMineCart = null;
			playerController.CurrentInputState = PlayerInputState.AllInput;
			StaticReferenceManager.ActiveMineCarts.Remove(playerController);
			if (doRoll)
			{
				playerController.ForceStartDodgeRoll();
				playerController.previousMineCart = this;
			}
			else if (isPitfalling)
			{
				playerController.previousMineCart = this;
			}
			else
			{
				Vector2 majorAxis = BraveUtility.GetMajorAxis(this.m_pathMover.GetPositionOfNode(this.m_pathMover.CurrentIndex) - this.m_pathMover.transform.position.XY());
				Vector2 vector = majorAxis.normalized * -1f;
				if (this.m_secondaryLerpCoroutine != null)
				{
					base.StopCoroutine(this.m_secondaryLerpCoroutine);
				}
				this.m_secondaryLerpCoroutine = base.StartCoroutine(this.HandleLerpCameraPlayerPosition(playerController, -vector));
				playerController.transform.position = playerController.transform.position + (majorAxis.normalized * -1f).ToVector3ZUp(0f);
				playerController.specRigidbody.Reinitialize();
			}
			if (this.m_secondaryRider.knockbackDoer)
			{
				this.m_secondaryRider.knockbackDoer.knockbackMultiplier = 1f;
			}
			this.m_secondaryRider.FallingProhibited = false;
			this.m_secondaryRider.specRigidbody.DeregisterSpecificCollisionException(base.specRigidbody);
			base.specRigidbody.DeregisterSpecificCollisionException(this.m_secondaryRider.specRigidbody);
			this.m_secondaryRider.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.LowObstacle));
			this.m_secondaryRider.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
			base.specRigidbody.RegisterTemporaryCollisionException(this.m_secondaryRider.specRigidbody, 0.25f, null);
			this.m_secondaryRider = null;
			if (this.m_rider == null)
			{
				this.occupation = MineCartController.CartOccupationState.EMPTY;
				base.specRigidbody.PixelColliders[0].CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			}
		}
	}

	// Token: 0x060064C9 RID: 25801 RVA: 0x002727C4 File Offset: 0x002709C4
	protected void AttachSecondaryRider()
	{
		Vector2 vector = this.m_secondaryRider.transform.position.XY() - this.m_secondaryRider.specRigidbody.UnitBottomCenter;
		Vector2 vector2 = new Vector2(0.125f, 0.25f);
		vector += vector2;
		if (this.m_secondaryRider is PlayerController)
		{
			Vector2 vector3 = (this.attachTransform.position + vector.ToVector3ZUp(0f)).XY() - this.m_secondaryRider.transform.position.XY();
			if (this.m_secondaryLerpCoroutine != null)
			{
				base.StopCoroutine(this.m_secondaryLerpCoroutine);
			}
			this.m_secondaryLerpCoroutine = base.StartCoroutine(this.HandleLerpCameraPlayerPosition(this.m_secondaryRider as PlayerController, -vector3));
		}
		this.m_secondaryRider.transform.position = this.attachTransform.position + vector.ToVector3ZUp(0f);
		this.m_secondaryRider.specRigidbody.Reinitialize();
		this.m_secondaryRider.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
		base.specRigidbody.RegisterSpecificCollisionException(this.m_secondaryRider.specRigidbody);
		this.m_secondaryRider.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.LowObstacle));
		if (this.m_secondaryRider.knockbackDoer)
		{
			this.m_secondaryRider.knockbackDoer.knockbackMultiplier = 0f;
		}
		this.m_secondaryRider.FallingProhibited = true;
		base.specRigidbody.RegisterCarriedRigidbody(this.m_secondaryRider.specRigidbody);
	}

	// Token: 0x060064CA RID: 25802 RVA: 0x00272964 File Offset: 0x00270B64
	public void ForceUpdatePositions()
	{
		this.EnsureRiderPosition();
	}

	// Token: 0x060064CB RID: 25803 RVA: 0x0027296C File Offset: 0x00270B6C
	protected void EnsureRiderPosition()
	{
		if (this.m_rider != null)
		{
			Vector2 vector = this.attachTransform.position.XY() + (this.m_rider.transform.position.XY() - this.m_rider.specRigidbody.UnitBottomCenter);
			float num = Vector2.Distance(vector, this.m_rider.transform.position);
			if (num > 0.0625f)
			{
				this.m_rider.transform.position = vector;
				this.m_rider.specRigidbody.Reinitialize();
			}
		}
		if (this.m_secondaryRider != null)
		{
			Vector2 vector2 = this.attachTransform.position.XY() + (this.m_secondaryRider.transform.position.XY() - this.m_secondaryRider.specRigidbody.UnitBottomCenter + new Vector2(0.125f, 0.25f));
			float num2 = Vector2.Distance(vector2, this.m_secondaryRider.transform.position);
			if (num2 > 0.0625f)
			{
				this.m_secondaryRider.transform.position = vector2;
				this.m_secondaryRider.specRigidbody.Reinitialize();
			}
		}
	}

	// Token: 0x060064CC RID: 25804 RVA: 0x00272ACC File Offset: 0x00270CCC
	protected void AttachRider()
	{
		Vector2 vector = this.m_rider.transform.position.XY() - this.m_rider.specRigidbody.UnitBottomCenter;
		if (this.m_rider is PlayerController)
		{
			Vector2 vector2 = (this.attachTransform.position + vector.ToVector3ZUp(0f)).XY() - this.m_rider.transform.position.XY();
			if (this.m_primaryLerpCoroutine != null)
			{
				base.StopCoroutine(this.m_primaryLerpCoroutine);
			}
			this.m_primaryLerpCoroutine = base.StartCoroutine(this.HandleLerpCameraPlayerPosition(this.m_rider as PlayerController, -vector2));
		}
		this.m_rider.transform.position = this.attachTransform.position + vector.ToVector3ZUp(0f);
		this.m_rider.specRigidbody.Reinitialize();
		this.m_rider.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
		base.specRigidbody.RegisterSpecificCollisionException(this.m_rider.specRigidbody);
		this.m_rider.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.LowObstacle));
		if (this.m_rider.knockbackDoer)
		{
			this.m_rider.knockbackDoer.knockbackMultiplier = 0f;
		}
		this.m_rider.FallingProhibited = true;
		base.specRigidbody.RegisterCarriedRigidbody(this.m_rider.specRigidbody);
	}

	// Token: 0x060064CD RID: 25805 RVA: 0x00272C54 File Offset: 0x00270E54
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.occupation == MineCartController.CartOccupationState.ENEMY || this.occupation == MineCartController.CartOccupationState.CARGO)
		{
			return 1000f;
		}
		return Vector2.Distance(point, base.specRigidbody.UnitCenter) / 2f;
	}

	// Token: 0x060064CE RID: 25806 RVA: 0x00272C8C File Offset: 0x00270E8C
	public void OnEnteredRange(PlayerController interactor)
	{
		if (this.occupation == MineCartController.CartOccupationState.PLAYER && (interactor == this.m_rider || interactor == this.m_secondaryRider))
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1.75f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		SpriteOutlineManager.AddOutlineToSprite(this.childAnimator.sprite, Color.white);
	}

	// Token: 0x060064CF RID: 25807 RVA: 0x00272CF8 File Offset: 0x00270EF8
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.RemoveOutlineFromSprite(this.childAnimator.sprite, true);
	}

	// Token: 0x060064D0 RID: 25808 RVA: 0x00272D18 File Offset: 0x00270F18
	public void Interact(PlayerController interactor)
	{
		if (this.occupation == MineCartController.CartOccupationState.ENEMY)
		{
			return;
		}
		if (this.occupation == MineCartController.CartOccupationState.PLAYER && this.m_rider == interactor)
		{
			return;
		}
		if (this.occupation == MineCartController.CartOccupationState.PLAYER && this.m_secondaryRider == interactor)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.RemoveOutlineFromSprite(this.childAnimator.sprite, false);
		this.BecomeOccupied(interactor);
	}

	// Token: 0x060064D1 RID: 25809 RVA: 0x00272D94 File Offset: 0x00270F94
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060064D2 RID: 25810 RVA: 0x00272DA0 File Offset: 0x00270FA0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x04006045 RID: 24645
	[NonSerialized]
	public bool ForceActive;

	// Token: 0x04006046 RID: 24646
	[DwarfConfigurable]
	public bool IsOnlyPlayerMinecart;

	// Token: 0x04006047 RID: 24647
	[DwarfConfigurable]
	public bool AlwaysMoving;

	// Token: 0x04006048 RID: 24648
	public tk2dSpriteAnimator childAnimator;

	// Token: 0x04006049 RID: 24649
	public Transform attachTransform;

	// Token: 0x0400604A RID: 24650
	public SpeculativeRigidbody carriedCargo;

	// Token: 0x0400604B RID: 24651
	public bool MoveCarriedCargoIntoCart = true;

	// Token: 0x0400604C RID: 24652
	public string HorizontalAnimationName;

	// Token: 0x0400604D RID: 24653
	public string VerticalAnimationName;

	// Token: 0x0400604E RID: 24654
	public float KnockbackStrengthPlayer = 3f;

	// Token: 0x0400604F RID: 24655
	public float KnockbackStrengthEnemy = 10f;

	// Token: 0x04006050 RID: 24656
	[DwarfConfigurable]
	public float MaxSpeed = 7f;

	// Token: 0x04006051 RID: 24657
	public float TimeToMaxSpeed = 1f;

	// Token: 0x04006052 RID: 24658
	private const float UnoccupiedSpeedDecay = 4f;

	// Token: 0x04006053 RID: 24659
	private CartTurretController m_turret;

	// Token: 0x04006054 RID: 24660
	public Transform Sparks_A;

	// Token: 0x04006055 RID: 24661
	public Transform Sparks_B;

	// Token: 0x04006056 RID: 24662
	[NonSerialized]
	public MineCartController.CartOccupationState occupation;

	// Token: 0x04006057 RID: 24663
	protected GameActor m_rider;

	// Token: 0x04006058 RID: 24664
	protected GameActor m_secondaryRider;

	// Token: 0x04006059 RID: 24665
	protected PathMover m_pathMover;

	// Token: 0x0400605A RID: 24666
	protected float m_elapsedOccupied;

	// Token: 0x0400605B RID: 24667
	protected float m_elapsedSecondary;

	// Token: 0x0400605C RID: 24668
	protected bool m_handlingQueuedAnimation;

	// Token: 0x0400605D RID: 24669
	protected RoomHandler m_room;

	// Token: 0x0400605E RID: 24670
	protected List<MineCartController> m_minecartsInRoom;

	// Token: 0x0400605F RID: 24671
	private float m_justRolledInTimer;

	// Token: 0x04006060 RID: 24672
	private bool m_hasHandledCornerAnimation;

	// Token: 0x04006061 RID: 24673
	private bool m_wasPushedThisFrame;

	// Token: 0x04006062 RID: 24674
	private SpeculativeRigidbody m_pusher;

	// Token: 0x04006063 RID: 24675
	private bool m_cartSoundActive;

	// Token: 0x04006064 RID: 24676
	private List<CollisionData> m_cachedCollisionList = new List<CollisionData>();

	// Token: 0x04006065 RID: 24677
	private float m_lastAccelVector;

	// Token: 0x04006066 RID: 24678
	private Dictionary<string, string> m_animationMap = new Dictionary<string, string>();

	// Token: 0x04006067 RID: 24679
	protected Coroutine m_primaryLerpCoroutine;

	// Token: 0x04006068 RID: 24680
	protected Coroutine m_secondaryLerpCoroutine;

	// Token: 0x020011A9 RID: 4521
	public enum CartOccupationState
	{
		// Token: 0x0400606A RID: 24682
		EMPTY,
		// Token: 0x0400606B RID: 24683
		PLAYER,
		// Token: 0x0400606C RID: 24684
		ENEMY,
		// Token: 0x0400606D RID: 24685
		CARGO
	}
}
