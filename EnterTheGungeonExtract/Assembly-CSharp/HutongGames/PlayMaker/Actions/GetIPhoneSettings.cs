using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200098B RID: 2443
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Get various iPhone settings.")]
	public class GetIPhoneSettings : FsmStateAction
	{
		// Token: 0x0600351F RID: 13599 RVA: 0x0011284C File Offset: 0x00110A4C
		public override void Reset()
		{
			this.getScreenCanDarken = null;
			this.getUniqueIdentifier = null;
			this.getName = null;
			this.getModel = null;
			this.getSystemName = null;
			this.getGeneration = null;
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00112878 File Offset: 0x00110A78
		public override void OnEnter()
		{
			base.Finish();
		}

		// Token: 0x04002686 RID: 9862
		[UIHint(UIHint.Variable)]
		[Tooltip("Allows device to fall into 'sleep' state with screen being dim if no touches occurred. Default value is true.")]
		public FsmBool getScreenCanDarken;

		// Token: 0x04002687 RID: 9863
		[Tooltip("A unique device identifier string. It is guaranteed to be unique for every device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getUniqueIdentifier;

		// Token: 0x04002688 RID: 9864
		[Tooltip("The user defined name of the device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getName;

		// Token: 0x04002689 RID: 9865
		[Tooltip("The model of the device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getModel;

		// Token: 0x0400268A RID: 9866
		[Tooltip("The name of the operating system running on the device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getSystemName;

		// Token: 0x0400268B RID: 9867
		[UIHint(UIHint.Variable)]
		[Tooltip("The generation of the device (Read Only).")]
		public FsmString getGeneration;
	}
}
