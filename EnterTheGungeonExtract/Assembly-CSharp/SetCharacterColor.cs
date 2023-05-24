using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
[AddComponentMenu("Daikon Forge/Examples/Color Picker/Set Character Color")]
public class SetCharacterColor : MonoBehaviour
{
	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06001916 RID: 6422 RVA: 0x000760D0 File Offset: 0x000742D0
	// (set) Token: 0x06001917 RID: 6423 RVA: 0x000760E8 File Offset: 0x000742E8
	public Color BeltColor
	{
		get
		{
			return this.CharacterRenderer.material.GetColor("_TeamColor");
		}
		set
		{
			this.CharacterRenderer.material.SetColor("_TeamColor", value);
		}
	}

	// Token: 0x040013C7 RID: 5063
	public SkinnedMeshRenderer CharacterRenderer;
}
