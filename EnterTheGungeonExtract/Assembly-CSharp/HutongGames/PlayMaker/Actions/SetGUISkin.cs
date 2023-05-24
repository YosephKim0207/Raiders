using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AEE RID: 2798
	[Tooltip("Sets the GUISkin used by GUI elements.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUISkin : FsmStateAction
	{
		// Token: 0x06003B25 RID: 15141 RVA: 0x0012B710 File Offset: 0x00129910
		public override void Reset()
		{
			this.skin = null;
			this.applyGlobally = true;
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x0012B728 File Offset: 0x00129928
		public override void OnGUI()
		{
			if (this.skin != null)
			{
				GUI.skin = this.skin;
			}
			if (this.applyGlobally.Value)
			{
				PlayMakerGUI.GUISkin = this.skin;
				base.Finish();
			}
		}

		// Token: 0x04002D69 RID: 11625
		[RequiredField]
		public GUISkin skin;

		// Token: 0x04002D6A RID: 11626
		public FsmBool applyGlobally;
	}
}
