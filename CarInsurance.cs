using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System.Linq;
using System.Collections.Generic;

namespace CarInsurance
{
    class CarInsurance : RocketPlugin
    {
        public List<Info> damagedOwners { get; private set; }
        public static CarInsurance Instance { get; private set; }

        protected override void Load()
        {
            VehicleManager.onDamageVehicleRequested += new DamageVehicleRequestHandler(OnVehicleDamage);
            Instance = this;
            damagedOwners = new List<Info>();
        }

        public ushort PlayerDeservesInsurance(CSteamID player)
        {
            for (int i = 0; i < damagedOwners.Count; i++)
            {
                if(damagedOwners[i].vehicleOwner == player)
                {
                    ushort toReturn = damagedOwners[i].vehicleId;
                    damagedOwners.RemoveAt(i);
                    return toReturn;
                }
            }

            return 0;
        }

        protected override void Unload()
        {
            VehicleManager.onDamageVehicleRequested -= new DamageVehicleRequestHandler(OnVehicleDamage);
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