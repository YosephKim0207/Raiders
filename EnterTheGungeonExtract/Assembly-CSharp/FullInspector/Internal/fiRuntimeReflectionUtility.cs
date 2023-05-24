using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x0200057A RID: 1402
	public class fiRuntimeReflectionUtility
	{
		// Token: 0x06002112 RID: 8466 RVA: 0x00091D68 File Offset: 0x0008FF68
		public static object InvokeStaticMethod(Type type, string methodName, object[] parameters)
		{
			try
			{
				return type.GetFlattenedMethod(methodName).Invoke(null, parameters);
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00091DA4 File Offset: 0x0008FFA4
		public static object InvokeStaticMethod(string typeName, string methodName, object[] parameters)
		{
			return fiRuntimeReflectionUtility.InvokeStaticMethod(TypeCache.FindType(typeName), methodName, parameters);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00091DB4 File Offset: 0x0008FFB4
		public static void InvokeMethod(Type type, string methodName, object thisInstance, object[] parameters)
		{
			try
			{
				type.GetFlattenedMethod(methodName).Invoke(thisInstance, parameters);
			}
			catch
			{
			}
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00091DEC File Offset: 0x0008FFEC
		public static T ReadField<TContext, T>(TContext context, string fieldName)
		{
			MemberInfo[] flattenedMember = typeof(TContext).GetFlattenedMember(fieldName);
			if (flattenedMember == null || flattenedMember.Length == 0)
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + " does not contain a field named \"" + fieldName + "\"");
			}
			if (flattenedMember.Length > 1)
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + " has more than one field named \"" + fieldName + "\"");
			}
			FieldInfo fieldInfo = flattenedMember[0] as FieldInfo;
			if (fieldInfo == null)
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + "." + fieldName + " is not a field");
			}
			if (fieldInfo.FieldType != typeof(T))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					typeof(TContext).CSharpName(),
					".",
					fieldName,
					" type is not compatable with ",
					typeof(T).CSharpName()
				}));
			}
			return (T)((object)fieldInfo.GetValue(context));
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00091F10 File Offset: 0x00090110
		public static T ReadFields<TContext, T>(TContext context, params string[] fieldNames)
		{
			foreach (string text in fieldNames)
			{
				MemberInfo[] flattenedMember = typeof(TContext).GetFlattenedMember(text);
				if (flattenedMember != null && flattenedMember.Length != 0)
				{
					if (flattenedMember.Length <= 1)
					{
						FieldInfo fieldInfo = flattenedMember[0] as FieldInfo;
						if (fieldInfo != null)
						{
							if (fieldInfo.FieldType == typeof(T))
							{
								return (T)((object)fieldInfo.GetValue(context));
							}
						}
					}
				}
			}
			throw new ArgumentException(string.Concat(new object[]
			{
				"Unable to read any of the following fields ",
				string.Join(", ", fieldNames),
				" on ",
				context
			}));
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00091FE0 File Offset: 0x000901E0
		public static IEnumerable<TInterface> GetAssemblyInstances<TInterface>()
		{
			return from assembly in fiRuntimeReflectionUtility.GetUserDefinedEditorAssemblies()
				from type in assembly.GetTypes()
				where !type.Resolve().IsGenericTypeDefinition
				where !type.Resolve().IsAbstract
				where !type.Resolve().IsInterface
				where typeof(TInterface).IsAssignableFrom(type)
				where type.GetDeclaredConstructor(fsPortableReflection.EmptyTypes) != null
				select (TInterface)((object)Activator.CreateInstance(type));
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x00092078 File Offset: 0x00090278
		public static IEnumerable<Type> GetUnityObjectTypes()
		{
			return from assembly in fiRuntimeReflectionUtility.GetRuntimeAssemblies()
				from type in assembly.GetTypes()
				where type.Resolve().IsVisible
				where !type.Resolve().IsGenericTypeDefinition
				where typeof(UnityEngine.Object).IsAssignableFrom(type)
				select type;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00092154 File Offset: 0x00090354
		private static string GetName(Assembly assembly)
		{
			int num = assembly.FullName.IndexOf(",");
			if (num >= 0)
			{
				return assembly.FullName.Substring(0, num);
			}
			return assembly.FullName;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00092190 File Offset: 0x00090390
		public static IEnumerable<Assembly> GetRuntimeAssemblies()
		{
			if (fiRuntimeReflectionUtility._cachedRuntimeAssemblies == null)
			{
				fiRuntimeReflectionUtility._cachedRuntimeAssemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where !fiRuntimeReflectionUtility.IsBannedAssembly(assembly)
					where !fiRuntimeReflectionUtility.IsUnityEditorAssembly(assembly)
					where !fiRuntimeReflectionUtility.GetName(assembly).Contains("-Editor")
					select assembly).ToList<Assembly>();
				fiLog.Blank();
				foreach (Assembly assembly2 in fiRuntimeReflectionUtility._cachedRuntimeAssemblies)
				{
					fiLog.Log(typeof(fiRuntimeReflectionUtility), "GetRuntimeAssemblies - " + fiRuntimeReflectionUtility.GetName(assembly2));
				}
				fiLog.Blank();
			}
			return fiRuntimeReflectionUtility._cachedRuntimeAssemblies;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x00092298 File Offset: 0x00090498
		public static IEnumerable<Assembly> GetUserDefinedEditorAssemblies()
		{
			if (fiRuntimeReflectionUtility._cachedUserDefinedEditorAssemblies == null)
			{
				fiRuntimeReflectionUtility._cachedUserDefinedEditorAssemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where !fiRuntimeReflectionUtility.IsBannedAssembly(assembly)
					where !fiRuntimeReflectionUtility.IsUnityEditorAssembly(assembly)
					select assembly).ToList<Assembly>();
				fiLog.Blank();
				foreach (Assembly assembly2 in fiRuntimeReflectionUtility._cachedUserDefinedEditorAssemblies)
				{
					fiLog.Log(typeof(fiRuntimeReflectionUtility), "GetUserDefinedEditorAssemblies - " + fiRuntimeReflectionUtility.GetName(assembly2));
				}
				fiLog.Blank();
			}
			return fiRuntimeReflectionUtility._cachedUserDefinedEditorAssemblies;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0009237C File Offset: 0x0009057C
		public static IEnumerable<Assembly> GetAllEditorAssemblies()
		{
			if (fiRuntimeReflectionUtility._cachedAllEditorAssembles == null)
			{
				fiRuntimeReflectionUtility._cachedAllEditorAssembles = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where !fiRuntimeReflectionUtility.IsBannedAssembly(assembly)
					select assembly).ToList<Assembly>();
				fiLog.Blank();
				foreach (Assembly assembly2 in fiRuntimeReflectionUtility._cachedAllEditorAssembles)
				{
					fiLog.Log(typeof(fiRuntimeReflectionUtility), "GetAllEditorAssemblies - " + fiRuntimeReflectionUtility.GetName(assembly2));
				}
				fiLog.Blank();
			}
			return fiRuntimeReflectionUtility._cachedAllEditorAssembles;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00092440 File Offset: 0x00090640
		private static bool IsUnityEditorAssembly(Assembly assembly)
		{
			string[] array = new string[] { "UnityEditor", "UnityEditor.UI" };
			return array.Contains(fiRuntimeReflectionUtility.GetName(assembly));
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x00092470 File Offset: 0x00090670
		private static bool IsBannedAssembly(Assembly assembly)
		{
			string[] array = new string[]
			{
				"AssetStoreTools", "AssetStoreToolsExtra", "UnityScript", "UnityScript.Lang", "Boo.Lang.Parser", "Boo.Lang", "Boo.Lang.Compiler", "mscorlib", "System.ComponentModel.DataAnnotations", "System.Xml.Linq",
				"ICSharpCode.NRefactory", "Mono.Cecil", "Mono.Cecil.Mdb", "Unity.DataContract", "Unity.IvyParser", "Unity.Locator", "Unity.PackageManager", "Unity.SerializationLogic", "UnityEngine.UI", "UnityEditor.Android.Extensions",
				"UnityEditor.BB10.Extensions", "UnityEditor.Metro.Extensions", "UnityEditor.WP8.Extensions", "UnityEditor.iOS.Extensions", "UnityEditor.iOS.Extensions.Xcode", "UnityEditor.WindowsStandalone.Extensions", "UnityEditor.LinuxStandalone.Extensions", "UnityEditor.OSXStandalone.Extensions", "UnityEditor.WebGL.Extensions", "UnityEditor.Graphs",
				"protobuf-net", "Newtonsoft.Json", "System", "System.Configuration", "System.Xml", "System.Core", "Mono.Security", "I18N", "I18N.West", "nunit.core",
				"nunit.core.interfaces", "nunit.framework", "NSubstitute", "UnityVS.VersionSpecific", "SyntaxTree.VisualStudio.Unity.Bridge", "SyntaxTree.VisualStudio.Unity.Messaging"
			};
			return array.Contains(fiRuntimeReflectionUtility.GetName(assembly));
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00092628 File Offset: 0x00090828
		public static IEnumerable<Type> AllSimpleTypesDerivingFrom(Type baseType)
		{
			return from assembly in fiRuntimeReflectionUtility.GetRuntimeAssemblies()
				from type in assembly.GetTypes()
				where baseType.IsAssignableFrom(type)
				where type.Resolve().IsClass
				where !type.Resolve().IsGenericTypeDefinition
				select type;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00092700 File Offset: 0x00090900
		public static IEnumerable<Type> AllSimpleCreatableTypesDerivingFrom(Type baseType)
		{
			return from type in fiRuntimeReflectionUtility.AllSimpleTypesDerivingFrom(baseType)
				where !type.Resolve().IsAbstract
				where !type.Resolve().IsGenericType
				where type.GetDeclaredConstructor(fsPortableReflection.EmptyTypes) != null
				select type;
		}

		// Token: 0x040017DF RID: 6111
		private static List<Assembly> _cachedRuntimeAssemblies;

		// Token: 0x040017E0 RID: 6112
		private static List<Assembly> _cachedUserDefinedEditorAssemblies;

		// Token: 0x040017E1 RID: 6113
		private static List<Assembly> _cachedAllEditorAssembles;
	}
}
