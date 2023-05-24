using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B5F RID: 2911
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Adds a XYZ values to Vector3 Variable.")]
	public class Vector3AddXYZ : FsmStateAction
	{
		// Token: 0x06003D04 RID: 15620 RVA: 0x00131BD4 File Offset: 0x0012FDD4
		public override void Reset()
		{
			this.vector3Variable = null;
			this.addX = 0f;
			this.addY = 0f;
			this.addZ = 0f;
			this.everyFrame = false;
			this.perSecond = false;
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x00131C28 File Offset: 0x0012FE28
		public override void OnEnter()
		{
			this.DoVector3AddXYZ();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x00131C44 File Offset: 0x0012FE44
		public override void OnUpdate()
		{
			this.DoVector3AddXYZ();
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x00131C4C File Offset: 0x0012FE4C
		private void DoVector3AddXYZ()
		{
			Vector3 vector = new Vector3(this.addX.Value, this.addY.Value, this.addZ.Value);
			if (this.perSecond)
			{
				this.vector3Variable.Value += vector * Time.deltaTime;
			}
			else
			{
				this.vector3Variable.Value += vector;
			}
		}

		// Token: 0x04002F4E RID: 12110
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F4F RID: 12111
		public FsmFloat addX;

		// Token: 0x04002F50 RID: 12112
		public FsmFloat addY;

		// Token: 0x04002F51 RID: 12113
		public FsmFloat addZ;

		// Token: 0x04002F52 RID: 12114
		public bool everyFrame;

		// Token: 0x04002F53 RID: 12115
		public bool perSecond;
	}
}
