using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200038A RID: 906
[AddComponentMenu("Daikon Forge/User Interface/Animation Clip")]
[Serializable]
public class dfAnimationClip : MonoBehaviour
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06000F83 RID: 3971 RVA: 0x000482BC File Offset: 0x000464BC
	// (set) Token: 0x06000F84 RID: 3972 RVA: 0x000482C4 File Offset: 0x000464C4
	public dfAtlas Atlas
	{
		get
		{
			return this.atlas;
		}
		set
		{
			this.atlas = value;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06000F85 RID: 3973 RVA: 0x000482D0 File Offset: 0x000464D0
	public List<string> Sprites
	{
		get
		{
			return this.sprites;
		}
	}

	// Token: 0x04000EC9 RID: 3785
	[SerializeField]
	private dfAtlas atlas;

	// Token: 0x04000ECA RID: 3786
	[SerializeField]
	private List<string> sprites = new List<string>();
}
