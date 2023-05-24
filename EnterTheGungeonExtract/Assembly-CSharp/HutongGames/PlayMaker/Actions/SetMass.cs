using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B01 RID: 2817
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Sets the Mass of a Game Object's Rigid Body.")]
	public class SetMass : ComponentAction<Rigidbody>
	{
		// Token: 0x06003B75 RID: 15221 RVA: 0x0012C030 File Offset: 0x0012A230
		public override void Reset()
		{
			this.gameObject = null;
			this.mass = 1f;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0012C04C File Offset: 0x0012A24C
		public override void OnEnter()
		{
			this.DoSetMass();
			base.Finish();
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0012C05C File Offset: 0x0012A25C
		private void DoSetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody.mass = this.mass.Value;
			}
		}

		// Token: 0x04002D97 RID: 11671
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D98 RID: 11672
		[HasFloatSlider(0.1f, 10f)]
		[RequiredField]
		public FsmFloat mass;
	}
}
