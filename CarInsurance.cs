using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System.Linq;
using System.Collections.Generic;
using Rocket.API.Collections;

namespace CarInsurance
{
    class CarInsurance : RocketPlugin
    {
        public List<Info> damagedOwners { get; private set; }
        public static CarInsurance Instance { get; private set; }
        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"success","Successfully got your car back from the insurance!"},
            {"fail","You don't have any insurance or not enough experience."}
        };

        protected override void Load()
        {
            VehicleManager.onDamageVehicleRequested += OnVehicleDamage;
            Instance = this;
            damagedOwners = new List<Info>();
        }

        public ushort PlayerDeservesInsurance(CSteamID player)
        {
            for (int i = damagedOwners.Count-1; i > -1; i--)
            {
                if(damagedOwners[i].vehicleOwner == player)
                {
                    ushort toReturn = damagedOwners[i].vehicleId;
                    damagedOwners[i] = damagedOwners[damagedOwners.Count - 1]; // move last element to current position, basically replacing current obj with the last obj
                    damagedOwners.RemoveAt(damagedOwners.Count-1); // by using .RemoveAt() for the last element we get O(1)
                    return toReturn;
                }
            }

            return 0;
        }

        protected override void Unload()
        {
            VehicleManager.onDamageVehicleRequested -= OnVehicleDamage;
            Instance = null;
        }

        public void OnVehicleDamage(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (vehicle.isLocked && vehicle.health - pendingTotalDamage <= 0)
            {
                Info info = new Info { vehicleId = vehicle.id, vehicleOwner = vehicle.lockedOwner };
                
                if (!damagedOwners.Any(x => x.vehicleOwner == info.vehicleOwner))
                {
                    damagedOwners.Add(info);
                }
            }
        }
    }

    public class Info
    {
        public ushort vehicleId;
        public CSteamID vehicleOwner;
    }
}