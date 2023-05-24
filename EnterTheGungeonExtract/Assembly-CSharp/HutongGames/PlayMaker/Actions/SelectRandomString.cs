using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB7 RID: 2743
	[Tooltip("Select a Random String from an array of Strings.")]
	[ActionCategory(ActionCategory.String)]
	public class SelectRandomString : FsmStateAction
	{
		// Token: 0x06003A35 RID: 14901 RVA: 0x00128530 File Offset: 0x00126730
		public override void Reset()
		{
			this.strings = new FsmString[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.storeString = null;
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x00128584 File Offset: 0x00126784
		public override void OnEnter()
		{
			this.DoSelectRandomString();
			base.Finish();
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x00128594 File Offset: 0x00126794
		private void DoSelectRandomString()
		{
			if (this.strings == null)
			{
				return;
			}
			if (this.strings.Length == 0)
			{
				return;
			}
			if (this.storeString == null)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
			if (randomWeightedIndex != -1)
			{
				this.storeString.Value = this.strings[randomWeightedIndex].Value;
			}
		}

		// Token: 0x04002C66 RID: 11366
		[CompoundArray("Strings", "String", "Weight")]
		public FsmString[] strings;

		// Token: 0x04002C67 RID: 11367
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002C68 RID: 11368
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeString;
	}
}
