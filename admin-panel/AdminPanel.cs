using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;

namespace Doom
{
    public class AdminPanel : BaseScript
    {
        public AdminPanel()
        {
            API.RegisterCommand("godmode", new Action(GodMode), false);
            API.RegisterCommand("position", new Action(GetPosition), false);
            API.RegisterCommand("destroy", new Action(DestroyVehicle), false);
            API.RegisterCommand("spawn", new Action<int, List<object>, string>(SpawnVehicle), false);
        }

        private static async void SpawnVehicle(int id, List<object> args, string raw)
        {
            string model = "";
            if (args.Count > 0) model = args[0].ToString();
            
            //Ped player = Game.PlayerPed;
            uint hash = (uint)API.GetHashKey(model);
            if (!API.IsModelInCdimage(hash) || !API.IsModelAVehicle(hash)) return;
           // if (player.IsInVehicle()) player.CurrentVehicle.Delete();

            //Vehicle vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);
            //player.SetIntoVehicle(vehicle, VehicleSeat.Driver);
        }

        private static void DestroyVehicle()
        {
            //Ped player = Game.PlayerPed;
            //if (player.IsInVehicle()) player.CurrentVehicle.Delete();
        }

        private static void AddWeapon(int id, List<object> args, string raw)
        {
           // Ped player = Game.PlayerPed;
        }

        private static void GodMode()
        {
           // Ped player = Game.PlayerPed;
            //player.IsInvincible = !player.IsInvincible;

            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                //args = new[] { "[Godmode]", $"{(player.IsInvincible ? "On" : "Off")}" }
            });
        }

        private static void GetPosition()
        {
           //Ped player = Game.PlayerPed;

            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                //args = new[] { "[Position]", $"{player.Position}" }
            });
        }
    }
}
