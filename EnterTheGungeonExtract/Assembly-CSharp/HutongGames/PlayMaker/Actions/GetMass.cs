using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000994 RID: 2452
	[Tooltip("Gets the Mass of a Game Object's Rigid Body.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetMass : ComponentAction<Rigidbody>
	{
		// Token: 0x0600353F RID: 13631 RVA: 0x00112B2C File Offset: 0x00110D2C
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x00112B3C File Offset: 0x00110D3C
		public override void OnEnter()
		{
			this.DoGetMass();
			base.Finish();
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x00112B4C File Offset: 0x00110D4C
		private void DoGetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				this.storeResult.Value = base.rigidbody.mass;
			}
		}

		// Token: 0x040026A2 RID: 9890
		[Tooltip("The GameObject that owns the Rigidbody")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026A3 RID: 9891
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the mass in a float variable.")]
		public FsmFloat storeResult;
	}
}
