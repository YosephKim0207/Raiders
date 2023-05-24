using System;
using System.Collections.Generic;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector.BackupService
{
	// Token: 0x020005F3 RID: 1523
	[AddComponentMenu("")]
	public class fiStorageComponent : MonoBehaviour, fiIEditorOnlyTag
	{
		// Token: 0x060023CF RID: 9167 RVA: 0x0009CCE8 File Offset: 0x0009AEE8
		public void RemoveInvalidBackups()
		{
			int i = 0;
			while (i < this.Objects.Count)
			{
				if (this.Objects[i].Target.Target == null)
				{
					this.Objects.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x040018EE RID: 6382
		public List<fiSerializedObject> Objects = new List<fiSerializedObject>();
	}
}
