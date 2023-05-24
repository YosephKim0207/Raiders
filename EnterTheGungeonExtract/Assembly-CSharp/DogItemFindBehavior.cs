using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E00 RID: 3584
public class DogItemFindBehavior : BehaviorBase
{
	// Token: 0x06004BE2 RID: 19426 RVA: 0x0019E0F8 File Offset: 0x0019C2F8
	public override void Start()
	{
		base.Start();
		if (this.m_aiActor.CompanionOwner != null)
		{
			this.m_aiActor.CompanionOwner.OnRoomClearEvent += this.HandleRoomCleared;
		}
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x0019E134 File Offset: 0x0019C334
	public override void Destroy()
	{
		if (this.m_aiActor.CompanionOwner != null)
		{
			this.m_aiActor.CompanionOwner.OnRoomClearEvent -= this.HandleRoomCleared;
		}
		base.Destroy();
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x0019E170 File Offset: 0x0019C370
	private IEnumerator DelayedSpawnItem(Vector2 spawnPoint)
	{
		yield return new WaitForSeconds(0.5f);
		LootEngine.SpawnItem(this.ItemFindLootTable.SelectByWeight(false), spawnPoint, Vector2.up, 1f, true, false, false);
		yield break;
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x0019E194 File Offset: 0x0019C394
	private void HandleRoomCleared(PlayerController obj)
	{
		if (UnityEngine.Random.value < this.ChanceToFindItemOnRoomClear)
		{
			this.m_findTimer = 4.5f;
			if (!string.IsNullOrEmpty(this.ItemFindAnimName))
			{
				this.m_aiAnimator.PlayUntilFinished(this.ItemFindAnimName, false, null, -1f, false);
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.DelayedSpawnItem(this.m_aiActor.CenterPosition));
		}
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x0019E208 File Offset: 0x0019C408
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004BE7 RID: 19431 RVA: 0x0019E210 File Offset: 0x0019C410
	public override BehaviorResult Update()
	{
		if (this.m_findTimer > 0f)
		{
			base.DecrementTimer(ref this.m_findTimer, false);
			this.m_aiActor.ClearPath();
		}
		return base.Update();
	}

	// Token: 0x040041C3 RID: 16835
	public GenericLootTable ItemFindLootTable;

	// Token: 0x040041C4 RID: 16836
	public float ChanceToFindItemOnRoomClear = 0.05f;

	// Token: 0x040041C5 RID: 16837
	public string ItemFindAnimName;

	// Token: 0x040041C6 RID: 16838
	private float m_findTimer;
}
