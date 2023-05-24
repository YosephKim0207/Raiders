using System;
using UnityEngine;

// Token: 0x02001433 RID: 5171
public class LockpicksItem : PlayerItem
{
	// Token: 0x06007561 RID: 30049 RVA: 0x002EC018 File Offset: 0x002EA218
	public override bool CanBeUsed(PlayerController user)
	{
		if (!user || user.CurrentRoom == null)
		{
			return false;
		}
		if (!this.m_isTransformed && user.HasActiveBonusSynergy(CustomSynergyType.MASTER_OF_UNLOCKING, false))
		{
			this.m_isTransformed = true;
			base.sprite.SetSprite("lockpicks_upgrade_001");
		}
		else if (this.m_isTransformed && !user.HasActiveBonusSynergy(CustomSynergyType.MASTER_OF_UNLOCKING, false))
		{
			this.m_isTransformed = false;
			base.sprite.SetSprite("lockpicks_001");
		}
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		if (nearestInteractable is InteractableLock || nearestInteractable is Chest || nearestInteractable is DungeonDoorController)
		{
			if (nearestInteractable is InteractableLock)
			{
				InteractableLock interactableLock = nearestInteractable as InteractableLock;
				if (interactableLock && !interactableLock.IsBusted && interactableLock.transform.position.GetAbsoluteRoom() == user.CurrentRoom && interactableLock.IsLocked && !interactableLock.HasBeenPicked && interactableLock.lockMode == InteractableLock.InteractableLockMode.NORMAL)
				{
					return base.CanBeUsed(user);
				}
			}
			else if (nearestInteractable is DungeonDoorController)
			{
				DungeonDoorController dungeonDoorController = nearestInteractable as DungeonDoorController;
				if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked && !dungeonDoorController.lockIsBusted)
				{
					return base.CanBeUsed(user);
				}
			}
			else if (nearestInteractable is Chest)
			{
				Chest chest = nearestInteractable as Chest;
				return chest && chest.GetAbsoluteParentRoom() == user.CurrentRoom && chest.IsLocked && !chest.IsLockBroken && base.CanBeUsed(user);
			}
		}
		return false;
	}

	// Token: 0x06007562 RID: 30050 RVA: 0x002EC1F0 File Offset: 0x002EA3F0
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		float num = this.ChanceToUnlock;
		if (user && user.HasActiveBonusSynergy(CustomSynergyType.MASTER_OF_UNLOCKING, false))
		{
			num = this.MasterOfUnlocking_ChanceToUnlock;
		}
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		if (nearestInteractable is InteractableLock || nearestInteractable is Chest || nearestInteractable is DungeonDoorController)
		{
			if (nearestInteractable is InteractableLock)
			{
				InteractableLock interactableLock = nearestInteractable as InteractableLock;
				if (interactableLock.lockMode == InteractableLock.InteractableLockMode.NORMAL)
				{
					interactableLock.HasBeenPicked = true;
					if (UnityEngine.Random.value < num)
					{
						AkSoundEngine.PostEvent("Play_OBJ_lock_pick_01", base.gameObject);
						interactableLock.ForceUnlock();
					}
					else
					{
						AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", base.gameObject);
						interactableLock.BreakLock();
					}
				}
				return;
			}
			if (nearestInteractable is DungeonDoorController)
			{
				DungeonDoorController dungeonDoorController = nearestInteractable as DungeonDoorController;
				if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked)
				{
					if (UnityEngine.Random.value < num)
					{
						AkSoundEngine.PostEvent("Play_OBJ_lock_pick_01", base.gameObject);
						dungeonDoorController.Unlock();
					}
					else
					{
						AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", base.gameObject);
						dungeonDoorController.BreakLock();
					}
				}
			}
			else if (nearestInteractable is Chest)
			{
				Chest chest = nearestInteractable as Chest;
				if (chest.IsLocked)
				{
					if (!chest.IsLockBroken)
					{
						if (UnityEngine.Random.value < num)
						{
							AkSoundEngine.PostEvent("Play_OBJ_lock_pick_01", base.gameObject);
							chest.ForceUnlock();
							return;
						}
						AkSoundEngine.PostEvent("Play_WPN_gun_empty_01", base.gameObject);
						chest.BreakLock();
						return;
					}
				}
			}
		}
	}

	// Token: 0x04007746 RID: 30534
	public float ChanceToUnlock = 0.2f;

	// Token: 0x04007747 RID: 30535
	public float MasterOfUnlocking_ChanceToUnlock = 0.8f;

	// Token: 0x04007748 RID: 30536
	private bool m_isTransformed;
}
