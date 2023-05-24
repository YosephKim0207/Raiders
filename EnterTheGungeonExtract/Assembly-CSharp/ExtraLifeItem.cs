using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020013FF RID: 5119
public class ExtraLifeItem : PassiveItem
{
	// Token: 0x06007431 RID: 29745 RVA: 0x002E3A30 File Offset: 0x002E1C30
	public static void ClearPerLevelData()
	{
		ExtraLifeItem.s_bonfiredRooms.Clear();
		ExtraLifeItem.LastActivatedBonfire = null;
	}

	// Token: 0x06007432 RID: 29746 RVA: 0x002E3A44 File Offset: 0x002E1C44
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		player.healthHaver.OnPreDeath += this.HandlePreDeath;
		base.Pickup(player);
	}

	// Token: 0x06007433 RID: 29747 RVA: 0x002E3A70 File Offset: 0x002E1C70
	protected override void Update()
	{
		base.Update();
		if (this.DoesBonfireSynergy && this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.THE_REAL_DARK_SOULS, false) && this.m_owner.CurrentRoom != null && !ExtraLifeItem.s_bonfiredRooms.Contains(this.m_owner.CurrentRoom) && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
		{
			RoomHandler currentRoom = this.m_owner.CurrentRoom;
			bool flag = currentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL && currentRoom.area.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
			flag |= currentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.EXIT;
			if (flag)
			{
				bool flag2 = false;
				IntVector2 centeredVisibleClearSpot = currentRoom.GetCenteredVisibleClearSpot(4, 4, out flag2, false);
				if (flag2)
				{
					ExtraLifeItem.LastActivatedBonfire = UnityEngine.Object.Instantiate<GameObject>(this.BonfireSynergyBonfire, (centeredVisibleClearSpot + new IntVector2(1, 1)).ToVector2().ToVector3ZisY(0f), Quaternion.identity);
					LootEngine.DoDefaultSynergyPoof(centeredVisibleClearSpot.ToVector2() + new Vector2(2f, 2f), false);
					PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(ExtraLifeItem.LastActivatedBonfire.GetComponent<SpeculativeRigidbody>(), null, false);
				}
				ExtraLifeItem.s_bonfiredRooms.Add(currentRoom);
			}
		}
	}

	// Token: 0x06007434 RID: 29748 RVA: 0x002E3BC8 File Offset: 0x002E1DC8
	private void HandlePreDeath(Vector2 damageDirection)
	{
		if (this.m_owner)
		{
			if (this.m_owner.IsInMinecart)
			{
				this.m_owner.currentMineCart.EvacuateSpecificPlayer(this.m_owner, true);
			}
			for (int i = 0; i < this.m_owner.passiveItems.Count; i++)
			{
				if (this.m_owner.passiveItems[i] is CompanionItem && this.m_owner.passiveItems[i].DisplayName == "Pig")
				{
					return;
				}
				if (this.m_owner.passiveItems[i] is ExtraLifeItem && this.extraLifeMode != ExtraLifeItem.ExtraLifeMode.DARK_SOULS)
				{
					ExtraLifeItem extraLifeItem = this.m_owner.passiveItems[i] as ExtraLifeItem;
					if (extraLifeItem.extraLifeMode == ExtraLifeItem.ExtraLifeMode.DARK_SOULS)
					{
						return;
					}
				}
			}
		}
		if (this.m_owner.IsInMinecart)
		{
			this.m_owner.currentMineCart.EvacuateSpecificPlayer(this.m_owner, true);
		}
		ExtraLifeItem.ExtraLifeMode extraLifeMode = this.extraLifeMode;
		if (extraLifeMode != ExtraLifeItem.ExtraLifeMode.ESCAPE_ROPE)
		{
			if (extraLifeMode != ExtraLifeItem.ExtraLifeMode.DARK_SOULS)
			{
				if (extraLifeMode == ExtraLifeItem.ExtraLifeMode.CLONE)
				{
					this.HandleCloneStyle();
					return;
				}
			}
			else
			{
				this.HandleDarkSoulsStyle();
			}
		}
		else
		{
			this.HandleEscapeRopeStyle();
		}
		if (this.consumedOnUse)
		{
			this.m_owner.RemovePassiveItem(this.PickupObjectId);
		}
	}

	// Token: 0x06007435 RID: 29749 RVA: 0x002E3D3C File Offset: 0x002E1F3C
	private void HandleEscapeRopeStyle()
	{
		this.m_owner.healthHaver.FullHeal();
		this.m_owner.EscapeRoom(PlayerController.EscapeSealedRoomStyle.NONE, true, null);
	}

	// Token: 0x06007436 RID: 29750 RVA: 0x002E3D5C File Offset: 0x002E1F5C
	private void HandleCloneStyle()
	{
		this.m_owner.HandleCloneItem(this);
	}

	// Token: 0x06007437 RID: 29751 RVA: 0x002E3D6C File Offset: 0x002E1F6C
	private void HandleDarkSoulsStyle()
	{
		this.m_owner.TriggerDarkSoulsReset(this.DropDarkSoulsItems, this.DarkSoulsCursedHealthMax);
	}

	// Token: 0x06007438 RID: 29752 RVA: 0x002E3D88 File Offset: 0x002E1F88
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.healthHaver.OnPreDeath -= this.HandlePreDeath;
		debrisObject.GetComponent<ExtraLifeItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007439 RID: 29753 RVA: 0x002E3DC4 File Offset: 0x002E1FC4
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			this.m_owner.healthHaver.OnPreDeath -= this.HandlePreDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x040075CB RID: 30155
	public static GameObject LastActivatedBonfire;

	// Token: 0x040075CC RID: 30156
	private static List<RoomHandler> s_bonfiredRooms = new List<RoomHandler>();

	// Token: 0x040075CD RID: 30157
	public ExtraLifeItem.ExtraLifeMode extraLifeMode;

	// Token: 0x040075CE RID: 30158
	public bool consumedOnUse = true;

	// Token: 0x040075CF RID: 30159
	[ShowInInspectorIf("extraLifeMode", 1, false)]
	public bool DropDarkSoulsItems;

	// Token: 0x040075D0 RID: 30160
	[ShowInInspectorIf("extraLifeMode", 1, false)]
	public int DarkSoulsCursedHealthMax = -1;

	// Token: 0x040075D1 RID: 30161
	[PickupIdentifier]
	public int[] ExcludedPickupIDs;

	// Token: 0x040075D2 RID: 30162
	public bool DoesBonfireSynergy;

	// Token: 0x040075D3 RID: 30163
	public GameObject BonfireSynergyBonfire;

	// Token: 0x02001400 RID: 5120
	public enum ExtraLifeMode
	{
		// Token: 0x040075D5 RID: 30165
		ESCAPE_ROPE,
		// Token: 0x040075D6 RID: 30166
		DARK_SOULS,
		// Token: 0x040075D7 RID: 30167
		CLONE
	}
}
