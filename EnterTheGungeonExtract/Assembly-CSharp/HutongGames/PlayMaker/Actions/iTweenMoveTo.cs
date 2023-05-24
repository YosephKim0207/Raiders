using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A00 RID: 2560
	[Tooltip("Changes a GameObject's position over time to a supplied destination.")]
	[ActionCategory("iTween")]
	public class iTweenMoveTo : iTweenFsmAction
	{
		// Token: 0x060036E5 RID: 14053 RVA: 0x00119114 File Offset: 0x00117314
		public override void OnDrawActionGizmos()
		{
			if (this.transforms.Length >= 2)
			{
				this.tempVct3 = new Vector3[this.transforms.Length];
				for (int i = 0; i < this.transforms.Length; i++)
				{
					if (this.transforms[i].IsNone)
					{
						this.tempVct3[i] = ((!this.vectors[i].IsNone) ? this.vectors[i].Value : Vector3.zero);
					}
					else if (this.transforms[i].Value == null)
					{
						this.tempVct3[i] = ((!this.vectors[i].IsNone) ? this.vectors[i].Value : Vector3.zero);
					}
					else
					{
						this.tempVct3[i] = this.transforms[i].Value.transform.position + ((!this.vectors[i].IsNone) ? this.vectors[i].Value : Vector3.zero);
					}
				}
				iTween.DrawPathGizmos(this.tempVct3, Color.yellow);
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x0011926C File Offset: 0x0011746C
		public override void Reset()
		{
			base.Reset();
			this.id = new FsmString
			{
				UseVariable = true
			};
			this.transformPosition = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
			this.delay = 0f;
			this.loopType = iTween.LoopType.none;
			this.speed = new FsmFloat
			{
				UseVariable = true
			};
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
			this.lookTime = new FsmFloat
			{
				UseVariable = true
			};
			this.moveToPath = true;
			this.lookAhead = new FsmFloat
			{
				UseVariable = true
			};
			this.transforms = new FsmGameObject[0];
			this.vectors = new FsmVector3[0];
			this.tempVct3 = new Vector3[0];
			this.axis = iTweenFsmAction.AxisRestriction.none;
			this.reverse = false;
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x001193A8 File Offset: 0x001175A8
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x001193D0 File Offset: 0x001175D0
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x001193E0 File Offset: 0x001175E0
		private void DoiTween()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((!this.vectorPosition.IsNone) ? this.vectorPosition.Value : Vector3.zero);
			if (!this.transformPosition.IsNone && this.transformPosition.Value)
			{
				vector = ((this.space != Space.World && !(ownerDefaultTarget.transform.parent == null)) ? (ownerDefaultTarget.transform.parent.InverseTransformPoint(this.transformPosition.Value.transform.position) + vector) : (this.transformPosition.Value.transform.position + vector));
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", vector);
			hashtable.Add((!this.speed.IsNone) ? "speed" : "time", (!this.speed.IsNone) ? this.speed.Value : ((!this.time.IsNone) ? this.time.Value : 1f));
			hashtable.Add("delay", (!this.delay.IsNone) ? this.delay.Value : 0f);
			hashtable.Add("easetype", this.easeType);
			hashtable.Add("looptype", this.loopType);
			hashtable.Add("oncomplete", "iTweenOnComplete");
			hashtable.Add("oncompleteparams", this.itweenID);
			hashtable.Add("onstart", "iTweenOnStart");
			hashtable.Add("onstartparams", this.itweenID);
			hashtable.Add("ignoretimescale", !this.realTime.IsNone && this.realTime.Value);
			hashtable.Add("name", (!this.id.IsNone) ? this.id.Value : string.Empty);
			hashtable.Add("islocal", this.space == Space.Self);
			hashtable.Add("axis", (this.axis != iTweenFsmAction.AxisRestriction.none) ? Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), this.axis) : string.Empty);
			if (!this.orientToPath.IsNone)
			{
				hashtable.Add("orienttopath", this.orientToPath.Value);
			}
			if (!this.lookAtObject.IsNone)
			{
				hashtable.Add("looktarget", (!this.lookAtVector.IsNone) ? (this.lookAtObject.Value.transform.position + this.lookAtVector.Value) : this.lookAtObject.Value.transform.position);
			}
			else if (!this.lookAtVector.IsNone)
			{
				hashtable.Add("looktarget", this.lookAtVector.Value);
			}
			if (!this.lookAtObject.IsNone || !this.lookAtVector.IsNone)
			{
				hashtable.Add("looktime", (!this.lookTime.IsNone) ? this.lookTime.Value : 0f);
			}
			if (this.transforms.Length >= 2)
			{
				this.tempVct3 = new Vector3[this.transforms.Length];
				if (!this.reverse.IsNone && this.reverse.Value)
				{
					for (int i = 0; i < this.transforms.Length; i++)
					{
						if (this.transforms[i].IsNone)
						{
							this.tempVct3[this.tempVct3.Length - 1 - i] = ((!this.vectors[i].IsNone) ? this.vectors[i].Value : Vector3.zero);
						}
						else if (this.transforms[i].Value == null)
						{
							this.tempVct3[this.tempVct3.Length - 1 - i] = ((!this.vectors[i].IsNone) ? this.vectors[i].Value : Vector3.zero);
						}
						else
						{
							this.tempVct3[this.tempVct3.Length - 1 - i] = ((this.space != Space.World) ? this.transforms[i].Value.transform.localPosition : this.transforms[i].Value.transform.position) + ((!this.vectors[i].IsNone) ? this.vectors[i].Value : Vector3.zero);
						}
					}
				}
				else
				{
					for (int j = 0; j < this.transforms.Length; j++)
					{
						if (this.transforms[j].IsNone)
						{
							this.tempVct3[j] = ((!this.vectors[j].IsNone) ? this.vectors[j].Value : Vector3.zero);
						}
						else if (this.transforms[j].Value == null)
						{
							this.tempVct3[j] = ((!this.vectors[j].IsNone) ? this.vectors[j].Value : Vector3.zero);
						}
						else
						{
							this.tempVct3[j] = ((this.space != Space.World) ? ownerDefaultTarget.transform.parent.InverseTransformPoint(this.transforms[j].Value.transform.position) : this.transforms[j].Value.transform.position) + ((!this.vectors[j].IsNone) ? this.vectors[j].Value : Vector3.zero);
						}
					}
				}
				hashtable.Add("path", this.tempVct3);
				hashtable.Add("movetopath", this.moveToPath.IsNone || this.moveToPath.Value);
				hashtable.Add("lookahead", (!this.lookAhead.IsNone) ? this.lookAhead.Value : 1f);
			}
			this.itweenType = "move";
			iTween.MoveTo(ownerDefaultTarget, hashtable);
		}

		// Token: 0x04002877 RID: 10359
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002878 RID: 10360
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x04002879 RID: 10361
		[Tooltip("Move To a transform position.")]
		public FsmGameObject transformPosition;

		// Token: 0x0400287A RID: 10362
		[Tooltip("Position the GameObject will animate to. If Transform Position is defined this is used as a local offset.")]
		public FsmVector3 vectorPosition;

		// Token: 0x0400287B RID: 10363
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x0400287C RID: 10364
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x0400287D RID: 10365
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x0400287E RID: 10366
		[Tooltip("Whether to animate in local or world space.")]
		public Space space;

		// Token: 0x0400287F RID: 10367
		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x04002880 RID: 10368
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x04002881 RID: 10369
		[Tooltip("Whether or not the GameObject will orient to its direction of travel. False by default.")]
		[ActionSection("LookAt")]
		public FsmBool orientToPath;

		// Token: 0x04002882 RID: 10370
		[Tooltip("A target object the GameObject will look at.")]
		public FsmGameObject lookAtObject;

		// Token: 0x04002883 RID: 10371
		[Tooltip("A target position the GameObject will look at.")]
		public FsmVector3 lookAtVector;

		// Token: 0x04002884 RID: 10372
		[Tooltip("The time in seconds the object will take to look at either the Look Target or Orient To Path. 0 by default")]
		public FsmFloat lookTime;

		// Token: 0x04002885 RID: 10373
		[Tooltip("Restricts rotation to the supplied axis only.")]
		public iTweenFsmAction.AxisRestriction axis;

		// Token: 0x04002886 RID: 10374
		[Tooltip("Whether to automatically generate a curve from the GameObject's current position to the beginning of the path. True by default.")]
		[ActionSection("Path")]
		public FsmBool moveToPath;

		// Token: 0x04002887 RID: 10375
		[Tooltip("How much of a percentage (from 0 to 1) to look ahead on a path to influence how strict Orient To Path is and how much the object will anticipate each curve.")]
		public FsmFloat lookAhead;

		// Token: 0x04002888 RID: 10376
		[CompoundArray("Path Nodes", "Transform", "Vector")]
		[Tooltip("A list of objects to draw a Catmull-Rom spline through for a curved animation path.")]
		public FsmGameObject[] transforms;

		// Token: 0x04002889 RID: 10377
		[Tooltip("A list of positions to draw a Catmull-Rom through for a curved animation path. If Transform is defined, this value is added as a local offset.")]
		public FsmVector3[] vectors;

		// Token: 0x0400288A RID: 10378
		[Tooltip("Reverse the path so object moves from End to Start node.")]
		public FsmBool reverse;

		// Token: 0x0400288B RID: 10379
		private Vector3[] tempVct3;
	}
}
