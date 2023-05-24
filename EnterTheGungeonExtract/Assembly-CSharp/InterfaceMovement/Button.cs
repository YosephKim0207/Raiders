using System;
using UnityEngine;

namespace InterfaceMovement
{
	// Token: 0x0200067A RID: 1658
	public class Button : MonoBehaviour
	{
		// Token: 0x060025BF RID: 9663 RVA: 0x000A1B0C File Offset: 0x0009FD0C
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000A1B1C File Offset: 0x0009FD1C
		private void Update()
		{
			bool flag = base.transform.parent.GetComponent<ButtonManager>().focusedButton == this;
			Color color = this.cachedRenderer.material.color;
			color.a = Mathf.MoveTowards(color.a, (!flag) ? 0.5f : 1f, Time.deltaTime * 3f);
			this.cachedRenderer.material.color = color;
		}

		// Token: 0x040019B3 RID: 6579
		private Renderer cachedRenderer;

		// Token: 0x040019B4 RID: 6580
		public Button up;

		// Token: 0x040019B5 RID: 6581
		public Button down;

		// Token: 0x040019B6 RID: 6582
		public Button left;

		// Token: 0x040019B7 RID: 6583
		public Button right;
	}
}
