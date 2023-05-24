using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001660 RID: 5728
public class ProjectileVolleyData : ScriptableObject
{
	// Token: 0x060085A0 RID: 34208 RVA: 0x00371CF4 File Offset: 0x0036FEF4
	public float GetVolleySpeedMod()
	{
		if (this.UsesShotgunStyleVelocityRandomizer)
		{
			return 1f + UnityEngine.Random.Range(this.DecreaseFinalSpeedPercentMin, this.IncreaseFinalSpeedPercentMax) / 100f;
		}
		return 1f;
	}

	// Token: 0x060085A1 RID: 34209 RVA: 0x00371D24 File Offset: 0x0036FF24
	public void InitializeFrom(ProjectileVolleyData source)
	{
		this.projectiles = new List<ProjectileModule>();
		this.UsesShotgunStyleVelocityRandomizer = source.UsesShotgunStyleVelocityRandomizer;
		this.DecreaseFinalSpeedPercentMin = source.DecreaseFinalSpeedPercentMin;
		this.IncreaseFinalSpeedPercentMax = source.IncreaseFinalSpeedPercentMax;
		for (int i = 0; i < source.projectiles.Count; i++)
		{
			this.projectiles.Add(ProjectileModule.CreateClone(source.projectiles[i], true, -1));
		}
		this.UsesBeamRotationLimiter = source.UsesBeamRotationLimiter;
		this.BeamRotationDegreesPerSecond = source.BeamRotationDegreesPerSecond;
		this.ModulesAreTiers = source.ModulesAreTiers;
	}

	// Token: 0x040089DC RID: 35292
	public List<ProjectileModule> projectiles;

	// Token: 0x040089DD RID: 35293
	public bool UsesBeamRotationLimiter;

	// Token: 0x040089DE RID: 35294
	public float BeamRotationDegreesPerSecond = 30f;

	// Token: 0x040089DF RID: 35295
	public bool ModulesAreTiers;

	// Token: 0x040089E0 RID: 35296
	public bool UsesShotgunStyleVelocityRandomizer;

	// Token: 0x040089E1 RID: 35297
	public float DecreaseFinalSpeedPercentMin = -5f;

	// Token: 0x040089E2 RID: 35298
	public float IncreaseFinalSpeedPercentMax = 5f;
}
