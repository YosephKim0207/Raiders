using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200114B RID: 4427
public class ElectricMushroom : DungeonPlaceableBehaviour
{
	// Token: 0x0600621E RID: 25118 RVA: 0x00260204 File Offset: 0x0025E404
	private void Awake()
	{
		this.EmissivePowerID = Shader.PropertyToID("_EmissivePower");
		this.AnimIndex = UnityEngine.Random.Range(0, this.ValidIdleAnims.Length);
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
		if (!StaticReferenceManager.MushroomMap.ContainsKey(intVector))
		{
			StaticReferenceManager.MushroomMap.Add(intVector, this);
		}
		else
		{
			Debug.LogError("Duplicate mushroom at: " + intVector);
		}
	}

	// Token: 0x0600621F RID: 25119 RVA: 0x00260280 File Offset: 0x0025E480
	private IEnumerator Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEnter));
		base.sprite.SetSprite(base.spriteAnimator.GetClipByName(this.ValidIdleAnims[this.AnimIndex]).frames[0].spriteId);
		yield return new WaitForSeconds(UnityEngine.Random.value);
		base.sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
		yield break;
	}

	// Token: 0x06006220 RID: 25120 RVA: 0x0026029C File Offset: 0x0025E49C
	private void Update()
	{
		if (!ElectricMushroom.m_updatedEmissiveThisFrame)
		{
			ElectricMushroom.m_updatedEmissiveThisFrame = true;
			base.sprite.renderer.sharedMaterial.SetFloat(this.EmissivePowerID, Mathf.Lerp(this.MinEmissive, this.MaxEmissive, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.realtimeSinceStartup * this.PulsesPerSecond, 1f))));
		}
	}

	// Token: 0x06006221 RID: 25121 RVA: 0x0026030C File Offset: 0x0025E50C
	private void LateUpdate()
	{
		ElectricMushroom.m_updatedEmissiveThisFrame = false;
	}

	// Token: 0x06006222 RID: 25122 RVA: 0x00260314 File Offset: 0x0025E514
	protected override void OnDestroy()
	{
		StaticReferenceManager.MushroomMap.Remove(base.transform.position.IntXY(VectorConversions.Round));
		base.OnDestroy();
	}

	// Token: 0x06006223 RID: 25123 RVA: 0x00260338 File Offset: 0x0025E538
	private void TriggerNearby()
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
		for (int i = 0; i < 4; i++)
		{
			if (StaticReferenceManager.MushroomMap.ContainsKey(intVector + IntVector2.Cardinals[i]))
			{
				StaticReferenceManager.MushroomMap[intVector + IntVector2.Cardinals[i]].Trigger(false);
			}
		}
	}

	// Token: 0x06006224 RID: 25124 RVA: 0x002603B4 File Offset: 0x0025E5B4
	public void Trigger(bool IsPrimaryTarget = false)
	{
		if (this.m_fireCooldownTime > 0f)
		{
			if (IsPrimaryTarget)
			{
				base.StartCoroutine(this.FrameDelayedBreak());
			}
		}
		else
		{
			base.StartCoroutine(this.Trigger_CR(IsPrimaryTarget));
		}
	}

	// Token: 0x06006225 RID: 25125 RVA: 0x002603EC File Offset: 0x0025E5EC
	public IEnumerator Trigger_CR(bool IsPrimaryTarget = false)
	{
		this.m_fireCooldownTime = 0.5f;
		this.m_remainingFireTime = 2f;
		if (!IsPrimaryTarget)
		{
			base.spriteAnimator.PlayForDuration(this.ValidIdleAnims[this.AnimIndex], 2f);
		}
		this.Electrify();
		if (!this.m_isFiring)
		{
			base.StartCoroutine(this.HandleFiring());
		}
		yield return new WaitForSeconds(0.1f);
		this.TriggerNearby();
		if (IsPrimaryTarget)
		{
			base.StartCoroutine(this.FrameDelayedBreak());
		}
		yield break;
	}

	// Token: 0x06006226 RID: 25126 RVA: 0x00260410 File Offset: 0x0025E610
	private IEnumerator FrameDelayedBreak()
	{
		yield return null;
		base.minorBreakable.Break();
		yield break;
	}

	// Token: 0x06006227 RID: 25127 RVA: 0x0026042C File Offset: 0x0025E62C
	private IEnumerator HandleFiring()
	{
		this.m_isFiring = true;
		while (this.m_remainingFireTime > 0f)
		{
			this.m_fireCooldownTime -= BraveTime.DeltaTime;
			this.m_remainingFireTime -= BraveTime.DeltaTime;
			yield return null;
		}
		this.m_isFiring = false;
		this.m_fireCooldownTime = 0f;
		this.m_remainingFireTime = 0f;
		base.spriteAnimator.StopAndResetFrame();
		yield break;
	}

	// Token: 0x06006228 RID: 25128 RVA: 0x00260448 File Offset: 0x0025E648
	public void Electrify()
	{
		this.ElectrifyVFX.renderer.enabled = true;
		this.ElectrifyVFX.PlayAndDisableRenderer(string.Empty);
		for (int i = 0; i < base.specRigidbody.PrimaryPixelCollider.TriggerCollisions.Count; i++)
		{
			TriggerCollisionData triggerCollisionData = base.specRigidbody.PrimaryPixelCollider.TriggerCollisions[i];
			if (triggerCollisionData.SpecRigidbody.gameActor != null && !triggerCollisionData.SpecRigidbody.gameActor.IsFlying)
			{
				if (triggerCollisionData.SpecRigidbody.gameActor is AIActor)
				{
					if (triggerCollisionData.SpecRigidbody.healthHaver)
					{
						triggerCollisionData.SpecRigidbody.healthHaver.ApplyDamage(this.DamageToEnemies, Vector2.zero, StringTableManager.GetEnemiesString("#MUSHROOM", -1), CoreDamageTypes.Electric, DamageCategory.Environment, false, null, false);
					}
				}
				else if (triggerCollisionData.SpecRigidbody.healthHaver)
				{
					triggerCollisionData.SpecRigidbody.healthHaver.ApplyDamage(0.5f, Vector2.zero, StringTableManager.GetEnemiesString("#MUSHROOM", -1), CoreDamageTypes.Electric, DamageCategory.Environment, false, null, false);
				}
			}
		}
	}

	// Token: 0x06006229 RID: 25129 RVA: 0x0026057C File Offset: 0x0025E77C
	private void HandleTriggerEnter(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (specRigidbody.gameActor != null && !specRigidbody.gameActor.IsFlying)
		{
			if (!specRigidbody.spriteAnimator && specRigidbody.spriteAnimator.QueryGroundedFrame())
			{
				return;
			}
			base.spriteAnimator.PlayForDuration(this.ValidHitAnims[this.AnimIndex], -1f, this.ValidIdleAnims[this.AnimIndex], false);
			this.Trigger(true);
		}
	}

	// Token: 0x04005D05 RID: 23813
	public string[] ValidIdleAnims;

	// Token: 0x04005D06 RID: 23814
	public string[] ValidHitAnims;

	// Token: 0x04005D07 RID: 23815
	public float MinEmissive = 10f;

	// Token: 0x04005D08 RID: 23816
	public float MaxEmissive = 30f;

	// Token: 0x04005D09 RID: 23817
	public float PulsesPerSecond = 1f;

	// Token: 0x04005D0A RID: 23818
	public float DamageToEnemies = 6f;

	// Token: 0x04005D0B RID: 23819
	public tk2dSpriteAnimator ElectrifyVFX;

	// Token: 0x04005D0C RID: 23820
	private int AnimIndex = -1;

	// Token: 0x04005D0D RID: 23821
	private int EmissivePowerID = -1;

	// Token: 0x04005D0E RID: 23822
	private static bool m_updatedEmissiveThisFrame;

	// Token: 0x04005D0F RID: 23823
	private bool m_isFiring;

	// Token: 0x04005D10 RID: 23824
	private float m_fireCooldownTime;

	// Token: 0x04005D11 RID: 23825
	private float m_remainingFireTime;
}
