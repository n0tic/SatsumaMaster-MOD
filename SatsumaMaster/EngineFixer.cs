using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace SatsumaMaster
{
    public class EngineFixer
    {
        SatsumaMaster modParent;

        //Engine parts
        public FsmFloat _wearAlternator, _wearCrankshaft, _wearGearbox, _wearHeadgasket, _wearPiston1, _wearPiston2, _wearPiston3, _wearPiston4, _wearRockershaft, _wearStarter, _wearWaterpump, _oilLevel, _satsumaFuel, _racingWater, _brakeFluidF, _brakeFluidR, _cluthFluid, _engineTemp;

        public EngineFixer(SatsumaMaster _modParent) { modParent = _modParent; }

        public void OptimizeEngineFixer()
        {
            if (modParent._satsuma != null)
            {
                PlayMakerFSM[] array = Resources.FindObjectsOfTypeAll<PlayMakerFSM>();
                foreach (PlayMakerFSM val in array)
                {
                    Func<string, FsmFloat> func = val.FsmVariables.FindFsmFloat;
                    if (val.FsmVariables.FindFsmFloat("WearAlternator") != null && val.FsmVariables.FindFsmFloat("WearGearbox") != null && val.FsmVariables.FindFsmFloat("WearCrankshaft") != null && func("WearHeadgasket") != null && func("WearPiston1") != null && func("WearPiston2") != null && func("WearPiston3") != null && func("WearPiston4") != null && func("WearRockershaft") != null && func("WearStarter") != null && func("WearWaterpump") != null)
                    {
                        _wearAlternator = val.FsmVariables.FindFsmFloat("WearAlternator");
                        _wearCrankshaft = val.FsmVariables.FindFsmFloat("WearCrankshaft");
                        _wearGearbox = val.FsmVariables.FindFsmFloat("WearGearbox");
                        _wearHeadgasket = val.FsmVariables.FindFsmFloat("WearHeadgasket");
                        _wearPiston1 = val.FsmVariables.FindFsmFloat("WearPiston1");
                        _wearPiston2 = val.FsmVariables.FindFsmFloat("WearPiston2");
                        _wearPiston3 = val.FsmVariables.FindFsmFloat("WearPiston3");
                        _wearPiston4 = val.FsmVariables.FindFsmFloat("WearPiston4");
                        _wearRockershaft = val.FsmVariables.FindFsmFloat("WearRockershaft");
                        _wearStarter = val.FsmVariables.FindFsmFloat("WearStarter");
                        _wearWaterpump = val.FsmVariables.FindFsmFloat("WearWaterpump");
                    }

                    if (val.gameObject.transform.root.name == "Database")
                    {
                        if (val.gameObject.name == "FuelTank")
                        {
                            _satsumaFuel = val.FsmVariables.FindFsmFloat("FuelLevel");
                        }
                        if (val.gameObject.name == "Oilpan")
                        {
                            _oilLevel = val.FsmVariables.FindFsmFloat("Oil");
                        }
                        if (val.gameObject.name == "Racing Radiator")
                        {
                            _racingWater = val.FsmVariables.FindFsmFloat("Water");
                        }
                        if (val.gameObject.name == "BrakeMasterCylinder")
                        {
                            _brakeFluidF = val.FsmVariables.FindFsmFloat("BrakeFluidF");
                            _brakeFluidR = val.FsmVariables.FindFsmFloat("BrakeFluidR");
                        }
                        if (val.gameObject.name == "ClutchMasterCylinder")
                        {
                            _cluthFluid = val.FsmVariables.FindFsmFloat("ClutchFluid");
                        }
                    }
                }

                _engineTemp = FsmVariables.GlobalVariables.FindFsmFloat("EngineTemp");

                modParent.enableEngineFixer = true;
            }
            else
                return;
        }
    }

    
}
