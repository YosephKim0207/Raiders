using System;
using UnityEngine;

// Token: 0x02000FD8 RID: 4056
public class BossFinalConvictDeathController : BraveBehaviour
{
	// Token: 0x06005878 RID: 22648 RVA: 0x0021D460 File Offset: 0x0021B660
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005879 RID: 22649 RVA: 0x0021D47C File Offset: 0x0021B67C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600587A RID: 22650 RVA: 0x0021D484 File Offset: 0x0021B684
	private void OnBossDeath(Vector2 dir)
	{
		ConvictPastController convictPastController = UnityEngine.Object.FindObjectOfType<ConvictPastController>();
		convictPastController.OnBossKilled(base.transform);
	}
}
