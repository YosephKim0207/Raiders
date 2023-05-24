using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B5C RID: 2908
	[Tooltip("Rotates a Vector2 direction from Current towards Target.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2RotateTowards : FsmStateAction
	{
		// Token: 0x06003CF7 RID: 15607 RVA: 0x001318F0 File Offset: 0x0012FAF0
		public override void Reset()
		{
			this.currentDirection = new FsmVector2
			{
				UseVariable = true
			};
			this.targetDirection = new FsmVector2
			{
				UseVariable = true
			};
			this.rotateSpeed = 360f;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00131938 File Offset: 0x0012FB38
		public override void OnEnter()
		{
			this.current = new Vector3(this.currentDirection.Value.x, this.currentDirection.Value.y, 0f);
			this.target = new Vector3(this.targetDirection.Value.x, this.targetDirection.Value.y, 0f);
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x001319B4 File Offset: 0x0012FBB4
		public override void OnUpdate()
		{
			this.current.x = this.currentDirection.Value.x;
			this.current.y = this.currentDirection.Value.y;
			this.current = Vector3.RotateTowards(this.current, this.target, this.rotateSpeed.Value * 0.017453292f * Time.deltaTime, 1000f);
			this.currentDirection.Value = new Vector2(this.current.x, this.current.y);
		}

		// Token: 0x04002F42 RID: 12098
		[Tooltip("The current direction. This will be the result of the rotation as well.")]
		[RequiredField]
		public FsmVector2 currentDirection;

		// Token: 0x04002F43 RID: 12099
		[Tooltip("The direction to reach")]
		[RequiredField]
		public FsmVector2 targetDirection;

		// Token: 0x04002F44 RID: 12100
		[Tooltip("Rotation speed in degrees per second")]
		[RequiredField]
		public FsmFloat rotateSpeed;

		// Token: 0x04002F45 RID: 12101
		private Vector3 current;

		// Token: 0x04002F46 RID: 12102
		private Vector3 target;
	}
}
