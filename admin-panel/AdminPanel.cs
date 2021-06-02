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
            API.RegisterCommand("godmode", new Action(GodMode), false);;
            API.RegisterCommand("teleport", new Action<int, List<object>, string>(Teleport), false);
            API.RegisterCommand("removeweps", new Action(RemoveAllWeapons), false);
            API.RegisterCommand("spawn", new Action<int, List<object>, string>(SpawnVehicle), false);

            int weaponsID = _menu.CreateSubmenu("Weapons", new Button[]
            {
                new Button("Remove All Weapons", RemoveAllWeapons)

            });

            _menu.AddSubmenu(weaponsID, "All Weapons", new Button[]
            {
                 new Button("RevolverSchofield", ()=> AddWeapon(WeaponHash.RevolverSchofield)),
                 new Button("RepeaterWinchesterJohn", ()=> AddWeapon(WeaponHash.RepeaterWinchesterJohn)),
                 new Button("ShotgunPump", ()=> AddWeapon(WeaponHash.ShotgunPump)),
                 new Button("RifleVarmint", ()=> AddWeapon(WeaponHash.RifleVarmint)),
            });

        }

        private void SpawnVehicle(int id, List<object> args, string raw)
        {

        }

        private static void DestroyVehicle()
        {
           
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

        bool open = false;
        public async Task Draw()
        {
            if (Input.JustPressed(1, Control.OpenSatchelHorseMenu))
            {
                open = !open;
                _menu.Closed = false;
            }
            if(open) _menu.Draw();
            await Task.FromResult(0);
        }


       
    }
}
