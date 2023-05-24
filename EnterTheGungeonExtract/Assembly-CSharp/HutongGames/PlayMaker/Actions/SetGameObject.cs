using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE6 RID: 2790
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the value of a Game Object Variable.")]
	public class SetGameObject : FsmStateAction
	{
		// Token: 0x06003B08 RID: 15112 RVA: 0x0012B3B0 File Offset: 0x001295B0
		public override void Reset()
		{
			this.variable = null;
			this.gameObject = null;
			this.everyFrame = false;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x0012B3C8 File Offset: 0x001295C8
		public override void OnEnter()
		{
			this.variable.Value = this.gameObject.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0012B3F4 File Offset: 0x001295F4
		public override void OnUpdate()
		{
			this.variable.Value = this.gameObject.Value;
		}

		// Token: 0x04002D56 RID: 11606
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject variable;

		// Token: 0x04002D57 RID: 11607
		public FsmGameObject gameObject;

		// Token: 0x04002D58 RID: 11608
		public bool everyFrame;
	}
}
