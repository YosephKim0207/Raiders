using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009AE RID: 2478
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets the Speed of a Game Object and stores it in a Float Variable. NOTE: The Game Object must have a rigid body.")]
	public class GetSpeed : ComponentAction<Rigidbody>
	{
		// Token: 0x060035AE RID: 13742 RVA: 0x00113D78 File Offset: 0x00111F78
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00113D90 File Offset: 0x00111F90
		public override void OnEnter()
		{
			this.DoGetSpeed();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x00113DAC File Offset: 0x00111FAC
		public override void OnUpdate()
		{
			this.DoGetSpeed();
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x00113DB4 File Offset: 0x00111FB4
		private void DoGetSpeed()
		{
			if (this.storeResult == null)
			{
				return;
			}
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (base.UpdateCache(gameObject))
			{
				Vector3 velocity = base.rigidbody.velocity;
				this.storeResult.Value = velocity.magnitude;
			}
		}

		// Token: 0x040026FA RID: 9978
		[Tooltip("The GameObject with a Rigidbody.")]
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026FB RID: 9979
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the speed in a float variable.")]
		public FsmFloat storeResult;

		// Token: 0x040026FC RID: 9980
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
