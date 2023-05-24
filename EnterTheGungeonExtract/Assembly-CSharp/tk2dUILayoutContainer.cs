using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C1C RID: 3100
public abstract class tk2dUILayoutContainer : tk2dUILayout
{
	// Token: 0x060042A6 RID: 17062 RVA: 0x00158AB0 File Offset: 0x00156CB0
	public Vector2 GetInnerSize()
	{
		return this.innerSize;
	}

	// Token: 0x060042A7 RID: 17063
	protected abstract void DoChildLayout();

	// Token: 0x1400009E RID: 158
	// (add) Token: 0x060042A8 RID: 17064 RVA: 0x00158AB8 File Offset: 0x00156CB8
	// (remove) Token: 0x060042A9 RID: 17065 RVA: 0x00158AF0 File Offset: 0x00156CF0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnChangeContent;

	// Token: 0x060042AA RID: 17066 RVA: 0x00158B28 File Offset: 0x00156D28
	public override void Reshape(Vector3 dMin, Vector3 dMax, bool updateChildren)
	{
		this.bMin += dMin;
		this.bMax += dMax;
		Vector3 vector = new Vector3(this.bMin.x, this.bMax.y);
		base.transform.position += vector;
		this.bMin -= vector;
		this.bMax -= vector;
		this.DoChildLayout();
		if (this.OnChangeContent != null)
		{
			this.OnChangeContent();
		}
	}

	// Token: 0x060042AB RID: 17067 RVA: 0x00158BD0 File Offset: 0x00156DD0
	public void AddLayout(tk2dUILayout layout, tk2dUILayoutItem item)
	{
		item.gameObj = layout.gameObject;
		item.layout = layout;
		this.layoutItems.Add(item);
		layout.gameObject.transform.parent = base.transform;
		base.Refresh();
	}

	// Token: 0x060042AC RID: 17068 RVA: 0x00158C10 File Offset: 0x00156E10
	public void AddLayoutAtIndex(tk2dUILayout layout, tk2dUILayoutItem item, int index)
	{
		item.gameObj = layout.gameObject;
		item.layout = layout;
		this.layoutItems.Insert(index, item);
		layout.gameObject.transform.parent = base.transform;
		base.Refresh();
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x00158C50 File Offset: 0x00156E50
	public void RemoveLayout(tk2dUILayout layout)
	{
		foreach (tk2dUILayoutItem tk2dUILayoutItem in this.layoutItems)
		{
			if (tk2dUILayoutItem.layout == layout)
			{
				this.layoutItems.Remove(tk2dUILayoutItem);
				layout.gameObject.transform.parent = null;
				break;
			}
		}
		base.Refresh();
	}

	// Token: 0x040034EF RID: 13551
	protected Vector2 innerSize = Vector2.zero;
}
