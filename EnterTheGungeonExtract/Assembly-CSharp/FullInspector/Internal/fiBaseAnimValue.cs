using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000546 RID: 1350
	public abstract class fiBaseAnimValue<T>
	{
		// Token: 0x06002016 RID: 8214 RVA: 0x0008F028 File Offset: 0x0008D228
		protected fiBaseAnimValue(T value)
		{
			this.m_Start = value;
			this.m_Target = value;
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x0008F058 File Offset: 0x0008D258
		public bool isAnimating
		{
			get
			{
				return this.m_Animating;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x0008F060 File Offset: 0x0008D260
		protected float lerpPosition
		{
			get
			{
				double num = 1.0 - this.m_LerpPosition;
				return (float)(1.0 - num * num * num * num);
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x0008F090 File Offset: 0x0008D290
		protected T start
		{
			get
			{
				return this.m_Start;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0008F098 File Offset: 0x0008D298
		// (set) Token: 0x0600201B RID: 8219 RVA: 0x0008F0A0 File Offset: 0x0008D2A0
		public T target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				if (this.m_Target.Equals(value))
				{
					return;
				}
				this.BeginAnimating(value, this.value);
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x0008F0CC File Offset: 0x0008D2CC
		// (set) Token: 0x0600201D RID: 8221 RVA: 0x0008F0D4 File Offset: 0x0008D2D4
		public T value
		{
			get
			{
				return this.GetValue();
			}
			set
			{
				this.StopAnim(value);
			}
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0008F0E0 File Offset: 0x0008D2E0
		private static T2 Clamp<T2>(T2 val, T2 min, T2 max) where T2 : IComparable<T2>
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			if (val.CompareTo(max) > 0)
			{
				return max;
			}
			return val;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0008F110 File Offset: 0x0008D310
		protected void BeginAnimating(T newTarget, T newStart)
		{
			this.m_Start = newStart;
			this.m_Target = newTarget;
			fiLateBindings.EditorApplication.AddUpdateFunc(new Action(this.Update));
			this.m_Animating = true;
			this.m_LastTime = fiLateBindings.EditorApplication.timeSinceStartup;
			this.m_LerpPosition = 0.0;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0008F160 File Offset: 0x0008D360
		private void Update()
		{
			if (!this.m_Animating)
			{
				return;
			}
			this.UpdateLerpPosition();
			if ((double)this.lerpPosition < 1.0)
			{
				return;
			}
			this.m_Animating = false;
			fiLateBindings.EditorApplication.RemUpdateFunc(new Action(this.Update));
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0008F1B0 File Offset: 0x0008D3B0
		private void UpdateLerpPosition()
		{
			double timeSinceStartup = fiLateBindings.EditorApplication.timeSinceStartup;
			this.m_LerpPosition = fiBaseAnimValue<T>.Clamp<double>(this.m_LerpPosition + (timeSinceStartup - this.m_LastTime) * (double)this.speed, 0.0, 1.0);
			this.m_LastTime = timeSinceStartup;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0008F200 File Offset: 0x0008D400
		protected void StopAnim(T newValue)
		{
			bool flag = false;
			if (!newValue.Equals(this.GetValue()) || this.m_LerpPosition < 1.0)
			{
				flag = true;
			}
			this.m_Target = newValue;
			this.m_Start = newValue;
			this.m_LerpPosition = 1.0;
			this.m_Animating = false;
			if (!flag)
			{
				return;
			}
		}

		// Token: 0x06002023 RID: 8227
		protected abstract T GetValue();

		// Token: 0x0400177F RID: 6015
		private double m_LerpPosition = 1.0;

		// Token: 0x04001780 RID: 6016
		public float speed = 2f;

		// Token: 0x04001781 RID: 6017
		private T m_Start;

		// Token: 0x04001782 RID: 6018
		[SerializeField]
		private T m_Target;

		// Token: 0x04001783 RID: 6019
		private double m_LastTime;

		// Token: 0x04001784 RID: 6020
		private bool m_Animating;
	}
}
