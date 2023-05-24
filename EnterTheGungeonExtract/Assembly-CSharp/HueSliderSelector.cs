using System;
using UnityEngine;

// Token: 0x02000443 RID: 1091
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Hue Slider")]
public class HueSliderSelector : MonoBehaviour
{
	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06001908 RID: 6408 RVA: 0x00075EEC File Offset: 0x000740EC
	// (set) Token: 0x06001909 RID: 6409 RVA: 0x00075EF4 File Offset: 0x000740F4
	public Color Hue
	{
		get
		{
			return this.hue;
		}
		set
		{
			if (!object.Equals(value, this.hue))
			{
				this.hue = value;
				if (this.slider != null)
				{
					this.slider.Value = HSBColor.FromColor(value).h;
				}
			}
		}
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x00075F50 File Offset: 0x00074150
	public void Start()
	{
		this.slider = base.GetComponent<dfSlider>();
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x00075F60 File Offset: 0x00074160
	public void OnValueChanged(dfControl control, float value)
	{
		HSBColor hsbcolor = new HSBColor(value, 1f, 1f, 1f);
		this.hue = hsbcolor.ToColor();
	}

	// Token: 0x040013BE RID: 5054
	private dfSlider slider;

	// Token: 0x040013BF RID: 5055
	private Color hue;
}
