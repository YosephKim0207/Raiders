using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB5 RID: 2741
	[Tooltip("Select a random Color from an array of Colors.")]
	[ActionCategory(ActionCategory.Color)]
	public class SelectRandomColor : FsmStateAction
	{
		// Token: 0x06003A2D RID: 14893 RVA: 0x00128398 File Offset: 0x00126598
		public override void Reset()
		{
			this.colors = new FsmColor[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.storeColor = null;
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x001283EC File Offset: 0x001265EC
		public override void OnEnter()
		{
			this.DoSelectRandomColor();
			base.Finish();
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x001283FC File Offset: 0x001265FC
		private void DoSelectRandomColor()
		{
			if (this.colors == null)
			{
				return;
			}
			if (this.colors.Length == 0)
			{
				return;
			}
			if (this.storeColor == null)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
			if (randomWeightedIndex != -1)
			{
				this.storeColor.Value = this.colors[randomWeightedIndex].Value;
			}
		}

		// Token: 0x04002C60 RID: 11360
		[CompoundArray("Colors", "Color", "Weight")]
		public FsmColor[] colors;

		// Token: 0x04002C61 RID: 11361
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002C62 RID: 11362
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor storeColor;
	}
}
