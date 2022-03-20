using MonomiPark.SlimeRancher.DataModel;
using SRML.SR.SaveSystem;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace PowerStorageMod
{
    internal class PowerStorageGadget : Gadget, GadgetModel.Participant
    {
        private Dictionary<Identifiable.Id, GameObject> spawnFX = new Dictionary<Identifiable.Id, GameObject>(Identifiable.idComparer);
        private Transform spawnPoint;
        private PowerStorageGadgetModel model;

        private void Awake()
        {
            GameObject gameObject = Instantiate(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
            gameObject.transform.localScale = transform.localScale;
            gameObject.transform.SetParent(transform.parent);
            spawnFX.Add(Identifiable.Id.VALLEY_AMMO_1, gameObject);
        }

        private void SpawnItem()
        {
#if ECHO
            Rigidbody component = SRBehaviour.InstantiateActor(this.lookupDir.GetPrefab(id), this.zoneDirector.regionSetId, this.spawnPoint.position, this.spawnPoint.rotation, false).GetComponent<Rigidbody>();
            if (component != null) {
            	float num = Identifiable.IsEcho(id) ? 20f : 1f;
            	component.AddForce(this.spawnPoint.forward * 50f + new Vector3(Randoms.SHARED.GetInRange(-num, num), Randoms.SHARED.GetInRange(-num, num), Randoms.SHARED.GetInRange(-num, num)));
            }
#endif

            GameObject FXObject = spawnFX.Get(Identifiable.Id.VALLEY_AMMO_1);
            if (FXObject != null)
                SRBehaviour.SpawnAndPlayFX(gameObject, spawnPoint.position, spawnPoint.rotation);
        }

        public int maxCount => 100;

        public int time = 40;
        public int countnow = 0;

        public void Update()
        {
            if (time == 0)
            {
                time = 40;
                countnow++;
            }
            else
            {
                time--;
            }
        }

        public void InitModel(GadgetModel model)
        {
            ((PowerStorageGadgetModel)model).energyValue = this.countnow;
        }

        public void SetModel(GadgetModel model)
        {
            this.model = (PowerStorageGadgetModel)model;
        }
    }

    internal class PowerStorageGadgetModel : GadgetModel, ISerializableModel
    {
        public int energyValue;
        private int Version => 0;

        public PowerStorageGadgetModel(Gadget.Id ident, string siteId, Transform transform) : base(ident, siteId, transform) { }

        public void LoadData(BinaryReader reader)
        {
            int version = reader.ReadInt32();
            this.energyValue = reader.ReadInt32();
        }

        public void WriteData(BinaryWriter writer)
        {
            writer.Write(this.Version);
            writer.Write(this.energyValue);
        }
    }
}