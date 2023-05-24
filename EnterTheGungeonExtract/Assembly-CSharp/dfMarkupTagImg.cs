using System;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
[dfMarkupTagInfo("img")]
public class dfMarkupTagImg : dfMarkupTag
{
	// Token: 0x06001BE4 RID: 7140 RVA: 0x00083D58 File Offset: 0x00081F58
	public dfMarkupTagImg()
		: base("img")
	{
		this.IsClosedTag = true;
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x00083D6C File Offset: 0x00081F6C
	public dfMarkupTagImg(dfMarkupTag original)
		: base(original)
	{
		this.IsClosedTag = true;
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x00083D7C File Offset: 0x00081F7C
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.Owner == null)
		{
			Debug.LogError("Tag has no parent: " + this);
			return;
		}
		style = this.applyStyleAttributes(style);
		dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "src" });
		if (dfMarkupAttribute == null)
		{
			return;
		}
		string value = dfMarkupAttribute.Value;
		dfMarkupBox dfMarkupBox = this.createImageBox(base.Owner.Atlas, value, style);
		if (dfMarkupBox == null)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		dfMarkupAttribute dfMarkupAttribute2 = base.findAttribute(new string[] { "height" });
		if (dfMarkupAttribute2 != null)
		{
			vector.y = (float)dfMarkupStyle.ParseSize(dfMarkupAttribute2.Value, (int)dfMarkupBox.Size.y);
		}
		dfMarkupAttribute dfMarkupAttribute3 = base.findAttribute(new string[] { "width" });
		if (dfMarkupAttribute3 != null)
		{
			vector.x = (float)dfMarkupStyle.ParseSize(dfMarkupAttribute3.Value, (int)dfMarkupBox.Size.x);
		}
		if (vector.sqrMagnitude <= 1E-45f)
		{
			vector = dfMarkupBox.Size;
		}
		else if (vector.x <= 1E-45f)
		{
			vector.x = vector.y * (dfMarkupBox.Size.x / dfMarkupBox.Size.y);
		}
		else if (vector.y <= 1E-45f)
		{
			vector.y = vector.x * (dfMarkupBox.Size.y / dfMarkupBox.Size.x);
		}
		dfMarkupBox.Size = vector;
		dfMarkupBox.Baseline = (int)vector.y;
		container.AddChild(dfMarkupBox);
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x00083F20 File Offset: 0x00082120
	private dfMarkupStyle applyStyleAttributes(dfMarkupStyle style)
	{
		dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "valign" });
		if (dfMarkupAttribute != null)
		{
			style.VerticalAlign = dfMarkupStyle.ParseVerticalAlignment(dfMarkupAttribute.Value);
		}
		dfMarkupAttribute dfMarkupAttribute2 = base.findAttribute(new string[] { "color" });
		if (dfMarkupAttribute2 != null)
		{
			Color color = dfMarkupStyle.ParseColor(dfMarkupAttribute2.Value, base.Owner.Color);
			color.a = style.Opacity;
			style.Color = color;
		}
		return style;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x00083FA8 File Offset: 0x000821A8
	private dfMarkupBox createImageBox(dfAtlas atlas, string source, dfMarkupStyle style)
	{
		if (source.ToLowerInvariant().StartsWith("http://"))
		{
			return null;
		}
		if (atlas != null && atlas[source] != null)
		{
			dfMarkupBoxSprite dfMarkupBoxSprite = new dfMarkupBoxSprite(this, dfMarkupDisplayType.inline, style);
			dfMarkupBoxSprite.LoadImage(atlas, source);
			return dfMarkupBoxSprite;
		}
		Texture texture = dfMarkupImageCache.Load(source);
		if (texture != null)
		{
			dfMarkupBoxTexture dfMarkupBoxTexture = new dfMarkupBoxTexture(this, dfMarkupDisplayType.inline, style);
			dfMarkupBoxTexture.LoadTexture(texture);
			return dfMarkupBoxTexture;
		}
		return null;
	}
}
