using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A49 RID: 2633
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if the Network View is controlled by a GameObject.")]
	public class NetworkViewIsMine : FsmStateAction
	{
		// Token: 0x06003816 RID: 14358 RVA: 0x0011FFD4 File Offset: 0x0011E1D4
		private void _getNetworkView()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._networkView = ownerDefaultTarget.GetComponent<NetworkView>();
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x0012000C File Offset: 0x0011E20C
		public override void Reset()
		{
			this.gameObject = null;
			this.isMine = null;
			this.isMineEvent = null;
			this.isNotMineEvent = null;
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x0012002C File Offset: 0x0011E22C
		public override void OnEnter()
		{
			this._getNetworkView();
			this.checkIsMine();
			base.Finish();
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x00120040 File Offset: 0x0011E240
		private void checkIsMine()
		{
			if (this._networkView == null)
			{
				return;
			}
			bool flag = this._networkView.isMine;
			this.isMine.Value = flag;
			if (flag)
			{
				if (this.isMineEvent != null)
				{
					base.Fsm.Event(this.isMineEvent);
				}
			}
			else if (this.isNotMineEvent != null)
			{
				base.Fsm.Event(this.isNotMineEvent);
			}
		}

		// Token: 0x04002A16 RID: 10774
		[CheckForComponent(typeof(NetworkView))]
		[Tooltip("The Game Object with the NetworkView attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002A17 RID: 10775
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the network view is controlled by this object.")]
		public FsmBool isMine;

		// Token: 0x04002A18 RID: 10776
		[Tooltip("Send this event if the network view controlled by this object.")]
		public FsmEvent isMineEvent;

		// Token: 0x04002A19 RID: 10777
		[Tooltip("Send this event if the network view is NOT controlled by this object.")]
		public FsmEvent isNotMineEvent;

		// Token: 0x04002A1A RID: 10778
		private NetworkView _networkView;
	}
}
