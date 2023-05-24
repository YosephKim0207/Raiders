using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010F5 RID: 4341
public class ArtfulDodgerProjectileController : MonoBehaviour
{
	// Token: 0x06005FAA RID: 24490 RVA: 0x0024D324 File Offset: 0x0024B524
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_projectile.OnDestruction += this.HandleDestruction;
		this.m_bouncer = base.GetComponent<BounceProjModifier>();
	}

	// Token: 0x06005FAB RID: 24491 RVA: 0x0024D358 File Offset: 0x0024B558
	private void HandleDestruction(Projectile source)
	{
		if (!this.hitTarget)
		{
			List<ArtfulDodgerTargetController> componentsAbsoluteInRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor)).GetComponentsAbsoluteInRoom<ArtfulDodgerTargetController>();
			for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
			{
				if (!componentsAbsoluteInRoom[i].IsBroken)
				{
					componentsAbsoluteInRoom[i].GetComponentInChildren<tk2dSpriteAnimator>().PlayForDuration("target_miss", 3f, "target_idle", false);
				}
			}
		}
		else
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.WINCHESTER_SHOTS_HIT, 1f);
		}
	}

	// Token: 0x06005FAC RID: 24492 RVA: 0x0024D3FC File Offset: 0x0024B5FC
	private void Update()
	{
		int numberOfBounces = this.m_bouncer.numberOfBounces;
		this.m_projectile.ChangeTintColorShader(0f, BraveUtility.GetRainbowColor(numberOfBounces));
	}

	// Token: 0x04005A2F RID: 23087
	[NonSerialized]
	public bool hitTarget;

	// Token: 0x04005A30 RID: 23088
	private Projectile m_projectile;

	// Token: 0x04005A31 RID: 23089
	private BounceProjModifier m_bouncer;
}
