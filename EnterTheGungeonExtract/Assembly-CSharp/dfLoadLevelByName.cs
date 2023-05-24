using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000468 RID: 1128
[AddComponentMenu("Daikon Forge/Examples/General/Load Level On Click")]
[Serializable]
public class dfLoadLevelByName : MonoBehaviour
{
	// Token: 0x06001A15 RID: 6677 RVA: 0x00079CA4 File Offset: 0x00077EA4
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.LevelName))
		{
			SceneManager.LoadScene(this.LevelName);
		}
	}

	// Token: 0x04001476 RID: 5238
	public string LevelName;
}
