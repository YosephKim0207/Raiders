using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020012A6 RID: 4774
public class PitParticleKiller : MonoBehaviour
{
	// Token: 0x06006ACF RID: 27343 RVA: 0x0029E028 File Offset: 0x0029C228
	private void Start()
	{
		this.m_transform = base.transform;
		this.m_dungeon = GameManager.Instance.Dungeon;
		this.m_system = base.GetComponent<ParticleSystem>();
		this.m_particleArray = new ParticleSystem.Particle[this.m_system.maxParticles];
	}

	// Token: 0x06006AD0 RID: 27344 RVA: 0x0029E068 File Offset: 0x0029C268
	private bool LocalCellSupportsFalling(Vector3 worldPos)
	{
		IntVector2 intVector = worldPos.IntXY(VectorConversions.Floor);
		if (!this.m_dungeon.data.CheckInBounds(intVector))
		{
			return false;
		}
		CellData cellData = this.m_dungeon.data[intVector];
		return cellData != null && cellData.type == CellType.PIT && !cellData.fallingPrevented;
	}

	// Token: 0x06006AD1 RID: 27345 RVA: 0x0029E0C8 File Offset: 0x0029C2C8
	private void LateUpdate()
	{
		int particles = this.m_system.GetParticles(this.m_particleArray);
		for (int i = 0; i < particles; i++)
		{
			Vector3 vector = this.m_transform.TransformPoint(this.m_particleArray[i].position);
			if (this.LocalCellSupportsFalling(vector))
			{
				this.m_particleArray[i].remainingLifetime = 0f;
			}
		}
		this.m_system.SetParticles(this.m_particleArray, particles);
	}

	// Token: 0x04006767 RID: 26471
	private ParticleSystem.Particle[] m_particleArray;

	// Token: 0x04006768 RID: 26472
	private ParticleSystem m_system;

	// Token: 0x04006769 RID: 26473
	private Dungeon m_dungeon;

	// Token: 0x0400676A RID: 26474
	private Transform m_transform;
}
