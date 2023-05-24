using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A0B RID: 2571
	[Tooltip("Similar to RotateTo but incredibly less expensive for usage inside the Update function or similar looping situations involving a 'live' set of changing values. Does not utilize an EaseType.")]
	[ActionCategory("iTween")]
	public class iTweenRotateUpdate : FsmStateAction
	{
		// Token: 0x0600371C RID: 14108 RVA: 0x0011B650 File Offset: 0x00119850
		public override void Reset()
		{
			this.transformRotation = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorRotation = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
			this.space = Space.World;
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0011B69C File Offset: 0x0011989C
		public override void OnEnter()
		{
			this.hash = new Hashtable();
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				base.Finish();
				return;
			}
			if (this.transformRotation.IsNone)
			{
				this.hash.Add("rotation", (!this.vectorRotation.IsNone) ? this.vectorRotation.Value : Vector3.zero);
			}
			else if (this.vectorRotation.IsNone)
			{
				this.hash.Add("rotation", this.transformRotation.Value.transform);
			}
			else if (this.space == Space.World)
			{
				this.hash.Add("rotation", this.transformRotation.Value.transform.eulerAngles + this.vectorRotation.Value);
			}
			else
			{
				this.hash.Add("rotation", this.transformRotation.Value.transform.localEulerAngles + this.vectorRotation.Value);
			}
			this.hash.Add("time", (!this.time.IsNone) ? this.time.Value : 1f);
			this.hash.Add("islocal", this.space == Space.Self);
			this.DoiTween();
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x0011B84C File Offset: 0x00119A4C
		public override void OnExit()
		{
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0011B850 File Offset: 0x00119A50
		public override void OnUpdate()
		{
			this.hash.Remove("rotation");
			if (this.transformRotation.IsNone)
			{
				this.hash.Add("rotation", (!this.vectorRotation.IsNone) ? this.vectorRotation.Value : Vector3.zero);
			}
			else if (this.vectorRotation.IsNone)
			{
				this.hash.Add("rotation", this.transformRotation.Value.transform);
			}
			else if (this.space == Space.World)
			{
				this.hash.Add("rotation", this.transformRotation.Value.transform.eulerAngles + this.vectorRotation.Value);
			}
			else
			{
				this.hash.Add("rotation", this.transformRotation.Value.transform.localEulerAngles + this.vectorRotation.Value);
			}
			this.DoiTween();
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0011B97C File Offset: 0x00119B7C
		private void DoiTween()
		{
			iTween.RotateUpdate(this.go, this.hash);
		}

		// Token: 0x040028DB RID: 10459
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028DC RID: 10460
		[Tooltip("Rotate to a transform rotation.")]
		public FsmGameObject transformRotation;

		// Token: 0x040028DD RID: 10461
		[Tooltip("A rotation the GameObject will animate from.")]
		public FsmVector3 vectorRotation;

		// Token: 0x040028DE RID: 10462
		[Tooltip("The time in seconds the animation will take to complete. If transformRotation is set, this is used as an offset.")]
		public FsmFloat time;

		// Token: 0x040028DF RID: 10463
		[Tooltip("Whether to animate in local or world space.")]
		public Space space;

		// Token: 0x040028E0 RID: 10464
		private Hashtable hash;

		// Token: 0x040028E1 RID: 10465
		private GameObject go;
	}
}
