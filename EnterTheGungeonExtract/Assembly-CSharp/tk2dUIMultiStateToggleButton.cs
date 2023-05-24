using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C08 RID: 3080
[AddComponentMenu("2D Toolkit/UI/tk2dUIMultiStateToggleButton")]
public class tk2dUIMultiStateToggleButton : tk2dUIBaseItemControl
{
	// Token: 0x1400008A RID: 138
	// (add) Token: 0x0600419E RID: 16798 RVA: 0x00153790 File Offset: 0x00151990
	// (remove) Token: 0x0600419F RID: 16799 RVA: 0x001537C8 File Offset: 0x001519C8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIMultiStateToggleButton> OnStateToggle;

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x060041A0 RID: 16800 RVA: 0x00153800 File Offset: 0x00151A00
	// (set) Token: 0x060041A1 RID: 16801 RVA: 0x00153808 File Offset: 0x00151A08
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			if (value >= this.states.Length)
			{
				value = this.states.Length;
			}
			if (value < 0)
			{
				value = 0;
			}
			if (this.index != value)
			{
				this.index = value;
				this.SetState();
				if (this.OnStateToggle != null)
				{
					this.OnStateToggle(this);
				}
				base.DoSendMessage(this.SendMessageOnStateToggleMethodName, this);
			}
		}
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x00153874 File Offset: 0x00151A74
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x0015387C File Offset: 0x00151A7C
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ButtonClick;
			this.uiItem.OnDown += this.ButtonDown;
		}
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x001538BC File Offset: 0x00151ABC
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ButtonClick;
			this.uiItem.OnDown -= this.ButtonDown;
		}
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x001538FC File Offset: 0x00151AFC
	private void ButtonClick()
	{
		if (!this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x00153910 File Offset: 0x00151B10
	private void ButtonDown()
	{
		if (this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x00153924 File Offset: 0x00151B24
	private void ButtonToggle()
	{
		if (this.Index + 1 >= this.states.Length)
		{
			this.Index = 0;
		}
		else
		{
			this.Index++;
		}
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x00153958 File Offset: 0x00151B58
	private void SetState()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			GameObject gameObject = this.states[i];
			if (gameObject != null)
			{
				if (i != this.index)
				{
					if (this.states[i].activeInHierarchy)
					{
						this.states[i].SetActive(false);
					}
				}
				else if (!this.states[i].activeInHierarchy)
				{
					this.states[i].SetActive(true);
				}
			}
		}
	}

	// Token: 0x04003436 RID: 13366
	public GameObject[] states;

	// Token: 0x04003437 RID: 13367
	public bool activateOnPress;

	// Token: 0x04003439 RID: 13369
	private int index;

	// Token: 0x0400343A RID: 13370
	public string SendMessageOnStateToggleMethodName = string.Empty;
}
