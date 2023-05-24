using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C67 RID: 3175
	[Tooltip("Handles light intensity for Brave lights.")]
	[ActionCategory(".Brave")]
	public class SetBraveLightIntensity : FsmStateAction
	{
		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06004443 RID: 17475 RVA: 0x00160C44 File Offset: 0x0015EE44
		// (set) Token: 0x06004444 RID: 17476 RVA: 0x00160C4C File Offset: 0x0015EE4C
		public bool IsKeptAction { get; set; }

		// Token: 0x06004445 RID: 17477 RVA: 0x00160C58 File Offset: 0x0015EE58
		public override void Reset()
		{
			this.specifyLights = new ShadowSystem[0];
			this.intensity = 1f;
			this.transitionTime = 0f;
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x00160C88 File Offset: 0x0015EE88
		public override void OnEnter()
		{
			if (this.specifyLights.Length == 0)
			{
				this.specifyLights = base.Owner.gameObject.GetComponentsInChildren<ShadowSystem>();
				if (this.specifyLights.Length == 0)
				{
					this.Finish();
					return;
				}
			}
			this.m_lightManagers = new SceneLightManager[this.specifyLights.Length];
			for (int i = 0; i < this.specifyLights.Length; i++)
			{
				this.m_lightManagers[i] = this.specifyLights[i].GetComponent<SceneLightManager>();
			}
			this.m_materials = new Material[this.specifyLights.Length];
			for (int j = 0; j < this.specifyLights.Length; j++)
			{
				this.m_materials[j] = this.specifyLights[j].GetComponent<Renderer>().material;
			}
			if (this.transitionTime.Value <= 0f)
			{
				for (int k = 0; k < this.specifyLights.Length; k++)
				{
					this.specifyLights[k].uLightIntensity = this.intensity.Value;
					if (this.m_lightManagers[k])
					{
						Color color = this.m_lightManagers[k].validColors[UnityEngine.Random.Range(0, this.m_lightManagers[k].validColors.Length)];
						this.m_materials[k].SetColor("_TintColor", color);
					}
					else
					{
						this.m_materials[k].SetColor("_TintColor", Color.white);
					}
				}
				this.Finish();
				return;
			}
			this.m_timer = 0f;
			this.m_startIntensity = null;
			this.m_startColors = null;
			this.m_endColors = null;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x00160E30 File Offset: 0x0015F030
		public override void OnUpdate()
		{
			if (this.m_startIntensity == null)
			{
				this.m_startIntensity = new float[this.specifyLights.Length];
				this.m_startColors = new Color[this.specifyLights.Length];
				this.m_endColors = new Color[this.specifyLights.Length];
				for (int i = 0; i < this.specifyLights.Length; i++)
				{
					this.m_startIntensity[i] = this.specifyLights[i].uLightIntensity;
					this.m_startColors[i] = this.m_materials[i].GetColor("_TintColor");
					if (this.intensity.Value <= 0f)
					{
						this.m_endColors[i] = new Color(0.5f, 0.5f, 0.5f, 1f);
					}
					else
					{
						this.m_endColors[i] = ((!this.m_lightManagers[i]) ? Color.white : this.m_lightManagers[i].validColors[UnityEngine.Random.Range(0, this.m_lightManagers[i].validColors.Length)]);
					}
				}
				this.m_timer = 0f;
			}
			else
			{
				this.m_timer += BraveTime.DeltaTime;
				for (int j = 0; j < this.specifyLights.Length; j++)
				{
					this.specifyLights[j].uLightIntensity = Mathf.Lerp(this.m_startIntensity[j], this.intensity.Value, this.m_timer / this.transitionTime.Value);
					this.m_materials[j].SetColor("_TintColor", Color.Lerp(this.m_startColors[j], this.m_endColors[j], this.m_timer / this.transitionTime.Value));
				}
				if (this.m_timer >= this.transitionTime.Value)
				{
					this.Finish();
				}
			}
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x00161048 File Offset: 0x0015F248
		public override void OnExit()
		{
			for (int i = 0; i < this.specifyLights.Length; i++)
			{
				this.specifyLights[i].uLightIntensity = this.intensity.Value;
				this.m_materials[i].SetColor("_TintColor", this.m_endColors[i]);
			}
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x001610AC File Offset: 0x0015F2AC
		public new void Finish()
		{
			if (!this.IsKeptAction)
			{
				base.Finish();
			}
			else
			{
				base.Finished = true;
			}
		}

		// Token: 0x04003656 RID: 13910
		[Tooltip("Specify lights to control; if empty, this action will affect all lights on its owner.")]
		public ShadowSystem[] specifyLights;

		// Token: 0x04003657 RID: 13911
		[Tooltip("New light intensity after the transition.")]
		public FsmFloat intensity;

		// Token: 0x04003658 RID: 13912
		[Tooltip("Duraiton of the transition.")]
		public FsmFloat transitionTime;

		// Token: 0x0400365A RID: 13914
		private float[] m_startIntensity;

		// Token: 0x0400365B RID: 13915
		private Color[] m_startColors;

		// Token: 0x0400365C RID: 13916
		private Color[] m_endColors;

		// Token: 0x0400365D RID: 13917
		private float m_timer;

		// Token: 0x0400365E RID: 13918
		private SceneLightManager[] m_lightManagers;

		// Token: 0x0400365F RID: 13919
		private Material[] m_materials;
	}
}
