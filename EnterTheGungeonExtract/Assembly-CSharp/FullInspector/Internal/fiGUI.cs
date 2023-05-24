using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x0200055C RID: 1372
	public static class fiGUI
	{
		// Token: 0x060020A6 RID: 8358 RVA: 0x00090AEC File Offset: 0x0008ECEC
		public static float PushLabelWidth(GUIContent controlLabel, float controlWidth)
		{
			fiGUI.s_regionWidths.Add(controlWidth);
			fiGUI.s_savedLabelWidths.Push(controlWidth);
			return fiGUI.ComputeActualLabelWidth(fiGUI.s_regionWidths[0], controlLabel, controlWidth);
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00090B18 File Offset: 0x0008ED18
		public static float PopLabelWidth()
		{
			fiGUI.s_regionWidths.RemoveAt(fiGUI.s_regionWidths.Count - 1);
			return fiGUI.s_savedLabelWidths.Pop();
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00090B3C File Offset: 0x0008ED3C
		public static float ComputeActualLabelWidth(float inspectorWidth, GUIContent controlLabel, float controlWidth)
		{
			float num = inspectorWidth - controlWidth;
			float num2 = Mathf.Max(inspectorWidth * fiSettings.LabelWidthPercentage - fiSettings.LabelWidthOffset, 120f);
			float num3 = num2 - num;
			float num4 = Mathf.Max(fiLateBindings.EditorStyles.label.CalcSize(controlLabel).x, fiSettings.LabelWidthMin);
			return Mathf.Clamp(num3, num4, fiSettings.LabelWidthMax);
		}

		// Token: 0x040017B1 RID: 6065
		private static readonly List<float> s_regionWidths = new List<float>();

		// Token: 0x040017B2 RID: 6066
		private static readonly Stack<float> s_savedLabelWidths = new Stack<float>();
	}
}
