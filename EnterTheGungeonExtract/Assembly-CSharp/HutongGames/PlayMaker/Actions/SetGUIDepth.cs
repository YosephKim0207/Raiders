using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AED RID: 2797
	[Tooltip("Sets the sorting depth of subsequent GUI elements.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIDepth : FsmStateAction
	{
		// Token: 0x06003B21 RID: 15137 RVA: 0x0012B6D4 File Offset: 0x001298D4
		public override void Reset()
		{
			this.depth = 0;
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x0012B6E4 File Offset: 0x001298E4
		public override void OnPreprocess()
		{
			base.Fsm.HandleOnGUI = true;
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x0012B6F4 File Offset: 0x001298F4
		public override void OnGUI()
		{
			GUI.depth = this.depth.Value;
		}

		// Token: 0x04002D68 RID: 11624
		[RequiredField]
		public FsmInt depth;
	}
}
