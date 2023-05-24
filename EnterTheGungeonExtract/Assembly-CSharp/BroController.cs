using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FFD RID: 4093
public class BroController : BraveBehaviour
{
	// Token: 0x06005981 RID: 22913 RVA: 0x00222CAC File Offset: 0x00220EAC
	public static void ClearPerLevelData()
	{
		StaticReferenceManager.AllBros.Clear();
	}

	// Token: 0x06005982 RID: 22914 RVA: 0x00222CB8 File Offset: 0x00220EB8
	public static BroController GetOtherBro(AIActor me)
	{
		return BroController.GetOtherBro(me.gameObject);
	}

	// Token: 0x06005983 RID: 22915 RVA: 0x00222CC8 File Offset: 0x00220EC8
	public static BroController GetOtherBro(GameObject me)
	{
		BroController broController = null;
		bool flag = false;
		List<BroController> allBros = StaticReferenceManager.AllBros;
		for (int i = 0; i < allBros.Count; i++)
		{
			if (allBros[i])
			{
				if (me == allBros[i].gameObject)
				{
					flag = true;
				}
				else
				{
					broController = allBros[i];
				}
			}
		}
		if (!flag)
		{
			Debug.LogWarning("Searched for a bro, but didn't find myself (" + me.name + ")", me);
		}
		return (!broController) ? null : broController;
	}

	// Token: 0x06005984 RID: 22916 RVA: 0x00222D68 File Offset: 0x00220F68
	public void Awake()
	{
		StaticReferenceManager.AllBros.Add(this);
	}

	// Token: 0x06005985 RID: 22917 RVA: 0x00222D78 File Offset: 0x00220F78
	public void Update()
	{
		if (!base.healthHaver.IsDead && this.m_shouldEnrage && base.behaviorSpeculator.IsInterruptable)
		{
			this.m_shouldEnrage = false;
			base.behaviorSpeculator.InterruptAndDisable();
			base.aiActor.ClearPath();
			base.StartCoroutine(this.EnrageCR());
		}
		if (this.m_isEnraged)
		{
			this.m_overheadVfxTimer += BraveTime.DeltaTime;
			if (this.m_overheadVfxInstance && this.m_overheadVfxTimer > 1.5f)
			{
				this.m_overheadVfxInstance.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
				this.m_overheadVfxInstance = null;
			}
			if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && base.aiActor && !base.aiActor.IsGone)
			{
				this.m_particleCounter += BraveTime.DeltaTime * 40f;
				if (this.m_particleCounter > 1f)
				{
					int num = Mathf.FloorToInt(this.m_particleCounter);
					this.m_particleCounter %= 1f;
					GlobalSparksDoer.DoRandomParticleBurst(num, base.aiActor.sprite.WorldBottomLeft.ToVector3ZisY(0f), base.aiActor.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
				}
			}
		}
	}

	// Token: 0x06005986 RID: 22918 RVA: 0x00222F20 File Offset: 0x00221120
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllBros.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x06005987 RID: 22919 RVA: 0x00222F34 File Offset: 0x00221134
	public void Enrage()
	{
		this.m_shouldEnrage = true;
	}

