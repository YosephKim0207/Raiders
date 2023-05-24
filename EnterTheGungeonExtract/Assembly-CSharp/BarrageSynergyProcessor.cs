using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020016DD RID: 5853
public class BarrageSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008824 RID: 34852 RVA: 0x00386F34 File Offset: 0x00385134
	private void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_currentCooldown = UnityEngine.Random.Range(this.MinBarrageCooldown, this.MaxBarrageCooldown);
	}

	// Token: 0x06008825 RID: 34853 RVA: 0x00386F5C File Offset: 0x0038515C
	private void Update()
	{
		if (Dungeon.IsGenerating || GameManager.IsBossIntro)
		{
			return;
		}
		if (this.BarrageIsAmbient && this.m_gun && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.HasActiveBonusSynergy(this.RequiredSynergy, false) && playerController.IsInCombat)
			{
				this.m_elapsed += BraveTime.DeltaTime;
				if (this.m_elapsed >= this.m_currentCooldown)
				{
					this.m_elapsed -= this.m_currentCooldown;
					this.m_currentCooldown = UnityEngine.Random.Range(this.MinBarrageCooldown, this.MaxBarrageCooldown);
					this.DoAmbientTargetedBarrage(playerController);
				}
			}
		}
	}

	// Token: 0x06008826 RID: 34854 RVA: 0x00387030 File Offset: 0x00385230
	private void DoAmbientTargetedBarrage(PlayerController p)
	{
		List<AIActor> activeEnemies = p.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies == null)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, activeEnemies.Count);
		Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
		Vector2 vector = activeEnemies[num].CenterPosition + -normalized * (this.Barrage.BarrageLength / 2f);
		if (activeEnemies[num])
		{
			this.Barrage.DoBarrage(vector, normalized, GameManager.Instance.Dungeon);
		}
	}

	// Token: 0x04008D6F RID: 36207
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008D70 RID: 36208
	public BarrageModule Barrage;

	// Token: 0x04008D71 RID: 36209
	public bool BarrageIsAmbient;

	// Token: 0x04008D72 RID: 36210
	public float MinBarrageCooldown = 5f;

	// Token: 0x04008D73 RID: 36211
	public float MaxBarrageCooldown = 5f;

	// Token: 0x04008D74 RID: 36212
	private Gun m_gun;

	// Token: 0x04008D75 RID: 36213
	private float m_elapsed;

	// Token: 0x04008D76 RID: 36214
	private float m_currentCooldown;
}
