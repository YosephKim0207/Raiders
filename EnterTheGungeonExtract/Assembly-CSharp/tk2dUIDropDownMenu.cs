using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C06 RID: 3078
[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownMenu")]
public class tk2dUIDropDownMenu : MonoBehaviour
{
	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06004179 RID: 16761 RVA: 0x00152E60 File Offset: 0x00151060
	// (set) Token: 0x0600417A RID: 16762 RVA: 0x00152E68 File Offset: 0x00151068
	public List<string> ItemList
	{
		get
		{
			return this.itemList;
		}
		set
		{
			this.itemList = value;
		}
	}

	// Token: 0x14000088 RID: 136
	// (add) Token: 0x0600417B RID: 16763 RVA: 0x00152E74 File Offset: 0x00151074
	// (remove) Token: 0x0600417C RID: 16764 RVA: 0x00152EAC File Offset: 0x001510AC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSelectedItemChange;

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x0600417D RID: 16765 RVA: 0x00152EE4 File Offset: 0x001510E4
	// (set) Token: 0x0600417E RID: 16766 RVA: 0x00152EEC File Offset: 0x001510EC
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = Mathf.Clamp(value, 0, this.ItemList.Count - 1);
			this.SetSelectedItem();
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x0600417F RID: 16767 RVA: 0x00152F10 File Offset: 0x00151110
	public string SelectedItem
	{
		get
		{
			if (this.index >= 0 && this.index < this.itemList.Count)
			{
				return this.itemList[this.index];
			}
			return string.Empty;
		}
	}

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06004180 RID: 16768 RVA: 0x00152F4C File Offset: 0x0015114C
	// (set) Token: 0x06004181 RID: 16769 RVA: 0x00152F6C File Offset: 0x0015116C
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.dropDownButton != null)
			{
				return this.dropDownButton.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.dropDownButton != null && this.dropDownButton.sendMessageTarget != value)
			{
				this.dropDownButton.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06004182 RID: 16770 RVA: 0x00152FA4 File Offset: 0x001511A4
	// (set) Token: 0x06004183 RID: 16771 RVA: 0x00152FAC File Offset: 0x001511AC
	public tk2dUILayout MenuLayoutItem
	{
		get
		{
			return this.menuLayoutItem;
		}
		set
		{
			this.menuLayoutItem = value;
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06004184 RID: 16772 RVA: 0x00152FB8 File Offset: 0x001511B8
	// (set) Token: 0x06004185 RID: 16773 RVA: 0x00152FC0 File Offset: 0x001511C0
	public tk2dUILayout TemplateLayoutItem
	{
		get
		{
			return this.templateLayoutItem;
		}
		set
		{
			this.templateLayoutItem = value;
		}
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x00152FCC File Offset: 0x001511CC
	private void Awake()
	{
		foreach (string text in this.startingItemList)
		{
			this.itemList.Add(text);
		}
		this.index = this.startingIndex;
		this.dropDownItemTemplate.gameObject.SetActive(false);
		this.UpdateList();
	}

	// Token: 0x06004187 RID: 16775 RVA: 0x00153028 File Offset: 0x00151228
	private void OnEnable()
	{
		this.dropDownButton.OnDown += this.ExpandButtonPressed;
	}

	// Token: 0x06004188 RID: 16776 RVA: 0x00153044 File Offset: 0x00151244
	private void OnDisable()
	{
		this.dropDownButton.OnDown -= this.ExpandButtonPressed;
	}

	// Token: 0x06004189 RID: 16777 RVA: 0x00153060 File Offset: 0x00151260
	public void UpdateList()
	{
		if (this.dropDownItems.Count > this.ItemList.Count)
		{
			for (int i = this.ItemList.Count; i < this.dropDownItems.Count; i++)
			{
				this.dropDownItems[i].gameObject.SetActive(false);
			}
		}
		while (this.dropDownItems.Count < this.ItemList.Count)
		{
			this.dropDownItems.Add(this.CreateAnotherDropDownItem());
		}
		for (int j = 0; j < this.ItemList.Count; j++)
		{
			tk2dUIDropDownItem tk2dUIDropDownItem = this.dropDownItems[j];
			Vector3 localPosition = tk2dUIDropDownItem.transform.localPosition;
			if (this.menuLayoutItem != null && this.templateLayoutItem != null)
			{
				localPosition.y = this.menuLayoutItem.bMin.y - (float)j * (this.templateLayoutItem.bMax.y - this.templateLayoutItem.bMin.y);
			}
			else
			{
				localPosition.y = -this.height - (float)j * tk2dUIDropDownItem.height;
			}
			tk2dUIDropDownItem.transform.localPosition = localPosition;
			if (tk2dUIDropDownItem.label != null)
			{
				tk2dUIDropDownItem.LabelText = this.itemList[j];
			}
			tk2dUIDropDownItem.Index = j;
		}
		this.SetSelectedItem();
	}

	// Token: 0x0600418A RID: 16778 RVA: 0x001531E4 File Offset: 0x001513E4
	public void SetSelectedItem()
	{
		if (this.index < 0 || this.index >= this.ItemList.Count)
		{
			this.index = 0;
		}
		if (this.index >= 0 && this.index < this.ItemList.Count)
		{
			this.selectedTextMesh.text = this.ItemList[this.index];
			this.selectedTextMesh.Commit();
		}
		else
		{
			this.selectedTextMesh.text = string.Empty;
			this.selectedTextMesh.Commit();
		}
		if (this.OnSelectedItemChange != null)
		{
			this.OnSelectedItemChange();
		}
		if (this.SendMessageTarget != null && this.SendMessageOnSelectedItemChangeMethodName.Length > 0)
		{
			this.SendMessageTarget.SendMessage(this.SendMessageOnSelectedItemChangeMethodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x0600418B RID: 16779 RVA: 0x001532D0 File Offset: 0x001514D0
	private tk2dUIDropDownItem CreateAnotherDropDownItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dropDownItemTemplate.gameObject);
		gameObject.name = "DropDownItem";
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = this.dropDownItemTemplate.transform.localPosition;
		gameObject.transform.localRotation = this.dropDownItemTemplate.transform.localRotation;
		gameObject.transform.localScale = this.dropDownItemTemplate.transform.localScale;
		tk2dUIDropDownItem component = gameObject.GetComponent<tk2dUIDropDownItem>();
		component.OnItemSelected += this.ItemSelected;
		tk2dUIUpDownHoverButton component2 = gameObject.GetComponent<tk2dUIUpDownHoverButton>();
		component.upDownHoverBtn = component2;
		component2.OnToggleOver += this.DropDownItemHoverBtnToggle;
		return component;
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x00153398 File Offset: 0x00151598
	private void ItemSelected(tk2dUIDropDownItem item)
	{
		if (this.isExpanded)
		{
			this.CollapseList();
		}
		this.Index = item.Index;
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x001533B8 File Offset: 0x001515B8
	private void ExpandButtonPressed()
	{
		if (this.isExpanded)
		{
			this.CollapseList();
		}
		else
		{
			this.ExpandList();
		}
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x001533D8 File Offset: 0x001515D8
	private void ExpandList()
	{
		this.isExpanded = true;
		int num = Mathf.Min(this.ItemList.Count, this.dropDownItems.Count);
		for (int i = 0; i < num; i++)
		{
			this.dropDownItems[i].gameObject.SetActive(true);
		}
		tk2dUIDropDownItem tk2dUIDropDownItem = this.dropDownItems[this.index];
		if (tk2dUIDropDownItem.upDownHoverBtn != null)
		{
			tk2dUIDropDownItem.upDownHoverBtn.IsOver = true;
		}
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x00153460 File Offset: 0x00151660
	private void CollapseList()
	{
		this.isExpanded = false;
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004190 RID: 16784 RVA: 0x001534C8 File Offset: 0x001516C8
	private void DropDownItemHoverBtnToggle(tk2dUIUpDownHoverButton upDownHoverButton)
	{
		if (upDownHoverButton.IsOver)
		{
			foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
			{
				if (tk2dUIDropDownItem.upDownHoverBtn != upDownHoverButton && tk2dUIDropDownItem.upDownHoverBtn != null)
				{
					tk2dUIDropDownItem.upDownHoverBtn.IsOver = false;
				}
			}
		}
	}

	// Token: 0x06004191 RID: 16785 RVA: 0x00153558 File Offset: 0x00151758
	private void OnDestroy()
	{
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.OnItemSelected -= this.ItemSelected;
			if (tk2dUIDropDownItem.upDownHoverBtn != null)
			{
				tk2dUIDropDownItem.upDownHoverBtn.OnToggleOver -= this.DropDownItemHoverBtnToggle;
			}
		}
	}

	// Token: 0x04003423 RID: 13347
	public tk2dUIItem dropDownButton;

	// Token: 0x04003424 RID: 13348
	public tk2dTextMesh selectedTextMesh;

	// Token: 0x04003425 RID: 13349
	[HideInInspector]
	public float height;

	// Token: 0x04003426 RID: 13350
	public tk2dUIDropDownItem dropDownItemTemplate;

	// Token: 0x04003427 RID: 13351
	[SerializeField]
	private string[] startingItemList;

	// Token: 0x04003428 RID: 13352
	[SerializeField]
	private int startingIndex;

	// Token: 0x04003429 RID: 13353
	private List<string> itemList = new List<string>();

	// Token: 0x0400342B RID: 13355
	public string SendMessageOnSelectedItemChangeMethodName = string.Empty;

	// Token: 0x0400342C RID: 13356
	private int index;

	// Token: 0x0400342D RID: 13357
	private List<tk2dUIDropDownItem> dropDownItems = new List<tk2dUIDropDownItem>();

	// Token: 0x0400342E RID: 13358
	private bool isExpanded;

	// Token: 0x0400342F RID: 13359
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout menuLayoutItem;

	// Token: 0x04003430 RID: 13360
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout templateLayoutItem;
}
