using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C16 RID: 3094
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownHoverButton")]
public class tk2dUIUpDownHoverButton : tk2dUIBaseItemControl
{
	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x06004251 RID: 16977 RVA: 0x00157148 File Offset: 0x00155348
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x06004252 RID: 16978 RVA: 0x00157150 File Offset: 0x00155350
	// (set) Token: 0x06004253 RID: 16979 RVA: 0x00157168 File Offset: 0x00155368
	public bool IsOver
	{
		get
		{
			return this.isDown || this.isHover;
		}
		set
		{
			if (value != this.isDown || this.isHover)
			{
				if (value)
				{
					this.isHover = true;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else if (this.isDown && this.isHover)
				{
					this.isDown = false;
					this.isHover = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else if (this.isDown)
				{
					this.isDown = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else
				{
					this.isHover = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				base.DoSendMessage(this.SendMessageOnToggleOverMethodName, this);
			}
		}
	}

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x06004254 RID: 16980 RVA: 0x00157268 File Offset: 0x00155468
	// (remove) Token: 0x06004255 RID: 16981 RVA: 0x001572A0 File Offset: 0x001554A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIUpDownHoverButton> OnToggleOver;

	// Token: 0x06004256 RID: 16982 RVA: 0x001572D8 File Offset: 0x001554D8
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06004257 RID: 16983 RVA: 0x001572E0 File Offset: 0x001554E0
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
			this.uiItem.OnHoverOver += this.ButtonHoverOver;
			this.uiItem.OnHoverOut += this.ButtonHoverOut;
		}
	}

	// Token: 0x06004258 RID: 16984 RVA: 0x00157380 File Offset: 0x00155580
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
			this.uiItem.OnHoverOver -= this.ButtonHoverOver;
			this.uiItem.OnHoverOut -= this.ButtonHoverOut;
		}
	}

	// Token: 0x06004259 RID: 16985 RVA: 0x00157420 File Offset: 0x00155620
	private void ButtonUp()
	{
		if (this.isDown)
		{
			this.isDown = false;
			this.SetState();
			if (!this.isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x0600425A RID: 16986 RVA: 0x0015745C File Offset: 0x0015565C
	private void ButtonDown()
	{
		if (!this.isDown)
		{
			this.isDown = true;
			this.SetState();
			if (!this.isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x0600425B RID: 16987 RVA: 0x00157498 File Offset: 0x00155698
	private void ButtonHoverOver()
	{
		if (!this.isHover)
		{
			this.isHover = true;
			this.SetState();
			if (!this.isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x0600425C RID: 16988 RVA: 0x001574D4 File Offset: 0x001556D4
	private void ButtonHoverOut()
	{
		if (this.isHover)
		{
			this.isHover = false;
			this.SetState();
			if (!this.isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x00157510 File Offset: 0x00155710
	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.upStateGO, !this.isDown && !this.isHover);
		if (this.downStateGO == this.hoverOverStateGO)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown || this.isHover);
		}
		else
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown);
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.hoverOverStateGO, this.isHover);
		}
	}

	// Token: 0x0600425E RID: 16990 RVA: 0x0015759C File Offset: 0x0015579C
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x040034B7 RID: 13495
	public GameObject upStateGO;

	// Token: 0x040034B8 RID: 13496
	public GameObject downStateGO;

	// Token: 0x040034B9 RID: 13497
	public GameObject hoverOverStateGO;

	// Token: 0x040034BA RID: 13498
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x040034BB RID: 13499
	private bool isDown;

	// Token: 0x040034BC RID: 13500
	private bool isHover;

	// Token: 0x040034BD RID: 13501
	public string SendMessageOnToggleOverMethodName = string.Empty;
}
