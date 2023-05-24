using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001633 RID: 5683
public class DebrisMelter : BraveBehaviour
{
	// Token: 0x060084A8 RID: 33960 RVA: 0x0036A3E0 File Offset: 0x003685E0
	public void Start()
	{
		DebrisObject debris = base.debris;
		debris.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debris.OnGrounded, new Action<DebrisObject>(this.OnGrounded));
	}

	// Token: 0x060084A9 RID: 33961 RVA: 0x0036A40C File Offset: 0x0036860C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060084AA RID: 33962 RVA: 0x0036A414 File Offset: 0x00368614
	private void OnGrounded(DebrisObject debrisObject)
	{
		base.StartCoroutine(this.DoMeltCR());
	}

	// Token: 0x060084AB RID: 33963 RVA: 0x0036A424 File Offset: 0x00368624
	private IEnumerator DoMeltCR()
	{
		yield return new WaitForSeconds(this.delay);
		if (this.doesGoop)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goop).TimedAddGoopCircle(base.sprite.WorldCenter, this.goopRadius, this.meltTime, false);
		}
		for (float timer = this.meltTime; timer > 0f; timer -= BraveTime.DeltaTime)
		{
			base.transform.localScale = Vector3.one * (timer / this.meltTime);
			yield return null;
		}
		SpawnManager.Despawn(base.gameObject);
		yield break;
	}

	// Token: 0x04008857 RID: 34903
	public float delay;

	// Token: 0x04008858 RID: 34904
	public float meltTime;

	// Token: 0x04008859 RID: 34905
	public bool doesGoop;

	// Token: 0x0400885A RID: 34906
	[ShowInInspectorIf("doesGoop", false)]
	public GoopDefinition goop;

	// Token: 0x0400885B RID: 34907
	[ShowInInspectorIf("doesGoop", false)]
	public float goopRadius = 1f;
}
