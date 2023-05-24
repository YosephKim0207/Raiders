using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000FAA RID: 4010
public class BloodbulonController : BraveBehaviour
{
	// Token: 0x06005747 RID: 22343 RVA: 0x0021498C File Offset: 0x00212B8C
	public void Start()
	{
		base.healthHaver.minimumHealth = 0.5f;
		GoopDoer[] components = base.GetComponents<GoopDoer>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].updateTiming == GoopDoer.UpdateTiming.Always)
			{
				this.m_goopDoer = components[i];
				break;
			}
		}
		this.m_shadowAnimator = base.aiActor.ShadowObject.GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x002149F8 File Offset: 0x00212BF8
	public void Update()
	{
		if (!base.aiActor || !base.healthHaver || base.healthHaver.IsDead)
		{
			return;
		}
		if (!this.m_isTransitioning)
		{
			if (this.m_state == BloodbulonController.State.Small && base.healthHaver.GetCurrentHealthPercentage() <= 0.666f)
			{
				base.StartCoroutine(this.GetBigger());
			}
			else if (this.m_state == BloodbulonController.State.Medium && base.healthHaver.GetCurrentHealthPercentage() < 0.333f)
			{
				base.StartCoroutine(this.GetBigger());
			}
		}
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x00214AA4 File Offset: 0x00212CA4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600574A RID: 22346 RVA: 0x00214AAC File Offset: 0x00212CAC
	private IEnumerator GetBigger()
	{
		this.m_isTransitioning = true;
		float cachedResistance = base.aiActor.GetResistanceForEffectType(EffectResistanceType.Freeze);
		base.aiActor.SetResistance(EffectResistanceType.Freeze, 1000000f);
		string transformAnim = string.Empty;
		if (this.m_state == BloodbulonController.State.Small)
		{
			transformAnim = "bloodbulon_grow";
			base.aiAnimator.PlayUntilCancelled(transformAnim, false, null, -1f, false);
			this.m_shadowAnimator.Play("bloodbulon_grow");
			base.aiAnimator.IdleAnimation.Prefix = "bloodbulub_idle";
			for (int i = 0; i < 6; i++)
			{
				base.aiAnimator.MoveAnimation.AnimNames[i] = base.aiAnimator.MoveAnimation.AnimNames[i].Replace("bloodbulon", "bloodbulub");
			}
			base.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "pitfall").anim.Prefix = "bloodbulub_pitfall";
		}
		else if (this.m_state == BloodbulonController.State.Medium)
		{
			transformAnim = "bloodbulub_grow";
			base.aiAnimator.PlayUntilCancelled(transformAnim, false, null, -1f, false);
			this.m_shadowAnimator.Play("bloodbulub_grow");
			base.aiAnimator.IdleAnimation.Prefix = "blooduburst_idle";
			base.aiAnimator.MoveAnimation.Type = DirectionalAnimation.DirectionType.Single;
			base.aiAnimator.MoveAnimation.Prefix = "blooduburst_move";
			base.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "pitfall").anim.Prefix = "blooduburst_pitfall";
		}
		float startMoveSpeed = base.aiActor.MovementSpeed;
		float endMoveSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed((this.m_state != BloodbulonController.State.Small) ? this.bloodbuburstMoveSpeed : this.bloodbulubMoveSpeed);
		float startGoopSize = this.m_goopDoer.defaultGoopRadius;
		float endGoopSize = ((this.m_state != BloodbulonController.State.Small) ? this.bloodbuburstGoopSize : this.bloodbulubGoopSize);
		while (base.aiAnimator.IsPlaying(transformAnim))
		{
			base.aiActor.MovementSpeed = Mathf.Lerp(startMoveSpeed, endMoveSpeed, base.aiAnimator.CurrentClipProgress);
			this.m_goopDoer.defaultGoopRadius = Mathf.Lerp(startGoopSize, endGoopSize, base.aiAnimator.CurrentClipProgress);
			yield return null;
		}
		base.aiActor.MovementSpeed = endMoveSpeed;
		this.m_goopDoer.defaultGoopRadius = endGoopSize;
		base.aiAnimator.EndAnimation();
		if (this.m_state == BloodbulonController.State.Small)
		{
			base.specRigidbody.PixelColliders[1].SpecifyBagelFrame = "bloodbulub_idle_001";
		}
		else if (this.m_state == BloodbulonController.State.Medium)
		{
			base.specRigidbody.PixelColliders[1].SpecifyBagelFrame = "bloodbuburst_idle_001";
		}
		base.specRigidbody.ForceRegenerate(null, null);
		this.m_state++;
		if (this.m_state == BloodbulonController.State.Large)
		{
			base.healthHaver.minimumHealth = 0f;
		}
		if (base.aiActor)
		{
			base.aiActor.SetResistance(EffectResistanceType.Freeze, cachedResistance);
		}
		this.m_isTransitioning = false;
		yield break;
	}

	// Token: 0x0400503B RID: 20539
	public float bloodbulubMoveSpeed = 2.5f;

	// Token: 0x0400503C RID: 20540
	public float bloodbuburstMoveSpeed = 1.5f;

	// Token: 0x0400503D RID: 20541
	public float bloodbulubGoopSize = 1.25f;

	// Token: 0x0400503E RID: 20542
	public float bloodbuburstGoopSize = 2f;

	// Token: 0x0400503F RID: 20543
	private GoopDoer m_goopDoer;

	// Token: 0x04005040 RID: 20544
	private tk2dSpriteAnimator m_shadowAnimator;

	// Token: 0x04005041 RID: 20545
	private BloodbulonController.State m_state;

	// Token: 0x04005042 RID: 20546
	private bool m_isTransitioning;

	// Token: 0x02000FAB RID: 4011
	private enum State
	{
		// Token: 0x04005044 RID: 20548
		Small,
		// Token: 0x04005045 RID: 20549
		Medium,
		// Token: 0x04005046 RID: 20550
		Large
	}
}
