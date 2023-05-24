using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000761 RID: 1889
	public class TouchTrackControl : TouchControl
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x000BF924 File Offset: 0x000BDB24
		public override void CreateControl()
		{
			this.ConfigureControl();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000BF92C File Offset: 0x000BDB2C
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000BF94C File Offset: 0x000BDB4C
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000BF968 File Offset: 0x000BDB68
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000BF97C File Offset: 0x000BDB7C
		private void OnValidate()
		{
			if (this.maxTapDuration < 0f)
			{
				this.maxTapDuration = 0f;
			}
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000BF99C File Offset: 0x000BDB9C
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000BF9B8 File Offset: 0x000BDBB8
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 vector = this.thisPosition - this.lastPosition;
			base.SubmitRawAnalogValue(this.target, vector * this.scale, updateTick, deltaTime);
			this.lastPosition = this.thisPosition;
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget, updateTick, deltaTime);
			this.fireButtonTarget = false;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000BFA20 File Offset: 0x000BDC20
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000BFA3C File Offset: 0x000BDC3C
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
				this.lastPosition = this.thisPosition;
				this.currentTouch = touch;
				this.beganTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000BFAB8 File Offset: 0x000BDCB8
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000BFAE4 File Offset: 0x000BDCE4
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 vector = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector2 = vector - this.beganPosition;
			float num = Time.realtimeSinceStartup - this.beganTime;
			if (vector2.magnitude <= this.maxTapMovement && num <= this.maxTapDuration && this.tapTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = true;
			}
			this.thisPosition = Vector3.zero;
			this.lastPosition = Vector3.zero;
			this.currentTouch = null;
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000BFB74 File Offset: 0x000BDD74
		// (set) Token: 0x06002A1E RID: 10782 RVA: 0x000BFB7C File Offset: 0x000BDD7C
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

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000BFBA0 File Offset: 0x000BDDA0
		// (set) Token: 0x06002A20 RID: 10784 RVA: 0x000BFBA8 File Offset: 0x000BDDA8
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

		// Token: 0x04001CE7 RID: 7399
		[Header("Dimensions")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x04001CE8 RID: 7400
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x04001CE9 RID: 7401
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x04001CEA RID: 7402
		public float scale = 1f;

		// Token: 0x04001CEB RID: 7403
		[Header("Button Target")]
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x04001CEC RID: 7404
		public float maxTapDuration = 0.5f;

		// Token: 0x04001CED RID: 7405
		public float maxTapMovement = 1f;

		// Token: 0x04001CEE RID: 7406
		private Rect worldActiveArea;

		// Token: 0x04001CEF RID: 7407
		private Vector3 lastPosition;

		// Token: 0x04001CF0 RID: 7408
		private Vector3 thisPosition;

		// Token: 0x04001CF1 RID: 7409
		private Touch currentTouch;

		// Token: 0x04001CF2 RID: 7410
		private bool dirty;

		// Token: 0x04001CF3 RID: 7411
		private bool fireButtonTarget;

		// Token: 0x04001CF4 RID: 7412
		private float beganTime;

		// Token: 0x04001CF5 RID: 7413
		private Vector3 beganPosition;
	}
}
