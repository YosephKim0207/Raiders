using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020013A6 RID: 5030
public class ChamberGunProcessor : MonoBehaviour, ILevelLoadedListener
{
	// Token: 0x060071F4 RID: 29172 RVA: 0x002D47D4 File Offset: 0x002D29D4
	private void Awake()
	{
		this.m_currentTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON;
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
	}

	// Token: 0x060071F5 RID: 29173 RVA: 0x002D4810 File Offset: 0x002D2A10
	private GlobalDungeonData.ValidTilesets GetFloorTileset()
	{
		if (GameManager.Instance.IsLoadingLevel || !GameManager.Instance.Dungeon)
		{
			return GlobalDungeonData.ValidTilesets.CASTLEGEON;
		}
		return GameManager.Instance.Dungeon.tileIndices.tilesetId;
	}

	// Token: 0x060071F6 RID: 29174 RVA: 0x002D484C File Offset: 0x002D2A4C
	private bool IsValidTileset(GlobalDungeonData.ValidTilesets t)
	{
		if (t == this.GetFloorTileset())
		{
			return true;
		}
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (playerController)
		{
			if (t == GlobalDungeonData.ValidTilesets.CASTLEGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Castle))
			{
				return true;
			}
			if (t == GlobalDungeonData.ValidTilesets.GUNGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Gungeon))
			{
				return true;
			}
			if (t == GlobalDungeonData.ValidTilesets.MINEGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Mines))
			{
				return true;
			}
			if (t == GlobalDungeonData.ValidTilesets.CATACOMBGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Catacombs))
			{
				return true;
			}
			if (t == GlobalDungeonData.ValidTilesets.FORGEGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Forge))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060071F7 RID: 29175 RVA: 0x002D4904 File Offset: 0x002D2B04
	private void ChangeToTileset(GlobalDungeonData.ValidTilesets t)
	{
		switch (t)
		{
		case GlobalDungeonData.ValidTilesets.GUNGEON:
			this.ChangeForme(this.GungeonGunID);
			this.m_currentTileset = GlobalDungeonData.ValidTilesets.GUNGEON;
			break;
		case GlobalDungeonData.ValidTilesets.CASTLEGEON:
			this.ChangeForme(this.CastleGunID);
			this.m_currentTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON;
			break;
		default:
			if (t != GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				if (t != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
				{
					if (t != GlobalDungeonData.ValidTilesets.FORGEGEON)
					{
						if (t != GlobalDungeonData.ValidTilesets.HELLGEON)
						{
							if (t != GlobalDungeonData.ValidTilesets.OFFICEGEON)
							{
								if (t != GlobalDungeonData.ValidTilesets.RATGEON)
								{
									this.ChangeForme(this.CastleGunID);
									this.m_currentTileset = this.GetFloorTileset();
								}
								else
								{
									this.ChangeForme(this.RatgeonGunID);
									this.m_currentTileset = GlobalDungeonData.ValidTilesets.RATGEON;
								}
							}
							else
							{
								this.ChangeForme(this.OfficeGunID);
								this.m_currentTileset = GlobalDungeonData.ValidTilesets.OFFICEGEON;
							}
						}
						else
						{
							this.ChangeForme(this.HellGunID);
							this.m_currentTileset = GlobalDungeonData.ValidTilesets.HELLGEON;
						}
					}
					else
					{
						this.ChangeForme(this.ForgeGunID);
						this.m_currentTileset = GlobalDungeonData.ValidTilesets.FORGEGEON;
					}
				}
				else
				{
					this.ChangeForme(this.HollowGunID);
					this.m_currentTileset = GlobalDungeonData.ValidTilesets.CATACOMBGEON;
				}
			}
			else
			{
				this.ChangeForme(this.MinesGunID);
				this.m_currentTileset = GlobalDungeonData.ValidTilesets.MINEGEON;
			}
			break;
		case GlobalDungeonData.ValidTilesets.SEWERGEON:
			this.ChangeForme(this.OublietteGunID);
			this.m_currentTileset = GlobalDungeonData.ValidTilesets.SEWERGEON;
			break;
		case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
			this.ChangeForme(this.AbbeyGunID);
			this.m_currentTileset = GlobalDungeonData.ValidTilesets.CATHEDRALGEON;
			break;
		}
	}

	// Token: 0x060071F8 RID: 29176 RVA: 0x002D4A94 File Offset: 0x002D2C94
	private void ChangeForme(int targetID)
	{
		Gun gun = PickupObjectDatabase.GetById(targetID) as Gun;
		this.m_gun.TransformToTargetGun(gun);
	}

	// Token: 0x060071F9 RID: 29177 RVA: 0x002D4ABC File Offset: 0x002D2CBC
	private void Update()
	{
		if (Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (this.m_gun && (!this.m_gun.CurrentOwner || !this.IsValidTileset(this.m_currentTileset)))
		{
			GlobalDungeonData.ValidTilesets validTilesets = this.GetFloorTileset();
			if (!this.m_gun.CurrentOwner)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.CASTLEGEON;
			}
			if (this.m_currentTileset != validTilesets)
			{
				this.ChangeToTileset(validTilesets);
			}
		}
		this.JustActiveReloaded = false;
	}

	// Token: 0x060071FA RID: 29178 RVA: 0x002D4B54 File Offset: 0x002D2D54
	private GlobalDungeonData.ValidTilesets GetNextTileset(GlobalDungeonData.ValidTilesets inTileset)
	{
		switch (inTileset)
		{
		case GlobalDungeonData.ValidTilesets.GUNGEON:
			return GlobalDungeonData.ValidTilesets.CATHEDRALGEON;
		case GlobalDungeonData.ValidTilesets.CASTLEGEON:
			return GlobalDungeonData.ValidTilesets.SEWERGEON;
		default:
			if (inTileset == GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				return GlobalDungeonData.ValidTilesets.RATGEON;
			}
			if (inTileset == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
			{
				return GlobalDungeonData.ValidTilesets.OFFICEGEON;
			}
			if (inTileset == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				return GlobalDungeonData.ValidTilesets.HELLGEON;
			}
			if (inTileset == GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				return GlobalDungeonData.ValidTilesets.CASTLEGEON;
			}
			if (inTileset == GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{
				return GlobalDungeonData.ValidTilesets.FORGEGEON;
			}
			if (inTileset != GlobalDungeonData.ValidTilesets.RATGEON)
			{
				return GlobalDungeonData.ValidTilesets.CASTLEGEON;
			}
			return GlobalDungeonData.ValidTilesets.CATACOMBGEON;
		case GlobalDungeonData.ValidTilesets.SEWERGEON:
			return GlobalDungeonData.ValidTilesets.GUNGEON;
		case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
			return GlobalDungeonData.ValidTilesets.MINEGEON;
		}
	}

	// Token: 0x060071FB RID: 29179 RVA: 0x002D4BEC File Offset: 0x002D2DEC
	private GlobalDungeonData.ValidTilesets GetNextValidTileset()
	{
		GlobalDungeonData.ValidTilesets validTilesets = this.GetNextTileset(this.m_currentTileset);
		while (!this.IsValidTileset(validTilesets))
		{
			validTilesets = this.GetNextTileset(validTilesets);
		}
		return validTilesets;
	}

	// Token: 0x060071FC RID: 29180 RVA: 0x002D4C20 File Offset: 0x002D2E20
	private void HandleReloadPressed(PlayerController ownerPlayer, Gun sourceGun, bool manual)
	{
		if (this.JustActiveReloaded)
		{
			return;
		}
		if (manual && !sourceGun.IsReloading)
		{
			GlobalDungeonData.ValidTilesets nextValidTileset = this.GetNextValidTileset();
			if (this.m_currentTileset != nextValidTileset)
			{
				this.ChangeToTileset(nextValidTileset);
			}
		}
	}

	// Token: 0x060071FD RID: 29181 RVA: 0x002D4C64 File Offset: 0x002D2E64
	public void BraveOnLevelWasLoaded()
	{
		if (this.RefillsOnFloorChange && this.m_gun && this.m_gun.CurrentOwner)
		{
			this.m_gun.StartCoroutine(this.DelayedRegainAmmo());
		}
	}

	// Token: 0x060071FE RID: 29182 RVA: 0x002D4CB4 File Offset: 0x002D2EB4
	private IEnumerator DelayedRegainAmmo()
	{
		yield return null;
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (this.RefillsOnFloorChange && this.m_gun && this.m_gun.CurrentOwner)
		{
			this.m_gun.GainAmmo(this.m_gun.AdjustedMaxAmmo);
		}
		yield break;
	}

	// Token: 0x0400735A RID: 29530
	[PickupIdentifier]
	public int CastleGunID;

	// Token: 0x0400735B RID: 29531
	[PickupIdentifier]
	public int GungeonGunID;

	// Token: 0x0400735C RID: 29532
	[PickupIdentifier]
	public int MinesGunID;

	// Token: 0x0400735D RID: 29533
	[PickupIdentifier]
	public int HollowGunID;

	// Token: 0x0400735E RID: 29534
	[PickupIdentifier]
	public int ForgeGunID;

	// Token: 0x0400735F RID: 29535
	[PickupIdentifier]
	public int HellGunID;

	// Token: 0x04007360 RID: 29536
	[PickupIdentifier]
	public int OublietteGunID;

	// Token: 0x04007361 RID: 29537
	[PickupIdentifier]
	public int AbbeyGunID;

	// Token: 0x04007362 RID: 29538
	[PickupIdentifier]
	public int RatgeonGunID;

	// Token: 0x04007363 RID: 29539
	[PickupIdentifier]
	public int OfficeGunID;

	// Token: 0x04007364 RID: 29540
	public bool RefillsOnFloorChange = true;

	// Token: 0x04007365 RID: 29541
	private GlobalDungeonData.ValidTilesets m_currentTileset;

	// Token: 0x04007366 RID: 29542
	private Gun m_gun;

	// Token: 0x04007367 RID: 29543
	[NonSerialized]
	public bool JustActiveReloaded;
}
