using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C07 RID: 3079
[AddComponentMenu("2D Toolkit/UI/tk2dUIHoverItem")]
public class tk2dUIHoverItem : tk2dUIBaseItemControl
{
	// Token: 0x14000089 RID: 137
	// (add) Token: 0x06004193 RID: 16787 RVA: 0x001535FC File Offset: 0x001517FC
	// (remove) Token: 0x06004194 RID: 16788 RVA: 0x00153634 File Offset: 0x00151834
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIHoverItem> OnToggleHover;

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06004195 RID: 16789 RVA: 0x0015366C File Offset: 0x0015186C
	// (set) Token: 0x06004196 RID: 16790 RVA: 0x00153674 File Offset: 0x00151874
	public bool IsOver
	{
		get
		{
			return this.isOver;
		}
		set
		{
			if (this.isOver != value)
			{
				this.isOver = value;
				this.SetState();
				if (this.OnToggleHover != null)
				{
					this.OnToggleHover(this);
				}
				base.DoSendMessage(this.SendMessageOnToggleHoverMethodName, this);
			}
		}
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x001536B4 File Offset: 0x001518B4
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x001536BC File Offset: 0x001518BC
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnHoverOver += this.HoverOver;
			this.uiItem.OnHoverOut += this.HoverOut;
		}
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x001536FC File Offset: 0x001518FC
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnHoverOver -= this.HoverOver;
			this.uiItem.OnHoverOut -= this.HoverOut;
		}
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x0015373C File Offset: 0x0015193C
	private void HoverOver()
	{
		this.IsOver = true;
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x00153748 File Offset: 0x00151948
	private void HoverOut()
	{
		this.IsOver = false;
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x00153754 File Offset: 0x00151954
	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.overStateGO, this.isOver);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.outStateGO, !this.isOver);
	}

	// Token: 0x04003431 RID: 13361
	public GameObject outStateGO;

	// Token: 0x04003432 RID: 13362
	public GameObject overStateGO;

	// Token: 0x04003433 RID: 13363
	private bool isOver;

	// Token: 0x04003435 RID: 13365
	public string SendMessageOnToggleHoverMethodName = string.Empty;
}
