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
        private Menu _menu = new Menu("Admin Panel");
        private Player _player;
        public AdminPanel()
        {
            _player = new Player();
            Tick += Draw;
            API.RegisterCommand("pos", new Action(GetPosition), false);
            API.RegisterCommand("godmode", new Action(GodMode), false);
            API.RegisterCommand("destroy", new Action(DestroyVehicle), false);
            API.RegisterCommand("teleport", new Action<int, List<object>, string>(Teleport), false);
            API.RegisterCommand("removeweps", new Action(RemoveAllWeapons), false);
            //API.RegisterCommand("addwep", new Action<int, List<object>, string>(AddWeapon), false);
            API.RegisterCommand("spawn", new Action<int, List<object>, string>(SpawnVehicle), false);

            int id = _menu.CreateSubmenu("Weapons", new Button[]
            {
                new Button("Add Weapon", AddWeapon),
                new Button("Remove All Weapons", RemoveAllWeapons)

            });

            //_menu.AddSubmenu(id, "All Weapons", new Action[]
            //{
            //    new Action(AddWeapon),
            //    ()=> AddWeapon(WeaponHash.RevolverSchofield),
            //    ()=> AddWeapon(WeaponHash.RepeaterWinchesterJohn),
            //    ()=> AddWeapon(WeaponHash.ShotgunPump),
            //    ()=> AddWeapon(WeaponHash.RifleVarmint),
            //});
      
        }

        private void SpawnVehicle(int id, List<object> args, string raw)
        {

        }

        private static void DestroyVehicle()
        {
            //Ped player = Game.PlayerPed;
            //if (player.IsInVehicle()) player.CurrentVehicle.Delete();
           
        }

        public void RemoveAllWeapons()
        {
            _player.Ped.RemoveAllWeapons();
        }

        private void AddWeapon()
        {
            _player.Ped.GiveWeapon(WeaponHash.RevolverCattleman, 100);
        }

        private void AddWeapon(WeaponHash hash, int ammo = 200)
        {
            _player.Ped.GiveWeapon(hash, ammo);
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

        public async Task Draw()
        {
           _menu.Draw();
        }


       
    }
}
