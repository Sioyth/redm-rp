using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinita.Core.Native;

namespace TrinitaRP
{
    public class AdminPanel : BaseScript
    {
        private Player _player;
        public AdminPanel()
        {
            _player = new Player();
            API.RegisterCommand("pos", new Action(GetPosition), false);
            API.RegisterCommand("godmode", new Action(GodMode), false);
            API.RegisterCommand("destroy", new Action(DestroyVehicle), false);
            API.RegisterCommand("teleport", new Action<int, List<object>, string>(Teleport), false);
            API.RegisterCommand("addwep", new Action<int, List<object>, string>(AddWeapon), false);
            API.RegisterCommand("spawn", new Action<int, List<object>, string>(SpawnVehicle), false);
        }

        private void SpawnVehicle(int id, List<object> args, string raw)
        {

        }

        private static void DestroyVehicle()
        {
            //Ped player = Game.PlayerPed;
            //if (player.IsInVehicle()) player.CurrentVehicle.Delete();
           
        }

        private void AddWeapon(int id, List<object> args, string raw)
        {
            //_player.Ped.GiveWeapon(args[0].ToString(), int.Parse(args[1].ToString()));
        }

        private void GodMode()
        {
            _player.SetInvincible(true);
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
            _player.Teleport(pos);
        }

       
    }
}
