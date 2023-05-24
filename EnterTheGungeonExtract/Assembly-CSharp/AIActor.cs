using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Dungeonator;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000F77 RID: 3959
[RequireComponent(typeof(SpeculativeRigidbody))]
[RequireComponent(typeof(HitEffectHandler))]
public class AIActor : GameActor, IPlaceConfigurable
{
	// Token: 0x0600554C RID: 21836 RVA: 0x002066EC File Offset: 0x002048EC
	public static void ClearPerLevelData()
	{
		StaticReferenceManager.AllEnemies.Clear();
	}

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x0600554D RID: 21837 RVA: 0x002066F8 File Offset: 0x002048F8
	public static float BaseLevelHealthModifier
	{
		get
		{
			float num = 1f;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				num = GameManager.Instance.COOP_ENEMY_HEALTH_MULTIPLIER;
			}
			GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
			if (lastLoadedLevelDefinition != null)
			{
				num *= lastLoadedLevelDefinition.enemyHealthMultiplier;
			}
			return num;
		}
	}

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x0600554E RID: 21838 RVA: 0x00206744 File Offset: 0x00204944
	// (set) Token: 0x0600554F RID: 21839 RVA: 0x0020674C File Offset: 0x0020494C
	public static float HealthModifier
	{
		get
		{
			return AIActor.m_healthModifier;
		}
		set
		{
			float healthModifier = AIActor.m_healthModifier;
			AIActor.m_healthModifier = value;
			for (int i = 0; i < StaticReferenceManager.AllEnemies.Count; i++)
			{
				if (StaticReferenceManager.AllEnemies[i] != null && StaticReferenceManager.AllEnemies[i])
				{
					HealthHaver healthHaver = StaticReferenceManager.AllEnemies[i].healthHaver;
					if (!healthHaver.healthIsNumberOfHits)
					{
						healthHaver.SetHealthMaximum(healthHaver.GetMaxHealth() / healthModifier * AIActor.m_healthModifier, null, false);
					}
				}
			}
		}
	}

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x06005550 RID: 21840 RVA: 0x002067E8 File Offset: 0x002049E8
	public float EnemyCollisionKnockbackStrength
	{
		get
		{
			return (this.EnemyCollisionKnockbackStrengthOverride < 0f) ? this.CollisionKnockbackStrength : this.EnemyCollisionKnockbackStrengthOverride;
		}
	}

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x06005551 RID: 21841 RVA: 0x0020680C File Offset: 0x00204A0C
	public Vector2 VoluntaryMovementVelocity
	{
		get
		{
			if (base.behaviorSpeculator && base.behaviorSpeculator.IsStunned)
			{
				return Vector2.zero;
			}
			if (this.BehaviorOverridesVelocity)
			{
				return this.BehaviorVelocity;
			}
			return this.GetPathVelocityContribution();
		}
	}

	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x06005552 RID: 21842 RVA: 0x0020684C File Offset: 0x00204A4C
	public bool IsMimicEnemy
	{
		get
		{
			bool? cachedIsMimicEnemy = this.m_cachedIsMimicEnemy;
			if (cachedIsMimicEnemy == null)
			{
				this.m_cachedIsMimicEnemy = new bool?(false);
				if (base.encounterTrackable && !string.IsNullOrEmpty(base.encounterTrackable.EncounterGuid))
				{
					this.m_cachedIsMimicEnemy = new bool?(base.encounterTrackable.EncounterGuid == GlobalEncounterGuids.Mimic);
				}
			}
			return this.m_cachedIsMimicEnemy.Value;
		}
	}

	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x06005553 RID: 21843 RVA: 0x002068CC File Offset: 0x00204ACC
	public float LocalDeltaTime
	{
		get
		{
			if (this.IsBlackPhantom)
			{
				return BraveTime.DeltaTime * this.LocalTimeScale * this.BlackPhantomProperties.LocalTimeScaleMultiplier;
			}
			return BraveTime.DeltaTime * this.LocalTimeScale;
		}
	}

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x06005554 RID: 21844 RVA: 0x00206900 File Offset: 0x00204B00
	// (set) Token: 0x06005555 RID: 21845 RVA: 0x00206908 File Offset: 0x00204B08
	public Vector2 EnemyScale
	{
		get
		{
			return this.m_currentlyAppliedEnemyScale;
		}
		set
		{
			this.m_currentlyAppliedEnemyScale = value;
			base.transform.localScale = value.ToVector3ZUp(1f);
			if (base.specRigidbody)
			{
				base.specRigidbody.UpdateCollidersOnScale = true;
				base.specRigidbody.RegenerateColliders = true;
			}
		}
	}

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x06005556 RID: 21846 RVA: 0x0020695C File Offset: 0x00204B5C
	// (set) Token: 0x06005557 RID: 21847 RVA: 0x00206964 File Offset: 0x00204B64
	[HideInInspector]
	public bool HasDamagedPlayer { get; set; }

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x06005558 RID: 21848 RVA: 0x00206970 File Offset: 0x00204B70
	// (set) Token: 0x06005559 RID: 21849 RVA: 0x00206978 File Offset: 0x00204B78
	public bool CanTargetPlayers
	{
		get
		{
			return this.m_canTargetPlayers;
		}
		set
		{
			this.PlayerTarget = null;
			this.m_canTargetPlayers = value;
		}
	}

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x0600555A RID: 21850 RVA: 0x00206988 File Offset: 0x00204B88
	// (set) Token: 0x0600555B RID: 21851 RVA: 0x00206990 File Offset: 0x00204B90
	public bool CanTargetEnemies
	{
		get
		{
			return this.m_canTargetEnemies;
		}
		set
		{
			this.PlayerTarget = null;
			this.m_canTargetEnemies = value;
		}
	}

	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x0600555C RID: 21852 RVA: 0x002069A0 File Offset: 0x00204BA0
	// (set) Token: 0x0600555D RID: 21853 RVA: 0x002069CC File Offset: 0x00204BCC
	public bool OverrideHitEnemies
	{
		get
		{
			int num = CollisionMask.LayerToMask(CollisionLayer.EnemyCollider);
			PixelCollider pixelCollider = base.specRigidbody.GetPixelCollider(ColliderType.Ground);
			return (pixelCollider.CollisionLayerCollidableOverride & num) == num;
		}
		set
		{
			int num = CollisionMask.LayerToMask(CollisionLayer.EnemyCollider);
			PixelCollider pixelCollider = base.specRigidbody.GetPixelCollider(ColliderType.Ground);
			if (value)
			{
				pixelCollider.CollisionLayerCollidableOverride |= num;
			}
			else
			{
				pixelCollider.CollisionLayerCollidableOverride &= ~num;
			}
		}
	}

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x0600555E RID: 21854 RVA: 0x00206A18 File Offset: 0x00204C18
	public bool IsOverPit
	{
		get
		{
			Vector2 vector = ((!(base.specRigidbody != null)) ? base.CenterPosition : base.specRigidbody.GroundPixelCollider.UnitCenter);
			return GameManager.Instance.Dungeon.CellSupportsFalling(vector);
		}
	}

	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x0600555F RID: 21855 RVA: 0x00206A68 File Offset: 0x00204C68
	public bool IsBlackPhantom
	{
		get
		{
			return this.m_championType == AIActor.EnemyChampionType.JAMMED;
		}
	}

	// Token: 0x17000BF6 RID: 3062
	// (get) Token: 0x06005560 RID: 21856 RVA: 0x00206A74 File Offset: 0x00204C74
	// (set) Token: 0x06005561 RID: 21857 RVA: 0x00206A7C File Offset: 0x00204C7C
	public bool SuppressBlackPhantomCorpseBurn { get; set; }

	// Token: 0x17000BF7 RID: 3063
	// (get) Token: 0x06005562 RID: 21858 RVA: 0x00206A88 File Offset: 0x00204C88
	// (set) Token: 0x06005563 RID: 21859 RVA: 0x00206A90 File Offset: 0x00204C90
	public Shader OverrideBlackPhantomShader { get; set; }

	// Token: 0x17000BF8 RID: 3064
	// (get) Token: 0x06005564 RID: 21860 RVA: 0x00206A9C File Offset: 0x00204C9C
	// (set) Token: 0x06005565 RID: 21861 RVA: 0x00206AA4 File Offset: 0x00204CA4
	public bool IsBuffEnemy { get; set; }

	// Token: 0x17000BF9 RID: 3065
	// (get) Token: 0x06005566 RID: 21862 RVA: 0x00206AB0 File Offset: 0x00204CB0
	public SpeculativeRigidbody TargetRigidbody
	{
		get
		{
			if (this.OverrideTarget != null)
			{
				if (this.OverrideTarget)
				{
					return this.OverrideTarget;
				}
				this.OverrideTarget = null;
			}
			if (this.PlayerTarget != null)
			{
				return this.PlayerTarget.specRigidbody;
			}
			return null;
		}
	}

	// Token: 0x17000BFA RID: 3066
	// (get) Token: 0x06005567 RID: 21863 RVA: 0x00206B10 File Offset: 0x00204D10
	public Vector2 TargetVelocity
	{
		get
		{
			if (this.OverrideTarget)
			{
				PlayerController playerController = this.OverrideTarget.gameActor as PlayerController;
				if (playerController)
				{
					return playerController.AverageVelocity;
				}
				return this.OverrideTarget.Velocity;
			}
			else
			{
				if (!this.PlayerTarget)
				{
					return Vector2.zero;
				}
				PlayerController playerController2 = this.PlayerTarget as PlayerController;
				if (playerController2)
				{
					return playerController2.AverageVelocity;
				}
				return this.PlayerTarget.specRigidbody.Velocity;
			}
		}
	}

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x06005568 RID: 21864 RVA: 0x00206BA0 File Offset: 0x00204DA0
	// (set) Token: 0x06005569 RID: 21865 RVA: 0x00206BA8 File Offset: 0x00204DA8
	public float SpeculatorDelayTime { get; set; }

	// Token: 0x17000BFC RID: 3068
	// (get) Token: 0x0600556A RID: 21866 RVA: 0x00206BB4 File Offset: 0x00204DB4
	// (set) Token: 0x0600556B RID: 21867 RVA: 0x00206BE0 File Offset: 0x00204DE0
	public bool IsWorthShootingAt
	{
		get
		{
			return (this.m_isWorthShootingAt == null) ? (!this.IsHarmlessEnemy) : this.m_isWorthShootingAt.Value;
		}
		set
		{
			this.m_isWorthShootingAt = new bool?(value);
		}
	}

	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x0600556C RID: 21868 RVA: 0x00206BF0 File Offset: 0x00204DF0
	// (set) Token: 0x0600556D RID: 21869 RVA: 0x00206BF8 File Offset: 0x00204DF8
	public bool HasDonePlayerEnterCheck { get; set; }

	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x0600556E RID: 21870 RVA: 0x00206C04 File Offset: 0x00204E04
	// (set) Token: 0x0600556F RID: 21871 RVA: 0x00206C0C File Offset: 0x00204E0C
	public bool PreventAutoKillOnBossDeath { get; set; }

	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x06005570 RID: 21872 RVA: 0x00206C18 File Offset: 0x00204E18
	// (set) Token: 0x06005571 RID: 21873 RVA: 0x00206C20 File Offset: 0x00204E20
	public string OverridePitfallAnim { get; set; }

	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06005572 RID: 21874 RVA: 0x00206C2C File Offset: 0x00204E2C
	// (remove) Token: 0x06005573 RID: 21875 RVA: 0x00206C64 File Offset: 0x00204E64
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AIActor.CustomPitHandlingDelegate CustomPitDeathHandling;

	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x06005574 RID: 21876 RVA: 0x00206C9C File Offset: 0x00204E9C
	// (set) Token: 0x06005575 RID: 21877 RVA: 0x00206CA4 File Offset: 0x00204EA4
	public GameActor PlayerTarget { get; set; }

	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x06005576 RID: 21878 RVA: 0x00206CB0 File Offset: 0x00204EB0
	// (set) Token: 0x06005577 RID: 21879 RVA: 0x00206CB8 File Offset: 0x00204EB8
	public SpeculativeRigidbody OverrideTarget { get; set; }

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x06005578 RID: 21880 RVA: 0x00206CC4 File Offset: 0x00204EC4
	// (set) Token: 0x06005579 RID: 21881 RVA: 0x00206CCC File Offset: 0x00204ECC
	public RoomHandler ParentRoom
	{
		get
		{
			return this.parentRoom;
		}
		set
		{
			this.parentRoom = value;
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x0600557A RID: 21882 RVA: 0x00206CD8 File Offset: 0x00204ED8
	// (set) Token: 0x0600557B RID: 21883 RVA: 0x00206CE0 File Offset: 0x00204EE0
	public bool HasBeenGlittered { get; set; }

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x0600557C RID: 21884 RVA: 0x00206CEC File Offset: 0x00204EEC
	// (set) Token: 0x0600557D RID: 21885 RVA: 0x00206CF4 File Offset: 0x00204EF4
	public bool IsTransmogrified { get; set; }

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x0600557E RID: 21886 RVA: 0x00206D00 File Offset: 0x00204F00
	// (set) Token: 0x0600557F RID: 21887 RVA: 0x00206D08 File Offset: 0x00204F08
	public AIActor.ActorState State { get; set; }

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x06005580 RID: 21888 RVA: 0x00206D14 File Offset: 0x00204F14
	public bool HasBeenAwoken
	{
		get
		{
			return this.State != AIActor.ActorState.Inactive && this.State != AIActor.ActorState.Awakening;
		}
	}

	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x06005581 RID: 21889 RVA: 0x00206D30 File Offset: 0x00204F30
	// (set) Token: 0x06005582 RID: 21890 RVA: 0x00206D38 File Offset: 0x00204F38
	public AIActor.AwakenAnimationType AwakenAnimType { get; set; }

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x06005583 RID: 21891 RVA: 0x00206D44 File Offset: 0x00204F44
	public virtual bool InBossAmmonomiconTab
	{
		get
		{
			return base.healthHaver && base.healthHaver.IsBoss && !base.healthHaver.IsSubboss;
		}
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x00206D78 File Offset: 0x00204F78
	public static AIActor Spawn(AIActor prefabActor, IntVector2 position, RoomHandler source, bool correctForWalls = false, AIActor.AwakenAnimationType awakenAnimType = AIActor.AwakenAnimationType.Default, bool autoEngage = true)
	{
		if (!prefabActor)
		{
			return null;
		}
		GameObject gameObject = prefabActor.gameObject;
		if (prefabActor is AIActorDummy)
		{
			gameObject = (prefabActor as AIActorDummy).realPrefab;
		}
		GameObject gameObject2 = DungeonPlaceableUtility.InstantiateDungeonPlaceable(gameObject, source, position - source.area.basePosition, false, awakenAnimType, autoEngage);
		if (!gameObject2)
		{
			return null;
		}
		AIActor component = gameObject2.GetComponent<AIActor>();
		if (!component)
		{
			return null;
		}
		component.specRigidbody.Initialize();
		if (correctForWalls)
		{
			component.CorrectForWalls();
		}
		return component;
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x00206E08 File Offset: 0x00205008
	public static AIActor Spawn(AIActor prefabActor, Vector2 position, RoomHandler source, bool correctForWalls = false, AIActor.AwakenAnimationType awakenAnimType = AIActor.AwakenAnimationType.Default, bool autoEngage = true)
	{
		GameObject gameObject = prefabActor.gameObject;
		if (prefabActor is AIActorDummy)
		{
			gameObject = (prefabActor as AIActorDummy).realPrefab;
		}
		IntVector2 intVector = position.ToIntVector2(VectorConversions.Floor);
		GameObject gameObject2 = DungeonPlaceableUtility.InstantiateDungeonPlaceable(gameObject, source, intVector - source.area.basePosition, false, awakenAnimType, autoEngage);
		if (!gameObject2)
		{
			return null;
		}
		AIActor component = gameObject2.GetComponent<AIActor>();
		if (!component)
		{
			return null;
		}
		component.specRigidbody.Initialize();
		component.transform.position -= component.specRigidbody.UnitCenter - position;
		component.specRigidbody.Reinitialize();
		if (correctForWalls)
		{
			component.CorrectForWalls();
		}
		return component;
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x00206ECC File Offset: 0x002050CC
	private void CorrectForWalls()
	{
		bool flag = PhysicsEngine.Instance.OverlapCast(base.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]);
		if (flag)
		{
			Vector2 vector = base.transform.position.XY();
			IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
			int num = 0;
			int num2 = 1;
			for (;;)
			{
				for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
				{
					base.transform.position = vector + PhysicsEngine.PixelToUnit(cardinalsAndOrdinals[i] * num2);
					base.specRigidbody.Reinitialize();
					if (!PhysicsEngine.Instance.OverlapCast(base.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]))
					{
						return;
					}
				}
				num2++;
				num++;
				if (num > 200)
				{
					goto Block_4;
				}
			}
			return;
			Block_4:
			UnityEngine.Debug.LogError("FREEZE AVERTED!  TELL RUBEL!  (you're welcome) 147");
			return;
		}
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x00206FF0 File Offset: 0x002051F0
	public override void Awake()
	{
		base.Awake();
		this.BaseMovementSpeed = this.MovementSpeed;
		this.m_currentlyAppliedEnemyScale = Vector2.one;
		StaticReferenceManager.AllEnemies.Add(this);
		if (base.healthHaver && base.healthHaver.healthIsNumberOfHits)
		{
			base.healthHaver.SetHealthMaximum(base.healthHaver.GetMaxHealth(), null, false);
		}
		else
		{
			base.healthHaver.SetHealthMaximum(base.healthHaver.GetMaxHealth() * AIActor.BaseLevelHealthModifier, null, false);
			base.healthHaver.SetHealthMaximum(base.healthHaver.GetMaxHealth() * AIActor.HealthModifier, null, false);
		}
		if (GameManager.Instance.InTutorial && base.name.Contains("turret", true))
		{
			this.HasDonePlayerEnterCheck = true;
		}
		this.m_customEngageDoer = base.GetComponent<CustomEngageDoer>();
		this.m_customReinforceDoer = base.GetComponent<CustomReinforceDoer>();
		this.m_rigidbodyExcluder = new Func<SpeculativeRigidbody, bool>(this.RigidbodyBlocksLineOfSight);
		if (base.aiShooter != null)
		{
			base.aiShooter.Initialize();
		}
		this.InitializeCallbacks();
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x00207130 File Offset: 0x00205330
	public override void Start()
	{
		base.Start();
		if (this.UsesVaryingEmissiveShaderPropertyBlock && base.sprite is tk2dSprite)
		{
			tk2dSprite tk2dSprite = base.sprite as tk2dSprite;
			tk2dSprite.ApplyEmissivePropertyBlock = true;
		}
		if (GameManager.Instance.InTutorial && base.name.Contains("turret", true))
		{
			List<AIActor> activeEnemies = this.parentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			Transform transform = base.transform;
			Transform transform2 = base.transform;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor = activeEnemies[i];
				if (aiactor.name.Contains("turret", true))
				{
					if (aiactor.transform.position.y < transform.position.y)
					{
						transform = aiactor.transform;
					}
					if (aiactor.transform.position.y > transform2.position.y)
					{
						transform2 = aiactor.transform;
					}
				}
			}
			if (transform != base.transform && transform2 != base.transform)
			{
				foreach (AIBulletBank.Entry entry in base.bulletBank.Bullets)
				{
					entry.PlayAudio = false;
				}
			}
		}
		if (this.PreventFallingInPitsEver && base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.NoPitsMovementRestrictor));
		}
		if (!string.IsNullOrEmpty(this.EnemySwitchState))
		{
			AkSoundEngine.SetSwitch("CHR_Enemy", this.EnemySwitchState, base.gameObject);
		}
		this.m_spriteDimensions = base.sprite.GetUntrimmedBounds().size;
		if (this.forceUsesTrimmedBounds)
		{
			base.sprite.depthUsesTrimmedBounds = true;
		}
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -1f);
		DepthLookupManager.ProcessRenderer(base.renderer);
		this.m_spawnPosition = base.transform.position;
		if (this.HitByEnemyBullets)
		{
			base.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
		}
		if (this.HasShadow)
		{
			if (!this.ShadowObject)
			{
				base.GenerateDefaultBlobShadow(this.shadowHeightOffset);
			}
			tk2dBaseSprite component = this.ShadowObject.GetComponent<tk2dSprite>();
			base.sprite.AttachRenderer(component);
			component.HeightOffGround = -0.05f;
			if (this.ShadowParent)
			{
				component.transform.parent = this.ShadowParent;
				component.transform.localPosition = Vector3.zero;
			}
			if (GameManager.Instance.InTutorial && base.name.Contains("turret", true))
			{
				component.renderer.enabled = false;
			}
		}
		base.gameObject.GetOrAddComponent<AkGameObj>();
		this.m_lastPosition = base.specRigidbody.UnitCenter;
		foreach (PixelCollider pixelCollider in base.specRigidbody.PixelColliders)
		{
			if (pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.BagelCollider && pixelCollider.CollisionLayer == CollisionLayer.BulletBlocker)
			{
				SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
				specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.ReflectBulletPreCollision));
				break;
			}
		}
		if ((this.PathableTiles & CellTypes.PIT) == CellTypes.PIT)
		{
			base.SetIsFlying(true, "innate flight", true, false);
		}
		this.InitializePalette();
		this.CheckForBlackPhantomness();
		if (this.procedurallyOutlined)
		{
			bool? forcedOutlines = this.m_forcedOutlines;
			if (forcedOutlines == null || this.m_forcedOutlines.Value)
			{
				this.SetOutlines(true);
			}
		}
		if (this.invisibleUntilAwaken)
		{
			if (this.State == AIActor.ActorState.Inactive)
			{
				this.ToggleRenderers(false);
			}
			if (!this.HasBeenAwoken)
			{
				base.specRigidbody.CollideWithOthers = false;
				base.IsGone = true;
				if (base.knockbackDoer)
				{
					base.knockbackDoer.SetImmobile(true, "awaken");
				}
			}
		}
		if (GameManager.Instance.InTutorial && base.name.StartsWith("BulletManTutorial"))
		{
			this.WanderHack();
		}
		if (this.OnPostStartInitialization != null)
		{
			this.OnPostStartInitialization();
		}
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x0020763C File Offset: 0x0020583C
	public void SetOverrideOutlineColor(Color c)
	{
		this.OverrideOutlineColor = new Color?(c);
		if (SpriteOutlineManager.HasOutline(base.sprite))
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", c);
			}
			HealthHaver healthHaver = base.healthHaver;
			if (healthHaver)
			{
				healthHaver.UpdateCachedOutlineColor(outlineMaterial, c);
			}
		}
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x002076A4 File Offset: 0x002058A4
	public void ClearOverrideOutlineColor()
	{
		this.OverrideOutlineColor = null;
		if (SpriteOutlineManager.HasOutline(base.sprite))
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", this.OutlineColor);
			}
		}
	}

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x0600558B RID: 21899 RVA: 0x002076FC File Offset: 0x002058FC
	private Color OutlineColor
	{
		get
		{
			if (this.OverrideOutlineColor != null)
			{
				return this.OverrideOutlineColor.Value;
			}
			return (!this.CanBeBloodthirsted) ? Color.black : Color.red;
		}
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x00207734 File Offset: 0x00205934
	public void SetOutlines(bool value)
	{
		if (!this.procedurallyOutlined)
		{
			return;
		}
		if (value)
		{
			if (!SpriteOutlineManager.HasOutline(base.sprite))
			{
				SpriteOutlineManager.AddOutlineToSprite(base.sprite, this.OutlineColor, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
			this.m_forcedOutlines = new bool?(true);
			return;
		}
		if (!value)
		{
			if (SpriteOutlineManager.HasOutline(base.sprite))
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			}
			this.m_forcedOutlines = new bool?(false);
			return;
		}
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x002077BC File Offset: 0x002059BC
	private void NoPitsMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (specRigidbody && specRigidbody.GroundPixelCollider != null)
		{
			Vector2 vector = specRigidbody.GroundPixelCollider.UnitCenter + PhysicsEngine.PixelToUnit(pixelOffset);
			if (GameManager.Instance.Dungeon.CellSupportsFalling(vector))
			{
				validLocation = false;
			}
		}
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x00207814 File Offset: 0x00205A14
	public string GetActorName()
	{
		if (!string.IsNullOrEmpty(this.OverrideDisplayName))
		{
			return StringTableManager.GetEnemiesString(this.OverrideDisplayName, -1);
		}
		if (base.encounterTrackable)
		{
			return base.encounterTrackable.journalData.GetPrimaryDisplayName(false);
		}
		return StringTableManager.GetEnemiesString("#KILLEDBYDEFAULT", -1);
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x0020786C File Offset: 0x00205A6C
	private void UpdateTurboMode()
	{
		if (this.CompanionOwner)
		{
			return;
		}
		if (this.m_cachedTurboness && !GameManager.IsTurboMode)
		{
			this.m_cachedTurboness = false;
			this.MovementSpeed /= TurboModeController.sEnemyMovementSpeedMultiplier;
			if (base.behaviorSpeculator)
			{
				base.behaviorSpeculator.CooldownScale /= TurboModeController.sEnemyCooldownMultiplier;
			}
		}
		else if (!this.m_cachedTurboness && GameManager.IsTurboMode)
		{
			this.m_cachedTurboness = true;
			this.MovementSpeed *= TurboModeController.sEnemyMovementSpeedMultiplier;
			if (base.behaviorSpeculator)
			{
				base.behaviorSpeculator.CooldownScale *= TurboModeController.sEnemyCooldownMultiplier;
			}
		}
		if (this.m_cachedTurboness && !this.m_turboWake && this.State == AIActor.ActorState.Awakening)
		{
			this.m_turboWake = true;
			if (base.aiAnimator)
			{
				base.aiAnimator.FpsScale *= TurboModeController.sEnemyWakeTimeMultiplier;
			}
		}
		else if ((!this.m_cachedTurboness || this.State != AIActor.ActorState.Awakening) && this.m_turboWake)
		{
			this.m_turboWake = false;
			if (base.aiAnimator)
			{
				base.aiAnimator.FpsScale /= TurboModeController.sEnemyWakeTimeMultiplier;
			}
		}
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x002079E0 File Offset: 0x00205BE0
	public override void Update()
	{
		base.Update();
		if (this.IsMimicEnemy)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
			if (absoluteRoomFromPosition != null && absoluteRoomFromPosition != this.parentRoom)
			{
				if (this.parentRoom != null)
				{
					this.parentRoom.DeregisterEnemy(this, false);
				}
				this.parentRoom = absoluteRoomFromPosition;
				this.parentRoom.RegisterEnemy(this);
			}
		}
		if (this.ReflectsProjectilesWhileInvulnerable && base.specRigidbody && base.spriteAnimator)
		{
			base.specRigidbody.ReflectProjectiles = base.spriteAnimator.QueryInvulnerabilityFrame();
			base.specRigidbody.ReflectBeams = base.spriteAnimator.QueryInvulnerabilityFrame();
		}
		if (this.State == AIActor.ActorState.Awakening && (string.IsNullOrEmpty(this.m_awakenAnimation) || (base.aiAnimator && !base.aiAnimator.IsPlaying(this.m_awakenAnimation))))
		{
			if (base.aiShooter)
			{
				base.aiShooter.ToggleGunAndHandRenderers(true, "Reinforce");
				base.aiShooter.ToggleGunAndHandRenderers(true, "Awaken");
			}
			this.State = AIActor.ActorState.Normal;
		}
		if (this.invisibleUntilAwaken)
		{
			if (this.State == AIActor.ActorState.Inactive && base.renderer.enabled)
			{
				this.ToggleRenderers(false);
			}
			if (this.State == AIActor.ActorState.Normal)
			{
				base.specRigidbody.CollideWithOthers = true;
				base.IsGone = false;
				if (base.knockbackDoer)
				{
					base.knockbackDoer.SetImmobile(false, "awaken");
				}
				this.invisibleUntilAwaken = false;
			}
		}
		if ((this.PathableTiles & CellTypes.PIT) != CellTypes.PIT)
		{
			base.HandlePitChecks();
		}
		if (base.healthHaver.IsDead)
		{
			base.specRigidbody.Velocity = ((!this.PreventDeathKnockback) ? this.m_knockbackVelocity : Vector2.zero);
			return;
		}
		CellVisualData.CellFloorType floorTypeFromPosition = GameManager.Instance.Dungeon.GetFloorTypeFromPosition(base.specRigidbody.UnitBottomCenter);
		if (this.m_prevFloorType == null || this.m_prevFloorType.Value != floorTypeFromPosition)
		{
			this.m_prevFloorType = new CellVisualData.CellFloorType?(floorTypeFromPosition);
			AkSoundEngine.SetSwitch("FS_Surfaces", AIActor.s_floorTypeNames[(int)floorTypeFromPosition], base.gameObject);
		}
		if (base.aiShooter != null)
		{
			base.aiShooter.AimAtTarget();
		}
		if (base.isActiveAndEnabled)
		{
		}
		Vector2 voluntaryMovementVelocity = this.VoluntaryMovementVelocity;
		if (this.UseMovementAudio)
		{
			bool flag = voluntaryMovementVelocity != Vector2.zero;
			if (flag && !this.m_audioMovedLastFrame)
			{
				AkSoundEngine.PostEvent(this.StartMovingEvent, base.gameObject);
			}
			else if (!flag && this.m_audioMovedLastFrame)
			{
				AkSoundEngine.PostEvent(this.StopMovingEvent, base.gameObject);
			}
			this.m_audioMovedLastFrame = flag;
		}
		base.specRigidbody.Velocity = base.ApplyMovementModifiers(voluntaryMovementVelocity, this.m_knockbackVelocity) * this.LocalTimeScale;
		base.specRigidbody.Velocity += this.ImpartedVelocity;
		this.ImpartedVelocity = Vector2.zero;
		if (this.m_isSafeMoving)
		{
			this.m_safeMoveTimer += BraveTime.DeltaTime;
			base.transform.position = Vector2.Lerp(this.m_safeMoveStartPos.Value, this.m_safeMoveEndPos.Value, Mathf.Clamp01(this.m_safeMoveTimer / this.m_safeMoveTime));
			base.specRigidbody.Reinitialize();
			if (this.m_safeMoveTimer >= this.m_safeMoveTime)
			{
				this.m_isSafeMoving = false;
			}
		}
		this.m_lastPosition = base.specRigidbody.UnitCenter;
		if (this.IsBlackPhantom)
		{
			this.UpdateBlackPhantomShaders();
		}
		if (this.IsBlackPhantom || this.ForceBlackPhantomParticles)
		{
			this.UpdateBlackPhantomParticles();
		}
		this.ProcessHealthOverrides();
		if (base.healthHaver && base.healthHaver.IsBoss)
		{
			if (base.FreezeAmount > 0f)
			{
				float num = base.GetResistanceForEffectType(EffectResistanceType.Freeze);
				num = Mathf.Clamp(num + 0.01f * BraveTime.DeltaTime, 0.6f, 1f);
				base.SetResistance(EffectResistanceType.Freeze, num);
			}
			if (base.GetEffect(EffectResistanceType.Fire) != null)
			{
				float num2 = base.GetResistanceForEffectType(EffectResistanceType.Fire);
				num2 = Mathf.Clamp(num2 + 0.025f * BraveTime.DeltaTime, 0.25f, 0.75f);
				base.SetResistance(EffectResistanceType.Fire, num2);
			}
		}
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x00207E94 File Offset: 0x00206094
	private void UpdateBlackPhantomShaders()
	{
		if (base.healthHaver.bodySprites.Count != this.m_cachedBodySpriteCount)
		{
			this.m_cachedBodySpriteCount = base.healthHaver.bodySprites.Count;
			for (int i = 0; i < base.healthHaver.bodySprites.Count; i++)
			{
				tk2dBaseSprite tk2dBaseSprite = base.healthHaver.bodySprites[i];
				tk2dBaseSprite.usesOverrideMaterial = true;
				Material material = tk2dBaseSprite.renderer.material;
				if (this.m_cachedBodySpriteShader == null)
				{
					this.m_cachedBodySpriteShader = material.shader;
				}
				if (this.IsBlackPhantom)
				{
					if (this.OverrideBlackPhantomShader != null)
					{
						material.shader = this.OverrideBlackPhantomShader;
					}
					else
					{
						material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
						material.SetFloat("_PhantomGradientScale", this.BlackPhantomProperties.GradientScale);
						material.SetFloat("_PhantomContrastPower", this.BlackPhantomProperties.ContrastPower);
						if (tk2dBaseSprite != base.sprite)
						{
							material.SetFloat("_ApplyFade", 0f);
						}
					}
				}
				else
				{
					material.shader = this.m_cachedBodySpriteShader;
				}
				tk2dBaseSprite.renderer.material = material;
			}
			if (base.aiShooter && base.aiShooter.CurrentGun)
			{
				tk2dBaseSprite sprite = base.aiShooter.CurrentGun.GetSprite();
				sprite.usesOverrideMaterial = true;
				Material material2 = sprite.renderer.material;
				if (this.m_cachedGunSpriteShader == null)
				{
					this.m_cachedGunSpriteShader = material2.shader;
				}
				if (this.IsBlackPhantom)
				{
					if (this.OverrideBlackPhantomShader != null)
					{
						material2.shader = this.OverrideBlackPhantomShader;
					}
					else
					{
						material2.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
						material2.SetFloat("_PhantomGradientScale", this.BlackPhantomProperties.GradientScale);
						material2.SetFloat("_PhantomContrastPower", this.BlackPhantomProperties.ContrastPower);
						material2.SetFloat("_ApplyFade", 0.3f);
					}
				}
				else
				{
					material2.shader = this.m_cachedBodySpriteShader;
				}
				sprite.renderer.material = material2;
			}
		}
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x002080E4 File Offset: 0x002062E4
	private void UpdateBlackPhantomParticles()
	{
		if (this.ShouldDoBlackPhantomParticles == null)
		{
			if (base.GetComponent<DraGunDeathController>())
			{
				this.ShouldDoBlackPhantomParticles = new bool?(false);
			}
			else
			{
				this.ShouldDoBlackPhantomParticles = new bool?(true);
			}
		}
		if (this.ShouldDoBlackPhantomParticles != null && !this.ShouldDoBlackPhantomParticles.Value)
		{
			return;
		}
		if (this.HasBeenEngaged && (!base.sprite || base.sprite.renderer.enabled) && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			PixelCollider pixelCollider = ((!this.OverrideBlackPhantomParticlesCollider) ? base.specRigidbody.HitboxPixelCollider : base.specRigidbody.PixelColliders[this.BlackPhantomParticlesCollider]);
			Vector3 vector = pixelCollider.UnitBottomLeft.ToVector3ZisY(0f);
			Vector3 vector2 = pixelCollider.UnitTopRight.ToVector3ZisY(0f);
			float num = (vector2.y - vector.y) * (vector2.x - vector.x);
			float num2 = 40f * num;
			int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
			GlobalSparksDoer.SparksType sparksType = ((!this.ForceBlackPhantomParticles) ? GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE : GlobalSparksDoer.SparksType.DARK_MAGICKS);
			int num4 = num3;
			Vector3 vector3 = vector;
			Vector3 vector4 = vector2;
			Vector3 vector5 = Vector3.up / 2f;
			float num5 = 120f;
			float num6 = 0.2f;
			float? num7 = new float?(UnityEngine.Random.Range(1f, 1.65f));
			GlobalSparksDoer.DoRandomParticleBurst(num4, vector3, vector4, vector5, num5, num6, null, num7, null, sparksType);
			if (UnityEngine.Random.value < 0.5f)
			{
				num4 = 1;
				vector5 = vector;
				vector4 = vector2.WithY(vector.y + 0.1f);
				vector3 = Vector3.right / 2f;
				num6 = 25f;
				num5 = 0.2f;
				num7 = new float?(UnityEngine.Random.Range(1f, 1.65f));
				GlobalSparksDoer.DoRandomParticleBurst(num4, vector5, vector4, vector3, num6, num5, null, num7, null, sparksType);
			}
			else
			{
				num4 = 1;
				vector3 = vector;
				vector4 = vector2.WithY(vector.y + 0.1f);
				vector5 = Vector3.left / 2f;
				num5 = 25f;
				num6 = 0.2f;
				num7 = new float?(UnityEngine.Random.Range(1f, 1.65f));
				GlobalSparksDoer.DoRandomParticleBurst(num4, vector3, vector4, vector5, num5, num6, null, num7, null, sparksType);
			}
		}
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x002083B8 File Offset: 0x002065B8
	public void LateUpdate()
	{
		base.sprite.UpdateZDepth();
		this.UpdateTurboMode();
		if (base.renderer && base.renderer.material && base.HasOverrideColor() && !this.OverrideColorOverridden && (this.m_colorOverridenMaterial != base.renderer.material || this.m_colorOverridenShader != base.renderer.material.shader))
		{
			base.OnOverrideColorsChanged();
		}
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x00208454 File Offset: 0x00206654
	protected virtual void OnWillRenderObject()
	{
		if (Pixelator.IsRenderingReflectionMap)
		{
			base.sprite.renderer.sharedMaterial.SetFloat("_ReflectionYOffset", base.sprite.GetBounds().min.y * 2f + this.actorReflectionAdditionalOffset);
		}
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x002084B0 File Offset: 0x002066B0
	protected override void OnDestroy()
	{
		if (this.CurrentGun != null)
		{
			this.CurrentGun.DespawnVFX();
		}
		StaticReferenceManager.AllEnemies.Remove(this);
		bool flag = !base.healthHaver || !base.healthHaver.IsBoss;
		if (GameManager.IsShuttingDown || GameManager.IsReturningToBreach || !GameManager.HasInstance || GameManager.Instance.IsLoadingLevel)
		{
			flag = false;
		}
		if (this.ParentRoom != null && flag)
		{
			this.ParentRoom.DeregisterEnemy(this, false);
		}
		if (this.parentRoom != null)
		{
			this.parentRoom.Entered -= this.OnPlayerEntered;
		}
		this.DeregisterCallbacks();
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x0020857C File Offset: 0x0020677C
	public void CompanionWarp(Vector3 targetPosition)
	{
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Breakable_Column_Puff");
		GameObject gameObject2 = SpawnManager.SpawnVFX(gameObject, false);
		gameObject2.GetComponent<tk2dSprite>().PlaceAtPositionByAnchor(base.specRigidbody.UnitBottomCenter, tk2dBaseSprite.Anchor.LowerCenter);
		Vector2 vector = base.specRigidbody.UnitBottomCenter - base.transform.position.XY();
		base.transform.position = targetPosition - vector.ToVector3ZUp(0f);
		base.specRigidbody.Reinitialize();
		this.CorrectForWalls();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		gameObject2 = SpawnManager.SpawnVFX(gameObject, false);
		gameObject2.GetComponent<tk2dSprite>().PlaceAtPositionByAnchor(base.specRigidbody.UnitBottomCenter, tk2dBaseSprite.Anchor.LowerCenter);
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x0020864C File Offset: 0x0020684C
	public void WanderHack()
	{
		if (base.behaviorSpeculator)
		{
			base.behaviorSpeculator.enabled = false;
		}
		base.StartCoroutine(this.WanderHackCR());
	}

	// Token: 0x06005598 RID: 21912 RVA: 0x00208678 File Offset: 0x00206878
	private IEnumerator WanderHackCR()
	{
		this.ClearPath();
		this.PlayerTarget = null;
		this.OverrideTarget = null;
		yield return new WaitForSeconds(1f);
		for (;;)
		{
			IntVector2 targetPos = this.PathTile + new IntVector2(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));
			if (GameManager.Instance.Dungeon.data.cellData[targetPos.x][targetPos.y].type == CellType.FLOOR && !GameManager.Instance.Dungeon.data.isTopWall(targetPos.x, targetPos.y))
			{
				this.PathfindToPosition(targetPos.ToCenterVector2(), null, true, null, null, null, false);
				while (!this.PathComplete)
				{
					yield return null;
				}
				yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
			}
		}
		yield break;
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x00208694 File Offset: 0x00206894
	public bool PathfindToPosition(Vector2 targetPosition, Vector2? overridePathEnd = null, bool smooth = true, CellValidator cellValidator = null, ExtraWeightingFunction extraWeightingFunction = null, CellTypes? overridePathableTiles = null, bool canPassOccupied = false)
	{
		bool flag = false;
		Pathfinder.Instance.RemoveActorPath(this.m_upcomingPathTiles);
		CellTypes cellTypes = ((overridePathableTiles == null) ? this.PathableTiles : overridePathableTiles.Value);
		Path path = null;
		if (Pathfinder.Instance.GetPath(this.PathTile, targetPosition.ToIntVector2(VectorConversions.Floor), out path, new IntVector2?(this.Clearance), cellTypes, cellValidator, extraWeightingFunction, canPassOccupied))
		{
			this.m_currentPath = path;
			this.m_overridePathEnd = overridePathEnd;
			if (this.m_currentPath != null && this.m_currentPath.WillReachFinalGoal)
			{
				flag = true;
			}
			if (this.m_currentPath.Count == 0)
			{
				this.m_currentPath = null;
			}
			else if (smooth)
			{
				path.Smooth(base.specRigidbody.UnitCenter, base.specRigidbody.UnitDimensions / 2f, cellTypes, canPassOccupied, this.Clearance);
			}
		}
		this.UpdateUpcomingPathTiles(2f);
		Pathfinder.Instance.UpdateActorPath(this.m_upcomingPathTiles);
		return flag;
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x0020879C File Offset: 0x0020699C
	public void FakePathToPosition(Vector2 targetPosition)
	{
		Pathfinder.Instance.RemoveActorPath(this.m_upcomingPathTiles);
		this.m_currentPath = null;
		this.m_overridePathEnd = new Vector2?(targetPosition);
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x002087C4 File Offset: 0x002069C4
	public void ClearPath()
	{
		Pathfinder.Instance.RemoveActorPath(this.m_upcomingPathTiles);
		this.m_upcomingPathTiles.Clear();
		this.m_upcomingPathTiles.Add(this.PathTile);
		Pathfinder.Instance.UpdateActorPath(this.m_upcomingPathTiles);
		this.m_currentPath = null;
		this.m_overridePathEnd = null;
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x00208824 File Offset: 0x00206A24
	private bool GetNextTargetPosition(out Vector2 targetPos)
	{
		if (this.m_currentPath != null && this.m_currentPath.Count > 0)
		{
			targetPos = this.m_currentPath.GetFirstCenterVector2();
			return true;
		}
		Vector2? overridePathEnd = this.m_overridePathEnd;
		if (overridePathEnd != null)
		{
			targetPos = this.m_overridePathEnd.Value;
			return true;
		}
		targetPos = Vector2.zero;
		return false;
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x00208894 File Offset: 0x00206A94
	private Vector2 GetPathTarget()
	{
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 vector = unitCenter;
		float num = this.MovementSpeed * this.LocalDeltaTime;
		Vector2 vector2 = unitCenter;
		Vector2 vector3 = unitCenter;
		while (num > 0f)
		{
			if (this.GetNextTargetPosition(out vector3))
			{
				float num2 = Vector2.Distance(vector3, unitCenter);
				if (num2 < num)
				{
					num -= num2;
					vector2 = vector3;
					vector = vector2;
					if (this.m_currentPath != null && this.m_currentPath.Count > 0)
					{
						this.m_currentPath.RemoveFirst();
					}
					else
					{
						this.m_overridePathEnd = null;
					}
					continue;
				}
				vector = (vector3 - vector2).normalized * num + vector2;
			}
			return vector;
		}
		return vector;
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x00208970 File Offset: 0x00206B70
	private Vector2 GetPathVelocityContribution()
	{
		Vector2? overridePathVelocity = this.OverridePathVelocity;
		if (overridePathVelocity != null)
		{
			return this.OverridePathVelocity.Value;
		}
		if (this.m_currentPath == null || this.m_currentPath.Count == 0)
		{
			Vector2? overridePathEnd = this.m_overridePathEnd;
			if (overridePathEnd == null)
			{
				return Vector2.zero;
			}
		}
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 pathTarget = this.GetPathTarget();
		Vector2 vector = pathTarget - unitCenter;
		if (this.MovementSpeed * this.LocalDeltaTime > vector.magnitude)
		{
			return vector / this.LocalDeltaTime;
		}
		return this.MovementSpeed * vector.normalized;
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x00208A28 File Offset: 0x00206C28
	private Vector2 GetPathVelocityContribution_Old()
	{
		if (this.m_currentPath == null || this.m_currentPath.Count == 0)
		{
			Vector2? overridePathEnd = this.m_overridePathEnd;
			if (overridePathEnd == null)
			{
				return Vector2.zero;
			}
		}
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 vector;
		if (this.m_currentPath != null)
		{
			vector = this.m_currentPath.GetFirstCenterVector2();
		}
		else
		{
			vector = this.m_overridePathEnd.Value;
		}
		int num = ((this.m_currentPath != null) ? this.m_currentPath.Count : 0);
		Vector2? overridePathEnd2 = this.m_overridePathEnd;
		bool flag = num + ((overridePathEnd2 != null) ? 1 : 0) == 1;
		bool flag2 = false;
		if (Vector2.Distance(unitCenter, vector) < PhysicsEngine.PixelToUnit(1))
		{
			flag2 = true;
		}
		else if (!flag)
		{
			Vector2 vector2 = BraveMathCollege.ClosestPointOnLineSegment(vector, this.m_lastPosition, unitCenter);
			if (Vector2.Distance(vector, vector2) < PhysicsEngine.PixelToUnit(1))
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			if (this.m_currentPath != null && this.m_currentPath.Count > 0)
			{
				this.m_currentPath.RemoveFirst();
				if (this.m_currentPath.Count == 0)
				{
					this.m_currentPath = null;
					return Vector2.zero;
				}
			}
			else
			{
				Vector2? overridePathEnd3 = this.m_overridePathEnd;
				if (overridePathEnd3 != null)
				{
					this.m_overridePathEnd = null;
				}
			}
		}
		Vector2 vector3 = vector - unitCenter;
		if (flag && this.MovementSpeed * this.LocalDeltaTime > vector3.magnitude)
		{
			return vector3 / this.LocalDeltaTime;
		}
		return this.MovementSpeed * vector3.normalized;
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x00208BE4 File Offset: 0x00206DE4
	public void ReflectBulletPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x00208BE8 File Offset: 0x00206DE8
	protected bool CheckTableRaycast(SpeculativeRigidbody source, SpeculativeRigidbody target)
	{
		if (target == null || source == null)
		{
			return true;
		}
		Vector2 unitCenter = source.GetUnitCenter(ColliderType.Ground);
		Vector2 unitCenter2 = target.GetUnitCenter(ColliderType.Ground);
		Vector2 vector = unitCenter2 - unitCenter;
		RaycastResult raycastResult;
		if (PhysicsEngine.Instance.RaycastWithIgnores(unitCenter, vector, vector.magnitude, out raycastResult, false, true, CollisionMask.LayerToMask(CollisionLayer.LowObstacle, CollisionLayer.HighObstacle), null, false, null, new SpeculativeRigidbody[] { source, target }))
		{
			RaycastResult.Pool.Free(ref raycastResult);
			return false;
		}
		return true;
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x00208C74 File Offset: 0x00206E74
	protected virtual void OnCollision(CollisionData collision)
	{
		if (this.ManualKnockbackHandling)
		{
			return;
		}
		if (collision.collisionType == CollisionData.CollisionType.Rigidbody)
		{
			if (base.IsFrozen)
			{
				PlayerController component = collision.OtherRigidbody.GetComponent<PlayerController>();
				if (component && collision.Overlap)
				{
					component.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
				}
			}
			else
			{
				if (this.CanTargetPlayers)
				{
					PlayerController component2 = collision.OtherRigidbody.GetComponent<PlayerController>();
					if (!base.healthHaver.IsDead && component2 != null)
					{
						bool flag = this.CheckTableRaycast(collision.MyRigidbody, collision.OtherRigidbody);
						if (flag)
						{
							Vector2 vector = (component2.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter).normalized;
							if (component2.IsDodgeRolling)
							{
								component2.ApplyRollDamage(this);
							}
							if (component2.ReceivesTouchDamage)
							{
								float num = this.CollisionDamage;
								if (this.IsBlackPhantom)
								{
									num = 1f;
								}
								if (base.IsCheezen)
								{
									num = 0f;
								}
								component2.healthHaver.ApplyDamage(num, vector, this.GetActorName(), CoreDamageTypes.None, (!this.IsBlackPhantom) ? DamageCategory.Collision : DamageCategory.BlackBullet, false, null, false);
								if (Mathf.Approximately(vector.magnitude, 0f))
								{
									vector = UnityEngine.Random.insideUnitCircle.normalized;
								}
								component2.knockbackDoer.ApplySourcedKnockback(vector, this.CollisionKnockbackStrength, base.gameObject, false);
								if (base.knockbackDoer)
								{
									base.knockbackDoer.ApplySourcedKnockback(-vector, component2.collisionKnockbackStrength, base.gameObject, false);
								}
							}
							else
							{
								if (Mathf.Approximately(vector.magnitude, 0f))
								{
									vector = UnityEngine.Random.insideUnitCircle.normalized;
								}
								component2.knockbackDoer.ApplySourcedKnockback(vector, Mathf.Max(50f, this.CollisionKnockbackStrength), base.gameObject, false);
								if (base.knockbackDoer)
								{
									base.knockbackDoer.ApplySourcedKnockback(-vector, Mathf.Max(50f, component2.collisionKnockbackStrength), base.gameObject, false);
								}
							}
							if (this.CollisionSetsPlayerOnFire)
							{
								component2.IsOnFire = true;
							}
							this.CollisionVFX.SpawnAtPosition(collision.Contact, 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(2f), false, null, null, false);
							if (collision.Overlap)
							{
								component2.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
							}
							if (this.DiesOnCollison)
							{
								base.healthHaver.ApplyDamage(1000f, -vector, "Contact", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
							}
						}
					}
				}
				if (this.CanTargetEnemies || this.OverrideHitEnemies)
				{
					AIActor component3 = collision.OtherRigidbody.GetComponent<AIActor>();
					if (component3 != null && !base.healthHaver.IsDead && (this.IsNormalEnemy || component3.IsNormalEnemy))
					{
						Vector2 vector2 = (component3.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter).normalized;
						if (this.CanTargetEnemies)
						{
							component3.healthHaver.ApplyDamage(this.CollisionDamage * 5f, vector2, this.GetActorName(), this.CollisionDamageTypes, DamageCategory.Collision, false, null, false);
						}
						if (Mathf.Approximately(vector2.magnitude, 0f))
						{
							vector2 = UnityEngine.Random.insideUnitCircle.normalized;
						}
						if (component3.knockbackDoer)
						{
							component3.knockbackDoer.ApplySourcedKnockback(vector2, this.EnemyCollisionKnockbackStrength, base.gameObject, false);
						}
						if (base.knockbackDoer)
						{
							base.knockbackDoer.ApplySourcedKnockback(-vector2, component3.EnemyCollisionKnockbackStrength, base.gameObject, false);
						}
						this.CollisionVFX.SpawnAtPosition(collision.Contact, 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(2f), false, null, null, false);
						if (collision.Overlap)
						{
							component3.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
						}
						if (this.DiesOnCollison)
						{
							base.healthHaver.ApplyDamage(1000f, -vector2, "Contact", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
						}
					}
				}
			}
		}
		if (!collision.OtherRigidbody || !collision.OtherRigidbody.gameActor)
		{
			this.NonActorCollisionVFX.SpawnAtPosition(collision.Contact, 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(2f), false, null, null, false);
		}
		this.m_strafeDirection *= -1;
	}

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x060055A3 RID: 21923 RVA: 0x0020917C File Offset: 0x0020737C
	public Vector3 SpawnPosition
	{
		get
		{
			return this.m_spawnPosition;
		}
	}

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x060055A4 RID: 21924 RVA: 0x00209184 File Offset: 0x00207384
	public IntVector2 SpawnGridPosition
	{
		get
		{
			return this.m_spawnPosition.IntXY(VectorConversions.Floor);
		}
	}

	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x060055A5 RID: 21925 RVA: 0x00209194 File Offset: 0x00207394
	public Vector3 Position
	{
		get
		{
			return base.specRigidbody.UnitCenter;
		}
	}

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x060055A6 RID: 21926 RVA: 0x002091A8 File Offset: 0x002073A8
	public IntVector2 GridPosition
	{
		get
		{
			return base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
		}
	}

	// Token: 0x17000C0E RID: 3086
	// (get) Token: 0x060055A7 RID: 21927 RVA: 0x002091BC File Offset: 0x002073BC
	public IntVector2 PathTile
	{
		get
		{
			return base.specRigidbody.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
		}
	}

	// Token: 0x17000C0F RID: 3087
	// (get) Token: 0x060055A8 RID: 21928 RVA: 0x002091D0 File Offset: 0x002073D0
	public bool PathComplete
	{
		get
		{
			bool flag;
			if (this.m_currentPath == null || this.m_currentPath.Count == 0)
			{
				Vector2? overridePathEnd = this.m_overridePathEnd;
				flag = overridePathEnd == null;
			}
			else
			{
				flag = false;
			}
			return flag;
		}
	}

	// Token: 0x17000C10 RID: 3088
	// (get) Token: 0x060055A9 RID: 21929 RVA: 0x0020920C File Offset: 0x0020740C
	public Path Path
	{
		get
		{
			return this.m_currentPath;
		}
	}

	// Token: 0x17000C11 RID: 3089
	// (get) Token: 0x060055AA RID: 21930 RVA: 0x00209214 File Offset: 0x00207414
	public float DistanceToTarget
	{
		get
		{
			SpeculativeRigidbody targetRigidbody = this.TargetRigidbody;
			if (this.TargetRigidbody == null)
			{
				return 0f;
			}
			return Vector2.Distance(base.specRigidbody.UnitCenter, targetRigidbody.GetUnitCenter(ColliderType.HitBox));
		}
	}

	// Token: 0x060055AB RID: 21931 RVA: 0x00209258 File Offset: 0x00207458
	public bool RigidbodyBlocksLineOfSight(SpeculativeRigidbody testRigidbody)
	{
		return testRigidbody.gameObject.CompareTag("Intangible");
	}

	// Token: 0x060055AC RID: 21932 RVA: 0x00209274 File Offset: 0x00207474
	public bool HasLineOfSightToRigidbody(SpeculativeRigidbody targetRigidbody)
	{
		if (targetRigidbody == null)
		{
			return false;
		}
		Vector2 unitCenter = targetRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2 vector = ((!this.LosPoint) ? base.specRigidbody.UnitCenter : this.LosPoint.transform.position.XY());
		float num = Vector2.Distance(vector, unitCenter);
		int complexEnemyVisibilityMask = CollisionMask.GetComplexEnemyVisibilityMask(this.CanTargetPlayers, this.CanTargetEnemies);
		RaycastResult raycastResult;
		if (!PhysicsEngine.Instance.Raycast(vector, unitCenter - vector, num, out raycastResult, true, true, complexEnemyVisibilityMask, null, false, this.m_rigidbodyExcluder, base.specRigidbody))
		{
			RaycastResult.Pool.Free(ref raycastResult);
			return false;
		}
		if (raycastResult.SpeculativeRigidbody == null || raycastResult.SpeculativeRigidbody != targetRigidbody)
		{
			RaycastResult.Pool.Free(ref raycastResult);
			return false;
		}
		RaycastResult.Pool.Free(ref raycastResult);
		return true;
	}

	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x060055AD RID: 21933 RVA: 0x00209368 File Offset: 0x00207568
	public bool HasLineOfSightToTarget
	{
		get
		{
			if (this.TargetRigidbody != this.m_cachedLosTarget || Time.frameCount != this.m_cachedLosFrame)
			{
				this.m_cachedHasLineOfSightToTarget = this.HasLineOfSightToRigidbody(this.TargetRigidbody);
				this.m_cachedLosTarget = this.TargetRigidbody;
				this.m_cachedLosFrame = Time.frameCount;
			}
			return this.m_cachedHasLineOfSightToTarget;
		}
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x002093CC File Offset: 0x002075CC
	public bool HasLineOfSightToTargetFromPosition(Vector2 hypotheticalPosition)
	{
		if (this.TargetRigidbody == null)
		{
			return false;
		}
		Vector2 unitCenter = this.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		float distanceToTarget = this.DistanceToTarget;
		int complexEnemyVisibilityMask = CollisionMask.GetComplexEnemyVisibilityMask(this.CanTargetPlayers, this.CanTargetEnemies);
		RaycastResult raycastResult;
		if (!PhysicsEngine.Instance.Raycast(hypotheticalPosition, unitCenter - hypotheticalPosition, distanceToTarget, out raycastResult, true, true, complexEnemyVisibilityMask, null, false, this.m_rigidbodyExcluder, base.specRigidbody))
		{
			RaycastResult.Pool.Free(ref raycastResult);
			return false;
		}
		if (raycastResult.SpeculativeRigidbody == null || raycastResult.SpeculativeRigidbody != this.TargetRigidbody)
		{
			RaycastResult.Pool.Free(ref raycastResult);
			return false;
		}
		RaycastResult.Pool.Free(ref raycastResult);
		return true;
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x060055AF RID: 21935 RVA: 0x00209498 File Offset: 0x00207698
	public float DesiredCombatDistance
	{
		get
		{
			if (base.behaviorSpeculator == null)
			{
				return -1f;
			}
			return base.behaviorSpeculator.GetDesiredCombatDistance();
		}
	}

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x060055B0 RID: 21936 RVA: 0x002094BC File Offset: 0x002076BC
	public IntVector2 Clearance
	{
		get
		{
			IntVector2? clearance = this.m_clearance;
			if (clearance == null)
			{
				this.m_clearance = new IntVector2?(base.specRigidbody.UnitDimensions.ToIntVector2(VectorConversions.Ceil));
			}
			return this.m_clearance.Value;
		}
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x00209508 File Offset: 0x00207708
	private void CheckForBlackPhantomness()
	{
		if (this.CompanionOwner != null || !this.IsNormalEnemy)
		{
			return;
		}
		if (this.PreventBlackPhantom)
		{
			return;
		}
		int totalCurse = PlayerStats.GetTotalCurse();
		float num;
		if (totalCurse <= 0)
		{
			num = 0f;
		}
		else if (totalCurse <= 2)
		{
			num = 0.01f;
		}
		else if (totalCurse <= 4)
		{
			num = 0.02f;
		}
		else if (totalCurse <= 6)
		{
			num = 0.05f;
		}
		else if (totalCurse <= 8)
		{
			num = 0.1f;
		}
		else if (totalCurse == 9)
		{
			num = 0.25f;
		}
		else
		{
			num = 0.5f;
		}
		if (base.healthHaver.IsBoss)
		{
			if (totalCurse < 7)
			{
				num = 0f;
			}
			else if (totalCurse < 9)
			{
				num = 0.2f;
			}
			else if (totalCurse < 10)
			{
				num = 0.3f;
			}
			else
			{
				num = 0.5f;
			}
		}
		if (this.ForceBlackPhantom || UnityEngine.Random.value < num)
		{
			this.BecomeBlackPhantom();
		}
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x00209628 File Offset: 0x00207828
	public void BecomeBlackPhantom()
	{
		if (!this.IsBlackPhantom)
		{
			this.m_championType = AIActor.EnemyChampionType.JAMMED;
			this.m_cachedBodySpriteCount = -1;
			this.UpdateBlackPhantomShaders();
			if (base.healthHaver && !base.healthHaver.healthIsNumberOfHits)
			{
				float num = this.BlackPhantomProperties.BonusHealthPercentIncrease;
				float num2 = this.BlackPhantomProperties.BonusHealthFlatIncrease;
				if (base.healthHaver.IsBoss)
				{
					num += BlackPhantomProperties.GlobalBossPercentIncrease;
					num2 += BlackPhantomProperties.GlobalBossFlatIncrease;
				}
				else
				{
					num += BlackPhantomProperties.GlobalPercentIncrease;
					num2 += BlackPhantomProperties.GlobalFlatIncrease;
				}
				float num3 = base.healthHaver.GetMaxHealth() * (1f + num) + num2;
				if (this.BlackPhantomProperties.MaxTotalHealth > 0f && !base.healthHaver.IsBoss)
				{
					num3 = Mathf.Min(num3, this.BlackPhantomProperties.MaxTotalHealth * AIActor.BaseLevelHealthModifier);
				}
				base.healthHaver.SetHealthMaximum(num3, null, true);
			}
			this.MovementSpeed *= this.BlackPhantomProperties.MovementSpeedMultiplier;
			if (base.behaviorSpeculator)
			{
				base.behaviorSpeculator.CooldownScale /= this.BlackPhantomProperties.CooldownMultiplier;
			}
		}
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x00209770 File Offset: 0x00207970
	public void UnbecomeBlackPhantom()
	{
		if (this.IsBlackPhantom)
		{
			this.m_championType = AIActor.EnemyChampionType.NORMAL;
			this.m_cachedBodySpriteCount = -1;
			this.UpdateBlackPhantomShaders();
			if (base.healthHaver)
			{
				float num = this.BlackPhantomProperties.BonusHealthPercentIncrease;
				float num2 = this.BlackPhantomProperties.BonusHealthFlatIncrease;
				if (base.healthHaver.IsBoss)
				{
					num += BlackPhantomProperties.GlobalBossPercentIncrease;
					num2 += BlackPhantomProperties.GlobalBossFlatIncrease;
				}
				else
				{
					num += BlackPhantomProperties.GlobalPercentIncrease;
					num2 += BlackPhantomProperties.GlobalFlatIncrease;
				}
				float num3 = (base.healthHaver.GetMaxHealth() - num2) / (1f + num);
				if (this.BlackPhantomProperties.MaxTotalHealth > 0f && !base.healthHaver.IsBoss)
				{
					num3 = Mathf.Max(num3, 10f);
				}
				base.healthHaver.SetHealthMaximum(num3, null, true);
			}
			this.MovementSpeed /= this.BlackPhantomProperties.MovementSpeedMultiplier;
			if (base.behaviorSpeculator)
			{
				base.behaviorSpeculator.CooldownScale *= this.BlackPhantomProperties.CooldownMultiplier;
			}
		}
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x0020989C File Offset: 0x00207A9C
	private void InitializePalette()
	{
		if (this.optionalPalette != null)
		{
			this.m_isPaletteSwapped = true;
			base.sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_COMPLEX;
			base.sprite.renderer.material.SetTexture("_PaletteTex", this.optionalPalette);
		}
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x002098F0 File Offset: 0x00207AF0
	private void ProcessHealthOverrides()
	{
		for (int i = 0; i < this.HealthOverrides.Count; i++)
		{
			AIActor.HealthOverride healthOverride = this.HealthOverrides[i];
			if (!healthOverride.HasBeenUsed && base.healthHaver.GetCurrentHealthPercentage() <= healthOverride.HealthPercentage)
			{
				foreach (FieldInfo fieldInfo in base.GetType().GetFields())
				{
					if (fieldInfo.Name == healthOverride.Stat)
					{
						fieldInfo.SetValue(this, healthOverride.Value);
						healthOverride.HasBeenUsed = true;
						break;
					}
				}
				if (!healthOverride.HasBeenUsed)
				{
					UnityEngine.Debug.LogError("Failed to find the field " + healthOverride.Stat + " on AIActor.");
					healthOverride.HasBeenUsed = true;
				}
			}
		}
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x002099D0 File Offset: 0x00207BD0
	private void UpdateUpcomingPathTiles(float time)
	{
		this.m_upcomingPathTiles.Clear();
		this.m_upcomingPathTiles.Add(this.PathTile);
		if (this.m_currentPath != null && this.m_currentPath.Count > 0)
		{
			float num = 0f;
			Vector2 vector = this.Position;
			LinkedListNode<IntVector2> linkedListNode = this.m_currentPath.Positions.First;
			Vector2 vector2 = linkedListNode.Value.ToCenterVector2();
			while (num < time)
			{
				Vector2 vector3 = vector2 - vector;
				if (vector3.sqrMagnitude > 0.04f)
				{
					vector3 = vector3.normalized * 0.2f;
				}
				vector += vector3;
				IntVector2 intVector = vector.ToIntVector2(VectorConversions.Floor);
				if (this.m_upcomingPathTiles[this.m_upcomingPathTiles.Count - 1] != intVector)
				{
					this.m_upcomingPathTiles.Add(intVector);
				}
				if (vector3.magnitude < 0.2f)
				{
					linkedListNode = linkedListNode.Next;
					if (linkedListNode == null)
					{
						break;
					}
					vector2 = linkedListNode.Value.ToCenterVector2();
				}
				num += vector3.magnitude / this.MovementSpeed;
			}
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x060055B7 RID: 21943 RVA: 0x00209B08 File Offset: 0x00207D08
	// (set) Token: 0x060055B8 RID: 21944 RVA: 0x00209B10 File Offset: 0x00207D10
	public int CurrentPhase
	{
		get
		{
			return this.m_currentPhase;
		}
		set
		{
			this.m_currentPhase = value;
		}
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x060055B9 RID: 21945 RVA: 0x00209B1C File Offset: 0x00207D1C
	// (set) Token: 0x060055BA RID: 21946 RVA: 0x00209B24 File Offset: 0x00207D24
	public bool HasBeenEngaged
	{
		get
		{
			return this.m_hasBeenEngaged;
		}
		set
		{
			if (value && !this.m_hasBeenEngaged)
			{
				this.OnEngaged(false);
			}
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x060055BB RID: 21947 RVA: 0x00209B40 File Offset: 0x00207D40
	public bool IsReadyForRepath
	{
		get
		{
			return this.m_isReadyForRepath;
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x060055BC RID: 21948 RVA: 0x00209B48 File Offset: 0x00207D48
	// (set) Token: 0x060055BD RID: 21949 RVA: 0x00209B50 File Offset: 0x00207D50
	public Vector2 KnockbackVelocity
	{
		get
		{
			return this.m_knockbackVelocity;
		}
		set
		{
			this.m_knockbackVelocity = value;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x060055BE RID: 21950 RVA: 0x00209B5C File Offset: 0x00207D5C
	public override Vector3 SpriteDimensions
	{
		get
		{
			return this.m_spriteDimensions;
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x060055BF RID: 21951 RVA: 0x00209B64 File Offset: 0x00207D64
	public override Gun CurrentGun
	{
		get
		{
			return (!(base.aiShooter != null)) ? null : base.aiShooter.CurrentGun;
		}
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x060055C0 RID: 21952 RVA: 0x00209B88 File Offset: 0x00207D88
	public override bool SpriteFlipped
	{
		get
		{
			return (!(base.aiAnimator != null)) ? base.sprite.FlipX : base.aiAnimator.SpriteFlipped;
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x060055C1 RID: 21953 RVA: 0x00209BB8 File Offset: 0x00207DB8
	public override Transform GunPivot
	{
		get
		{
			return (!(base.aiShooter != null)) ? null : base.aiShooter.gunAttachPoint;
		}
	}

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x060055C2 RID: 21954 RVA: 0x00209BDC File Offset: 0x00207DDC
	// (set) Token: 0x060055C3 RID: 21955 RVA: 0x00209BE4 File Offset: 0x00207DE4
	public bool ManualKnockbackHandling { get; set; }

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x060055C4 RID: 21956 RVA: 0x00209BF0 File Offset: 0x00207DF0
	// (set) Token: 0x060055C5 RID: 21957 RVA: 0x00209BF8 File Offset: 0x00207DF8
	public bool SuppressTargetSwitch { get; set; }

	// Token: 0x060055C6 RID: 21958 RVA: 0x00209C04 File Offset: 0x00207E04
	private void OnEnable()
	{
		if (this.invisibleUntilAwaken)
		{
			if (this.State == AIActor.ActorState.Inactive)
			{
				this.ToggleRenderers(false);
			}
			if (!this.HasBeenAwoken)
			{
				base.specRigidbody.CollideWithOthers = false;
				base.IsGone = true;
				if (base.knockbackDoer)
				{
					base.knockbackDoer.SetImmobile(true, "awaken");
				}
			}
		}
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x00209C70 File Offset: 0x00207E70
	private void OnDisable()
	{
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x00209C74 File Offset: 0x00207E74
	public void ToggleRenderers(bool e)
	{
		tk2dSprite[] componentsInChildren = base.GetComponentsInChildren<tk2dSprite>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = e;
		}
		tk2dSpriteAnimator[] componentsInChildren2 = base.GetComponentsInChildren<tk2dSpriteAnimator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enabled = e;
		}
		Renderer[] componentsInChildren3 = base.GetComponentsInChildren<Renderer>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].enabled = e;
		}
		if (e)
		{
			bool? forcedOutlines = this.m_forcedOutlines;
			if (forcedOutlines != null && !this.m_forcedOutlines.Value)
			{
				for (int l = 0; l < componentsInChildren.Length; l++)
				{
					if (componentsInChildren[l].IsOutlineSprite)
					{
						componentsInChildren[l].renderer.enabled = false;
					}
				}
			}
		}
		if (base.aiShooter && e)
		{
			base.aiShooter.UpdateGunRenderers();
			base.aiShooter.UpdateHandRenderers();
		}
	}

	// Token: 0x060055C9 RID: 21961 RVA: 0x00209D80 File Offset: 0x00207F80
	private void InitializeCallbacks()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath += this.PreDeath;
			base.healthHaver.OnDeath += this.Die;
			base.healthHaver.OnDamaged += this.Damaged;
		}
		if (base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		}
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}
	}

	// Token: 0x060055CA RID: 21962 RVA: 0x00209E54 File Offset: 0x00208054
	private void DeregisterCallbacks()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.PreDeath;
			base.healthHaver.OnDeath -= this.Die;
			base.healthHaver.OnDamaged -= this.Damaged;
		}
		if (base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		}
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}
	}

	// Token: 0x060055CB RID: 21963 RVA: 0x00209F28 File Offset: 0x00208128
	protected void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (GameManager.AUDIO_ENABLED)
		{
			for (int i = 0; i < this.animationAudioEvents.Count; i++)
			{
				if (this.animationAudioEvents[i].eventTag == frame.eventInfo)
				{
					AkSoundEngine.PostEvent(this.animationAudioEvents[i].eventName, base.gameObject);
				}
			}
		}
		if (this.procedurallyOutlined && frame.eventOutline != tk2dSpriteAnimationFrame.OutlineModifier.Unspecified)
		{
			if (frame.eventOutline == tk2dSpriteAnimationFrame.OutlineModifier.TurnOn)
			{
				this.SetOutlines(true);
			}
			else if (frame.eventOutline == tk2dSpriteAnimationFrame.OutlineModifier.TurnOff)
			{
				this.SetOutlines(false);
			}
		}
		if ((this.State == AIActor.ActorState.Inactive || this.State == AIActor.ActorState.Awakening) && frame.finishedSpawning)
		{
			base.specRigidbody.CollideWithOthers = true;
			base.IsGone = false;
			if (base.knockbackDoer)
			{
				base.knockbackDoer.SetImmobile(false, "awaken");
			}
			base.healthHaver.IsVulnerable = true;
		}
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x0020A048 File Offset: 0x00208248
	public void SkipOnEngaged()
	{
		this.m_hasBeenEngaged = true;
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x0020A054 File Offset: 0x00208254
	public void DelayActions(float delay)
	{
		this.SpeculatorDelayTime += delay;
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x0020A064 File Offset: 0x00208264
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.parentRoom = room;
		this.parentRoom.RegisterEnemy(this);
		this.parentRoom.Entered += this.OnPlayerEntered;
		if (base.healthHaver.IsBoss && !GameManager.Instance.InTutorial && GameManager.Instance.BestActivePlayer.CurrentRoom != room)
		{
			if (!this.CanDropItems && this.CustomLootTable != null)
			{
				room.OverrideBossRewardTable = this.CustomLootTable;
			}
			SpeculativeRigidbody[] componentsInChildren = base.GetComponentsInChildren<SpeculativeRigidbody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CollideWithOthers = false;
			}
			base.IsGone = true;
		}
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x0020A124 File Offset: 0x00208324
	private void OnPlayerEntered(PlayerController enterer)
	{
		if (!this.HasDonePlayerEnterCheck && this.isPassable)
		{
			base.specRigidbody.Initialize();
			Vector2 unitCenter = GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter;
			bool flag = !Pathfinder.Instance.IsPassable(this.PathTile, new IntVector2?(this.Clearance), new CellTypes?(this.PathableTiles), false, null);
			if (flag)
			{
				UnityEngine.Debug.LogErrorFormat("Tried to spawn a {0} in an invalid location in room {1}.", new object[]
				{
					base.name,
					this.ParentRoom.GetRoomName()
				});
			}
			if (base.GetComponent<KeyBulletManController>())
			{
				this.TeleportSomewhere(null, true);
			}
			else if (flag || (!this.IsHarmlessEnemy && Vector2.Distance(unitCenter, base.specRigidbody.UnitCenter) < 8f))
			{
				this.TeleportSomewhere(null, false);
			}
			this.HasDonePlayerEnterCheck = true;
		}
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x0020A228 File Offset: 0x00208428
	public void TeleportSomewhere(IntVector2? overrideClearance = null, bool keepClose = false)
	{
		float sqrMinDist = 64f;
		float sqrMaxDist = 225f;
		PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
		Vector2 playerPosition = primaryPlayer.specRigidbody.UnitCenter;
		Vector2? otherPlayerPosition = null;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(primaryPlayer);
			if (otherPlayer && otherPlayer.healthHaver && otherPlayer.healthHaver.IsAlive)
			{
				otherPlayerPosition = new Vector2?(otherPlayer.specRigidbody.UnitCenter);
			}
		}
		IntVector2 clearance = ((overrideClearance == null) ? this.Clearance : overrideClearance.Value);
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			if ((playerPosition - c.ToCenterVector2()).sqrMagnitude <= sqrMinDist)
			{
				return false;
			}
			if (otherPlayerPosition != null && (otherPlayerPosition.Value - c.ToCenterVector2()).sqrMagnitude <= sqrMinDist)
			{
				return false;
			}
			if (keepClose)
			{
				bool flag = false;
				if ((playerPosition - c.ToCenterVector2()).sqrMagnitude <= sqrMaxDist)
				{
					flag = true;
				}
				if (otherPlayerPosition != null && (otherPlayerPosition.Value - c.ToCenterVector2()).sqrMagnitude <= sqrMaxDist)
				{
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
			}
			for (int i = 0; i < clearance.x; i++)
			{
				for (int j = 0; j < clearance.y; j++)
				{
					if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(new IntVector2(c.x + i, c.y + j)))
					{
						return false;
					}
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
					if (!GameManager.Instance.Dungeon.data[c.x + i, c.y + j].isGridConnected)
					{
						return false;
					}
				}
			}
			return true;
		};
		IntVector2? randomAvailableCell = this.ParentRoom.GetRandomAvailableCell(new IntVector2?(clearance), new CellTypes?(this.PathableTiles), false, cellValidator);
		if (randomAvailableCell != null)
		{
			base.specRigidbody.Initialize();
			Vector2 vector = base.specRigidbody.UnitCenter - base.transform.position.XY();
			Vector2 vector2 = Pathfinder.GetClearanceOffset(randomAvailableCell.Value, this.Clearance) - vector;
			vector2 = BraveUtility.QuantizeVector(vector2);
			base.transform.position = vector2;
			base.specRigidbody.Reinitialize();
		}
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x0020A3B4 File Offset: 0x002085B4
	public void HandleReinforcementFallIntoRoom(float delay = 0f)
	{
		this.HasDonePlayerEnterCheck = true;
		this.IsInReinforcementLayer = true;
		base.StartCoroutine(this.HandleReinforcementFall_CR(delay));
	}

	// Token: 0x060055D2 RID: 21970 RVA: 0x0020A3D4 File Offset: 0x002085D4
	protected void DisableOutlinesPostStart()
	{
		this.OnPostStartInitialization = (Action)Delegate.Remove(this.OnPostStartInitialization, new Action(this.DisableOutlinesPostStart));
		if (this.procedurallyOutlined)
		{
			this.SetOutlines(false);
		}
		if (this.HasShadow)
		{
			base.ToggleShadowVisiblity(false);
		}
	}

	// Token: 0x060055D3 RID: 21971 RVA: 0x0020A428 File Offset: 0x00208628
	private IEnumerator HandleReinforcementFall_CR(float delay)
	{
		if (this.m_customReinforceDoer)
		{
			this.m_customReinforceDoer.StartIntro();
			while (!this.m_customReinforceDoer.IsFinished)
			{
				yield return null;
			}
			this.m_customReinforceDoer.OnCleanup();
		}
		else
		{
			if (this.reinforceType == AIActor.ReinforceType.Instant)
			{
				this.ToggleRenderers(true);
				this.OnEngaged(true);
				yield break;
			}
			this.ToggleRenderers(false);
			this.invisibleUntilAwaken = true;
			base.specRigidbody.CollideWithOthers = false;
			base.IsGone = true;
			if (base.knockbackDoer)
			{
				base.knockbackDoer.SetImmobile(true, "awaken");
			}
			if (base.behaviorSpeculator)
			{
				base.behaviorSpeculator.enabled = false;
			}
			base.healthHaver.IsVulnerable = false;
			if (base.aiShooter)
			{
				base.aiShooter.ToggleGunAndHandRenderers(false, "Reinforce");
			}
			this.OnPostStartInitialization = (Action)Delegate.Combine(this.OnPostStartInitialization, new Action(this.DisableOutlinesPostStart));
			float elapsed = 0f;
			while (elapsed < delay)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
			float duration = 1.5f;
			if (this.reinforceType == AIActor.ReinforceType.FullVfx)
			{
				tk2dBaseSprite component = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_SpawnEnemy_Reticle"))).GetComponent<tk2dBaseSprite>();
				Vector3 vector = base.transform.position + base.sprite.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.LowerCenter).ToVector3ZUp(0f);
				AkSoundEngine.PostEvent(string.IsNullOrEmpty(this.OverrideSpawnReticleAudio) ? "Play_ENM_spawn_reticle_01" : this.OverrideSpawnReticleAudio, base.gameObject);
				component.transform.position = vector;
				component.HeightOffGround = -1.5f;
				component.UpdateZDepth();
				tk2dSpriteAnimator component2 = component.GetComponent<tk2dSpriteAnimator>();
				if (component2)
				{
					duration = (float)component2.DefaultClip.frames.Length / component2.DefaultClip.fps;
				}
			}
			elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += this.LocalDeltaTime;
				this.ToggleRenderers(false);
				if (base.aiShooter)
				{
					base.aiShooter.ToggleGunAndHandRenderers(false, "Reinforce");
				}
				yield return null;
			}
			if (this.reinforceType == AIActor.ReinforceType.FullVfx)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Bullet_Spawn"));
				AkSoundEngine.PostEvent(string.IsNullOrEmpty(this.OverrideSpawnAppearAudio) ? "Play_ENM_spawn_appear_01" : this.OverrideSpawnAppearAudio, base.gameObject);
				tk2dBaseSprite component3 = gameObject.GetComponent<tk2dBaseSprite>();
				gameObject.transform.localScale = new Vector3(Mathf.Sign(UnityEngine.Random.value - 0.5f), Mathf.Sign(UnityEngine.Random.value - 0.5f), 1f);
				component3.transform.position = base.specRigidbody.UnitCenter;
				component3.HeightOffGround = 0.5f;
				component3.UpdateZDepth();
			}
			duration = 0.125f;
			while (elapsed < duration)
			{
				elapsed += this.LocalDeltaTime;
				this.ToggleRenderers(false);
				if (base.aiShooter)
				{
					base.aiShooter.ToggleGunAndHandRenderers(false, "Reinforce");
				}
				yield return null;
			}
			if (!this.invisibleUntilAwaken)
			{
				base.specRigidbody.CollideWithOthers = true;
				base.IsGone = false;
				if (base.knockbackDoer)
				{
					base.knockbackDoer.SetImmobile(false, "awaken");
				}
			}
			if (base.behaviorSpeculator)
			{
				base.behaviorSpeculator.enabled = true;
			}
			base.healthHaver.IsVulnerable = true;
			this.ToggleRenderers(true);
			if (this.procedurallyOutlined)
			{
				this.SetOutlines(true);
			}
			bool isPlayingAwaken = false;
			if (base.aiAnimator)
			{
				this.m_awakenAnimation = base.aiAnimator.PlayDefaultSpawnState(out isPlayingAwaken);
			}
			this.State = ((!string.IsNullOrEmpty(this.m_awakenAnimation)) ? AIActor.ActorState.Awakening : AIActor.ActorState.Normal);
			if (base.aiShooter)
			{
				bool flag = isPlayingAwaken || this.State == AIActor.ActorState.Normal;
				base.aiShooter.ToggleGunAndHandRenderers(flag, "Reinforce");
			}
			this.OnEngaged(true);
		}
		if (base.behaviorSpeculator)
		{
			List<TargetBehaviorBase> targetBehaviors = base.behaviorSpeculator.TargetBehaviors;
			for (int i = 0; i < targetBehaviors.Count; i++)
			{
				TargetPlayerBehavior targetPlayerBehavior = targetBehaviors[i] as TargetPlayerBehavior;
				if (targetPlayerBehavior != null)
				{
					targetPlayerBehavior.LineOfSight = false;
					targetPlayerBehavior.Radius = 1000f;
					targetPlayerBehavior.ObjectPermanence = true;
				}
			}
		}
		yield break;
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x0020A44C File Offset: 0x0020864C
	private void OnEngaged(bool isReinforcement = false)
	{
		if (this.m_hasBeenEngaged)
		{
			return;
		}
		if (this.SetsFlagOnActivation)
		{
			GameStatsManager.Instance.SetFlag(this.FlagToSetOnActivation, true);
		}
		if (!isReinforcement && this.m_customEngageDoer && !this.m_customEngageDoer.IsFinished)
		{
			base.StartCoroutine(this.DoCustomEngage());
			return;
		}
		if (this.invisibleUntilAwaken)
		{
			this.ToggleRenderers(true);
		}
		if (base.aiAnimator != null && this.m_awakenAnimation == null)
		{
			if (this.AwakenAnimType == AIActor.AwakenAnimationType.Spawn)
			{
				this.m_awakenAnimation = base.aiAnimator.PlayDefaultAwakenedState();
			}
			else
			{
				this.m_awakenAnimation = base.aiAnimator.PlayDefaultSpawnState();
			}
		}
		this.State = ((!string.IsNullOrEmpty(this.m_awakenAnimation)) ? AIActor.ActorState.Awakening : AIActor.ActorState.Normal);
		if (base.aiShooter && this.invisibleUntilAwaken && this.State == AIActor.ActorState.Awakening)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "Awaken");
		}
		if (base.healthHaver.IsBoss && base.healthHaver.HasHealthBar)
		{
			GameUIBossHealthController gameUIBossHealthController = ((!base.healthHaver.UsesVerticalBossBar) ? (base.healthHaver.UsesSecondaryBossBar ? GameUIRoot.Instance.bossController2 : GameUIRoot.Instance.bossController) : GameUIRoot.Instance.bossControllerSide);
			string text = this.GetActorName();
			if (!string.IsNullOrEmpty(base.healthHaver.overrideBossName))
			{
				text = StringTableManager.GetEnemiesString(base.healthHaver.overrideBossName, -1);
			}
			gameUIBossHealthController.RegisterBossHealthHaver(base.healthHaver, text);
		}
		if (this.OnEngagedVFX != null)
		{
			Vector2 relativePositionFromAnchor = base.sprite.GetRelativePositionFromAnchor(this.OnEngagedVFXAnchor);
			GameObject gameObject = SpawnManager.SpawnVFX(this.OnEngagedVFX, false);
			Transform transform = gameObject.transform;
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			transform.parent = base.transform;
			transform.localPosition = relativePositionFromAnchor.ToVector3ZUp(-0.1f);
			component.automaticallyManagesDepth = false;
			base.sprite.AttachRenderer(component);
		}
		int num;
		if (this.IdentifierForEffects == AIActor.EnemyTypeIdentifier.SNIPER_TYPE && PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.SNIPER_WOLF, out num))
		{
			base.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
		}
		this.m_hasBeenEngaged = true;
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x0020A6C8 File Offset: 0x002088C8
	private IEnumerator DoCustomEngage()
	{
		this.m_customEngageDoer.StartIntro();
		while (!this.m_customEngageDoer.IsFinished)
		{
			yield return null;
		}
		this.m_customEngageDoer.OnCleanup();
		yield break;
	}

	// Token: 0x060055D6 RID: 21974 RVA: 0x0020A6E4 File Offset: 0x002088E4
	private void HandleLootPinata(int additionalMetas = 0)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			return;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		Vector3 vector = ((!base.specRigidbody) ? base.transform.position : base.specRigidbody.UnitCenter.ToVector3ZUp(base.transform.position.z));
		if (this.SpawnLootAtRewardChestPos && this.parentRoom != null)
		{
			IntVector2 rewardChestSpawnPosition = this.parentRoom.area.runtimePrototypeData.rewardChestSpawnPosition;
			if (rewardChestSpawnPosition.x >= 0 && rewardChestSpawnPosition.y >= 0)
			{
				vector = (this.parentRoom.area.UnitBottomLeft + rewardChestSpawnPosition.ToCenterVector2()).ToVector3ZisY(0f);
			}
		}
		if (base.healthHaver.IsBoss && !this.parentRoom.HasOtherBoss(this))
		{
			int num;
			if (base.healthHaver.IsSubboss)
			{
				num = 1;
			}
			else
			{
				num = UnityEngine.Random.Range(GameManager.Instance.RewardManager.CurrentRewardData.MinMetaCurrencyFromBoss, GameManager.Instance.RewardManager.CurrentRewardData.MaxMetaCurrencyFromBoss + 1);
				GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
				if (tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON || tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
				{
					num = 0;
				}
			}
			if (GameManager.Instance.InTutorial)
			{
				num = 0;
			}
			num += additionalMetas;
			if (GameManager.Instance.BestActivePlayer.CharacterUsesRandomGuns && (!ChallengeManager.CHALLENGE_MODE_ACTIVE || ChallengeManager.Instance.ChallengeMode != ChallengeModeType.ChallengeMegaMode))
			{
				num = 0;
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
			{
				num = UnityEngine.Random.Range(1, 3);
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
			{
				num = 0;
			}
			int num2 = this.AssignedCurrencyToDrop;
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH || GameManager.Instance.InTutorial)
			{
				num2 = 0;
			}
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				num2 = 0;
			}
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].IsDarkSoulsHollow && !GameManager.Instance.AllPlayers[i].IsGhost)
				{
					num2 = 0;
					num = 0;
				}
			}
			if (num > 0)
			{
				if (this.ParentRoom != null && !this.ParentRoom.PlayerHasTakenDamageInThisRoom)
				{
					num *= 2;
				}
				Vector2 vector2 = Vector2.down * 1.5f;
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON && GameManager.Instance.BestActivePlayer && base.specRigidbody)
				{
					vector2 = BraveUtility.GetMajorAxis(GameManager.Instance.BestActivePlayer.CenterPosition - vector.XY()).normalized * 1.5f;
				}
				float num3 = 0.05f;
				float num4 = 4f;
				if (base.specRigidbody)
				{
					num3 = Mathf.Max(0.05f, base.specRigidbody.UnitCenter.y - base.specRigidbody.UnitBottom);
					UnityEngine.Debug.Log("assigning SZH: " + num3);
				}
				LootEngine.SpawnCurrency(vector.XY(), num, true, new Vector2?(vector2), new float?(45f), num4, num3);
			}
			if (PassiveItem.IsFlagSetAtAll(typeof(BankBagItem)))
			{
				num2 *= 2;
			}
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController)
				{
					float statValue = playerController.stats.GetStatValue(PlayerStats.StatType.MoneyMultiplierFromEnemies);
					if (statValue != 1f && statValue > 0f)
					{
						num2 = Mathf.RoundToInt((float)num2 * statValue);
					}
				}
			}
			if (num2 > 0)
			{
				list.AddRange(GameManager.Instance.Dungeon.sharedSettingsPrefab.GetCurrencyToDrop(num2, false, false));
			}
		}
		else if ((this.CanDropCurrency || this.AdditionalSingleCoinDropChance > 0f) && !this.HasDamagedPlayer && !base.healthHaver.IsBoss)
		{
			GenericCurrencyDropSettings currencyDropSettings = GameManager.Instance.Dungeon.sharedSettingsPrefab.currencyDropSettings;
			int num5 = ((!this.CanDropCurrency) ? 0 : this.AssignedCurrencyToDrop);
			if (this.AdditionalSingleCoinDropChance > 0f && UnityEngine.Random.value < this.AdditionalSingleCoinDropChance)
			{
				num5++;
			}
			if (this.IsBlackPhantom)
			{
				num5 += currencyDropSettings.blackPhantomCoinDropChances.SelectByWeight();
			}
			for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			{
				if (GameManager.Instance.AllPlayers[k].IsDarkSoulsHollow && !GameManager.Instance.AllPlayers[k].IsGhost)
				{
					num5 = 0;
				}
			}
			if (PassiveItem.IsFlagSetAtAll(typeof(BankBagItem)))
			{
				num5 *= 2;
			}
			for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
			{
				PlayerController playerController2 = GameManager.Instance.AllPlayers[l];
				if (playerController2)
				{
					float statValue2 = playerController2.stats.GetStatValue(PlayerStats.StatType.MoneyMultiplierFromEnemies);
					if (statValue2 != 1f && statValue2 > 0f)
					{
						num5 = Mathf.RoundToInt((float)num5 * statValue2);
					}
				}
			}
			if (num5 > 0)
			{
				list.AddRange(GameManager.Instance.Dungeon.sharedSettingsPrefab.GetCurrencyToDrop(num5, false, false));
			}
		}
		if (this.AdditionalSimpleItemDrops.Count > 0)
		{
			for (int m = 0; m < this.AdditionalSimpleItemDrops.Count; m++)
			{
				list.Add(this.AdditionalSimpleItemDrops[m].gameObject);
			}
		}
		float num6 = 360f / (float)list.Count;
		for (int n = 0; n < list.Count; n++)
		{
			Vector3 vector3 = Quaternion.Euler(0f, 0f, num6 * (float)n) * Vector3.up;
			vector3 *= 2f;
			float num7 = 0f;
			tk2dBaseSprite component = list[n].GetComponent<tk2dBaseSprite>();
			if (component != null)
			{
				num7 = -1f * component.GetBounds().center.x;
			}
			bool flag = list[n].GetComponent<RobotArmBalloonsItem>() || list[n].GetComponent<RobotArmItem>();
			if (flag)
			{
				LootEngine.SpawnItem(list[n], vector + new Vector3(num7, 0f, 0f), Vector2.zero, 0.5f, true, true, false);
			}
			else
			{
				GameObject gameObject = SpawnManager.SpawnDebris(list[n], vector + new Vector3(num7, 0f, 0f), Quaternion.identity);
				DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
				orAddComponent.shouldUseSRBMotion = true;
				orAddComponent.angularVelocity = 0f;
				orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
				orAddComponent.Trigger(vector3.WithZ(4f), 0.05f, 1f);
				orAddComponent.canRotate = false;
			}
		}
		if (this.CustomChestTable != null && UnityEngine.Random.value < this.ChanceToDropCustomChest)
		{
			GameObject gameObject2 = this.CustomChestTable.SelectByWeight(false);
			if (gameObject2)
			{
				IntVector2? randomAvailableCell = this.parentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 4), new CellTypes?(CellTypes.FLOOR), false, null);
				IntVector2? intVector = ((randomAvailableCell == null) ? null : new IntVector2?(randomAvailableCell.GetValueOrDefault() + IntVector2.One));
				if (intVector != null)
				{
					Chest chest = Chest.Spawn(gameObject2.GetComponent<Chest>(), intVector.Value);
					if (chest)
					{
						chest.RegisterChestOnMinimap(this.parentRoom);
					}
				}
			}
		}
		GameObject gameObject3 = null;
		if (this.CanDropItems && this.CustomLootTable != null)
		{
			int num8 = UnityEngine.Random.Range(this.CustomLootTableMinDrops, this.CustomLootTableMaxDrops);
			if (num8 == 1)
			{
				gameObject3 = this.CustomLootTable.SelectByWeight(false);
				if (gameObject3 != null)
				{
					LootEngine.SpawnItem(gameObject3, vector, Vector2.up, 1f, true, true, false);
				}
			}
			else
			{
				List<GameObject> list2 = new List<GameObject>();
				for (int num9 = 0; num9 < num8; num9++)
				{
					for (int num10 = 0; num10 < 3; num10++)
					{
						gameObject3 = this.CustomLootTable.SelectByWeight(false);
						if (this.CanDropDuplicateItems || gameObject3 == null || !list2.Contains(gameObject3))
						{
							break;
						}
					}
					if (gameObject3 != null && (this.CanDropDuplicateItems || !list2.Contains(gameObject3)))
					{
						list2.Add(gameObject3);
					}
				}
				LootEngine.SpewLoot(list2, vector);
			}
		}
		bool flag2 = this.AdditionalSafeItemDrops.Count > 0 && GameStatsManager.Instance.IsRainbowRun && this.IsMimicEnemy;
		if (flag2)
		{
			Vector2 vector4 = vector.XY();
			RoomHandler absoluteRoom = vector4.GetAbsoluteRoom();
			LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteMimic, vector4, absoluteRoom, true);
		}
		else
		{
			int num11 = 0;
			while (num11 < this.AdditionalSafeItemDrops.Count)
			{
				RoomHandler absoluteRoomFromPosition = this.parentRoom;
				if (!this.IsMimicEnemy)
				{
					goto IL_AE0;
				}
				IntVector2 intVector2 = vector.XY().ToIntVector2(VectorConversions.Floor);
				if (GameManager.Instance.Dungeon.data.GetRoomFromPosition(intVector2) != null)
				{
					absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector2);
					goto IL_AE0;
				}
				LootEngine.SpawnItem(this.AdditionalSafeItemDrops[num11].gameObject, vector, Vector2.up, 1f, true, true, false);
				IL_B26:
				num11++;
				continue;
				IL_AE0:
				IntVector2 bestRewardLocation = absoluteRoomFromPosition.GetBestRewardLocation(IntVector2.One, vector.XY(), true);
				LootEngine.SpawnItem(this.AdditionalSafeItemDrops[num11].gameObject, bestRewardLocation.ToCenterVector2(), Vector2.zero, 0f, true, false, false);
				goto IL_B26;
			}
		}
	}

	// Token: 0x060055D7 RID: 21975 RVA: 0x0020B230 File Offset: 0x00209430
	public void EraseFromExistenceWithRewards(bool suppressDeathSounds = false)
	{
		this.HandleRewards();
		this.EraseFromExistence(suppressDeathSounds);
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x0020B240 File Offset: 0x00209440
	public void EraseFromExistence(bool suppressDeathSounds = false)
	{
		base.StealthDeath = true;
		if (base.behaviorSpeculator)
		{
			base.behaviorSpeculator.InterruptAndDisable();
		}
		if (suppressDeathSounds)
		{
			base.healthHaver.SuppressDeathSounds = true;
		}
		base.healthHaver.ApplyDamage(10000000f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060055D9 RID: 21977 RVA: 0x0020B2AC File Offset: 0x002094AC
	private void PreDeath(Vector2 finalDamageDirection)
	{
		if (base.aiAnimator)
		{
			base.aiAnimator.FpsScale = 1f;
		}
		if (this.shadowDeathType != AIActor.ShadowDeathType.None)
		{
			float num = 0f;
			if (base.aiAnimator && base.aiAnimator.HasDirectionalAnimation("death"))
			{
				num = base.aiAnimator.GetDirectionalAnimationLength("death");
			}
			else
			{
				tk2dSpriteAnimationClip deathClip = base.healthHaver.GetDeathClip(finalDamageDirection.ToAngle());
				if (deathClip != null)
				{
					num = (float)deathClip.frames.Length / deathClip.fps;
				}
			}
			if (num > 0f)
			{
				if (this.shadowDeathType == AIActor.ShadowDeathType.Fade)
				{
					base.StartCoroutine(this.FadeShadowCR(num));
				}
				else
				{
					base.StartCoroutine(this.ScaleShadowCR(num));
				}
			}
		}
		if (base.healthHaver.IsBoss)
		{
			if (this.HasBeenGlittered)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.KILL_BOSS_WITH_GLITTER, 0);
			}
			if (this.IsBlackPhantom)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_A_JAMMED_BOSS, 0);
			}
		}
		else if (this.IsBlackPhantom && !this.SuppressBlackPhantomCorpseBurn)
		{
			base.StartCoroutine(this.BurnBlackPhantomCorpse());
		}
		if (this.IsNormalEnemy)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController && playerController.healthHaver.IsAlive && playerController.IsInMinecart)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.ENEMIES_KILLED_WHILE_IN_CARTS, 1f);
					break;
				}
			}
		}
	}

	// Token: 0x060055DA RID: 21978 RVA: 0x0020B46C File Offset: 0x0020966C
	private IEnumerator FadeShadowCR(float scaleTime)
	{
		if (!this.ShadowObject || !base.aiAnimator)
		{
			yield break;
		}
		float timer = 0f;
		tk2dSprite shadowSprite = this.ShadowObject.GetComponent<tk2dSprite>();
		while (timer < scaleTime)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			shadowSprite.color = shadowSprite.color.WithAlpha(1f - base.aiAnimator.CurrentClipProgress);
		}
		yield break;
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x0020B490 File Offset: 0x00209690
	private IEnumerator ScaleShadowCR(float scaleTime)
	{
		if (!this.ShadowObject)
		{
			yield break;
		}
		float timer = 0f;
		while (timer < scaleTime)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			this.ShadowObject.transform.localScale = Vector3.one * Mathf.Clamp01(1f - timer / scaleTime);
		}
		yield break;
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x0020B4B4 File Offset: 0x002096B4
	public void Transmogrify(AIActor EnemyPrefab, GameObject EffectVFX)
	{
		if (this.IsTransmogrified && this.ActorName == EnemyPrefab.ActorName)
		{
			return;
		}
		if (this.IsMimicEnemy || !base.healthHaver || base.healthHaver.IsBoss || !base.healthHaver.IsVulnerable)
		{
			return;
		}
		if (this.parentRoom == null)
		{
			return;
		}
		Vector2 centerPosition = base.CenterPosition;
		if (EffectVFX != null)
		{
			SpawnManager.SpawnVFX(EffectVFX, centerPosition, Quaternion.identity);
		}
		AIActor aiactor = AIActor.Spawn(EnemyPrefab, centerPosition.ToIntVector2(VectorConversions.Floor), this.parentRoom, true, AIActor.AwakenAnimationType.Default, true);
		if (aiactor)
		{
			aiactor.IsTransmogrified = true;
		}
		if (EnemyPrefab.name == "Chicken")
		{
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
			AkSoundEngine.PostEvent("Play_PET_chicken_cluck_01", base.gameObject);
		}
		else if (EnemyPrefab.name == "Snake")
		{
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
		}
		else
		{
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
		}
		this.HandleRewards();
		this.EraseFromExistence(false);
	}

	// Token: 0x060055DD RID: 21981 RVA: 0x0020B600 File Offset: 0x00209800
	private void Die(Vector2 finalDamageDirection)
	{
		this.ForceDeath(finalDamageDirection, true);
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x0020B60C File Offset: 0x0020980C
	private IEnumerator BurnBlackPhantomCorpse()
	{
		Material targetMaterial = base.sprite.renderer.material;
		float ela = 0f;
		float dura = 0.5f;
		while (ela < dura)
		{
			ela += BraveTime.DeltaTime;
			float t = ela / dura;
			targetMaterial.SetFloat("_BurnAmount", t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060055DF RID: 21983 RVA: 0x0020B628 File Offset: 0x00209828
	private void HandleRewards()
	{
		if (this.m_hasGivenRewards || this.IsTransmogrified)
		{
			return;
		}
		GameStatsManager.Instance.huntProgress.ProcessKill(this);
		int num = 0;
		if (this.SetsFlagOnDeath)
		{
			if (this.FlagToSetOnDeath == GungeonFlags.TUTORIAL_COMPLETED && !GameStatsManager.Instance.GetFlag(GungeonFlags.TUTORIAL_RECEIVED_META_CURRENCY))
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.TUTORIAL_RECEIVED_META_CURRENCY, true);
				num = 10;
			}
			GameStatsManager.Instance.SetFlag(this.FlagToSetOnDeath, true);
			if (this.FlagToSetOnDeath == GungeonFlags.BOSSKILLED_DRAGUN || this.FlagToSetOnDeath == GungeonFlags.BOSSKILLED_HIGHDRAGUN)
			{
				if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Robot)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_DRAGUN_WITH_ROBOT, true);
				}
				if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Bullet)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_DRAGUN_WITH_BULLET, true);
				}
				if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_DRAGUN_PARADOX, true);
				}
				if (GameManager.Instance.BestActivePlayer.CharacterUsesRandomGuns)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.SORCERESS_BLESSED_MODE_COMPLETE, true);
				}
				if (GameManager.IsTurboMode)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_DRAGUN_TURBO_MODE, true);
					GameStatsManager.Instance.SetFlag(GungeonFlags.TONIC_TURBO_MODE_COMPLETE, true);
				}
			}
		}
		if (this.SetsCharacterSpecificFlagOnDeath)
		{
			GameStatsManager.Instance.SetCharacterSpecificFlag(this.CharacterSpecificFlagToSetOnDeath, true);
		}
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.ENEMIES_KILLED, 1f);
		this.HandleLootPinata(num);
		if (this.OnHandleRewards != null)
		{
			this.OnHandleRewards();
		}
		this.m_hasGivenRewards = true;
	}

	// Token: 0x060055E0 RID: 21984 RVA: 0x0020B7E0 File Offset: 0x002099E0
	public void ForceDeath(Vector2 finalDamageDirection, bool allowCorpse = true)
	{
		EncounterTrackable component = base.GetComponent<EncounterTrackable>();
		if (component != null)
		{
			GameStatsManager.Instance.HandleEncounteredObject(component);
		}
		SpawnEnemyOnDeath component2 = base.GetComponent<SpawnEnemyOnDeath>();
		if (component2)
		{
			component2.ManuallyTrigger(finalDamageDirection);
		}
		this.HandleRewards();
		if (!base.StealthDeath)
		{
			this.OnCorpseVFX.SpawnAtPosition(base.specRigidbody.GetUnitCenter(ColliderType.HitBox), 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), null, false, null, null, false);
		}
		if (this.CorpseObject != null && !this.m_isFalling && allowCorpse && !base.StealthDeath)
		{
			if (this.IsBlackPhantom)
			{
				if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && base.sprite)
				{
					Vector3 vector = base.sprite.WorldBottomLeft.ToVector3ZisY(0f);
					Vector3 vector2 = base.sprite.WorldTopRight.ToVector3ZisY(0f);
					Vector3 vector3 = vector2 - vector;
					vector += vector3 * 0.15f;
					vector2 -= vector3 * 0.15f;
					float num = (vector2.y - vector.y) * (vector2.x - vector.x);
					int num2 = Mathf.CeilToInt(40f * num);
					int num3 = num2;
					Vector3 vector4 = vector;
					Vector3 vector5 = vector2;
					Vector3 vector6 = Vector3.up / 2f;
					float num4 = 120f;
					float num5 = 0.2f;
					float? num6 = new float?(UnityEngine.Random.Range(1f, 1.65f));
					GlobalSparksDoer.DoRandomParticleBurst(num3, vector4, vector5, vector6, num4, num5, null, num6, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
				}
				base.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_BlackPhantomDeath") as GameObject, Vector3.zero, false, false, false);
			}
			else
			{
				GameObject gameObject = SpawnManager.SpawnDebris(this.CorpseObject, base.transform.position, Quaternion.identity);
				DebrisObject component3 = gameObject.GetComponent<DebrisObject>();
				if (component3)
				{
					if (PassiveItem.IsFlagSetAtAll(typeof(CorpseExplodeActiveItem)))
					{
						component3.Priority = EphemeralObject.EphemeralPriority.Critical;
					}
					component3.IsCorpse = true;
				}
				StaticReferenceManager.AllCorpses.Add(gameObject);
				tk2dSprite component4 = gameObject.GetComponent<tk2dSprite>();
				CorpseSpawnController component5 = gameObject.GetComponent<CorpseSpawnController>();
				if (component5)
				{
					component5.Init(this);
				}
				bool flag = true;
				if (component4 != null && component5 == null)
				{
					Material sharedMaterial = base.sprite.renderer.sharedMaterial;
					component4.SetSprite(base.sprite.Collection, base.sprite.spriteId);
					component4.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_COMPLEX;
					if (this.CorpseShadow && !this.m_isPaletteSwapped)
					{
						Renderer renderer = component4.renderer;
						if (sharedMaterial.HasProperty("_OverrideColor") && sharedMaterial.GetColor("_OverrideColor").a > 0f)
						{
							renderer.material = sharedMaterial;
							Color color = base.CurrentOverrideColor;
							for (int i = 0; i < this.m_activeEffects.Count; i++)
							{
								if (this.m_activeEffects[i].AppliesDeathTint)
								{
									color = this.m_activeEffects[i].DeathTintColor;
									renderer.material.SetFloat("_ValueMaximum", 0.6f);
									renderer.material.SetFloat("_ValueMinimum", 0.2f);
									if (renderer.material.shader.name.Contains("PixelShadow"))
									{
										renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
									}
								}
							}
							renderer.material.SetColor("_OverrideColor", color);
							renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_OFF");
							renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
							renderer.material.DisableKeyword("EMISSIVE_ON");
							renderer.material.EnableKeyword("EMISSIVE_OFF");
							flag = false;
						}
						else
						{
							renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutoutFastPixelShadow");
						}
					}
					else if (this.CorpseShadow && this.m_isPaletteSwapped)
					{
						Renderer renderer2 = component4.renderer;
						if (sharedMaterial.HasProperty("_OverrideColor") && sharedMaterial.GetColor("_OverrideColor").a > 0f)
						{
							renderer2.material = sharedMaterial;
							Color color2 = base.CurrentOverrideColor;
							for (int j = 0; j < this.m_activeEffects.Count; j++)
							{
								if (this.m_activeEffects[j].AppliesDeathTint)
								{
									color2 = this.m_activeEffects[j].DeathTintColor;
									renderer2.material.SetFloat("_ValueMaximum", 0.6f);
									renderer2.material.SetFloat("_ValueMinimum", 0.2f);
								}
							}
							renderer2.material.SetColor("_OverrideColor", color2);
							renderer2.material.DisableKeyword("BRIGHTNESS_CLAMP_OFF");
							renderer2.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
							renderer2.material.DisableKeyword("EMISSIVE_ON");
							renderer2.material.EnableKeyword("EMISSIVE_OFF");
							flag = false;
						}
						else
						{
							renderer2.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutoutFastPixelShadowPalette");
							renderer2.material.SetTexture("_MainTex", sharedMaterial.GetTexture("_MainTex"));
							renderer2.material.SetTexture("_PaletteTex", sharedMaterial.GetTexture("_PaletteTex"));
						}
					}
					else if (this.m_isPaletteSwapped)
					{
						component4.renderer.material = sharedMaterial;
					}
					if (this.TransferShadowToCorpse && this.ShadowObject)
					{
						this.ShadowObject.transform.parent = gameObject.transform;
					}
					component4.IsPerpendicular = false;
					component4.HeightOffGround = -1f;
					component4.UpdateZDepth();
				}
				if (component3 != null)
				{
					if (finalDamageDirection != Vector2.zero)
					{
						finalDamageDirection.Normalize();
					}
					component3.Trigger(finalDamageDirection, 0.1f, 1f);
					if (flag)
					{
						component3.FadeToOverrideColor(new Color(0f, 0f, 0f, 0.6f), 0.25f, 0f);
					}
					component3.AssignFinalWorldDepth(-1.25f);
				}
			}
		}
		if (this.IsMimicEnemy)
		{
			for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			{
				if (GameManager.Instance.AllPlayers[k] && GameManager.Instance.AllPlayers[k].OnChestBroken != null)
				{
					GameManager.Instance.AllPlayers[k].OnChestBroken(GameManager.Instance.AllPlayers[k], null);
				}
			}
		}
		if (base.healthHaver.IsBoss && !base.healthHaver.IsSubboss && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
		{
			bool flag2 = false;
			List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			for (int l = 0; l < activeEnemies.Count; l++)
			{
				HealthHaver healthHaver = activeEnemies[l].healthHaver;
				if (healthHaver && healthHaver.IsBoss && healthHaver.IsAlive && activeEnemies[l] != base.aiActor)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
				{
					InfinilichDeathController component6 = base.GetComponent<InfinilichDeathController>();
					if (component6)
					{
						GameManager.Instance.Dungeon.FloorCleared();
					}
				}
				else
				{
					GameManager.Instance.Dungeon.FloorCleared();
				}
			}
		}
		if (this.parentRoom != null)
		{
			this.parentRoom.DeregisterEnemy(this, false);
		}
		else
		{
			UnityEngine.Debug.LogError("An enemy who does not have a parent room is dying... this could be a problem.");
		}
		Pathfinder.Instance.RemoveActorPath(this.m_upcomingPathTiles);
	}

	// Token: 0x060055E1 RID: 21985 RVA: 0x0020C08C File Offset: 0x0020A28C
	private void Damaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (!this.HasBeenEngaged)
		{
			this.HasBeenEngaged = true;
		}
		if (damageCategory != DamageCategory.DamageOverTime && base.aiAnimator)
		{
			base.aiAnimator.PlayHitState(damageDirection);
		}
	}

	// Token: 0x060055E2 RID: 21986 RVA: 0x0020C0C8 File Offset: 0x0020A2C8
	public void StrafeTarget(float targetDistance)
	{
	}

	// Token: 0x060055E3 RID: 21987 RVA: 0x0020C0CC File Offset: 0x0020A2CC
	public void JumpToPoint(Vector2 targetPoint, float speedMultiplier, float jumpHeight)
	{
		float num = Vector2.Distance(base.transform.position.XY(), targetPoint);
		float num2 = num / (this.MovementSpeed * speedMultiplier);
		base.StartCoroutine(this.HandleJumpToPoint(targetPoint, num2, jumpHeight));
	}

	// Token: 0x060055E4 RID: 21988 RVA: 0x0020C10C File Offset: 0x0020A30C
	private IEnumerator HandleJumpToPoint(Vector2 flattenedEndPosition, float jumpTime, float jumpHeight)
	{
		this.m_isReadyForRepath = false;
		float elapsed = 0f;
		while (elapsed < jumpTime)
		{
			this.m_currentPath = null;
			yield return null;
		}
		this.m_isReadyForRepath = true;
		yield break;
	}

	// Token: 0x060055E5 RID: 21989 RVA: 0x0020C130 File Offset: 0x0020A330
	public void SetAIMovementContribution(Vector2 vel)
	{
		this.m_currentPath = null;
	}

	// Token: 0x060055E6 RID: 21990 RVA: 0x0020C13C File Offset: 0x0020A33C
	private IEnumerator DashInDirection(Vector3 direction, float duration, float speedMultiplier)
	{
		this.m_isReadyForRepath = false;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += this.LocalDeltaTime;
			this.m_currentPath = null;
			yield return null;
		}
		this.m_isReadyForRepath = true;
		yield break;
	}

	// Token: 0x060055E7 RID: 21991 RVA: 0x0020C160 File Offset: 0x0020A360
	private IEnumerator DashByDistance(Vector3 direction, float distance, float speedMultiplier)
	{
		this.m_isReadyForRepath = false;
		float velocityMagnitude = (direction.XY().normalized * this.MovementSpeed * speedMultiplier).magnitude;
		float elapsedDist = 0f;
		while (elapsedDist < distance)
		{
			elapsedDist += velocityMagnitude * this.LocalDeltaTime;
			this.m_currentPath = null;
			yield return null;
		}
		this.m_isReadyForRepath = true;
		yield break;
	}

	// Token: 0x060055E8 RID: 21992 RVA: 0x0020C190 File Offset: 0x0020A390
	public void SimpleMoveToPosition(Vector3 targetPosition)
	{
		IntVector2 intVector = targetPosition.IntXY(VectorConversions.Round);
		Path path = new Path();
		path.Positions = new LinkedList<IntVector2>();
		path.Positions.AddFirst(intVector);
		this.m_currentPath = path;
	}

	// Token: 0x060055E9 RID: 21993 RVA: 0x0020C1CC File Offset: 0x0020A3CC
	private Vector2 CalculateTargetStrafeVelocity(Vector3 targetPosition, int direction, float targetDistance)
	{
		Vector2 vector = targetPosition - base.specRigidbody.UnitCenter;
		float magnitude = vector.magnitude;
		float num = 90f;
		if (magnitude > targetDistance)
		{
			num = 45f;
		}
		return (Quaternion.Euler(0f, 0f, num * Mathf.Sign((float)direction)) * new Vector3(vector.x, vector.y, 0f)).normalized * this.MovementSpeed;
	}

	// Token: 0x060055EA RID: 21994 RVA: 0x0020C258 File Offset: 0x0020A458
	private Vector2 CalculateSteering()
	{
		if (!this.TryDodgeBullets)
		{
			return Vector2.zero;
		}
		float num = 5f;
		Collider[] array = Physics.OverlapSphere(base.specRigidbody.UnitCenter, this.AvoidRadius);
		Vector2 vector = Vector2.zero;
		int num2 = 0;
		foreach (Collider collider in array)
		{
			if (collider.transform.parent != null && collider.transform.parent.GetComponent<Projectile>() != null)
			{
				SpeculativeRigidbody component = collider.transform.parent.GetComponent<SpeculativeRigidbody>();
				Vector2 velocity = component.Velocity;
				float num3 = Vector3.Distance(collider.transform.position, base.specRigidbody.UnitCenter);
				Vector3 vector2 = collider.transform.position + new Vector3(velocity.normalized.x * num3, velocity.normalized.y * num3, 0f);
				if (Vector3.Distance(base.specRigidbody.UnitCenter, vector2) <= Vector3.Distance(base.specRigidbody.UnitCenter, collider.transform.position))
				{
					int num4 = ((base.specRigidbody.UnitCenter.x >= collider.transform.position.x) ? 1 : (-1));
					Vector2 vector3 = (base.specRigidbody.UnitCenter - collider.transform.position) * (float)num4;
					Vector2 vector4 = (vector2 - collider.transform.position) * (float)num4;
					float num5 = Mathf.Atan2(vector3.y, vector3.x);
					float num6 = Mathf.Atan2(vector4.y, vector4.x);
					int num7 = ((num5 <= num6) ? (-90) : 90);
					float num8 = num3 / this.AvoidRadius;
					Vector3 vector5 = Quaternion.Euler(0f, 0f, (float)num7) * new Vector3(velocity.x, velocity.y, 0f);
					Vector2 vector6 = new Vector2(vector5.x, vector5.y);
					Vector2 normalized = vector6.normalized;
					vector += normalized * (1f - num8);
					num2++;
				}
			}
		}
		if (num2 > 0)
		{
			vector = vector / (float)num2 * num;
		}
		return vector;
	}

	// Token: 0x060055EB RID: 21995 RVA: 0x0020C508 File Offset: 0x0020A708
	protected override void Fall()
	{
		if (this.m_isFalling)
		{
			return;
		}
		base.Fall();
		if (base.behaviorSpeculator)
		{
			base.behaviorSpeculator.InterruptAndDisable();
		}
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "Pitfall");
		}
		if (base.aiAnimator)
		{
			base.aiAnimator.FpsScale = 1f;
			if (!string.IsNullOrEmpty(this.OverridePitfallAnim))
			{
				base.aiAnimator.PlayUntilCancelled(this.OverridePitfallAnim, false, null, -1f, false);
			}
			else if (base.aiAnimator.HasDirectionalAnimation("pitfall"))
			{
				base.aiAnimator.PlayUntilCancelled("pitfall", false, null, -1f, false);
			}
			else if (base.spriteAnimator.GetClipByName("pitfall") != null)
			{
				base.aiAnimator.PlayUntilCancelled("pitfall", false, null, -1f, false);
			}
			else if (base.spriteAnimator.GetClipByName("pitfall_right") != null)
			{
				base.aiAnimator.PlayUntilCancelled("pitfall_right", false, null, -1f, false);
			}
		}
		base.StartCoroutine(this.FallDownCR());
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x060055EC RID: 21996 RVA: 0x0020C654 File Offset: 0x0020A854
	// (set) Token: 0x060055ED RID: 21997 RVA: 0x0020C65C File Offset: 0x0020A85C
	public bool HasSplashed { get; set; }

	// Token: 0x060055EE RID: 21998 RVA: 0x0020C668 File Offset: 0x0020A868
	private IEnumerator FallDownCR()
	{
		base.specRigidbody.CollideWithTileMap = false;
		base.specRigidbody.CollideWithOthers = false;
		base.IsGone = true;
		base.specRigidbody.Velocity = Vector2.zero;
		Vector2 accelVec = new Vector2(0f, -80f);
		float elapsed = 0f;
		Tribool readyForDepthSwap = Tribool.Unready;
		float m_cachedHeightOffGround = base.sprite.HeightOffGround;
		this.HasSplashed = false;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PitFallMovementRestrictor));
		while (base.renderer.enabled)
		{
			base.specRigidbody.Velocity = base.specRigidbody.Velocity + accelVec * this.LocalDeltaTime;
			bool isPlayingPitfall = base.aiAnimator && (base.aiAnimator.IsPlaying("pitfall") || base.aiAnimator.IsPlaying("pitfall_down") || base.aiAnimator.IsPlaying("pitfall_right"));
			if (!string.IsNullOrEmpty(this.OverridePitfallAnim) && base.aiAnimator)
			{
				isPlayingPitfall |= base.aiAnimator.IsPlaying(this.OverridePitfallAnim);
			}
			if (!isPlayingPitfall && elapsed > 0.1f)
			{
				base.renderer.enabled = false;
				SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
				base.specRigidbody.Velocity = Vector2.zero;
				accelVec = Vector2.zero;
				if (!this.HasSplashed)
				{
					this.HasSplashed = true;
					GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(base.sprite.WorldCenter);
				}
			}
			if (!(readyForDepthSwap == Tribool.Complete) || !base.renderer.enabled)
			{
				if (readyForDepthSwap)
				{
					base.sprite.HeightOffGround = -4f;
					if (this.IsNormalEnemy)
					{
						TileSpriteClipper tileSpriteClipper = base.sprite.gameObject.AddComponent<TileSpriteClipper>();
						tileSpriteClipper.updateEveryFrame = true;
						tileSpriteClipper.doOptimize = false;
						tileSpriteClipper.clipMode = TileSpriteClipper.ClipMode.PitBounds;
						tileSpriteClipper.enabled = true;
						tk2dBaseSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(base.sprite);
						for (int i = 0; i < outlineSprites.Length; i++)
						{
							if (outlineSprites[i])
							{
								tileSpriteClipper = outlineSprites[i].gameObject.AddComponent<TileSpriteClipper>();
								tileSpriteClipper.updateEveryFrame = true;
								tileSpriteClipper.doOptimize = false;
								tileSpriteClipper.clipMode = TileSpriteClipper.ClipMode.PitBounds;
								tileSpriteClipper.enabled = true;
							}
						}
					}
					readyForDepthSwap = Tribool.op_Increment(readyForDepthSwap);
				}
				else if (!readyForDepthSwap)
				{
					Vector3 vector = base.sprite.transform.position + base.sprite.GetBounds().center + new Vector3(0f, base.sprite.GetBounds().extents.y, 0f);
					if (GameManager.Instance.Dungeon.CellSupportsFalling(vector))
					{
						readyForDepthSwap = Tribool.op_Increment(readyForDepthSwap);
					}
				}
			}
			base.sprite.UpdateZDepth();
			elapsed += this.LocalDeltaTime;
			yield return null;
		}
		base.sprite.HeightOffGround = m_cachedHeightOffGround;
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PitFallMovementRestrictor));
		bool suppressDamage = false;
		if (this.CustomPitDeathHandling != null)
		{
			this.CustomPitDeathHandling(this, ref suppressDamage);
		}
		if (!suppressDamage)
		{
			base.healthHaver.IsVulnerable = true;
			base.healthHaver.minimumHealth = 0f;
			base.healthHaver.ApplyDamage(float.MaxValue, Vector2.zero, "enemy pit", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.ENEMIES_KILLED_WITH_PITS, 1f);
		}
		yield break;
	}

	// Token: 0x060055EF RID: 21999 RVA: 0x0020C684 File Offset: 0x0020A884
	public override void RecoverFromFall()
	{
		base.RecoverFromFall();
		if (base.behaviorSpeculator)
		{
			base.behaviorSpeculator.enabled = true;
		}
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(true, "Pitfall");
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
		if (base.aiAnimator)
		{
			base.aiAnimator.EndAnimation();
		}
		base.specRigidbody.CollideWithTileMap = true;
		base.specRigidbody.CollideWithOthers = true;
		base.IsGone = false;
		base.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
	}

	// Token: 0x060055F0 RID: 22000 RVA: 0x0020C73C File Offset: 0x0020A93C
	private void PitFallMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		PixelCollider hitboxPixelCollider = specRigidbody.HitboxPixelCollider;
		Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(hitboxPixelCollider.UpperLeft);
		Vector2 vector2 = PhysicsEngine.PixelToUnitMidpoint(hitboxPixelCollider.UpperRight);
		Vector2 vector3 = PhysicsEngine.PixelToUnitMidpoint(hitboxPixelCollider.LowerLeft);
		Vector2 vector4 = PhysicsEngine.PixelToUnitMidpoint(hitboxPixelCollider.LowerRight);
		Vector2 vector5 = PhysicsEngine.PixelToUnit(prevPixelOffset);
		Vector2 vector6 = PhysicsEngine.PixelToUnit(pixelOffset);
		if ((GameManager.Instance.Dungeon.CellIsPit(vector + vector5) && !GameManager.Instance.Dungeon.CellIsPit(vector + vector6)) || (GameManager.Instance.Dungeon.CellIsPit(vector2 + vector5) && !GameManager.Instance.Dungeon.CellIsPit(vector2 + vector6)) || (GameManager.Instance.Dungeon.CellIsPit(vector3 + vector5) && !GameManager.Instance.Dungeon.CellIsPit(vector3 + vector6)) || (GameManager.Instance.Dungeon.CellIsPit(vector4 + vector5) && !GameManager.Instance.Dungeon.CellIsPit(vector4 + vector6)))
		{
			validLocation = false;
			return;
		}
	}

	// Token: 0x060055F1 RID: 22001 RVA: 0x0020C8AC File Offset: 0x0020AAAC
	public void MoveToSafeSpot(float time)
	{
		this.m_isSafeMoving = false;
		if (!GameManager.HasInstance || GameManager.Instance.Dungeon == null)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		Vector2[] array = new Vector2[]
		{
			base.specRigidbody.UnitBottomLeft,
			base.specRigidbody.UnitBottomCenter,
			base.specRigidbody.UnitBottomRight,
			base.specRigidbody.UnitTopLeft,
			base.specRigidbody.UnitTopCenter,
			base.specRigidbody.UnitTopRight
		};
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			IntVector2 intVector = array[i].ToIntVector2(VectorConversions.Floor);
			if (!data.CheckInBoundsAndValid(intVector) || data.isWall(intVector) || data.isTopWall(intVector.x, intVector.y) || data[intVector].isOccupied)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int j = 0; j < this.Clearance.x; j++)
			{
				int num = c.x + j;
				for (int k = 0; k < this.Clearance.y; k++)
				{
					int num2 = c.y + k;
					if (GameManager.Instance.Dungeon.data.isTopWall(num, num2))
					{
						return false;
					}
				}
			}
			return true;
		};
		Vector2 vector = base.specRigidbody.UnitBottomCenter - base.transform.position.XY();
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		IntVector2? nearestAvailableCell = absoluteRoomFromPosition.GetNearestAvailableCell(base.specRigidbody.UnitCenter, new IntVector2?(this.Clearance), new CellTypes?(this.PathableTiles), false, cellValidator);
		if (nearestAvailableCell != null)
		{
			this.m_isSafeMoving = true;
			this.m_safeMoveTimer = 0f;
			this.m_safeMoveTime = time;
			this.m_safeMoveStartPos = new Vector2?(base.transform.position);
			this.m_safeMoveEndPos = new Vector2?(Pathfinder.GetClearanceOffset(nearestAvailableCell.Value, this.Clearance).WithY((float)nearestAvailableCell.Value.y) - vector);
		}
		else
		{
			this.m_safeMoveStartPos = null;
			this.m_safeMoveEndPos = null;
		}
	}

	// Token: 0x04004E36 RID: 20022
	private static readonly string[] s_floorTypeNames = Enum.GetNames(typeof(CellVisualData.CellFloorType));

	// Token: 0x04004E37 RID: 20023
	private static float m_healthModifier = 1f;

	// Token: 0x04004E38 RID: 20024
	[HideInInspector]
	public int EnemyId = -1;

	// Token: 0x04004E39 RID: 20025
	[DisableInInspector]
	public string EnemyGuid;

	// Token: 0x04004E3A RID: 20026
	[DisableInInspector]
	public int ForcedPositionInAmmonomicon = -1;

	// Token: 0x04004E3B RID: 20027
	[Header("Flags")]
	public bool SetsFlagOnDeath;

	// Token: 0x04004E3C RID: 20028
	[LongEnum]
	public GungeonFlags FlagToSetOnDeath;

	// Token: 0x04004E3D RID: 20029
	public bool SetsFlagOnActivation;

	// Token: 0x04004E3E RID: 20030
	[LongEnum]
	public GungeonFlags FlagToSetOnActivation;

	// Token: 0x04004E3F RID: 20031
	public bool SetsCharacterSpecificFlagOnDeath;

	// Token: 0x04004E40 RID: 20032
	[LongEnum]
	public CharacterSpecificGungeonFlags CharacterSpecificFlagToSetOnDeath;

	// Token: 0x04004E41 RID: 20033
	[Header("Core Enemy Stats")]
	public bool IsNormalEnemy = true;

	// Token: 0x04004E42 RID: 20034
	public bool IsSignatureEnemy;

	// Token: 0x04004E43 RID: 20035
	public bool IsHarmlessEnemy;

	// Token: 0x04004E44 RID: 20036
	[HideInInspectorIf("IsNormalEnemy", false)]
	public ActorCompanionSettings CompanionSettings;

	// Token: 0x04004E45 RID: 20037
	[NonSerialized]
	public bool ForceBlackPhantom;

	// Token: 0x04004E46 RID: 20038
	[NonSerialized]
	public bool PreventBlackPhantom;

	// Token: 0x04004E47 RID: 20039
	[NonSerialized]
	public bool IsInReinforcementLayer;

	// Token: 0x04004E48 RID: 20040
	[NonSerialized]
	public PlayerController CompanionOwner;

	// Token: 0x04004E49 RID: 20041
	[FormerlySerializedAs("m_movementSpeed")]
	[SerializeField]
	public float MovementSpeed = 2f;

	// Token: 0x04004E4A RID: 20042
	[EnumFlags]
	public CellTypes PathableTiles = CellTypes.FLOOR;

	// Token: 0x04004E4B RID: 20043
	public GameObject LosPoint;

	// Token: 0x04004E4C RID: 20044
	[Header("Collision Data")]
	public bool DiesOnCollison;

	// Token: 0x04004E4D RID: 20045
	public float CollisionDamage = 1f;

	// Token: 0x04004E4E RID: 20046
	public float CollisionKnockbackStrength = 5f;

	// Token: 0x04004E4F RID: 20047
	public CoreDamageTypes CollisionDamageTypes;

	// Token: 0x04004E50 RID: 20048
	public float EnemyCollisionKnockbackStrengthOverride = -1f;

	// Token: 0x04004E51 RID: 20049
	public VFXPool CollisionVFX;

	// Token: 0x04004E52 RID: 20050
	public VFXPool NonActorCollisionVFX;

	// Token: 0x04004E53 RID: 20051
	public bool CollisionSetsPlayerOnFire;

	// Token: 0x04004E54 RID: 20052
	public bool TryDodgeBullets = true;

	// Token: 0x04004E55 RID: 20053
	public float AvoidRadius = 4f;

	// Token: 0x04004E56 RID: 20054
	public bool ReflectsProjectilesWhileInvulnerable;

	// Token: 0x04004E57 RID: 20055
	public bool HitByEnemyBullets;

	// Token: 0x04004E58 RID: 20056
	public bool HasOverrideDodgeRollDeath;

	// Token: 0x04004E59 RID: 20057
	[ShowInInspectorIf("HasOverrideDodgeRollDeath", false)]
	public string OverrideDodgeRollDeath;

	// Token: 0x04004E5A RID: 20058
	[Header("Loot Settings")]
	public bool CanDropCurrency = true;

	// Token: 0x04004E5B RID: 20059
	public float AdditionalSingleCoinDropChance;

	// Token: 0x04004E5C RID: 20060
	[NonSerialized]
	public int AssignedCurrencyToDrop;

	// Token: 0x04004E5D RID: 20061
	public bool CanDropItems = true;

	// Token: 0x04004E5E RID: 20062
	[ShowInInspectorIf("CanDropCurrency", true)]
	public GenericLootTable CustomLootTable;

	// Token: 0x04004E5F RID: 20063
	public bool CanDropDuplicateItems;

	// Token: 0x04004E60 RID: 20064
	public int CustomLootTableMinDrops = 1;

	// Token: 0x04004E61 RID: 20065
	public int CustomLootTableMaxDrops = 1;

	// Token: 0x04004E62 RID: 20066
	public GenericLootTable CustomChestTable;

	// Token: 0x04004E63 RID: 20067
	public float ChanceToDropCustomChest;

	// Token: 0x04004E64 RID: 20068
	public bool IgnoreForRoomClear;

	// Token: 0x04004E65 RID: 20069
	[HideInInspector]
	[NonSerialized]
	public List<PickupObject> AdditionalSimpleItemDrops = new List<PickupObject>();

	// Token: 0x04004E66 RID: 20070
	[HideInInspector]
	[NonSerialized]
	public List<PickupObject> AdditionalSafeItemDrops = new List<PickupObject>();

	// Token: 0x04004E67 RID: 20071
	public bool SpawnLootAtRewardChestPos;

	// Token: 0x04004E68 RID: 20072
	[Header("Extra Visual Settings")]
	public GameObject CorpseObject;

	// Token: 0x04004E69 RID: 20073
	[ShowInInspectorIf("CorpseObject", true)]
	public bool CorpseShadow = true;

	// Token: 0x04004E6A RID: 20074
	[ShowInInspectorIf("CorpseObject", true)]
	public bool TransferShadowToCorpse;

	// Token: 0x04004E6B RID: 20075
	public AIActor.ShadowDeathType shadowDeathType = AIActor.ShadowDeathType.Fade;

	// Token: 0x04004E6C RID: 20076
	public bool PreventDeathKnockback;

	// Token: 0x04004E6D RID: 20077
	public VFXPool OnCorpseVFX;

	// Token: 0x04004E6E RID: 20078
	public GameObject OnEngagedVFX;

	// Token: 0x04004E6F RID: 20079
	public tk2dBaseSprite.Anchor OnEngagedVFXAnchor;

	// Token: 0x04004E70 RID: 20080
	public float shadowHeightOffset;

	// Token: 0x04004E71 RID: 20081
	public bool invisibleUntilAwaken;

	// Token: 0x04004E72 RID: 20082
	public bool procedurallyOutlined = true;

	// Token: 0x04004E73 RID: 20083
	public bool forceUsesTrimmedBounds = true;

	// Token: 0x04004E74 RID: 20084
	public AIActor.ReinforceType reinforceType;

	// Token: 0x04004E75 RID: 20085
	public Texture2D optionalPalette;

	// Token: 0x04004E76 RID: 20086
	public bool UsesVaryingEmissiveShaderPropertyBlock;

	// Token: 0x04004E77 RID: 20087
	public Transform OverrideBuffEffectPosition;

	// Token: 0x04004E78 RID: 20088
	[Header("Audio")]
	public string EnemySwitchState;

	// Token: 0x04004E79 RID: 20089
	public string OverrideSpawnReticleAudio;

	// Token: 0x04004E7A RID: 20090
	public string OverrideSpawnAppearAudio;

	// Token: 0x04004E7B RID: 20091
	public bool UseMovementAudio;

	// Token: 0x04004E7C RID: 20092
	[ShowInInspectorIf("UseMovementAudio", true)]
	public string StartMovingEvent;

	// Token: 0x04004E7D RID: 20093
	[ShowInInspectorIf("UseMovementAudio", true)]
	public string StopMovingEvent;

	// Token: 0x04004E7E RID: 20094
	private bool m_audioMovedLastFrame;

	// Token: 0x04004E7F RID: 20095
	[SerializeField]
	public List<ActorAudioEvent> animationAudioEvents;

	// Token: 0x04004E80 RID: 20096
	[Header("Other")]
	public List<AIActor.HealthOverride> HealthOverrides;

	// Token: 0x04004E81 RID: 20097
	public AIActor.EnemyTypeIdentifier IdentifierForEffects;

	// Token: 0x04004E82 RID: 20098
	[HideInInspector]
	public bool BehaviorOverridesVelocity;

	// Token: 0x04004E83 RID: 20099
	[HideInInspector]
	public Vector2 BehaviorVelocity = Vector2.zero;

	// Token: 0x04004E84 RID: 20100
	public Vector2? OverridePathVelocity;

	// Token: 0x04004E85 RID: 20101
	public bool AlwaysShowOffscreenArrow;

	// Token: 0x04004E86 RID: 20102
	[NonSerialized]
	public float BaseMovementSpeed;

	// Token: 0x04004E87 RID: 20103
	[NonSerialized]
	public float LocalTimeScale = 1f;

	// Token: 0x04004E88 RID: 20104
	[NonSerialized]
	public bool UniquePlayerTargetFlag;

	// Token: 0x04004E89 RID: 20105
	private bool? m_cachedIsMimicEnemy;

	// Token: 0x04004E8A RID: 20106
	[NonSerialized]
	public bool HasBeenBloodthirstProcessed;

	// Token: 0x04004E8B RID: 20107
	[NonSerialized]
	public bool CanBeBloodthirsted;

	// Token: 0x04004E8C RID: 20108
	private Vector2 m_currentlyAppliedEnemyScale = Vector2.one;

	// Token: 0x04004E8E RID: 20110
	private bool m_canTargetPlayers = true;

	// Token: 0x04004E8F RID: 20111
	private bool m_canTargetEnemies;

	// Token: 0x04004E90 RID: 20112
	public BlackPhantomProperties BlackPhantomProperties;

	// Token: 0x04004E91 RID: 20113
	public bool ForceBlackPhantomParticles;

	// Token: 0x04004E92 RID: 20114
	public bool OverrideBlackPhantomParticlesCollider;

	// Token: 0x04004E93 RID: 20115
	[ShowInInspectorIf("OverrideBlackPhantomParticlesCollider", true)]
	public int BlackPhantomParticlesCollider;

	// Token: 0x04004E94 RID: 20116
	private AIActor.EnemyChampionType m_championType;

	// Token: 0x04004E99 RID: 20121
	private bool? m_isWorthShootingAt;

	// Token: 0x04004E9B RID: 20123
	public bool PreventFallingInPitsEver;

	// Token: 0x04004EA1 RID: 20129
	private bool m_isPaletteSwapped;

	// Token: 0x04004EA6 RID: 20134
	[NonSerialized]
	private Color? OverrideOutlineColor;

	// Token: 0x04004EA7 RID: 20135
	private bool m_cachedTurboness;

	// Token: 0x04004EA8 RID: 20136
	private bool m_turboWake;

	// Token: 0x04004EA9 RID: 20137
	private int m_cachedBodySpriteCount;

	// Token: 0x04004EAA RID: 20138
	private Shader m_cachedBodySpriteShader;

	// Token: 0x04004EAB RID: 20139
	private Shader m_cachedGunSpriteShader;

	// Token: 0x04004EAC RID: 20140
	private bool? ShouldDoBlackPhantomParticles;

	// Token: 0x04004EAD RID: 20141
	private const float c_particlesPerSecond = 40f;

	// Token: 0x04004EAE RID: 20142
	private List<IntVector2> m_upcomingPathTiles = new List<IntVector2>();

	// Token: 0x04004EAF RID: 20143
	private bool m_cachedHasLineOfSightToTarget;

	// Token: 0x04004EB0 RID: 20144
	private SpeculativeRigidbody m_cachedLosTarget;

	// Token: 0x04004EB1 RID: 20145
	private int m_cachedLosFrame;

	// Token: 0x04004EB2 RID: 20146
	private Vector2 m_lastPosition;

	// Token: 0x04004EB3 RID: 20147
	private IntVector2? m_clearance;

	// Token: 0x04004EB4 RID: 20148
	private CellVisualData.CellFloorType? m_prevFloorType;

	// Token: 0x04004EB7 RID: 20151
	protected Action OnPostStartInitialization;

	// Token: 0x04004EB8 RID: 20152
	private bool m_hasGivenRewards;

	// Token: 0x04004EB9 RID: 20153
	public Action OnHandleRewards;

	// Token: 0x04004EBB RID: 20155
	private bool m_isSafeMoving;

	// Token: 0x04004EBC RID: 20156
	private float m_safeMoveTimer;

	// Token: 0x04004EBD RID: 20157
	private float m_safeMoveTime;

	// Token: 0x04004EBE RID: 20158
	private Vector2? m_safeMoveStartPos;

	// Token: 0x04004EBF RID: 20159
	private Vector2? m_safeMoveEndPos;

	// Token: 0x04004EC0 RID: 20160
	private CustomEngageDoer m_customEngageDoer;

	// Token: 0x04004EC1 RID: 20161
	private CustomReinforceDoer m_customReinforceDoer;

	// Token: 0x04004EC2 RID: 20162
	private Func<SpeculativeRigidbody, bool> m_rigidbodyExcluder;

	// Token: 0x04004EC3 RID: 20163
	private Vector3 m_spriteDimensions;

	// Token: 0x04004EC4 RID: 20164
	private Vector3 m_spawnPosition;

	// Token: 0x04004EC5 RID: 20165
	private RoomHandler parentRoom;

	// Token: 0x04004EC6 RID: 20166
	private int m_currentPhase;

	// Token: 0x04004EC7 RID: 20167
	private bool m_isReadyForRepath = true;

	// Token: 0x04004EC8 RID: 20168
	private Path m_currentPath;

	// Token: 0x04004EC9 RID: 20169
	private Vector2? m_overridePathEnd;

	// Token: 0x04004ECA RID: 20170
	private int m_strafeDirection = 1;

	// Token: 0x04004ECB RID: 20171
	private bool m_hasBeenEngaged;

	// Token: 0x04004ECC RID: 20172
	private string m_awakenAnimation;

	// Token: 0x04004ECD RID: 20173
	protected bool? m_forcedOutlines;

	// Token: 0x04004ECE RID: 20174
	private Vector2 m_knockbackVelocity;

	// Token: 0x04004ECF RID: 20175
	public const float c_minStartingDistanceFromPlayer = 8f;

	// Token: 0x04004ED0 RID: 20176
	public const float c_maxCloseStartingDistanceFromPlayer = 15f;

	// Token: 0x02000F78 RID: 3960
	public enum ReinforceType
	{
		// Token: 0x04004ED2 RID: 20178
		FullVfx,
		// Token: 0x04004ED3 RID: 20179
		SkipVfx,
		// Token: 0x04004ED4 RID: 20180
		Instant
	}

	// Token: 0x02000F79 RID: 3961
	public enum ShadowDeathType
	{
		// Token: 0x04004ED6 RID: 20182
		Fade = 10,
		// Token: 0x04004ED7 RID: 20183
		Scale = 20,
		// Token: 0x04004ED8 RID: 20184
		None = 30
	}

	// Token: 0x02000F7A RID: 3962
	public enum EnemyTypeIdentifier
	{
		// Token: 0x04004EDA RID: 20186
		UNIDENTIFIED,
		// Token: 0x04004EDB RID: 20187
		SNIPER_TYPE
	}

	// Token: 0x02000F7B RID: 3963
	public enum EnemyChampionType
	{
		// Token: 0x04004EDD RID: 20189
		NORMAL,
		// Token: 0x04004EDE RID: 20190
		JAMMED,
		// Token: 0x04004EDF RID: 20191
		KTHULIBER_JAMMED
	}

	// Token: 0x02000F7C RID: 3964
	// (Invoke) Token: 0x060055F4 RID: 22004
	public delegate void CustomPitHandlingDelegate(AIActor actor, ref bool suppressDamage);

	// Token: 0x02000F7D RID: 3965
	public enum ActorState
	{
		// Token: 0x04004EE1 RID: 20193
		Inactive,
		// Token: 0x04004EE2 RID: 20194
		Awakening,
		// Token: 0x04004EE3 RID: 20195
		Normal
	}

	// Token: 0x02000F7E RID: 3966
	public enum AwakenAnimationType
	{
		// Token: 0x04004EE5 RID: 20197
		Default,
		// Token: 0x04004EE6 RID: 20198
		Awaken,
		// Token: 0x04004EE7 RID: 20199
		Spawn
	}

	// Token: 0x02000F7F RID: 3967
	[Serializable]
	public class HealthOverride
	{
		// Token: 0x04004EE8 RID: 20200
		public float HealthPercentage;

		// Token: 0x04004EE9 RID: 20201
		public string Stat;

		// Token: 0x04004EEA RID: 20202
		public float Value;

		// Token: 0x04004EEB RID: 20203
		[NonSerialized]
		public bool HasBeenUsed;
	}
}
