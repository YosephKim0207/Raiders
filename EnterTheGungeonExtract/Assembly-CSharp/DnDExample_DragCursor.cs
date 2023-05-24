using System;
using UnityEngine;

// Token: 0x0200044B RID: 1099
[AddComponentMenu("Daikon Forge/Examples/Drag and Drop/Drag Cursor")]
public class DnDExample_DragCursor : MonoBehaviour
{
	// Token: 0x06001951 RID: 6481 RVA: 0x00076FB8 File Offset: 0x000751B8
	public void Start()
	{
		DnDExample_DragCursor._sprite = base.GetComponent<dfSprite>();
		DnDExample_DragCursor._sprite.IsVisible = false;
		DnDExample_DragCursor._label = DnDExample_DragCursor._sprite.Find<dfLabel>("Count");
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x00076FE4 File Offset: 0x000751E4
	public void Update()
	{
		if (DnDExample_DragCursor._sprite.IsVisible)
		{
			DnDExample_DragCursor.SetPosition(Input.mousePosition);
		}
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x00077004 File Offset: 0x00075204
	public static void Show(DndExample_InventoryItem item, Vector2 Position)
	{
		DnDExample_DragCursor.SetPosition(Position);
		DnDExample_DragCursor._sprite.SpriteName = item.Icon;
		DnDExample_DragCursor._sprite.IsVisible = true;
		DnDExample_DragCursor._sprite.BringToFront();
		DnDExample_DragCursor._label.Text = ((item.Count <= 1) ? string.Empty : item.Count.ToString());
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x00077070 File Offset: 0x00075270
	public static void Hide()
	{
		DnDExample_DragCursor._sprite.IsVisible = false;
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x00077080 File Offset: 0x00075280
	public static void SetPosition(Vector2 position)
	{
		position = DnDExample_DragCursor._sprite.GetManager().ScreenToGui(position);
		DnDExample_DragCursor._sprite.RelativePosition = position - DnDExample_DragCursor._sprite.Size * 0.5f;
	}

	// Token: 0x040013E1 RID: 5089
	private static dfSprite _sprite;

	// Token: 0x040013E2 RID: 5090
	private static dfLabel _label;
}
