using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016D9 RID: 5849
public class AmbientProjectileSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008811 RID: 34833 RVA: 0x0038698C File Offset: 0x00384B8C
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06008812 RID: 34834 RVA: 0x0038699C File Offset: 0x00384B9C
	private void Update()
	{
		if (this.m_gun && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.HasActiveBonusSynergy(this.SynergyToCheck, false) && (this.ActiveEvenWithoutEnemies || (playerController.CurrentRoom != null && playerController.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))))
			{
				this.m_elapsed += BraveTime.DeltaTime;
				if (this.UsesRadius)
				{
					float maxValue = float.MaxValue;
					playerController.CurrentRoom.GetNearestEnemy(playerController.CenterPosition, out maxValue, true, false);
					if (maxValue > this.Radius)
					{
						return;
					}
				}
				if (this.m_elapsed > this.TimeBetweenAmbientProjectiles)
				{
					this.m_elapsed = 0f;
					this.Ambience.DoBurst(playerController, null, null);
				}
			}
		}
	}

	// Token: 0x04008D49 RID: 36169
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008D4A RID: 36170
	public float TimeBetweenAmbientProjectiles = 5f;

	// Token: 0x04008D4B RID: 36171
	public bool ActiveEvenWithoutEnemies;

	// Token: 0x04008D4C RID: 36172
	public bool UsesRadius;

	// Token: 0x04008D4D RID: 36173
	public float Radius = 5f;

	// Token: 0x04008D4E RID: 36174
	public RadialBurstInterface Ambience;

	// Token: 0x04008D4F RID: 36175
	private Gun m_gun;

	// Token: 0x04008D50 RID: 36176
	private float m_elapsed;
}
