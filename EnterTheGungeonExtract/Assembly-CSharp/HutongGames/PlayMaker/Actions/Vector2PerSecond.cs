using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B5B RID: 2907
	[Tooltip("Multiplies a Vector2 variable by Time.deltaTime. Useful for frame rate independent motion.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2PerSecond : FsmStateAction
	{
		// Token: 0x06003CF3 RID: 15603 RVA: 0x00131880 File Offset: 0x0012FA80
		public override void Reset()
		{
			this.vector2Variable = null;
			this.everyFrame = true;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00131890 File Offset: 0x0012FA90
		public override void OnEnter()
		{
			this.vector2Variable.Value = this.vector2Variable.Value * Time.deltaTime;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x001318C4 File Offset: 0x0012FAC4
		public override void OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Variable.Value * Time.deltaTime;
		}

		// Token: 0x04002F40 RID: 12096
		[RequiredField]
		[Tooltip("The Vector2")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F41 RID: 12097
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
