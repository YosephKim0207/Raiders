using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000760 RID: 1888
	public class TouchSwipeControl : TouchControl
	{
		// Token: 0x06002A02 RID: 10754 RVA: 0x000BF424 File Offset: 0x000BD624
		public override void CreateControl()
		{
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000BF428 File Offset: 0x000BD628
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000BF448 File Offset: 0x000BD648
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000BF464 File Offset: 0x000BD664
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000BF478 File Offset: 0x000BD678
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000BF494 File Offset: 0x000BD694
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 vector = TouchControl.SnapTo(this.currentVector, this.snapAngles);
			base.SubmitAnalogValue(this.target, vector, 0f, 1f, updateTick, deltaTime);
			base.SubmitButtonState(this.upTarget, this.fireButtonTarget && this.nextButtonTarget == this.upTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.downTarget, this.fireButtonTarget && this.nextButtonTarget == this.downTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.leftTarget, this.fireButtonTarget && this.nextButtonTarget == this.leftTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.rightTarget, this.fireButtonTarget && this.nextButtonTarget == this.rightTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget && this.nextButtonTarget == this.tapTarget, updateTick, deltaTime);
			if (this.fireButtonTarget && this.nextButtonTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = !this.oneSwipePerTouch;
				this.lastButtonTarget = this.nextButtonTarget;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000BF5E0 File Offset: 0x000BD7E0
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.upTarget);
			base.CommitButton(this.downTarget);
			base.CommitButton(this.leftTarget);
			base.CommitButton(this.rightTarget);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000BF638 File Offset: 0x000BD838
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.lastPosition = this.beganPosition;
				this.currentTouch = touch;
				this.currentVector = Vector2.zero;
				this.currentVectorIsSet = false;
				this.fireButtonTarget = true;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000BF6B8 File Offset: 0x000BD8B8
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 vector = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector2 = vector - this.lastPosition;
			if (vector2.magnitude >= this.sensitivity)
			{
				this.lastPosition = vector;
				if (!this.oneSwipePerTouch || !this.currentVectorIsSet)
				{
					this.currentVector = vector2.normalized;
					this.currentVectorIsSet = true;
				}
				if (this.fireButtonTarget)
				{
					TouchControl.ButtonTarget buttonTargetForVector = this.GetButtonTargetForVector(this.currentVector);
					if (buttonTargetForVector != this.lastButtonTarget)
					{
						this.nextButtonTarget = buttonTargetForVector;
					}
				}
			}
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000BF760 File Offset: 0x000BD960
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.currentTouch = null;
			this.currentVector = Vector2.zero;
			this.currentVectorIsSet = false;
			Vector3 vector = TouchManager.ScreenToWorldPoint(touch.position);
			if ((this.beganPosition - vector).magnitude < this.sensitivity)
			{
				this.fireButtonTarget = true;
				this.nextButtonTarget = this.tapTarget;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
				return;
			}
			this.fireButtonTarget = false;
			this.nextButtonTarget = TouchControl.ButtonTarget.None;
			this.lastButtonTarget = TouchControl.ButtonTarget.None;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000BF7F4 File Offset: 0x000BD9F4
		private TouchControl.ButtonTarget GetButtonTargetForVector(Vector2 vector)
		{
			Vector2 vector2 = TouchControl.SnapTo(vector, TouchControl.SnapAngles.Four);
			if (vector2 == Vector2.up)
			{
				return this.upTarget;
			}
			if (vector2 == Vector2.right)
			{
				return this.rightTarget;
			}
			if (vector2 == -Vector2.up)
			{
				return this.downTarget;
			}
			if (vector2 == -Vector2.right)
			{
				return this.leftTarget;
			}
			return TouchControl.ButtonTarget.None;
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002A0D RID: 10765 RVA: 0x000BF878 File Offset: 0x000BDA78
		// (set) Token: 0x06002A0E RID: 10766 RVA: 0x000BF880 File Offset: 0x000BDA80
		public Rect ActiveArea
		{
			get
			{
				return this.activeArea;
			}
			set
			{
				if (this.activeArea != value)
				{
					this.activeArea = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002A0F RID: 10767 RVA: 0x000BF8A4 File Offset: 0x000BDAA4
		// (set) Token: 0x06002A10 RID: 10768 RVA: 0x000BF8AC File Offset: 0x000BDAAC
		public TouchUnitType AreaUnitType
		{
			get
			{
				return this.areaUnitType;
			}
			set
			{
				if (this.areaUnitType != value)
				{
					this.areaUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x04001CD2 RID: 7378
		[SerializeField]
		[Header("Position")]
		private TouchUnitType areaUnitType;

		// Token: 0x04001CD3 RID: 7379
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x04001CD4 RID: 7380
		[Range(0f, 1f)]
		[Header("Options")]
		public float sensitivity = 0.1f;

		// Token: 0x04001CD5 RID: 7381
		public bool oneSwipePerTouch;

		// Token: 0x04001CD6 RID: 7382
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target;

		// Token: 0x04001CD7 RID: 7383
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x04001CD8 RID: 7384
		[Header("Button Targets")]
		public TouchControl.ButtonTarget upTarget;

		// Token: 0x04001CD9 RID: 7385
		public TouchControl.ButtonTarget downTarget;

		// Token: 0x04001CDA RID: 7386
		public TouchControl.ButtonTarget leftTarget;

		// Token: 0x04001CDB RID: 7387
		public TouchControl.ButtonTarget rightTarget;

		// Token: 0x04001CDC RID: 7388
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x04001CDD RID: 7389
		private Rect worldActiveArea;

		// Token: 0x04001CDE RID: 7390
		private Vector3 currentVector;

		// Token: 0x04001CDF RID: 7391
		private bool currentVectorIsSet;

		// Token: 0x04001CE0 RID: 7392
		private Vector3 beganPosition;

		// Token: 0x04001CE1 RID: 7393
		private Vector3 lastPosition;

		// Token: 0x04001CE2 RID: 7394
		private Touch currentTouch;

		// Token: 0x04001CE3 RID: 7395
		private bool fireButtonTarget;

		// Token: 0x04001CE4 RID: 7396
		private TouchControl.ButtonTarget nextButtonTarget;

		// Token: 0x04001CE5 RID: 7397
		private TouchControl.ButtonTarget lastButtonTarget;

		// Token: 0x04001CE6 RID: 7398
		private bool dirty;
	}
}
