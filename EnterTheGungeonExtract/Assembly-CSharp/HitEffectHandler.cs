using System;
using System.Collections;
using UnityEngine;

// Token: 0x020010A3 RID: 4259
public class HitEffectHandler : BraveBehaviour
{
	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x06005DE8 RID: 24040 RVA: 0x002404D0 File Offset: 0x0023E6D0
	// (set) Token: 0x06005DE9 RID: 24041 RVA: 0x002404D8 File Offset: 0x0023E6D8
	public bool SuppressAdditionalHitEffects { get; set; }

	// Token: 0x06005DEA RID: 24042 RVA: 0x002404E4 File Offset: 0x0023E6E4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x002404EC File Offset: 0x0023E6EC
	public void HandleAdditionalHitEffects(Vector2 projVelocity, PixelCollider hitPixelCollider)
	{
		if (this.SuppressAdditionalHitEffects)
		{
			return;
		}
		for (int i = 0; i < this.additionalHitEffects.Length; i++)
		{
			HitEffectHandler.AdditionalHitEffect additionalHitEffect = this.additionalHitEffects[i];
			if (additionalHitEffect.delayTimer <= 0f)
			{
				if (additionalHitEffect.chance >= 1f || UnityEngine.Random.value <= additionalHitEffect.chance)
				{
					if (additionalHitEffect.specificPixelCollider)
					{
						int num = base.specRigidbody.PixelColliders.IndexOf(hitPixelCollider);
						if (additionalHitEffect.pixelColliderIndex != num)
						{
							goto IL_293;
						}
					}
					float num2 = ((!additionalHitEffect.flipNormals) ? projVelocity : (-projVelocity)).ToAngle();
					if (additionalHitEffect.spawnOnGround)
					{
						Vector2 vector = base.specRigidbody.UnitCenter + BraveMathCollege.DegreesToVector(num2 + UnityEngine.Random.Range(-additionalHitEffect.angleVariance, additionalHitEffect.angleVariance), UnityEngine.Random.Range(additionalHitEffect.minDistance, additionalHitEffect.maxDistance));
						additionalHitEffect.hitEffect.SpawnAtPosition(vector, num2, null, null, null, null, false, null, null, false);
					}
					else
					{
						Vector2 vector2 = ((!additionalHitEffect.transform) ? base.specRigidbody.GetUnitCenter(ColliderType.HitBox) : additionalHitEffect.transform.position.XY());
						vector2 = vector2 - base.transform.position.XY() + BraveMathCollege.DegreesToVector(num2, additionalHitEffect.radius);
						if (additionalHitEffect.doForce)
						{
							Vector2 vector3 = BraveMathCollege.DegreesToVector(num2 + UnityEngine.Random.Range(-additionalHitEffect.angleVariance, additionalHitEffect.angleVariance), UnityEngine.Random.Range(additionalHitEffect.minForce, additionalHitEffect.maxForce));
							vector3 += new Vector2(0f, additionalHitEffect.additionalVerticalForce);
							Vector2 vector4 = (Quaternion.Euler(0f, 0f, 90f) * vector3).normalized;
							additionalHitEffect.hitEffect.SpawnAtLocalPosition(vector2, num2, base.transform, new Vector2?(vector4), new Vector2?(vector3), false, null, false);
						}
						else
						{
							additionalHitEffect.hitEffect.SpawnAtLocalPosition(vector2, num2, base.transform, null, null, false, null, false);
						}
					}
					if (additionalHitEffect.delay > 0f)
					{
						additionalHitEffect.delayTimer = additionalHitEffect.delay;
						if (!this.m_isTrackingDelays)
						{
							base.StartCoroutine(this.TrackDelaysCR());
						}
					}
				}
			}
			IL_293:;
		}
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x002407A0 File Offset: 0x0023E9A0
	private IEnumerator TrackDelaysCR()
	{
		this.m_isTrackingDelays = true;
		for (;;)
		{
			for (int i = 0; i < this.additionalHitEffects.Length; i++)
			{
				this.additionalHitEffects[i].delayTimer = Mathf.Max(0f, this.additionalHitEffects[i].delayTimer - BraveTime.DeltaTime);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040057F3 RID: 22515
	public DungeonMaterial overrideMaterialDefinition;

	// Token: 0x040057F4 RID: 22516
	public VFXComplex overrideHitEffect;

	// Token: 0x040057F5 RID: 22517
	public VFXPool overrideHitEffectPool;

	// Token: 0x040057F6 RID: 22518
	public HitEffectHandler.AdditionalHitEffect[] additionalHitEffects;

	// Token: 0x040057F7 RID: 22519
	public bool SuppressAllHitEffects;

	// Token: 0x040057F9 RID: 22521
	private bool m_isTrackingDelays;

	// Token: 0x020010A4 RID: 4260
	[Serializable]
	public class AdditionalHitEffect
	{
		// Token: 0x040057FA RID: 22522
		public VFXPool hitEffect;

		// Token: 0x040057FB RID: 22523
		public float chance = 1f;

		// Token: 0x040057FC RID: 22524
		public Transform transform;

		// Token: 0x040057FD RID: 22525
		public bool flipNormals;

		// Token: 0x040057FE RID: 22526
		public float radius;

		// Token: 0x040057FF RID: 22527
		public float delay;

		// Token: 0x04005800 RID: 22528
		public float angleVariance;

		// Token: 0x04005801 RID: 22529
		public bool doForce;

		// Token: 0x04005802 RID: 22530
		[ShowInInspectorIf("doForce", true)]
		public float minForce;

		// Token: 0x04005803 RID: 22531
		[ShowInInspectorIf("doForce", true)]
		public float maxForce;

		// Token: 0x04005804 RID: 22532
		[ShowInInspectorIf("doForce", true)]
		public float additionalVerticalForce;

		// Token: 0x04005805 RID: 22533
		public bool spawnOnGround;

		// Token: 0x04005806 RID: 22534
		[ShowInInspectorIf("spawnOnGround", true)]
		public float minDistance;

		// Token: 0x04005807 RID: 22535
		[ShowInInspectorIf("spawnOnGround", true)]
		public float maxDistance;

		// Token: 0x04005808 RID: 22536
		public bool specificPixelCollider;

		// Token: 0x04005809 RID: 22537
		[ShowInInspectorIf("specificPixelCollider", false)]
		public int pixelColliderIndex;

		// Token: 0x0400580A RID: 22538
		[NonSerialized]
		public float delayTimer;
	}
}
