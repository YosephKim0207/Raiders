using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009FE RID: 2558
	[Tooltip("Adds the supplied vector to a GameObject's position.")]
	[ActionCategory("iTween")]
	public class iTweenMoveBy : iTweenFsmAction
	{
		// Token: 0x060036DB RID: 14043 RVA: 0x00118770 File Offset: 0x00116970
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

		// Token: 0x060036DC RID: 14044 RVA: 0x00118838 File Offset: 0x00116A38
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x00118860 File Offset: 0x00116A60
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x00118870 File Offset: 0x00116A70
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
			iTween.MoveBy(ownerDefaultTarget, hashtable);
		}

		// Token: 0x0400285A RID: 10330
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400285B RID: 10331
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x0400285C RID: 10332
		[Tooltip("The vector to add to the GameObject's position.")]
		[RequiredField]
		public FsmVector3 vector;

		// Token: 0x0400285D RID: 10333
		[Tooltip("For the time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x0400285E RID: 10334
		[Tooltip("For the time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x0400285F RID: 10335
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x04002860 RID: 10336
		[Tooltip("For the shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x04002861 RID: 10337
		[Tooltip("For the type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x04002862 RID: 10338
		public Space space;

		// Token: 0x04002863 RID: 10339
		[Tooltip("For whether or not the GameObject will orient to its direction of travel. False by default.")]
		[ActionSection("LookAt")]
		public FsmBool orientToPath;

		// Token: 0x04002864 RID: 10340
		[Tooltip("For a target the GameObject will look at.")]
		public FsmGameObject lookAtObject;

		// Token: 0x04002865 RID: 10341
		[Tooltip("For a target the GameObject will look at.")]
		public FsmVector3 lookAtVector;

		// Token: 0x04002866 RID: 10342
		[Tooltip("For the time in seconds the object will take to look at either the 'looktarget' or 'orienttopath'. 0 by default")]
		public FsmFloat lookTime;

		// Token: 0x04002867 RID: 10343
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis;
	}
}
