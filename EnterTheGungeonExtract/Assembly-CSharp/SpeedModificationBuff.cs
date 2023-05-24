using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E2F RID: 3631
public class SpeedModificationBuff : AppliedEffectBase
{
	// Token: 0x06004CCD RID: 19661 RVA: 0x001A4474 File Offset: 0x001A2674
	public static void ApplySpeedModificationToTarget(GameObject target, float maxSpeedMod, float lifetime, float maxLifetime)
	{
		if (target.GetComponent<SpeculativeRigidbody>() == null)
		{
			return;
		}
		SpeedModificationBuff speedModificationBuff = target.GetComponent<SpeedModificationBuff>();
		if (speedModificationBuff != null)
		{
			speedModificationBuff.ExtendLength(lifetime);
			return;
		}
		speedModificationBuff = target.AddComponent<SpeedModificationBuff>();
		speedModificationBuff.maximumSpeedModifier = maxSpeedMod;
		speedModificationBuff.lifespan = lifetime;
		speedModificationBuff.maxLifespan = maxLifetime;
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001A44CC File Offset: 0x001A26CC
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<SpeculativeRigidbody>() == null)
		{
			return;
		}
		SpeedModificationBuff component = target.GetComponent<SpeedModificationBuff>();
		if (component != null)
		{
			component.ExtendLength(this.lifespan);
			return;
		}
		SpeedModificationBuff speedModificationBuff = target.AddComponent<SpeedModificationBuff>();
		speedModificationBuff.Initialize(this);
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001A451C File Offset: 0x001A271C
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is SpeedModificationBuff)
		{
			SpeedModificationBuff speedModificationBuff = source as SpeedModificationBuff;
			this.maximumSpeedModifier = speedModificationBuff.maximumSpeedModifier;
			this.lifespan = speedModificationBuff.lifespan;
			this.maxLifespan = speedModificationBuff.maxLifespan;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x001A456C File Offset: 0x001A276C
	public void ExtendLength(float time)
	{
		this.lifespan = Mathf.Min(this.lifespan + time, this.elapsed + this.maxLifespan);
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001A4590 File Offset: 0x001A2790
	private IEnumerator ApplyModification()
	{
		this.elapsed = 0f;
		while (this.elapsed < this.lifespan)
		{
			this.elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		UnityEngine.Object.Destroy(this);
		yield break;
	}

	// Token: 0x040042E6 RID: 17126
	public float maximumSpeedModifier;

	// Token: 0x040042E7 RID: 17127
	public float lifespan;

	// Token: 0x040042E8 RID: 17128
	public float maxLifespan;

	// Token: 0x040042E9 RID: 17129
	private float elapsed;
}
