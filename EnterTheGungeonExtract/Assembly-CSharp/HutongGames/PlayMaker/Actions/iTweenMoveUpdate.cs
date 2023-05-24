using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A01 RID: 2561
	[Tooltip("Similar to MoveTo but incredibly less expensive for usage inside the Update function or similar looping situations involving a 'live' set of changing values. Does not utilize an EaseType.")]
	[ActionCategory("iTween")]
	public class iTweenMoveUpdate : FsmStateAction
	{
		// Token: 0x060036EB RID: 14059 RVA: 0x00119B68 File Offset: 0x00117D68
		public override void Reset()
		{
			this.transformPosition = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
			this.space = Space.World;
			this.orientToPath = new FsmBool
			{
				Value = true
			};
			this.lookAtObject = new FsmGameObject
			{
				UseVariable = true
			};
			this.lookAtVector = new FsmVector3
			{
				UseVariable = true
			};
			this.lookTime = 0f;
			this.axis = iTweenFsmAction.AxisRestriction.none;
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x00119C08 File Offset: 0x00117E08
		public override void OnEnter()
		{
			this.hash = new Hashtable();
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				base.Finish();
				return;
			}
			if (this.transformPosition.IsNone)
			{
				this.hash.Add("position", (!this.vectorPosition.IsNone) ? this.vectorPosition.Value : Vector3.zero);
			}
			else if (this.vectorPosition.IsNone)
			{
				this.hash.Add("position", this.transformPosition.Value.transform);
			}
			else if (this.space == Space.World || this.go.transform.parent == null)
			{
				this.hash.Add("position", this.transformPosition.Value.transform.position + this.vectorPosition.Value);
			}
			else
			{
				this.hash.Add("position", this.go.transform.parent.InverseTransformPoint(this.transformPosition.Value.transform.position) + this.vectorPosition.Value);
			}
			this.hash.Add("time", (!this.time.IsNone) ? this.time.Value : 1f);
			this.hash.Add("islocal", this.space == Space.Self);
			this.hash.Add("axis", (this.axis != iTweenFsmAction.AxisRestriction.none) ? Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), this.axis) : string.Empty);
			if (!this.orientToPath.IsNone)
			{
				this.hash.Add("orienttopath", this.orientToPath.Value);
			}
			if (this.lookAtObject.IsNone)
			{
				if (!this.lookAtVector.IsNone)
				{
					this.hash.Add("looktarget", this.lookAtVector.Value);
				}
			}
			else
			{
				this.hash.Add("looktarget", this.lookAtObject.Value.transform);
			}
			if (!this.lookAtObject.IsNone || !this.lookAtVector.IsNone)
			{
				this.hash.Add("looktime", (!this.lookTime.IsNone) ? this.lookTime.Value : 0f);
			}
			this.DoiTween();
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x00119F14 File Offset: 0x00118114
		public override void OnUpdate()
		{
			this.hash.Remove("position");
			if (this.transformPosition.IsNone)
			{
				this.hash.Add("position", (!this.vectorPosition.IsNone) ? this.vectorPosition.Value : Vector3.zero);
			}
			else if (this.vectorPosition.IsNone)
			{
				this.hash.Add("position", this.transformPosition.Value.transform);
			}
			else if (this.space == Space.World)
			{
				this.hash.Add("position", this.transformPosition.Value.transform.position + this.vectorPosition.Value);
			}
			else
			{
				this.hash.Add("position", this.transformPosition.Value.transform.localPosition + this.vectorPosition.Value);
			}
			this.DoiTween();
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x0011A040 File Offset: 0x00118240
		public override void OnExit()
		{
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x0011A044 File Offset: 0x00118244
		private void DoiTween()
		{
			iTween.MoveUpdate(this.go, this.hash);
		}

		// Token: 0x0400288C RID: 10380
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400288D RID: 10381
		[Tooltip("Move From a transform rotation.")]
		public FsmGameObject transformPosition;

		// Token: 0x0400288E RID: 10382
		[Tooltip("The position the GameObject will animate from.  If transformPosition is set, this is used as an offset.")]
		public FsmVector3 vectorPosition;

		// Token: 0x0400288F RID: 10383
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x04002890 RID: 10384
		[Tooltip("Whether to animate in local or world space.")]
		public Space space;

		// Token: 0x04002891 RID: 10385
		[Tooltip("Whether or not the GameObject will orient to its direction of travel. False by default.")]
		[ActionSection("LookAt")]
		public FsmBool orientToPath;

		// Token: 0x04002892 RID: 10386
		[Tooltip("A target object the GameObject will look at.")]
		public FsmGameObject lookAtObject;

		// Token: 0x04002893 RID: 10387
		[Tooltip("A target position the GameObject will look at.")]
		public FsmVector3 lookAtVector;

		// Token: 0x04002894 RID: 10388
		[Tooltip("The time in seconds the object will take to look at either the Look At Target or Orient To Path. 0 by default")]
		public FsmFloat lookTime;

		// Token: 0x04002895 RID: 10389
		[Tooltip("Restricts rotation to the supplied axis only.")]
		public iTweenFsmAction.AxisRestriction axis;

		// Token: 0x04002896 RID: 10390
		private Hashtable hash;

		// Token: 0x04002897 RID: 10391
		private GameObject go;
	}
}
