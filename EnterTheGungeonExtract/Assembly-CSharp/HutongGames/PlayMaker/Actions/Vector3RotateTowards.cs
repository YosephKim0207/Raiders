using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B6B RID: 2923
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Rotates a Vector3 direction from Current towards Target.")]
	public class Vector3RotateTowards : FsmStateAction
	{
		// Token: 0x06003D34 RID: 15668 RVA: 0x00132728 File Offset: 0x00130928
		public override void Reset()
		{
			this.currentDirection = new FsmVector3
			{
				UseVariable = true
			};
			this.targetDirection = new FsmVector3
			{
				UseVariable = true
			};
			this.rotateSpeed = 360f;
			this.maxMagnitude = 1f;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00132780 File Offset: 0x00130980
		public override void OnUpdate()
		{
			this.currentDirection.Value = Vector3.RotateTowards(this.currentDirection.Value, this.targetDirection.Value, this.rotateSpeed.Value * 0.017453292f * Time.deltaTime, this.maxMagnitude.Value);
		}

		// Token: 0x04002F87 RID: 12167
		[RequiredField]
		public FsmVector3 currentDirection;

		// Token: 0x04002F88 RID: 12168
		[RequiredField]
		public FsmVector3 targetDirection;

		// Token: 0x04002F89 RID: 12169
		[RequiredField]
		[Tooltip("Rotation speed in degrees per second")]
		public FsmFloat rotateSpeed;

		// Token: 0x04002F8A RID: 12170
		[Tooltip("Max Magnitude per second")]
		[RequiredField]
		public FsmFloat maxMagnitude;
	}
}
