using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B4A RID: 2890
	[Tooltip("Get the XY channels of a Vector2 Variable and store them in Float Variables.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class GetVector2XY : FsmStateAction
	{
		// Token: 0x06003CAC RID: 15532 RVA: 0x00130A84 File Offset: 0x0012EC84
		public override void Reset()
		{
			this.vector2Variable = null;
			this.storeX = null;
			this.storeY = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x00130AA4 File Offset: 0x0012ECA4
		public override void OnEnter()
		{
			this.DoGetVector2XYZ();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00130AC0 File Offset: 0x0012ECC0
		public override void OnUpdate()
		{
			this.DoGetVector2XYZ();
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x00130AC8 File Offset: 0x0012ECC8
		private void DoGetVector2XYZ()
		{
			if (this.vector2Variable == null)
			{
				return;
			}
			if (this.storeX != null)
			{
				this.storeX.Value = this.vector2Variable.Value.x;
			}
			if (this.storeY != null)
			{
				this.storeY.Value = this.vector2Variable.Value.y;
			}
		}

		// Token: 0x04002EF5 RID: 12021
		[Tooltip("The vector2 source")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002EF6 RID: 12022
		[Tooltip("The x component")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		// Token: 0x04002EF7 RID: 12023
		[UIHint(UIHint.Variable)]
		[Tooltip("The y component")]
		public FsmFloat storeY;

		// Token: 0x04002EF8 RID: 12024
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
