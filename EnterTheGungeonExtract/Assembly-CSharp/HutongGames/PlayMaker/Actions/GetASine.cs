using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B3D RID: 2877
	[ActionCategory(ActionCategory.Trigonometry)]
	[Tooltip("Get the Arc sine. You can get the result in degrees, simply check on the RadToDeg conversion")]
	public class GetASine : FsmStateAction
	{
		// Token: 0x06003C75 RID: 15477 RVA: 0x00130340 File Offset: 0x0012E540
		public override void Reset()
		{
			this.angle = null;
			this.RadToDeg = true;
			this.everyFrame = false;
			this.Value = null;
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00130364 File Offset: 0x0012E564
		public override void OnEnter()
		{
			this.DoASine();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00130380 File Offset: 0x0012E580
		public override void OnUpdate()
		{
			this.DoASine();
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00130388 File Offset: 0x0012E588
		private void DoASine()
		{
			float num = Mathf.Asin(this.Value.Value);
			if (this.RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			this.angle.Value = num;
		}

		// Token: 0x04002EC6 RID: 11974
		[Tooltip("The value of the sine")]
		[RequiredField]
		public FsmFloat Value;

		// Token: 0x04002EC7 RID: 11975
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002EC8 RID: 11976
		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		// Token: 0x04002EC9 RID: 11977
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
