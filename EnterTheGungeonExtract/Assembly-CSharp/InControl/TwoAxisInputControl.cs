using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006AA RID: 1706
	public class TwoAxisInputControl : IInputControl
	{
		// Token: 0x0600278A RID: 10122 RVA: 0x000A8EC8 File Offset: 0x000A70C8
		public TwoAxisInputControl()
		{
			this.Left = new OneAxisInputControl();
			this.Right = new OneAxisInputControl();
			this.Up = new OneAxisInputControl();
			this.Down = new OneAxisInputControl();
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x000A8F20 File Offset: 0x000A7120
		// (set) Token: 0x0600278C RID: 10124 RVA: 0x000A8F28 File Offset: 0x000A7128
		public float X { get; protected set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x000A8F34 File Offset: 0x000A7134
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x000A8F3C File Offset: 0x000A713C
		public float Y { get; protected set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000A8F48 File Offset: 0x000A7148
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x000A8F50 File Offset: 0x000A7150
		public OneAxisInputControl Left { get; protected set; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x000A8F5C File Offset: 0x000A715C
		// (set) Token: 0x06002792 RID: 10130 RVA: 0x000A8F64 File Offset: 0x000A7164
		public OneAxisInputControl Right { get; protected set; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x000A8F70 File Offset: 0x000A7170
		// (set) Token: 0x06002794 RID: 10132 RVA: 0x000A8F78 File Offset: 0x000A7178
		public OneAxisInputControl Up { get; protected set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x000A8F84 File Offset: 0x000A7184
		// (set) Token: 0x06002796 RID: 10134 RVA: 0x000A8F8C File Offset: 0x000A718C
		public OneAxisInputControl Down { get; protected set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002797 RID: 10135 RVA: 0x000A8F98 File Offset: 0x000A7198
		// (set) Token: 0x06002798 RID: 10136 RVA: 0x000A8FA0 File Offset: 0x000A71A0
		public ulong UpdateTick { get; protected set; }

		// Token: 0x06002799 RID: 10137 RVA: 0x000A8FAC File Offset: 0x000A71AC
		public void ClearInputState()
		{
			this.Left.ClearInputState();
			this.Right.ClearInputState();
			this.Up.ClearInputState();
			this.Down.ClearInputState();
			this.lastState = false;
			this.lastValue = Vector2.zero;
			this.thisState = false;
			this.thisValue = Vector2.zero;
			this.X = 0f;
			this.Y = 0f;
			this.clearInputState = true;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000A9028 File Offset: 0x000A7228
		public void Filter(TwoAxisInputControl twoAxisInputControl, float deltaTime)
		{
			this.UpdateWithAxes(twoAxisInputControl.X, twoAxisInputControl.Y, InputManager.CurrentTick, deltaTime);
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000A9044 File Offset: 0x000A7244
		internal void UpdateWithAxes(float x, float y, ulong updateTick, float deltaTime)
		{
			this.lastState = this.thisState;
			this.lastValue = this.thisValue;
			this.thisValue = ((!this.Raw) ? Utility.ApplyCircularDeadZone(x, y, this.LowerDeadZone, this.UpperDeadZone) : new Vector2(x, y));
			this.X = this.thisValue.x;
			this.Y = this.thisValue.y;
			this.Left.CommitWithValue(Mathf.Max(0f, -this.X), updateTick, deltaTime);
			this.Right.CommitWithValue(Mathf.Max(0f, this.X), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.Up.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
			}
			else
			{
				this.Up.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
			}
			this.thisState = this.Up.State || this.Down.State || this.Left.State || this.Right.State;
			if (this.clearInputState)
			{
				this.lastState = this.thisState;
				this.lastValue = this.thisValue;
				this.clearInputState = false;
				this.HasChanged = false;
				return;
			}
			if (this.thisValue != this.lastValue)
			{
				this.UpdateTick = updateTick;
				this.HasChanged = true;
			}
			else
			{
				this.HasChanged = false;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000A9228 File Offset: 0x000A7428
		// (set) Token: 0x0600279D RID: 10141 RVA: 0x000A9230 File Offset: 0x000A7430
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
				this.Left.Sensitivity = this.sensitivity;
				this.Right.Sensitivity = this.sensitivity;
				this.Up.Sensitivity = this.sensitivity;
				this.Down.Sensitivity = this.sensitivity;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x000A9290 File Offset: 0x000A7490
		// (set) Token: 0x0600279F RID: 10143 RVA: 0x000A9298 File Offset: 0x000A7498
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
				this.Left.StateThreshold = this.stateThreshold;
				this.Right.StateThreshold = this.stateThreshold;
				this.Up.StateThreshold = this.stateThreshold;
				this.Down.StateThreshold = this.stateThreshold;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000A92F8 File Offset: 0x000A74F8
		// (set) Token: 0x060027A1 RID: 10145 RVA: 0x000A9300 File Offset: 0x000A7500
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
				this.Left.LowerDeadZone = this.lowerDeadZone;
				this.Right.LowerDeadZone = this.lowerDeadZone;
				this.Up.LowerDeadZone = this.lowerDeadZone;
				this.Down.LowerDeadZone = this.lowerDeadZone;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060027A2 RID: 10146 RVA: 0x000A9360 File Offset: 0x000A7560
		// (set) Token: 0x060027A3 RID: 10147 RVA: 0x000A9368 File Offset: 0x000A7568
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
				this.Left.UpperDeadZone = this.upperDeadZone;
				this.Right.UpperDeadZone = this.upperDeadZone;
				this.Up.UpperDeadZone = this.upperDeadZone;
				this.Down.UpperDeadZone = this.upperDeadZone;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x000A93C8 File Offset: 0x000A75C8
		public bool State
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060027A5 RID: 10149 RVA: 0x000A93D0 File Offset: 0x000A75D0
		public bool LastState
		{
			get
			{
				return this.lastState;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000A93D8 File Offset: 0x000A75D8
		public Vector2 Value
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x000A93E0 File Offset: 0x000A75E0
		public Vector2 LastValue
		{
			get
			{
				return this.lastValue;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000A93E8 File Offset: 0x000A75E8
		public Vector2 Vector
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000A93F0 File Offset: 0x000A75F0
		// (set) Token: 0x060027AA RID: 10154 RVA: 0x000A93F8 File Offset: 0x000A75F8
		public bool HasChanged { get; protected set; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000A9404 File Offset: 0x000A7604
		public bool IsPressed
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x000A940C File Offset: 0x000A760C
		public bool WasPressed
		{
			get
			{
				return this.thisState && !this.lastState;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060027AD RID: 10157 RVA: 0x000A9428 File Offset: 0x000A7628
		public bool WasReleased
		{
			get
			{
				return !this.thisState && this.lastState;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x000A9440 File Offset: 0x000A7640
		public float Angle
		{
			get
			{
				return Utility.VectorToAngle(this.thisValue);
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000A9450 File Offset: 0x000A7650
		public static implicit operator bool(TwoAxisInputControl instance)
		{
			return instance.thisState;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000A9458 File Offset: 0x000A7658
		public static implicit operator Vector2(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000A9460 File Offset: 0x000A7660
		public static implicit operator Vector3(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x04001BAA RID: 7082
		public static readonly TwoAxisInputControl Null = new TwoAxisInputControl();

		// Token: 0x04001BB2 RID: 7090
		private float sensitivity = 1f;

		// Token: 0x04001BB3 RID: 7091
		private float lowerDeadZone;

		// Token: 0x04001BB4 RID: 7092
		private float upperDeadZone = 1f;

		// Token: 0x04001BB5 RID: 7093
		private float stateThreshold;

		// Token: 0x04001BB6 RID: 7094
		public bool Raw;

		// Token: 0x04001BB7 RID: 7095
		private bool thisState;

		// Token: 0x04001BB8 RID: 7096
		private bool lastState;

		// Token: 0x04001BB9 RID: 7097
		private Vector2 thisValue;

		// Token: 0x04001BBA RID: 7098
		private Vector2 lastValue;

		// Token: 0x04001BBB RID: 7099
		private bool clearInputState;
	}
}
