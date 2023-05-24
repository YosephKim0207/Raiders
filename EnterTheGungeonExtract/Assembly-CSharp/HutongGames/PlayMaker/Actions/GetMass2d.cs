using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A56 RID: 2646
	[Tooltip("Gets the Mass of a Game Object's Rigid Body 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetMass2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x06003853 RID: 14419 RVA: 0x00120BAC File Offset: 0x0011EDAC
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x00120BBC File Offset: 0x0011EDBC
		public override void OnEnter()
		{
			this.DoGetMass();
			base.Finish();
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x00120BCC File Offset: 0x0011EDCC
		private void DoGetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			this.storeResult.Value = base.rigidbody2d.mass;
		}

		// Token: 0x04002A4C RID: 10828
		[Tooltip("The GameObject with a Rigidbody2D attached.")]
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002A4D RID: 10829
		[Tooltip("Store the mass of gameObject.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;
	}
}
