using System;
using UnityEngine;

// Token: 0x02000424 RID: 1060
[AddComponentMenu("Daikon Forge/Input/Debugging/Simulate Touch with Mouse")]
public class dfMouseTouchSourceComponent : dfTouchInputSourceComponent
{
	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06001854 RID: 6228 RVA: 0x000735A4 File Offset: 0x000717A4
	public override IDFTouchInputSource Source
	{
		get
		{
			if (this.editorOnly && !Application.isEditor)
			{
				return null;
			}
			if (this.source == null)
			{
				this.source = new dfMouseTouchInputSource();
			}
			return this.source;
		}
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x000735DC File Offset: 0x000717DC
	public void Start()
	{
		base.useGUILayout = false;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000735E8 File Offset: 0x000717E8
	public void OnGUI()
	{
		if (this.source != null)
		{
			this.source.MirrorAlt = !Event.current.control && !Event.current.shift;
			this.source.ParallelAlt = !this.source.MirrorAlt && Event.current.shift;
		}
	}

	// Token: 0x0400135A RID: 4954
	public bool editorOnly = true;

	// Token: 0x0400135B RID: 4955
	private dfMouseTouchInputSource source;
}
