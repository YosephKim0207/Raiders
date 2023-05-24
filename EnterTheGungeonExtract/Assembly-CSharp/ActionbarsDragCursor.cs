using System;
using UnityEngine;

// Token: 0x02000429 RID: 1065
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Drag Cursor")]
public class ActionbarsDragCursor : MonoBehaviour
{
	// Token: 0x0600186D RID: 6253 RVA: 0x00073A9C File Offset: 0x00071C9C
	public void Start()
	{
		ActionbarsDragCursor._sprite = base.GetComponent<dfSprite>();
		ActionbarsDragCursor._sprite.Hide();
		ActionbarsDragCursor._sprite.IsInteractive = false;
		ActionbarsDragCursor._sprite.IsEnabled = false;
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x00073ACC File Offset: 0x00071CCC
	public void Update()
	{
		if (ActionbarsDragCursor._sprite.IsVisible)
		{
			ActionbarsDragCursor.setPosition(Input.mousePosition);
		}
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x00073AEC File Offset: 0x00071CEC
	public static void Show(dfSprite sprite, Vector2 position, Vector2 offset)
	{
		ActionbarsDragCursor._cursorOffset = offset;
		ActionbarsDragCursor.setPosition(position);
		ActionbarsDragCursor._sprite.Size = sprite.Size;
		ActionbarsDragCursor._sprite.Atlas = sprite.Atlas;
		ActionbarsDragCursor._sprite.SpriteName = sprite.SpriteName;
		ActionbarsDragCursor._sprite.IsVisible = true;
		ActionbarsDragCursor._sprite.BringToFront();
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x00073B4C File Offset: 0x00071D4C
	public static void Hide()
	{
		ActionbarsDragCursor._sprite.IsVisible = false;
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x00073B5C File Offset: 0x00071D5C
	private static void setPosition(Vector2 position)
	{
		position = ActionbarsDragCursor._sprite.GetManager().ScreenToGui(position);
		ActionbarsDragCursor._sprite.RelativePosition = position - ActionbarsDragCursor._cursorOffset;
	}

	// Token: 0x04001364 RID: 4964
	private static dfSprite _sprite;

	// Token: 0x04001365 RID: 4965
	private static Vector2 _cursorOffset;
}
