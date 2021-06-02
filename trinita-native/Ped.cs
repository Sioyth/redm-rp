
using CitizenFX.Core.Native;

namespace Trinita.Core.Native
{
    public class Ped : Entity
    {
        protected int _id;

        //public Gender Gender => Function.Call<bool>(Hash.IS_PED_MALE, Handle) ? Gender.Male : Gender.Female;
        public bool IsJumping => Function.Call<bool>(Hash.IS_PED_JUMPING, _id);
        public bool IsInMeleeCombat => Function.Call<bool>(Hash.IS_PED_IN_MELEE_COMBAT, _id);
        public bool IsInCombat => Function.Call<bool>(Hash.IS_PED_IN_COMBAT, _id);
        public bool IsClimbing => Function.Call<bool>(Hash.IS_PED_CLIMBING, _id);
        public bool IsPlayer => Function.Call<bool>(Hash.IS_PED_A_PLAYER, _id);
        public bool IsHuman => Function.Call<bool>(Hash.IS_PED_HUMAN, _id);
        public bool IsFleeing => Function.Call<bool>(Hash.IS_PED_FLEEING, _id);
        public bool IsGettingUp => Function.Call<bool>(Hash.IS_PED_GETTING_UP, _id);
        public bool IsGettingIntoVehicle => Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, _id);
        public bool IsInVehicle => Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, _id);
        public bool IsOnFoot => Function.Call<bool>(Hash.IS_PED_ON_FOOT, _id);
        public bool IsOnMount => Function.Call<bool>(Hash.IS_PED_ON_MOUNT, _id);

        //public Vehicle CurrentVehicle => (Vehicle)FromHandle(Function.Call<int>(Hash.GET_VEHICLE_PED_IS_IN, Handle, false));
        //public Vehicle LastVehicle => (Vehicle)FromHandle(Function.Call<int>(Hash.GET_VEHICLE_PED_IS_IN, Handle, true));

        public int ID { get => _id; set => _id = value; }

        public Ped(int id) 
        {
            //TODO: API.CreatePed()
            _id = id; 
        }

        public void GiveWeapon(WeaponHash hash, int ammo = 0, bool equipNow = false, bool isLeftHanded = false)
        {
            if (!Function.Call<bool>(Hash.IS_WEAPON_VALID, hash)) return;
            Function.Call<bool>(Hash._GIVE_WEAPON_TO_PED_2, _id, hash, ammo, equipNow, true, 1, false, 0.5f, 1.0f, 752097756, isLeftHanded, 0.0f, false);
        }

        public void RemoveAllWeapons()
        {
            Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, _id, true, true);
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
