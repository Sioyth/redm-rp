using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrinitaRP
{
    public class AdminPanel : BaseScript
    {
        public AdminPanel()
        {

            API.RegisterCommand("pos", new Action(GetPosition), false);
            API.RegisterCommand("godmode", new Action(GodMode), false);
            API.RegisterCommand("destroy", new Action(DestroyVehicle), false);
            API.RegisterCommand("teleport", new Action<int, List<object>, string>(Teleport), false);
            API.RegisterCommand("addwep", new Action<int, List<object>, string>(AddWeapon), false);
            API.RegisterCommand("spawn", new Action<int, List<object>, string>(SpawnVehicle), false);
        }

        private async Task<bool> LoadModel(uint hash)   
        {
            if (!API.IsModelAVehicle(hash)) return false;

            API.RequestModel(hash, false);
            //while(!API.HasModelLoaded(hash))
            //{
            //    Debug.WriteLine("Waiting for model to load!");
            //    await Delay(100);
            //}

            return true;
        }

        private void SpawnVehicle(int id, List<object> args, string raw)
        {
            string model = "";
            if (args.Count > 0) model = args[0].ToString();
            
            int ped = API.PlayerPedId();
            uint hash = (uint)API.GetHashKey(model);
            Vector3 coords = API.GetEntityCoords(ped, false, false);
            if(LoadModel(hash).Result) return;
            _ = LoadModel(hash);
            //if (API.IsPedInAnyVehicle(ped, false))


            int vehicle = API.CreateVehicle(hash, coords.X, coords.Y, coords.Z, API.GetEntityHeading(ped), false, false, false, false);
            API.GetPedInVehicleSeat(vehicle, 0);

        }

        private static void DestroyVehicle()
        {
            //Ped player = Game.PlayerPed;
            //if (player.IsInVehicle()) player.CurrentVehicle.Delete();
        }

        private static void AddWeapon(int id, List<object> args, string raw)
        {
            int ped = API.PlayerPedId();

            uint hash = (uint)API.GetHashKey(args[0].ToString());
            if (!API.IsWeaponValid(hash))
            {
                Debug.WriteLine("Weapon not valid!");
                return;
            }
            uint weaponGroup = (uint)API.GetWeapontypeGroup(hash);
            Debug.WriteLine(weaponGroup.ToString());
            API.GiveWeaponToPed_2(ped, hash, 100, false, true, 1, false, 0.5f, 1.0f, 0, false, 0.0f, false);
            //uint GiveWeaponToPed(int ped, uint weaponHash, int ammoCount, bool bForceInHand, bool bForceInHolster, int attachPoint, bool bAllowMultipleCopies, float p7, float p8, uint addReason, bool bIgnoreUnlocks, float p11, bool p12)
        }

        private static void GodMode()
        {
            int id = API.PlayerId();
            API.SetPlayerInvincible(id, !API.GetPlayerInvincible(id));
            //player.IsInvincible = !player.IsInvincible;

            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                args = new[] { "[Godmode]", $"{(API.GetPlayerInvincible(id) ? "On" : "Off")}" }
            });
        }

        private void GetPosition()
        {
            Vector3 pos = API.GetEntityCoords(API.PlayerPedId(), false, false);
            Debug.WriteLine(pos.X + ", " + pos.Y + ", " + pos.Z);
        }

        private void Teleport(int id, List<object> args, string raw)
        {
            if (args.Count < 3) return;

            Vector3 pos = new Vector3(float.Parse(args[0].ToString()), float.Parse(args[1].ToString()), float.Parse(args[2].ToString()));
            API.SetEntityCoordsNoOffset(API.PlayerPedId(), pos.X, pos.Y, pos.Z, false, false, false);
        }

       
    }
}
