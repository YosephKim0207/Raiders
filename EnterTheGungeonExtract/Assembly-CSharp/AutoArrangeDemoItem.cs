using System;
using UnityEngine;

// Token: 0x02000446 RID: 1094
[AddComponentMenu("Daikon Forge/Examples/Containers/Auto-Arrange Item")]
public class AutoArrangeDemoItem : MonoBehaviour
{
	// Token: 0x06001919 RID: 6425 RVA: 0x00076108 File Offset: 0x00074308
	private void Start()
	{
		this.control = base.GetComponent<dfButton>();
		this.size = new dfAnimatedVector2(this.control.Size, this.control.Size, 0.33f);
		this.control.Text = "#" + (this.control.ZOrder + 1);
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x00076170 File Offset: 0x00074370
	private void Update()
	{
		this.control.Size = this.size.Value.RoundToInt();
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x00076190 File Offset: 0x00074390
	private void OnClick()
	{
		this.Toggle();
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x00076198 File Offset: 0x00074398
	public void Expand()
	{
		this.size.StartValue = this.size.EndValue;
		this.size.EndValue = new Vector2(128f, 96f);
		this.isExpanded = true;
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x000761D4 File Offset: 0x000743D4
	public void Collapse()
	{
		this.size.StartValue = this.size.EndValue;
		this.size.EndValue = new Vector2(48f, 48f);
		this.isExpanded = false;
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x00076210 File Offset: 0x00074410
	public void Toggle()
	{
		if (this.isExpanded)
		{
			this.Collapse();
		}
		else
		{
			this.Expand();
		}
	}

	// Token: 0x040013C8 RID: 5064
	private dfButton control;

	// Token: 0x040013C9 RID: 5065
	private dfAnimatedVector2 size;

	// Token: 0x040013CA RID: 5066
	private bool isExpanded;
}
