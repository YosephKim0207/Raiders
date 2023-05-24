using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B6F RID: 2927
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Forces a Game Object's Rigid Body to wake up.")]
	public class WakeUp : ComponentAction<Rigidbody>
	{
		// Token: 0x06003D44 RID: 15684 RVA: 0x00132A18 File Offset: 0x00130C18
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00132A24 File Offset: 0x00130C24
		public override void OnEnter()
		{
			this.DoWakeUp();
			base.Finish();
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00132A34 File Offset: 0x00130C34
		private void DoWakeUp()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (base.UpdateCache(gameObject))
			{
				base.rigidbody.WakeUp();
			}
		}

		// Token: 0x04002F95 RID: 12181
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;
	}
}
