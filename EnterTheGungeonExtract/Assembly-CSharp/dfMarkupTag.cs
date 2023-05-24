using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class dfMarkupTag : dfMarkupElement
{
	// Token: 0x06001BAB RID: 7083 RVA: 0x00082C8C File Offset: 0x00080E8C
	public dfMarkupTag(string tagName)
	{
		this.Attributes = new List<dfMarkupAttribute>();
		this.TagName = tagName;
		this.id = tagName + dfMarkupTag.ELEMENTID++.ToString("X");
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x00082CD8 File Offset: 0x00080ED8
	public dfMarkupTag(dfMarkupTag original)
	{
		this.TagName = original.TagName;
		this.Attributes = original.Attributes;
		this.IsEndTag = original.IsEndTag;
		this.IsClosedTag = original.IsClosedTag;
		this.IsInline = original.IsInline;
		this.id = original.id;
		List<dfMarkupElement> childNodes = original.ChildNodes;
		for (int i = 0; i < childNodes.Count; i++)
		{
			dfMarkupElement dfMarkupElement = childNodes[i];
			base.AddChildNode(dfMarkupElement);
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06001BAD RID: 7085 RVA: 0x00082D60 File Offset: 0x00080F60
	// (set) Token: 0x06001BAE RID: 7086 RVA: 0x00082D68 File Offset: 0x00080F68
	public string TagName { get; set; }

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06001BAF RID: 7087 RVA: 0x00082D74 File Offset: 0x00080F74
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x00082D7C File Offset: 0x00080F7C
	// (set) Token: 0x06001BB1 RID: 7089 RVA: 0x00082D84 File Offset: 0x00080F84
	public virtual bool IsEndTag { get; set; }

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x00082D90 File Offset: 0x00080F90
	// (set) Token: 0x06001BB3 RID: 7091 RVA: 0x00082D98 File Offset: 0x00080F98
	public virtual bool IsClosedTag { get; set; }

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x00082DA4 File Offset: 0x00080FA4
	// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x00082DAC File Offset: 0x00080FAC
	public virtual bool IsInline { get; set; }

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x00082DB8 File Offset: 0x00080FB8
	// (set) Token: 0x06001BB7 RID: 7095 RVA: 0x00082DC0 File Offset: 0x00080FC0
	public dfRichTextLabel Owner
	{
		get
		{
			return this.owner;
		}
		set
		{
			this.owner = value;
			for (int i = 0; i < base.ChildNodes.Count; i++)
			{
				dfMarkupTag dfMarkupTag = base.ChildNodes[i] as dfMarkupTag;
				if (dfMarkupTag != null)
				{
					dfMarkupTag.Owner = value;
				}
			}
		}
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x00082E10 File Offset: 0x00081010
	internal override void Release()
	{
		base.Release();
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00082E18 File Offset: 0x00081018
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (this.IsEndTag)
		{
			return;
		}
		dfMarkupAttribute dfMarkupAttribute = this.findAttribute(new string[] { "margin" });
		if (dfMarkupAttribute != null)
		{
		}
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			base.ChildNodes[i].PerformLayout(container, style);
		}
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x00082E7C File Offset: 0x0008107C
	protected dfMarkupStyle applyTextStyleAttributes(dfMarkupStyle style)
	{
		dfMarkupAttribute dfMarkupAttribute = this.findAttribute(new string[] { "font", "font-family" });
		if (dfMarkupAttribute != null)
		{
			style.Font = dfDynamicFont.FindByName(dfMarkupAttribute.Value) ?? this.owner.Font;
		}
		dfMarkupAttribute dfMarkupAttribute2 = this.findAttribute(new string[] { "style", "font-style" });
		if (dfMarkupAttribute2 != null)
		{
			style.FontStyle = dfMarkupStyle.ParseFontStyle(dfMarkupAttribute2.Value, style.FontStyle);
		}
		dfMarkupAttribute dfMarkupAttribute3 = this.findAttribute(new string[] { "size", "font-size" });
		if (dfMarkupAttribute3 != null)
		{
			style.FontSize = dfMarkupStyle.ParseSize(dfMarkupAttribute3.Value, style.FontSize);
		}
		dfMarkupAttribute dfMarkupAttribute4 = this.findAttribute(new string[] { "color" });
		if (dfMarkupAttribute4 != null)
		{
			Color color = dfMarkupStyle.ParseColor(dfMarkupAttribute4.Value, style.Color);
			color.a = style.Opacity;
			style.Color = color;
		}
		dfMarkupAttribute dfMarkupAttribute5 = this.findAttribute(new string[] { "align", "text-align" });
		if (dfMarkupAttribute5 != null)
		{
			style.Align = dfMarkupStyle.ParseTextAlignment(dfMarkupAttribute5.Value);
		}
		dfMarkupAttribute dfMarkupAttribute6 = this.findAttribute(new string[] { "valign", "vertical-align" });
		if (dfMarkupAttribute6 != null)
		{
			style.VerticalAlign = dfMarkupStyle.ParseVerticalAlignment(dfMarkupAttribute6.Value);
		}
		dfMarkupAttribute dfMarkupAttribute7 = this.findAttribute(new string[] { "line-height" });
		if (dfMarkupAttribute7 != null)
		{
			style.LineHeight = dfMarkupStyle.ParseSize(dfMarkupAttribute7.Value, style.LineHeight);
		}
		dfMarkupAttribute dfMarkupAttribute8 = this.findAttribute(new string[] { "text-decoration" });
		if (dfMarkupAttribute8 != null)
		{
			style.TextDecoration = dfMarkupStyle.ParseTextDecoration(dfMarkupAttribute8.Value);
		}
		dfMarkupAttribute dfMarkupAttribute9 = this.findAttribute(new string[] { "background", "background-color" });
		if (dfMarkupAttribute9 != null)
		{
			style.BackgroundColor = dfMarkupStyle.ParseColor(dfMarkupAttribute9.Value, Color.clear);
			style.BackgroundColor.a = style.Opacity;
		}
		return style;
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x000830B8 File Offset: 0x000812B8
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		if (this.IsEndTag)
		{
			stringBuilder.Append("/");
		}
		stringBuilder.Append(this.TagName);
		for (int i = 0; i < this.Attributes.Count; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Attributes[i].ToString());
		}
		if (this.IsClosedTag)
		{
			stringBuilder.Append("/");
		}
		stringBuilder.Append("]");
		if (!this.IsClosedTag)
		{
			for (int j = 0; j < base.ChildNodes.Count; j++)
			{
				stringBuilder.Append(base.ChildNodes[j].ToString());
			}
			stringBuilder.Append("[/");
			stringBuilder.Append(this.TagName);
			stringBuilder.Append("]");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x000831C8 File Offset: 0x000813C8
	protected dfMarkupAttribute findAttribute(params string[] names)
	{
		for (int i = 0; i < this.Attributes.Count; i++)
		{
			for (int j = 0; j < names.Length; j++)
			{
				if (this.Attributes[i].Name == names[j])
				{
					return this.Attributes[i];
				}
			}
		}
		return null;
	}

	// Token: 0x040015B8 RID: 5560
	private static int ELEMENTID;

	// Token: 0x040015BD RID: 5565
	public List<dfMarkupAttribute> Attributes;

	// Token: 0x040015BE RID: 5566
	private dfRichTextLabel owner;

	// Token: 0x040015BF RID: 5567
	private string id;
}
