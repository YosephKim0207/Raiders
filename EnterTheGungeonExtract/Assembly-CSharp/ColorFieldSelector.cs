using System;
using UnityEngine;

// Token: 0x0200043E RID: 1086
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Color Field Selector")]
public class ColorFieldSelector : MonoBehaviour
{
	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x060018DF RID: 6367 RVA: 0x000753AC File Offset: 0x000735AC
	// (set) Token: 0x060018E0 RID: 6368 RVA: 0x000753B4 File Offset: 0x000735B4
	public Color SelectedColor
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
			this.updateHotspot();
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x060018E1 RID: 6369 RVA: 0x000753C4 File Offset: 0x000735C4
	// (set) Token: 0x060018E2 RID: 6370 RVA: 0x000753CC File Offset: 0x000735CC
	public Color Hue
	{
		get
		{
			return this.hue;
		}
		set
		{
			this.hue = value;
			this.updateSelectedColor();
		}
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x000753DC File Offset: 0x000735DC
	public void Start()
	{
		this.control = base.GetComponent<dfTextureSprite>();
		this.hue = HSBColor.GetHue(this.control.Color);
		this.color = this.control.Color;
		this.updateHotspot();
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x0007542C File Offset: 0x0007362C
	public void Update()
	{
		Material renderMaterial = this.control.RenderMaterial;
		if (renderMaterial != null)
		{
			this.control.RenderMaterial.color = this.hue;
		}
		if (this.selectedColorDisplay != null)
		{
			this.selectedColorDisplay.Color = this.color;
		}
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x00075490 File Offset: 0x00073690
	public void OnMouseDown(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.updateHotspot(mouseEvent);
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x0007549C File Offset: 0x0007369C
	public void OnMouseMove(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (mouseEvent.Buttons == dfMouseButtons.Left)
		{
			this.updateHotspot(mouseEvent);
		}
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x000754B4 File Offset: 0x000736B4
	private void updateHotspot()
	{
		if (this.control == null)
		{
			return;
		}
		HSBColor hsbcolor = HSBColor.FromColor(this.SelectedColor);
		Vector2 vector = new Vector2(hsbcolor.s * this.control.Width, (1f - hsbcolor.b) * this.control.Height);
		this.indicator.RelativePosition = vector - this.indicator.Size * 0.5f;
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x00075540 File Offset: 0x00073740
	private void updateHotspot(dfMouseEventArgs mouseEvent)
	{
		if (this.indicator == null)
		{
			return;
		}
		Vector2 hitPosition = this.control.GetHitPosition(mouseEvent);
		this.indicator.RelativePosition = hitPosition - this.indicator.Size * 0.5f;
		this.updateSelectedColor();
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x000755A0 File Offset: 0x000737A0
	private void updateSelectedColor()
	{
		if (this.control == null)
		{
			this.control = base.GetComponent<dfTextureSprite>();
		}
		Vector3 vector = this.indicator.RelativePosition + this.indicator.Size * 0.5f;
		this.color = this.getColor(vector.x, vector.y, this.control.Width, this.control.Height, this.Hue);
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x0007562C File Offset: 0x0007382C
	private Color getColor(float x, float y, float width, float height, Color hue)
	{
		float num = x / width;
		float num2 = y / height;
		num = Mathf.Clamp01(num);
		num2 = Mathf.Clamp01(num2);
		return Vector4.Lerp(Color.white, hue, num) * (1f - num2);
	}

	// Token: 0x040013AE RID: 5038
	public dfControl indicator;

	// Token: 0x040013AF RID: 5039
	public dfControl sliders;

	// Token: 0x040013B0 RID: 5040
	public dfControl selectedColorDisplay;

	// Token: 0x040013B1 RID: 5041
	private dfTextureSprite control;

	// Token: 0x040013B2 RID: 5042
	private Color hue;

	// Token: 0x040013B3 RID: 5043
	private Color color;
}
