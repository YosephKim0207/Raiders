using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000547 RID: 1351
	[Serializable]
	public class fiAnimFloat : fiBaseAnimValue<float>
	{
		// Token: 0x06002024 RID: 8228 RVA: 0x0008F270 File Offset: 0x0008D470
		public fiAnimFloat(float value)
			: base(value)
		{
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0008F27C File Offset: 0x0008D47C
		protected override float GetValue()
		{
			this.m_Value = Mathf.Lerp(base.start, base.target, base.lerpPosition);
			return this.m_Value;
		}

		// Token: 0x04001785 RID: 6021
		[SerializeField]
		private float m_Value;
	}
}
