using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200139A RID: 5018
public class DirectionalAttackActiveItem : PlayerItem
{
	// Token: 0x060071A8 RID: 29096 RVA: 0x002D28DC File Offset: 0x002D0ADC
	public override void Update()
	{
		base.Update();
		if (this.m_currentUser && this.m_currentUser.HasActiveBonusSynergy(CustomSynergyType.EXPLOSIVE_AIRSTRIKE, false) && this.itemName == "Airstrike" && !this.m_airstrikeSynergyProcessed)
		{
			this.m_airstrikeSynergyProcessed = true;
			this.BarrageColumns = 3;
			this.initialWidth *= 3f;
			this.finalWidth *= 3f;
		}
		else if (this.m_currentUser && !this.m_currentUser.HasActiveBonusSynergy(CustomSynergyType.EXPLOSIVE_AIRSTRIKE, false) && this.m_airstrikeSynergyProcessed)
		{
			this.m_airstrikeSynergyProcessed = false;
			this.BarrageColumns = 1;
			this.initialWidth /= 3f;
			this.finalWidth /= 3f;
		}
		if (base.IsCurrentlyActive && this.m_extantReticleQuad)
		{
			Vector2 centerPosition = this.m_currentUser.CenterPosition;
			Vector2 normalized = (this.m_currentUser.unadjustedAimPoint.XY() - centerPosition).normalized;
			this.m_extantReticleQuad.transform.localRotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(normalized));
			Vector2 vector = centerPosition + normalized * this.startDistance + (Quaternion.Euler(0f, 0f, -90f) * normalized * (this.initialWidth / 2f)).XY();
			this.m_extantReticleQuad.transform.position = vector;
		}
	}

	// Token: 0x060071A9 RID: 29097 RVA: 0x002D2A98 File Offset: 0x002D0C98
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		if (this.m_extantReticleQuad)
		{
			UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
		}
	}

	// Token: 0x060071AA RID: 29098 RVA: 0x002D2AC4 File Offset: 0x002D0CC4
	protected override void DoEffect(PlayerController user)
	{
		base.IsCurrentlyActive = true;
		this.m_currentUser = user;
		Vector2 centerPosition = user.CenterPosition;
		Vector2 normalized = (user.unadjustedAimPoint.XY() - centerPosition).normalized;
		if (this.m_currentUser && this.m_currentUser.HasActiveBonusSynergy(CustomSynergyType.EXPLOSIVE_AIRSTRIKE, false) && this.itemName == "Airstrike" && !this.m_airstrikeSynergyProcessed)
		{
			this.m_airstrikeSynergyProcessed = true;
			this.BarrageColumns = 3;
			this.initialWidth *= 3f;
			this.finalWidth *= 3f;
		}
		else if (this.m_currentUser && !this.m_currentUser.HasActiveBonusSynergy(CustomSynergyType.EXPLOSIVE_AIRSTRIKE, false) && this.m_airstrikeSynergyProcessed)
		{
			this.m_airstrikeSynergyProcessed = false;
			this.BarrageColumns = 1;
			this.initialWidth /= 3f;
			this.finalWidth /= 3f;
		}
		if (this.SkipTargetingStep)
		{
			this.DoActiveEffect(user);
		}
		else
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.reticleQuad);
			this.m_extantReticleQuad = gameObject.GetComponent<tk2dSlicedSprite>();
			this.m_extantReticleQuad.dimensions = new Vector2(this.attackLength * 16f, this.initialWidth * 16f);
			if (normalized != Vector2.zero)
			{
				this.m_extantReticleQuad.transform.localRotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(normalized));
			}
			Vector2 vector = centerPosition + normalized * this.startDistance + (Quaternion.Euler(0f, 0f, -90f) * normalized * (this.initialWidth / 2f)).XY();
			this.m_extantReticleQuad.transform.position = vector;
		}
	}

	// Token: 0x060071AB RID: 29099 RVA: 0x002D2CCC File Offset: 0x002D0ECC
	protected override void DoActiveEffect(PlayerController user)
	{
		if (this.m_isDoingBarrage)
		{
			return;
		}
		if (this.m_extantReticleQuad)
		{
			UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
		}
		Vector2 vector = user.CenterPosition;
		Vector2 normalized = (user.unadjustedAimPoint.XY() - vector).normalized;
		vector += normalized * this.startDistance;
		if (this.doesGoop)
		{
			this.HandleEngoopening(vector, normalized);
		}
		if (this.doesBarrage)
		{
			List<Vector2> list = this.AcquireBarrageTargets(vector, normalized);
			user.StartCoroutine(this.HandleBarrage(list));
		}
		else
		{
			base.IsCurrentlyActive = false;
		}
		if (!string.IsNullOrEmpty(this.AudioEvent))
		{
			AkSoundEngine.PostEvent(this.AudioEvent, base.gameObject);
		}
	}

	// Token: 0x060071AC RID: 29100 RVA: 0x002D2D9C File Offset: 0x002D0F9C
	protected void HandleEngoopening(Vector2 startPoint, Vector2 direction)
	{
		float num = 1f;
		DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
		goopManagerForGoopType.TimedAddGoopLine(startPoint, startPoint + direction * this.attackLength, this.barrageRadius, num);
	}

	// Token: 0x060071AD RID: 29101 RVA: 0x002D2DDC File Offset: 0x002D0FDC
	private IEnumerator HandleBarrage(List<Vector2> targets)
	{
		this.m_isDoingBarrage = true;
		while (targets.Count > 0)
		{
			Vector2 currentTarget = targets[0];
			targets.RemoveAt(0);
			Exploder.Explode(currentTarget, this.barrageExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			yield return new WaitForSeconds(this.delayBetweenStrikes / (float)this.BarrageColumns);
		}
		yield return new WaitForSeconds(0.25f);
		this.m_isDoingBarrage = false;
		base.IsCurrentlyActive = false;
		yield break;
	}

	// Token: 0x060071AE RID: 29102 RVA: 0x002D2E00 File Offset: 0x002D1000
	protected List<Vector2> AcquireBarrageTargets(Vector2 startPoint, Vector2 direction)
	{
		List<Vector2> list = new List<Vector2>();
		float num = -this.barrageRadius / 2f;
		float num2 = BraveMathCollege.Atan2Degrees(direction);
		Quaternion quaternion = Quaternion.Euler(0f, 0f, num2);
		while (num < this.attackLength)
		{
			float num3 = Mathf.Clamp01(num / this.attackLength);
			float num4 = Mathf.Lerp(this.initialWidth, this.finalWidth, num3);
			float num5 = Mathf.Clamp(num, 0f, this.attackLength);
			for (int i = 0; i < this.BarrageColumns; i++)
			{
				float num6 = Mathf.Lerp(-num4, num4, ((float)i + 1f) / ((float)this.BarrageColumns + 1f));
				float num7 = UnityEngine.Random.Range(-num4 / (4f * (float)this.BarrageColumns), num4 / (4f * (float)this.BarrageColumns));
				Vector2 vector = new Vector2(num5, num6 + num7);
				Vector2 vector2 = (quaternion * vector).XY();
				list.Add(startPoint + vector2);
			}
			num += this.barrageRadius;
		}
		return list;
	}

	// Token: 0x060071AF RID: 29103 RVA: 0x002D2F24 File Offset: 0x002D1124
	protected override void OnDestroy()
	{
		if (this.m_extantReticleQuad)
		{
			UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
		}
		base.OnDestroy();
	}

	// Token: 0x04007314 RID: 29460
	public float initialWidth = 3f;

	// Token: 0x04007315 RID: 29461
	public float finalWidth = 3f;

	// Token: 0x04007316 RID: 29462
	public float startDistance = 1f;

	// Token: 0x04007317 RID: 29463
	public float attackLength = 10f;

	// Token: 0x04007318 RID: 29464
	public GameObject reticleQuad;

	// Token: 0x04007319 RID: 29465
	public bool doesGoop;

	// Token: 0x0400731A RID: 29466
	public GoopDefinition goopDefinition;

	// Token: 0x0400731B RID: 29467
	public bool doesBarrage;

	// Token: 0x0400731C RID: 29468
	public int BarrageColumns = 1;

	// Token: 0x0400731D RID: 29469
	public GameObject barrageVFX;

	// Token: 0x0400731E RID: 29470
	public ExplosionData barrageExplosionData;

	// Token: 0x0400731F RID: 29471
	public float barrageRadius = 1.5f;

	// Token: 0x04007320 RID: 29472
	public float delayBetweenStrikes = 0.25f;

	// Token: 0x04007321 RID: 29473
	public bool SkipTargetingStep;

	// Token: 0x04007322 RID: 29474
	public string AudioEvent;

	// Token: 0x04007323 RID: 29475
	private PlayerController m_currentUser;

	// Token: 0x04007324 RID: 29476
	private tk2dSlicedSprite m_extantReticleQuad;

	// Token: 0x04007325 RID: 29477
	private bool m_airstrikeSynergyProcessed;

	// Token: 0x04007326 RID: 29478
	private bool m_isDoingBarrage;
}
