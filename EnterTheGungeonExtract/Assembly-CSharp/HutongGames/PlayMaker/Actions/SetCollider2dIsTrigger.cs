using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A6A RID: 2666
	[Tooltip("Set the isTrigger option of a Collider2D. Optionally set all collider2D found on the gameobject Target.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetCollider2dIsTrigger : FsmStateAction
	{
		// Token: 0x060038B6 RID: 14518 RVA: 0x00123310 File Offset: 0x00121510
		public override void Reset()
		{
			this.gameObject = null;
			this.isTrigger = false;
			this.setAllColliders = false;
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0012332C File Offset: 0x0012152C
		public override void OnEnter()
		{
			this.DoSetIsTrigger();
			base.Finish();
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0012333C File Offset: 0x0012153C
		private void DoSetIsTrigger()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.setAllColliders)
			{
				Collider2D[] components = ownerDefaultTarget.GetComponents<Collider2D>();
				foreach (Collider2D collider2D in components)
				{
					collider2D.isTrigger = this.isTrigger.Value;
				}
			}
			else if (ownerDefaultTarget.GetComponent<Collider2D>() != null)
			{
				ownerDefaultTarget.GetComponent<Collider2D>().isTrigger = this.isTrigger.Value;
			}
		}

		// Token: 0x04002B0E RID: 11022
		[Tooltip("The GameObject with the Collider2D attached")]
		[CheckForComponent(typeof(Collider2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B0F RID: 11023
		[Tooltip("The flag value")]
		[RequiredField]
		public FsmBool isTrigger;

		// Token: 0x04002B10 RID: 11024
		[Tooltip("Set all Colliders on the GameObject target")]
		public bool setAllColliders;
	}
}
