using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B0F RID: 2831
	[Tooltip("Sets the individual fields of a Rect Variable. To leave any field unchanged, set variable to 'None'.")]
	[ActionCategory(ActionCategory.Rect)]
	public class SetRectFields : FsmStateAction
	{
		// Token: 0x06003BAF RID: 15279 RVA: 0x0012CE28 File Offset: 0x0012B028
		public override void Reset()
		{
			this.rectVariable = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.width = new FsmFloat
			{
				UseVariable = true
			};
			this.height = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x0012CE94 File Offset: 0x0012B094
		public override void OnEnter()
		{
			this.DoSetRectFields();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x0012CEB0 File Offset: 0x0012B0B0
		public override void OnUpdate()
		{
			this.DoSetRectFields();
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0012CEB8 File Offset: 0x0012B0B8
		private void DoSetRectFields()
		{
			if (this.rectVariable.IsNone)
			{
				return;
			}
			Rect value = this.rectVariable.Value;
			if (!this.x.IsNone)
			{
				value.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				value.y = this.y.Value;
			}
			if (!this.width.IsNone)
			{
				value.width = this.width.Value;
			}
			if (!this.height.IsNone)
			{
				value.height = this.height.Value;
			}
			this.rectVariable.Value = value;
		}

		// Token: 0x04002DCF RID: 11727
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmRect rectVariable;

		// Token: 0x04002DD0 RID: 11728
		public FsmFloat x;

		// Token: 0x04002DD1 RID: 11729
		public FsmFloat y;

		// Token: 0x04002DD2 RID: 11730
		public FsmFloat width;

		// Token: 0x04002DD3 RID: 11731
		public FsmFloat height;

		// Token: 0x04002DD4 RID: 11732
		public bool everyFrame;
	}
}
