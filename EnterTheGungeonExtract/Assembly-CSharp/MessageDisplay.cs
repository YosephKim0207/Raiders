using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000430 RID: 1072
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Message Scroller")]
public class MessageDisplay : MonoBehaviour
{
	// Token: 0x06001894 RID: 6292 RVA: 0x000741A4 File Offset: 0x000723A4
	public void AddMessage(string text)
	{
		if (this.lblTemplate == null)
		{
			return;
		}
		for (int i = 0; i < this.messages.Count; i++)
		{
			dfLabel label = this.messages[i].label;
			label.RelativePosition += new Vector3(0f, -label.Height);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lblTemplate.gameObject);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = this.lblTemplate.transform.position;
		gameObject.name = "Message" + this.messages.Count;
		dfLabel component = gameObject.GetComponent<dfLabel>();
		component.Text = text;
		component.IsVisible = true;
		this.messages.Add(new MessageDisplay.MessageInfo
		{
			label = component,
			startTime = Time.realtimeSinceStartup
		});
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x000742B0 File Offset: 0x000724B0
	public void onSpellActivated(SpellDefinition spell)
	{
		this.AddMessage("You cast " + spell.Name);
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000742C8 File Offset: 0x000724C8
	private void OnClick(dfControl sender, dfMouseEventArgs args)
	{
		this.AddMessage("New test message added to the list at " + DateTime.Now);
		args.Use();
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x000742EC File Offset: 0x000724EC
	private void OnEnable()
	{
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x000742F0 File Offset: 0x000724F0
	private void Start()
	{
		this.lblTemplate = base.GetComponentInChildren<dfLabel>();
		this.lblTemplate.IsVisible = false;
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x0007430C File Offset: 0x0007250C
	private void Update()
	{
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			MessageDisplay.MessageInfo messageInfo = this.messages[i];
			float num = Time.realtimeSinceStartup - messageInfo.startTime;
			if (num >= 3f)
			{
				if (num >= 5f)
				{
					this.messages.RemoveAt(i);
					UnityEngine.Object.Destroy(messageInfo.label.gameObject);
				}
				else
				{
					float num2 = 1f - (num - 3f) / 2f;
					messageInfo.label.Opacity = num2;
				}
			}
		}
	}

	// Token: 0x0400137B RID: 4987
	private const float TIME_BEFORE_FADE = 3f;

	// Token: 0x0400137C RID: 4988
	private const float FADE_TIME = 2f;

	// Token: 0x0400137D RID: 4989
	private List<MessageDisplay.MessageInfo> messages = new List<MessageDisplay.MessageInfo>();

	// Token: 0x0400137E RID: 4990
	private dfLabel lblTemplate;

	// Token: 0x02000431 RID: 1073
	private class MessageInfo
	{
		// Token: 0x0400137F RID: 4991
		public dfLabel label;

		// Token: 0x04001380 RID: 4992
		public float startTime;
	}
}
