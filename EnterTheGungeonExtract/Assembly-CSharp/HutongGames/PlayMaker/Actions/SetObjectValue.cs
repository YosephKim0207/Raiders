using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B09 RID: 2825
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Sets the value of an Object Variable.")]
	public class SetObjectValue : FsmStateAction
	{
		// Token: 0x06003B96 RID: 15254 RVA: 0x0012C87C File Offset: 0x0012AA7C
		public override void Reset()
		{
			this.objectVariable = null;
			this.objectValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x0012C894 File Offset: 0x0012AA94
		public override void OnEnter()
		{
			this.objectVariable.Value = this.objectValue.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0012C8C0 File Offset: 0x0012AAC0
		public override void OnUpdate()
		{
			this.objectVariable.Value = this.objectValue.Value;
		}

		// Token: 0x04002DB7 RID: 11703
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmObject objectVariable;

		// Token: 0x04002DB8 RID: 11704
		[RequiredField]
		public FsmObject objectValue;

		// Token: 0x04002DB9 RID: 11705
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
