using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class dfMarkupBox
{
	// Token: 0x06001B31 RID: 6961 RVA: 0x0007FF08 File Offset: 0x0007E108
	private dfMarkupBox()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x0007FF60 File Offset: 0x0007E160
	public dfMarkupBox(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
	{
		this.Element = element;
		this.Display = display;
		this.Style = style;
		this.Baseline = style.FontSize;
	}

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06001B33 RID: 6963 RVA: 0x0007FFD4 File Offset: 0x0007E1D4
	// (set) Token: 0x06001B34 RID: 6964 RVA: 0x0007FFDC File Offset: 0x0007E1DC
	public dfMarkupBox Parent { get; protected set; }

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06001B35 RID: 6965 RVA: 0x0007FFE8 File Offset: 0x0007E1E8
	// (set) Token: 0x06001B36 RID: 6966 RVA: 0x0007FFF0 File Offset: 0x0007E1F0
	public dfMarkupElement Element { get; protected set; }

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06001B37 RID: 6967 RVA: 0x0007FFFC File Offset: 0x0007E1FC
	public List<dfMarkupBox> Children
	{
		get
		{
			return this.children;
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06001B38 RID: 6968 RVA: 0x00080004 File Offset: 0x0007E204
	// (set) Token: 0x06001B39 RID: 6969 RVA: 0x00080014 File Offset: 0x0007E214
	public int Width
	{
		get
		{
			return (int)this.Size.x;
		}
		set
		{
			this.Size = new Vector2((float)value, this.Size.y);
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06001B3A RID: 6970 RVA: 0x00080030 File Offset: 0x0007E230
	// (set) Token: 0x06001B3B RID: 6971 RVA: 0x00080040 File Offset: 0x0007E240
	public int Height
	{
		get
		{
			return (int)this.Size.y;
		}
		set
		{
			this.Size = new Vector2(this.Size.x, (float)value);
		}
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x0008005C File Offset: 0x0007E25C
	internal dfMarkupBox HitTest(Vector2 point)
	{
		Vector2 offset = this.GetOffset();
		Vector2 vector = offset + this.Size;
		if (point.x < offset.x || point.x > vector.x || point.y < offset.y || point.y > vector.y)
		{
			return null;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox dfMarkupBox = this.children[i].HitTest(point);
			if (dfMarkupBox != null)
			{
				return dfMarkupBox;
			}
		}
		return this;
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x00080104 File Offset: 0x0007E304
	internal dfRenderData Render()
	{
		dfRenderData dfRenderData;
		try
		{
			this.endCurrentLine();
			dfRenderData = this.OnRebuildRenderData();
		}
		finally
		{
		}
		return dfRenderData;
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x00080138 File Offset: 0x0007E338
	public virtual Vector2 GetOffset()
	{
		Vector2 vector = Vector2.zero;
		for (dfMarkupBox dfMarkupBox = this; dfMarkupBox != null; dfMarkupBox = dfMarkupBox.Parent)
		{
			vector += dfMarkupBox.Position;
		}
		return vector;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x00080170 File Offset: 0x0007E370
	internal void AddLineBreak()
	{
		if (this.currentLine != null)
		{
			this.currentLine.IsNewline = true;
		}
		int verticalPosition = this.getVerticalPosition(0);
		this.endCurrentLine();
		dfMarkupBox containingBlock = this.GetContainingBlock();
		this.currentLine = new dfMarkupBox(this.Element, dfMarkupDisplayType.block, this.Style)
		{
			Size = new Vector2(containingBlock.Size.x, (float)this.Style.FontSize),
			Position = new Vector2(0f, (float)verticalPosition),
			Parent = this
		};
		this.children.Add(this.currentLine);
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x00080210 File Offset: 0x0007E410
	public virtual void AddChild(dfMarkupBox box)
	{
		dfMarkupDisplayType display = box.Display;
		bool flag = display == dfMarkupDisplayType.block || display == dfMarkupDisplayType.table || display == dfMarkupDisplayType.listItem || display == dfMarkupDisplayType.tableRow;
		if (flag)
		{
			this.addBlock(box);
		}
		else
		{
			this.addInline(box);
		}
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x0008025C File Offset: 0x0007E45C
	public virtual void Release()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].Release();
		}
		this.children.Clear();
		this.Element = null;
		this.Parent = null;
		this.Margins = default(dfMarkupBorders);
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x000802C0 File Offset: 0x0007E4C0
	protected virtual dfRenderData OnRebuildRenderData()
	{
		return null;
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000802C4 File Offset: 0x0007E4C4
	protected void renderDebugBox(dfRenderData renderData)
	{
		Vector3 zero = Vector3.zero;
		Vector3 vector = zero + Vector3.right * this.Size.x;
		Vector3 vector2 = vector + Vector3.down * this.Size.y;
		Vector3 vector3 = zero + Vector3.down * this.Size.y;
		renderData.Vertices.Add(zero);
		renderData.Vertices.Add(vector);
		renderData.Vertices.Add(vector2);
		renderData.Vertices.Add(vector3);
		renderData.Triangles.AddRange(new int[] { 0, 1, 3, 3, 1, 2 });
		renderData.UV.Add(Vector2.zero);
		renderData.UV.Add(Vector2.zero);
		renderData.UV.Add(Vector2.zero);
		renderData.UV.Add(Vector2.zero);
		Color backgroundColor = this.Style.BackgroundColor;
		renderData.Colors.Add(backgroundColor);
		renderData.Colors.Add(backgroundColor);
		renderData.Colors.Add(backgroundColor);
		renderData.Colors.Add(backgroundColor);
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x0008040C File Offset: 0x0007E60C
	public void FitToContents()
	{
		this.FitToContents(false);
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x00080418 File Offset: 0x0007E618
	public void FitToContents(bool recursive)
	{
		if (this.children.Count == 0)
		{
			this.Size = new Vector2(this.Size.x, 0f);
			return;
		}
		this.endCurrentLine();
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox dfMarkupBox = this.children[i];
			vector = Vector2.Max(vector, dfMarkupBox.Position + dfMarkupBox.Size);
		}
		this.Size = vector;
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000804A8 File Offset: 0x0007E6A8
	private dfMarkupBox GetContainingBlock()
	{
		for (dfMarkupBox dfMarkupBox = this; dfMarkupBox != null; dfMarkupBox = dfMarkupBox.Parent)
		{
			dfMarkupDisplayType display = dfMarkupBox.Display;
			bool flag = display == dfMarkupDisplayType.block || display == dfMarkupDisplayType.inlineBlock || display == dfMarkupDisplayType.listItem || display == dfMarkupDisplayType.table || display == dfMarkupDisplayType.tableRow || display == dfMarkupDisplayType.tableCell;
			if (flag)
			{
				return dfMarkupBox;
			}
		}
		return null;
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x00080508 File Offset: 0x0007E708
	private void addInline(dfMarkupBox box)
	{
		dfMarkupBorders margins = box.Margins;
		bool flag = !this.Style.Preformatted && this.currentLine != null && (float)this.currentLinePos + box.Size.x > this.currentLine.Size.x;
		if (this.currentLine == null || flag)
		{
			this.endCurrentLine();
			int verticalPosition = this.getVerticalPosition(margins.top);
			dfMarkupBox containingBlock = this.GetContainingBlock();
			if (containingBlock == null)
			{
				Debug.LogError("Containing block not found");
				return;
			}
			dfDynamicFont dfDynamicFont = this.Style.Font ?? this.Style.Host.Font;
			float num = (float)dfDynamicFont.FontSize / (float)dfDynamicFont.FontSize;
			float num2 = (float)dfDynamicFont.Baseline * num;
			this.currentLine = new dfMarkupBox(this.Element, dfMarkupDisplayType.block, this.Style)
			{
				Size = new Vector2(containingBlock.Size.x, (float)this.Style.LineHeight),
				Position = new Vector2(0f, (float)verticalPosition),
				Parent = this,
				Baseline = (int)num2
			};
			this.children.Add(this.currentLine);
		}
		if (this.currentLinePos == 0 && !box.Style.PreserveWhitespace && box is dfMarkupBoxText)
		{
			dfMarkupBoxText dfMarkupBoxText = box as dfMarkupBoxText;
			if (dfMarkupBoxText.IsWhitespace)
			{
				return;
			}
		}
		Vector2 vector = new Vector2((float)(this.currentLinePos + margins.left), (float)margins.top);
		box.Position = vector;
		box.Parent = this.currentLine;
		this.currentLine.children.Add(box);
		this.currentLinePos = (int)(vector.x + box.Size.x + (float)box.Margins.right);
		float num3 = Mathf.Max(this.currentLine.Size.x, vector.x + box.Size.x);
		float num4 = Mathf.Max(this.currentLine.Size.y, vector.y + box.Size.y);
		this.currentLine.Size = new Vector2(num3, num4);
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x00080768 File Offset: 0x0007E968
	private int getVerticalPosition(int topMargin)
	{
		if (this.children.Count == 0)
		{
			return topMargin;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox dfMarkupBox = this.children[i];
			float num3 = dfMarkupBox.Position.y + dfMarkupBox.Size.y + (float)dfMarkupBox.Margins.bottom;
			if (num3 > (float)num)
			{
				num = (int)num3;
				num2 = i;
			}
		}
		dfMarkupBox dfMarkupBox2 = this.children[num2];
		int num4 = Mathf.Max(dfMarkupBox2.Margins.bottom, topMargin);
		return (int)(dfMarkupBox2.Position.y + dfMarkupBox2.Size.y + (float)num4);
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x0008082C File Offset: 0x0007EA2C
	private void addBlock(dfMarkupBox box)
	{
		if (this.currentLine != null)
		{
			this.currentLine.IsNewline = true;
			this.endCurrentLine(true);
		}
		dfMarkupBox containingBlock = this.GetContainingBlock();
		if (box.Size.sqrMagnitude <= 1E-45f)
		{
			box.Size = new Vector2(containingBlock.Size.x - (float)box.Margins.horizontal, (float)this.Style.FontSize);
		}
		int verticalPosition = this.getVerticalPosition(box.Margins.top);
		box.Position = new Vector2((float)box.Margins.left, (float)verticalPosition);
		this.Size = new Vector2(this.Size.x, Mathf.Max(this.Size.y, box.Position.y + box.Size.y));
		box.Parent = this;
		this.children.Add(box);
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x00080920 File Offset: 0x0007EB20
	private void endCurrentLine()
	{
		this.endCurrentLine(false);
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x0008092C File Offset: 0x0007EB2C
	private void endCurrentLine(bool removeEmpty)
	{
		if (this.currentLine == null)
		{
			return;
		}
		if (this.currentLinePos == 0)
		{
			if (removeEmpty)
			{
				this.children.Remove(this.currentLine);
			}
		}
		else
		{
			this.currentLine.doHorizontalAlignment();
			this.currentLine.doVerticalAlignment();
		}
		this.currentLine = null;
		this.currentLinePos = 0;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x00080994 File Offset: 0x0007EB94
	private void doVerticalAlignment()
	{
		if (this.children.Count == 0)
		{
			return;
		}
		float num = float.MinValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		this.Baseline = (int)(this.Size.y * 0.95f);
		for (int i = 0; i < this.children.Count; i++)
		{
			dfMarkupBox dfMarkupBox = this.children[i];
			num3 = Mathf.Max(num3, dfMarkupBox.Position.y + (float)dfMarkupBox.Baseline);
		}
		for (int j = 0; j < this.children.Count; j++)
		{
			dfMarkupBox dfMarkupBox2 = this.children[j];
			dfMarkupVerticalAlign verticalAlign = dfMarkupBox2.Style.VerticalAlign;
			Vector2 position = dfMarkupBox2.Position;
			if (verticalAlign == dfMarkupVerticalAlign.Baseline)
			{
				position.y = num3 - (float)dfMarkupBox2.Baseline;
			}
			dfMarkupBox2.Position = position;
		}
		for (int k = 0; k < this.children.Count; k++)
		{
			dfMarkupBox dfMarkupBox3 = this.children[k];
			Vector2 position2 = dfMarkupBox3.Position;
			Vector2 size = dfMarkupBox3.Size;
			num2 = Mathf.Min(num2, position2.y);
			num = Mathf.Max(num, position2.y + size.y);
		}
		for (int l = 0; l < this.children.Count; l++)
		{
			dfMarkupBox dfMarkupBox4 = this.children[l];
			dfMarkupVerticalAlign verticalAlign2 = dfMarkupBox4.Style.VerticalAlign;
			Vector2 position3 = dfMarkupBox4.Position;
			Vector2 size2 = dfMarkupBox4.Size;
			if (verticalAlign2 == dfMarkupVerticalAlign.Top)
			{
				position3.y = num2;
			}
			else if (verticalAlign2 == dfMarkupVerticalAlign.Bottom)
			{
				position3.y = num - size2.y;
			}
			else if (verticalAlign2 == dfMarkupVerticalAlign.Middle)
			{
				position3.y = (this.Size.y - size2.y) * 0.5f;
			}
			dfMarkupBox4.Position = position3;
		}
		int num4 = int.MaxValue;
		for (int m = 0; m < this.children.Count; m++)
		{
			num4 = Mathf.Min(num4, (int)this.children[m].Position.y);
		}
		for (int n = 0; n < this.children.Count; n++)
		{
			Vector2 position4 = this.children[n].Position;
			position4.y -= (float)num4;
			this.children[n].Position = position4;
		}
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x00080C44 File Offset: 0x0007EE44
	private void doHorizontalAlignment()
	{
		if (this.Style.Align == dfMarkupTextAlign.Left || this.children.Count == 0)
		{
			return;
		}
		int i;
		for (i = this.children.Count - 1; i > 0; i--)
		{
			dfMarkupBoxText dfMarkupBoxText = this.children[i] as dfMarkupBoxText;
			if (dfMarkupBoxText == null || !dfMarkupBoxText.IsWhitespace)
			{
				break;
			}
		}
		if (this.Style.Align == dfMarkupTextAlign.Center)
		{
			float num = 0f;
			for (int j = 0; j <= i; j++)
			{
				num += this.children[j].Size.x;
			}
			float num2 = (this.Size.x - (float)this.Padding.horizontal - num) * 0.5f;
			for (int k = 0; k <= i; k++)
			{
				Vector2 position = this.children[k].Position;
				position.x += num2;
				this.children[k].Position = position;
			}
		}
		else if (this.Style.Align == dfMarkupTextAlign.Right)
		{
			float num3 = this.Size.x - (float)this.Padding.horizontal;
			for (int l = i; l >= 0; l--)
			{
				Vector2 position2 = this.children[l].Position;
				position2.x = num3 - this.children[l].Size.x;
				this.children[l].Position = position2;
				num3 -= this.children[l].Size.x;
			}
		}
		else
		{
			if (this.Style.Align != dfMarkupTextAlign.Justify)
			{
				throw new NotImplementedException("text-align: " + this.Style.Align + " is not implemented");
			}
			if (this.children.Count <= 1)
			{
				return;
			}
			if (this.IsNewline || this.children[this.children.Count - 1].IsNewline)
			{
				return;
			}
			float num4 = 0f;
			for (int m = 0; m <= i; m++)
			{
				dfMarkupBox dfMarkupBox = this.children[m];
				num4 = Mathf.Max(num4, dfMarkupBox.Position.x + dfMarkupBox.Size.x);
			}
			float num5 = (this.Size.x - (float)this.Padding.horizontal - num4) / (float)this.children.Count;
			for (int n = 1; n <= i; n++)
			{
				this.children[n].Position += new Vector2((float)n * num5, 0f);
			}
			dfMarkupBox dfMarkupBox2 = this.children[i];
			Vector2 position3 = dfMarkupBox2.Position;
			position3.x = this.Size.x - (float)this.Padding.horizontal - dfMarkupBox2.Size.x;
			dfMarkupBox2.Position = position3;
		}
	}

	// Token: 0x04001564 RID: 5476
	public Vector2 Position = Vector2.zero;

	// Token: 0x04001565 RID: 5477
	public Vector2 Size = Vector2.zero;

	// Token: 0x04001566 RID: 5478
	public dfMarkupDisplayType Display;

	// Token: 0x04001567 RID: 5479
	public dfMarkupBorders Margins = new dfMarkupBorders(0, 0, 0, 0);

	// Token: 0x04001568 RID: 5480
	public dfMarkupBorders Padding = new dfMarkupBorders(0, 0, 0, 0);

	// Token: 0x04001569 RID: 5481
	public dfMarkupStyle Style;

	// Token: 0x0400156A RID: 5482
	public bool IsNewline;

	// Token: 0x0400156B RID: 5483
	public int Baseline;

	// Token: 0x0400156C RID: 5484
	private List<dfMarkupBox> children = new List<dfMarkupBox>();

	// Token: 0x0400156D RID: 5485
	private dfMarkupBox currentLine;

	// Token: 0x0400156E RID: 5486
	private int currentLinePos;
}
