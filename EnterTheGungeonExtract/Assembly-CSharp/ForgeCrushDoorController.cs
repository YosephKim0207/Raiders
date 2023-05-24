using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200115D RID: 4445
public class ForgeCrushDoorController : DungeonPlaceableBehaviour
{
	// Token: 0x060062A8 RID: 25256 RVA: 0x00263EE8 File Offset: 0x002620E8
	private void Start()
	{
		if (base.specRigidbody == null)
		{
			base.specRigidbody = base.GetComponentInChildren<SpeculativeRigidbody>();
		}
		if (base.spriteAnimator == null)
		{
			base.spriteAnimator = base.GetComponentInChildren<tk2dSpriteAnimator>();
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTrigger));
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		base.spriteAnimator.Sprite.UpdateZDepth();
	}

	// Token: 0x060062A9 RID: 25257 RVA: 0x00263F90 File Offset: 0x00262190
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060062AA RID: 25258 RVA: 0x00263F98 File Offset: 0x00262198
	private void HandleTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_isCrushing)
		{
			return;
		}
		this.m_isCrushing = true;
		base.StartCoroutine(this.HandleCrush());
	}

	// Token: 0x060062AB RID: 25259 RVA: 0x00263FBC File Offset: 0x002621BC
	private IEnumerator HandleCrush()
	{
		this.m_isCrushing = true;
		yield return new WaitForSeconds(this.DelayTime);
		base.spriteAnimator.Play(this.CloseAnimName);
		if (this.SubsidiaryAnimator != null)
		{
			this.SubsidiaryAnimator.Play(this.SubsidiaryCloseAnimName);
		}
		this.vfxAnimator.renderer.enabled = true;
		this.vfxAnimator.PlayAndDisableRenderer(string.Empty);
		while (base.spriteAnimator.IsPlaying(this.CloseAnimName))
		{
			yield return null;
		}
		yield return new WaitForSeconds(this.TimeClosed);
		base.spriteAnimator.Play(this.OpenAnimName);
		if (this.SubsidiaryAnimator != null)
		{
			this.SubsidiaryAnimator.Play(this.SubsidiaryOpenAnimName);
		}
		base.spriteAnimator.Sprite.UpdateZDepth();
		base.specRigidbody.PixelColliders[1].Enabled = false;
		while (base.spriteAnimator.IsPlaying(this.OpenAnimName))
		{
			base.spriteAnimator.Sprite.UpdateZDepth();
			yield return null;
		}
		base.spriteAnimator.Sprite.UpdateZDepth();
		yield return new WaitForSeconds(this.CooldownTime);
		this.m_isCrushing = false;
		yield break;
	}

	// Token: 0x060062AC RID: 25260 RVA: 0x00263FD8 File Offset: 0x002621D8
	private void HandleAnimationEvent(tk2dSpriteAnimator sourceAnimator, tk2dSpriteAnimationClip sourceClip, int sourceFrame)
	{
		if (sourceClip.frames[sourceFrame].eventInfo == "impact")
		{
			if (this.DoScreenShake)
			{
				GameManager.Instance.MainCameraController.DoScreenShake(this.ScreenShake, new Vector2?(base.specRigidbody.UnitCenter), false);
			}
			base.specRigidbody.PixelColliders[1].Enabled = true;
			base.specRigidbody.Reinitialize();
			Exploder.DoRadialMinorBreakableBreak(base.spriteAnimator.Sprite.WorldCenter, 1f);
			List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
			for (int i = 0; i < overlappingRigidbodies.Count; i++)
			{
				if (overlappingRigidbodies[i].gameActor)
				{
					Vector2 vector = overlappingRigidbodies[i].UnitCenter - base.specRigidbody.UnitCenter;
					if (overlappingRigidbodies[i].gameActor is PlayerController)
					{
						if (overlappingRigidbodies[i].healthHaver)
						{
							overlappingRigidbodies[i].healthHaver.ApplyDamage(0.5f, vector, StringTableManager.GetEnemiesString("#TRAP", -1), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						}
						if (overlappingRigidbodies[i].knockbackDoer)
						{
							overlappingRigidbodies[i].knockbackDoer.ApplyKnockback(vector, this.KnockbackForcePlayers, false);
						}
					}
					else
					{
						if (overlappingRigidbodies[i].healthHaver)
						{
							overlappingRigidbodies[i].healthHaver.ApplyDamage(this.DamageToEnemies, vector, StringTableManager.GetEnemiesString("#TRAP", -1), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						}
						if (overlappingRigidbodies[i].knockbackDoer)
						{
							overlappingRigidbodies[i].knockbackDoer.ApplyKnockback(vector, this.KnockbackForceEnemies, false);
						}
					}
				}
			}
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		}
	}

	// Token: 0x04005DB5 RID: 23989
	public float DamageToEnemies = 30f;

	// Token: 0x04005DB6 RID: 23990
	public float KnockbackForcePlayers = 50f;

	// Token: 0x04005DB7 RID: 23991
	public float KnockbackForceEnemies = 50f;

	// Token: 0x04005DB8 RID: 23992
	public bool DoScreenShake;

	// Token: 0x04005DB9 RID: 23993
	public ScreenShakeSettings ScreenShake;

	// Token: 0x04005DBA RID: 23994
	public string CloseAnimName;

	// Token: 0x04005DBB RID: 23995
	public string OpenAnimName;

	// Token: 0x04005DBC RID: 23996
	public tk2dSpriteAnimator SubsidiaryAnimator;

	// Token: 0x04005DBD RID: 23997
	public string SubsidiaryCloseAnimName;

	// Token: 0x04005DBE RID: 23998
	public string SubsidiaryOpenAnimName;

	// Token: 0x04005DBF RID: 23999
	public tk2dSpriteAnimator vfxAnimator;

	// Token: 0x04005DC0 RID: 24000
	public float DelayTime = 0.25f;

	// Token: 0x04005DC1 RID: 24001
	public float TimeClosed = 1f;

	// Token: 0x04005DC2 RID: 24002
	public float CooldownTime = 3f;

	// Token: 0x04005DC3 RID: 24003
	private bool m_isCrushing;
}
