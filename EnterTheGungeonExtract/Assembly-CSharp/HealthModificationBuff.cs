using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E2B RID: 3627
public class HealthModificationBuff : AppliedEffectBase
{
	// Token: 0x06004CBA RID: 19642 RVA: 0x001A3998 File Offset: 0x001A1B98
	private void InitializeSelf(float startChange, float endChange, float length, float period, float maxLength)
	{
		this.hh = base.GetComponent<HealthHaver>();
		this.healthChangeAtStart = startChange;
		this.healthChangeAtEnd = endChange;
		this.tickPeriod = period;
		this.lifetime = length;
		this.maxLifetime = maxLength;
		if (this.hh != null)
		{
			base.StartCoroutine(this.ApplyModification());
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CBB RID: 19643 RVA: 0x001A3A00 File Offset: 0x001A1C00
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is HealthModificationBuff)
		{
			HealthModificationBuff healthModificationBuff = source as HealthModificationBuff;
			this.InitializeSelf(healthModificationBuff.healthChangeAtStart, healthModificationBuff.healthChangeAtEnd, healthModificationBuff.lifetime, healthModificationBuff.tickPeriod, healthModificationBuff.maxLifetime);
			this.type = healthModificationBuff.type;
			if (healthModificationBuff.vfx != null)
			{
				bool flag = true;
				if (this.wasDuplicate && this.ChanceToApplyVFX < 1f && UnityEngine.Random.value > this.ChanceToApplyVFX)
				{
					flag = false;
				}
				if (flag)
				{
					this.instantiatedVFX = SpawnManager.SpawnVFX(healthModificationBuff.vfx, base.transform.position, Quaternion.identity);
					tk2dSprite component = this.instantiatedVFX.GetComponent<tk2dSprite>();
					tk2dSprite component2 = base.GetComponent<tk2dSprite>();
					if (component != null && component2 != null)
					{
						component2.AttachRenderer(component);
						component.HeightOffGround = 0.1f;
						component.IsPerpendicular = true;
						component.usesOverrideMaterial = true;
					}
					BuffVFXAnimator component3 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
					if (component3 != null)
					{
						component3.Initialize(base.GetComponent<GameActor>());
					}
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CBC RID: 19644 RVA: 0x001A3B34 File Offset: 0x001A1D34
	public void ExtendLength(float time)
	{
		this.lifetime = Mathf.Min(this.lifetime + time, this.elapsed + this.maxLifetime);
	}

	// Token: 0x06004CBD RID: 19645 RVA: 0x001A3B58 File Offset: 0x001A1D58
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<HealthHaver>() == null)
		{
			return;
		}
		bool flag = false;
		HealthModificationBuff[] components = target.GetComponents<HealthModificationBuff>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].type == this.type)
			{
				if (!this.supportsMultipleInstances)
				{
					components[i].ExtendLength(this.lifetime);
					return;
				}
				flag = true;
			}
		}
		HealthModificationBuff healthModificationBuff = target.AddComponent<HealthModificationBuff>();
		healthModificationBuff.wasDuplicate = flag;
		healthModificationBuff.Initialize(this);
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001A3BD8 File Offset: 0x001A1DD8
	private IEnumerator ApplyModification()
	{
		this.elapsed = 0f;
		while (this.elapsed < this.lifetime && this.hh && !this.hh.IsDead)
		{
			this.elapsed += this.tickPeriod;
			float changeThisTick = Mathf.Lerp(this.healthChangeAtStart, this.healthChangeAtEnd, this.elapsed / this.lifetime);
			if (changeThisTick < 0f)
			{
				this.hh.ApplyDamage(-1f * changeThisTick, Vector2.zero, base.name, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);
			}
			else
			{
				this.hh.ApplyHealing(changeThisTick);
			}
			yield return new WaitForSeconds(this.tickPeriod);
		}
		if (this.instantiatedVFX)
		{
			BuffVFXAnimator component = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
			if (component != null && component.persistsOnDeath)
			{
				tk2dSpriteAnimator component2 = component.GetComponent<tk2dSpriteAnimator>();
				if (component2 != null)
				{
					component2.Stop();
				}
				this.instantiatedVFX.GetComponent<PersistentVFXBehaviour>().BecomeDebris(Vector3.zero, 0.5f, new Type[0]);
			}
			else
			{
				UnityEngine.Object.Destroy(this.instantiatedVFX);
			}
		}
		UnityEngine.Object.Destroy(this);
		yield break;
	}

	// Token: 0x040042C6 RID: 17094
	public HealthModificationBuff.HealthModificationType type;

	// Token: 0x040042C7 RID: 17095
	public bool supportsMultipleInstances;

	// Token: 0x040042C8 RID: 17096
	[Tooltip("Time between damage or healing ticks.")]
	public float tickPeriod;

	// Token: 0x040042C9 RID: 17097
	[Tooltip("How long each application lasts.")]
	public float lifetime;

	// Token: 0x040042CA RID: 17098
	[Tooltip("Damage or healing at start of duration.")]
	public float healthChangeAtStart;

	// Token: 0x040042CB RID: 17099
	[Tooltip("Damage or healing at end of duration.")]
	public float healthChangeAtEnd;

	// Token: 0x040042CC RID: 17100
	[Tooltip("The maximum length of time this debuff can be extended to by repeat applications.")]
	public float maxLifetime;

	// Token: 0x040042CD RID: 17101
	public GameObject vfx;

	// Token: 0x040042CE RID: 17102
	public float ChanceToApplyVFX = 1f;

	// Token: 0x040042CF RID: 17103
	private float elapsed;

	// Token: 0x040042D0 RID: 17104
	private GameObject instantiatedVFX;

	// Token: 0x040042D1 RID: 17105
	private HealthHaver hh;

	// Token: 0x040042D2 RID: 17106
	private bool wasDuplicate;

	// Token: 0x02000E2C RID: 3628
	public enum HealthModificationType
	{
		// Token: 0x040042D4 RID: 17108
		BLEED,
		// Token: 0x040042D5 RID: 17109
		POISON,
		// Token: 0x040042D6 RID: 17110
		REGEN,
		// Token: 0x040042D7 RID: 17111
		UNIQUE
	}
}
