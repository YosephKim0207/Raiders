using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B4F RID: 2895
	[Tooltip("Adds a XY values to Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2AddXY : FsmStateAction
	{
		// Token: 0x06003CC3 RID: 15555 RVA: 0x00130E50 File Offset: 0x0012F050
		public override void Reset()
		{
			this.vector2Variable = null;
			this.addX = 0f;
			this.addY = 0f;
			this.everyFrame = false;
			this.perSecond = false;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00130E88 File Offset: 0x0012F088
		public override void OnEnter()
		{
			this.DoVector2AddXYZ();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00130EA4 File Offset: 0x0012F0A4
		public override void OnUpdate()
		{
			this.DoVector2AddXYZ();
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x00130EAC File Offset: 0x0012F0AC
		private void DoVector2AddXYZ()
		{
			Vector2 vector = new Vector2(this.addX.Value, this.addY.Value);
			if (this.perSecond)
			{
				this.vector2Variable.Value += vector * Time.deltaTime;
			}
			else
			{
				this.vector2Variable.Value += vector;
			}
		}

		// Token: 0x04002F08 RID: 12040
		[Tooltip("The vector2 target")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F09 RID: 12041
		[Tooltip("The x component to add")]
		public FsmFloat addX;

		// Token: 0x04002F0A RID: 12042
		[Tooltip("The y component to add")]
		public FsmFloat addY;

		// Token: 0x04002F0B RID: 12043
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		// Token: 0x04002F0C RID: 12044
		[Tooltip("Add the value on a per second bases.")]
		public bool perSecond;
	}
}
