using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020013B6 RID: 5046
public class GungeonEggItem : PlayerItem
{
	// Token: 0x06007254 RID: 29268 RVA: 0x002D7824 File Offset: 0x002D5A24
	protected override void Start()
	{
		base.Start();
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerCollision));
	}

	// Token: 0x06007255 RID: 29269 RVA: 0x002D7854 File Offset: 0x002D5A54
	private bool IsPointOnFire(Vector2 testPos)
	{
		IntVector2 intVector = (testPos / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
		if (DeadlyDeadlyGoopManager.allGoopPositionMap.ContainsKey(intVector))
		{
			DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = DeadlyDeadlyGoopManager.allGoopPositionMap[intVector];
			return deadlyDeadlyGoopManager.IsPositionOnFire(testPos);
		}
		return false;
	}

	// Token: 0x06007256 RID: 29270 RVA: 0x002D7898 File Offset: 0x002D5A98
	private void HatchToDragun()
	{
		this.m_isBroken = true;
		base.spriteAnimator.PlayAndDestroyObject("gungeon_egg_hatch", null);
		base.StartCoroutine(this.HandleDelayedShards());
		this.m_pickedUp = true;
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		GameObject gameObject = this.BabyDragunPlaceable.InstantiateObject(absoluteRoom, base.transform.position.IntXY(VectorConversions.Round) - absoluteRoom.area.basePosition + IntVector2.NegOne, false);
		gameObject.transform.position = base.transform.position + new Vector3(-0.25f, -0.5f, 0f);
		tk2dBaseSprite componentInChildren = gameObject.GetComponentInChildren<tk2dBaseSprite>();
		componentInChildren.UpdateZDepth();
		SpeculativeRigidbody componentInChildren2 = gameObject.GetComponentInChildren<SpeculativeRigidbody>();
		componentInChildren2.Reinitialize();
		DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(base.transform.position.XY() + new Vector2(0.25f, 0.5f), 3f);
	}

	// Token: 0x06007257 RID: 29271 RVA: 0x002D7994 File Offset: 0x002D5B94
	private void HandleTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_isBroken)
		{
			return;
		}
		if (!base.enabled || this.m_pickedUp)
		{
			return;
		}
		bool flag = this.IsPointOnFire(specRigidbody.UnitCenter);
		if (flag)
		{
			this.CanBeSold = false;
			this.HatchToDragun();
		}
		else if (this.m_numberElapsedFloors > 0 && !this.m_isBroken)
		{
			if (specRigidbody && specRigidbody.projectile && specRigidbody.projectile.Owner is PlayerController)
			{
				this.m_isBroken = true;
				this.CreateRewardItem();
				this.m_pickedUp = true;
				this.CanBeSold = false;
				base.spriteAnimator.PlayAndDestroyObject("gungeon_egg_hatch", null);
				base.StartCoroutine(this.HandleDelayedShards());
			}
		}
		else if (this.m_numberElapsedFloors == 0 && !this.m_isBroken && specRigidbody && specRigidbody.projectile && specRigidbody.projectile.Owner is PlayerController)
		{
			this.m_isBroken = true;
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.GudetamaGuid);
			AIActor aiactor = AIActor.Spawn(orLoadByGuid, base.transform.position.XY().ToIntVector2(VectorConversions.Round), base.transform.position.GetAbsoluteRoom(), false, AIActor.AwakenAnimationType.Default, true);
			if (aiactor)
			{
				aiactor.healthHaver.TriggerInvulnerabilityPeriod(0.5f);
				aiactor.PreventAutoKillOnBossDeath = true;
			}
			this.m_pickedUp = true;
			base.spriteAnimator.PlayAndDestroyObject("gungeon_egg_hatch", null);
			base.StartCoroutine(this.HandleDelayedShards());
		}
	}

	// Token: 0x06007258 RID: 29272 RVA: 0x002D7B40 File Offset: 0x002D5D40
	private IEnumerator HandleDelayedShards()
	{
		yield return new WaitForSeconds(0.8f);
		if (this.DoShards)
		{
			this.Shards.HandleShardSpawns(base.sprite.WorldCenter, Vector2.up * 3f);
		}
		yield break;
	}

	// Token: 0x06007259 RID: 29273 RVA: 0x002D7B5C File Offset: 0x002D5D5C
	public override bool CanBeUsed(PlayerController user)
	{
		return user.healthHaver.GetCurrentHealthPercentage() < 1f && base.CanBeUsed(user);
	}

	// Token: 0x0600725A RID: 29274 RVA: 0x002D7B7C File Offset: 0x002D5D7C
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		user.healthHaver.FullHeal();
		user.PlayEffectOnActor(this.HealVFX, Vector3.zero, true, false, false);
		AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", base.gameObject);
	}

	// Token: 0x0600725B RID: 29275 RVA: 0x002D7BB8 File Offset: 0x002D5DB8
	protected void CreateRewardItem()
	{
		PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
		if (this.m_numberElapsedFloors >= 4)
		{
			itemQuality = PickupObject.ItemQuality.S;
			if (this.m_numberElapsedFloors >= 9)
			{
			}
		}
		else
		{
			switch (this.m_numberElapsedFloors)
			{
			case 1:
				itemQuality = PickupObject.ItemQuality.C;
				break;
			case 2:
				itemQuality = PickupObject.ItemQuality.B;
				break;
			case 3:
				itemQuality = PickupObject.ItemQuality.A;
				break;
			}
		}
		PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable, false);
		if (itemOfTypeAndQuality)
		{
			LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, base.transform.position, Vector2.up, 0.1f, true, false, false);
		}
	}

	// Token: 0x0600725C RID: 29276 RVA: 0x002D7C8C File Offset: 0x002D5E8C
	public override void Update()
	{
		base.Update();
		if (!this.m_pickedUp && !this.m_isBroken)
		{
			bool flag = this.IsPointOnFire(base.specRigidbody.UnitCenter);
			if (flag)
			{
				this.m_elapsedInFire += BraveTime.DeltaTime;
				if (this.m_elapsedInFire > this.TimeInFireToHatch)
				{
					this.HatchToDragun();
				}
			}
			else
			{
				this.m_elapsedInFire = 0f;
			}
		}
		if (!base.spriteAnimator.IsPlaying("gungeon_egg_hatch"))
		{
			if (this.m_numberElapsedFloors >= 2 && this.m_numberElapsedFloors < 4 && !base.spriteAnimator.IsPlaying("gungeon_egg_stir_2"))
			{
				base.spriteAnimator.Play("gungeon_egg_stir_2");
			}
			else if (this.m_numberElapsedFloors >= 4 && !base.spriteAnimator.IsPlaying("gungeon_egg_stir_3"))
			{
				base.spriteAnimator.Play("gungeon_egg_stir_3");
			}
		}
	}

	// Token: 0x0600725D RID: 29277 RVA: 0x002D7D90 File Offset: 0x002D5F90
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleLevelLoaded));
	}

	// Token: 0x0600725E RID: 29278 RVA: 0x002D7DBC File Offset: 0x002D5FBC
	private void HandleLevelLoaded(PlayerController source)
	{
		if (!this.m_coroutineActive)
		{
			this.m_coroutineActive = true;
			base.StartCoroutine(this.DelayedProcessing());
		}
	}

	// Token: 0x0600725F RID: 29279 RVA: 0x002D7DE0 File Offset: 0x002D5FE0
	private IEnumerator DelayedProcessing()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		this.m_numberElapsedFloors++;
		yield return new WaitForSeconds(1f);
		this.m_coroutineActive = false;
		yield break;
	}

	// Token: 0x06007260 RID: 29280 RVA: 0x002D7DFC File Offset: 0x002D5FFC
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		user.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(user.OnNewFloorLoaded, new Action<PlayerController>(this.HandleLevelLoaded));
	}

	// Token: 0x06007261 RID: 29281 RVA: 0x002D7E28 File Offset: 0x002D6028
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.LastOwner)
		{
			PlayerController lastOwner = this.LastOwner;
			lastOwner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(lastOwner.OnNewFloorLoaded, new Action<PlayerController>(this.HandleLevelLoaded));
		}
	}

	// Token: 0x06007262 RID: 29282 RVA: 0x002D7E68 File Offset: 0x002D6068
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.m_numberElapsedFloors);
	}

	// Token: 0x06007263 RID: 29283 RVA: 0x002D7E84 File Offset: 0x002D6084
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 1)
		{
			this.m_numberElapsedFloors = (int)data[0];
		}
	}

	// Token: 0x040073B5 RID: 29621
	public int m_numberElapsedFloors;

	// Token: 0x040073B6 RID: 29622
	public GameObject HealVFX;

	// Token: 0x040073B7 RID: 29623
	[EnemyIdentifier]
	public string GudetamaGuid;

	// Token: 0x040073B8 RID: 29624
	[PickupIdentifier]
	public int BabyDragunItemId;

	// Token: 0x040073B9 RID: 29625
	public DungeonPlaceableBehaviour BabyDragunPlaceable;

	// Token: 0x040073BA RID: 29626
	public float TimeInFireToHatch = 4f;

	// Token: 0x040073BB RID: 29627
	public bool DoShards;

	// Token: 0x040073BC RID: 29628
	public ShardsModule Shards;

	// Token: 0x040073BD RID: 29629
	private bool m_isBroken;

	// Token: 0x040073BE RID: 29630
	private float m_elapsedInFire;

	// Token: 0x040073BF RID: 29631
	private bool m_coroutineActive;
}
