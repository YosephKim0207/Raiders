using System;
using System.IO;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x0200057F RID: 1407
	public class fiSettings
	{
		// Token: 0x06002152 RID: 8530 RVA: 0x00092F10 File Offset: 0x00091110
		static fiSettings()
		{
			foreach (fiSettingsProcessor fiSettingsProcessor in fiRuntimeReflectionUtility.GetAssemblyInstances<fiSettingsProcessor>())
			{
				fiSettingsProcessor.Process();
			}
			if (fiUtility.IsEditor)
			{
				fiSettings.EnsureRootDirectory();
			}
			if (fiSettings.RootGeneratedDirectory == null)
			{
				fiSettings.RootGeneratedDirectory = fiSettings.RootDirectory.TrimEnd(new char[] { '/' }) + "_Generated/";
			}
			if (fiUtility.IsEditor && !fiDirectory.Exists(fiSettings.RootGeneratedDirectory))
			{
				Debug.Log("Creating directory at " + fiSettings.RootGeneratedDirectory);
				fiDirectory.CreateDirectory(fiSettings.RootGeneratedDirectory);
			}
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0009303C File Offset: 0x0009123C
		private static void EnsureRootDirectory()
		{
			if (fiSettings.RootDirectory == null || !fiDirectory.Exists(fiSettings.RootDirectory))
			{
				Debug.Log("Failed to find FullInspector root directory at \"" + fiSettings.RootDirectory + "\"; running scan to find it.");
				string text = fiSettings.FindDirectoryPathByName("Assets", "FullInspector2");
				if (text == null)
				{
					Debug.LogError("Unable to locate \"FullInspector2\" directory. Please make sure that Full Inspector is located within \"FullInspector2\"");
				}
				else
				{
					text = text.Replace('\\', '/').TrimEnd(new char[] { '/' }) + '/';
					fiSettings.RootDirectory = text;
					Debug.Log("Found FullInspector at \"" + text + "\". Please add the following code to your project in a non-Editor folder:\n\n" + fiSettings.FormatCustomizerForNewPath(text));
				}
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000930E8 File Offset: 0x000912E8
		private static string FormatCustomizerForNewPath(string path)
		{
			return string.Concat(new string[]
			{
				"using FullInspector;",
				Environment.NewLine,
				Environment.NewLine,
				"public class UpdateFullInspectorRootDirectory : fiSettingsProcessor {",
				Environment.NewLine,
				"    public void Process() {",
				Environment.NewLine,
				"        fiSettings.RootDirectory = \"",
				path,
				"\";",
				Environment.NewLine,
				"    }",
				Environment.NewLine,
				"}",
				Environment.NewLine
			});
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0009317C File Offset: 0x0009137C
		private static string FindDirectoryPathByName(string currentDirectory, string targetDirectory)
		{
			targetDirectory = Path.GetFileName(targetDirectory);
			foreach (string text in fiDirectory.GetDirectories(currentDirectory))
			{
				if (Path.GetFileName(text) == targetDirectory)
				{
					return text;
				}
				string text2 = fiSettings.FindDirectoryPathByName(text, targetDirectory);
				if (text2 != null)
				{
					return text2;
				}
			}
			return null;
		}

		// Token: 0x04001801 RID: 6145
		public static bool PrettyPrintSerializedJson;

		// Token: 0x04001802 RID: 6146
		public static CommentType DefaultCommentType = CommentType.Info;

		// Token: 0x04001803 RID: 6147
		public static bool ForceDisplayInlineObjectEditor;

		// Token: 0x04001804 RID: 6148
		public static bool EnableAnimation = true;

		// Token: 0x04001805 RID: 6149
		public static bool ForceSaveAllAssetsOnSceneSave;

		// Token: 0x04001806 RID: 6150
		public static bool ForceSaveAllAssetsOnRecompilation;

		// Token: 0x04001807 RID: 6151
		public static bool ForceRestoreAllAssetsOnRecompilation;

		// Token: 0x04001808 RID: 6152
		public static bool AutomaticReferenceInstantation;

		// Token: 0x04001809 RID: 6153
		public static bool InspectorAutomaticReferenceInstantation = true;

		// Token: 0x0400180A RID: 6154
		public static bool InspectorRequireShowInInspector;

		// Token: 0x0400180B RID: 6155
		public static bool SerializeAutoProperties = true;

		// Token: 0x0400180C RID: 6156
		public static bool EmitWarnings;

		// Token: 0x0400180D RID: 6157
		public static bool EmitGraphMetadataCulls;

		// Token: 0x0400180E RID: 6158
		public static float MinimumFoldoutHeight = 80f;

		// Token: 0x0400180F RID: 6159
		public static bool EnableOpenScriptButton = true;

		// Token: 0x04001810 RID: 6160
		public static bool ForceDisableMultithreadedSerialization;

		// Token: 0x04001811 RID: 6161
		public static float LabelWidthPercentage = 0.45f;

		// Token: 0x04001812 RID: 6162
		public static float LabelWidthOffset = 30f;

		// Token: 0x04001813 RID: 6163
		public static float LabelWidthMax = 600f;

		// Token: 0x04001814 RID: 6164
		public static float LabelWidthMin;

		// Token: 0x04001815 RID: 6165
		public static int DefaultPageMinimumCollectionLength = 20;

		// Token: 0x04001816 RID: 6166
		public static string RootDirectory = "Assets/FullInspector2/";

		// Token: 0x04001817 RID: 6167
		public static string RootGeneratedDirectory;
	}
}
