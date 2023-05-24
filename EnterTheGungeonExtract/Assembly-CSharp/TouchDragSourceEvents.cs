using System;
using UnityEngine;

// Token: 0x02000490 RID: 1168
[AddComponentMenu("Daikon Forge/Examples/Touch/Drag Source Events")]
public class TouchDragSourceEvents : MonoBehaviour
{
	// Token: 0x06001AD6 RID: 6870 RVA: 0x0007D904 File Offset: 0x0007BB04
	public void Start()
	{
		this._label = base.GetComponent<dfLabel>();
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x0007D914 File Offset: 0x0007BB14
	public void OnGUI()
	{
		if (!this.isDragging)
		{
			return;
		}
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = (float)Screen.height - mousePosition.y;
		Rect rect = new Rect(mousePosition.x - 100f, mousePosition.y - 50f, 200f, 100f);
		GUI.Box(rect, this._label.name);
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x0007D984 File Offset: 0x0007BB84
	public void OnDragEnd(dfControl control, dfDragEventArgs dragEvent)
	{
		if (dragEvent.State == dfDragDropState.Dropped)
		{
			this._label.Text = "Dropped on " + dragEvent.Target.name;
		}
		else
		{
			this._label.Text = "Drag Ended: " + dragEvent.State;
		}
		this.isDragging = false;
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x0007D9EC File Offset: 0x0007BBEC
	public void OnDragStart(dfControl control, dfDragEventArgs dragEvent)
	{
		this._label.Text = "Dragging...";
		dragEvent.Data = base.name;
		dragEvent.State = dfDragDropState.Dragging;
		dragEvent.Use();
		this.isDragging = true;
	}

	// Token: 0x0400152D RID: 5421
	private dfLabel _label;

	// Token: 0x0400152E RID: 5422
	private bool isDragging;
}
