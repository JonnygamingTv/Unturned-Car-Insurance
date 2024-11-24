using SDG.Unturned;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace CarInsurance
{
    public class InsuranceCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "insurance";
        public string Help => "This command will allow you to bring back your damaged car!";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer client = caller as UnturnedPlayer;

            Info vehicle = CarInsurance.Instance.PlayerGetInsurance(client.CSteamID);

            if(vehicle != null && client.Experience >= CarInsurance.Instance.Configuration.Instance.Cost)
            {
                VehicleManager.SpawnVehicleV3((VehicleAsset)Assets.FindBaseVehicleAssetByGuidOrLegacyId(vehicle.vehicleGuid, vehicle.vehicleId), 0, 0, 0, client.Position, client.Player.transform.rotation, false, false, false, false, 100, 100, 100, client.CSteamID, client.SteamGroupID, false, new byte[0][], byte.MaxValue);
                UnturnedChat.Say(caller, CarInsurance.Instance.Translate("success"));
                client.Experience -= CarInsurance.Instance.Configuration.Instance.Cost;
            }
            else
            {
                UnturnedChat.Say(caller, CarInsurance.Instance.Translate("fail"));
            }  
        }
    }
}
