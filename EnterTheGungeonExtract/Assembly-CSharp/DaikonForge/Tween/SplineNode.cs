using System;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x0200052C RID: 1324
	[ExecuteInEditMode]
	public class SplineNode : MonoBehaviour
	{
		// Token: 0x06001FCC RID: 8140 RVA: 0x0008E88C File Offset: 0x0008CA8C
		public void OnDestroy()
		{
			if (Application.isPlaying || base.transform.parent == null)
			{
				return;
			}
			SplineObject component = base.transform.parent.GetComponent<SplineObject>();
			if (component == null)
			{
				return;
			}
			component.ControlPoints.Remove(base.transform);
		}
	}
}
