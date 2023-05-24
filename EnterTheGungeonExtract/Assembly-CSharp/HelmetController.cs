using System;
using UnityEngine;

// Token: 0x020010A2 RID: 4258
public class HelmetController : BraveBehaviour
{
	// Token: 0x06005DE4 RID: 24036 RVA: 0x00240400 File Offset: 0x0023E600
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x06005DE5 RID: 24037 RVA: 0x0024041C File Offset: 0x0023E61C
	protected override void OnDestroy()
	{
		base.healthHaver.OnPreDeath -= this.OnPreDeath;
		base.OnDestroy();
	}

	// Token: 0x06005DE6 RID: 24038 RVA: 0x0024043C File Offset: 0x0023E63C
	public void OnPreDeath(Vector2 finalDamageDirection)
	{
		if (base.aiActor.IsFalling)
		{
			return;
		}
		if (this.helmetEffect != null)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.helmetEffect, base.specRigidbody.UnitTopLeft, Quaternion.identity);
			DebrisObject component = gameObject.GetComponent<DebrisObject>();
			if (component)
			{
				component.Trigger(finalDamageDirection.normalized * this.helmetForce, 1f, 1f);
			}
		}
	}

	// Token: 0x040057F1 RID: 22513
	public GameObject helmetEffect;

	// Token: 0x040057F2 RID: 22514
	public float helmetForce = 5f;
}
