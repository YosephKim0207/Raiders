using System;
using UnityEngine;

// Token: 0x02001028 RID: 4136
public class GatlingGullDeathController : BraveBehaviour
{
	// Token: 0x06005AB4 RID: 23220 RVA: 0x0022A36C File Offset: 0x0022856C
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x0022A388 File Offset: 0x00228588
	protected override void OnDestroy()
	{
		if (GameManager.HasInstance)
		{
			this.Cleanup();
		}
		base.OnDestroy();
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x0022A3A0 File Offset: 0x002285A0
	private void OnBossDeath(Vector2 dir)
	{
		this.Cleanup();
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x0022A3A8 File Offset: 0x002285A8
	private void Cleanup()
	{
		SkyRocket[] array = UnityEngine.Object.FindObjectsOfType<SkyRocket>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DieInAir();
		}
	}
}
