using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000548 RID: 1352
	[Serializable]
	public class fiAnimBool : fiBaseAnimValue<bool>
	{
		// Token: 0x06002026 RID: 8230 RVA: 0x0008F2A4 File Offset: 0x0008D4A4
		public fiAnimBool()
			: base(false)
		{
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0008F2B0 File Offset: 0x0008D4B0
		public fiAnimBool(bool value)
			: base(value)
		{
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x0008F2BC File Offset: 0x0008D4BC
		public float faded
		{
			get
			{
				this.GetValue();
				return this.m_Value;
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0008F2CC File Offset: 0x0008D4CC
		protected override bool GetValue()
		{
			float num = (base.target ? 0f : 1f);
			float num2 = 1f - num;
			this.m_Value = Mathf.Lerp(num, num2, base.lerpPosition);
			return (double)this.m_Value > 0.5;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0008F324 File Offset: 0x0008D524
		public float Fade(float from, float to)
		{
			return Mathf.Lerp(from, to, this.faded);
		}

		// Token: 0x04001786 RID: 6022
		[SerializeField]
		private float m_Value;
	}
}
