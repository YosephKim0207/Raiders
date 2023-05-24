using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001640 RID: 5696
public class DevolverModifier : MonoBehaviour
{
	// Token: 0x0600850B RID: 34059 RVA: 0x0036DA88 File Offset: 0x0036BC88
	private void Start()
	{
		Projectile component = base.GetComponent<Projectile>();
		if (component)
		{
			Projectile projectile = component;
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		}
	}

	// Token: 0x0600850C RID: 34060 RVA: 0x0036DACC File Offset: 0x0036BCCC
	private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody enemyRigidbody, bool killingBlow)
	{
		if (killingBlow)
		{
			return;
		}
		if (!enemyRigidbody || !enemyRigidbody.aiActor)
		{
			return;
		}
		if (UnityEngine.Random.value > this.chanceToDevolve)
		{
			return;
		}
		AIActor aiActor = enemyRigidbody.aiActor;
		if (!aiActor.IsNormalEnemy || aiActor.IsHarmlessEnemy || aiActor.healthHaver.IsBoss)
		{
			return;
		}
		string enemyGuid = aiActor.EnemyGuid;
		for (int i = 0; i < this.EnemyGuidsToIgnore.Count; i++)
		{
			if (this.EnemyGuidsToIgnore[i] == enemyGuid)
			{
				return;
			}
		}
		int num = this.DevolverHierarchy.Count - 1;
		for (int j = 0; j < this.DevolverHierarchy.Count; j++)
		{
			List<string> tierGuids = this.DevolverHierarchy[j].tierGuids;
			for (int k = 0; k < tierGuids.Count; k++)
			{
				if (tierGuids[k] == enemyGuid)
				{
					num = j - 1;
					break;
				}
			}
		}
		if (num >= 0 && num < this.DevolverHierarchy.Count)
		{
			List<string> tierGuids2 = this.DevolverHierarchy[num].tierGuids;
			string text = tierGuids2[UnityEngine.Random.Range(0, tierGuids2.Count)];
			aiActor.Transmogrify(EnemyDatabase.GetOrLoadByGuid(text), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			AkSoundEngine.PostEvent("Play_WPN_devolver_morph_01", base.gameObject);
		}
	}

	// Token: 0x040088E9 RID: 35049
	public float chanceToDevolve = 0.1f;

	// Token: 0x040088EA RID: 35050
	public List<DevolverTier> DevolverHierarchy = new List<DevolverTier>();

	// Token: 0x040088EB RID: 35051
	public List<string> EnemyGuidsToIgnore = new List<string>();
}
