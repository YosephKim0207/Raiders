using System;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020010A8 RID: 4264
public class KeyBulletManController : BraveBehaviour
{
	// Token: 0x06005E01 RID: 24065 RVA: 0x00240E48 File Offset: 0x0023F048
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnPreDeath;
		AIActor aiActor = base.aiActor;
		aiActor.OnHandleRewards = (Action)Delegate.Combine(aiActor.OnHandleRewards, new Action(this.OnHandleRewards));
		base.aiActor.SuppressBlackPhantomCorpseBurn = true;
	}

	// Token: 0x06005E02 RID: 24066 RVA: 0x00240EA0 File Offset: 0x0023F0A0
	protected override void OnDestroy()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		if (base.aiActor)
		{
			AIActor aiActor = base.aiActor;
			aiActor.OnHandleRewards = (Action)Delegate.Remove(aiActor.OnHandleRewards, new Action(this.OnHandleRewards));
		}
		base.OnDestroy();
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x00240F14 File Offset: 0x0023F114
	private void OnPreDeath(Vector2 dir)
	{
		this.m_cachedIsBlackPhantom = base.aiActor.IsBlackPhantom;
		if (this.lookPickupId == GlobalItemIds.Key && base.aiActor.IsBlackPhantom)
		{
			base.aiActor.UnbecomeBlackPhantom();
		}
		if (this.RemoveShaderOnDeath)
		{
			base.renderer.sharedMaterials = new Material[] { base.renderer.sharedMaterials[0] };
		}
	}

	// Token: 0x06005E04 RID: 24068 RVA: 0x00240F8C File Offset: 0x0023F18C
	public void ForceHandleRewards()
	{
		this.OnHandleRewards();
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x00240F94 File Offset: 0x0023F194
	private void OnHandleRewards()
	{
		bool flag = false;
		if (this.lookPickupId == GlobalItemIds.Key)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.KEYBULLETMEN_KILLED, 1f);
			flag = true;
		}
		Vector3 vector = base.transform.position + this.offset;
		if (!flag && GameManager.Instance.Dungeon.data.isAnyFaceWall(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y + 0.5f)))
		{
			vector += new Vector3(0f, -1f, 0f);
		}
		CellData cellData = (vector + new Vector3(0f, 0.5f, 0f)).GetCell();
		if (cellData == null || cellData.type == CellType.WALL || cellData.IsAnyFaceWall())
		{
			Vector3 vector2;
			vector = (vector2 = vector + Vector3.down);
			cellData = vector2.GetCell();
			if (cellData != null && cellData.type != CellType.WALL)
			{
				vector += Vector3.down;
			}
		}
		if (this.doubleForBlackPhantom && this.m_cachedIsBlackPhantom)
		{
			LootEngine.SpawnItem(this.GetReward(), vector, Vector2.left, 2f, false, false, true);
			LootEngine.SpawnItem(this.GetReward(), vector, Vector2.right, 2f, false, false, true);
		}
		else if (flag)
		{
			LootEngine.SpawnItem(this.GetReward(), vector, Vector2.zero, 0f, true, false, true);
		}
		else
		{
			LootEngine.SpawnItem(this.GetReward(), vector, Vector2.up, 0.1f, true, false, true);
		}
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x00241138 File Offset: 0x0023F338
	private GameObject GetReward()
	{
		if (this.lootTable)
		{
			return this.lootTable.SelectByWeight(false);
		}
		return PickupObjectDatabase.GetById(this.lookPickupId).gameObject;
	}

	// Token: 0x0400581A RID: 22554
	[PickupIdentifier]
	[FormerlySerializedAs("keyId")]
	public int lookPickupId = -1;

	// Token: 0x0400581B RID: 22555
	public GenericLootTable lootTable;

	// Token: 0x0400581C RID: 22556
	public Vector2 offset;

	// Token: 0x0400581D RID: 22557
	public bool doubleForBlackPhantom = true;

	// Token: 0x0400581E RID: 22558
	public bool RemoveShaderOnDeath;

	// Token: 0x0400581F RID: 22559
	private bool m_cachedIsBlackPhantom;
}
