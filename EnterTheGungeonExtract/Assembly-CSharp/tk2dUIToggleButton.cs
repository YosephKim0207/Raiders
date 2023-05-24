using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C10 RID: 3088
[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButton")]
public class tk2dUIToggleButton : tk2dUIBaseItemControl
{
	// Token: 0x1400008E RID: 142
	// (add) Token: 0x0600421B RID: 16923 RVA: 0x00156654 File Offset: 0x00154854
	// (remove) Token: 0x0600421C RID: 16924 RVA: 0x0015668C File Offset: 0x0015488C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIToggleButton> OnToggle;

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x0600421D RID: 16925 RVA: 0x001566C4 File Offset: 0x001548C4
	// (set) Token: 0x0600421E RID: 16926 RVA: 0x001566CC File Offset: 0x001548CC
	public bool IsOn
	{
		get
		{
			return this.isOn;
		}
		set
		{
			if (this.isOn != value)
			{
				this.isOn = value;
				this.SetState();
				if (this.OnToggle != null)
				{
					this.OnToggle(this);
				}
			}
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x0600421F RID: 16927 RVA: 0x00156700 File Offset: 0x00154900
	// (set) Token: 0x06004220 RID: 16928 RVA: 0x00156708 File Offset: 0x00154908
	public bool IsInToggleGroup
	{
		get
		{
			return this.isInToggleGroup;
		}
		set
		{
			this.isInToggleGroup = value;
		}
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x00156714 File Offset: 0x00154914
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x0015671C File Offset: 0x0015491C
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ButtonClick;
			this.uiItem.OnDown += this.ButtonDown;
		}
	}

	// Token: 0x06004223 RID: 16931 RVA: 0x0015675C File Offset: 0x0015495C
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ButtonClick;
			this.uiItem.OnDown -= this.ButtonDown;
		}
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x0015679C File Offset: 0x0015499C
	private void ButtonClick()
	{
		if (!this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x001567B0 File Offset: 0x001549B0
	private void ButtonDown()
	{
		if (this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x06004226 RID: 16934 RVA: 0x001567C4 File Offset: 0x001549C4
	private void ButtonToggle()
	{
		if (!this.isOn || !this.isInToggleGroup)
		{
			this.isOn = !this.isOn;
			this.SetState();
			if (this.OnToggle != null)
			{
				this.OnToggle(this);
			}
			base.DoSendMessage(this.SendMessageOnToggleMethodName, this);
		}
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x00156820 File Offset: 0x00154A20
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.offStateGO, !this.isOn);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.onStateGO, this.isOn);
	}

	// Token: 0x04003498 RID: 13464
	public GameObject offStateGO;

	// Token: 0x04003499 RID: 13465
	public GameObject onStateGO;

	// Token: 0x0400349A RID: 13466
	public bool activateOnPress;

	// Token: 0x0400349B RID: 13467
	[SerializeField]
	private bool isOn = true;

	// Token: 0x0400349C RID: 13468
	private bool isInToggleGroup;

	// Token: 0x0400349E RID: 13470
	public string SendMessageOnToggleMethodName = string.Empty;
}
