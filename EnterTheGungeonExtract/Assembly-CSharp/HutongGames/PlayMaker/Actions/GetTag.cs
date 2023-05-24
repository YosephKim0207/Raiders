using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B4 RID: 2484
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Game Object's Tag and stores it in a String Variable.")]
	public class GetTag : FsmStateAction
	{
		// Token: 0x060035CB RID: 13771 RVA: 0x00114170 File Offset: 0x00112370
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x00114188 File Offset: 0x00112388
		public override void OnEnter()
		{
			this.DoGetTag();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x001141A4 File Offset: 0x001123A4
		public override void OnUpdate()
		{
			this.DoGetTag();
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x001141AC File Offset: 0x001123AC
		private void DoGetTag()
		{
			if (this.gameObject.Value == null)
			{
				return;
			}
			this.storeResult.Value = this.gameObject.Value.tag;
		}

		// Token: 0x04002710 RID: 10000
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x04002711 RID: 10001
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		// Token: 0x04002712 RID: 10002
		public bool everyFrame;
	}
}
