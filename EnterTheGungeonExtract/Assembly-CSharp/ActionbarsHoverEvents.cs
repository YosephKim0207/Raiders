using System;
using UnityEngine;

// Token: 0x0200042A RID: 1066
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Hover Events")]
public class ActionbarsHoverEvents : MonoBehaviour
{
	// Token: 0x06001873 RID: 6259 RVA: 0x00073B94 File Offset: 0x00071D94
	public void Start()
	{
		this.actionBar = base.GetComponent<dfControl>();
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x00073BA4 File Offset: 0x00071DA4
	public void OnMouseHover(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (this.isTooltipVisible)
		{
			return;
		}
		bool flag = this.actionBar.Controls.Contains(mouseEvent.Source);
		if (flag)
		{
			this.target = mouseEvent.Source;
			if (this.target == this.lastTarget)
			{
				return;
			}
			this.lastTarget = this.target;
			this.isTooltipVisible = true;
			SpellSlot componentInChildren = this.target.GetComponentInChildren<SpellSlot>();
			if (string.IsNullOrEmpty(componentInChildren.Spell))
			{
				return;
			}
			SpellDefinition spellDefinition = SpellDefinition.FindByName(componentInChildren.Spell);
			if (spellDefinition == null)
			{
				return;
			}
			ActionbarsTooltip.Show(spellDefinition);
		}
		else
		{
			this.lastTarget = null;
		}
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x00073C54 File Offset: 0x00071E54
	public void OnMouseDown()
	{
		this.isTooltipVisible = false;
		ActionbarsTooltip.Hide();
		this.target = null;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x00073C6C File Offset: 0x00071E6C
	public void OnMouseLeave()
	{
		if (this.target == null)
		{
			return;
		}
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = (float)Screen.height - mousePosition.y;
		if (!this.target.GetScreenRect().Contains(mousePosition, true))
		{
			this.isTooltipVisible = false;
			ActionbarsTooltip.Hide();
			this.target = null;
		}
	}

	// Token: 0x04001366 RID: 4966
	private dfControl actionBar;

	// Token: 0x04001367 RID: 4967
	private dfControl lastTarget;

	// Token: 0x04001368 RID: 4968
	private dfControl target;

	// Token: 0x04001369 RID: 4969
	private bool isTooltipVisible;
}
