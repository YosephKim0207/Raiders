using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B6A RID: 2922
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Multiplies a Vector3 variable by Time.deltaTime. Useful for frame rate independent motion.")]
	public class Vector3PerSecond : FsmStateAction
	{
		// Token: 0x06003D30 RID: 15664 RVA: 0x001326B8 File Offset: 0x001308B8
		public override void Reset()
		{
			this.vector3Variable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x001326C8 File Offset: 0x001308C8
		public override void OnEnter()
		{
			this.vector3Variable.Value = this.vector3Variable.Value * Time.deltaTime;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x001326FC File Offset: 0x001308FC
		public override void OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Variable.Value * Time.deltaTime;
		}

		// Token: 0x04002F85 RID: 12165
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F86 RID: 12166
		public bool everyFrame;
	}
}
