using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA5 RID: 2725
	[Tooltip("Tests if a point is inside a rectangle.")]
	[ActionCategory(ActionCategory.Rect)]
	public class RectContains : FsmStateAction
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x00127048 File Offset: 0x00125248
		public override void Reset()
		{
			this.rectangle = new FsmRect
			{
				UseVariable = true
			};
			this.point = new FsmVector3
			{
				UseVariable = true
			};
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.storeResult = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.everyFrame = false;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x001270C4 File Offset: 0x001252C4
		public override void OnEnter()
		{
			this.DoRectContains();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x001270E0 File Offset: 0x001252E0
		public override void OnUpdate()
		{
			this.DoRectContains();
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001270E8 File Offset: 0x001252E8
		private void DoRectContains()
		{
			if (this.rectangle.IsNone)
			{
				return;
			}
			Vector3 value = this.point.Value;
			if (!this.x.IsNone)
			{
				value.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				value.y = this.y.Value;
			}
			bool flag = this.rectangle.Value.Contains(value);
			this.storeResult.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002C14 RID: 11284
		[Tooltip("Rectangle to test.")]
		[RequiredField]
		public FsmRect rectangle;

		// Token: 0x04002C15 RID: 11285
		[Tooltip("Point to test.")]
		public FsmVector3 point;

		// Token: 0x04002C16 RID: 11286
		[Tooltip("Specify/override X value.")]
		public FsmFloat x;

		// Token: 0x04002C17 RID: 11287
		[Tooltip("Specify/override Y value.")]
		public FsmFloat y;

		// Token: 0x04002C18 RID: 11288
		[Tooltip("Event to send if the Point is inside the Rectangle.")]
		public FsmEvent trueEvent;

		// Token: 0x04002C19 RID: 11289
		[Tooltip("Event to send if the Point is outside the Rectangle.")]
		public FsmEvent falseEvent;

		// Token: 0x04002C1A RID: 11290
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		// Token: 0x04002C1B RID: 11291
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
