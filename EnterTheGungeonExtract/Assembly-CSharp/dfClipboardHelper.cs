using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class dfClipboardHelper
{
	// Token: 0x06001654 RID: 5716 RVA: 0x00069F88 File Offset: 0x00068188
	private static PropertyInfo GetSystemCopyBufferProperty()
	{
		if (dfClipboardHelper.m_systemCopyBufferProperty == null)
		{
			Type typeFromHandle = typeof(GUIUtility);
			dfClipboardHelper.m_systemCopyBufferProperty = typeFromHandle.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
			if (dfClipboardHelper.m_systemCopyBufferProperty == null)
			{
				throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
			}
		}
		return dfClipboardHelper.m_systemCopyBufferProperty;
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06001655 RID: 5717 RVA: 0x00069FD8 File Offset: 0x000681D8
	// (set) Token: 0x06001656 RID: 5718 RVA: 0x0006A01C File Offset: 0x0006821C
	public static string clipBoard
	{
		get
		{
			string text;
			try
			{
				PropertyInfo systemCopyBufferProperty = dfClipboardHelper.GetSystemCopyBufferProperty();
				text = (string)systemCopyBufferProperty.GetValue(null, null);
			}
			catch
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			try
			{
				PropertyInfo systemCopyBufferProperty = dfClipboardHelper.GetSystemCopyBufferProperty();
				systemCopyBufferProperty.SetValue(null, value, null);
			}
			catch
			{
			}
		}
	}

	// Token: 0x04001292 RID: 4754
	private static PropertyInfo m_systemCopyBufferProperty;
}
