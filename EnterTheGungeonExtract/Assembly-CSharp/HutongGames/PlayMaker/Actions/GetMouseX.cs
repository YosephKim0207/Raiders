using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200099A RID: 2458
	[Tooltip("Gets the X Position of the mouse and stores it in a Float Variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseX : FsmStateAction
	{
		// Token: 0x06003559 RID: 13657 RVA: 0x00113054 File Offset: 0x00111254
		public override void Reset()
		{
			this.storeResult = null;
			this.normalize = true;
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x00113064 File Offset: 0x00111264
		public override void OnEnter()
		{
			this.DoGetMouseX();
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x0011306C File Offset: 0x0011126C
		public override void OnUpdate()
		{
			this.DoGetMouseX();
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x00113074 File Offset: 0x00111274
		private void DoGetMouseX()
		{
			if (this.storeResult != null)
			{
				float num = Input.mousePosition.x;
				if (this.normalize)
				{
					num /= (float)Screen.width;
				}
				this.storeResult.Value = num;
			}
		}

		// Token: 0x040026B7 RID: 9911
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		// Token: 0x040026B8 RID: 9912
		public bool normalize;
	}
}
