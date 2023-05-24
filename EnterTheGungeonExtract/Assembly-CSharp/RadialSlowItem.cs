using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001475 RID: 5237
public class RadialSlowItem : AffectEnemiesInRadiusItem
{
	// Token: 0x0600770B RID: 30475 RVA: 0x002F7AAC File Offset: 0x002F5CAC
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_time_bell_01", base.gameObject);
		base.DoEffect(user);
		if (this.AllowStealing)
		{
			user.StartCoroutine(this.HandleStealEffect(user));
		}
	}

	// Token: 0x0600770C RID: 30476 RVA: 0x002F7AE0 File Offset: 0x002F5CE0
	private IEnumerator HandleStealEffect(PlayerController user)
	{
		user.SetCapableOfStealing(true, "AffectEnemiesInRadiusItem", null);
		this.m_activeDuration = this.InTime + this.HoldTime + this.OutTime;
		while (this.m_activeElapsed < this.m_activeDuration)
		{
			this.m_activeElapsed += BraveTime.DeltaTime;
			yield return null;
		}
		user.SetCapableOfStealing(false, "AffectEnemiesInRadiusItem", null);
		yield break;
	}

	// Token: 0x0600770D RID: 30477 RVA: 0x002F7B04 File Offset: 0x002F5D04
	protected override void AffectEnemy(AIActor target)
	{
		if (!base.IsCurrentlyActive)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
		}
		target.StartCoroutine(this.ProcessSlow(target));
	}

	// Token: 0x0600770E RID: 30478 RVA: 0x002F7B38 File Offset: 0x002F5D38
	protected override void AffectForgeHammer(ForgeHammerController target)
	{
		if (!base.IsCurrentlyActive)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
		}
		target.StartCoroutine(this.ProcessHammerSlow(target));
	}

	// Token: 0x0600770F RID: 30479 RVA: 0x002F7B6C File Offset: 0x002F5D6C
	protected override void AffectProjectileTrap(ProjectileTrapController target)
	{
		if (!base.IsCurrentlyActive)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
		}
		target.StartCoroutine(this.ProcessTrapSlow(target));
	}

	// Token: 0x06007710 RID: 30480 RVA: 0x002F7BA0 File Offset: 0x002F5DA0
	protected override void AffectShop(BaseShopController target)
	{
		if (this.AllowStealing && target && target.shopkeepFSM)
		{
			AIAnimator component = target.shopkeepFSM.GetComponent<AIAnimator>();
			if (!base.IsCurrentlyActive)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
			}
			target.StartCoroutine(this.ProcessShopSlow(target, component));
		}
	}

	// Token: 0x06007711 RID: 30481 RVA: 0x002F7C10 File Offset: 0x002F5E10
	protected override void AffectMajorBreakable(MajorBreakable target)
	{
		if (target.behaviorSpeculator)
		{
			target.StartCoroutine(this.ProcessBehaviorSpeculatorSlow(target.behaviorSpeculator));
		}
	}

	// Token: 0x06007712 RID: 30482 RVA: 0x002F7C38 File Offset: 0x002F5E38
	private IEnumerator HandleActive()
	{
		base.IsCurrentlyActive = true;
		this.m_activeDuration = this.InTime + this.HoldTime + this.OutTime;
		while (this.m_activeElapsed < this.m_activeDuration)
		{
			this.m_activeElapsed += BraveTime.DeltaTime;
			yield return null;
		}
		base.IsCurrentlyActive = false;
		yield break;
	}

	// Token: 0x06007713 RID: 30483 RVA: 0x002F7C54 File Offset: 0x002F5E54
	private IEnumerator ProcessSlow(AIActor target)
	{
		float elapsed = 0f;
		if (this.InTime > 0f)
		{
			while (elapsed < this.InTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / this.InTime;
				target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, t);
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.HoldTime > 0f)
		{
			while (elapsed < this.HoldTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = this.MaxTimeModifier;
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.OutTime > 0f)
		{
			while (elapsed < this.OutTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t2 = elapsed / this.OutTime;
				target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, t2);
				yield return null;
			}
		}
		if (target)
		{
			target.LocalTimeScale = 1f;
		}
		yield break;
	}

	// Token: 0x06007714 RID: 30484 RVA: 0x002F7C78 File Offset: 0x002F5E78
	private IEnumerator ProcessHammerSlow(ForgeHammerController target)
	{
		float elapsed = 0f;
		if (this.InTime > 0f)
		{
			while (elapsed < this.InTime)
			{
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, elapsed / this.InTime);
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.HoldTime > 0f)
		{
			while (elapsed < this.HoldTime)
			{
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = this.MaxTimeModifier;
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.OutTime > 0f)
		{
			while (elapsed < this.OutTime)
			{
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, elapsed / this.OutTime);
				yield return null;
			}
		}
		if (target)
		{
			target.LocalTimeScale = 1f;
		}
		yield break;
	}

	// Token: 0x06007715 RID: 30485 RVA: 0x002F7C9C File Offset: 0x002F5E9C
	private IEnumerator ProcessTrapSlow(ProjectileTrapController target)
	{
		float elapsed = 0f;
		if (this.InTime > 0f)
		{
			while (elapsed < this.InTime)
			{
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, elapsed / this.InTime);
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.HoldTime > 0f)
		{
			while (elapsed < this.HoldTime)
			{
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = this.MaxTimeModifier;
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.OutTime > 0f)
		{
			while (elapsed < this.OutTime)
			{
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, elapsed / this.OutTime);
				yield return null;
			}
		}
		if (target)
		{
			target.LocalTimeScale = 1f;
		}
		yield break;
	}

	// Token: 0x06007716 RID: 30486 RVA: 0x002F7CC0 File Offset: 0x002F5EC0
	private IEnumerator ProcessShopSlow(BaseShopController target, AIAnimator shopkeep)
	{
		target.SetCapableOfBeingStolenFrom(true, "RadialSlowItem", null);
		float elapsed = 0f;
		if (this.HoldTime + this.InTime > 0f)
		{
			while (elapsed < this.HoldTime + this.InTime && !target.WasCaughtStealing)
			{
				elapsed += BraveTime.DeltaTime;
				shopkeep.aiAnimator.FpsScale = this.MaxTimeModifier;
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.OutTime > 0f)
		{
			while (elapsed < this.OutTime && !target.WasCaughtStealing)
			{
				elapsed += BraveTime.DeltaTime;
				shopkeep.aiAnimator.FpsScale = Mathf.Lerp(this.MaxTimeModifier, 1f, elapsed / this.OutTime);
				yield return null;
			}
		}
		shopkeep.aiAnimator.FpsScale = 1f;
		target.SetCapableOfBeingStolenFrom(false, "RadialSlowItem", null);
		yield break;
	}

	// Token: 0x06007717 RID: 30487 RVA: 0x002F7CEC File Offset: 0x002F5EEC
	private IEnumerator ProcessBehaviorSpeculatorSlow(BehaviorSpeculator target)
	{
		float elapsed = 0f;
		AIAnimator aiAnimator = ((!target) ? null : target.aiAnimator);
		if (this.InTime > 0f)
		{
			while (elapsed < this.InTime)
			{
				if (!target)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / this.InTime;
				target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, t);
				if (aiAnimator)
				{
					aiAnimator.FpsScale = Mathf.Lerp(1f, this.MaxTimeModifier, t);
				}
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.HoldTime > 0f)
		{
			while (elapsed < this.HoldTime)
			{
				if (!target)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				target.LocalTimeScale = this.MaxTimeModifier;
				if (aiAnimator)
				{
					aiAnimator.FpsScale = this.MaxTimeModifier;
				}
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.OutTime > 0f)
		{
			while (elapsed < this.OutTime)
			{
				if (!target)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t2 = elapsed / this.OutTime;
				target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, t2);
				if (aiAnimator)
				{
					aiAnimator.FpsScale = Mathf.Lerp(this.MaxTimeModifier, 1f, t2);
				}
				yield return null;
			}
		}
		if (aiAnimator)
		{
			aiAnimator.FpsScale = 1f;
		}
		if (target)
		{
			target.LocalTimeScale = 1f;
		}
		yield break;
	}

	// Token: 0x06007718 RID: 30488 RVA: 0x002F7D10 File Offset: 0x002F5F10
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400790F RID: 30991
	public float InTime;

	// Token: 0x04007910 RID: 30992
	public float HoldTime = 5f;

	// Token: 0x04007911 RID: 30993
	public float OutTime = 3f;

	// Token: 0x04007912 RID: 30994
	public float MaxTimeModifier = 0.25f;

	// Token: 0x04007913 RID: 30995
	public bool AllowStealing;
}
