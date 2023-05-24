using System;
using UnityEngine;

// Token: 0x0200044C RID: 1100
[AddComponentMenu("Daikon Forge/Examples/Drag and Drop/Inventory Item")]
public class DndExample_InventoryItem : MonoBehaviour
{
	// Token: 0x06001957 RID: 6487 RVA: 0x000770C8 File Offset: 0x000752C8
	public void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000770D0 File Offset: 0x000752D0
	public void OnDoubleClick(dfControl source, dfMouseEventArgs args)
	{
		this.OnClick(source, args);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000770DC File Offset: 0x000752DC
	public void OnClick(dfControl source, dfMouseEventArgs args)
	{
		if (string.IsNullOrEmpty(this.ItemName))
		{
			return;
		}
		if (args.Buttons == dfMouseButtons.Left)
		{
			this.Count++;
		}
		else if (args.Buttons == dfMouseButtons.Right)
		{
			this.Count = Mathf.Max(this.Count - 1, 1);
		}
		this.Refresh();
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x00077140 File Offset: 0x00075340
	public void OnDragStart(dfControl source, dfDragEventArgs args)
	{
		if (this.Count > 0)
		{
			args.Data = this;
			args.State = dfDragDropState.Dragging;
			args.Use();
			DnDExample_DragCursor.Show(this, args.Position);
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x00077170 File Offset: 0x00075370
	public void OnDragEnd(dfControl source, dfDragEventArgs args)
	{
		DnDExample_DragCursor.Hide();
		if (args.State == dfDragDropState.Dropped)
		{
			this.Count = 0;
			this.ItemName = string.Empty;
			this.Icon = string.Empty;
			this.Refresh();
		}
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x000771A8 File Offset: 0x000753A8
	public void OnDragDrop(dfControl source, dfDragEventArgs args)
	{
		if (this.Count == 0 && args.Data is DndExample_InventoryItem)
		{
			DndExample_InventoryItem dndExample_InventoryItem = (DndExample_InventoryItem)args.Data;
			this.ItemName = dndExample_InventoryItem.ItemName;
			this.Icon = dndExample_InventoryItem.Icon;
			this.Count = dndExample_InventoryItem.Count;
			args.State = dfDragDropState.Dropped;
			args.Use();
		}
		this.Refresh();
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x00077214 File Offset: 0x00075414
	private void Refresh()
	{
		DndExample_InventoryItem._container = base.GetComponent<dfPanel>();
		DndExample_InventoryItem._sprite = DndExample_InventoryItem._container.Find("Icon") as dfSprite;
		DndExample_InventoryItem._label = DndExample_InventoryItem._container.Find("Count") as dfLabel;
		DndExample_InventoryItem._sprite.SpriteName = this.Icon;
		DndExample_InventoryItem._label.Text = ((this.Count <= 1) ? string.Empty : this.Count.ToString());
	}

	// Token: 0x040013E3 RID: 5091
	public string ItemName;

	// Token: 0x040013E4 RID: 5092
	public int Count;

	// Token: 0x040013E5 RID: 5093
	public string Icon;

	// Token: 0x040013E6 RID: 5094
	private static dfPanel _container;

	// Token: 0x040013E7 RID: 5095
	private static dfSprite _sprite;

	// Token: 0x040013E8 RID: 5096
	private static dfLabel _label;
}
