using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200138F RID: 5007
public class CuccoController : CompanionController
{
	// Token: 0x0600717B RID: 29051 RVA: 0x002D1760 File Offset: 0x002CF960
	private void Start()
	{
		base.healthHaver.OnDamaged += this.HandleDamaged;
	}

	// Token: 0x0600717C RID: 29052 RVA: 0x002D177C File Offset: 0x002CF97C
	public override void Update()
	{
		base.Update();
		this.m_elapsed += BraveTime.DeltaTime;
		this.m_internalCooldown = Mathf.Max(0f, this.m_internalCooldown - BraveTime.DeltaTime);
		if (this.m_elapsed > this.HitDecayTime)
		{
			if (this.m_numRecentHits > 0)
			{
				this.m_numRecentHits--;
			}
			this.m_elapsed -= this.HitDecayTime;
		}
	}

	// Token: 0x0600717D RID: 29053 RVA: 0x002D17FC File Offset: 0x002CF9FC
	private void HandleDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		base.healthHaver.FullHeal();
		if (this.m_internalCooldown > 0f)
		{
			return;
		}
		AkSoundEngine.PostEvent("Play_PET_chicken_cluck_01", base.gameObject);
		this.m_numRecentHits++;
		if (PassiveItem.IsFlagSetAtAll(typeof(BattleStandardItem)) || (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsLuteCompanionBuff))
		{
			this.m_numRecentHits++;
		}
		if (this.m_numRecentHits >= this.HitsRequired)
		{
			base.StartCoroutine(this.HandleAggro());
		}
	}

	// Token: 0x0600717E RID: 29054 RVA: 0x002D18C0 File Offset: 0x002CFAC0
	private IEnumerator HandleAggro()
	{
		this.m_internalCooldown = this.InternalCooldown;
		float elapsed = 0f;
		base.aiAnimator.PlayForDuration("angry", this.SpawnDuration, false, null, -1f, false);
		AkSoundEngine.PostEvent("Play_PET_chicken_summon_01", base.gameObject);
		float cuccoElapsed = 0f;
		while (elapsed < this.SpawnDuration)
		{
			elapsed += BraveTime.DeltaTime;
			cuccoElapsed += BraveTime.DeltaTime;
			if (cuccoElapsed > this.SpawnDuration / (float)this.NumToSpawn)
			{
				cuccoElapsed -= this.SpawnDuration / (float)this.NumToSpawn;
				Vector2 vector = GameManager.Instance.MainCameraController.transform.position.XY() + UnityEngine.Random.insideUnitCircle.normalized * GameManager.Instance.MainCameraController.Camera.orthographicSize * 2f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MicroCuccoPrefab, vector, Quaternion.identity);
				gameObject.GetComponent<MicroCuccoController>().Initialize(this.m_owner);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400713A RID: 28986
	public int HitsRequired = 5;

	// Token: 0x0400713B RID: 28987
	public float HitDecayTime = 5f;

	// Token: 0x0400713C RID: 28988
	public int NumToSpawn = 20;

	// Token: 0x0400713D RID: 28989
	public float SpawnDuration = 5f;

	// Token: 0x0400713E RID: 28990
	public float InternalCooldown;

	// Token: 0x0400713F RID: 28991
	public GameObject MicroCuccoPrefab;

	// Token: 0x04007140 RID: 28992
	private float m_elapsed;

	// Token: 0x04007141 RID: 28993
	private int m_numRecentHits;

	// Token: 0x04007142 RID: 28994
	private float m_internalCooldown;
}
