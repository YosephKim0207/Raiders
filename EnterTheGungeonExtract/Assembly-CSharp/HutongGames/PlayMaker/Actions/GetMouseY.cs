using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200099B RID: 2459
	[Tooltip("Gets the Y Position of the mouse and stores it in a Float Variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseY : FsmStateAction
	{
		// Token: 0x0600355E RID: 13662 RVA: 0x001130C4 File Offset: 0x001112C4
		public override void Reset()
		{
			this.storeResult = null;
			this.normalize = true;
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x001130D4 File Offset: 0x001112D4
		public override void OnEnter()
		{
			this.DoGetMouseY();
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x001130DC File Offset: 0x001112DC
		public override void OnUpdate()
		{
			this.DoGetMouseY();
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x001130E4 File Offset: 0x001112E4
		private void DoGetMouseY()
		{
			if (this.storeResult != null)
			{
				float num = Input.mousePosition.y;
				if (this.normalize)
				{
					num /= (float)Screen.height;
				}
				this.storeResult.Value = num;
			}
		}

		// Token: 0x040026B9 RID: 9913
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		// Token: 0x040026BA RID: 9914
		public bool normalize;
	}
}
