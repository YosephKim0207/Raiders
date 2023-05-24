using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A10 RID: 2576
	[Tooltip("CSimilar to ScaleTo but incredibly less expensive for usage inside the Update function or similar looping situations involving a 'live' set of changing values. Does not utilize an EaseType.")]
	[ActionCategory("iTween")]
	public class iTweenScaleUpdate : FsmStateAction
	{
		// Token: 0x06003736 RID: 14134 RVA: 0x0011C570 File Offset: 0x0011A770
		public override void Reset()
		{
			this.transformScale = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorScale = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x0011C5B8 File Offset: 0x0011A7B8
		public override void OnEnter()
		{
			this.hash = new Hashtable();
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				base.Finish();
				return;
			}
			if (this.transformScale.IsNone)
			{
				this.hash.Add("scale", (!this.vectorScale.IsNone) ? this.vectorScale.Value : Vector3.zero);
			}
			else if (this.vectorScale.IsNone)
			{
				this.hash.Add("scale", this.transformScale.Value.transform);
			}
			else
			{
				this.hash.Add("scale", this.transformScale.Value.transform.localScale + this.vectorScale.Value);
			}
			this.hash.Add("time", (!this.time.IsNone) ? this.time.Value : 1f);
			this.DoiTween();
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x0011C700 File Offset: 0x0011A900
		public override void OnExit()
		{
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x0011C704 File Offset: 0x0011A904
		public override void OnUpdate()
		{
			this.hash.Remove("scale");
			if (this.transformScale.IsNone)
			{
				this.hash.Add("scale", (!this.vectorScale.IsNone) ? this.vectorScale.Value : Vector3.zero);
			}
			else if (this.vectorScale.IsNone)
			{
				this.hash.Add("scale", this.transformScale.Value.transform);
			}
			else
			{
				this.hash.Add("scale", this.transformScale.Value.transform.localScale + this.vectorScale.Value);
			}
			this.DoiTween();
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x0011C7E8 File Offset: 0x0011A9E8
		private void DoiTween()
		{
			iTween.ScaleUpdate(this.go, this.hash);
		}

		// Token: 0x04002904 RID: 10500
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002905 RID: 10501
		[Tooltip("Scale To a transform scale.")]
		public FsmGameObject transformScale;

		// Token: 0x04002906 RID: 10502
		[Tooltip("A scale vector the GameObject will animate To.")]
		public FsmVector3 vectorScale;

		// Token: 0x04002907 RID: 10503
		[Tooltip("The time in seconds the animation will take to complete. If transformScale is set, this is used as an offset.")]
		public FsmFloat time;

		// Token: 0x04002908 RID: 10504
		private Hashtable hash;

		// Token: 0x04002909 RID: 10505
		private GameObject go;
	}
}
