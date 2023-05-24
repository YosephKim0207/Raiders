using System;
using UnityEngine;

// Token: 0x0200048F RID: 1167
[AddComponentMenu("Daikon Forge/Input/Debugging/Touch Visualizer")]
public class DEMO_TouchVisualizer : MonoBehaviour
{
	// Token: 0x06001AD2 RID: 6866 RVA: 0x0007D72C File Offset: 0x0007B92C
	private void Awake()
	{
		base.useGUILayout = false;
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x0007D738 File Offset: 0x0007B938
	public void OnGUI()
	{
		if (this.editorOnly && !Application.isEditor)
		{
			return;
		}
		if (this.input == null)
		{
			dfInputManager component = base.GetComponent<dfInputManager>();
			if (component == null)
			{
				Debug.LogError("No dfInputManager instance found", this);
				base.enabled = false;
				return;
			}
			if (!component.UseTouch)
			{
				if (Application.isPlaying)
				{
					base.enabled = false;
				}
				return;
			}
			this.input = component.TouchInputSource;
			if (this.input == null)
			{
				Debug.LogError("No dfTouchInputSource component found", this);
				base.enabled = false;
				return;
			}
		}
		if (this.showPlatformInfo)
		{
			Rect rect = new Rect(5f, 0f, 800f, 25f);
			GUI.Label(rect, string.Concat(new object[]
			{
				"Touch Source: ",
				this.input,
				", Platform: ",
				Application.platform
			}));
		}
		if (this.showMouse && !Application.isEditor)
		{
			this.drawTouchIcon(Input.mousePosition);
		}
		int touchCount = this.input.TouchCount;
		for (int i = 0; i < touchCount; i++)
		{
			this.drawTouchIcon(this.input.GetTouch(i).position);
		}
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x0007D894 File Offset: 0x0007BA94
	private void drawTouchIcon(Vector3 pos)
	{
		int height = Screen.height;
		pos.y = (float)height - pos.y;
		Rect rect = new Rect(pos.x - (float)(this.iconSize / 2), pos.y - (float)(this.iconSize / 2), (float)this.iconSize, (float)this.iconSize);
		GUI.DrawTexture(rect, this.touchIcon);
	}

	// Token: 0x04001527 RID: 5415
	public bool editorOnly;

	// Token: 0x04001528 RID: 5416
	public bool showMouse;

	// Token: 0x04001529 RID: 5417
	public bool showPlatformInfo;

	// Token: 0x0400152A RID: 5418
	public int iconSize = 32;

	// Token: 0x0400152B RID: 5419
	public Texture2D touchIcon;

	// Token: 0x0400152C RID: 5420
	private IDFTouchInputSource input;
}
