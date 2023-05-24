using System;
using UnityEngine;

// Token: 0x0200120F RID: 4623
public class SimpleRenamer : MonoBehaviour
{
	// Token: 0x0600676D RID: 26477 RVA: 0x00287D2C File Offset: 0x00285F2C
	public void Start()
	{
		base.name = this.OverrideName;
	}

	// Token: 0x0400634F RID: 25423
	public string OverrideName;
}
