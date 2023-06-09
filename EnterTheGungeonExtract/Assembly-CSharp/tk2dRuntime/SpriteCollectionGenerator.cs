﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace tk2dRuntime
{
	// Token: 0x02000BAE RID: 2990
	internal static class SpriteCollectionGenerator
	{
		// Token: 0x06003F23 RID: 16163 RVA: 0x0013ECA0 File Offset: 0x0013CEA0
		public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
		{
			return SpriteCollectionGenerator.CreateFromTexture(texture, size, new string[] { "Unnamed" }, new Rect[] { region }, new Vector2[] { anchor });
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0013ECE0 File Offset: 0x0013CEE0
		public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, string[] names, Rect[] regions, Vector2[] anchors)
		{
			Vector2 vector = new Vector2((float)texture.width, (float)texture.height);
			return SpriteCollectionGenerator.CreateFromTexture(texture, size, vector, names, regions, null, anchors, null);
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0013ED10 File Offset: 0x0013CF10
		public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Vector2 textureDimensions, string[] names, Rect[] regions, Rect[] trimRects, Vector2[] anchors, bool[] rotated)
		{
			return SpriteCollectionGenerator.CreateFromTexture(null, texture, size, textureDimensions, names, regions, trimRects, anchors, rotated, string.Empty);
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0013ED34 File Offset: 0x0013CF34
		public static tk2dSpriteCollectionData CreateFromTexture(GameObject parentObject, Texture texture, tk2dSpriteCollectionSize size, Vector2 textureDimensions, string[] names, Rect[] regions, Rect[] trimRects, Vector2[] anchors, bool[] rotated, string CustomShader = "")
		{
			GameObject gameObject = ((!(parentObject != null)) ? new GameObject("SpriteCollection") : parentObject);
			tk2dSpriteCollectionData tk2dSpriteCollectionData = gameObject.AddComponent<tk2dSpriteCollectionData>();
			tk2dSpriteCollectionData.Transient = true;
			tk2dSpriteCollectionData.version = 3;
			tk2dSpriteCollectionData.invOrthoSize = 1f / size.OrthoSize;
			tk2dSpriteCollectionData.halfTargetHeight = size.TargetHeight * 0.5f;
			tk2dSpriteCollectionData.premultipliedAlpha = false;
			string text = (string.IsNullOrEmpty(CustomShader) ? "tk2d/BlendVertexColor" : CustomShader);
			tk2dSpriteCollectionData.material = new Material(Shader.Find(text));
			tk2dSpriteCollectionData.material.mainTexture = texture;
			tk2dSpriteCollectionData.materials = new Material[] { tk2dSpriteCollectionData.material };
			tk2dSpriteCollectionData.textures = new Texture[] { texture };
			tk2dSpriteCollectionData.buildKey = UnityEngine.Random.Range(0, int.MaxValue);
			float num = 2f * size.OrthoSize / size.TargetHeight;
			Rect rect = new Rect(0f, 0f, 0f, 0f);
			tk2dSpriteCollectionData.spriteDefinitions = new tk2dSpriteDefinition[regions.Length];
			for (int i = 0; i < regions.Length; i++)
			{
				bool flag = rotated != null && rotated[i];
				if (trimRects != null)
				{
					rect = trimRects[i];
				}
				else if (flag)
				{
					rect.Set(0f, 0f, regions[i].height, regions[i].width);
				}
				else
				{
					rect.Set(0f, 0f, regions[i].width, regions[i].height);
				}
				tk2dSpriteCollectionData.spriteDefinitions[i] = SpriteCollectionGenerator.CreateDefinitionForRegionInTexture(names[i], textureDimensions, num, regions[i], rect, anchors[i], flag);
			}
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in tk2dSpriteCollectionData.spriteDefinitions)
			{
				tk2dSpriteDefinition.material = tk2dSpriteCollectionData.material;
			}
			return tk2dSpriteCollectionData;
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x0013EF64 File Offset: 0x0013D164
		private static tk2dSpriteDefinition CreateDefinitionForRegionInTexture(string name, Vector2 textureDimensions, float scale, Rect uvRegion, Rect trimRect, Vector2 anchor, bool rotated)
		{
			float height = uvRegion.height;
			float width = uvRegion.width;
			float x = textureDimensions.x;
			float y = textureDimensions.y;
			tk2dSpriteDefinition tk2dSpriteDefinition = new tk2dSpriteDefinition();
			tk2dSpriteDefinition.flipped = ((!rotated) ? tk2dSpriteDefinition.FlipMode.None : tk2dSpriteDefinition.FlipMode.TPackerCW);
			tk2dSpriteDefinition.extractRegion = false;
			tk2dSpriteDefinition.name = name;
			tk2dSpriteDefinition.colliderType = tk2dSpriteDefinition.ColliderType.Unset;
			Vector2 vector = new Vector2(0.001f, 0.001f);
			Vector2 vector2 = new Vector2((uvRegion.x + vector.x) / x, 1f - (uvRegion.y + uvRegion.height + vector.y) / y);
			Vector2 vector3 = new Vector2((uvRegion.x + uvRegion.width - vector.x) / x, 1f - (uvRegion.y - vector.y) / y);
			Vector2 vector4 = new Vector2(trimRect.x - anchor.x, -trimRect.y + anchor.y);
			if (rotated)
			{
				vector4.y -= width;
			}
			vector4 *= scale;
			Vector3 vector5 = new Vector3(-anchor.x * scale, anchor.y * scale, 0f);
			Vector3 vector6 = vector5 + new Vector3(trimRect.width * scale, -trimRect.height * scale, 0f);
			Vector3 vector7 = new Vector3(0f, -height * scale, 0f);
			Vector3 vector8 = vector7 + new Vector3(width * scale, height * scale, 0f);
			if (rotated)
			{
				tk2dSpriteDefinition.position0 = new Vector3(-vector8.y + vector4.x, vector7.x + vector4.y, 0f);
				tk2dSpriteDefinition.position1 = new Vector3(-vector7.y + vector4.x, vector7.x + vector4.y, 0f);
				tk2dSpriteDefinition.position2 = new Vector3(-vector8.y + vector4.x, vector8.x + vector4.y, 0f);
				tk2dSpriteDefinition.position3 = new Vector3(-vector7.y + vector4.x, vector8.x + vector4.y, 0f);
				tk2dSpriteDefinition.uvs = new Vector2[]
				{
					new Vector2(vector2.x, vector3.y),
					new Vector2(vector2.x, vector2.y),
					new Vector2(vector3.x, vector3.y),
					new Vector2(vector3.x, vector2.y)
				};
			}
			else
			{
				tk2dSpriteDefinition.position0 = new Vector3(vector7.x + vector4.x, vector7.y + vector4.y, 0f);
				tk2dSpriteDefinition.position1 = new Vector3(vector8.x + vector4.x, vector7.y + vector4.y, 0f);
				tk2dSpriteDefinition.position2 = new Vector3(vector7.x + vector4.x, vector8.y + vector4.y, 0f);
				tk2dSpriteDefinition.position3 = new Vector3(vector8.x + vector4.x, vector8.y + vector4.y, 0f);
				tk2dSpriteDefinition.uvs = new Vector2[]
				{
					new Vector2(vector2.x, vector2.y),
					new Vector2(vector3.x, vector2.y),
					new Vector2(vector2.x, vector3.y),
					new Vector2(vector3.x, vector3.y)
				};
			}
			tk2dSpriteDefinition.normals = new Vector3[0];
			tk2dSpriteDefinition.tangents = new Vector4[0];
			tk2dSpriteDefinition.indices = new int[] { 0, 3, 1, 2, 3, 0 };
			Vector3 vector9 = new Vector3(vector5.x, vector6.y, 0f);
			Vector3 vector10 = new Vector3(vector6.x, vector5.y, 0f);
			tk2dSpriteDefinition.boundsDataCenter = (vector10 + vector9) / 2f;
			tk2dSpriteDefinition.boundsDataExtents = vector10 - vector9;
			tk2dSpriteDefinition.untrimmedBoundsDataCenter = (vector10 + vector9) / 2f;
			tk2dSpriteDefinition.untrimmedBoundsDataExtents = vector10 - vector9;
			tk2dSpriteDefinition.texelSize = new Vector2(scale, scale);
			return tk2dSpriteDefinition;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0013F464 File Offset: 0x0013D664
		public static tk2dSpriteCollectionData CreateFromTexturePacker(tk2dSpriteCollectionSize spriteCollectionSize, string texturePackerFileContents, Texture texture)
		{
			List<string> list = new List<string>();
			List<Rect> list2 = new List<Rect>();
			List<Rect> list3 = new List<Rect>();
			List<Vector2> list4 = new List<Vector2>();
			List<bool> list5 = new List<bool>();
			int num = 0;
			TextReader textReader = new StringReader(texturePackerFileContents);
			bool flag = false;
			bool flag2 = false;
			string text = string.Empty;
			Rect rect = default(Rect);
			Rect rect2 = default(Rect);
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			for (string text2 = textReader.ReadLine(); text2 != null; text2 = textReader.ReadLine())
			{
				if (text2.Length > 0)
				{
					char c = text2[0];
					if (num != 0)
					{
						if (num == 1)
						{
							switch (c)
							{
							case 'n':
								text = text2.Substring(2);
								break;
							case 'o':
							{
								string[] array = text2.Split(new char[0]);
								rect2.Set((float)int.Parse(array[1]), (float)int.Parse(array[2]), (float)int.Parse(array[3]), (float)int.Parse(array[4]));
								flag2 = true;
								break;
							}
							default:
								if (c == '~')
								{
									list.Add(text);
									list5.Add(flag);
									list2.Add(rect);
									if (!flag2)
									{
										if (flag)
										{
											rect2.Set(0f, 0f, rect.height, rect.width);
										}
										else
										{
											rect2.Set(0f, 0f, rect.width, rect.height);
										}
									}
									list3.Add(rect2);
									zero2.Set((float)((int)(rect2.width / 2f)), (float)((int)(rect2.height / 2f)));
									list4.Add(zero2);
									text = string.Empty;
									flag2 = false;
									flag = false;
								}
								break;
							case 'r':
								flag = int.Parse(text2.Substring(2)) == 1;
								break;
							case 's':
							{
								string[] array2 = text2.Split(new char[0]);
								rect.Set((float)int.Parse(array2[1]), (float)int.Parse(array2[2]), (float)int.Parse(array2[3]), (float)int.Parse(array2[4]));
								break;
							}
							}
						}
					}
					else if (c != 'h')
					{
						if (c != 'i')
						{
							if (c != 'w')
							{
								if (c == '~')
								{
									num++;
								}
							}
							else
							{
								zero.x = (float)int.Parse(text2.Substring(2));
							}
						}
					}
					else
					{
						zero.y = (float)int.Parse(text2.Substring(2));
					}
				}
			}
			return SpriteCollectionGenerator.CreateFromTexture(texture, spriteCollectionSize, zero, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), list5.ToArray());
		}
	}
}
