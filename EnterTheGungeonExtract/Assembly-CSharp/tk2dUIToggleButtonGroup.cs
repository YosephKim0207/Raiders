using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C11 RID: 3089
[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButtonGroup")]
public class tk2dUIToggleButtonGroup : MonoBehaviour
{
	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x06004229 RID: 16937 RVA: 0x0015685C File Offset: 0x00154A5C
	public tk2dUIToggleButton[] ToggleBtns
	{
		get
		{
			return this.toggleBtns;
		}
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x0600422A RID: 16938 RVA: 0x00156864 File Offset: 0x00154A64
	// (set) Token: 0x0600422B RID: 16939 RVA: 0x0015686C File Offset: 0x00154A6C
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			if (this.selectedIndex != value)
			{
				this.selectedIndex = value;
				this.SetToggleButtonUsingSelectedIndex();
			}
		}
	}

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x0600422C RID: 16940 RVA: 0x00156888 File Offset: 0x00154A88
	// (set) Token: 0x0600422D RID: 16941 RVA: 0x00156890 File Offset: 0x00154A90
	public tk2dUIToggleButton SelectedToggleButton
	{
		get
		{
			return this.selectedToggleButton;
		}
		set
		{
			this.ButtonToggle(value);
		}
	}

	// Token: 0x1400008F RID: 143
	// (add) Token: 0x0600422E RID: 16942 RVA: 0x0015689C File Offset: 0x00154A9C
	// (remove) Token: 0x0600422F RID: 16943 RVA: 0x001568D4 File Offset: 0x00154AD4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIToggleButtonGroup> OnChange;

	// Token: 0x06004230 RID: 16944 RVA: 0x0015690C File Offset: 0x00154B0C
	protected virtual void Awake()
	{
		this.Setup();
	}

	// Token: 0x06004231 RID: 16945 RVA: 0x00156914 File Offset: 0x00154B14
	protected void Setup()
	{
		foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
		{
			if (tk2dUIToggleButton != null)
			{
				tk2dUIToggleButton.IsInToggleGroup = true;
				tk2dUIToggleButton.IsOn = false;
				tk2dUIToggleButton.OnToggle += this.ButtonToggle;
			}
		}
		this.SetToggleButtonUsingSelectedIndex();
	}

	// Token: 0x06004232 RID: 16946 RVA: 0x00156974 File Offset: 0x00154B74
	public void AddNewToggleButtons(tk2dUIToggleButton[] newToggleBtns)
	{
		this.ClearExistingToggleBtns();
		this.toggleBtns = newToggleBtns;
		this.Setup();
	}

	// Token: 0x06004233 RID: 16947 RVA: 0x0015698C File Offset: 0x00154B8C
	private void ClearExistingToggleBtns()
	{
		if (this.toggleBtns != null && this.toggleBtns.Length > 0)
		{
			foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
			{
				tk2dUIToggleButton.IsInToggleGroup = false;
				tk2dUIToggleButton.OnToggle -= this.ButtonToggle;
				tk2dUIToggleButton.IsOn = false;
			}
		}
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x001569F4 File Offset: 0x00154BF4
	private void SetToggleButtonUsingSelectedIndex()
	{
		if (this.selectedIndex >= 0 && this.selectedIndex < this.toggleBtns.Length)
		{
			tk2dUIToggleButton tk2dUIToggleButton = this.toggleBtns[this.selectedIndex];
			tk2dUIToggleButton.IsOn = true;
		}
		else
		{
			tk2dUIToggleButton tk2dUIToggleButton = null;
			this.selectedIndex = -1;
			this.ButtonToggle(tk2dUIToggleButton);
		}
	}

	// Token: 0x06004235 RID: 16949 RVA: 0x00156A4C File Offset: 0x00154C4C
	private void ButtonToggle(tk2dUIToggleButton toggleButton)
	{
		if (toggleButton == null || toggleButton.IsOn)
		{
			foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
			{
				if (tk2dUIToggleButton != toggleButton)
				{
					tk2dUIToggleButton.IsOn = false;
				}
			}
			if (toggleButton != this.selectedToggleButton)
			{
				this.selectedToggleButton = toggleButton;
				this.SetSelectedIndexFromSelectedToggleButton();
				if (this.OnChange != null)
				{
					this.OnChange(this);
				}
				if (this.sendMessageTarget != null && this.SendMessageOnChangeMethodName.Length > 0)
				{
					this.sendMessageTarget.SendMessage(this.SendMessageOnChangeMethodName, this, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}

	// Token: 0x06004236 RID: 16950 RVA: 0x00156B0C File Offset: 0x00154D0C
	private void SetSelectedIndexFromSelectedToggleButton()
	{
		this.selectedIndex = -1;
		for (int i = 0; i < this.toggleBtns.Length; i++)
		{
			tk2dUIToggleButton tk2dUIToggleButton = this.toggleBtns[i];
			if (tk2dUIToggleButton == this.selectedToggleButton)
			{
				this.selectedIndex = i;
				break;
			}
		}
	}

	// Token: 0x0400349F RID: 13471
	[SerializeField]
	private tk2dUIToggleButton[] toggleBtns;

	// Token: 0x040034A0 RID: 13472
	public GameObject sendMessageTarget;

	// Token: 0x040034A1 RID: 13473
	[SerializeField]
	private int selectedIndex;

	// Token: 0x040034A2 RID: 13474
	private tk2dUIToggleButton selectedToggleButton;

	// Token: 0x040034A4 RID: 13476
	public string SendMessageOnChangeMethodName = string.Empty;
}
