using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016D3 RID: 5843
public class StickyFrictionManager : TimeInvariantMonoBehaviour
{
	// Token: 0x1700144E RID: 5198
	// (get) Token: 0x060087F0 RID: 34800 RVA: 0x00385EB0 File Offset: 0x003840B0
	public static StickyFrictionManager Instance
	{
		get
		{
			if (StickyFrictionManager.m_instance == null)
			{
				StickyFrictionManager.m_instance = (StickyFrictionManager)UnityEngine.Object.FindObjectOfType(typeof(StickyFrictionManager));
				if (StickyFrictionManager.m_instance == null)
				{
					GameObject gameObject = new GameObject("_TimRogers");
					StickyFrictionManager.m_instance = gameObject.AddComponent<StickyFrictionManager>();
				}
			}
			return StickyFrictionManager.m_instance;
		}
	}

	// Token: 0x060087F1 RID: 34801 RVA: 0x00385F14 File Offset: 0x00384114
	protected override void InvariantUpdate(float realDeltaTime)
	{
		GameManager.Instance.MainCameraController.CurrentStickyFriction = 1f;
		if (GameManager.Instance.IsPaused)
		{
			return;
		}
		if (!this.FrictionEnabled)
		{
			return;
		}
		if (this.m_fricts != null && this.m_fricts.Count > 0)
		{
			float num = 1f;
			for (int i = this.m_fricts.Count - 1; i >= 0; i--)
			{
				this.m_fricts[i].elapsed += realDeltaTime;
				if (this.m_fricts[i].elapsed >= 0f && this.m_fricts[i].elapsed < this.m_fricts[i].length)
				{
					num *= this.m_fricts[i].GetCurrentMagnitude();
				}
				else
				{
					this.m_fricts.RemoveAt(i);
				}
			}
			BraveTime.SetTimeScaleMultiplier(num, base.gameObject);
		}
	}

	// Token: 0x060087F2 RID: 34802 RVA: 0x00386020 File Offset: 0x00384220
	protected override void OnDestroy()
	{
		BraveTime.ClearMultiplier(base.gameObject);
		base.OnDestroy();
	}

	// Token: 0x060087F3 RID: 34803 RVA: 0x00386034 File Offset: 0x00384234
	public void RegisterSwordDamageStickyFriction(float damage)
	{
		this.RegisterDamageStickyFriction(damage, this.swordDamage);
	}

	// Token: 0x060087F4 RID: 34804 RVA: 0x00386044 File Offset: 0x00384244
	public void RegisterPlayerDamageStickyFriction(float damage)
	{
		this.RegisterDamageStickyFriction(damage, this.playerDamage);
	}

	// Token: 0x060087F5 RID: 34805 RVA: 0x00386054 File Offset: 0x00384254
	public void RegisterOtherDamageStickyFriction(float damage)
	{
		this.RegisterDamageStickyFriction(damage, this.enemyDamage);
	}

	// Token: 0x060087F6 RID: 34806 RVA: 0x00386064 File Offset: 0x00384264
	private void InternalRegisterFrict(StickyFrictionModifier newFrict, bool ignoreFrictReduction = false)
	{
		newFrict.magnitude = Mathf.Clamp01(Mathf.Max(newFrict.magnitude, this.m_currentMinFriction));
		this.m_fricts.Add(newFrict);
		if (!ignoreFrictReduction)
		{
			base.StartCoroutine(this.HandleAdditionalFrictReduction(newFrict));
		}
	}

	// Token: 0x060087F7 RID: 34807 RVA: 0x003860A4 File Offset: 0x003842A4
	private IEnumerator HandleAdditionalFrictReduction(StickyFrictionModifier newFrict)
	{
		float elapsed = 0f;
		float curContribution = 1f;
		this.m_currentMinFriction += curContribution;
		while (elapsed < 0.5f)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < 0.5f)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			this.m_currentMinFriction -= curContribution;
			curContribution = Mathf.Lerp(1f, 0f, elapsed / 0.5f);
			this.m_currentMinFriction += curContribution;
			yield return null;
		}
		this.m_currentMinFriction -= curContribution;
		if (this.m_currentMinFriction < 0f)
		{
			this.m_currentMinFriction = 0f;
		}
		yield break;
	}

	// Token: 0x060087F8 RID: 34808 RVA: 0x003860C0 File Offset: 0x003842C0
	public void RegisterExplosionStickyFriction()
	{
		this.InternalRegisterFrict(new StickyFrictionModifier(this.explosionFriction, 0f, false), false);
	}

	// Token: 0x060087F9 RID: 34809 RVA: 0x003860DC File Offset: 0x003842DC
	public void RegisterDeathStickyFriction()
	{
		this.InternalRegisterFrict(new StickyFrictionModifier(this.enemyDeathFriction, 0f, false), false);
	}

	// Token: 0x060087FA RID: 34810 RVA: 0x003860F8 File Offset: 0x003842F8
	public void RegisterCustomStickyFriction(float length, float magnitude, bool falloff, bool ignoreFrictReduction = false)
	{
		if (this.m_fricts.Count > 0)
		{
			this.m_fricts.Clear();
		}
		this.InternalRegisterFrict(new StickyFrictionModifier(length, magnitude, falloff), ignoreFrictReduction);
	}

	// Token: 0x060087FB RID: 34811 RVA: 0x00386128 File Offset: 0x00384328
	private void RegisterDamageStickyFriction(float damage, StickyFrictionManager.DamageFriction frictionData)
	{
		if (!frictionData.enabled)
		{
			return;
		}
		float num = Mathf.InverseLerp(frictionData.minDamage, frictionData.maxDamage, damage);
		float num2 = Mathf.Lerp(frictionData.minFriction, frictionData.maxFriction, num);
		this.InternalRegisterFrict(new StickyFrictionModifier(num2, 0f, false), false);
	}

	// Token: 0x04008D24 RID: 36132
	public bool FrictionEnabled = true;

	// Token: 0x04008D25 RID: 36133
	[Header("Damage")]
	public StickyFrictionManager.DamageFriction enemyDamage;

	// Token: 0x04008D26 RID: 36134
	public StickyFrictionManager.DamageFriction playerDamage;

	// Token: 0x04008D27 RID: 36135
	public StickyFrictionManager.DamageFriction swordDamage;

	// Token: 0x04008D28 RID: 36136
	[Header("Death")]
	public float enemyDeathFriction = 0.075f;

	// Token: 0x04008D29 RID: 36137
	[Header("Explosions")]
	public float explosionFriction = 0.1f;

	// Token: 0x04008D2A RID: 36138
	private const float FRICTION_REDUCTION_TIME = 0.5f;

	// Token: 0x04008D2B RID: 36139
	private const float FRICTION_REDUCTION_FALLOFF = 0.5f;

	// Token: 0x04008D2C RID: 36140
	private static StickyFrictionManager m_instance;

	// Token: 0x04008D2D RID: 36141
	private List<StickyFrictionModifier> m_fricts = new List<StickyFrictionModifier>();

	// Token: 0x04008D2E RID: 36142
	private float m_currentMinFriction;

	// Token: 0x020016D4 RID: 5844
	[Serializable]
	public class DamageFriction
	{
		// Token: 0x04008D2F RID: 36143
		public bool enabled;

		// Token: 0x04008D30 RID: 36144
		public float minFriction;

		// Token: 0x04008D31 RID: 36145
		public float maxFriction;

		// Token: 0x04008D32 RID: 36146
		public float minDamage;

		// Token: 0x04008D33 RID: 36147
		public float maxDamage;
	}
}
