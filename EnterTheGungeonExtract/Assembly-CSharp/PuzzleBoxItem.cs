using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001470 RID: 5232
public class PuzzleBoxItem : PlayerItem
{
	// Token: 0x060076F1 RID: 30449 RVA: 0x002F6F5C File Offset: 0x002F515C
	public override bool CanBeUsed(PlayerController user)
	{
		return (!user || !user.InExitCell) && (!user || user.CurrentRoom == null || !user.CurrentRoom.IsShop) && base.CanBeUsed(user);
	}

	// Token: 0x060076F2 RID: 30450 RVA: 0x002F6FB0 File Offset: 0x002F51B0
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
	}

	// Token: 0x060076F3 RID: 30451 RVA: 0x002F6FBC File Offset: 0x002F51BC
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
	}

	// Token: 0x060076F4 RID: 30452 RVA: 0x002F6FC8 File Offset: 0x002F51C8
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.m_numberOfUses);
	}

	// Token: 0x060076F5 RID: 30453 RVA: 0x002F6FE4 File Offset: 0x002F51E4
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 1)
		{
			this.m_numberOfUses = (int)data[0];
		}
	}

	// Token: 0x060076F6 RID: 30454 RVA: 0x002F700C File Offset: 0x002F520C
	private void PlayTeleporterEffect(PlayerController p)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (!GameManager.Instance.AllPlayers[i].IsGhost)
			{
				GameManager.Instance.AllPlayers[i].healthHaver.TriggerInvulnerabilityPeriod(1f);
				GameManager.Instance.AllPlayers[i].knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			}
		}
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Tentacleport");
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(p.specRigidbody.UnitBottomCenter + new Vector2(0f, -1f), tk2dBaseSprite.Anchor.LowerCenter);
			gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
			gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		}
	}

	// Token: 0x060076F7 RID: 30455 RVA: 0x002F7100 File Offset: 0x002F5300
	protected override void DoEffect(PlayerController user)
	{
		this.m_numberOfUses++;
		this.CheckRitual(user, this.m_numberOfUses >= this.NumberOfUsesToOpen);
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.LAMENT_CONFIGURUM_USES, 1f);
		if (this.m_numberOfUses >= this.NumberOfUsesToOpen)
		{
			user.PlayEffectOnActor(this.OpenVFX, new Vector3(0.03125f, 1.5f, 0f), true, false, false);
			PickupObject pickupObject = this.Open(user);
			this.NumberOfUsesToOpen += this.NumUsesIncreasePerUsage;
			this.m_numberOfUses = 0;
			if (this.CurseIncreasePerItem > 0)
			{
				StatModifier statModifier = new StatModifier();
				statModifier.statToBoost = PlayerStats.StatType.Curse;
				statModifier.amount = (float)this.CurseIncreasePerItem;
				statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				if (pickupObject)
				{
					if (pickupObject is Gun)
					{
						Gun gun = pickupObject as Gun;
						Array.Resize<StatModifier>(ref gun.passiveStatModifiers, gun.passiveStatModifiers.Length + 1);
						gun.passiveStatModifiers[gun.passiveStatModifiers.Length - 1] = statModifier;
					}
					else if (pickupObject is PassiveItem)
					{
						PassiveItem passiveItem = pickupObject as PassiveItem;
						Array.Resize<StatModifier>(ref passiveItem.passiveStatModifiers, passiveItem.passiveStatModifiers.Length + 1);
						passiveItem.passiveStatModifiers[passiveItem.passiveStatModifiers.Length - 1] = statModifier;
					}
					else if (pickupObject is PlayerItem)
					{
						PlayerItem playerItem = pickupObject as PlayerItem;
						Array.Resize<StatModifier>(ref playerItem.passiveStatModifiers, playerItem.passiveStatModifiers.Length + 1);
						playerItem.passiveStatModifiers[playerItem.passiveStatModifiers.Length - 1] = statModifier;
					}
				}
				else
				{
					user.ownerlessStatModifiers.Add(statModifier);
					user.stats.RecalculateStats(user, false, false);
				}
			}
			this.DemonicRitualChance += this.RitualChanceIncreasePerUsage;
		}
		else
		{
			if (this.CurseIncreasePerAttempt > 0 && UnityEngine.Random.value < this.ChanceToIncreaseCursePerAttempt)
			{
				StatModifier statModifier2 = new StatModifier();
				statModifier2.statToBoost = PlayerStats.StatType.Curse;
				statModifier2.amount = (float)this.CurseIncreasePerAttempt;
				statModifier2.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(statModifier2);
				user.stats.RecalculateStats(user, false, false);
			}
			user.PlayEffectOnActor(this.UseVFX, new Vector3(0.03125f, 1.53125f, 0f), true, false, false);
		}
	}

	// Token: 0x060076F8 RID: 30456 RVA: 0x002F7348 File Offset: 0x002F5548
	private IEnumerator TimedKill(AIActor targetActor)
	{
		yield return new WaitForSeconds(60f);
		if (targetActor && targetActor.healthHaver && targetActor.healthHaver.IsAlive)
		{
			targetActor.EraseFromExistence(false);
		}
		yield break;
	}

	// Token: 0x060076F9 RID: 30457 RVA: 0x002F7364 File Offset: 0x002F5564
	private void DoDamageIfIShould(PlayerController user)
	{
		if (user.HasActiveBonusSynergy(CustomSynergyType.HEART_SHAPED_BOX, false))
		{
			AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
			user.healthHaver.ApplyHealing(0.5f);
		}
		else if (UnityEngine.Random.value < this.ChanceToDamagePlayerOnSuccess)
		{
			this.AmountToDamagePlayer += 0.5f;
			user.healthHaver.ApplyDamage(this.AmountToDamagePlayer, Vector2.zero, StringTableManager.GetItemsString("#LAMENTBOX_ENCNAME", -1), CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
		}
	}

	// Token: 0x060076FA RID: 30458 RVA: 0x002F73F0 File Offset: 0x002F55F0
	private void CheckRitual(PlayerController user, bool shouldOpen)
	{
		if ((shouldOpen || this.ShouldUseRitualEveryUse) && UnityEngine.Random.value < this.DemonicRitualChance)
		{
			bool flag = !user.CurrentRoom.IsSealed;
			FloodFillUtility.PreprocessContiguousCells(this.LastOwner.CurrentRoom, this.LastOwner.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), 0);
			IntVector2? targetCenter = new IntVector2?(user.CenterPosition.ToIntVector2(VectorConversions.Floor));
			int num = 0;
			this.NumEnemiesToSpawn = UnityEngine.Random.Range(2f, this.MaxEnemiesToSpawn);
			int num2 = 0;
			while ((float)num2 < this.NumEnemiesToSpawn)
			{
				string text = this.DevilEnemyGuid;
				if (this.AdditionalEnemyGuids.Length > 0)
				{
					int num3 = UnityEngine.Random.Range(-1, this.AdditionalEnemyGuids.Length);
					if (num3 >= 0)
					{
						text = this.AdditionalEnemyGuids[num3];
					}
				}
				AIActor enemyPrefab = EnemyDatabase.GetOrLoadByGuid(text);
				bool checkContiguous = true;
				CellValidator cellValidator = delegate(IntVector2 c)
				{
					if (checkContiguous && !FloodFillUtility.WasFilled(c))
					{
						return false;
					}
					for (int j = 0; j < enemyPrefab.Clearance.x; j++)
					{
						for (int k = 0; k < enemyPrefab.Clearance.y; k++)
						{
							if (GameManager.Instance.Dungeon.data.isTopWall(c.x + j, c.y + k))
							{
								return false;
							}
							if (targetCenter != null)
							{
								if (IntVector2.Distance(targetCenter.Value, c.x + j, c.y + k) < 4f)
								{
									return false;
								}
								if (IntVector2.Distance(targetCenter.Value, c.x + j, c.y + k) > 20f)
								{
									return false;
								}
							}
						}
					}
					return true;
				};
				checkContiguous = true;
				IntVector2? intVector = user.CurrentRoom.GetRandomAvailableCell(new IntVector2?(enemyPrefab.Clearance), new CellTypes?(enemyPrefab.PathableTiles), false, cellValidator);
				if (intVector == null)
				{
					checkContiguous = false;
					intVector = user.CurrentRoom.GetRandomAvailableCell(new IntVector2?(enemyPrefab.Clearance), new CellTypes?(enemyPrefab.PathableTiles), false, cellValidator);
				}
				if (intVector != null)
				{
					AIActor aiactor = AIActor.Spawn(enemyPrefab, intVector.Value, user.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
					aiactor.StartCoroutine(this.TimedKill(aiactor));
					num++;
					aiactor.HandleReinforcementFallIntoRoom(0f);
				}
				num2++;
			}
			if (num > 0)
			{
				if (user.CurrentRoom.area.runtimePrototypeData != null)
				{
					bool flag2 = false;
					for (int i = 0; i < user.CurrentRoom.area.runtimePrototypeData.roomEvents.Count; i++)
					{
						RoomEventDefinition roomEventDefinition = user.CurrentRoom.area.runtimePrototypeData.roomEvents[i];
						if (roomEventDefinition.condition == RoomEventTriggerCondition.ON_ENEMIES_CLEARED && roomEventDefinition.action == RoomEventTriggerAction.UNSEAL_ROOM)
						{
							flag2 = true;
						}
					}
					if (!flag2)
					{
						user.CurrentRoom.area.runtimePrototypeData.roomEvents.Add(new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM));
					}
				}
				if (flag)
				{
					user.CurrentRoom.PreventStandardRoomReward = true;
				}
				user.CurrentRoom.SealRoom();
			}
		}
	}

	// Token: 0x060076FB RID: 30459 RVA: 0x002F76AC File Offset: 0x002F58AC
	private PickupObject Open(PlayerController user)
	{
		DebrisObject debrisObject = GameManager.Instance.RewardManager.SpawnTotallyRandomItem(user.CenterPosition, PickupObject.ItemQuality.B, PickupObject.ItemQuality.S);
		this.DoDamageIfIShould(user);
		if (debrisObject)
		{
			Vector2 vector = ((!debrisObject.sprite) ? (debrisObject.transform.position.XY() + new Vector2(0.5f, 0.5f)) : debrisObject.sprite.WorldCenter);
			GameObject gameObject = SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_BlackPhantomDeath", ".prefab"), vector, Quaternion.identity, false);
			if (gameObject && gameObject.GetComponent<tk2dSprite>())
			{
				tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
				component.HeightOffGround = 5f;
				component.UpdateZDepth();
			}
			return debrisObject.GetComponentInChildren<PickupObject>();
		}
		return null;
	}

	// Token: 0x060076FC RID: 30460 RVA: 0x002F778C File Offset: 0x002F598C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040078EF RID: 30959
	public int NumberOfUsesToOpen = 3;

	// Token: 0x040078F0 RID: 30960
	public int NumUsesIncreasePerUsage = 1;

	// Token: 0x040078F1 RID: 30961
	public GameObject UseVFX;

	// Token: 0x040078F2 RID: 30962
	public GameObject OpenVFX;

	// Token: 0x040078F3 RID: 30963
	public bool ShouldUseRitualEveryUse;

	// Token: 0x040078F4 RID: 30964
	public float DemonicRitualChance = 0.05f;

	// Token: 0x040078F5 RID: 30965
	public float HurtPlayerChance = 0.2f;

	// Token: 0x040078F6 RID: 30966
	public float AmountToDamagePlayer = 0.5f;

	// Token: 0x040078F7 RID: 30967
	public float RitualChanceIncreasePerUsage = 0.05f;

	// Token: 0x040078F8 RID: 30968
	public float MaxEnemiesToSpawn = 5f;

	// Token: 0x040078F9 RID: 30969
	private float NumEnemiesToSpawn = 3f;

	// Token: 0x040078FA RID: 30970
	public float ChanceToIncreaseCursePerAttempt = 0.5f;

	// Token: 0x040078FB RID: 30971
	public int CurseIncreasePerAttempt;

	// Token: 0x040078FC RID: 30972
	public float ChanceToDamagePlayerOnSuccess = 0.2f;

	// Token: 0x040078FD RID: 30973
	public int CurseIncreasePerItem = 1;

	// Token: 0x040078FE RID: 30974
	public float ChanceToEyeball = 0.001f;

	// Token: 0x040078FF RID: 30975
	[EnemyIdentifier]
	public string DevilEnemyGuid;

	// Token: 0x04007900 RID: 30976
	[EnemyIdentifier]
	public string[] AdditionalEnemyGuids;

	// Token: 0x04007901 RID: 30977
	private int m_numberOfUses;
}
