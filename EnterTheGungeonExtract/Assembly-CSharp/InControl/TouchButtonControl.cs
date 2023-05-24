using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200075C RID: 1884
	public class TouchButtonControl : TouchControl
	{
		// Token: 0x060029D2 RID: 10706 RVA: 0x000BE784 File Offset: 0x000BC984
		public override void CreateControl()
		{
			this.button.Create("Button", base.transform, 1000);
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000BE7A4 File Offset: 0x000BC9A4
		public override void DestroyControl()
		{
			this.button.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x000BE7D0 File Offset: 0x000BC9D0
		public override void ConfigureControl()
		{
			base.transform.position = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, this.lockAspectRatio);
			this.button.Update(true);
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x000BE808 File Offset: 0x000BCA08
		public override void DrawGizmos()
		{
			this.button.DrawGizmos(this.ButtonPosition, Color.yellow);
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000BE820 File Offset: 0x000BCA20
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.button.Update();
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000BE84C File Offset: 0x000BCA4C
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			if (this.pressureSensitive)
			{
				float num = 0f;
				if (this.currentTouch == null)
				{
					if (this.allowSlideToggle)
					{
						int touchCount = TouchManager.TouchCount;
						for (int i = 0; i < touchCount; i++)
						{
							Touch touch = TouchManager.GetTouch(i);
							if (this.button.Contains(touch))
							{
								num = Utility.Max(num, touch.normalizedPressure);
							}
						}
					}
				}
				else
				{
					num = this.currentTouch.normalizedPressure;
				}
				this.ButtonState = num > 0f;
				base.SubmitButtonValue(this.target, num, updateTick, deltaTime);
				return;
			}
			if (this.currentTouch == null && this.allowSlideToggle)
			{
				this.ButtonState = false;
				int touchCount2 = TouchManager.TouchCount;
				for (int j = 0; j < touchCount2; j++)
				{
					this.ButtonState = this.ButtonState || this.button.Contains(TouchManager.GetTouch(j));
				}
			}
			base.SubmitButtonState(this.target, this.ButtonState, updateTick, deltaTime);
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000BE964 File Offset: 0x000BCB64
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitButton(this.target);
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000BE974 File Offset: 0x000BCB74
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			if (this.button.Contains(touch))
			{
				this.ButtonState = true;
				this.currentTouch = touch;
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x000BE9A4 File Offset: 0x000BCBA4
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			if (this.toggleOnLeave && !this.button.Contains(touch))
			{
				this.ButtonState = false;
				this.currentTouch = null;
			}
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000BE9E0 File Offset: 0x000BCBE0
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.ButtonState = false;
			this.currentTouch = null;
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x000BEA00 File Offset: 0x000BCC00
		// (set) Token: 0x060029DD RID: 10717 RVA: 0x000BEA08 File Offset: 0x000BCC08
		private bool ButtonState
		{
			get
			{
				return this.buttonState;
			}
			set
			{
				if (this.buttonState != value)
				{
					this.buttonState = value;
					this.button.State = value;
				}
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000BEA2C File Offset: 0x000BCC2C
		// (set) Token: 0x060029DF RID: 10719 RVA: 0x000BEA5C File Offset: 0x000BCC5C
		public Vector3 ButtonPosition
		{
			get
			{
				return (!this.button.Ready) ? base.transform.position : this.button.Position;
			}
			set
			{
				if (this.button.Ready)
				{
					this.button.Position = value;
				}
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x000BEA7C File Offset: 0x000BCC7C
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x000BEA84 File Offset: 0x000BCC84
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

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x000BEAA0 File Offset: 0x000BCCA0
		// (set) Token: 0x060029E3 RID: 10723 RVA: 0x000BEAA8 File Offset: 0x000BCCA8
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

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x000BEACC File Offset: 0x000BCCCC
		// (set) Token: 0x060029E5 RID: 10725 RVA: 0x000BEAD4 File Offset: 0x000BCCD4
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

		// Token: 0x04001CA1 RID: 7329
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomRight;

		// Token: 0x04001CA2 RID: 7330
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x04001CA3 RID: 7331
		[SerializeField]
		private Vector2 offset = new Vector2(-10f, 10f);

		// Token: 0x04001CA4 RID: 7332
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x04001CA5 RID: 7333
		[Header("Options")]
		public TouchControl.ButtonTarget target = TouchControl.ButtonTarget.Action1;

		// Token: 0x04001CA6 RID: 7334
		public bool allowSlideToggle = true;

		// Token: 0x04001CA7 RID: 7335
		public bool toggleOnLeave;

		// Token: 0x04001CA8 RID: 7336
		public bool pressureSensitive;

		// Token: 0x04001CA9 RID: 7337
		[Header("Sprites")]
		public TouchSprite button = new TouchSprite(15f);

		// Token: 0x04001CAA RID: 7338
		private bool buttonState;

		// Token: 0x04001CAB RID: 7339
		private Touch currentTouch;

		// Token: 0x04001CAC RID: 7340
		private bool dirty;
	}
}
