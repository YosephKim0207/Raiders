using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020011AE RID: 4526
public class MineCartFactory : DungeonPlaceableBehaviour
{
	// Token: 0x060064ED RID: 25837 RVA: 0x0027381C File Offset: 0x00271A1C
	private void Start()
	{
		this.m_room = base.GetAbsoluteParentRoom();
		this.m_spawnedCarts = new List<MineCartController>();
	}

	// Token: 0x060064EE RID: 25838 RVA: 0x00273838 File Offset: 0x00271A38
	private void Update()
	{
		if (!GameManager.Instance.IsAnyPlayerInRoom(this.m_room))
		{
			return;
		}
		for (int i = 0; i < this.m_spawnedCarts.Count; i++)
		{
			if (!this.m_spawnedCarts[i])
			{
				this.m_spawnedCarts.RemoveAt(i);
				i--;
				this.m_delayTimer = Mathf.Max(this.DelayUponDestruction, this.m_delayTimer);
			}
		}
		if (this.m_delayTimer <= 0f && (float)this.m_spawnedCarts.Count < this.MaxCarts)
		{
			this.m_delayTimer = this.DelayBetweenCarts;
			this.DoSpawnCart();
		}
		this.m_delayTimer = Mathf.Max(0f, this.m_delayTimer - BraveTime.DeltaTime);
	}

	// Token: 0x060064EF RID: 25839 RVA: 0x0027390C File Offset: 0x00271B0C
	private IEnumerator DelayedApplyVelocity(MineCartController mcc)
	{
		yield return null;
		mcc.ApplyVelocity(mcc.MaxSpeedEnemy);
		yield break;
	}

	// Token: 0x060064F0 RID: 25840 RVA: 0x00273928 File Offset: 0x00271B28
	protected void DoSpawnCart()
	{
		RoomHandler absoluteParentRoom = base.GetAbsoluteParentRoom();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MineCartPrefab.gameObject, base.transform.position, Quaternion.identity);
		MineCartController component = gameObject.GetComponent<MineCartController>();
		PathMover component2 = gameObject.GetComponent<PathMover>();
		if (this.ForceCartActive)
		{
			component.ForceActive = true;
		}
		absoluteParentRoom.RegisterInteractable(component);
		component2.Path = absoluteParentRoom.area.runtimePrototypeData.paths[Mathf.RoundToInt(this.TargetPathIndex)];
		component2.PathStartNode = Mathf.RoundToInt(this.TargetPathNodeIndex);
		component2.RoomHandler = absoluteParentRoom;
		this.m_spawnedCarts.Add(component);
		if (component.occupation == MineCartController.CartOccupationState.EMPTY && (float)this.m_spawnedCarts.Count < this.MaxCarts)
		{
			base.StartCoroutine(this.DelayedApplyVelocity(component));
		}
	}

	// Token: 0x060064F1 RID: 25841 RVA: 0x00273A00 File Offset: 0x00271C00
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006089 RID: 24713
	public MineCartController MineCartPrefab;

	// Token: 0x0400608A RID: 24714
	[DwarfConfigurable]
	public float TargetPathIndex;

	// Token: 0x0400608B RID: 24715
	[DwarfConfigurable]
	public float TargetPathNodeIndex;

	// Token: 0x0400608C RID: 24716
	[DwarfConfigurable]
	public float MaxCarts = 5f;

	// Token: 0x0400608D RID: 24717
	[DwarfConfigurable]
	public float DelayBetweenCarts = 3f;

	// Token: 0x0400608E RID: 24718
	[DwarfConfigurable]
	public float DelayUponDestruction = 1f;

	// Token: 0x0400608F RID: 24719
	public bool ForceCartActive;

	// Token: 0x04006090 RID: 24720
	[NonSerialized]
	private List<MineCartController> m_spawnedCarts;

	// Token: 0x04006091 RID: 24721
	[NonSerialized]
	private float m_delayTimer = 1f;

	// Token: 0x04006092 RID: 24722
	private RoomHandler m_room;
}
