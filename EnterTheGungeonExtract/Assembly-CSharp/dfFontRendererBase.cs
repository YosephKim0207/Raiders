using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003CA RID: 970
public abstract class dfFontRendererBase : IDisposable
{
	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06001278 RID: 4728 RVA: 0x000552DC File Offset: 0x000534DC
	// (set) Token: 0x06001279 RID: 4729 RVA: 0x000552E4 File Offset: 0x000534E4
	public dfFontBase Font { get; protected set; }

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x0600127A RID: 4730 RVA: 0x000552F0 File Offset: 0x000534F0
	// (set) Token: 0x0600127B RID: 4731 RVA: 0x000552F8 File Offset: 0x000534F8
	public Vector2 MaxSize { get; set; }

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x0600127C RID: 4732 RVA: 0x00055304 File Offset: 0x00053504
	// (set) Token: 0x0600127D RID: 4733 RVA: 0x0005530C File Offset: 0x0005350C
	public float PixelRatio { get; set; }

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x0600127E RID: 4734 RVA: 0x00055318 File Offset: 0x00053518
	// (set) Token: 0x0600127F RID: 4735 RVA: 0x00055320 File Offset: 0x00053520
	public float TextScale { get; set; }

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06001280 RID: 4736 RVA: 0x0005532C File Offset: 0x0005352C
	// (set) Token: 0x06001281 RID: 4737 RVA: 0x00055334 File Offset: 0x00053534
	public int CharacterSpacing { get; set; }

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06001282 RID: 4738 RVA: 0x00055340 File Offset: 0x00053540
	// (set) Token: 0x06001283 RID: 4739 RVA: 0x00055348 File Offset: 0x00053548
	public Vector3 VectorOffset { get; set; }

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06001284 RID: 4740 RVA: 0x00055354 File Offset: 0x00053554
	// (set) Token: 0x06001285 RID: 4741 RVA: 0x0005535C File Offset: 0x0005355C
	public Vector3 PerCharacterAccumulatedOffset { get; set; }

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06001286 RID: 4742 RVA: 0x00055368 File Offset: 0x00053568
	// (set) Token: 0x06001287 RID: 4743 RVA: 0x00055370 File Offset: 0x00053570
	public bool ProcessMarkup { get; set; }

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06001288 RID: 4744 RVA: 0x0005537C File Offset: 0x0005357C
	// (set) Token: 0x06001289 RID: 4745 RVA: 0x00055384 File Offset: 0x00053584
	public bool WordWrap { get; set; }

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x0600128A RID: 4746 RVA: 0x00055390 File Offset: 0x00053590
	// (set) Token: 0x0600128B RID: 4747 RVA: 0x00055398 File Offset: 0x00053598
	public bool MultiLine { get; set; }

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x0600128C RID: 4748 RVA: 0x000553A4 File Offset: 0x000535A4
	// (set) Token: 0x0600128D RID: 4749 RVA: 0x000553AC File Offset: 0x000535AC
	public bool OverrideMarkupColors { get; set; }

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x0600128E RID: 4750 RVA: 0x000553B8 File Offset: 0x000535B8
	// (set) Token: 0x0600128F RID: 4751 RVA: 0x000553C0 File Offset: 0x000535C0
	public bool ColorizeSymbols { get; set; }

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06001290 RID: 4752 RVA: 0x000553CC File Offset: 0x000535CC
	// (set) Token: 0x06001291 RID: 4753 RVA: 0x000553D4 File Offset: 0x000535D4
	public TextAlignment TextAlign { get; set; }

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06001292 RID: 4754 RVA: 0x000553E0 File Offset: 0x000535E0
	// (set) Token: 0x06001293 RID: 4755 RVA: 0x000553E8 File Offset: 0x000535E8
	public Color32 DefaultColor { get; set; }

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06001294 RID: 4756 RVA: 0x000553F4 File Offset: 0x000535F4
	// (set) Token: 0x06001295 RID: 4757 RVA: 0x000553FC File Offset: 0x000535FC
	public Color32? BottomColor { get; set; }

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06001296 RID: 4758 RVA: 0x00055408 File Offset: 0x00053608
	// (set) Token: 0x06001297 RID: 4759 RVA: 0x00055410 File Offset: 0x00053610
	public float Opacity { get; set; }

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06001298 RID: 4760 RVA: 0x0005541C File Offset: 0x0005361C
	// (set) Token: 0x06001299 RID: 4761 RVA: 0x00055424 File Offset: 0x00053624
	public bool Outline { get; set; }

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x0600129A RID: 4762 RVA: 0x00055430 File Offset: 0x00053630
	// (set) Token: 0x0600129B RID: 4763 RVA: 0x00055438 File Offset: 0x00053638
	public int OutlineSize { get; set; }

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x0600129C RID: 4764 RVA: 0x00055444 File Offset: 0x00053644
	// (set) Token: 0x0600129D RID: 4765 RVA: 0x0005544C File Offset: 0x0005364C
	public Color32 OutlineColor { get; set; }

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x0600129E RID: 4766 RVA: 0x00055458 File Offset: 0x00053658
	// (set) Token: 0x0600129F RID: 4767 RVA: 0x00055460 File Offset: 0x00053660
	public bool Shadow { get; set; }

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0005546C File Offset: 0x0005366C
	// (set) Token: 0x060012A1 RID: 4769 RVA: 0x00055474 File Offset: 0x00053674
	public Color32 ShadowColor { get; set; }

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x060012A2 RID: 4770 RVA: 0x00055480 File Offset: 0x00053680
	// (set) Token: 0x060012A3 RID: 4771 RVA: 0x00055488 File Offset: 0x00053688
	public Vector2 ShadowOffset { get; set; }

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x060012A4 RID: 4772 RVA: 0x00055494 File Offset: 0x00053694
	// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0005549C File Offset: 0x0005369C
	public int TabSize { get; set; }

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x060012A6 RID: 4774 RVA: 0x000554A8 File Offset: 0x000536A8
	// (set) Token: 0x060012A7 RID: 4775 RVA: 0x000554B0 File Offset: 0x000536B0
	public List<int> TabStops { get; set; }

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x060012A8 RID: 4776 RVA: 0x000554BC File Offset: 0x000536BC
	// (set) Token: 0x060012A9 RID: 4777 RVA: 0x000554C4 File Offset: 0x000536C4
	public Vector2 RenderedSize { get; internal set; }

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x000554D0 File Offset: 0x000536D0
	// (set) Token: 0x060012AB RID: 4779 RVA: 0x000554D8 File Offset: 0x000536D8
	public int LinesRendered { get; internal set; }

	// Token: 0x060012AC RID: 4780
	public abstract void Release();

	// Token: 0x060012AD RID: 4781
	public abstract float[] GetCharacterWidths(string text);

	// Token: 0x060012AE RID: 4782
	public abstract Vector2 MeasureString(string text);

	// Token: 0x060012AF RID: 4783
	public abstract void Render(string text, dfRenderData destination);

	// Token: 0x060012B0 RID: 4784 RVA: 0x000554E4 File Offset: 0x000536E4
	protected virtual void Reset()
	{
		this.Font = null;
		this.PixelRatio = 0f;
		this.TextScale = 1f;
		this.CharacterSpacing = 0;
		this.VectorOffset = Vector3.zero;
		this.PerCharacterAccumulatedOffset = Vector3.zero;
		this.ProcessMarkup = false;
		this.WordWrap = false;
		this.MultiLine = false;
		this.OverrideMarkupColors = false;
		this.ColorizeSymbols = false;
		this.TextAlign = TextAlignment.Left;
		this.DefaultColor = Color.white;
		this.BottomColor = null;
		this.Opacity = 1f;
		this.Outline = false;
		this.Shadow = false;
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x00055590 File Offset: 0x00053790
	public void Dispose()
	{
		this.Release();
	}
}
