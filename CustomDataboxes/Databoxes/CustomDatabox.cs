using System;
using System.Collections.Generic;
using System.Collections;
using Nautilus.Assets;
using Nautilus.Handlers;
using UWE;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;
using Nautilus.Assets.Gadgets;

namespace CustomDataboxes.Databoxes
{
    internal class CustomDatabox : CustomPrefab
    {
        public string PrimaryDescription { get; set; }

        public string SecondaryDescription { get; set; }

        public TechType TechTypeToUnlock { get; set; }

        public List<LootDistributionData.BiomeData> BiomesToSpawn { get; set; }

        public List<SpawnLocation> coordinatedSpawns { get; set; }

        public Action<GameObject> ModifyGameObject { get; set; }
        public List<LootDistributionData.BiomeData> BiomesToSpawnIn => this.BiomesToSpawn;

        public List<SpawnLocation> CoordinatedSpawns => this.coordinatedSpawns;

        [SetsRequiredMembers]
        public CustomDatabox(string databoxID) : base(databoxID, databoxID, databoxID)
        {
            Debug.Log("Initializing CustomDatabox...");

            try
            {
                SetGameObject(GetGameObjectAsync);

                // Initialize BiomesToSpawn with a valid list of biomes
                this.BiomesToSpawn = BiomesToSpawnIn;

                if (BiomesToSpawnIn == null)
                {
                    Debug.LogError("BiomesToSpawn is null.");
                }
                else
                {
                    // Log the contents of the BiomesToSpawn list for debugging
                    foreach (var biome in BiomesToSpawnIn)
                    {
                        Debug.Log($"Biome: {biome.biome}, Probability: {biome.probability}, Count: {biome.count}");
                    }

                    // Set spawns using the List<> overload
                    this.SetSpawns(EntityInfo, BiomesToSpawnIn.ToArray());
                }

               
                Debug.Log("CustomDatabox initialized.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred during CustomDatabox initialization: {ex.Message}");
                Debug.LogError(ex.StackTrace);
            }
        }



        public WorldEntityInfo EntityInfo => new WorldEntityInfo()
        {
            cellLevel = LargeWorldEntity.CellLevel.Medium,
            classId = Info.ClassID,
            localScale = Vector3.one,
            prefabZUp = false,
            slotType = EntitySlot.Type.Medium,
            techType = this.Info.TechType
        };

       

#if SN1
        public IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            Debug.Log("Starting GetGameObjectAsync...");
            IPrefabRequest request = PrefabDatabase.GetPrefabAsync("02c0e341-6de9-466a-9c25-9a667ddb6158");

            yield return request;

            request.TryGetPrefab(out GameObject prefab);


            GameObject _GameObject = GameObject.Instantiate(prefab);

            _GameObject.name = Info.ClassID;
            BlueprintHandTarget blueprintHandTarget = _GameObject.GetComponent<BlueprintHandTarget>();
            if (blueprintHandTarget == null)
            {
                Debug.LogError("BlueprintHandTarget component is missing on the prefab.");
                gameObject.Set(null);
                yield break;
            }

            blueprintHandTarget.primaryTooltip = PrimaryDescription;
            blueprintHandTarget.secondaryTooltip = SecondaryDescription ?? PrimaryDescription;
            blueprintHandTarget.unlockTechType = TechTypeToUnlock;
            _GameObject.SetActive(true);

            gameObject.Set(_GameObject);
            Debug.Log("GetGameObjectAsync completed successfully.");
        }
        public static LootDistributionData.BiomeData[] ConvertListToArray(List<LootDistributionData.BiomeData> biomeList)
        {
            return biomeList.ToArray();
        }
        
        
          
        

#elif BZ
        public IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            Debug.Log("Starting GetGameObjectAsync...");
            IPrefabRequest request = PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/DataBoxes/CyclopsSonarModuleDataBox");

            yield return request;

            bool prefabRetrieved = request.TryGetPrefab(out GameObject prefab);
            if (!prefabRetrieved)
            {
                Debug.LogError("Prefab request failed. Prefab is null.");
                gameObject.Set(null);
                yield break;
            }

            GameObject _GameObject = GameObject.Instantiate(prefab);
            if (_GameObject == null)
            {
                Debug.LogError("Instantiated GameObject is null.");
                gameObject.Set(null);
                yield break;
            }

            _GameObject.name = Info.ClassID;
            BlueprintHandTarget blueprintHandTarget = _GameObject.GetComponent<BlueprintHandTarget>();
            if (blueprintHandTarget == null)
            {
                Debug.LogError("BlueprintHandTarget component is missing on the prefab.");
                gameObject.Set(null);
                yield break;
            }

            blueprintHandTarget.primaryTooltip = PrimaryDescription;
            blueprintHandTarget.secondaryTooltip = SecondaryDescription ?? PrimaryDescription;
            blueprintHandTarget.unlockTechType = TechTypeToUnlock;
            _GameObject.SetActive(false);

            gameObject.Set(_GameObject);
            Debug.Log("GetGameObjectAsync completed successfully.");
        }
#endif
    }
}