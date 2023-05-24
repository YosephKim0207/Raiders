using System;
using System.Collections.Generic;

// Token: 0x0200049E RID: 1182
public abstract class dfMarkupElement
{
	// Token: 0x06001B71 RID: 7025 RVA: 0x00081BC8 File Offset: 0x0007FDC8
	public dfMarkupElement()
	{
		this.ChildNodes = new List<dfMarkupElement>();
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06001B72 RID: 7026 RVA: 0x00081BDC File Offset: 0x0007FDDC
	// (set) Token: 0x06001B73 RID: 7027 RVA: 0x00081BE4 File Offset: 0x0007FDE4
	public dfMarkupElement Parent { get; protected set; }

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06001B74 RID: 7028 RVA: 0x00081BF0 File Offset: 0x0007FDF0
	// (set) Token: 0x06001B75 RID: 7029 RVA: 0x00081BF8 File Offset: 0x0007FDF8
	private protected List<dfMarkupElement> ChildNodes { protected get; private set; }

	// Token: 0x06001B76 RID: 7030 RVA: 0x00081C04 File Offset: 0x0007FE04
	public void AddChildNode(dfMarkupElement node)
	{
		node.Parent = this;
		this.ChildNodes.Add(node);
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x00081C1C File Offset: 0x0007FE1C
	public void PerformLayout(dfMarkupBox container, dfMarkupStyle style)
	{
		this._PerformLayoutImpl(container, style);
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x00081C28 File Offset: 0x0007FE28
	internal virtual void Release()
	{
		this.Parent = null;
		this.ChildNodes.Clear();
	}

	// Token: 0x06001B79 RID: 7033
	protected abstract void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style);
}
