using System;
using UnityEngine;

// Token: 0x02000FDF RID: 4063
public class BossFinalMarineLavaController : BraveBehaviour
{
	// Token: 0x060058A0 RID: 22688 RVA: 0x0021E128 File Offset: 0x0021C328
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		this.m_dimensionFog = UnityEngine.Object.FindObjectOfType<DimensionFogController>();
	}

	// Token: 0x060058A1 RID: 22689 RVA: 0x0021E15C File Offset: 0x0021C35C
	private void OnTriggerCollision(SpeculativeRigidbody speculativeRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		PlayerController component = speculativeRigidbody.GetComponent<PlayerController>();
		Vector2 unitCenter = component.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (component.spriteAnimator.QueryGroundedFrame() && Vector2.Distance(unitCenter, this.m_dimensionFog.transform.position) < this.m_dimensionFog.ApparentRadius)
		{
			component.IncreasePoison(BraveTime.DeltaTime * 1.5f);
			if (component.CurrentPoisonMeterValue >= 1f)
			{
				component.healthHaver.ApplyDamage(0.5f, Vector2.zero, StringTableManager.GetEnemiesString("#GOOP", -1), CoreDamageTypes.Poison, DamageCategory.Environment, true, null, false);
				component.CurrentPoisonMeterValue = 0f;
			}
		}
	}

	// Token: 0x040051D0 RID: 20944
	public GoopDefinition goopDefinition;

	// Token: 0x040051D1 RID: 20945
	private DimensionFogController m_dimensionFog;
}
