using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B4B RID: 2891
	[Tooltip("Select a Random Vector2 from a Vector2 array.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class SelectRandomVector2 : FsmStateAction
	{
		// Token: 0x06003CB1 RID: 15537 RVA: 0x00130B3C File Offset: 0x0012ED3C
		public override void Reset()
		{
			this.vector2Array = new FsmVector2[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.storeVector2 = null;
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00130B90 File Offset: 0x0012ED90
		public override void OnEnter()
		{
			this.DoSelectRandomColor();
			base.Finish();
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00130BA0 File Offset: 0x0012EDA0
		private void DoSelectRandomColor()
		{
			if (this.vector2Array == null)
			{
				return;
			}
			if (this.vector2Array.Length == 0)
			{
				return;
			}
			if (this.storeVector2 == null)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
			if (randomWeightedIndex != -1)
			{
				this.storeVector2.Value = this.vector2Array[randomWeightedIndex].Value;
			}
		}

		// Token: 0x04002EF9 RID: 12025
		[CompoundArray("Vectors", "Vector", "Weight")]
		[Tooltip("The array of Vectors and respective weights")]
		public FsmVector2[] vector2Array;

		// Token: 0x04002EFA RID: 12026
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002EFB RID: 12027
		[RequiredField]
		[Tooltip("The picked vector2")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeVector2;
	}
}
