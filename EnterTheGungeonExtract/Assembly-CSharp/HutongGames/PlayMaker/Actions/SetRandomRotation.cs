using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B0E RID: 2830
	[Tooltip("Sets Random Rotation for a Game Object. Uncheck an axis to keep its current value.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SetRandomRotation : FsmStateAction
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x0012CD20 File Offset: 0x0012AF20
		public override void Reset()
		{
			this.gameObject = null;
			this.x = true;
			this.y = true;
			this.z = true;
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x0012CD50 File Offset: 0x0012AF50
		public override void OnEnter()
		{
			this.DoRandomRotation();
			base.Finish();
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x0012CD60 File Offset: 0x0012AF60
		private void DoRandomRotation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 localEulerAngles = ownerDefaultTarget.transform.localEulerAngles;
			float num = localEulerAngles.x;
			float num2 = localEulerAngles.y;
			float num3 = localEulerAngles.z;
			if (this.x.Value)
			{
				num = (float)UnityEngine.Random.Range(0, 360);
			}
			if (this.y.Value)
			{
				num2 = (float)UnityEngine.Random.Range(0, 360);
			}
			if (this.z.Value)
			{
				num3 = (float)UnityEngine.Random.Range(0, 360);
			}
			ownerDefaultTarget.transform.localEulerAngles = new Vector3(num, num2, num3);
		}

		// Token: 0x04002DCB RID: 11723
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DCC RID: 11724
		[RequiredField]
		public FsmBool x;

		// Token: 0x04002DCD RID: 11725
		[RequiredField]
		public FsmBool y;

		// Token: 0x04002DCE RID: 11726
		[RequiredField]
		public FsmBool z;
	}
}
