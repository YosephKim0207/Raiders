using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Color Preset")]
public class ColorPickerPreset : MonoBehaviour
{
	// Token: 0x060018F6 RID: 6390 RVA: 0x000758C4 File Offset: 0x00073AC4
	public void OnDragDrop(dfControl control, dfDragEventArgs dragEvent)
	{
		if (dragEvent.Data is Color32)
		{
			control.Color = (Color32)dragEvent.Data;
			dragEvent.State = dfDragDropState.Dropped;
			dragEvent.Use();
		}
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x000758F4 File Offset: 0x00073AF4
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (this.colorField != null)
		{
			this.colorField.Hue = HSBColor.GetHue(control.Color);
			this.colorField.SelectedColor = control.Color;
		}
	}

	// Token: 0x040013B9 RID: 5049
	public ColorFieldSelector colorField;
}
