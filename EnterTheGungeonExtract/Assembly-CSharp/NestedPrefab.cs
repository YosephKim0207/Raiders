using System;
using UnityEngine;

// Token: 0x020015A7 RID: 5543
public class NestedPrefab : BraveBehaviour
{
	// Token: 0x06007F3A RID: 32570 RVA: 0x003362D4 File Offset: 0x003344D4
	public void Awake()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, base.transform.position, Quaternion.identity);
		gameObject.transform.parent = base.transform;
		if (this.localScale != Vector3.zero)
		{
			gameObject.transform.localScale = this.localScale;
		}
		if (this.localRotation != Vector3.zero)
		{
			gameObject.transform.localRotation = Quaternion.Euler(this.localRotation);
		}
		if (this.localScale != Vector3.one)
		{
			gameObject.transform.localScale = this.localScale;
		}
	}

	// Token: 0x06007F3B RID: 32571 RVA: 0x00336388 File Offset: 0x00334588
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040081EC RID: 33260
	public Vector3 localPosition = Vector3.zero;

	// Token: 0x040081ED RID: 33261
	public Vector3 localRotation = Vector3.zero;

	// Token: 0x040081EE RID: 33262
	public Vector3 localScale = Vector3.one;

	// Token: 0x040081EF RID: 33263
	public GameObject prefab;
}
