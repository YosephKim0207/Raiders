using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB8 RID: 2744
	[Tooltip("Select a Random Vector3 from a Vector3 array.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class SelectRandomVector3 : FsmStateAction
	{
		// Token: 0x06003A39 RID: 14905 RVA: 0x001285FC File Offset: 0x001267FC
		public override void Reset()
		{
			this.vector3Array = new FsmVector3[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.storeVector3 = null;
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00128650 File Offset: 0x00126850
		public override void OnEnter()
		{
			this.DoSelectRandomColor();
			base.Finish();
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x00128660 File Offset: 0x00126860
		private void DoSelectRandomColor()
		{
			if (this.vector3Array == null)
			{
				return;
			}
			if (this.vector3Array.Length == 0)
			{
				return;
			}
			if (this.storeVector3 == null)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
			if (randomWeightedIndex != -1)
			{
				this.storeVector3.Value = this.vector3Array[randomWeightedIndex].Value;
			}
		}

		// Token: 0x04002C69 RID: 11369
		[CompoundArray("Vectors", "Vector", "Weight")]
		public FsmVector3[] vector3Array;

		// Token: 0x04002C6A RID: 11370
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002C6B RID: 11371
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector3;
	}
}
