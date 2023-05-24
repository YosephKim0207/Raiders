using System;
using UnityEngine;

// Token: 0x02001519 RID: 5401
public class ForceToSortingLayer : MonoBehaviour
{
	// Token: 0x06007B47 RID: 31559 RVA: 0x00316058 File Offset: 0x00314258
	private void OnEnable()
	{
		DepthLookupManager.AssignRendererToSortingLayer(base.GetComponent<Renderer>(), this.sortingLayer);
		if (this.targetSortingOrder != -1)
		{
			DepthLookupManager.UpdateRendererWithWorldYPosition(base.GetComponent<Renderer>(), base.transform.position.y);
		}
	}

	// Token: 0x04007DC9 RID: 32201
	public DepthLookupManager.GungeonSortingLayer sortingLayer;

	// Token: 0x04007DCA RID: 32202
	public int targetSortingOrder = -1;
}
