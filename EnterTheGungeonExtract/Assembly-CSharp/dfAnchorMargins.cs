using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
[Serializable]
public class dfAnchorMargins
{
	// Token: 0x060011C0 RID: 4544 RVA: 0x00052B44 File Offset: 0x00050D44
	public override string ToString()
	{
		return string.Format("[L:{0},T:{1},R:{2},B:{3}]", new object[] { this.left, this.top, this.right, this.bottom });
	}

	// Token: 0x04000FEC RID: 4076
	[SerializeField]
	public float left;

	// Token: 0x04000FED RID: 4077
	[SerializeField]
	public float top;

	// Token: 0x04000FEE RID: 4078
	[SerializeField]
	public float right;

	// Token: 0x04000FEF RID: 4079
	[SerializeField]
	public float bottom;
}
