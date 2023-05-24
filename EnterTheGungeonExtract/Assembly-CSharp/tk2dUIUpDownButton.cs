using System;
using UnityEngine;

// Token: 0x02000C15 RID: 3093
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownButton")]
public class tk2dUIUpDownButton : tk2dUIBaseItemControl
{
	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x06004248 RID: 16968 RVA: 0x00156FE8 File Offset: 0x001551E8
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x00156FF0 File Offset: 0x001551F0
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x00156FF8 File Offset: 0x001551F8
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			if (this.useOnReleaseInsteadOfOnUp)
			{
				this.uiItem.OnRelease += this.ButtonUp;
			}
			else
			{
				this.uiItem.OnUp += this.ButtonUp;
			}
		}
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x0015706C File Offset: 0x0015526C
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			if (this.useOnReleaseInsteadOfOnUp)
			{
				this.uiItem.OnRelease -= this.ButtonUp;
			}
			else
			{
				this.uiItem.OnUp -= this.ButtonUp;
			}
		}
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x001570E0 File Offset: 0x001552E0
	private void ButtonUp()
	{
		this.isDown = false;
		this.SetState();
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x001570F0 File Offset: 0x001552F0
	private void ButtonDown()
	{
		this.isDown = true;
		this.SetState();
	}

	// Token: 0x0600424E RID: 16974 RVA: 0x00157100 File Offset: 0x00155300
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.upStateGO, !this.isDown);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown);
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x00157128 File Offset: 0x00155328
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x040034B3 RID: 13491
	public GameObject upStateGO;

	// Token: 0x040034B4 RID: 13492
	public GameObject downStateGO;

	// Token: 0x040034B5 RID: 13493
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x040034B6 RID: 13494
	private bool isDown;
}
