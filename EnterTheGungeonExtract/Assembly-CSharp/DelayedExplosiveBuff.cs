using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E1C RID: 3612
public class DelayedExplosiveBuff : AppliedEffectBase
{
	// Token: 0x06004C7B RID: 19579 RVA: 0x001A17CC File Offset: 0x0019F9CC
	private void InitializeSelf(float delayBefore, bool doRefresh, ExplosionData data)
	{
		this.explosionData = data;
		this.additionalInstancesRefreshDelay = doRefresh;
		this.delayBeforeBurst = delayBefore;
		this.hh = base.GetComponent<HealthHaver>();
		if (this.hh != null)
		{
			base.StartCoroutine(this.ApplyModification());
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x001A1824 File Offset: 0x0019FA24
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is DelayedExplosiveBuff)
		{
			DelayedExplosiveBuff delayedExplosiveBuff = source as DelayedExplosiveBuff;
			this.InitializeSelf(delayedExplosiveBuff.delayBeforeBurst, delayedExplosiveBuff.additionalInstancesRefreshDelay, delayedExplosiveBuff.explosionData);
			if (delayedExplosiveBuff.vfx != null)
			{
				this.instantiatedVFX = SpawnManager.SpawnVFX(delayedExplosiveBuff.vfx, base.transform.position, Quaternion.identity, true);
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
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x001A1908 File Offset: 0x0019FB08
	public void ExtendLength()
	{
		this.elapsed = 0f;
	}

	// Token: 0x06004C7E RID: 19582 RVA: 0x001A1918 File Offset: 0x0019FB18
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<HealthHaver>() == null)
		{
			return;
		}
		bool flag = false;
		if (this.additionalInstancesRefreshDelay)
		{
			DelayedExplosiveBuff[] components = target.GetComponents<DelayedExplosiveBuff>();
			for (int i = 0; i < components.Length; i++)
			{
				flag = true;
				components[i].ExtendLength();
			}
		}
		DelayedExplosiveBuff delayedExplosiveBuff = target.AddComponent<DelayedExplosiveBuff>();
		delayedExplosiveBuff.IsSecondaryBuff = flag;
		delayedExplosiveBuff.Initialize(this);
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x001A1980 File Offset: 0x0019FB80
	private IEnumerator ApplyModification()
	{
		this.elapsed = 0f;
		while (this.elapsed < this.delayBeforeBurst && this.hh && !this.hh.IsDead)
		{
			this.elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (this.hh)
		{
			if (this.IsSecondaryBuff)
			{
				this.hh.ApplyDamage(this.explosionData.damage, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			else
			{
				Exploder.Explode(this.hh.aiActor.CenterPosition, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
			}
		}
		if (this.instantiatedVFX)
		{
			BuffVFXAnimator component = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
			if (component != null && component.persistsOnDeath)
			{
				tk2dSpriteAnimator component2 = component.GetComponent<tk2dSpriteAnimator>();
				component2.Sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_COMPLEX;
				component2.PlayAndDestroyObject(string.Empty, null);
			}
			else
			{
				UnityEngine.Object.Destroy(this.instantiatedVFX);
			}
		}
		UnityEngine.Object.Destroy(this);
		yield break;
	}

	// Token: 0x04004269 RID: 17001
	public bool additionalInstancesRefreshDelay = true;

	// Token: 0x0400426A RID: 17002
	public float delayBeforeBurst = 0.25f;

	// Token: 0x0400426B RID: 17003
	public ExplosionData explosionData;

	// Token: 0x0400426C RID: 17004
	public GameObject vfx;

	// Token: 0x0400426D RID: 17005
	[NonSerialized]
	public bool IsSecondaryBuff;

	// Token: 0x0400426E RID: 17006
	private float elapsed;

	// Token: 0x0400426F RID: 17007
	private GameObject instantiatedVFX;

	// Token: 0x04004270 RID: 17008
	private HealthHaver hh;
}
