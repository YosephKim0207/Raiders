using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001144 RID: 4420
public class DemonFaceController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x060061E8 RID: 25064 RVA: 0x0025EEA8 File Offset: 0x0025D0A8
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTrigger));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody2.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.HandleTriggerExit));
		SpeculativeRigidbody speculativeRigidbody = this.interiorRigidbody;
		speculativeRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody speculativeRigidbody2 = this.interiorRigidbody;
		speculativeRigidbody2.OnHitByBeam = (Action<BasicBeamController>)Delegate.Combine(speculativeRigidbody2.OnHitByBeam, new Action<BasicBeamController>(this.HandleBeam));
	}

	// Token: 0x060061E9 RID: 25065 RVA: 0x0025EF54 File Offset: 0x0025D154
	private void HandleRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.projectile)
		{
			otherRigidbody.projectile.ForceDestruction();
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x060061EA RID: 25066 RVA: 0x0025EF78 File Offset: 0x0025D178
	private void HandleTriggerExit(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		if (specRigidbody)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component)
			{
				this.m_containedPlayers.Remove(component);
			}
		}
	}

	// Token: 0x060061EB RID: 25067 RVA: 0x0025EFB0 File Offset: 0x0025D1B0
	private void HandleBeam(BasicBeamController obj)
	{
		if (obj.projectile && (obj.projectile.damageTypes | CoreDamageTypes.Water) == obj.projectile.damageTypes)
		{
			this.HitWithWater();
		}
	}

	// Token: 0x060061EC RID: 25068 RVA: 0x0025EFE8 File Offset: 0x0025D1E8
	private void HandleTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component)
		{
			this.m_containedPlayers.Add(component);
			bool flag = false;
			if (component.carriedConsumables.Currency >= this.RequiredCurrency)
			{
				flag = true;
			}
			if ((float)PlayerStats.GetTotalCurse() >= this.RequiredCurse)
			{
				flag = true;
			}
			if (flag)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleEjection(component, true));
			}
			else
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleEjection(component, false));
			}
		}
		else
		{
			Projectile projectile = specRigidbody.projectile;
			if (projectile && (projectile.damageTypes | CoreDamageTypes.Water) == projectile.damageTypes)
			{
				this.HitWithWater();
			}
		}
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x0025F0AC File Offset: 0x0025D2AC
	private void WarpToBlackMarket(PlayerController triggerPlayer)
	{
		GameManager.Instance.platformInterface.AchievementUnlock(Achievement.REACH_BLACK_MARKET, 0);
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
			if (roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL && roomHandler.area.PrototypeRoomName == "Black Market")
			{
				triggerPlayer.AttemptTeleportToRoom(roomHandler, true, true);
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					GameManager.Instance.GetOtherPlayer(triggerPlayer).AttemptTeleportToRoom(roomHandler, true, false);
				}
			}
		}
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x0025F168 File Offset: 0x0025D368
	private IEnumerator HandleEjection(PlayerController triggerPlayer, bool success)
	{
		triggerPlayer.SetInputOverride("demon face");
		triggerPlayer.ForceStaticFaceDirection(Vector2.up);
		bool playerHasYellowChamber = false;
		for (int i = 0; i < triggerPlayer.passiveItems.Count; i++)
		{
			if (triggerPlayer.passiveItems[i] is YellowChamberItem)
			{
				playerHasYellowChamber = true;
				break;
			}
		}
		if (playerHasYellowChamber)
		{
			this.interiorRigidbody.spriteAnimator.Play();
			yield return new WaitForSeconds(1.75f);
		}
		triggerPlayer.FlatColorOverridden = true;
		triggerPlayer.ToggleGunRenderers(false, "face");
		triggerPlayer.ToggleHandRenderers(false, "face");
		triggerPlayer.ForceMoveInDirectionUntilThreshold(Vector2.up, triggerPlayer.CenterPosition.y + 1f, 0f, 2f, null);
		float ela = 0f;
		if (!triggerPlayer.IsDodgeRolling)
		{
			while (ela < 1f)
			{
				ela += BraveTime.DeltaTime;
				triggerPlayer.ChangeFlatColorOverride(new Color(0f, 0f, 0f, ela));
				yield return null;
			}
		}
		else
		{
			ela = 1f;
			triggerPlayer.ForceStopDodgeRoll();
			triggerPlayer.ChangeFlatColorOverride(new Color(0f, 0f, 0f, 1f));
		}
		triggerPlayer.ToggleGunRenderers(false, "face");
		triggerPlayer.ToggleHandRenderers(false, "face");
		while (ela < 1.5f)
		{
			ela += BraveTime.DeltaTime;
			yield return null;
		}
		if (success && !false)
		{
			this.WarpToBlackMarket(triggerPlayer);
			yield return new WaitForSeconds(0.625f);
		}
		triggerPlayer.ToggleGunRenderers(true, string.Empty);
		triggerPlayer.ToggleHandRenderers(true, string.Empty);
		triggerPlayer.usingForcedInput = false;
		triggerPlayer.FlatColorOverridden = false;
		triggerPlayer.ChangeFlatColorOverride(new Color(0f, 0f, 0f, 0f));
		triggerPlayer.ClearInputOverride("demon face");
		if (!success)
		{
			if (this.m_containedPlayers.Contains(triggerPlayer))
			{
				triggerPlayer.healthHaver.ApplyDamage(0.5f, Vector2.down, StringTableManager.GetItemsString("#DEMONFACE", -1), CoreDamageTypes.None, DamageCategory.Environment, false, null, false);
				triggerPlayer.knockbackDoer.ApplyKnockback(Vector2.down, 80f, false);
			}
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x0025F194 File Offset: 0x0025D394
	private void HitWithWater()
	{
		if (!this.m_hasDrunkDeeplyFromTheSweetSweetWaters)
		{
			this.m_hasDrunkDeeplyFromTheSweetSweetWaters = true;
			GameManager.Instance.Dungeon.StartCoroutine(this.ProvideDumbReward());
		}
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x0025F1C0 File Offset: 0x0025D3C0
	private IEnumerator ProvideDumbReward()
	{
		yield return new WaitForSeconds(0.5f);
		PickupObject prefabItem = this.WaterRewardTable.GetSingleItemForPlayer(GameManager.Instance.PrimaryPlayer, 0);
		if (prefabItem != null)
		{
			float x = prefabItem.GetComponent<tk2dBaseSprite>().GetBounds().center.x;
			DebrisObject debrisObject = LootEngine.SpawnItem(prefabItem.gameObject, base.specRigidbody.PixelColliders[base.specRigidbody.PixelColliders.Count - 1].UnitCenter + new Vector2(-x, 0f), Vector2.down, 4f, true, false, false);
			if (debrisObject.specRigidbody)
			{
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(debrisObject.specRigidbody, null, false);
			}
		}
		yield break;
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x0025F1DC File Offset: 0x0025D3DC
	public void ConfigureOnPlacement(RoomHandler room)
	{
		room.OptionalDoorTopDecorable = ResourceCache.Acquire("Global Prefabs/Purple_Lantern") as GameObject;
	}

	// Token: 0x04005CD1 RID: 23761
	public SpeculativeRigidbody interiorRigidbody;

	// Token: 0x04005CD2 RID: 23762
	public LootData WaterRewardTable;

	// Token: 0x04005CD3 RID: 23763
	private List<PlayerController> m_containedPlayers = new List<PlayerController>();

	// Token: 0x04005CD4 RID: 23764
	public int RequiredCurrency = 100;

	// Token: 0x04005CD5 RID: 23765
	public float RequiredCurse = 0.01f;

	// Token: 0x04005CD6 RID: 23766
	private bool m_hasDrunkDeeplyFromTheSweetSweetWaters;
}
