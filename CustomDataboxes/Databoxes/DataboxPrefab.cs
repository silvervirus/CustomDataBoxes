using System.Collections.Generic;
using Nautilus.Assets;
using UWE;
using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Nautilus.Handlers;
using Nautilus.Assets.Gadgets;

namespace CustomDataboxes.Databoxes
{
    internal class DataboxPrefab : CustomPrefab
    {
        readonly string alreadyUnlockedTooltip;
        readonly string primaryTooltip;
        readonly string secondaryTooltip;
        readonly TechType unlockTechType;
        readonly List<LootDistributionData.BiomeData> biomesToSpawnIn;
        readonly List<SpawnLocation> coordinatedSpawns;

        [SetsRequiredMembers]
        public DataboxPrefab(string classId, string alreadyUnlockedTooltip, string primaryTooltip,
            string secondaryTooltip, TechType unlockTechType, List<LootDistributionData.BiomeData> biomesToSpawnIn,
            List<SpawnLocation> coordinatedSpawns)
            : base(classId, classId, classId + " Databox")
        {
            this.alreadyUnlockedTooltip = alreadyUnlockedTooltip;
            this.primaryTooltip = primaryTooltip;
            this.secondaryTooltip = secondaryTooltip;
            this.unlockTechType = unlockTechType;
            this.biomesToSpawnIn = biomesToSpawnIn;
            this.coordinatedSpawns = coordinatedSpawns;
            SetGameObject(GetGameObjectAsync);
            
            
        }

        public  WorldEntityInfo EntityInfo => new WorldEntityInfo() { cellLevel = LargeWorldEntity.CellLevel.Medium, classId = Info.ClassID, localScale = Vector3.one, prefabZUp = false, slotType = EntitySlot.Type.Medium, techType = this.Info.TechType };

        public List<LootDistributionData.BiomeData> BiomesToSpawnIn => this.biomesToSpawnIn;

        public List<SpawnLocation> CoordinatedSpawns => coordinatedSpawns;

#if SN1

        public IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            Debug.Log("Starting GetGameObjectAsync...");
            IPrefabRequest request = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Alterra/DataBoxes/UltraGlideFinsDataBox.prefab");

            yield return request;

            request.TryGetPrefab(out GameObject prefab);

            GameObject _GameObject = GameObject.Instantiate(prefab);


            _GameObject.name = Info.ClassID;
            _GameObject.SetActive(false);

            BlueprintHandTarget blueprintHandTarget = _GameObject.GetComponent<BlueprintHandTarget>();
            if (blueprintHandTarget != null)
            {
                blueprintHandTarget.alreadyUnlockedTooltip = alreadyUnlockedTooltip;
                blueprintHandTarget.primaryTooltip = primaryTooltip;
                blueprintHandTarget.secondaryTooltip = secondaryTooltip;
                blueprintHandTarget.unlockTechType = unlockTechType;

                Debug.Log("BlueprintHandTarget properties set");
            }
            else
            {
                Debug.LogWarning("BlueprintHandTarget component not found on instantiated object.");
            }

            gameObject.Set(_GameObject);
            Debug.Log("GetGameObjectAsync completed successfully.");
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
            _GameObject.SetActive(false);

            BlueprintHandTarget blueprintHandTarget = _GameObject.GetComponent<BlueprintHandTarget>();
            if (blueprintHandTarget != null)
            {
                blueprintHandTarget.alreadyUnlockedTooltip = alreadyUnlockedTooltip;
                blueprintHandTarget.primaryTooltip = primaryTooltip;
                blueprintHandTarget.secondaryTooltip = secondaryTooltip;
                blueprintHandTarget.unlockTechType = unlockTechType;

                Debug.Log("BlueprintHandTarget properties set");
            }
            else
            {
                Debug.LogWarning("BlueprintHandTarget component not found on instantiated object.");
            }

            gameObject.Set(_GameObject);
            Debug.Log("GetGameObjectAsync completed successfully.");
        }
#endif
    }
}