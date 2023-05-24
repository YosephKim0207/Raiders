using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001347 RID: 4935
public class ArtfulDodgerGunController : MonoBehaviour
{
	// Token: 0x06006FE9 RID: 28649 RVA: 0x002C5C94 File Offset: 0x002C3E94
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.NotifyFired));
	}

	// Token: 0x06006FEA RID: 28650 RVA: 0x002C5CCC File Offset: 0x002C3ECC
	private void NotifyFired(Projectile obj)
	{
		this.m_lastProjectile = obj;
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.WINCHESTER_SHOTS_FIRED, 1f);
	}

	// Token: 0x06006FEB RID: 28651 RVA: 0x002C5CE8 File Offset: 0x002C3EE8
	private void Start()
	{
		this.m_startRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		this.m_gun.DoubleWideLaserSight = true;
		List<ArtfulDodgerRoomController> componentsAbsoluteInRoom = this.m_startRoom.GetComponentsAbsoluteInRoom<ArtfulDodgerRoomController>();
		if (componentsAbsoluteInRoom != null && componentsAbsoluteInRoom.Count > 0)
		{
			componentsAbsoluteInRoom[0].gamePlayingPlayer = this.m_gun.CurrentOwner as PlayerController;
		}
	}

	// Token: 0x06006FEC RID: 28652 RVA: 0x002C5D68 File Offset: 0x002C3F68
	private IEnumerator HandleDelayedReward()
	{
		float elapsed = 5f;
		while (this.m_lastProjectile && elapsed > 0f)
		{
			elapsed -= BraveTime.DeltaTime;
			yield return null;
		}
		this.m_startRoom.GetComponentsAbsoluteInRoom<ArtfulDodgerRoomController>()[0].DoHandleReward();
		yield break;
	}

	// Token: 0x06006FED RID: 28653 RVA: 0x002C5D84 File Offset: 0x002C3F84
	private void Update()
	{
		if (this.m_gun && this.m_gun.CurrentOwner)
		{
			(this.m_gun.CurrentOwner as PlayerController).HighAccuracyAimMode = true;
		}
		if (this.m_gun.ammo == 0)
		{
			if (this.m_gun.CurrentOwner is PlayerController)
			{
				PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
				playerController.HighAccuracyAimMode = false;
				playerController.SuppressThisClick = true;
				playerController.inventory.DestroyGun(this.m_gun);
				playerController.StartCoroutine(this.HandleDelayedReward());
			}
		}
		else if (this.m_gun.CurrentOwner != null && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController2 = this.m_gun.CurrentOwner as PlayerController;
			if (playerController2.CurrentRoom != this.m_startRoom)
			{
				playerController2.HighAccuracyAimMode = false;
				playerController2.SuppressThisClick = true;
				playerController2.inventory.DestroyGun(this.m_gun);
				playerController2.StartCoroutine(this.HandleDelayedReward());
			}
		}
	}

	// Token: 0x04006F3E RID: 28478
	private Gun m_gun;

	// Token: 0x04006F3F RID: 28479
	private Projectile m_lastProjectile;

	// Token: 0x04006F40 RID: 28480
	private RoomHandler m_startRoom;
}
