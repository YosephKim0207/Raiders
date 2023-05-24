using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013D7 RID: 5079
public class ProjectileTrailDamageZone : MonoBehaviour
{
	// Token: 0x06007342 RID: 29506 RVA: 0x002DDC84 File Offset: 0x002DBE84
	public void OnSpawned()
	{
		base.StartCoroutine(this.HandleSpawnBehavior());
	}

	// Token: 0x06007343 RID: 29507 RVA: 0x002DDC94 File Offset: 0x002DBE94
	public IEnumerator HandleSpawnBehavior()
	{
		float elapsed = 0f;
		while (elapsed < this.delayTime)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		List<SpeculativeRigidbody> overlaps = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.GetComponent<SpeculativeRigidbody>(), null, false);
		for (int i = 0; i < overlaps.Count; i++)
		{
			if (overlaps[i])
			{
				AIActor component = overlaps[i].GetComponent<AIActor>();
				if (component && component.healthHaver)
				{
					component.healthHaver.ApplyDamage(this.damageToDeal, Vector2.zero, string.Empty, CoreDamageTypes.Fire, DamageCategory.Normal, false, null, false);
					if (this.AppliesFire)
					{
						component.ApplyEffect(this.FireEffect, 1f, null);
					}
				}
			}
		}
		yield return new WaitForSeconds(this.additionalDestroyTime);
		SpawnManager.Despawn(base.gameObject);
		yield break;
	}

	// Token: 0x040074CA RID: 29898
	public float delayTime = 0.5f;

	// Token: 0x040074CB RID: 29899
	public float additionalDestroyTime = 0.5f;

	// Token: 0x040074CC RID: 29900
	public float damageToDeal = 5f;

	// Token: 0x040074CD RID: 29901
	public bool AppliesFire;

	// Token: 0x040074CE RID: 29902
	public GameActorFireEffect FireEffect;
}
