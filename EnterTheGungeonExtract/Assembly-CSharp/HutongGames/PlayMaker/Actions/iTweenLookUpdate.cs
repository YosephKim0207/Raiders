using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009FC RID: 2556
	[Tooltip("Rotates a GameObject to look at a supplied Transform or Vector3 over time.")]
	[ActionCategory("iTween")]
	public class iTweenLookUpdate : FsmStateAction
	{
		// Token: 0x060036D0 RID: 14032 RVA: 0x0011801C File Offset: 0x0011621C
		public override void Reset()
		{
			this.transformTarget = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorTarget = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
			this.axis = iTweenFsmAction.AxisRestriction.none;
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x00118068 File Offset: 0x00116268
		public override void OnEnter()
		{
			this.hash = new Hashtable();
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				base.Finish();
				return;
			}
			if (this.transformTarget.IsNone)
			{
				this.hash.Add("looktarget", (!this.vectorTarget.IsNone) ? this.vectorTarget.Value : Vector3.zero);
			}
			else if (this.vectorTarget.IsNone)
			{
				this.hash.Add("looktarget", this.transformTarget.Value.transform);
			}
			else
			{
				this.hash.Add("looktarget", this.transformTarget.Value.transform.position + this.vectorTarget.Value);
			}
			this.hash.Add("time", (!this.time.IsNone) ? this.time.Value : 1f);
			this.hash.Add("axis", (this.axis != iTweenFsmAction.AxisRestriction.none) ? Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), this.axis) : string.Empty);
			this.DoiTween();
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x001181EC File Offset: 0x001163EC
		public override void OnExit()
		{
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x001181F0 File Offset: 0x001163F0
		public override void OnUpdate()
		{
			this.hash.Remove("looktarget");
			if (this.transformTarget.IsNone)
			{
				this.hash.Add("looktarget", (!this.vectorTarget.IsNone) ? this.vectorTarget.Value : Vector3.zero);
			}
			else if (this.vectorTarget.IsNone)
			{
				this.hash.Add("looktarget", this.transformTarget.Value.transform);
			}
			else
			{
				this.hash.Add("looktarget", this.transformTarget.Value.transform.position + this.vectorTarget.Value);
			}
			this.DoiTween();
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x001182D4 File Offset: 0x001164D4
		private void DoiTween()
		{
			iTween.LookUpdate(this.go, this.hash);
		}

		// Token: 0x04002845 RID: 10309
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002846 RID: 10310
		[Tooltip("Look at a transform position.")]
		public FsmGameObject transformTarget;

		// Token: 0x04002847 RID: 10311
		[Tooltip("A target position the GameObject will look at. If Transform Target is defined this is used as a look offset.")]
		public FsmVector3 vectorTarget;

		// Token: 0x04002848 RID: 10312
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x04002849 RID: 10313
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis;

		// Token: 0x0400284A RID: 10314
		private Hashtable hash;

		// Token: 0x0400284B RID: 10315
		private GameObject go;
	}
}
