using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000932 RID: 2354
	[Tooltip("Destroys the Owner of the Fsm! Useful for spawned Prefabs that need to kill themselves, e.g., a projectile that explodes on impact.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class DestroySelf : FsmStateAction
	{
		// Token: 0x0600339E RID: 13214 RVA: 0x0010DB4C File Offset: 0x0010BD4C
		public override void Reset()
		{
			this.detachChildren = false;
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0010DB5C File Offset: 0x0010BD5C
		public override void OnEnter()
		{
			if (base.Owner != null)
			{
				if (this.detachChildren.Value)
				{
					base.Owner.transform.DetachChildren();
				}
				UnityEngine.Object.Destroy(base.Owner);
			}
			base.Finish();
		}

		// Token: 0x040024C8 RID: 9416
		[Tooltip("Detach children before destroying the Owner.")]
		public FsmBool detachChildren;
	}
}
