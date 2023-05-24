using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020011B1 RID: 4529
public class MinorBreakable : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x060064FD RID: 25853 RVA: 0x00273BC8 File Offset: 0x00271DC8
	public bool IsBroken
	{
		get
		{
			return this.m_isBroken;
		}
	}

	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x060064FE RID: 25854 RVA: 0x00273BD0 File Offset: 0x00271DD0
	public bool IsBig
	{
		get
		{
			if (this.ForceSmallForCollisions)
			{
				this.m_cachedIsBig = new bool?(false);
			}
			else if (this.m_cachedIsBig == null && base.specRigidbody && base.specRigidbody.PrimaryPixelCollider != null)
			{
				PixelCollider pixelCollider = base.specRigidbody.HitboxPixelCollider ?? base.specRigidbody.PrimaryPixelCollider;
				this.m_cachedIsBig = new bool?(pixelCollider.Dimensions.x > 8 || pixelCollider.Dimensions.y > 8);
			}
			return this.m_cachedIsBig.Value;
		}
	}

	// Token: 0x17000EF3 RID: 3827
	// (get) Token: 0x060064FF RID: 25855 RVA: 0x00273C88 File Offset: 0x00271E88
	// (set) Token: 0x06006500 RID: 25856 RVA: 0x00273C90 File Offset: 0x00271E90
	public MinorBreakableGroupManager GroupManager
	{
		get
		{
			return this.m_groupManager;
		}
		set
		{
			this.m_groupManager = value;
		}
	}

	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x06006501 RID: 25857 RVA: 0x00273C9C File Offset: 0x00271E9C
	public Vector2 CenterPoint
	{
		get
		{
			if (base.specRigidbody)
			{
				return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			if (base.sprite)
			{
				return base.sprite.WorldCenter;
			}
			return base.transform.position.XY();
		}
	}

	// Token: 0x06006502 RID: 25858 RVA: 0x00273CF4 File Offset: 0x00271EF4
	private void Awake()
	{
		StaticReferenceManager.AllMinorBreakables.Add(this);
	}

	// Token: 0x06006503 RID: 25859 RVA: 0x00273D04 File Offset: 0x00271F04
	private IEnumerator Start()
	{
		this.m_transform = base.transform;
		this.m_sprite = base.GetComponent<tk2dSprite>();
		this.m_spriteAnimator = base.GetComponent<tk2dSpriteAnimator>();
		if (base.specRigidbody != null)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		}
		this.m_isBroken = false;
		yield return null;
		if (this.IsDecorativeOnly && this.m_spriteAnimator && (base.spriteAnimator.CurrentClip == null || !base.spriteAnimator.IsPlaying(base.spriteAnimator.CurrentClip)))
		{
			tk2dSpriteAnimator spriteAnimator = this.m_spriteAnimator;
			spriteAnimator.OnPlayAnimationCalled = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.OnPlayAnimationCalled, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(delegate(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
			{
				if (!anim.enabled)
				{
					anim.enabled = true;
				}
			}));
			this.m_spriteAnimator.enabled = false;
		}
		yield break;
	}

	// Token: 0x06006504 RID: 25860 RVA: 0x00273D20 File Offset: 0x00271F20
	public void CleanupCallbacks()
	{
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		}
	}

	// Token: 0x06006505 RID: 25861 RVA: 0x00273D5C File Offset: 0x00271F5C
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (myCollider.IsTrigger)
		{
			return;
		}
		if (this.OnlyBrokenByCode)
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		if (this.m_isBroken)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (this.OnlyBreaksOnScreen && !base.renderer.isVisible)
		{
			return;
		}
		Projectile component = otherRigidbody.GetComponent<Projectile>();
		if (this.onlyVulnerableToGunfire && component == null)
		{
			return;
		}
		if (this.OnlyPlayerProjectilesCanBreak && component && !(component.Owner is PlayerController))
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (this.isInvulnerableToGameActors && otherRigidbody.gameActor != null)
		{
			return;
		}
		if (this.isImpermeableToGameActors && otherRigidbody.gameActor != null)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.gameActor is PlayerController && (otherRigidbody.gameActor as PlayerController).IsEthereal)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.minorBreakable != null)
		{
			return;
		}
		Vector2 normalized = otherRigidbody.Velocity.normalized;
		float num = otherRigidbody.Velocity.magnitude;
		num = Mathf.Min(num, 5f);
		this.Break(normalized * num);
		if (!this.stopsBullets)
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (otherRigidbody.gameActor != null)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06006506 RID: 25862 RVA: 0x00273EE0 File Offset: 0x002720E0
	private void OnBreakAnimationComplete()
	{
		if (this.explodesOnBreak)
		{
			Exploder.Explode(base.specRigidbody.UnitCenter, this.explosionData, Vector2.zero, new Action(this.FinishBreak), false, CoreDamageTypes.None, false);
		}
		else
		{
			this.FinishBreak();
		}
	}

	// Token: 0x06006507 RID: 25863 RVA: 0x00273F34 File Offset: 0x00272134
	private void FinishBreak()
	{
		if (this.goopsOnBreak)
		{
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopType);
			goopManagerForGoopType.TimedAddGoopCircle(base.specRigidbody.UnitCenter, this.goopRadius, 0.5f, false);
		}
		if (this.destroyOnBreak)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (this.makeParallelOnBreak)
		{
			this.m_sprite.IsPerpendicular = false;
		}
	}

	// Token: 0x06006508 RID: 25864 RVA: 0x00273FA8 File Offset: 0x002721A8
	public void SpawnShards(Vector2 direction, float minAngle, float maxAngle, float verticalSpeed, float minMagnitude, float maxMagnitude)
	{
		if (GameManager.Options.DebrisQuantity == GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			return;
		}
		if (this.m_sprite == null || this.m_transform == null)
		{
			this.m_transform = base.transform;
			this.m_sprite = base.GetComponent<tk2dSprite>();
		}
		if (this.m_sprite == null || this.m_transform == null)
		{
			Debug.LogError("trying to spawn shards on a object with no transform or sprite");
			return;
		}
		Vector3 vector = this.m_sprite.WorldCenter.ToVector3ZUp(this.m_sprite.WorldCenter.y) + this.ShardSpawnOffset.ToVector3ZUp(0f);
		if (this.shardClusters != null && this.shardClusters.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, 10);
			for (int i = 0; i < this.shardClusters.Length; i++)
			{
				ShardCluster shardCluster = this.shardClusters[i];
				int num2 = UnityEngine.Random.Range(shardCluster.minFromCluster, shardCluster.maxFromCluster + 1);
				int num3 = UnityEngine.Random.Range(0, shardCluster.clusterObjects.Length);
				for (int j = 0; j < num2; j++)
				{
					float lowDiscrepancyRandom = BraveMathCollege.GetLowDiscrepancyRandom(num);
					num++;
					float num4 = Mathf.Lerp(minAngle, maxAngle, lowDiscrepancyRandom);
					Vector3 vector2 = Quaternion.Euler(0f, 0f, num4) * (direction.normalized * UnityEngine.Random.Range(minMagnitude, maxMagnitude)).ToVector3ZUp(verticalSpeed);
					int num5 = (num3 + j) % shardCluster.clusterObjects.Length;
					GameObject gameObject = SpawnManager.SpawnDebris(shardCluster.clusterObjects[num5].gameObject, vector, Quaternion.identity);
					tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
					if (this.m_sprite.attachParent != null && component != null)
					{
						component.attachParent = this.m_sprite.attachParent;
						component.HeightOffGround = this.m_sprite.HeightOffGround;
					}
					DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
					vector2 = Vector3.Scale(vector2, shardCluster.forceAxialMultiplier) * shardCluster.forceMultiplier;
					component2.Trigger(vector2, this.heightOffGround + this.AdditionalSpawnedObjectHeight, shardCluster.rotationMultiplier);
				}
			}
		}
		if (this.AdditionalVFXObject != null)
		{
			SpawnManager.SpawnVFX(this.AdditionalVFXObject, vector, Quaternion.identity);
		}
	}

	// Token: 0x06006509 RID: 25865 RVA: 0x00274218 File Offset: 0x00272418
	private void SpawnStain()
	{
		if (this.stainObject != null)
		{
			GameObject gameObject = SpawnManager.SpawnDecal(this.stainObject);
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			component.PlaceAtPositionByAnchor(base.sprite.WorldCenter.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
			component.HeightOffGround = 0.1f;
			if (this.parentSurface != null && !this.parentSurface.IsDestabilized)
			{
				component.HeightOffGround = 0.1f;
				if (this.parentSurface.sprite != null)
				{
					this.parentSurface.sprite.AttachRenderer(component);
					this.parentSurface.sprite.UpdateZDepth();
				}
				MajorBreakable component2 = this.parentSurface.GetComponent<MajorBreakable>();
				if (component2 != null)
				{
					component2.AttachDestructibleVFX(gameObject);
				}
			}
			else
			{
				component.HeightOffGround = -1f;
				component.UpdateZDepth();
			}
		}
	}

	// Token: 0x0600650A RID: 25866 RVA: 0x00274308 File Offset: 0x00272508
	private void HandleSynergies()
	{
		if (this.IgnoredForPotShotsModifier || this.OnlyBrokenByCode)
		{
			return;
		}
		int num = 0;
		if (PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.MINOR_BLANKABLES, out num))
		{
			float value = UnityEngine.Random.value;
			float num2 = 0.01f;
			if (value < num2 * (float)num)
			{
				Vector2 vector = ((!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter);
				PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
				Vector2? vector2 = new Vector2?(vector);
				bestActivePlayer.ForceBlank(25f, 0.5f, false, true, vector2, false, -1f);
			}
		}
	}

	// Token: 0x0600650B RID: 25867 RVA: 0x002743B0 File Offset: 0x002725B0
	private void HandleShardSpawns(Vector2 sourceVelocity)
	{
		MinorBreakable.BreakStyle breakStyle = this.breakStyle;
		if (sourceVelocity == Vector2.zero)
		{
			breakStyle = MinorBreakable.BreakStyle.BURST;
		}
		float num = 1.5f;
		this.SpawnLoot();
		switch (breakStyle)
		{
		case MinorBreakable.BreakStyle.CONE:
			this.SpawnShards(sourceVelocity, -45f, 45f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f);
			break;
		case MinorBreakable.BreakStyle.BURST:
			this.SpawnShards(Vector2.right, -180f, 180f, num, 1f, 2f);
			break;
		case MinorBreakable.BreakStyle.JET:
			this.SpawnShards(sourceVelocity, -15f, 15f, num, sourceVelocity.magnitude * 0.5f, sourceVelocity.magnitude * 1.5f);
			break;
		case MinorBreakable.BreakStyle.WALL_DOWNWARD_BURST:
			this.SpawnShards(Vector2.down, -30f, 30f, 0f, 0.25f, 0.75f);
			break;
		}
		this.SpawnStain();
	}

	// Token: 0x0600650C RID: 25868 RVA: 0x002744B8 File Offset: 0x002726B8
	public void SpawnLoot()
	{
		if (this.dropCoins && UnityEngine.Random.value < this.chanceToRain)
		{
			Vector3 vector = Vector3.up;
			vector *= 2f;
			GameObject gameObject = SpawnManager.SpawnDebris(GameManager.Instance.Dungeon.sharedSettingsPrefab.currencyDropSettings.bronzeCoinPrefab, base.specRigidbody.UnitCenter.ToVector3ZUp(base.transform.position.z), Quaternion.identity);
			DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
			orAddComponent.shouldUseSRBMotion = true;
			orAddComponent.angularVelocity = 0f;
			orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
			orAddComponent.Trigger(vector.WithZ(4f), 0.05f, 1f);
			orAddComponent.canRotate = false;
		}
	}

	// Token: 0x0600650D RID: 25869 RVA: 0x0027457C File Offset: 0x0027277C
	private void HandleParticulates(Vector2 vel)
	{
		if (this.hasParticulates)
		{
			Vector3 vector = base.sprite.WorldBottomLeft;
			Vector3 vector2 = base.sprite.WorldTopRight;
			GlobalSparksDoer.EmitRegionStyle emitStyle = this.EmitStyle;
			if (emitStyle != GlobalSparksDoer.EmitRegionStyle.RADIAL)
			{
				if (emitStyle == GlobalSparksDoer.EmitRegionStyle.RANDOM)
				{
					GlobalSparksDoer.DoRandomParticleBurst(UnityEngine.Random.Range(this.MinParticlesOnBurst, this.MaxParticlesOnBurst), vector, vector2, vel.normalized * this.ParticleMagnitude, 45f, this.ParticleMagnitudeVariance, new float?(this.ParticleSize), new float?(this.ParticleLifespan), new Color?(this.ParticleColor), this.ParticleType);
				}
			}
			else
			{
				GlobalSparksDoer.DoRadialParticleBurst(UnityEngine.Random.Range(this.MinParticlesOnBurst, this.MaxParticlesOnBurst), vector, vector2, 30f, this.ParticleMagnitude, this.ParticleMagnitudeVariance, new float?(this.ParticleSize), new float?(this.ParticleLifespan), new Color?(this.ParticleColor), this.ParticleType);
			}
		}
	}

	// Token: 0x0600650E RID: 25870 RVA: 0x0027468C File Offset: 0x0027288C
	public void Break()
	{
		if (!this || !base.enabled)
		{
			return;
		}
		if (this.m_isBroken)
		{
			return;
		}
		this.m_isBroken = true;
		if (this.m_groupManager != null)
		{
			this.m_groupManager.InformBroken(this, Vector2.zero, this.heightOffGround);
		}
		if (GameManager.Instance.InTutorial && !base.name.Contains("table", true) && !base.name.Contains("red", true))
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerBrokeShit");
		}
		if (this.m_occupiedCells != null)
		{
			this.m_occupiedCells.Clear();
		}
		IPlayerInteractable @interface = base.gameObject.GetInterface<IPlayerInteractable>();
		if (@interface != null)
		{
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
			if (roomFromPosition.IsRegistered(@interface))
			{
				roomFromPosition.DeregisterInteractable(@interface);
			}
		}
		if (base.specRigidbody != null)
		{
			base.specRigidbody.enabled = false;
		}
		bool flag = false;
		if (this.m_spriteAnimator != null && this.breakAnimName != string.Empty)
		{
			tk2dSpriteAnimationClip clipByName = this.m_spriteAnimator.GetClipByName(this.breakAnimName);
			if (clipByName != null)
			{
				this.m_spriteAnimator.Play(clipByName);
				flag = true;
				base.Invoke("OnBreakAnimationComplete", clipByName.BaseClipLength);
			}
		}
		else if (!string.IsNullOrEmpty(this.breakAnimFrame))
		{
			this.m_sprite.SetSprite(this.breakAnimFrame);
		}
		if (!this.m_transform)
		{
			this.m_transform = base.transform;
		}
		if (this.m_transform)
		{
			AkSoundEngine.SetObjectPosition(base.gameObject, this.m_transform.position.x, this.m_transform.position.y, this.m_transform.position.z, this.m_transform.forward.x, this.m_transform.forward.y, this.m_transform.forward.z, this.m_transform.up.x, this.m_transform.up.y, this.m_transform.up.z);
		}
		if (!string.IsNullOrEmpty(this.breakAudioEventName))
		{
			AkSoundEngine.PostEvent(this.breakAudioEventName, base.gameObject);
		}
		this.HandleShardSpawns(Vector2.zero);
		this.HandleParticulates(Vector2.zero);
		this.HandleSynergies();
		SurfaceDecorator component = base.GetComponent<SurfaceDecorator>();
		if (component != null)
		{
			component.Destabilize(Vector2.zero);
		}
		this.DestabilizeAttachedObjects(Vector2.zero);
		if (this.OnBreak != null)
		{
			this.OnBreak();
		}
		if (this.OnBreakContext != null)
		{
			this.OnBreakContext(this);
		}
		if (this.destroyOnBreak && !flag)
		{
			if (this.ForcedDestroyDelay > 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.ForcedDestroyDelay);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600650F RID: 25871 RVA: 0x002749F4 File Offset: 0x00272BF4
	private void DestabilizeAttachedObjects(Vector2 vec)
	{
		if (this.m_doneAdditionalDestabilize)
		{
			return;
		}
		this.m_doneAdditionalDestabilize = true;
		for (int i = 0; i < this.AdditionalDestabilizedObjects.Count; i++)
		{
			if (this.AdditionalDestabilizedObjects[i])
			{
				Vector3 vector = UnityEngine.Random.insideUnitCircle.ToVector3ZUp(0.5f);
				vector *= UnityEngine.Random.Range(2.5f, 4f);
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FINALGEON)
				{
					vector.y = Mathf.Abs(vector.y) * -1f;
				}
				this.AdditionalDestabilizedObjects[i].transform.parent = SpawnManager.Instance.Debris;
				this.AdditionalDestabilizedObjects[i].Trigger(vector, 0.5f, 1f);
			}
		}
	}

	// Token: 0x06006510 RID: 25872 RVA: 0x00274AE4 File Offset: 0x00272CE4
	public void Break(Vector2 direction)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_isBroken)
		{
			return;
		}
		this.m_isBroken = true;
		if (this.m_groupManager != null)
		{
			this.m_groupManager.InformBroken(this, direction, this.heightOffGround);
		}
		bool flag = GameManager.Instance.InTutorial;
		if (GameManager.Instance.PrimaryPlayer.CurrentRoom != null)
		{
			flag = flag || GameManager.Instance.PrimaryPlayer.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL;
		}
		if (flag && !base.name.Contains("table", true) && !this.explodesOnBreak)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerBrokeShit");
		}
		if (this.m_occupiedCells != null)
		{
			this.m_occupiedCells.Clear();
		}
		IPlayerInteractable @interface = base.gameObject.GetInterface<IPlayerInteractable>();
		if (@interface != null)
		{
			RoomHandler roomHandler = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
			if (roomHandler == null)
			{
				roomHandler = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round) + IntVector2.Right);
			}
			if (roomHandler != null && roomHandler.IsRegistered(@interface))
			{
				roomHandler.DeregisterInteractable(@interface);
			}
		}
		if (base.specRigidbody != null)
		{
			base.specRigidbody.enabled = false;
		}
		bool flag2 = false;
		if (this.m_spriteAnimator != null && this.breakAnimName != string.Empty)
		{
			tk2dSpriteAnimationClip clipByName = this.m_spriteAnimator.GetClipByName(this.breakAnimName);
			if (clipByName != null)
			{
				this.m_spriteAnimator.Play(clipByName);
				flag2 = true;
				base.Invoke("OnBreakAnimationComplete", clipByName.BaseClipLength);
			}
		}
		else if (!string.IsNullOrEmpty(this.breakAnimFrame))
		{
			this.m_sprite.SetSprite(this.breakAnimFrame);
		}
		if (!this.m_transform)
		{
			this.m_transform = base.transform;
		}
		if (this.m_transform)
		{
			AkSoundEngine.SetObjectPosition(base.gameObject, this.m_transform.position.x, this.m_transform.position.y, this.m_transform.position.z, this.m_transform.forward.x, this.m_transform.forward.y, this.m_transform.forward.z, this.m_transform.up.x, this.m_transform.up.y, this.m_transform.up.z);
		}
		if (!string.IsNullOrEmpty(this.breakAudioEventName))
		{
			AkSoundEngine.PostEvent(this.breakAudioEventName, base.gameObject);
		}
		this.HandleShardSpawns(direction);
		this.HandleParticulates(direction);
		this.HandleSynergies();
		SurfaceDecorator component = base.GetComponent<SurfaceDecorator>();
		if (component != null)
		{
			component.Destabilize(direction.normalized);
		}
		this.DestabilizeAttachedObjects(direction.normalized);
		if (this.canSpawnFairy && GameManager.Instance.Dungeon.sharedSettingsPrefab.RandomShouldSpawnPotFairy())
		{
			IntVector2 intVector = base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
			PotFairyEngageDoer.InstantSpawn = true;
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(GameManager.Instance.Dungeon.sharedSettingsPrefab.PotFairyGuid);
			AIActor.Spawn(orLoadByGuid, intVector, roomFromPosition, true, AIActor.AwakenAnimationType.Default, true);
		}
		if (this.OnBreak != null)
		{
			this.OnBreak();
		}
		if (this.OnBreakContext != null)
		{
			this.OnBreakContext(this);
		}
		if (this.destroyOnBreak && !flag2)
		{
			if (this.ForcedDestroyDelay > 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.ForcedDestroyDelay);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06006511 RID: 25873 RVA: 0x00274F20 File Offset: 0x00273120
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllMinorBreakables.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x06006512 RID: 25874 RVA: 0x00274F34 File Offset: 0x00273134
	public void ConfigureOnPlacement(RoomHandler room)
	{
		if (this.isInvulnerableToGameActors && base.specRigidbody != null)
		{
			base.specRigidbody.Initialize();
			this.m_occupiedCells = new OccupiedCells(base.specRigidbody, room);
		}
	}

	// Token: 0x04006099 RID: 24729
	public MinorBreakable.BreakStyle breakStyle;

	// Token: 0x0400609A RID: 24730
	[BetterList]
	public ShardCluster[] shardClusters;

	// Token: 0x0400609B RID: 24731
	public GameObject stainObject;

	// Token: 0x0400609C RID: 24732
	public Vector2 ShardSpawnOffset;

	// Token: 0x0400609D RID: 24733
	public bool destroyOnBreak = true;

	// Token: 0x0400609E RID: 24734
	public float ForcedDestroyDelay;

	// Token: 0x0400609F RID: 24735
	public bool makeParallelOnBreak = true;

	// Token: 0x040060A0 RID: 24736
	public bool resistsExplosions;

	// Token: 0x040060A1 RID: 24737
	public bool stopsBullets;

	// Token: 0x040060A2 RID: 24738
	public bool canSpawnFairy;

	// Token: 0x040060A3 RID: 24739
	[Header("DropLoots")]
	public bool dropCoins;

	// Token: 0x040060A4 RID: 24740
	public float amountToRain;

	// Token: 0x040060A5 RID: 24741
	public float chanceToRain;

	// Token: 0x040060A6 RID: 24742
	[Header("Explosive?")]
	public bool explodesOnBreak;

	// Token: 0x040060A7 RID: 24743
	public ExplosionData explosionData;

	// Token: 0x040060A8 RID: 24744
	[Header("Goops?")]
	public bool goopsOnBreak;

	// Token: 0x040060A9 RID: 24745
	[ShowInInspectorIf("goopsOnBreak", false)]
	public GoopDefinition goopType;

	// Token: 0x040060AA RID: 24746
	[ShowInInspectorIf("goopsOnBreak", false)]
	public float goopRadius = 3f;

	// Token: 0x040060AB RID: 24747
	[Header("Particulates")]
	public bool hasParticulates;

	// Token: 0x040060AC RID: 24748
	[ShowInInspectorIf("hasParticulates", false)]
	public int MinParticlesOnBurst;

	// Token: 0x040060AD RID: 24749
	[ShowInInspectorIf("hasParticulates", false)]
	public int MaxParticlesOnBurst;

	// Token: 0x040060AE RID: 24750
	[ShowInInspectorIf("hasParticulates", false)]
	public float ParticleSize = 0.0625f;

	// Token: 0x040060AF RID: 24751
	[ShowInInspectorIf("hasParticulates", false)]
	public float ParticleLifespan = 0.25f;

	// Token: 0x040060B0 RID: 24752
	[ShowInInspectorIf("hasParticulates", false)]
	public float ParticleMagnitude = 1f;

	// Token: 0x040060B1 RID: 24753
	[ShowInInspectorIf("hasParticulates", false)]
	public float ParticleMagnitudeVariance = 0.5f;

	// Token: 0x040060B2 RID: 24754
	[ShowInInspectorIf("hasParticulates", false)]
	public Color ParticleColor;

	// Token: 0x040060B3 RID: 24755
	[ShowInInspectorIf("hasParticulates", false)]
	public GlobalSparksDoer.EmitRegionStyle EmitStyle = GlobalSparksDoer.EmitRegionStyle.RADIAL;

	// Token: 0x040060B4 RID: 24756
	[ShowInInspectorIf("hasParticulates", false)]
	public GlobalSparksDoer.SparksType ParticleType;

	// Token: 0x040060B5 RID: 24757
	[Header("Animation and Audio")]
	[CheckAnimation(null)]
	public string breakAnimName;

	// Token: 0x040060B6 RID: 24758
	public string breakAnimFrame;

	// Token: 0x040060B7 RID: 24759
	public string breakAudioEventName;

	// Token: 0x040060B8 RID: 24760
	public Action OnBreak;

	// Token: 0x040060B9 RID: 24761
	public Action<MinorBreakable> OnBreakContext;

	// Token: 0x040060BA RID: 24762
	public float AdditionalSpawnedObjectHeight;

	// Token: 0x040060BB RID: 24763
	public Vector2 SpawnedObjectOffsetVector = Vector2.zero;

	// Token: 0x040060BC RID: 24764
	[NonSerialized]
	public float heightOffGround = 0.1f;

	// Token: 0x040060BD RID: 24765
	[NonSerialized]
	public bool OnlyBreaksOnScreen;

	// Token: 0x040060BE RID: 24766
	public GameObject AdditionalVFXObject;

	// Token: 0x040060BF RID: 24767
	public bool OnlyBrokenByCode;

	// Token: 0x040060C0 RID: 24768
	public bool isInvulnerableToGameActors;

	// Token: 0x040060C1 RID: 24769
	[Header("Unusual Settings")]
	public bool CastleReplacedWithWaterDrum;

	// Token: 0x040060C2 RID: 24770
	[HideInInspector]
	public bool isImpermeableToGameActors;

	// Token: 0x040060C3 RID: 24771
	[HideInInspector]
	public bool onlyVulnerableToGunfire;

	// Token: 0x040060C4 RID: 24772
	public bool OnlyPlayerProjectilesCanBreak;

	// Token: 0x040060C5 RID: 24773
	[HideInInspector]
	public SurfaceDecorator parentSurface;

	// Token: 0x040060C6 RID: 24774
	public List<DebrisObject> AdditionalDestabilizedObjects;

	// Token: 0x040060C7 RID: 24775
	public bool ForceSmallForCollisions;

	// Token: 0x040060C8 RID: 24776
	public bool IgnoredForPotShotsModifier;

	// Token: 0x040060C9 RID: 24777
	private bool? m_cachedIsBig;

	// Token: 0x040060CA RID: 24778
	private bool m_isBroken;

	// Token: 0x040060CB RID: 24779
	private Transform m_transform;

	// Token: 0x040060CC RID: 24780
	private tk2dSprite m_sprite;

	// Token: 0x040060CD RID: 24781
	private tk2dSpriteAnimator m_spriteAnimator;

	// Token: 0x040060CE RID: 24782
	private MinorBreakableGroupManager m_groupManager;

	// Token: 0x040060CF RID: 24783
	public bool IsDecorativeOnly;

	// Token: 0x040060D0 RID: 24784
	private bool m_doneAdditionalDestabilize;

	// Token: 0x040060D1 RID: 24785
	private OccupiedCells m_occupiedCells;

	// Token: 0x020011B2 RID: 4530
	public enum BreakStyle
	{
		// Token: 0x040060D3 RID: 24787
		CONE,
		// Token: 0x040060D4 RID: 24788
		BURST,
		// Token: 0x040060D5 RID: 24789
		JET,
		// Token: 0x040060D6 RID: 24790
		WALL_DOWNWARD_BURST,
		// Token: 0x040060D7 RID: 24791
		CUSTOM = 100
	}
}
