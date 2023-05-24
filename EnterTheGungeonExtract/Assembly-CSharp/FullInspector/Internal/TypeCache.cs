using System;
using System.Collections.Generic;
using System.Reflection;

namespace FullInspector.Internal
{
	// Token: 0x020005DE RID: 1502
	public static class TypeCache
	{
		// Token: 0x060023A4 RID: 9124 RVA: 0x0009C4F4 File Offset: 0x0009A6F4
		static TypeCache()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				TypeCache._assembliesByName[assembly.FullName] = assembly;
				TypeCache._assembliesByIndex.Add(assembly);
			}
			TypeCache._cachedTypes = new Dictionary<string, Type>();
			AppDomain.CurrentDomain.AssemblyLoad += TypeCache.OnAssemblyLoaded;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0009C590 File Offset: 0x0009A790
		private static void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			TypeCache._assembliesByName[args.LoadedAssembly.FullName] = args.LoadedAssembly;
			TypeCache._assembliesByIndex.Add(args.LoadedAssembly);
			TypeCache._cachedTypes = new Dictionary<string, Type>();
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0009C5C8 File Offset: 0x0009A7C8
		private static bool TryDirectTypeLookup(string assemblyName, string typeName, out Type type)
		{
			Assembly assembly;
			if (assemblyName != null && TypeCache._assembliesByName.TryGetValue(assemblyName, out assembly))
			{
				type = assembly.GetType(typeName, false);
				return type != null;
			}
			type = null;
			return false;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0009C604 File Offset: 0x0009A804
		private static bool TryIndirectTypeLookup(string typeName, out Type type)
		{
			for (int i = 0; i < TypeCache._assembliesByIndex.Count; i++)
			{
				Assembly assembly = TypeCache._assembliesByIndex[i];
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return true;
				}
				foreach (Type type2 in assembly.GetTypes())
				{
					if (type2.FullName == typeName)
					{
						type = type2;
						return true;
					}
				}
			}
			type = null;
			return false;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0009C688 File Offset: 0x0009A888
		public static void Reset()
		{
			TypeCache._cachedTypes = new Dictionary<string, Type>();
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0009C694 File Offset: 0x0009A894
		public static Type FindType(string name)
		{
			return TypeCache.FindType(name, null);
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0009C6A0 File Offset: 0x0009A8A0
		public static Type FindType(string name, string assemblyHint)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type type;
			if (!TypeCache._cachedTypes.TryGetValue(name, out type))
			{
				if (TypeCache.TryDirectTypeLookup(assemblyHint, name, out type) || !TypeCache.TryIndirectTypeLookup(name, out type))
				{
				}
				TypeCache._cachedTypes[name] = type;
			}
			return type;
		}

		// Token: 0x040018BE RID: 6334
		private static Dictionary<string, Type> _cachedTypes = new Dictionary<string, Type>();

		// Token: 0x040018BF RID: 6335
		private static Dictionary<string, Assembly> _assembliesByName = new Dictionary<string, Assembly>();

		// Token: 0x040018C0 RID: 6336
		private static List<Assembly> _assembliesByIndex = new List<Assembly>();
	}
}
