using System;
using System.Collections.Generic;
using FullSerializer.Internal;

namespace FullSerializer
{
	// Token: 0x020005B1 RID: 1457
	public class fsSerializer
	{
		// Token: 0x0600228A RID: 8842 RVA: 0x00098104 File Offset: 0x00096304
		public fsSerializer()
		{
			this._cachedConverters = new Dictionary<Type, fsBaseConverter>();
			this._cachedProcessors = new Dictionary<Type, List<fsObjectProcessor>>();
			this._references = new fsCyclicReferenceManager();
			this._lazyReferenceWriter = new fsSerializer.fsLazyCycleDefinitionWriter();
			this._availableConverters = new List<fsConverter>
			{
				new fsNullableConverter
				{
					Serializer = this
				},
				new fsGuidConverter
				{
					Serializer = this
				},
				new fsTypeConverter
				{
					Serializer = this
				},
				new fsDateConverter
				{
					Serializer = this
				},
				new fsEnumConverter
				{
					Serializer = this
				},
				new fsPrimitiveConverter
				{
					Serializer = this
				},
				new fsArrayConverter
				{
					Serializer = this
				},
				new fsDictionaryConverter
				{
					Serializer = this
				},
				new fsIEnumerableConverter
				{
					Serializer = this
				},
				new fsKeyValuePairConverter
				{
					Serializer = this
				},
				new fsWeakReferenceConverter
				{
					Serializer = this
				},
				new fsReflectedConverter
				{
					Serializer = this
				}
			};
			this._availableDirectConverters = new Dictionary<Type, fsDirectConverter>();
			this._processors = new List<fsObjectProcessor>
			{
				new fsSerializationCallbackProcessor()
			};
			this.Context = new fsContext();
			foreach (Type type in fsConverterRegistrar.Converters)
			{
				this.AddConverter((fsBaseConverter)Activator.CreateInstance(type));
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000982E8 File Offset: 0x000964E8
		public static bool IsReservedKeyword(string key)
		{
			return fsSerializer._reservedKeywords.Contains(key);
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000982F8 File Offset: 0x000964F8
		private static bool IsObjectReference(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$ref");
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00098318 File Offset: 0x00096518
		private static bool IsObjectDefinition(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$id");
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00098338 File Offset: 0x00096538
		private static bool IsVersioned(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$version");
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00098358 File Offset: 0x00096558
		private static bool IsTypeSpecified(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$type");
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00098378 File Offset: 0x00096578
		private static bool IsWrappedData(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$content");
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x00098398 File Offset: 0x00096598
		public static void StripDeserializationMetadata(ref fsData data)
		{
			if (data.IsDictionary && data.AsDictionary.ContainsKey("$content"))
			{
				data = data.AsDictionary["$content"];
			}
			if (data.IsDictionary)
			{
				Dictionary<string, fsData> asDictionary = data.AsDictionary;
				asDictionary.Remove("$ref");
				asDictionary.Remove("$id");
				asDictionary.Remove("$type");
				asDictionary.Remove("$version");
			}
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x00098420 File Offset: 0x00096620
		private static void ConvertLegacyData(ref fsData data)
		{
			if (!data.IsDictionary)
			{
				return;
			}
			Dictionary<string, fsData> asDictionary = data.AsDictionary;
			if (asDictionary.Count > 2)
			{
				return;
			}
			string text = "ReferenceId";
			string text2 = "SourceId";
			string text3 = "Data";
			string text4 = "Type";
			string text5 = "Data";
			if (asDictionary.Count == 2 && asDictionary.ContainsKey(text4) && asDictionary.ContainsKey(text5))
			{
				data = asDictionary[text5];
				fsSerializer.EnsureDictionary(data);
				fsSerializer.ConvertLegacyData(ref data);
				data.AsDictionary["$type"] = asDictionary[text4];
			}
			else if (asDictionary.Count == 2 && asDictionary.ContainsKey(text2) && asDictionary.ContainsKey(text3))
			{
				data = asDictionary[text3];
				fsSerializer.EnsureDictionary(data);
				fsSerializer.ConvertLegacyData(ref data);
				data.AsDictionary["$id"] = asDictionary[text2];
			}
			else if (asDictionary.Count == 1 && asDictionary.ContainsKey(text))
			{
				data = fsData.CreateDictionary();
				data.AsDictionary["$ref"] = asDictionary[text];
			}
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x00098558 File Offset: 0x00096758
		private static void Invoke_OnBeforeSerialize(List<fsObjectProcessor> processors, Type storageType, object instance)
		{
			for (int i = 0; i < processors.Count; i++)
			{
				processors[i].OnBeforeSerialize(storageType, instance);
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x0009858C File Offset: 0x0009678C
		private static void Invoke_OnAfterSerialize(List<fsObjectProcessor> processors, Type storageType, object instance, ref fsData data)
		{
			for (int i = processors.Count - 1; i >= 0; i--)
			{
				processors[i].OnAfterSerialize(storageType, instance, ref data);
			}
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000985C4 File Offset: 0x000967C4
		private static void Invoke_OnBeforeDeserialize(List<fsObjectProcessor> processors, Type storageType, ref fsData data)
		{
			for (int i = 0; i < processors.Count; i++)
			{
				processors[i].OnBeforeDeserialize(storageType, ref data);
			}
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000985F8 File Offset: 0x000967F8
		private static void Invoke_OnBeforeDeserializeAfterInstanceCreation(List<fsObjectProcessor> processors, Type storageType, object instance, ref fsData data)
		{
			for (int i = 0; i < processors.Count; i++)
			{
				processors[i].OnBeforeDeserializeAfterInstanceCreation(storageType, instance, ref data);
			}
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x0009862C File Offset: 0x0009682C
		private static void Invoke_OnAfterDeserialize(List<fsObjectProcessor> processors, Type storageType, object instance)
		{
			for (int i = processors.Count - 1; i >= 0; i--)
			{
				processors[i].OnAfterDeserialize(storageType, instance);
			}
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00098660 File Offset: 0x00096860
		private static void EnsureDictionary(fsData data)
		{
			if (!data.IsDictionary)
			{
				fsData fsData = data.Clone();
				data.BecomeDictionary();
				data.AsDictionary["$content"] = fsData;
			}
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x00098698 File Offset: 0x00096898
		public void AddProcessor(fsObjectProcessor processor)
		{
			this._processors.Add(processor);
			this._cachedProcessors = new Dictionary<Type, List<fsObjectProcessor>>();
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000986B4 File Offset: 0x000968B4
		private List<fsObjectProcessor> GetProcessors(Type type)
		{
			fsObjectAttribute attribute = fsPortableReflection.GetAttribute<fsObjectAttribute>(type);
			List<fsObjectProcessor> list;
			if (attribute != null && attribute.Processor != null)
			{
				fsObjectProcessor fsObjectProcessor = (fsObjectProcessor)Activator.CreateInstance(attribute.Processor);
				list = new List<fsObjectProcessor>();
				list.Add(fsObjectProcessor);
				this._cachedProcessors[type] = list;
			}
			else if (!this._cachedProcessors.TryGetValue(type, out list))
			{
				list = new List<fsObjectProcessor>();
				for (int i = 0; i < this._processors.Count; i++)
				{
					fsObjectProcessor fsObjectProcessor2 = this._processors[i];
					if (fsObjectProcessor2.CanProcess(type))
					{
						list.Add(fsObjectProcessor2);
					}
				}
				this._cachedProcessors[type] = list;
			}
			return list;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00098770 File Offset: 0x00096970
		public void AddConverter(fsBaseConverter converter)
		{
			if (converter.Serializer != null)
			{
				throw new InvalidOperationException("Cannot add a single converter instance to multiple fsConverters -- please construct a new instance for " + converter);
			}
			if (converter is fsDirectConverter)
			{
				fsDirectConverter fsDirectConverter = (fsDirectConverter)converter;
				this._availableDirectConverters[fsDirectConverter.ModelType] = fsDirectConverter;
			}
			else
			{
				if (!(converter is fsConverter))
				{
					throw new InvalidOperationException("Unable to add converter " + converter + "; the type association strategy is unknown. Please use either fsDirectConverter or fsConverter as your base type.");
				}
				this._availableConverters.Insert(0, (fsConverter)converter);
			}
			converter.Serializer = this;
			this._cachedConverters = new Dictionary<Type, fsBaseConverter>();
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x0009880C File Offset: 0x00096A0C
		private fsBaseConverter GetConverter(Type type)
		{
			fsObjectAttribute attribute = fsPortableReflection.GetAttribute<fsObjectAttribute>(type);
			fsBaseConverter fsBaseConverter;
			if (attribute != null && attribute.Converter != null)
			{
				fsBaseConverter = (fsBaseConverter)Activator.CreateInstance(attribute.Converter);
				fsBaseConverter.Serializer = this;
				this._cachedConverters[type] = fsBaseConverter;
			}
			else if (!this._cachedConverters.TryGetValue(type, out fsBaseConverter))
			{
				if (this._availableDirectConverters.ContainsKey(type))
				{
					fsBaseConverter = this._availableDirectConverters[type];
					this._cachedConverters[type] = fsBaseConverter;
				}
				else
				{
					for (int i = 0; i < this._availableConverters.Count; i++)
					{
						if (this._availableConverters[i].CanProcess(type))
						{
							fsBaseConverter = this._availableConverters[i];
							this._cachedConverters[type] = fsBaseConverter;
							break;
						}
					}
				}
			}
			if (fsBaseConverter == null)
			{
				throw new InvalidOperationException("Internal error -- could not find a converter for " + type);
			}
			return fsBaseConverter;
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00098908 File Offset: 0x00096B08
		public fsResult TrySerialize<T>(T instance, out fsData data)
		{
			return this.TrySerialize(typeof(T), instance, out data);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00098924 File Offset: 0x00096B24
		public fsResult TryDeserialize<T>(fsData data, ref T instance)
		{
			object obj = instance;
			fsResult fsResult = this.TryDeserialize(data, typeof(T), ref obj);
			if (fsResult.Succeeded)
			{
				instance = (T)((object)obj);
			}
			return fsResult;
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0009896C File Offset: 0x00096B6C
		public fsResult TrySerialize(Type storageType, object instance, out fsData data)
		{
			List<fsObjectProcessor> processors = this.GetProcessors((instance != null) ? instance.GetType() : storageType);
			fsSerializer.Invoke_OnBeforeSerialize(processors, storageType, instance);
			if (object.ReferenceEquals(instance, null))
			{
				data = new fsData();
				fsSerializer.Invoke_OnAfterSerialize(processors, storageType, instance, ref data);
				return fsResult.Success;
			}
			fsResult fsResult = this.InternalSerialize_1_ProcessCycles(storageType, instance, out data);
			fsSerializer.Invoke_OnAfterSerialize(processors, storageType, instance, ref data);
			return fsResult;
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x000989D0 File Offset: 0x00096BD0
		private fsResult InternalSerialize_1_ProcessCycles(Type storageType, object instance, out fsData data)
		{
			fsResult fsResult;
			try
			{
				this._references.Enter();
				if (!this.GetConverter(instance.GetType()).RequestCycleSupport(instance.GetType()))
				{
					fsResult = this.InternalSerialize_2_Inheritance(storageType, instance, out data);
				}
				else if (this._references.IsReference(instance))
				{
					data = fsData.CreateDictionary();
					this._lazyReferenceWriter.WriteReference(this._references.GetReferenceId(instance), data.AsDictionary);
					fsResult = fsResult.Success;
				}
				else
				{
					this._references.MarkSerialized(instance);
					fsResult fsResult2 = this.InternalSerialize_2_Inheritance(storageType, instance, out data);
					if (fsResult2.Failed)
					{
						fsResult = fsResult2;
					}
					else
					{
						this._lazyReferenceWriter.WriteDefinition(this._references.GetReferenceId(instance), data);
						fsResult = fsResult2;
					}
				}
			}
			finally
			{
				if (this._references.Exit())
				{
					this._lazyReferenceWriter.Clear();
				}
			}
			return fsResult;
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x00098ACC File Offset: 0x00096CCC
		private fsResult InternalSerialize_2_Inheritance(Type storageType, object instance, out fsData data)
		{
			fsResult fsResult = this.InternalSerialize_3_ProcessVersioning(instance, out data);
			if (fsResult.Failed)
			{
				return fsResult;
			}
			if (storageType != instance.GetType() && this.GetConverter(storageType).RequestInheritanceSupport(storageType))
			{
				fsSerializer.EnsureDictionary(data);
				data.AsDictionary["$type"] = new fsData(instance.GetType().FullName);
			}
			return fsResult;
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00098B38 File Offset: 0x00096D38
		private fsResult InternalSerialize_3_ProcessVersioning(object instance, out fsData data)
		{
			fsOption<fsVersionedType> versionedType = fsVersionManager.GetVersionedType(instance.GetType());
			if (!versionedType.HasValue)
			{
				return this.InternalSerialize_4_Converter(instance, out data);
			}
			fsVersionedType value = versionedType.Value;
			fsResult fsResult = this.InternalSerialize_4_Converter(instance, out data);
			if (fsResult.Failed)
			{
				return fsResult;
			}
			fsSerializer.EnsureDictionary(data);
			data.AsDictionary["$version"] = new fsData(value.VersionString);
			return fsResult;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00098BAC File Offset: 0x00096DAC
		private fsResult InternalSerialize_4_Converter(object instance, out fsData data)
		{
			Type type = instance.GetType();
			return this.GetConverter(type).TrySerialize(instance, out data, type);
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00098BD0 File Offset: 0x00096DD0
		public fsResult TryDeserialize(fsData data, Type storageType, ref object result)
		{
			if (data.IsNull)
			{
				result = null;
				List<fsObjectProcessor> processors = this.GetProcessors(storageType);
				fsSerializer.Invoke_OnBeforeDeserialize(processors, storageType, ref data);
				fsSerializer.Invoke_OnAfterDeserialize(processors, storageType, null);
				return fsResult.Success;
			}
			fsSerializer.ConvertLegacyData(ref data);
			fsResult fsResult2;
			try
			{
				this._references.Enter();
				List<fsObjectProcessor> list;
				fsResult fsResult = this.InternalDeserialize_1_CycleReference(data, storageType, ref result, out list);
				if (fsResult.Succeeded)
				{
					fsSerializer.Invoke_OnAfterDeserialize(list, storageType, result);
				}
				fsResult2 = fsResult;
			}
			finally
			{
				this._references.Exit();
			}
			return fsResult2;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x00098C64 File Offset: 0x00096E64
		private fsResult InternalDeserialize_1_CycleReference(fsData data, Type storageType, ref object result, out List<fsObjectProcessor> processors)
		{
			if (fsSerializer.IsObjectReference(data))
			{
				int num = int.Parse(data.AsDictionary["$ref"].AsString);
				result = this._references.GetReferenceObject(num);
				processors = this.GetProcessors(result.GetType());
				return fsResult.Success;
			}
			return this.InternalDeserialize_2_Version(data, storageType, ref result, out processors);
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00098CC8 File Offset: 0x00096EC8
		private fsResult InternalDeserialize_2_Version(fsData data, Type storageType, ref object result, out List<fsObjectProcessor> processors)
		{
			if (fsSerializer.IsVersioned(data))
			{
				string asString = data.AsDictionary["$version"].AsString;
				fsOption<fsVersionedType> versionedType = fsVersionManager.GetVersionedType(storageType);
				if (versionedType.HasValue && versionedType.Value.VersionString != asString)
				{
					fsResult fsResult = fsResult.Success;
					List<fsVersionedType> list;
					fsResult += fsVersionManager.GetVersionImportPath(asString, versionedType.Value, out list);
					if (fsResult.Failed)
					{
						processors = this.GetProcessors(storageType);
						return fsResult;
					}
					fsResult += this.InternalDeserialize_3_Inheritance(data, list[0].ModelType, ref result, out processors);
					if (fsResult.Failed)
					{
						return fsResult;
					}
					for (int i = 1; i < list.Count; i++)
					{
						result = list[i].Migrate(result);
					}
					processors = this.GetProcessors(fsResult.GetType());
					return fsResult;
				}
			}
			return this.InternalDeserialize_3_Inheritance(data, storageType, ref result, out processors);
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x00098DDC File Offset: 0x00096FDC
		private fsResult InternalDeserialize_3_Inheritance(fsData data, Type storageType, ref object result, out List<fsObjectProcessor> processors)
		{
			fsResult success = fsResult.Success;
			processors = this.GetProcessors(storageType);
			fsSerializer.Invoke_OnBeforeDeserialize(processors, storageType, ref data);
			Type type = storageType;
			if (fsSerializer.IsTypeSpecified(data))
			{
				fsData fsData = data.AsDictionary["$type"];
				if (!fsData.IsString)
				{
					success.AddMessage("$type value must be a string (in " + data + ")");
				}
				else
				{
					string asString = fsData.AsString;
					Type type2 = fsTypeLookup.GetType(asString);
					if (type2 == null)
					{
						success.AddMessage("Unable to locate specified type \"" + asString + "\"");
					}
					else if (!storageType.IsAssignableFrom(type2))
					{
						success.AddMessage(string.Concat(new object[] { "Ignoring type specifier; a field/property of type ", storageType, " cannot hold an instance of ", type2 }));
					}
					else
					{
						type = type2;
					}
				}
			}
			if (object.ReferenceEquals(result, null) || result.GetType() != type)
			{
				result = this.GetConverter(type).CreateInstance(data, type);
			}
			fsSerializer.Invoke_OnBeforeDeserializeAfterInstanceCreation(processors, storageType, result, ref data);
			return success + this.InternalDeserialize_4_Cycles(data, type, ref result);
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00098F04 File Offset: 0x00097104
		private fsResult InternalDeserialize_4_Cycles(fsData data, Type resultType, ref object result)
		{
			if (fsSerializer.IsObjectDefinition(data))
			{
				int num = int.Parse(data.AsDictionary["$id"].AsString);
				this._references.AddReferenceWithId(num, result);
			}
			return this.InternalDeserialize_5_Converter(data, resultType, ref result);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x00098F50 File Offset: 0x00097150
		private fsResult InternalDeserialize_5_Converter(fsData data, Type resultType, ref object result)
		{
			if (fsSerializer.IsWrappedData(data))
			{
				data = data.AsDictionary["$content"];
			}
			return this.GetConverter(resultType).TryDeserialize(data, ref result, resultType);
		}

		// Token: 0x04001859 RID: 6233
		private static HashSet<string> _reservedKeywords = new HashSet<string> { "$ref", "$id", "$type", "$version", "$content" };

		// Token: 0x0400185A RID: 6234
		private const string Key_ObjectReference = "$ref";

		// Token: 0x0400185B RID: 6235
		private const string Key_ObjectDefinition = "$id";

		// Token: 0x0400185C RID: 6236
		private const string Key_InstanceType = "$type";

		// Token: 0x0400185D RID: 6237
		private const string Key_Version = "$version";

		// Token: 0x0400185E RID: 6238
		private const string Key_Content = "$content";

		// Token: 0x0400185F RID: 6239
		private Dictionary<Type, fsBaseConverter> _cachedConverters;

		// Token: 0x04001860 RID: 6240
		private Dictionary<Type, List<fsObjectProcessor>> _cachedProcessors;

		// Token: 0x04001861 RID: 6241
		private readonly List<fsConverter> _availableConverters;

		// Token: 0x04001862 RID: 6242
		private readonly Dictionary<Type, fsDirectConverter> _availableDirectConverters;

		// Token: 0x04001863 RID: 6243
		private readonly List<fsObjectProcessor> _processors;

		// Token: 0x04001864 RID: 6244
		private readonly fsCyclicReferenceManager _references;

		// Token: 0x04001865 RID: 6245
		private readonly fsSerializer.fsLazyCycleDefinitionWriter _lazyReferenceWriter;

		// Token: 0x04001866 RID: 6246
		public fsContext Context;

		// Token: 0x020005B2 RID: 1458
		internal class fsLazyCycleDefinitionWriter
		{
			// Token: 0x060022AB RID: 8875 RVA: 0x00098FA0 File Offset: 0x000971A0
			public void WriteDefinition(int id, fsData data)
			{
				if (this._references.Contains(id))
				{
					fsSerializer.EnsureDictionary(data);
					data.AsDictionary["$id"] = new fsData(id.ToString());
				}
				else
				{
					this._pendingDefinitions[id] = data;
				}
			}

			// Token: 0x060022AC RID: 8876 RVA: 0x00098FF8 File Offset: 0x000971F8
			public void WriteReference(int id, Dictionary<string, fsData> dict)
			{
				if (this._pendingDefinitions.ContainsKey(id))
				{
					fsData fsData = this._pendingDefinitions[id];
					fsSerializer.EnsureDictionary(fsData);
					fsData.AsDictionary["$id"] = new fsData(id.ToString());
					this._pendingDefinitions.Remove(id);
				}
				else
				{
					this._references.Add(id);
				}
				dict["$ref"] = new fsData(id.ToString());
			}

			// Token: 0x060022AD RID: 8877 RVA: 0x00099088 File Offset: 0x00097288
			public void Clear()
			{
				this._pendingDefinitions.Clear();
			}

			// Token: 0x04001867 RID: 6247
			private Dictionary<int, fsData> _pendingDefinitions = new Dictionary<int, fsData>();

			// Token: 0x04001868 RID: 6248
			private HashSet<int> _references = new HashSet<int>();
		}
	}
}
