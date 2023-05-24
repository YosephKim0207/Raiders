using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
[AddComponentMenu("Daikon Forge/Examples/Containers/Auto-Arrange Options")]
[ExecuteInEditMode]
public class AutoArrangeOptions : MonoBehaviour
{
	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06001920 RID: 6432 RVA: 0x00076238 File Offset: 0x00074438
	// (set) Token: 0x06001921 RID: 6433 RVA: 0x00076248 File Offset: 0x00074448
	public int FlowDirection
	{
		get
		{
			return (int)this.Panel.FlowDirection;
		}
		set
		{
			this.Panel.FlowDirection = (dfScrollPanel.LayoutDirection)value;
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06001922 RID: 6434 RVA: 0x00076258 File Offset: 0x00074458
	// (set) Token: 0x06001923 RID: 6435 RVA: 0x0007626C File Offset: 0x0007446C
	public int PaddingLeft
	{
		get
		{
			return this.Panel.FlowPadding.left;
		}
		set
		{
			this.Panel.FlowPadding.left = value;
			this.Panel.Reset();
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06001924 RID: 6436 RVA: 0x0007628C File Offset: 0x0007448C
	// (set) Token: 0x06001925 RID: 6437 RVA: 0x000762A0 File Offset: 0x000744A0
	public int PaddingRight
	{
		get
		{
			return this.Panel.FlowPadding.right;
		}
		set
		{
			this.Panel.FlowPadding.right = value;
			this.Panel.Reset();
		}
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06001926 RID: 6438 RVA: 0x000762C0 File Offset: 0x000744C0
	// (set) Token: 0x06001927 RID: 6439 RVA: 0x000762D4 File Offset: 0x000744D4
	public int PaddingTop
	{
		get
		{
			return this.Panel.FlowPadding.top;
		}
		set
		{
			this.Panel.FlowPadding.top = value;
			this.Panel.Reset();
		}
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x06001928 RID: 6440 RVA: 0x000762F4 File Offset: 0x000744F4
	// (set) Token: 0x06001929 RID: 6441 RVA: 0x00076308 File Offset: 0x00074508
	public int PaddingBottom
	{
		get
		{
			return this.Panel.FlowPadding.bottom;
		}
		set
		{
			this.Panel.FlowPadding.bottom = value;
			this.Panel.Reset();
		}
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x00076328 File Offset: 0x00074528
	private void Start()
	{
		if (this.Panel == null)
		{
			this.Panel = base.GetComponent<dfScrollPanel>();
		}
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x00076348 File Offset: 0x00074548
	public void ExpandAll()
	{
		for (int i = 0; i < this.Panel.Controls.Count; i++)
		{
			AutoArrangeDemoItem component = this.Panel.Controls[i].GetComponent<AutoArrangeDemoItem>();
			component.Expand();
		}
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00076394 File Offset: 0x00074594
	public void CollapseAll()
	{
		for (int i = 0; i < this.Panel.Controls.Count; i++)
		{
			AutoArrangeDemoItem component = this.Panel.Controls[i].GetComponent<AutoArrangeDemoItem>();
			component.Collapse();
		}
	}

	// Token: 0x040013CB RID: 5067
	public dfScrollPanel Panel;
}
