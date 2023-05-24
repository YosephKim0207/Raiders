using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000991 RID: 2449
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Game Object's Layer and stores it in an Int Variable.")]
	public class GetLayer : FsmStateAction
	{
		// Token: 0x06003533 RID: 13619 RVA: 0x00112A24 File Offset: 0x00110C24
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00112A3C File Offset: 0x00110C3C
		public override void OnEnter()
		{
			this.DoGetLayer();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00112A58 File Offset: 0x00110C58
		public override void OnUpdate()
		{
			this.DoGetLayer();
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00112A60 File Offset: 0x00110C60
		private void DoGetLayer()
		{
			if (this.gameObject.Value == null)
			{
				return;
			}
			this.storeResult.Value = this.gameObject.Value.layer;
		}

		// Token: 0x04002697 RID: 9879
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x04002698 RID: 9880
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeResult;

		// Token: 0x04002699 RID: 9881
		public bool everyFrame;
	}
}
