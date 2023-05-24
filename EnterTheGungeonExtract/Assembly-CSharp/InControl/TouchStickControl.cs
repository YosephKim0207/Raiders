using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200075F RID: 1887
	public class TouchStickControl : TouchControl
	{
		// Token: 0x060029E7 RID: 10727 RVA: 0x000BEBC0 File Offset: 0x000BCDC0
		public override void CreateControl()
		{
			this.ring.Create("Ring", base.transform, 1000);
			this.knob.Create("Knob", base.transform, 1001);
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000BEBF8 File Offset: 0x000BCDF8
		public override void DestroyControl()
		{
			this.ring.Delete();
			this.knob.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000BEC30 File Offset: 0x000BCE30
		public override void ConfigureControl()
		{
			this.resetPosition = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, true);
			base.transform.position = this.resetPosition;
			this.ring.Update(true);
			this.knob.Update(true);
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
			this.worldKnobRange = TouchManager.ConvertToWorld(this.knobRange, this.knob.SizeUnitType);
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000BECB8 File Offset: 0x000BCEB8
		public override void DrawGizmos()
		{
			this.ring.DrawGizmos(this.RingPosition, Color.yellow);
			this.knob.DrawGizmos(this.KnobPosition, Color.yellow);
			Utility.DrawCircleGizmo(this.RingPosition, this.worldKnobRange, Color.red);
			Utility.DrawRectGizmo(this.worldActiveArea, Color.green);
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000BED1C File Offset: 0x000BCF1C
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.ring.Update();
				this.knob.Update();
			}
			if (this.IsNotActive)
			{
				if (this.resetWhenDone && this.KnobPosition != this.resetPosition)
				{
					Vector3 vector = this.KnobPosition - this.RingPosition;
					this.RingPosition = Vector3.MoveTowards(this.RingPosition, this.resetPosition, this.ringResetSpeed * Time.deltaTime);
					this.KnobPosition = this.RingPosition + vector;
				}
				if (this.KnobPosition != this.RingPosition)
				{
					this.KnobPosition = Vector3.MoveTowards(this.KnobPosition, this.RingPosition, this.knobResetSpeed * Time.deltaTime);
				}
			}
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000BEE08 File Offset: 0x000BD008
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			base.SubmitAnalogValue(this.target, this.value, this.lowerDeadZone, this.upperDeadZone, updateTick, deltaTime);
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000BEE30 File Offset: 0x000BD030
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000BEE40 File Offset: 0x000BD040
		public override void TouchBegan(Touch touch)
		{
			if (this.IsActive)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			bool flag = this.worldActiveArea.Contains(this.beganPosition);
			bool flag2 = this.ring.Contains(this.beganPosition);
			if (this.snapToInitialTouch && (flag || flag2))
			{
				this.RingPosition = this.beganPosition;
				this.KnobPosition = this.beganPosition;
				this.currentTouch = touch;
			}
			else if (flag2)
			{
				this.KnobPosition = this.beganPosition;
				this.beganPosition = this.RingPosition;
				this.currentTouch = touch;
			}
			if (this.IsActive)
			{
				this.TouchMoved(touch);
				this.ring.State = true;
				this.knob.State = true;
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000BEF20 File Offset: 0x000BD120
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.movedPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.lockToAxis == LockAxis.Horizontal && this.allowDraggingAxis == DragAxis.Horizontal)
			{
				this.movedPosition.y = this.beganPosition.y;
			}
			else if (this.lockToAxis == LockAxis.Vertical && this.allowDraggingAxis == DragAxis.Vertical)
			{
				this.movedPosition.x = this.beganPosition.x;
			}
			Vector3 vector = this.movedPosition - this.beganPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			if (this.allowDragging)
			{
				float num = magnitude - this.worldKnobRange;
				if (num < 0f)
				{
					num = 0f;
				}
				Vector3 vector2 = num * normalized;
				if (this.allowDraggingAxis == DragAxis.Horizontal)
				{
					vector2.y = 0f;
				}
				else if (this.allowDraggingAxis == DragAxis.Vertical)
				{
					vector2.x = 0f;
				}
				this.beganPosition += vector2;
				this.RingPosition = this.beganPosition;
			}
			this.movedPosition = this.beganPosition + Mathf.Clamp(magnitude, 0f, this.worldKnobRange) * normalized;
			if (this.lockToAxis == LockAxis.Horizontal)
			{
				this.movedPosition.y = this.beganPosition.y;
			}
			else if (this.lockToAxis == LockAxis.Vertical)
			{
				this.movedPosition.x = this.beganPosition.x;
			}
			if (this.snapAngles != TouchControl.SnapAngles.None)
			{
				this.movedPosition = TouchControl.SnapTo(this.movedPosition - this.beganPosition, this.snapAngles) + this.beganPosition;
			}
			this.RingPosition = this.beganPosition;
			this.KnobPosition = this.movedPosition;
			this.value = (this.movedPosition - this.beganPosition) / this.worldKnobRange;
			this.value.x = this.inputCurve.Evaluate(Utility.Abs(this.value.x)) * Mathf.Sign(this.value.x);
			this.value.y = this.inputCurve.Evaluate(Utility.Abs(this.value.y)) * Mathf.Sign(this.value.y);
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x000BF1A8 File Offset: 0x000BD3A8
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.value = Vector3.zero;
			float magnitude = (this.resetPosition - this.RingPosition).magnitude;
			this.ringResetSpeed = ((!Utility.IsZero(this.resetDuration)) ? (magnitude / this.resetDuration) : magnitude);
			float magnitude2 = (this.RingPosition - this.KnobPosition).magnitude;
			this.knobResetSpeed = ((!Utility.IsZero(this.resetDuration)) ? (magnitude2 / this.resetDuration) : this.knobRange);
			this.currentTouch = null;
			this.ring.State = false;
			this.knob.State = false;
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000BF270 File Offset: 0x000BD470
		public bool IsActive
		{
			get
			{
				return this.currentTouch != null;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x000BF280 File Offset: 0x000BD480
		public bool IsNotActive
		{
			get
			{
				return this.currentTouch == null;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000BF28C File Offset: 0x000BD48C
		// (set) Token: 0x060029F4 RID: 10740 RVA: 0x000BF2BC File Offset: 0x000BD4BC
		public Vector3 RingPosition
		{
			get
			{
				return (!this.ring.Ready) ? base.transform.position : this.ring.Position;
			}
			set
			{
				if (this.ring.Ready)
				{
					this.ring.Position = value;
				}
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000BF2DC File Offset: 0x000BD4DC
		// (set) Token: 0x060029F6 RID: 10742 RVA: 0x000BF30C File Offset: 0x000BD50C
		public Vector3 KnobPosition
		{
			get
			{
				return (!this.knob.Ready) ? base.transform.position : this.knob.Position;
			}
			set
			{
				if (this.knob.Ready)
				{
					this.knob.Position = value;
				}
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000BF32C File Offset: 0x000BD52C
		// (set) Token: 0x060029F8 RID: 10744 RVA: 0x000BF334 File Offset: 0x000BD534
		public TouchControlAnchor Anchor
		{
			get
			{
				return this.anchor;
			}
			set
			{
				if (this.anchor != value)
				{
					this.anchor = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x000BF350 File Offset: 0x000BD550
		// (set) Token: 0x060029FA RID: 10746 RVA: 0x000BF358 File Offset: 0x000BD558
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (this.offset != value)
				{
					this.offset = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x000BF37C File Offset: 0x000BD57C
		// (set) Token: 0x060029FC RID: 10748 RVA: 0x000BF384 File Offset: 0x000BD584
		public TouchUnitType OffsetUnitType
		{
			get
			{
				return this.offsetUnitType;
			}
			set
			{
				if (this.offsetUnitType != value)
				{
					this.offsetUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x000BF3A0 File Offset: 0x000BD5A0
		// (set) Token: 0x060029FE RID: 10750 RVA: 0x000BF3A8 File Offset: 0x000BD5A8
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

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x000BF3CC File Offset: 0x000BD5CC
		// (set) Token: 0x06002A00 RID: 10752 RVA: 0x000BF3D4 File Offset: 0x000BD5D4
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

		// Token: 0x04001CB5 RID: 7349
		[SerializeField]
		[Header("Position")]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomLeft;

		// Token: 0x04001CB6 RID: 7350
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x04001CB7 RID: 7351
		[SerializeField]
		private Vector2 offset = new Vector2(20f, 20f);

		// Token: 0x04001CB8 RID: 7352
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x04001CB9 RID: 7353
		[SerializeField]
		private Rect activeArea = new Rect(0f, 0f, 50f, 100f);

		// Token: 0x04001CBA RID: 7354
		[Header("Options")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x04001CBB RID: 7355
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x04001CBC RID: 7356
		public LockAxis lockToAxis;

		// Token: 0x04001CBD RID: 7357
		[Range(0f, 1f)]
		public float lowerDeadZone = 0.1f;

		// Token: 0x04001CBE RID: 7358
		[Range(0f, 1f)]
		public float upperDeadZone = 0.9f;

		// Token: 0x04001CBF RID: 7359
		public AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001CC0 RID: 7360
		public bool allowDragging;

		// Token: 0x04001CC1 RID: 7361
		public DragAxis allowDraggingAxis;

		// Token: 0x04001CC2 RID: 7362
		public bool snapToInitialTouch = true;

		// Token: 0x04001CC3 RID: 7363
		public bool resetWhenDone = true;

		// Token: 0x04001CC4 RID: 7364
		public float resetDuration = 0.1f;

		// Token: 0x04001CC5 RID: 7365
		[Header("Sprites")]
		public TouchSprite ring = new TouchSprite(20f);

		// Token: 0x04001CC6 RID: 7366
		public TouchSprite knob = new TouchSprite(10f);

		// Token: 0x04001CC7 RID: 7367
		public float knobRange = 7.5f;

		// Token: 0x04001CC8 RID: 7368
		private Vector3 resetPosition;

		// Token: 0x04001CC9 RID: 7369
		private Vector3 beganPosition;

		// Token: 0x04001CCA RID: 7370
		private Vector3 movedPosition;

		// Token: 0x04001CCB RID: 7371
		private float ringResetSpeed;

		// Token: 0x04001CCC RID: 7372
		private float knobResetSpeed;

		// Token: 0x04001CCD RID: 7373
		private Rect worldActiveArea;

		// Token: 0x04001CCE RID: 7374
		private float worldKnobRange;

		// Token: 0x04001CCF RID: 7375
		private Vector3 value;

		// Token: 0x04001CD0 RID: 7376
		private Touch currentTouch;

		// Token: 0x04001CD1 RID: 7377
		private bool dirty;
	}
}
