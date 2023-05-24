using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020014B6 RID: 5302
public class SpawnObjectPlayerItem : PlayerItem
{
	// Token: 0x0600788E RID: 30862 RVA: 0x00302F38 File Offset: 0x00301138
	public override bool CanBeUsed(PlayerController user)
	{
		return (!this.IsCigarettes || !user || !user.healthHaver || user.healthHaver.IsVulnerable) && (!this.RequireEnemiesInRoom || !user || user.CurrentRoom == null || user.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) != 0) && base.CanBeUsed(user);
	}

	// Token: 0x0600788F RID: 30863 RVA: 0x00302FB8 File Offset: 0x003011B8
	protected override void DoEffect(PlayerController user)
	{
		if (this.IsCigarettes)
		{
			user.healthHaver.ApplyDamage(0.5f, Vector2.zero, StringTableManager.GetEnemiesString("#SMOKING", -1), CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
			StatModifier statModifier = new StatModifier();
			statModifier.statToBoost = PlayerStats.StatType.Coolness;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.amount = 1f;
			user.ownerlessStatModifiers.Add(statModifier);
			user.stats.RecalculateStats(user, false, false);
		}
		else if (this.itemName == "Molotov" && user && user.HasActiveBonusSynergy(CustomSynergyType.DOUBLE_MOLOTOV, false))
		{
			user.CurrentGun.GainAmmo(5);
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
			return;
		}
		if (this.IsKageBunshinItem && user && user.HasActiveBonusSynergy(CustomSynergyType.KINJUTSU, false))
		{
			for (int i = 0; i < 3; i++)
			{
				float num = (float)(90 * (i + 1));
				this.DoSpawn(user, num);
			}
			if (this.PreventCooldownWhileExtant)
			{
				base.IsCurrentlyActive = true;
			}
			if (!string.IsNullOrEmpty(this.AudioEvent))
			{
				AkSoundEngine.PostEvent(this.AudioEvent, base.gameObject);
			}
		}
		else if (this.SpawnRadialCopies)
		{
			for (int j = 0; j < this.RadialCopiesToSpawn; j++)
			{
				float num2 = 360f / (float)this.RadialCopiesToSpawn * (float)j;
				this.DoSpawn(user, num2);
			}
		}
		else
		{
			this.DoSpawn(user, 0f);
			if (this.PreventCooldownWhileExtant)
			{
				base.IsCurrentlyActive = true;
			}
			if (!string.IsNullOrEmpty(this.AudioEvent))
			{
				AkSoundEngine.PostEvent(this.AudioEvent, base.gameObject);
			}
		}
	}

	// Token: 0x06007890 RID: 30864 RVA: 0x00303184 File Offset: 0x00301384
	public override void Update()
	{
		if (base.IsCurrentlyActive && this.PreventCooldownWhileExtant && !this.spawnedPlayerObject)
		{
			if (this.m_elapsedCooldownWhileExtantTimer < 0.5f)
			{
				this.m_elapsedCooldownWhileExtantTimer += BraveTime.DeltaTime;
			}
			else
			{
				Debug.LogError("clearing the dillywop");
				this.m_elapsedCooldownWhileExtantTimer = 0f;
				base.IsCurrentlyActive = false;
			}
		}
		base.Update();
	}

	// Token: 0x06007891 RID: 30865 RVA: 0x00303200 File Offset: 0x00301400
	protected void DoSpawn(PlayerController user, float angleFromAim)
	{
		if (!string.IsNullOrEmpty(this.enemyGuidToSpawn))
		{
			this.objectToSpawn = EnemyDatabase.GetOrLoadByGuid(this.enemyGuidToSpawn).gameObject;
		}
		GameObject synergyObjectToSpawn = this.objectToSpawn;
		if (this.HasOverrideSynergyItem && user.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			synergyObjectToSpawn = this.SynergyObjectToSpawn;
		}
		Projectile component = synergyObjectToSpawn.GetComponent<Projectile>();
		this.m_elapsedCooldownWhileExtantTimer = 0f;
		if (component != null)
		{
			Vector2 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
			this.spawnedPlayerObject = UnityEngine.Object.Instantiate<GameObject>(synergyObjectToSpawn, user.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(vector)));
		}
		else if (this.tossForce == 0f)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(synergyObjectToSpawn, user.specRigidbody.UnitCenter, Quaternion.identity);
			this.spawnedPlayerObject = gameObject;
			tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
			if (component2 != null)
			{
				component2.PlaceAtPositionByAnchor(user.specRigidbody.UnitCenter.ToVector3ZUp(component2.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
				if (component2.specRigidbody != null)
				{
					component2.specRigidbody.RegisterGhostCollisionException(user.specRigidbody);
				}
			}
			KageBunshinController component3 = gameObject.GetComponent<KageBunshinController>();
			if (component3)
			{
				component3.InitializeOwner(user);
			}
			if (this.IsKageBunshinItem && user.HasActiveBonusSynergy(CustomSynergyType.KINJUTSU, false))
			{
				component3.UsesRotationInsteadOfInversion = true;
				component3.RotationAngle = angleFromAim;
			}
			gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
		}
		else
		{
			Vector3 vector2 = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
			Vector3 vector3 = user.specRigidbody.UnitCenter;
			if (vector2.y > 0f)
			{
				vector3 += Vector3.up * 0.25f;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(synergyObjectToSpawn, vector3, Quaternion.identity);
			tk2dBaseSprite component4 = gameObject2.GetComponent<tk2dBaseSprite>();
			if (component4)
			{
				component4.PlaceAtPositionByAnchor(vector3, tk2dBaseSprite.Anchor.MiddleCenter);
			}
			this.spawnedPlayerObject = gameObject2;
			Vector2 vector4 = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
			vector4 = Quaternion.Euler(0f, 0f, angleFromAim) * vector4;
			DebrisObject debrisObject = LootEngine.DropItemWithoutInstantiating(gameObject2, gameObject2.transform.position, vector4, this.tossForce, false, false, true, false);
			if (gameObject2.GetComponent<BlackHoleDoer>())
			{
				debrisObject.PreventFallingInPits = true;
				debrisObject.PreventAbsorption = true;
			}
			if (vector2.y > 0f && debrisObject)
			{
				debrisObject.additionalHeightBoost = -1f;
				if (debrisObject.sprite)
				{
					debrisObject.sprite.UpdateZDepth();
				}
			}
			debrisObject.IsAccurateDebris = true;
			debrisObject.Priority = EphemeralObject.EphemeralPriority.Critical;
			debrisObject.bounceCount = ((!this.canBounce) ? 0 : 1);
		}
		if (this.spawnedPlayerObject)
		{
			PortableTurretController component5 = this.spawnedPlayerObject.GetComponent<PortableTurretController>();
			if (component5)
			{
				component5.sourcePlayer = this.LastOwner;
			}
			Projectile componentInChildren = this.spawnedPlayerObject.GetComponentInChildren<Projectile>();
			if (componentInChildren)
			{
				componentInChildren.Owner = this.LastOwner;
				componentInChildren.TreatedAsNonProjectileForChallenge = true;
			}
			SpawnObjectItem componentInChildren2 = this.spawnedPlayerObject.GetComponentInChildren<SpawnObjectItem>();
			if (componentInChildren2)
			{
				componentInChildren2.SpawningPlayer = this.LastOwner;
			}
		}
	}

	// Token: 0x06007892 RID: 30866 RVA: 0x003035CC File Offset: 0x003017CC
	protected override void OnPreDrop(PlayerController user)
	{
		if (this.spawnedPlayerObject != null)
		{
			PortableTurretController component = this.spawnedPlayerObject.GetComponent<PortableTurretController>();
			if (component != null)
			{
				component.NotifyDropped();
			}
		}
		base.OnPreDrop(user);
	}

	// Token: 0x06007893 RID: 30867 RVA: 0x00303610 File Offset: 0x00301810
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007ABC RID: 31420
	[Header("Spawn Object Settings")]
	public GameObject objectToSpawn;

	// Token: 0x04007ABD RID: 31421
	[EnemyIdentifier]
	public string enemyGuidToSpawn;

	// Token: 0x04007ABE RID: 31422
	public bool HasOverrideSynergyItem;

	// Token: 0x04007ABF RID: 31423
	[LongNumericEnum]
	[ShowInInspectorIf("HasOverrideSynergyItem", false)]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007AC0 RID: 31424
	[ShowInInspectorIf("HasOverrideSynergyItem", false)]
	public GameObject SynergyObjectToSpawn;

	// Token: 0x04007AC1 RID: 31425
	public float tossForce;

	// Token: 0x04007AC2 RID: 31426
	public bool canBounce = true;

	// Token: 0x04007AC3 RID: 31427
	public bool IsCigarettes;

	// Token: 0x04007AC4 RID: 31428
	[NonSerialized]
	public GameObject spawnedPlayerObject;

	// Token: 0x04007AC5 RID: 31429
	public bool PreventCooldownWhileExtant;

	// Token: 0x04007AC6 RID: 31430
	public bool RequireEnemiesInRoom;

	// Token: 0x04007AC7 RID: 31431
	public bool SpawnRadialCopies;

	// Token: 0x04007AC8 RID: 31432
	[ShowInInspectorIf("SpawnRadialCopies", false)]
	public int RadialCopiesToSpawn = 1;

	// Token: 0x04007AC9 RID: 31433
	public string AudioEvent;

	// Token: 0x04007ACA RID: 31434
	public bool IsKageBunshinItem;

	// Token: 0x04007ACB RID: 31435
	private float m_elapsedCooldownWhileExtantTimer;
}
