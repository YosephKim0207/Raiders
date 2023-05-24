using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013FB RID: 5115
public class EstusFlaskItem : PlayerItem
{
	// Token: 0x17001175 RID: 4469
	// (get) Token: 0x06007414 RID: 29716 RVA: 0x002E3284 File Offset: 0x002E1484
	public int RemainingDrinks
	{
		get
		{
			return this.m_remainingDrinksThisFloor;
		}
	}

	// Token: 0x06007415 RID: 29717 RVA: 0x002E328C File Offset: 0x002E148C
	public override void Pickup(PlayerController player)
	{
		this.m_owner = player;
		if (!this.m_pickedUpThisRun)
		{
			this.m_remainingDrinksThisFloor = this.numDrinksPerFloor;
		}
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.ResetFlaskForFloor));
		base.Pickup(player);
	}

	// Token: 0x06007416 RID: 29718 RVA: 0x002E32E0 File Offset: 0x002E14E0
	protected override void OnPreDrop(PlayerController user)
	{
		user.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(user.OnNewFloorLoaded, new Action<PlayerController>(this.ResetFlaskForFloor));
		this.m_owner = null;
		base.OnPreDrop(user);
	}

	// Token: 0x06007417 RID: 29719 RVA: 0x002E3314 File Offset: 0x002E1514
	private void ResetFlaskForFloor(PlayerController obj)
	{
		this.m_remainingDrinksThisFloor = this.numDrinksPerFloor;
		base.sprite.SetSprite(this.HasDrinkSprite);
	}

	// Token: 0x06007418 RID: 29720 RVA: 0x002E3334 File Offset: 0x002E1534
	public override bool CanBeUsed(PlayerController user)
	{
		return this.m_remainingDrinksThisFloor > 0;
	}

	// Token: 0x06007419 RID: 29721 RVA: 0x002E3340 File Offset: 0x002E1540
	protected override void DoEffect(PlayerController user)
	{
		if (this.m_remainingDrinksThisFloor > 0)
		{
			this.m_remainingDrinksThisFloor--;
			user.StartCoroutine(this.HandleDrinkEstus(user));
		}
		if (this.m_remainingDrinksThisFloor <= 0)
		{
			base.sprite.SetSprite(this.NoDrinkSprite);
		}
	}

	// Token: 0x0600741A RID: 29722 RVA: 0x002E3394 File Offset: 0x002E1594
	private IEnumerator HandleDrinkEstus(PlayerController user)
	{
		float elapsed = 0f;
		if (this.healVFX != null)
		{
			user.PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
		}
		user.SetInputOverride("estus");
		while (elapsed < this.drinkDuration)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		user.ClearInputOverride("estus");
		user.healthHaver.ApplyHealing(this.healingAmount);
		AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", base.gameObject);
		yield break;
	}

	// Token: 0x0600741B RID: 29723 RVA: 0x002E33B8 File Offset: 0x002E15B8
	protected override void CopyStateFrom(PlayerItem other)
	{
		base.CopyStateFrom(other);
		EstusFlaskItem estusFlaskItem = other as EstusFlaskItem;
		if (estusFlaskItem)
		{
			this.m_remainingDrinksThisFloor = estusFlaskItem.m_remainingDrinksThisFloor;
		}
	}

	// Token: 0x0600741C RID: 29724 RVA: 0x002E33EC File Offset: 0x002E15EC
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			PlayerController owner = this.m_owner;
			owner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(owner.OnNewFloorLoaded, new Action<PlayerController>(this.ResetFlaskForFloor));
		}
		base.OnDestroy();
	}

	// Token: 0x040075A7 RID: 30119
	public int numDrinksPerFloor = 2;

	// Token: 0x040075A8 RID: 30120
	public float healingAmount = 1f;

	// Token: 0x040075A9 RID: 30121
	public float drinkDuration = 1f;

	// Token: 0x040075AA RID: 30122
	public string HasDrinkSprite;

	// Token: 0x040075AB RID: 30123
	public string NoDrinkSprite;

	// Token: 0x040075AC RID: 30124
	public GameObject healVFX;

	// Token: 0x040075AD RID: 30125
	private PlayerController m_owner;

	// Token: 0x040075AE RID: 30126
	private int m_remainingDrinksThisFloor;
}
