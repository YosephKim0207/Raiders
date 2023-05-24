using System;
using UnityEngine;

// Token: 0x020013C8 RID: 5064
public class MagazineRackItem : PlayerItem
{
	// Token: 0x060072D8 RID: 29400 RVA: 0x002DA5E4 File Offset: 0x002D87E4
	public override bool CanBeUsed(PlayerController user)
	{
		return !this.m_instanceRack && base.CanBeUsed(user);
	}

	// Token: 0x060072D9 RID: 29401 RVA: 0x002DA600 File Offset: 0x002D8800
	protected override void DoEffect(PlayerController user)
	{
		this.m_instanceRack = UnityEngine.Object.Instantiate<GameObject>(this.MagazineRackPrefab, user.CenterPosition.ToVector3ZisY(0f), Quaternion.identity, null);
	}

	// Token: 0x0400742C RID: 29740
	public GameObject MagazineRackPrefab;

	// Token: 0x0400742D RID: 29741
	private GameObject m_instanceRack;
}
