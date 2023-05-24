using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

// Token: 0x020004A2 RID: 1186
public class dfMarkupParser
{
	// Token: 0x06001B91 RID: 7057 RVA: 0x00081FF4 File Offset: 0x000801F4
	static dfMarkupParser()
	{
		RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant;
		dfMarkupParser.TAG_PATTERN = new Regex("(\\<\\/?)(?<tag>[a-zA-Z0-9$_]+)(\\s(?<attr>.+?))?([\\/]*\\>)", regexOptions);
		dfMarkupParser.ATTR_PATTERN = new Regex("(?<key>[a-zA-Z0-9$_]+)=(?<value>(\"((\\\\\")|\\\\\\\\|[^\"\\n])*\")|('((\\\\')|\\\\\\\\|[^'\\n])*')|\\d+|\\w+)", regexOptions);
		dfMarkupParser.STYLE_PATTERN = new Regex("(?<key>[a-zA-Z0-9\\-]+)(\\s*\\:\\s*)(?<value>[^;]+)", regexOptions);
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x00082064 File Offset: 0x00080264
	public static dfList<dfMarkupElement> Parse(dfRichTextLabel owner, string source)
	{
		dfList<dfMarkupElement> dfList2;
		try
		{
			dfMarkupParser.parserInstance.owner = owner;
			dfList<dfMarkupElement> dfList = dfMarkupParser.parserInstance.parseMarkup(source);
			dfList2 = dfList;
		}
		finally
		{
		}
		return dfList2;
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x000820A4 File Offset: 0x000802A4
	private dfList<dfMarkupElement> parseMarkup(string source)
	{
		Queue<dfMarkupElement> queue = new Queue<dfMarkupElement>();
		MatchCollection matchCollection = dfMarkupParser.TAG_PATTERN.Matches(source);
		int num = 0;
		for (int i = 0; i < matchCollection.Count; i++)
		{
			Match match = matchCollection[i];
			if (match.Index > num)
			{
				string text = source.Substring(num, match.Index - num);
				dfMarkupString dfMarkupString = new dfMarkupString(text);
				queue.Enqueue(dfMarkupString);
			}
			num = match.Index + match.Length;
			queue.Enqueue(this.parseTag(match));
		}
		if (num < source.Length)
		{
			string text2 = source.Substring(num);
			dfMarkupString dfMarkupString2 = new dfMarkupString(text2);
			queue.Enqueue(dfMarkupString2);
		}
		return this.processTokens(queue);
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x00082160 File Offset: 0x00080360
	private dfList<dfMarkupElement> processTokens(Queue<dfMarkupElement> tokens)
	{
		dfList<dfMarkupElement> dfList = dfList<dfMarkupElement>.Obtain();
		while (tokens.Count > 0)
		{
			dfList.Add(this.parseElement(tokens));
		}
		for (int i = 0; i < dfList.Count; i++)
		{
			if (dfList[i] is dfMarkupTag)
			{
				((dfMarkupTag)dfList[i]).Owner = this.owner;
			}
		}
		return dfList;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000821D4 File Offset: 0x000803D4
	private dfMarkupElement parseElement(Queue<dfMarkupElement> tokens)
	{
		dfMarkupElement dfMarkupElement = tokens.Dequeue();
		if (dfMarkupElement is dfMarkupString)
		{
			return ((dfMarkupString)dfMarkupElement).SplitWords();
		}
		dfMarkupTag dfMarkupTag = (dfMarkupTag)dfMarkupElement;
		if (dfMarkupTag.IsClosedTag || dfMarkupTag.IsEndTag)
		{
			return this.refineTag(dfMarkupTag);
		}
		while (tokens.Count > 0)
		{
			dfMarkupElement dfMarkupElement2 = this.parseElement(tokens);
			if (dfMarkupElement2 is dfMarkupTag)
			{
				dfMarkupTag dfMarkupTag2 = (dfMarkupTag)dfMarkupElement2;
				if (dfMarkupTag2.IsEndTag)
				{
					if (dfMarkupTag2.TagName == dfMarkupTag.TagName)
					{
						break;
					}
					return this.refineTag(dfMarkupTag);
				}
			}
			dfMarkupTag.AddChildNode(dfMarkupElement2);
		}
		return this.refineTag(dfMarkupTag);
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x0008228C File Offset: 0x0008048C
	private dfMarkupTag refineTag(dfMarkupTag original)
	{
		if (original.IsEndTag)
		{
			return original;
		}
		if (dfMarkupParser.tagTypes == null)
		{
			dfMarkupParser.tagTypes = new Dictionary<string, Type>();
			foreach (Type type in this.getAssemblyTypes())
			{
				if (typeof(dfMarkupTag).IsAssignableFrom(type))
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(dfMarkupTagInfoAttribute), true);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							string tagName = ((dfMarkupTagInfoAttribute)customAttributes[j]).TagName;
							dfMarkupParser.tagTypes[tagName] = type;
						}
					}
				}
			}
		}
		if (dfMarkupParser.tagTypes.ContainsKey(original.TagName))
		{
			Type type2 = dfMarkupParser.tagTypes[original.TagName];
			return (dfMarkupTag)Activator.CreateInstance(type2, new object[] { original });
		}
		return original;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x0008238C File Offset: 0x0008058C
	private Type[] getAssemblyTypes()
	{
		return Assembly.GetExecutingAssembly().GetExportedTypes();
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x00082398 File Offset: 0x00080598
	private dfMarkupElement parseTag(Match tag)
	{
		string text = tag.Groups["tag"].Value.ToLowerInvariant();
		if (tag.Value.StartsWith("</"))
		{
			return new dfMarkupTag(text)
			{
				IsEndTag = true
			};
		}
		dfMarkupTag dfMarkupTag = new dfMarkupTag(text);
		string value = tag.Groups["attr"].Value;
		MatchCollection matchCollection = dfMarkupParser.ATTR_PATTERN.Matches(value);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			Match match = matchCollection[i];
			string value2 = match.Groups["key"].Value;
			string text2 = dfMarkupEntity.Replace(match.Groups["value"].Value);
			if (text2.StartsWith("\""))
			{
				text2 = text2.Trim(new char[] { '"' });
			}
			else if (text2.StartsWith("'"))
			{
				text2 = text2.Trim(new char[] { '\'' });
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (value2 == "style")
				{
					this.parseStyleAttribute(dfMarkupTag, text2);
				}
				else
				{
					dfMarkupTag.Attributes.Add(new dfMarkupAttribute(value2, text2));
				}
			}
		}
		if (tag.Value.EndsWith("/>") || text == "br" || text == "img")
		{
			dfMarkupTag.IsClosedTag = true;
		}
		return dfMarkupTag;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x0008253C File Offset: 0x0008073C
	private void parseStyleAttribute(dfMarkupTag element, string text)
	{
		MatchCollection matchCollection = dfMarkupParser.STYLE_PATTERN.Matches(text);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			Match match = matchCollection[i];
			string text2 = match.Groups["key"].Value.ToLowerInvariant();
			string value = match.Groups["value"].Value;
			element.Attributes.Add(new dfMarkupAttribute(text2, value));
		}
	}

	// Token: 0x0400158C RID: 5516
	private static Regex TAG_PATTERN = null;

	// Token: 0x0400158D RID: 5517
	private static Regex ATTR_PATTERN = null;

	// Token: 0x0400158E RID: 5518
	private static Regex STYLE_PATTERN = null;

	// Token: 0x0400158F RID: 5519
	private static Dictionary<string, Type> tagTypes = null;

	// Token: 0x04001590 RID: 5520
	private static dfMarkupParser parserInstance = new dfMarkupParser();

	// Token: 0x04001591 RID: 5521
	private dfRichTextLabel owner;
}
