using System;
using UnityEngine;

// Token: 0x02001325 RID: 4901
public class GunAttachmentUnbreaker : MonoBehaviour
{
	// Token: 0x06006F07 RID: 28423 RVA: 0x002C0610 File Offset: 0x002BE810
	private void Start()
	{
	}

	// Token: 0x06006F08 RID: 28424 RVA: 0x002C0614 File Offset: 0x002BE814
	private void Update()
	{
		if (base.gameObject.transform.position.y < 0f)
		{
			base.gameObject.transform.position = new Vector3(base.transform.position.x, Mathf.Abs(base.gameObject.transform.position.y), base.transform.position.z);
		}
	}
}
