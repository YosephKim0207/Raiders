using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001784 RID: 6020
public class GameUIHeartController : MonoBehaviour, ILevelLoadedListener
{
	// Token: 0x17001500 RID: 5376
	// (get) Token: 0x06008C61 RID: 35937 RVA: 0x003AAE1C File Offset: 0x003A901C
	public dfPanel Panel
	{
		get
		{
			return this.m_panel;
		}
	}

	// Token: 0x06008C62 RID: 35938 RVA: 0x003AAE24 File Offset: 0x003A9024
	private void Awake()
	{
		this.m_currentFullHeartName = this.fullHeartSpriteName;
		this.m_currentHalfHeartName = this.halfHeartSpriteName;
		this.m_currentEmptyHeartName = this.emptyHeartSpriteName;
		this.m_currentArmorName = this.armorSpritePrefab.SpriteName;
		this.m_panel = base.GetComponent<dfPanel>();
		this.extantHearts = new List<dfSprite>();
		this.extantArmors = new List<dfSprite>();
	}

	// Token: 0x06008C63 RID: 35939 RVA: 0x003AAE88 File Offset: 0x003A9088
	private void Start()
	{
		Collider[] components = base.GetComponents<Collider>();
		for (int i = 0; i < components.Length; i++)
		{
			UnityEngine.Object.Destroy(components[i]);
		}
	}

	// Token: 0x06008C64 RID: 35940 RVA: 0x003AAEB8 File Offset: 0x003A90B8
	public void BraveOnLevelWasLoaded()
	{
		if (this.extantHearts != null)
		{
			for (int i = 0; i < this.extantHearts.Count; i++)
			{
				if (!this.extantHearts[i])
				{
					this.extantHearts.RemoveAt(i);
					i--;
				}
			}
		}
		if (this.extantArmors != null)
		{
			for (int j = 0; j < this.extantArmors.Count; j++)
			{
				if (!this.extantArmors[j])
				{
					this.extantArmors.RemoveAt(j);
					j--;
				}
			}
		}
	}

	// Token: 0x06008C65 RID: 35941 RVA: 0x003AAF60 File Offset: 0x003A9160
	public void UpdateScale()
	{
		for (int i = 0; i < this.extantHearts.Count; i++)
		{
			dfSprite dfSprite = this.extantHearts[i];
			if (dfSprite)
			{
				Vector2 sizeInPixels = dfSprite.SpriteInfo.sizeInPixels;
				dfSprite.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
			}
		}
		for (int j = 0; j < this.extantArmors.Count; j++)
		{
			dfSprite dfSprite2 = this.extantArmors[j];
			if (dfSprite2)
			{
				Vector2 sizeInPixels2 = dfSprite2.SpriteInfo.sizeInPixels;
				dfSprite2.Size = sizeInPixels2 * Pixelator.Instance.CurrentTileScale;
			}
		}
	}

