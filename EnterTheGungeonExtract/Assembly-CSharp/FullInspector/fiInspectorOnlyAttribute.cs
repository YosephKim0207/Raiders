using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x0200052E RID: 1326
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field)]
	public sealed class fiInspectorOnlyAttribute : PropertyAttribute
	{
	}
}
