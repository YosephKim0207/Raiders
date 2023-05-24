using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009EC RID: 2540
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Adds a value to an Integer Variable.")]
	public class IntAdd : FsmStateAction
	{
		// Token: 0x0600368B RID: 13963 RVA: 0x00116EA8 File Offset: 0x001150A8
		public override void Reset()
		{
			this.intVariable = null;
			this.add = null;
			this.everyFrame = false;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x00116EC0 File Offset: 0x001150C0
		public override void OnEnter()
		{
			this.intVariable.Value += this.add.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x00116EF0 File Offset: 0x001150F0
		public override void OnUpdate()
		{
			this.intVariable.Value += this.add.Value;
		}

		// Token: 0x040027E6 RID: 10214
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		// Token: 0x040027E7 RID: 10215
		[RequiredField]
		public FsmInt add;

		// Token: 0x040027E8 RID: 10216
		public bool everyFrame;
	}
}
