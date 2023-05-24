using System;
using UnityEngine;

// Token: 0x02000FFF RID: 4095
public class BulletBroDeathController : BraveBehaviour
{
	// Token: 0x06005992 RID: 22930 RVA: 0x002234A4 File Offset: 0x002216A4
	private void Start()
	{
		base.healthHaver.OnDeath += this.OnDeath;
	}

	// Token: 0x06005993 RID: 22931 RVA: 0x002234C0 File Offset: 0x002216C0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005994 RID: 22932 RVA: 0x002234C8 File Offset: 0x002216C8
	private void OnDeath(Vector2 finalDeathDir)
	{
		BroController otherBro = BroController.GetOtherBro(base.gameObject);
		if (otherBro)
		{
			otherBro.Enrage();
		}
		else
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_BULLET_BROS, true);
		}
	}
}
