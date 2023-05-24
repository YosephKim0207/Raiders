using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C6 RID: 2246
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the position and rotation of the body. A GameObject can be set to control the position and rotation, or it can be manually expressed.")]
	public class SetAnimatorBody : FsmStateAction
	{
		// Token: 0x060031D4 RID: 12756 RVA: 0x001071C4 File Offset: 0x001053C4
		public override void Reset()
		{
			this.gameObject = null;
			this.target = null;
			this.position = new FsmVector3
			{
				UseVariable = true
			};
			this.rotation = new FsmQuaternion
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x00107210 File Offset: 0x00105410
		public override void OnPreprocess()
		{
			base.Fsm.HandleAnimatorIK = true;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x00107220 File Offset: 0x00105420
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			if (this._animator == null)
			{
				base.Finish();
				return;
			}
			GameObject value = this.target.Value;
			if (value != null)
			{
				this._transform = value.transform;
			}
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x0010729C File Offset: 0x0010549C
		public override void DoAnimatorIK(int layerIndex)
		{
			this.DoSetBody();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x001072B8 File Offset: 0x001054B8
		private void DoSetBody()
		{
			if (this._animator == null)
			{
				return;
			}
			if (this._transform != null)
			{
				if (this.position.IsNone)
				{
					this._animator.bodyPosition = this._transform.position;
				}
				else
				{
					this._animator.bodyPosition = this._transform.position + this.position.Value;
				}
				if (this.rotation.IsNone)
				{
					this._animator.bodyRotation = this._transform.rotation;
				}
				else
				{
					this._animator.bodyRotation = this._transform.rotation * this.rotation.Value;
				}
			}
			else
			{
				if (!this.position.IsNone)
				{
					this._animator.bodyPosition = this.position.Value;
				}
				if (!this.rotation.IsNone)
				{
					this._animator.bodyRotation = this.rotation.Value;
				}
			}
		}

		// Token: 0x040022F6 RID: 8950
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022F7 RID: 8951
		[Tooltip("The gameObject target of the ik goal")]
		public FsmGameObject target;

		// Token: 0x040022F8 RID: 8952
		[Tooltip("The position of the ik goal. If Goal GameObject set, position is used as an offset from Goal")]
		public FsmVector3 position;

		// Token: 0x040022F9 RID: 8953
		[Tooltip("The rotation of the ik goal.If Goal GameObject set, rotation is used as an offset from Goal")]
		public FsmQuaternion rotation;

		// Token: 0x040022FA RID: 8954
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x040022FB RID: 8955
		private Animator _animator;

		// Token: 0x040022FC RID: 8956
		private Transform _transform;
	}
}
