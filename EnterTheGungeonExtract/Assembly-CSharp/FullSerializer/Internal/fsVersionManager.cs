using System;
using System.Collections.Generic;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x020005BE RID: 1470
	public static class fsVersionManager
	{
		// Token: 0x060022EB RID: 8939 RVA: 0x00099ADC File Offset: 0x00097CDC
		public static fsResult GetVersionImportPath(string currentVersion, fsVersionedType targetVersion, out List<fsVersionedType> path)
		{
			path = new List<fsVersionedType>();
			if (!fsVersionManager.GetVersionImportPathRecursive(path, currentVersion, targetVersion))
			{
				return fsResult.Fail(string.Concat(new string[] { "There is no migration path from \"", currentVersion, "\" to \"", targetVersion.VersionString, "\"" }));
			}
			path.Add(targetVersion);
			return fsResult.Success;
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x00099B44 File Offset: 0x00097D44
		private static bool GetVersionImportPathRecursive(List<fsVersionedType> path, string currentVersion, fsVersionedType current)
		{
			for (int i = 0; i < current.Ancestors.Length; i++)
			{
				fsVersionedType fsVersionedType = current.Ancestors[i];
				if (fsVersionedType.VersionString == currentVersion || fsVersionManager.GetVersionImportPathRecursive(path, currentVersion, fsVersionedType))
				{
					path.Add(fsVersionedType);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x00099BA8 File Offset: 0x00097DA8
		public static fsOption<fsVersionedType> GetVersionedType(Type type)
		{
			fsOption<fsVersionedType> fsOption;
			if (!fsVersionManager._cache.TryGetValue(type, out fsOption))
			{
				fsObjectAttribute attribute = fsPortableReflection.GetAttribute<fsObjectAttribute>(type);
				if (attribute != null && (!string.IsNullOrEmpty(attribute.VersionString) || attribute.PreviousModels != null))
				{
					if (attribute.PreviousModels != null && string.IsNullOrEmpty(attribute.VersionString))
					{
						throw new Exception("fsObject attribute on " + type + " contains a PreviousModels specifier - it must also include a VersionString modifier");
					}
					fsVersionedType[] array = new fsVersionedType[(attribute.PreviousModels == null) ? 0 : attribute.PreviousModels.Length];
					for (int i = 0; i < array.Length; i++)
					{
						fsOption<fsVersionedType> versionedType = fsVersionManager.GetVersionedType(attribute.PreviousModels[i]);
						if (versionedType.IsEmpty)
						{
							throw new Exception("Unable to create versioned type for ancestor " + versionedType + "; please add an [fsObject(VersionString=\"...\")] attribute");
						}
						array[i] = versionedType.Value;
					}
					fsVersionedType fsVersionedType = new fsVersionedType
					{
						Ancestors = array,
						VersionString = attribute.VersionString,
						ModelType = type
					};
					fsVersionManager.VerifyUniqueVersionStrings(fsVersionedType);
					fsVersionManager.VerifyConstructors(fsVersionedType);
					fsOption = fsOption.Just<fsVersionedType>(fsVersionedType);
				}
				fsVersionManager._cache[type] = fsOption;
			}
			return fsOption;
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x00099CEC File Offset: 0x00097EEC
		private static void VerifyConstructors(fsVersionedType type)
		{
			ConstructorInfo[] declaredConstructors = type.ModelType.GetDeclaredConstructors();
			for (int i = 0; i < type.Ancestors.Length; i++)
			{
				Type modelType = type.Ancestors[i].ModelType;
				bool flag = false;
				for (int j = 0; j < declaredConstructors.Length; j++)
				{
					ParameterInfo[] parameters = declaredConstructors[j].GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == modelType)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					throw new fsMissingVersionConstructorException(type.ModelType, modelType);
				}
			}
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x00099D8C File Offset: 0x00097F8C
		private static void VerifyUniqueVersionStrings(fsVersionedType type)
		{
			Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
			Queue<fsVersionedType> queue = new Queue<fsVersionedType>();
			queue.Enqueue(type);
			while (queue.Count > 0)
			{
				fsVersionedType fsVersionedType = queue.Dequeue();
				if (dictionary.ContainsKey(fsVersionedType.VersionString) && dictionary[fsVersionedType.VersionString] != fsVersionedType.ModelType)
				{
					throw new fsDuplicateVersionNameException(dictionary[fsVersionedType.VersionString], fsVersionedType.ModelType, fsVersionedType.VersionString);
				}
				dictionary[fsVersionedType.VersionString] = fsVersionedType.ModelType;
				foreach (fsVersionedType fsVersionedType2 in fsVersionedType.Ancestors)
				{
					queue.Enqueue(fsVersionedType2);
				}
			}
		}

		// Token: 0x04001882 RID: 6274
		private static readonly Dictionary<Type, fsOption<fsVersionedType>> _cache = new Dictionary<Type, fsOption<fsVersionedType>>();
	}
}
