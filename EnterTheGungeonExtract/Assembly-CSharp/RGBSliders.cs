using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
[AddComponentMenu("Daikon Forge/Examples/Color Picker/RGB Sliders Container")]
public class RGBSliders : MonoBehaviour
{
	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x0600190D RID: 6413 RVA: 0x00075F9C File Offset: 0x0007419C
	// (set) Token: 0x0600190E RID: 6414 RVA: 0x00075FA4 File Offset: 0x000741A4
	public Color SelectedColor
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
			this.updateSliders();
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x0600190F RID: 6415 RVA: 0x00075FB4 File Offset: 0x000741B4
	// (set) Token: 0x06001910 RID: 6416 RVA: 0x00075FBC File Offset: 0x000741BC
	public Color Hue
	{
		get
		{
			return this.hue;
		}
		set
		{
			this.hue = value;
		}
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x00075FC8 File Offset: 0x000741C8
	public void Start()
	{
		this.container = base.GetComponent<dfPanel>();
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x00075FD8 File Offset: 0x000741D8
	public void Update()
	{
		if (!this.container.ContainsFocus)
		{
			this.SelectedColor = this.colorField.SelectedColor;
		}
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x00075FFC File Offset: 0x000741FC
	public void OnValueChanged(dfControl source, float value)
	{
		if (!this.container.ContainsFocus)
		{
			return;
		}
		this.color = new Color(this.redSlider.Value, this.greenSlider.Value, this.blueSlider.Value);
		this.colorField.Hue = (this.hue = HSBColor.GetHue(this.color));
		this.colorField.SelectedColor = this.color;
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x00076078 File Offset: 0x00074278
	private void updateSliders()
	{
		this.redSlider.Value = this.color.r;
		this.greenSlider.Value = this.color.g;
		this.blueSlider.Value = this.color.b;
	}

	// Token: 0x040013C0 RID: 5056
	public ColorFieldSelector colorField;

	// Token: 0x040013C1 RID: 5057
	public dfSlider redSlider;

	// Token: 0x040013C2 RID: 5058
	public dfSlider greenSlider;

	// Token: 0x040013C3 RID: 5059
	public dfSlider blueSlider;

	// Token: 0x040013C4 RID: 5060
	private dfPanel container;

	// Token: 0x040013C5 RID: 5061
	private Color color;

	// Token: 0x040013C6 RID: 5062
	private Color hue;
}
