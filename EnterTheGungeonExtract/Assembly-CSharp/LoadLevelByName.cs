using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000470 RID: 1136
[AddComponentMenu("Daikon Forge/Examples/General/Load Level On Click")]
[Serializable]
public class LoadLevelByName : MonoBehaviour
{
	// Token: 0x06001A2F RID: 6703 RVA: 0x0007A424 File Offset: 0x00078624
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.LevelName))
		{
			SceneManager.LoadScene(this.LevelName);
		}
	}

	// Token: 0x0400148A RID: 5258
	public string LevelName;
}
