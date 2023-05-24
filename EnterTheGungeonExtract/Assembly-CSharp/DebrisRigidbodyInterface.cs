using System;

// Token: 0x02001143 RID: 4419
public class DebrisRigidbodyInterface : BraveBehaviour
{
	// Token: 0x060061E5 RID: 25061 RVA: 0x0025EE24 File Offset: 0x0025D024
	private void Start()
	{
		if (this.IsWall)
		{
			DebrisObject.SRB_Walls.Add(base.specRigidbody);
		}
		if (this.IsPit)
		{
			base.specRigidbody.PrimaryPixelCollider.IsTrigger = true;
			DebrisObject.SRB_Pits.Add(base.specRigidbody);
		}
	}

	// Token: 0x060061E6 RID: 25062 RVA: 0x0025EE78 File Offset: 0x0025D078
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005CCF RID: 23759
	public bool IsWall;

	// Token: 0x04005CD0 RID: 23760
	public bool IsPit;
}
