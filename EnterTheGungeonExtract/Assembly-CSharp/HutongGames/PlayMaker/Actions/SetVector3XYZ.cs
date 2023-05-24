using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B1B RID: 2843
	[Tooltip("Sets the XYZ channels of a Vector3 Variable. To leave any channel unchanged, set variable to 'None'.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class SetVector3XYZ : FsmStateAction
	{
		// Token: 0x06003BE8 RID: 15336 RVA: 0x0012DA60 File Offset: 0x0012BC60
		public override void Reset()
		{
			this.vector3Variable = null;
			this.vector3Value = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.z = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0012DAC0 File Offset: 0x0012BCC0
		public override void OnEnter()
		{
			this.DoSetVector3XYZ();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0012DADC File Offset: 0x0012BCDC
		public override void OnUpdate()
		{
			this.DoSetVector3XYZ();
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0012DAE4 File Offset: 0x0012BCE4
		private void DoSetVector3XYZ()
		{
			if (this.vector3Variable == null)
			{
				return;
			}
			Vector3 vector = this.vector3Variable.Value;
			if (!this.vector3Value.IsNone)
			{
				vector = this.vector3Value.Value;
			}
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			if (!this.z.IsNone)
			{
				vector.z = this.z.Value;
			}
			this.vector3Variable.Value = vector;
		}

		// Token: 0x04002E05 RID: 11781
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		// Token: 0x04002E06 RID: 11782
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Value;

		// Token: 0x04002E07 RID: 11783
		public FsmFloat x;

		// Token: 0x04002E08 RID: 11784
		public FsmFloat y;

		// Token: 0x04002E09 RID: 11785
		public FsmFloat z;

		// Token: 0x04002E0A RID: 11786
		public bool everyFrame;
	}
}
