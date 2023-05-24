using System;
using UnityEngine;

// Token: 0x0200160F RID: 5647
[Serializable]
public class ProjectileData
{
	// Token: 0x060083AA RID: 33706 RVA: 0x0035F3C0 File Offset: 0x0035D5C0
	public ProjectileData()
	{
	}

	// Token: 0x060083AB RID: 33707 RVA: 0x0035F400 File Offset: 0x0035D600
	public ProjectileData(ProjectileData other)
	{
		this.SetAll(other);
	}

	// Token: 0x060083AC RID: 33708 RVA: 0x0035F454 File Offset: 0x0035D654
	public void SetAll(ProjectileData other)
	{
		this.damage = other.damage;
		this.speed = other.speed;
		this.range = other.range;
		this.force = other.force;
		this.damping = other.damping;
		this.UsesCustomAccelerationCurve = other.UsesCustomAccelerationCurve;
		this.AccelerationCurve = other.AccelerationCurve;
		this.CustomAccelerationCurveDuration = other.CustomAccelerationCurveDuration;
		this.onDestroyBulletScript = other.onDestroyBulletScript.Clone();
	}

	// Token: 0x040086E9 RID: 34537
	public static float FixedFallbackDamageToEnemies = 10f;

	// Token: 0x040086EA RID: 34538
	public static float FixedEnemyDamageToBreakables = 8f;

	// Token: 0x040086EB RID: 34539
	public float damage = 2.5f;

	// Token: 0x040086EC RID: 34540
	public float speed = 10f;

	// Token: 0x040086ED RID: 34541
	public float range = 10f;

	// Token: 0x040086EE RID: 34542
	public float force = 10f;

	// Token: 0x040086EF RID: 34543
	public float damping;

	// Token: 0x040086F0 RID: 34544
	public bool UsesCustomAccelerationCurve;

	// Token: 0x040086F1 RID: 34545
	[ShowInInspectorIf("UsesCustomAccelerationCurve", true)]
	public AnimationCurve AccelerationCurve;

	// Token: 0x040086F2 RID: 34546
	[ShowInInspectorIf("UsesCustomAccelerationCurve", true)]
	public float CustomAccelerationCurveDuration = 1f;

	// Token: 0x040086F3 RID: 34547
	[NonSerialized]
	public float IgnoreAccelCurveTime;

	// Token: 0x040086F4 RID: 34548
	public BulletScriptSelector onDestroyBulletScript;
}
