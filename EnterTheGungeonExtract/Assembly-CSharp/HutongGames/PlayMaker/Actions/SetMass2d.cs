using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A70 RID: 2672
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Sets the Mass of a Game Object's Rigid Body 2D.")]
	public class SetMass2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038D1 RID: 14545 RVA: 0x001238C4 File Offset: 0x00121AC4
		public override void Reset()
		{
			this.gameObject = null;
			this.mass = 1f;
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x001238E0 File Offset: 0x00121AE0
		public override void OnEnter()
		{
			this.DoSetMass();
			base.Finish();
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x001238F0 File Offset: 0x00121AF0
		private void DoSetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			base.rigidbody2d.mass = this.mass.Value;
		}

		// Token: 0x04002B27 RID: 11047
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B28 RID: 11048
		[Tooltip("The Mass")]
		[HasFloatSlider(0.1f, 10f)]
		[RequiredField]
		public FsmFloat mass;
	}
}
