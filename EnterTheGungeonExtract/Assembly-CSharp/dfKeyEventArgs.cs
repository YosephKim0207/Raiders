using System;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public class dfKeyEventArgs : dfControlEventArgs
{
	// Token: 0x06001208 RID: 4616 RVA: 0x0005311C File Offset: 0x0005131C
	internal dfKeyEventArgs(dfControl source, KeyCode Key, bool Control, bool Shift, bool Alt)
		: base(source)
	{
		this.KeyCode = Key;
		this.Control = Control;
		this.Shift = Shift;
		this.Alt = Alt;
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06001209 RID: 4617 RVA: 0x00053144 File Offset: 0x00051344
	// (set) Token: 0x0600120A RID: 4618 RVA: 0x0005314C File Offset: 0x0005134C
	public KeyCode KeyCode { get; set; }

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x0600120B RID: 4619 RVA: 0x00053158 File Offset: 0x00051358
	// (set) Token: 0x0600120C RID: 4620 RVA: 0x00053160 File Offset: 0x00051360
	public char Character { get; set; }

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x0600120D RID: 4621 RVA: 0x0005316C File Offset: 0x0005136C
	// (set) Token: 0x0600120E RID: 4622 RVA: 0x00053174 File Offset: 0x00051374
	public bool Control { get; set; }

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600120F RID: 4623 RVA: 0x00053180 File Offset: 0x00051380
	// (set) Token: 0x06001210 RID: 4624 RVA: 0x00053188 File Offset: 0x00051388
	public bool Shift { get; set; }

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06001211 RID: 4625 RVA: 0x00053194 File Offset: 0x00051394
	// (set) Token: 0x06001212 RID: 4626 RVA: 0x0005319C File Offset: 0x0005139C
	public bool Alt { get; set; }

	// Token: 0x06001213 RID: 4627 RVA: 0x000531A8 File Offset: 0x000513A8
	public override string ToString()
	{
		return string.Format("Key: {0}, Control: {1}, Shift: {2}, Alt: {3}", new object[] { this.KeyCode, this.Control, this.Shift, this.Alt });
	}
}
