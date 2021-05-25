
using CitizenFX.Core.Native;

namespace Trinita.Core.Native
{
    public class Ped : Entity
    {
        protected int _id;
        public int ID { get => _id; set => _id = value; }

        public Ped() 
        {
            //TODO: API.CreatePed()
            _id = 0; 
        }

        public void GiveWeapon(string weapon, int ammo = 0, bool equipNow = false)
        {
            uint hash = (uint)API.GetHashKey(weapon);
            if (!Function.Call<bool>(Hash.IS_WEAPON_VALID, hash)) return;

            Function.Call<bool>(Hash._GIVE_WEAPON_TO_PED_2, _id, hash, ammo, equipNow, true, 1, false, 0.5f, 1.0f, 0, false, 0.0f, false);
        }

        public void Freeze(bool freeze)
        {
            if (!freeze)
            {
                if (!API.IsEntityVisible(_id)) API.SetEntityVisible(_id, true);
                if (!API.IsPedInAnyVehicle(_id, true)) API.SetEntityCollision(_id, true, true);
                API.FreezeEntityPosition(_id, false);
            }
            else
            {
                if (API.IsEntityVisible(_id)) API.SetEntityVisible(_id, false);
                API.SetEntityCollision(_id, false, true);
                API.FreezeEntityPosition(_id, true);
                if (API.IsPedFatallyInjured(_id)) API.ClearPedTasksImmediately(_id, 0, 0);
            }
        }
    }
}
