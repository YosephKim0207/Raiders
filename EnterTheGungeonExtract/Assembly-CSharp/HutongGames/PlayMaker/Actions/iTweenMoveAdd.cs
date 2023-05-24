using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009FD RID: 2557
	[ActionCategory("iTween")]
	[Tooltip("Translates a GameObject's position over time.")]
	public class iTweenMoveAdd : iTweenFsmAction
	{
		// Token: 0x060036D6 RID: 14038 RVA: 0x001182F8 File Offset: 0x001164F8
		public override void Reset()
		{
			base.Reset();
			this.id = new FsmString
			{
				UseVariable = true
			};
			this.time = 1f;
			this.delay = 0f;
			this.loopType = iTween.LoopType.none;
			this.vector = new FsmVector3
			{
				UseVariable = true
			};
			this.speed = new FsmFloat
			{
				UseVariable = true
			};
			this.space = Space.World;
			this.orientToPath = false;
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

		// Token: 0x060036D7 RID: 14039 RVA: 0x001183C0 File Offset: 0x001165C0
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x001183E8 File Offset: 0x001165E8
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x001183F8 File Offset: 0x001165F8
		private void DoiTween()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("amount", (!this.vector.IsNone) ? this.vector.Value : Vector3.zero);
			hashtable.Add((!this.speed.IsNone) ? "speed" : "time", (!this.speed.IsNone) ? this.speed.Value : ((!this.time.IsNone) ? this.time.Value : 1f));
			hashtable.Add("delay", (!this.delay.IsNone) ? this.delay.Value : 0f);
			hashtable.Add("easetype", this.easeType);
			hashtable.Add("looptype", this.loopType);
			hashtable.Add("oncomplete", "iTweenOnComplete");
			hashtable.Add("oncompleteparams", this.itweenID);
			hashtable.Add("onstart", "iTweenOnStart");
			hashtable.Add("onstartparams", this.itweenID);
			hashtable.Add("ignoretimescale", !this.realTime.IsNone && this.realTime.Value);
			hashtable.Add("space", this.space);
			hashtable.Add("name", (!this.id.IsNone) ? this.id.Value : string.Empty);
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
			this.itweenType = "move";
			iTween.MoveAdd(ownerDefaultTarget, hashtable);
		}

		// Token: 0x0400284C RID: 10316
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400284D RID: 10317
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x0400284E RID: 10318
		[Tooltip("A vector that will be added to a GameObjects position.")]
		[RequiredField]
		public FsmVector3 vector;

		// Token: 0x0400284F RID: 10319
		[Tooltip("For the time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x04002850 RID: 10320
		[Tooltip("For the time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x04002851 RID: 10321
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x04002852 RID: 10322
		[Tooltip("For the shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x04002853 RID: 10323
		[Tooltip("For the type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x04002854 RID: 10324
		public Space space;

		// Token: 0x04002855 RID: 10325
		[Tooltip("For whether or not the GameObject will orient to its direction of travel. False by default.")]
		[ActionSection("LookAt")]
		public FsmBool orientToPath;

		// Token: 0x04002856 RID: 10326
		[Tooltip("A target object the GameObject will look at.")]
		public FsmGameObject lookAtObject;

		// Token: 0x04002857 RID: 10327
		[Tooltip("A target position the GameObject will look at.")]
		public FsmVector3 lookAtVector;

		// Token: 0x04002858 RID: 10328
		[Tooltip("The time in seconds the object will take to look at either the Look At Target or Orient To Path. 0 by default")]
		public FsmFloat lookTime;

		// Token: 0x04002859 RID: 10329
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis;
	}
}
