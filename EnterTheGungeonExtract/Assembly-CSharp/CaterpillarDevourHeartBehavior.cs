using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DFE RID: 3582
public class CaterpillarDevourHeartBehavior : OverrideBehaviorBase
{
	// Token: 0x06004BD4 RID: 19412 RVA: 0x0019DCAC File Offset: 0x0019BEAC
	public override void Start()
	{
		base.Start();
		this.m_cachedSpeed = this.m_aiActor.MovementSpeed;
		this.m_heartsMunched = 0;
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x0019DCCC File Offset: 0x0019BECC
	private bool IsHeartInRoom()
	{
		PlayerController companionOwner = this.m_aiActor.CompanionOwner;
		if (!companionOwner || companionOwner.CurrentRoom == null)
		{
			return false;
		}
		List<HealthPickup> componentsAbsoluteInRoom = companionOwner.CurrentRoom.GetComponentsAbsoluteInRoom<HealthPickup>();
		for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
		{
			HealthPickup healthPickup = componentsAbsoluteInRoom[i];
			if (healthPickup)
			{
				if (healthPickup.armorAmount == 0)
				{
					componentsAbsoluteInRoom.RemoveAt(i);
					i--;
				}
			}
		}
		HealthPickup closestToPosition = BraveUtility.GetClosestToPosition<HealthPickup>(componentsAbsoluteInRoom, this.m_aiActor.CenterPosition, new HealthPickup[0]);
		if (closestToPosition != null)
		{
			this.m_targetHeart = closestToPosition;
			return true;
		}
		return false;
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x0019DD80 File Offset: 0x0019BF80
	private void MunchHeart(PickupObject targetHeart)
	{
		UnityEngine.Object.Destroy(targetHeart.gameObject);
		this.m_heartsMunched++;
		this.m_aiAnimator.PlayUntilFinished("munch", false, null, -1f, false);
		if (this.m_heartsMunched >= this.RequiredHearts)
		{
			this.DoTransformation();
		}
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x0019DDD8 File Offset: 0x0019BFD8
	private void DoTransformation()
	{
		if (this.m_aiActor.CompanionOwner != null)
		{
			if (this.TransformVFX)
			{
				SpawnManager.SpawnVFX(this.TransformVFX, this.m_aiActor.sprite.WorldBottomCenter, Quaternion.identity);
			}
			GameManager.Instance.StartCoroutine(this.DelayedGiveItem(this.m_aiActor.CompanionOwner));
			if (this.SourceCompanionItemId >= 0)
			{
				this.m_aiActor.CompanionOwner.RemovePassiveItem(this.SourceCompanionItemId);
			}
		}
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x0019DE70 File Offset: 0x0019C070
	private IEnumerator DelayedGiveItem(PlayerController targetPlayer)
	{
		yield return new WaitForSeconds(3.375f);
		if (targetPlayer && !targetPlayer.IsGhost)
		{
			PickupObject byId = PickupObjectDatabase.GetById(this.WingsItemIdToGive);
			if (byId != null)
			{
				LootEngine.GivePrefabToPlayer(byId.gameObject, targetPlayer);
			}
		}
		yield break;
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x0019DE94 File Offset: 0x0019C094
	public override void Upkeep()
	{
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.Upkeep();
	}

	// Token: 0x06004BDA RID: 19418 RVA: 0x0019DEAC File Offset: 0x0019C0AC
	public override BehaviorResult Update()
	{
		if (!this.m_targetHeart)
		{
			this.m_aiActor.MovementSpeed = this.m_cachedSpeed;
			this.m_targetHeart = null;
			if (this.m_repathTimer <= 0f)
			{
				this.m_repathTimer = 1f;
				if (this.IsHeartInRoom())
				{
					return BehaviorResult.SkipAllRemainingBehaviors;
				}
			}
			return base.Update();
		}
		this.m_aiActor.PathfindToPosition(this.m_targetHeart.sprite.WorldCenter, new Vector2?(this.m_targetHeart.sprite.WorldCenter), true, null, null, null, false);
		if (this.m_aiActor.Path != null && this.m_aiActor.Path.WillReachFinalGoal)
		{
			this.m_aiActor.MovementSpeed = 1f;
			if (Vector2.Distance(this.m_targetHeart.sprite.WorldCenter, this.m_aiActor.CenterPosition) < 1.25f)
			{
				this.MunchHeart(this.m_targetHeart);
				this.m_targetHeart = null;
			}
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		if (Vector2.Distance(this.m_targetHeart.sprite.WorldCenter, this.m_aiActor.CenterPosition) < 1.25f)
		{
			this.MunchHeart(this.m_targetHeart);
			this.m_targetHeart = null;
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		return base.Update();
	}

	// Token: 0x040041B4 RID: 16820
	public string MunchAnimName = "munch";

	// Token: 0x040041B5 RID: 16821
	public int RequiredHearts = 3;

	// Token: 0x040041B6 RID: 16822
	public GameObject NoticedHeartVFX;

	// Token: 0x040041B7 RID: 16823
	[PickupIdentifier]
	public int SourceCompanionItemId = -1;

	// Token: 0x040041B8 RID: 16824
	[PickupIdentifier]
	public int WingsItemIdToGive = -1;

	// Token: 0x040041B9 RID: 16825
	public GameObject TransformVFX;

	// Token: 0x040041BA RID: 16826
	[NonSerialized]
	private int m_heartsMunched;

	// Token: 0x040041BB RID: 16827
	private PickupObject m_targetHeart;

	// Token: 0x040041BC RID: 16828
	private float m_repathTimer = 0.25f;

	// Token: 0x040041BD RID: 16829
	private float m_cachedSpeed;
}
