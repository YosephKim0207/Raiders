using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016BE RID: 5822
public static class ShaderCache
{
	// Token: 0x06008765 RID: 34661 RVA: 0x003827F4 File Offset: 0x003809F4
	public static Shader Acquire(string shaderName)
	{
		if (ShaderCache.m_shaderCache.ContainsKey(shaderName) && !ShaderCache.m_shaderCache[shaderName])
		{
			ShaderCache.m_shaderCache.Remove(shaderName);
		}
		if (!ShaderCache.m_shaderCache.ContainsKey(shaderName))
		{
			ShaderCache.m_shaderCache.Add(shaderName, Shader.Find(shaderName));
		}
		return ShaderCache.m_shaderCache[shaderName];
	}

	// Token: 0x06008766 RID: 34662 RVA: 0x00382860 File Offset: 0x00380A60
	public static void ClearCache()
	{
		ShaderCache.m_shaderCache.Clear();
	}

	// Token: 0x04008C91 RID: 35985
	private static Dictionary<string, Shader> m_shaderCache = new Dictionary<string, Shader>();
}
