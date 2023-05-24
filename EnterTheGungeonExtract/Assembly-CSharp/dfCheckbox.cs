using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000390 RID: 912
[AddComponentMenu("Daikon Forge/User Interface/Checkbox")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_checkbox.html")]
[dfCategory("Basic Controls")]
[ExecuteInEditMode]
[dfTooltip("Implements a standard checkbox (or toggle) control")]
[Serializable]
public class dfCheckbox : dfControl
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000FF4 RID: 4084 RVA: 0x00049C34 File Offset: 0x00047E34
	// (remove) Token: 0x06000FF5 RID: 4085 RVA: 0x00049C6C File Offset: 0x00047E6C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<bool> CheckChanged;

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00049CA4 File Offset: 0x00047EA4
	// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x00049CAC File Offset: 0x00047EAC
	public bool ClickWhenSpacePressed
	{
		get
		{
			return this.clickWhenSpacePressed;
		}
		set
		{
			this.clickWhenSpacePressed = value;
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00049CB8 File Offset: 0x00047EB8
	// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x00049CC0 File Offset: 0x00047EC0
	public bool IsChecked
	{
		get
		{
			return this.isChecked;
		}
		set
		{
			if (value != this.isChecked)
			{
				this.isChecked = value;
				this.OnCheckChanged();
				if (value && this.group != null)
				{
					this.handleGroupedCheckboxChecked();
				}
			}
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00049CF8 File Offset: 0x00047EF8
	// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00049D00 File Offset: 0x00047F00
	public dfControl CheckIcon
	{
		get
		{
			return this.checkIcon;
		}
		set
		{
			if (value != this.checkIcon)
			{
				this.checkIcon = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00049D20 File Offset: 0x00047F20
	// (set) Token: 0x06000FFD RID: 4093 RVA: 0x00049D28 File Offset: 0x00047F28
	public dfLabel Label
	{
		get
		{
			return this.label;
		}
		set
		{
			if (value != this.label)
			{
				this.label = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00049D48 File Offset: 0x00047F48
	// (set) Token: 0x06000FFF RID: 4095 RVA: 0x00049D50 File Offset: 0x00047F50
	public dfControl GroupContainer
	{
		get
		{
			return this.group;
		}
		set
		{
			if (value != this.group)
			{
				this.group = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06001000 RID: 4096 RVA: 0x00049D70 File Offset: 0x00047F70
	// (set) Token: 0x06001001 RID: 4097 RVA: 0x00049D94 File Offset: 0x00047F94
	public string Text
	{
		get
		{
			if (this.label != null)
			{
				return this.label.Text;
			}
			return "[LABEL NOT SET]";
		}
		set
		{
			if (this.label != null)
			{
				this.label.Text = value;
			}
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001002 RID: 4098 RVA: 0x00049DB4 File Offset: 0x00047FB4
	public override bool CanFocus
	{
		get
		{
			return base.IsEnabled && base.IsVisible;
		}
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x00049DCC File Offset: 0x00047FCC
	public override void Start()
	{
		base.Start();
		if (this.checkIcon != null)
		{
			this.checkIcon.BringToFront();
			this.checkIcon.IsVisible = this.IsChecked;
		}
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x00049E04 File Offset: 0x00048004
	protected internal override void OnKeyPress(dfKeyEventArgs args)
	{
		if (this.ClickWhenSpacePressed && this.IsInteractive && args.KeyCode == KeyCode.Space)
		{
			this.OnClick(new dfMouseEventArgs(this, dfMouseButtons.Left, 1, default(Ray), Vector2.zero, 0f));
			return;
		}
		base.OnKeyPress(args);
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x00049E60 File Offset: 0x00048060
	protected internal override void OnClick(dfMouseEventArgs args)
	{
		base.OnClick(args);
		if (!this.IsInteractive)
		{
			return;
		}
		if (this.group == null)
		{
			this.IsChecked = !this.IsChecked;
		}
		else
		{
			this.handleGroupedCheckboxChecked();
		}
		args.Use();
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x00049EB4 File Offset: 0x000480B4
	protected internal void OnCheckChanged()
	{
		base.SignalHierarchy("OnCheckChanged", new object[] { this, this.isChecked });
		if (this.CheckChanged != null)
		{
			this.CheckChanged(this, this.isChecked);
		}
		if (this.checkIcon != null)
		{
			if (this.IsChecked)
			{
				this.checkIcon.BringToFront();
			}
			this.checkIcon.IsVisible = this.IsChecked;
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x00049F3C File Offset: 0x0004813C
	private void handleGroupedCheckboxChecked()
	{
		if (this.group == null)
		{
			return;
		}
		foreach (dfCheckbox dfCheckbox in this.group.transform.GetComponentsInChildren<dfCheckbox>())
		{
			if (dfCheckbox != this && dfCheckbox.GroupContainer == this.GroupContainer && dfCheckbox.IsChecked)
			{
				dfCheckbox.IsChecked = false;
			}
		}
		this.IsChecked = true;
	}

	// Token: 0x04000F0B RID: 3851
	[SerializeField]
	protected bool isChecked;

	// Token: 0x04000F0C RID: 3852
	[SerializeField]
	protected dfControl checkIcon;

	// Token: 0x04000F0D RID: 3853
	[SerializeField]
	protected dfLabel label;

	// Token: 0x04000F0E RID: 3854
	[SerializeField]
	protected dfControl group;

	// Token: 0x04000F0F RID: 3855
	[SerializeField]
	protected bool clickWhenSpacePressed = true;
}
