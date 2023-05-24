using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x0200054C RID: 1356
	public static class fiCommentUtility
	{
		// Token: 0x06002038 RID: 8248 RVA: 0x0008F434 File Offset: 0x0008D634
		public static int GetCommentHeight(string comment, CommentType commentType)
		{
			int num = 38;
			if (commentType == CommentType.None)
			{
				num = 17;
			}
			GUIStyle guistyle = "HelpBox";
			return Math.Max((int)guistyle.CalcHeight(new GUIContent(comment), (float)Screen.width), num);
		}
	}
}
