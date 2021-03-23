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

            ushort vehicle = CarInsurance.Instance.PlayerDeservesInsurance(client.CSteamID);

            if(vehicle > 0)
            {
                VehicleTool.giveVehicle(client.Player, vehicle);
                UnturnedChat.Say(caller, "Successfully got your car back from the insurance!");
            }
            else
            {
                UnturnedChat.Say(caller, "You don't have any insurance.");
            }  
        }
    }
}
