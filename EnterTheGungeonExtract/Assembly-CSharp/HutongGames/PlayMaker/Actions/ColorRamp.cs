using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200090D RID: 2317
	[Tooltip("Samples a Color on a continuous Colors gradient.")]
	[ActionCategory(ActionCategory.Color)]
	public class ColorRamp : FsmStateAction
	{
		// Token: 0x06003309 RID: 13065 RVA: 0x0010BF98 File Offset: 0x0010A198
		public override void Reset()
		{
			this.colors = new FsmColor[3];
			this.sampleAt = 0f;
			this.storeColor = null;
			this.everyFrame = false;
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x0010BFC4 File Offset: 0x0010A1C4
		public override void OnEnter()
		{
			this.DoColorRamp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x0010BFE0 File Offset: 0x0010A1E0
		public override void OnUpdate()
		{
			this.DoColorRamp();
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x0010BFE8 File Offset: 0x0010A1E8
		private void DoColorRamp()
		{
			if (this.colors == null)
			{
				return;
			}
			if (this.colors.Length == 0)
			{
				return;
			}
			if (this.sampleAt == null)
			{
				return;
			}
			if (this.storeColor == null)
			{
				return;
			}
			float num = Mathf.Clamp(this.sampleAt.Value, 0f, (float)(this.colors.Length - 1));
			Color color;
			if (num == 0f)
			{
				color = this.colors[0].Value;
			}
			else if (num == (float)this.colors.Length)
			{
				color = this.colors[this.colors.Length - 1].Value;
			}
			else
			{
				Color value = this.colors[Mathf.FloorToInt(num)].Value;
				Color value2 = this.colors[Mathf.CeilToInt(num)].Value;
				num -= Mathf.Floor(num);
				color = Color.Lerp(value, value2, num);
			}
			this.storeColor.Value = color;
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x0010C0D8 File Offset: 0x0010A2D8
		public override string ErrorCheck()
		{
			if (this.colors.Length < 2)
			{
				return "Define at least 2 colors to make a gradient.";
			}
			return null;
		}

		// Token: 0x0400243B RID: 9275
		[RequiredField]
		[Tooltip("Array of colors to defining the gradient.")]
		public FsmColor[] colors;

		// Token: 0x0400243C RID: 9276
		[Tooltip("Point on the gradient to sample. Should be between 0 and the number of colors in the gradient.")]
		[RequiredField]
		public FsmFloat sampleAt;

		// Token: 0x0400243D RID: 9277
		[RequiredField]
		[Tooltip("Store the sampled color in a Color variable.")]
		[UIHint(UIHint.Variable)]
		public FsmColor storeColor;

		// Token: 0x0400243E RID: 9278
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
