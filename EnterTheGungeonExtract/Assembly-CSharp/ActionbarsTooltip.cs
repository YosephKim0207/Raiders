using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Tooltip")]
public class ActionbarsTooltip : MonoBehaviour
{
	// Token: 0x06001878 RID: 6264 RVA: 0x00073CDC File Offset: 0x00071EDC
	public void Start()
	{
		ActionbarsTooltip._instance = this;
		ActionbarsTooltip._panel = base.GetComponent<dfPanel>();
		ActionbarsTooltip._name = ActionbarsTooltip._panel.Find<dfLabel>("lblName");
		ActionbarsTooltip._info = ActionbarsTooltip._panel.Find<dfLabel>("lblInfo");
		ActionbarsTooltip._panel.Hide();
		ActionbarsTooltip._panel.IsInteractive = false;
		ActionbarsTooltip._panel.IsEnabled = false;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x00073D44 File Offset: 0x00071F44
	public void Update()
	{
		if (ActionbarsTooltip._panel.IsVisible)
		{
			ActionbarsTooltip.setPosition(Input.mousePosition);
		}
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x00073D64 File Offset: 0x00071F64
	public static void Show(SpellDefinition spell)
	{
		if (spell == null)
		{
			ActionbarsTooltip.Hide();
			return;
		}
		ActionbarsTooltip._name.Text = spell.Name;
		ActionbarsTooltip._info.Text = spell.Description;
		float num = ActionbarsTooltip._info.RelativePosition.y + ActionbarsTooltip._info.Size.y;
		ActionbarsTooltip._panel.Height = num;
		ActionbarsTooltip._cursorOffset = new Vector2(0f, num + 10f);
		ActionbarsTooltip._panel.Show();
		ActionbarsTooltip._panel.BringToFront();
		ActionbarsTooltip._instance.Update();
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x00073E04 File Offset: 0x00072004
	public static void Hide()
	{
		ActionbarsTooltip._panel.Hide();
		ActionbarsTooltip._panel.SendToBack();
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x00073E1C File Offset: 0x0007201C
	private static void setPosition(Vector2 position)
	{
		position = ActionbarsTooltip._panel.GetManager().ScreenToGui(position);
		ActionbarsTooltip._panel.RelativePosition = position - ActionbarsTooltip._cursorOffset;
	}

	// Token: 0x0400136A RID: 4970
	private static ActionbarsTooltip _instance;

	// Token: 0x0400136B RID: 4971
	private static dfPanel _panel;

	// Token: 0x0400136C RID: 4972
	private static dfLabel _name;

	// Token: 0x0400136D RID: 4973
	private static dfLabel _info;

	// Token: 0x0400136E RID: 4974
	private static Vector2 _cursorOffset;
}
