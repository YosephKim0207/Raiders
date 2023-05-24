using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000940 RID: 2368
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Enables/Disables an FSM component on a GameObject.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class EnableFSM : FsmStateAction
	{
		// Token: 0x060033D1 RID: 13265 RVA: 0x0010E468 File Offset: 0x0010C668
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.enable = true;
			this.resetOnExit = true;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0010E49C File Offset: 0x0010C69C
		public override void OnEnter()
		{
			this.DoEnableFSM();
			base.Finish();
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0010E4AC File Offset: 0x0010C6AC
		private void DoEnableFSM()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (gameObject == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.fsmName.Value))
			{
				PlayMakerFSM[] components = gameObject.GetComponents<PlayMakerFSM>();
				foreach (PlayMakerFSM playMakerFSM in components)
				{
					if (playMakerFSM.FsmName == this.fsmName.Value)
					{
						this.fsmComponent = playMakerFSM;
						break;
					}
				}
			}
			else
			{
				this.fsmComponent = gameObject.GetComponent<PlayMakerFSM>();
			}
			if (this.fsmComponent == null)
			{
				base.LogError("Missing FsmComponent!");
				return;
			}
			this.fsmComponent.enabled = this.enable.Value;
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x0010E598 File Offset: 0x0010C798
		public override void OnExit()
		{
			if (this.fsmComponent == null)
			{
				return;
			}
			if (this.resetOnExit.Value)
			{
				this.fsmComponent.enabled = !this.enable.Value;
			}
		}

		// Token: 0x040024F3 RID: 9459
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM component.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040024F4 RID: 9460
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on GameObject. Useful if you have more than one FSM on a GameObject.")]
		public FsmString fsmName;

		// Token: 0x040024F5 RID: 9461
		[Tooltip("Set to True to enable, False to disable.")]
		public FsmBool enable;

		// Token: 0x040024F6 RID: 9462
		[Tooltip("Reset the initial enabled state when exiting the state.")]
		public FsmBool resetOnExit;

		// Token: 0x040024F7 RID: 9463
		private PlayMakerFSM fsmComponent;
	}
}
