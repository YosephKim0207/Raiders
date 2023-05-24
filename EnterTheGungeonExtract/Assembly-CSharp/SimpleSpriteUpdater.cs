using System;
using UnityEngine;

// Token: 0x020016C3 RID: 5827
public class SimpleSpriteUpdater : MonoBehaviour
{
	// Token: 0x06008778 RID: 34680 RVA: 0x00382C4C File Offset: 0x00380E4C
	private void Start()
	{
		base.GetComponent<tk2dBaseSprite>().UpdateZDepth();
	}
}
