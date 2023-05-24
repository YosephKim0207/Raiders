using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C2 RID: 2498
	[Tooltip("GUI base action - don't use!")]
	public abstract class GUIAction : FsmStateAction
	{
		// Token: 0x06003606 RID: 13830 RVA: 0x00114C24 File Offset: 0x00112E24
		public override void Reset()
		{
			this.screenRect = null;
			this.left = 0f;
			this.top = 0f;
			this.width = 1f;
			this.height = 1f;
			this.normalized = true;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x00114C84 File Offset: 0x00112E84
		public override void OnGUI()
		{
			this.rect = (this.screenRect.IsNone ? default(Rect) : this.screenRect.Value);
			if (!this.left.IsNone)
			{
				this.rect.x = this.left.Value;
			}
			if (!this.top.IsNone)
			{
				this.rect.y = this.top.Value;
			}
			if (!this.width.IsNone)
			{
				this.rect.width = this.width.Value;
			}
			if (!this.height.IsNone)
			{
				this.rect.height = this.height.Value;
			}
			if (this.normalized.Value)
			{
				this.rect.x = this.rect.x * (float)Screen.width;
				this.rect.width = this.rect.width * (float)Screen.width;
				this.rect.y = this.rect.y * (float)Screen.height;
				this.rect.height = this.rect.height * (float)Screen.height;
			}
		}

		// Token: 0x0400274B RID: 10059
		[UIHint(UIHint.Variable)]
		public FsmRect screenRect;

		// Token: 0x0400274C RID: 10060
		public FsmFloat left;

		// Token: 0x0400274D RID: 10061
		public FsmFloat top;

		// Token: 0x0400274E RID: 10062
		public FsmFloat width;

		// Token: 0x0400274F RID: 10063
		public FsmFloat height;

		// Token: 0x04002750 RID: 10064
		[RequiredField]
		public FsmBool normalized;

		// Token: 0x04002751 RID: 10065
		internal Rect rect;
	}
}
