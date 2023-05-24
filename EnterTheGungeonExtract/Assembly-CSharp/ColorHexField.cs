using System;
using System.Globalization;
using UnityEngine;

// Token: 0x0200043F RID: 1087
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Color Hex Field")]
public class ColorHexField : MonoBehaviour
{
	// Token: 0x060018EC RID: 6380 RVA: 0x00075680 File Offset: 0x00073880
	public void Start()
	{
		this.control = base.GetComponent<dfTextbox>();
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x00075690 File Offset: 0x00073890
	public void Update()
	{
		if (!this.control.HasFocus)
		{
			Color32 color = this.colorField.SelectedColor;
			this.control.Text = string.Format("{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
		}
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x000756F8 File Offset: 0x000738F8
	public void OnTextSubmitted(dfControl control, string value)
	{
		uint num = 0U;
		if (uint.TryParse(value, NumberStyles.HexNumber, null, out num))
		{
			Color color = this.UIntToColor(num);
			this.colorField.Hue = HSBColor.GetHue(color);
			this.colorField.SelectedColor = color;
		}
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x00075740 File Offset: 0x00073940
	private Color UIntToColor(uint color)
	{
		byte b = (byte)(color >> 24);
		byte b2 = (byte)(color >> 16);
		byte b3 = (byte)(color >> 8);
		byte b4 = (byte)(color >> 0);
		return new Color32(b2, b3, b4, b);
	}

	// Token: 0x040013B4 RID: 5044
	public ColorFieldSelector colorField;

	// Token: 0x040013B5 RID: 5045
	private dfTextbox control;
}
