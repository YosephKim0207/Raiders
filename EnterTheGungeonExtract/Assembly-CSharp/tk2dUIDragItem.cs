using System;
using UnityEngine;

// Token: 0x02000C04 RID: 3076
[AddComponentMenu("2D Toolkit/UI/tk2dUIDragItem")]
public class tk2dUIDragItem : tk2dUIBaseItemControl
{
	// Token: 0x06004168 RID: 16744 RVA: 0x00152B04 File Offset: 0x00150D04
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			this.uiItem.OnRelease += this.ButtonRelease;
		}
	}

	// Token: 0x06004169 RID: 16745 RVA: 0x00152B44 File Offset: 0x00150D44
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			this.uiItem.OnRelease -= this.ButtonRelease;
		}
		if (this.isBtnActive)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.UpdateBtnPosition;
			}
			this.isBtnActive = false;
		}
	}

	// Token: 0x0600416A RID: 16746 RVA: 0x00152BC8 File Offset: 0x00150DC8
	private void UpdateBtnPosition()
	{
		base.transform.position = this.CalculateNewPos();
	}

	// Token: 0x0600416B RID: 16747 RVA: 0x00152BDC File Offset: 0x00150DDC
	private Vector3 CalculateNewPos()
	{
		Vector2 position = this.uiItem.Touch.position;
		Camera uicameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		Vector3 vector = uicameraForControl.ScreenToWorldPoint(new Vector3(position.x, position.y, base.transform.position.z - uicameraForControl.transform.position.z));
		vector.z = base.transform.position.z;
		return vector + this.offset;
	}

	// Token: 0x0600416C RID: 16748 RVA: 0x00152C7C File Offset: 0x00150E7C
	public void ButtonDown()
	{
		if (!this.isBtnActive)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.UpdateBtnPosition;
		}
		this.isBtnActive = true;
		this.offset = Vector3.zero;
		Vector3 vector = this.CalculateNewPos();
		this.offset = base.transform.position - vector;
	}

	// Token: 0x0600416D RID: 16749 RVA: 0x00152CDC File Offset: 0x00150EDC
	public void ButtonRelease()
	{
		if (this.isBtnActive)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.UpdateBtnPosition;
		}
		this.isBtnActive = false;
	}

	// Token: 0x0400341B RID: 13339
	public tk2dUIManager uiManager;

	// Token: 0x0400341C RID: 13340
	private Vector3 offset = Vector3.zero;

	// Token: 0x0400341D RID: 13341
	private bool isBtnActive;
}