	// Token: 0x06008C66 RID: 35942 RVA: 0x003AB02C File Offset: 0x003A922C
	public void AddArmor()
	{
		Vector3 position = base.transform.position;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.armorSpritePrefab.gameObject, position, Quaternion.identity);
		gameObject.transform.parent = base.transform.parent;
		gameObject.layer = base.gameObject.layer;
		dfSprite component = gameObject.GetComponent<dfSprite>();
		if (this.IsRightAligned)
		{
			component.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Right;
		}
		Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
		component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
		component.IsInteractive = false;
		if (!this.IsRightAligned)
		{
			float num = ((this.extantHearts.Count <= 0) ? 0f : ((this.extantHearts[0].Width + Pixelator.Instance.CurrentTileScale) * (float)this.extantHearts.Count));
			float num2 = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)this.extantArmors.Count;
			component.RelativePosition = this.m_panel.RelativePosition + new Vector3(num + num2, 0f, 0f);
		}
		else
		{
			component.RelativePosition = this.m_panel.RelativePosition - new Vector3(component.Width, 0f, 0f);
			for (int i = 0; i < this.extantArmors.Count; i++)
			{
				dfSprite dfSprite = this.extantArmors[i];
				if (dfSprite)
				{
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, true, false, true);
					dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
					GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
				}
			}
			for (int j = 0; j < this.extantHearts.Count; j++)
			{
				dfSprite dfSprite2 = this.extantHearts[j];
				if (dfSprite2)
				{
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, true, false, true);
					dfSprite2.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
					GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
				}
			}
		}
		this.extantArmors.Add(component);
		GameUIRoot.Instance.AddControlToMotionGroups(component, (!this.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
	}

	// Token: 0x06008C67 RID: 35943 RVA: 0x003AB314 File Offset: 0x003A9514
	public void RemoveArmor()
	{
		if (this.extantArmors.Count > 0)
		{
			dfSprite dfSprite = this.damagedArmorAnimationPrefab;
			dfSprite dfSprite2 = this.extantArmors[this.extantArmors.Count - 1];
			if (dfSprite2)
			{
				if (dfSprite2.SpriteName == this.crestSpritePrefab.SpriteName)
				{
					dfSprite = this.damagedCrestAnimationPrefab;
				}
				if (dfSprite != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(dfSprite.gameObject);
					gameObject.transform.parent = base.transform.parent;
					gameObject.layer = base.gameObject.layer;
					dfSprite component = gameObject.GetComponent<dfSprite>();
					component.BringToFront();
					dfSprite2.Parent.AddControl(component);
					dfSprite2.Parent.BringToFront();
					component.ZOrder = dfSprite2.ZOrder - 1;
					component.RelativePosition = dfSprite2.RelativePosition + this.damagedArmorPrefabOffset;
					this.m_panel.AddControl(component);
				}
			}
			float width = this.extantArmors[0].Width;
			if (dfSprite2)
			{
				GameUIRoot.Instance.RemoveControlFromMotionGroups(dfSprite2);
				UnityEngine.Object.Destroy(this.extantArmors[this.extantArmors.Count - 1]);
			}
			this.extantArmors.RemoveAt(this.extantArmors.Count - 1);
			if (this.IsRightAligned)
			{
				for (int i = 0; i < this.extantArmors.Count; i++)
				{
					dfSprite dfSprite3 = this.extantArmors[i];
					if (dfSprite3)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, true, false, true);
						dfSprite3.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite3);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
				for (int j = 0; j < this.extantHearts.Count; j++)
				{
					dfSprite dfSprite4 = this.extantHearts[j];
					if (dfSprite4)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite4, true, false, true);
						dfSprite4.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite4);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite4, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
			}
		}
	}

	// Token: 0x06008C68 RID: 35944 RVA: 0x003AB5C0 File Offset: 0x003A97C0
	private void ClearAllArmor()
	{
		if (this.extantArmors.Count > 0)
		{
			while (this.extantArmors.Count > 0)
			{
				this.RemoveArmor();
			}
		}
	}

	// Token: 0x06008C69 RID: 35945 RVA: 0x003AB5F0 File Offset: 0x003A97F0
	public dfSprite AddHeart()
	{
		int count = this.extantArmors.Count;
		this.ClearAllArmor();
		Vector3 position = base.transform.position;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.heartSpritePrefab.gameObject, position, Quaternion.identity);
		gameObject.transform.parent = base.transform.parent;
		gameObject.layer = base.gameObject.layer;
		dfSprite component = gameObject.GetComponent<dfSprite>();
		if (this.IsRightAligned)
		{
			component.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Right;
		}
		Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
		component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
		component.IsInteractive = false;
		if (!this.IsRightAligned)
		{
			float num = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)this.extantHearts.Count;
			component.RelativePosition = this.m_panel.RelativePosition + new Vector3(num, 0f, 0f);
		}
		else
		{
			component.RelativePosition = this.m_panel.RelativePosition - new Vector3(component.Width, 0f, 0f);
			for (int i = 0; i < this.extantHearts.Count; i++)
			{
				dfSprite dfSprite = this.extantHearts[i];
				if (dfSprite)
				{
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, true, false, true);
					dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
					GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
				}
			}
			for (int j = 0; j < this.extantArmors.Count; j++)
			{
				dfSprite dfSprite2 = this.extantArmors[j];
				if (dfSprite2)
				{
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, true, false, true);
					dfSprite2.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
					GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
					GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
				}
			}
		}
		this.extantHearts.Add(component);
		GameUIRoot.Instance.AddControlToMotionGroups(component, (!this.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
		for (int k = 0; k < count; k++)
		{
			this.AddArmor();
		}
		return component;
	}

	// Token: 0x06008C6A RID: 35946 RVA: 0x003AB8C0 File Offset: 0x003A9AC0
	public void RemoveHeart()
	{
		if (this.extantHearts.Count > 0)
		{
			float width = this.extantHearts[0].Width;
			dfSprite dfSprite = this.extantHearts[this.extantHearts.Count - 1];
			if (dfSprite)
			{
				GameUIRoot.Instance.RemoveControlFromMotionGroups(dfSprite);
				UnityEngine.Object.Destroy(dfSprite);
			}
			this.extantHearts.RemoveAt(this.extantHearts.Count - 1);
			if (this.IsRightAligned)
			{
				for (int i = 0; i < this.extantHearts.Count; i++)
				{
					dfSprite dfSprite2 = this.extantHearts[i];
					if (dfSprite2)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, true, false, true);
						dfSprite2.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
			}
			else if (this.extantArmors != null && this.extantArmors.Count > 0 && this.extantHearts.Count > 0)
			{
				for (int j = 0; j < this.extantArmors.Count; j++)
				{
					float num = this.extantHearts[0].Size.x + Pixelator.Instance.CurrentTileScale;
					dfSprite dfSprite3 = this.extantArmors[j];
					if (dfSprite3)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, true, false, true);
						dfSprite3.RelativePosition -= new Vector3(num, 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite3);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
			}
			this.ClearAllArmor();
		}
	}

	// Token: 0x06008C6B RID: 35947 RVA: 0x003ABAD4 File Offset: 0x003A9CD4
	public void UpdateHealth(HealthHaver hh)
	{
		float num = hh.GetCurrentHealth();
		float maxHealth = hh.GetMaxHealth();
		float num2 = hh.Armor;
		if (hh.NextShotKills)
		{
			num = 0.5f;
			num2 = 0f;
		}
		int num3 = Mathf.CeilToInt(maxHealth);
		int num4 = Mathf.CeilToInt(num2);
		if (this.extantHearts.Count < num3)
		{
			for (int i = this.extantHearts.Count; i < num3; i++)
			{
				dfSprite dfSprite = this.AddHeart();
				if ((float)(i + 1) > num)
				{
					if ((float)Mathf.FloorToInt(num) != num && num + 1f > (float)(i + 1))
					{
						dfSprite.SpriteName = this.m_currentHalfHeartName;
					}
					else
					{
						dfSprite.SpriteName = this.m_currentEmptyHeartName;
					}
				}
			}
		}
		else if (this.extantHearts.Count > num3)
		{
			while (this.extantHearts.Count > num3)
			{
				this.RemoveHeart();
			}
		}
		if (this.extantArmors.Count < num4)
		{
			for (int j = this.extantArmors.Count; j < num4; j++)
			{
				this.AddArmor();
			}
		}
		else if (this.extantArmors.Count > num4)
		{
			while (this.extantArmors.Count > num4)
			{
				this.RemoveArmor();
			}
		}
		int num5 = Mathf.FloorToInt(num);
		for (int k = 0; k < this.extantHearts.Count; k++)
		{
			dfSprite dfSprite2 = this.extantHearts[k];
			if (dfSprite2)
			{
				if (k < num5)
				{
					dfSprite2.SpriteName = this.m_currentFullHeartName;
				}
				else if (k != num5 || num - (float)num5 <= 0f)
				{
					if (dfSprite2.SpriteName == this.m_currentFullHeartName || dfSprite2.SpriteName == this.m_currentHalfHeartName)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.damagedHeartAnimationPrefab.gameObject);
						gameObject.transform.parent = base.transform.parent;
						gameObject.layer = base.gameObject.layer;
						dfSprite component = gameObject.GetComponent<dfSprite>();
						component.BringToFront();
						dfSprite2.Parent.AddControl(component);
						dfSprite2.Parent.BringToFront();
						component.ZOrder = dfSprite2.ZOrder - 1;
						component.RelativePosition = dfSprite2.RelativePosition + this.damagedPrefabOffset;
					}
					dfSprite2.SpriteName = this.m_currentEmptyHeartName;
				}
			}
		}
		if (num - (float)num5 > 0f && this.extantHearts != null && this.extantHearts.Count > 0)
		{
			dfSprite dfSprite3 = this.extantHearts[num5];
			if (dfSprite3)
			{
				if (dfSprite3.SpriteName == this.m_currentFullHeartName)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.damagedHeartAnimationPrefab.gameObject);
					gameObject2.transform.parent = base.transform.parent;
					gameObject2.layer = base.gameObject.layer;
					dfSprite component2 = gameObject2.GetComponent<dfSprite>();
					component2.BringToFront();
					dfSprite3.Parent.AddControl(component2);
					dfSprite3.Parent.BringToFront();
					component2.ZOrder = dfSprite3.ZOrder - 1;
					component2.RelativePosition = dfSprite3.RelativePosition + this.damagedPrefabOffset;
				}
				dfSprite3.SpriteName = this.m_currentHalfHeartName;
			}
		}
		PlayerController playerController = hh.gameActor as PlayerController;
		this.ProcessHeartSpriteModifications(playerController);
		if (hh.HasCrest && num2 > 0f)
		{
			for (int l = 0; l < this.extantArmors.Count; l++)
			{
				dfSprite dfSprite4 = this.extantArmors[l];
				if (dfSprite4)
				{
					if (l < this.extantArmors.Count - 1)
					{
						if (dfSprite4.SpriteName != this.m_currentArmorName)
						{
							dfSprite4.SpriteName = this.m_currentArmorName;
							dfSprite4.Color = this.armorSpritePrefab.Color;
							dfPanel motionGroupParent = GameUIRoot.Instance.GetMotionGroupParent(dfSprite4);
							motionGroupParent.Width -= Pixelator.Instance.CurrentTileScale;
							motionGroupParent.Height -= Pixelator.Instance.CurrentTileScale;
							dfSprite4.RelativePosition = dfSprite4.RelativePosition.WithY(0f);
						}
					}
					else if (dfSprite4.SpriteName != this.crestSpritePrefab.SpriteName)
					{
						dfSprite4.SpriteName = this.crestSpritePrefab.SpriteName;
						dfSprite4.Color = this.crestSpritePrefab.Color;
						dfPanel motionGroupParent2 = GameUIRoot.Instance.GetMotionGroupParent(dfSprite4);
						motionGroupParent2.Width += Pixelator.Instance.CurrentTileScale;
						motionGroupParent2.Height += Pixelator.Instance.CurrentTileScale;
						dfSprite4.RelativePosition = dfSprite4.RelativePosition.WithY(Pixelator.Instance.CurrentTileScale);
					}
				}
			}
		}
		else
		{
			for (int m = 0; m < this.extantArmors.Count; m++)
			{
				dfSprite dfSprite5 = this.extantArmors[m];
				if (dfSprite5)
				{
					if (dfSprite5.SpriteName != this.m_currentArmorName)
					{
						dfSprite5.SpriteName = this.m_currentArmorName;
						dfPanel motionGroupParent3 = GameUIRoot.Instance.GetMotionGroupParent(dfSprite5);
						motionGroupParent3.Width -= Pixelator.Instance.CurrentTileScale;
						motionGroupParent3.Height -= Pixelator.Instance.CurrentTileScale;
						dfSprite5.RelativePosition = dfSprite5.RelativePosition.WithY(0f);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite5, true, false, true);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite5);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite5, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
					dfSprite5.Color = this.armorSpritePrefab.Color;
					dfSprite5.RelativePosition = dfSprite5.RelativePosition.WithY(0f);
				}
			}
		}
		for (int n = 0; n < this.extantHearts.Count; n++)
		{
			dfSprite dfSprite6 = this.extantHearts[n];
			if (dfSprite6)
			{
				dfSprite6.Size = dfSprite6.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
			}
		}
		for (int num6 = 0; num6 < this.extantArmors.Count; num6++)
		{
			dfSprite dfSprite7 = this.extantArmors[num6];
			if (dfSprite7)
			{
				dfSprite7.Size = dfSprite7.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
			}
		}
	}

	// Token: 0x06008C6C RID: 35948 RVA: 0x003AC200 File Offset: 0x003AA400
	private void ProcessHeartSpriteModifications(PlayerController associatedPlayer)
	{
		bool flag = false;
		if (associatedPlayer)
		{
			if (associatedPlayer.HealthAndArmorSwapped)
			{
				this.m_currentFullHeartName = "heart_shield_full_001";
				this.m_currentHalfHeartName = "heart_shield_half_001";
				this.m_currentEmptyHeartName = "heart_shield_empty_001";
				this.m_currentArmorName = "armor_shield_heart_idle_001";
				flag = true;
			}
			else if (associatedPlayer.CurrentGun)
			{
				bool isUndertaleGun = associatedPlayer.CurrentGun.IsUndertaleGun;
				if (isUndertaleGun)
				{
					this.m_currentFullHeartName = "heart_full_yellow_001";
					this.m_currentHalfHeartName = "heart_half_yellow_001";
					flag = true;
				}
			}
		}
		if (!flag)
		{
			this.m_currentFullHeartName = this.fullHeartSpriteName;
			this.m_currentHalfHeartName = this.halfHeartSpriteName;
			this.m_currentEmptyHeartName = this.emptyHeartSpriteName;
			this.m_currentArmorName = this.armorSpritePrefab.SpriteName;
		}
	}

	// Token: 0x040093AF RID: 37807
	public dfSprite heartSpritePrefab;

	// Token: 0x040093B0 RID: 37808
	public dfSprite damagedHeartAnimationPrefab;

	// Token: 0x040093B1 RID: 37809
	public Vector3 damagedPrefabOffset;

	// Token: 0x040093B2 RID: 37810
	public dfSprite armorSpritePrefab;

	// Token: 0x040093B3 RID: 37811
	public dfSprite damagedArmorAnimationPrefab;

	// Token: 0x040093B4 RID: 37812
	public Vector3 damagedArmorPrefabOffset;

	// Token: 0x040093B5 RID: 37813
	public dfSprite crestSpritePrefab;

	// Token: 0x040093B6 RID: 37814
	public dfSprite damagedCrestAnimationPrefab;

	// Token: 0x040093B7 RID: 37815
	public List<dfSprite> extantHearts;

	// Token: 0x040093B8 RID: 37816
	public List<dfSprite> extantArmors;

	// Token: 0x040093B9 RID: 37817
	public string fullHeartSpriteName;

	// Token: 0x040093BA RID: 37818
	public string halfHeartSpriteName;

	// Token: 0x040093BB RID: 37819
	public string emptyHeartSpriteName;

	// Token: 0x040093BC RID: 37820
	public bool IsRightAligned;

	// Token: 0x040093BD RID: 37821
	private dfPanel m_panel;

	// Token: 0x040093BE RID: 37822
	private string m_currentFullHeartName;

	// Token: 0x040093BF RID: 37823
	private string m_currentHalfHeartName;

	// Token: 0x040093C0 RID: 37824
	private string m_currentEmptyHeartName;

	// Token: 0x040093C1 RID: 37825
	private string m_currentArmorName;
}
