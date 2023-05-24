using System;
using UnityEngine;

// Token: 0x02000FD4 RID: 4052
public class BossDoorMimicDeathController : BraveBehaviour
{
	// Token: 0x06005860 RID: 22624 RVA: 0x0021C8BC File Offset: 0x0021AABC
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x0021C8D8 File Offset: 0x0021AAD8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x0021C8E0 File Offset: 0x0021AAE0
	private void OnBossDeath(Vector2 dir)
	{
		BossDoorMimicIntroDoer component = base.GetComponent<BossDoorMimicIntroDoer>();
		if (component)
		{
			component.PhantomDoorBlocker.Unseal();
		}
	}
}