	// Token: 0x06005988 RID: 22920 RVA: 0x00222F40 File Offset: 0x00221140
	private IEnumerator EnrageCR()
	{
		if (base.healthHaver.GetCurrentHealthPercentage() < this.enrageHealToPercent)
		{
			base.healthHaver.ForceSetCurrentHealth(this.enrageHealToPercent * base.healthHaver.GetMaxHealth());
		}
		for (int i = 0; i < base.behaviorSpeculator.AttackBehaviors.Count; i++)
		{
			if (base.behaviorSpeculator.AttackBehaviors[i] is AttackBehaviorGroup)
			{
				this.ProcessAttackGroup(base.behaviorSpeculator.AttackBehaviors[i] as AttackBehaviorGroup);
			}
		}
		base.aiShooter.ToggleGunAndHandRenderers(false, "BroController");
		base.aiAnimator.PlayUntilFinished(this.enrageAnim, true, null, -1f, false);
		Color startingColor = base.aiActor.CurrentOverrideColor;
		float timer = 0f;
		this.m_isEnraged = false;
		while (timer < this.enrageAnimTime)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			base.aiActor.RegisterOverrideColor(Color.Lerp(startingColor, this.enrageColor, timer / this.enrageAnimTime), "BroEnrage");
			if (!this.m_isEnraged && timer / this.enrageAnimTime >= 0.5f)
			{
				if (this.enrageVfx)
				{
					GameObject gameObject = SpawnManager.SpawnVFX(this.enrageVfx, this.enrageVfxTransform.position, Quaternion.identity);
					this.m_enrageVfx = gameObject.GetComponent<tk2dBaseSprite>();
					this.m_enrageVfx.transform.parent = this.enrageVfxTransform;
					this.m_enrageVfx.HeightOffGround = 0.5f;
					base.sprite.AttachRenderer(this.m_enrageVfx);
					base.healthHaver.OnPreDeath += this.OnPreDeath;
				}
				if (this.overheadVfx)
				{
					this.m_overheadVfxInstance = base.aiActor.PlayEffectOnActor(this.overheadVfx, new Vector3(0f, 1.375f, 0f), true, true, false);
					this.m_overheadVfxTimer = 0f;
				}
				this.m_isEnraged = true;
			}
		}
		base.aiAnimator.EndAnimationIf(this.enrageAnim);
		base.aiShooter.ToggleGunAndHandRenderers(true, "BroController");
		if (this.postEnrageMoveSpeed >= 0f)
		{
			base.aiActor.MovementSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed(this.postEnrageMoveSpeed);
		}
		base.behaviorSpeculator.enabled = true;
		yield break;
	}

	// Token: 0x06005989 RID: 22921 RVA: 0x00222F5C File Offset: 0x0022115C
	private void ProcessAttackGroup(AttackBehaviorGroup attackGroup)
	{
		for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
		{
			AttackBehaviorGroup.AttackGroupItem attackGroupItem = attackGroup.AttackBehaviors[i];
			if (attackGroupItem.Behavior is AttackBehaviorGroup)
			{
				this.ProcessAttackGroup(attackGroupItem.Behavior as AttackBehaviorGroup);
			}
			else if (attackGroupItem.Behavior is ShootGunBehavior)
			{
				ShootGunBehavior shootGunBehavior = attackGroupItem.Behavior as ShootGunBehavior;
				if (shootGunBehavior.WeaponType == WeaponType.AIShooterProjectile)
				{
					attackGroupItem.Probability = 0f;
				}
				else if (shootGunBehavior.WeaponType == WeaponType.BulletScript)
				{
					attackGroupItem.Probability = 1f;
					shootGunBehavior.StopDuringAttack = false;
				}
			}
			else if (attackGroupItem.Behavior is SpawnReinforcementsBehavior)
			{
				if (attackGroupItem.Probability > 0f)
				{
					this.m_cachedSpawnProbability = attackGroupItem.Probability;
					attackGroupItem.Probability = 0f;
				}
				else
				{
					attackGroupItem.Probability = this.m_cachedSpawnProbability;
				}
			}
			else if (attackGroupItem.Behavior is ShootBehavior)
			{
				if (attackGroupItem.Probability > 0f)
				{
					attackGroupItem.Probability = 0f;
				}
				else
				{
					attackGroupItem.Probability = 1f;
				}
			}
		}
	}

	// Token: 0x0600598A RID: 22922 RVA: 0x0022309C File Offset: 0x0022129C
	private void OnPreDeath(Vector2 finalDeathDir)
	{
		if (this.m_enrageVfx)
		{
			SpawnManager.Despawn(this.m_enrageVfx.gameObject);
		}
	}

	// Token: 0x040052D8 RID: 21208
	public string enrageAnim;

	// Token: 0x040052D9 RID: 21209
	public float enrageAnimTime = 1f;

	// Token: 0x040052DA RID: 21210
	public Color enrageColor;

	// Token: 0x040052DB RID: 21211
	public GameObject enrageVfx;

	// Token: 0x040052DC RID: 21212
	public Transform enrageVfxTransform;

	// Token: 0x040052DD RID: 21213
	public GameObject overheadVfx;

	// Token: 0x040052DE RID: 21214
	public float postEnrageMoveSpeed = -1f;

	// Token: 0x040052DF RID: 21215
	public float enrageHealToPercent = 0.5f;

	// Token: 0x040052E0 RID: 21216
	private bool m_shouldEnrage;

	// Token: 0x040052E1 RID: 21217
	private float m_cachedSpawnProbability = 0.1f;

	// Token: 0x040052E2 RID: 21218
	private bool m_isEnraged;

	// Token: 0x040052E3 RID: 21219
	private GameObject m_overheadVfxInstance;

	// Token: 0x040052E4 RID: 21220
	private float m_overheadVfxTimer;

	// Token: 0x040052E5 RID: 21221
	private float m_particleCounter;

	// Token: 0x040052E6 RID: 21222
	private tk2dBaseSprite m_enrageVfx;
}
