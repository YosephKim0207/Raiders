using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Drag and Drop")]
public class ColorPickerDrag : MonoBehaviour
{
	// Token: 0x060018F1 RID: 6385 RVA: 0x0007577C File Offset: 0x0007397C
	public void Start()
	{
		this.control = base.GetComponent<dfSlicedSprite>();
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x0007578C File Offset: 0x0007398C
	private void OnGUI()
	{
		if (!Application.isPlaying || !this.isDragging)
		{
			return;
		}
		if (this.dragTexture == null)
		{
			this.dragTexture = new Texture2D(2, 2);
			this.dragTexture.SetPixel(0, 0, Color.white);
			this.dragTexture.SetPixel(0, 1, Color.white);
			this.dragTexture.SetPixel(1, 0, Color.white);
			this.dragTexture.SetPixel(1, 1, Color.white);
			this.dragTexture.Apply();
		}
		Vector3 mousePosition = Input.mousePosition;
		Rect rect = new Rect(mousePosition.x - 15f, (float)Screen.height - mousePosition.y - 5f, 30f, 15f);
		Color color = GUI.color;
		GUI.color = this.control.Color;
		GUI.DrawTexture(rect, this.dragTexture);
		GUI.color = color;
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x00075888 File Offset: 0x00073A88
	public void OnDragStart(dfControl control, dfDragEventArgs dragEvent)
	{
		this.isDragging = true;
		dragEvent.Data = control.Color;
		dragEvent.State = dfDragDropState.Dragging;
		dragEvent.Use();
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x000758B0 File Offset: 0x00073AB0
	public void OnDragEnd(dfControl source, dfDragEventArgs args)
	{
		this.isDragging = false;
	}

	// Token: 0x040013B6 RID: 5046
	private Texture2D dragTexture;

	// Token: 0x040013B7 RID: 5047
	private dfSlicedSprite control;

	// Token: 0x040013B8 RID: 5048
	private bool isDragging;
}
