using System;

// Token: 0x02001837 RID: 6199
public struct Tribool
{
	// Token: 0x060092BE RID: 37566 RVA: 0x003DFF48 File Offset: 0x003DE148
	public Tribool(int v)
	{
		this.value = v;
	}

	// Token: 0x060092BF RID: 37567 RVA: 0x003DFF54 File Offset: 0x003DE154
	public override string ToString()
	{
		return string.Format("[Tribool] " + this.value.ToString(), new object[0]);
	}

	// Token: 0x060092C0 RID: 37568 RVA: 0x003DFF7C File Offset: 0x003DE17C
	public static bool operator true(Tribool a)
	{
		return a.value == 1;
	}

	// Token: 0x060092C1 RID: 37569 RVA: 0x003DFF88 File Offset: 0x003DE188
	public static bool operator false(Tribool a)
	{
		return a.value == 0;
	}

	// Token: 0x060092C2 RID: 37570 RVA: 0x003DFF94 File Offset: 0x003DE194
	public static bool operator !(Tribool a)
	{
		return a.value == 0;
	}

	// Token: 0x060092C3 RID: 37571 RVA: 0x003DFFA0 File Offset: 0x003DE1A0
	public static bool operator ==(Tribool a, Tribool b)
	{
		return a.value == b.value;
	}

	// Token: 0x060092C4 RID: 37572 RVA: 0x003DFFB4 File Offset: 0x003DE1B4
	public static bool operator !=(Tribool a, Tribool b)
	{
		return a.value != b.value;
	}

	// Token: 0x060092C5 RID: 37573 RVA: 0x003DFFCC File Offset: 0x003DE1CC
	public static Tribool operator ++(Tribool a)
	{
		return new Tribool(Math.Min(2, a.value + 1));
	}

	// Token: 0x060092C6 RID: 37574 RVA: 0x003DFFE4 File Offset: 0x003DE1E4
	public override bool Equals(object obj)
	{
		if (obj is Tribool)
		{
			return this == (Tribool)obj;
		}
		return base.Equals(obj);
	}

	// Token: 0x060092C7 RID: 37575 RVA: 0x003E0014 File Offset: 0x003DE214
	public override int GetHashCode()
	{
		return this.value;
	}

	// Token: 0x04009A5A RID: 39514
	public static Tribool Complete = new Tribool(2);

	// Token: 0x04009A5B RID: 39515
	public static Tribool Ready = new Tribool(1);

	// Token: 0x04009A5C RID: 39516
	public static Tribool Unready = new Tribool(0);

	// Token: 0x04009A5D RID: 39517
	public int value;
}
