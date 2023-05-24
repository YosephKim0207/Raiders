using System;

// Token: 0x020003FC RID: 1020
public static class dfStringExtensions
{
	// Token: 0x06001658 RID: 5720 RVA: 0x0006A058 File Offset: 0x00068258
	public static string MakeRelativePath(this string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return string.Empty;
		}
		return path.Substring(path.IndexOf("Assets/", StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x0006A080 File Offset: 0x00068280
	public static bool Contains(this string value, string pattern, bool caseInsensitive)
	{
		if (caseInsensitive)
		{
			return value.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) != -1;
		}
		return value.IndexOf(pattern) != -1;
	}
}
