using System;
using UnityEngine;

// Token: 0x020010C9 RID: 4297
public class TarnisherController : BraveBehaviour
{
	// Token: 0x06005EA6 RID: 24230 RVA: 0x00245B54 File Offset: 0x00243D54
	public void Awake()
	{
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x06005EA7 RID: 24231 RVA: 0x00245B70 File Offset: 0x00243D70
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.healthHaver.OnPreDeath -= this.OnPreDeath;
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x00245B90 File Offset: 0x00243D90
	private void OnPreDeath(Vector2 vector2)
	{
		base.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "pitfall").anim.Prefix = "pitfall_dead";
	}
}
