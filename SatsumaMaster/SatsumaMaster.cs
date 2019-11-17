using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace SatsumaMaster
{
    public class SatsumaMaster : Mod
    {
        public override string ID => "Satsuma Master"; //Your mod ID (unique)
        public override string Name => "Satsuma Master"; //You mod name
        public override string Author => "N0tiC"; //Your Username
        public override string Version => "1.6"; //Version

        public override bool UseAssetsFolder => true; //true, if you will be load custom assets
        public override bool LoadInMenu => false; //Load this mod in Main Menu. (in most cases should be FALSE, use only if are SURE that you need this). Do not abuse.

        string minMSCLoaderVersion = "1.1.4";
        bool versionOK = false;
        bool setupOK = false;

        //Gradient Text
        Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        //Mod References
        public GameObject _satsuma;
        GameObject player;
        public Drivetrain _satsumaDriveTrain;

        //Power Boost
        public float powerMultiplier = 1f;
        public float defaultRPM;
        public float defaultShiftUpRPM;
        public float defaultShiftDownRPM;

        //Seperated Mods
        public UIHandler uiHandler;
        public Teleport teleportMod;
        public SoundController soundControllerMod;
        public SuspensionWheelController wheelSuspensionMod;
        public SixGears sixGearsMod;
        public EngineFixer engineFixerMod;

        //Mod Controller (Need satsuma)
        public bool enableBoost, enableEngineFixer, enableWheelMod;
        public bool boost, engineFixer, sixGears;
        public bool fixABS, fixESP, fixDrivetype;

        //Keybinds
        Keybind ShowUIBind = new Keybind("powerBind", "Satsuma Master Menu", KeyCode.L);

        public override void OnLoad()
        {
            if (ModLoader.MSCLoader_Ver.ToString() != minMSCLoaderVersion)
                versionOK = false;
            else
                versionOK = true;

            if (versionOK)
            {
                ModConsole.Print("Passed MSCLoader version check. Setting up...");
                try
                {

                    _satsuma = GameObject.Find("SATSUMA(557kg, 248)");
                    player = GameObject.Find("PLAYER");

                    // Called once, when mod is loading after game is fully loaded
                    AssetBundle ab = LoadAssets.LoadBundle(this, "satsumamaster.unity3d");

                    uiHandler = new UIHandler(this, ab);

                    SetupGradient();
                    uiHandler.ToggleStats();
                    uiHandler.ToggleXFSPanel();
                    uiHandler.ToggleTPPanel();
                    uiHandler.ShowUI();

                    soundControllerMod = new SoundController(this, ab);

                    ab.Unload(false);

                    wheelSuspensionMod = new SuspensionWheelController(this);
                    teleportMod = new Teleport(player, _satsuma);
                    sixGearsMod = new SixGears(this);
                    engineFixerMod = new EngineFixer(this);

                    if (_satsuma != null)
                    {
                        _satsumaDriveTrain = _satsuma.GetComponent<Drivetrain>();
                        if (_satsumaDriveTrain != null)
                        {
                            defaultRPM = _satsumaDriveTrain.maxRPM;
                            defaultShiftUpRPM = _satsumaDriveTrain.shiftUpRPM;
                            defaultShiftDownRPM = _satsumaDriveTrain.shiftDownRPM;
                            sixGearsMod.oldRatio = _satsumaDriveTrain.gearRatios;
                            enableBoost = true;

                            engineFixerMod.OptimizeEngineFixer();
                        }
                    }

                    ModConsole.Print(Name + ": Success!");

                    setupOK = true;

                    Keybind.Add(this, ShowUIBind);

                }
                catch (Exception e)
                {
                    ModConsole.Error("Asset load and setup failed.");
                    return;
                }
            }
        }
        
        void SetupGradient()
        {
            gradient = new Gradient();
            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[3];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 0.5f;
            colorKey[2].color = new Color(0f, .6f, .1f, 1f);
            colorKey[2].time = 1.0f;
            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            gradient.SetKeys(colorKey, alphaKey);
        }

        /*
         * Apparently I cant change the values inside EngineBoost from Update function. This worked though!
         */
        public override void FixedUpdate()
        {
            if (versionOK && setupOK)
                EngineBoost(boost);
        }

        void EngineBoost(bool state)
        {
            if (state)
                if (FsmVariables.GlobalVariables.FindFsmString("PlayerCurrentVehicle").Value == "Satsuma")
                    _satsumaDriveTrain.powerMultiplier = powerMultiplier;
                else
                    _satsumaDriveTrain.powerMultiplier = 1f;
        }

        public override void Update()
        {
            if (versionOK && setupOK)
            {
                if (fixABS)
                    if (!_satsuma.GetComponent<CarController>().ABS)
                        _satsuma.GetComponent<CarController>().ABS = true;

                if (fixESP)
                    if (!_satsuma.GetComponent<CarController>().ESP)
                        _satsuma.GetComponent<CarController>().ESP = true;

                if (fixDrivetype)
                    if (_satsumaDriveTrain.transmission != sixGearsMod.lastDrivetype)
                        _satsumaDriveTrain.SetTransmission(sixGearsMod.lastDrivetype);

                if (sixGears)
                    if (_satsumaDriveTrain.gearRatios != sixGearsMod.newRatio)
                    {
                        _satsumaDriveTrain.gearRatios = sixGearsMod.newRatio;
                        _satsumaDriveTrain.maxRPM = 8400f;
                    }

                if (sixGearsMod.autoTransEnabled)
                    if (_satsumaDriveTrain.automatic)
                    {
                        _satsumaDriveTrain.automatic = true;
                        _satsumaDriveTrain.autoReverse = false; // Force autoReverse off.
                    }

                if (boost)
                {
                    _satsumaDriveTrain.canStall = false;

                    if (_satsumaDriveTrain.powerMultiplier != powerMultiplier)
                        _satsumaDriveTrain.powerMultiplier = powerMultiplier;
                }


                if (ShowUIBind.IsDown())
                    uiHandler.ShowUI();

                if (uiHandler.rootUI.activeSelf)
                    uiHandler.boostPowerText.text = Math.Round(powerMultiplier, 2).ToString();

                if (engineFixer)
                {
                    if (engineFixerMod._wearAlternator != null)
                        if (CalcPercentage(engineFixerMod._wearAlternator.Value, 100f) < 30)
                            engineFixerMod._wearAlternator.Value = 100f;

                    if (engineFixerMod._wearCrankshaft != null)
                        if (CalcPercentage(engineFixerMod._wearCrankshaft.Value, 100f) < 30)
                            engineFixerMod._wearCrankshaft.Value = 100f;

                    if (engineFixerMod._wearGearbox != null)
                        if (CalcPercentage(engineFixerMod._wearGearbox.Value, 100f) < 30)
                            engineFixerMod._wearGearbox.Value = 100f;

                    if (engineFixerMod._wearHeadgasket != null)
                        if (CalcPercentage(engineFixerMod._wearHeadgasket.Value, 100f) < 30)
                            engineFixerMod._wearHeadgasket.Value = 100f;

                    if (engineFixerMod._wearPiston1 != null)
                        if (CalcPercentage(engineFixerMod._wearPiston1.Value, 100f) < 30)
                            engineFixerMod._wearPiston1.Value = 100f;

                    if (engineFixerMod._wearPiston2 != null)
                        if (CalcPercentage(engineFixerMod._wearPiston2.Value, 100f) < 30)
                            engineFixerMod._wearPiston2.Value = 100f;

                    if (engineFixerMod._wearPiston3 != null)
                        if (CalcPercentage(engineFixerMod._wearPiston3.Value, 100f) < 30)
                            engineFixerMod._wearPiston3.Value = 100f;

                    if (engineFixerMod._wearPiston4 != null)
                        if (CalcPercentage(engineFixerMod._wearPiston4.Value, 100f) < 30)
                            engineFixerMod._wearPiston4.Value = 100f;

                    if (engineFixerMod._wearRockershaft != null)
                        if (CalcPercentage(engineFixerMod._wearRockershaft.Value, 100f) < 30)
                            engineFixerMod._wearRockershaft.Value = 100f;

                    if (engineFixerMod._wearStarter != null)
                        if (CalcPercentage(engineFixerMod._wearStarter.Value, 100f) < 30)
                            engineFixerMod._wearStarter.Value = 100f;

                    if (engineFixerMod._wearWaterpump != null)
                        if (CalcPercentage(engineFixerMod._wearWaterpump.Value, 100f) < 30)
                            engineFixerMod._wearWaterpump.Value = 100f;

                    if (engineFixerMod._oilLevel != null)
                        if (CalcPercentage(engineFixerMod._oilLevel.Value, 3f) < 30)
                            engineFixerMod._oilLevel.Value = 3f;

                    if (engineFixerMod._satsumaFuel != null)
                        if (CalcPercentage(engineFixerMod._satsumaFuel.Value, 36f) < 30)
                            engineFixerMod._satsumaFuel.Value = 36f;

                    if (engineFixerMod._racingWater != null)
                        if (CalcPercentage(engineFixerMod._racingWater.Value, 7f) < 30)
                            engineFixerMod._racingWater.Value = 7f;

                    if (engineFixerMod._brakeFluidF != null)
                        if (CalcPercentage(engineFixerMod._brakeFluidF.Value, 1f) < 30)
                            engineFixerMod._brakeFluidF.Value = 1f;

                    if (engineFixerMod._brakeFluidR != null)
                        if (CalcPercentage(engineFixerMod._brakeFluidR.Value, 1f) < 30)
                            engineFixerMod._brakeFluidR.Value = 1f;

                    if (engineFixerMod._cluthFluid != null)
                        if (CalcPercentage(engineFixerMod._cluthFluid.Value, .5f) < 30)
                            engineFixerMod._cluthFluid.Value = .5f;

                    if (engineFixerMod._engineTemp != null)
                        if (engineFixerMod._engineTemp.Value < 83f)
                            engineFixerMod._engineTemp = 80f;
                }

                if (soundControllerMod.sfx && _satsumaDriveTrain.gear != 0 && _satsumaDriveTrain.gear != 1 && _satsumaDriveTrain.gear != sixGearsMod.rememberGear && _satsumaDriveTrain.gear > sixGearsMod.rememberGear && _satsumaDriveTrain.rpm > 3500f && Mathf.Round(_satsumaDriveTrain.differentialSpeed) > 30f)
                {
                    soundControllerMod._bovSound.GetComponent<AudioSource>().PlayOneShot(soundControllerMod._sfxSounds[soundControllerMod.bovChoice], .1f); // We play the BOV sound with the volume.
                    sixGearsMod.rememberGear = _satsumaDriveTrain.gear; // We get the current gear and store it as the last used gear.
                }
                else
                    sixGearsMod.rememberGear = _satsumaDriveTrain.gear; // We get the new gear as the last remembered.

                if (uiHandler.statsPanel.activeSelf)
                {
                    //Add all text updates...
                    uiHandler.currentHP.text = "Current Power: " + Mathf.Round(_satsumaDriveTrain.currentPower).ToString();
                    uiHandler.currentSpeed.text = "Speed: " + Mathf.Round(_satsumaDriveTrain.differentialSpeed).ToString();
                    uiHandler.currentRPM.text = "RPM: " + Mathf.Round(_satsumaDriveTrain.rpm).ToString();
                    uiHandler.currentEngineTemp.text = "Engine temp: " + Mathf.Round(engineFixerMod._engineTemp.Value).ToString();

                    if (enableEngineFixer)
                    {
                        uiHandler.wearAlternator.text = "Alternator: " + WearStatus(uiHandler.wearAlternator, engineFixerMod._wearAlternator, 100f);
                        uiHandler.wearCrankshaft.text = "Crankshaft: " + WearStatus(uiHandler.wearCrankshaft, engineFixerMod._wearCrankshaft, 100f);
                        uiHandler.wearGearbox.text = "Gearbox: " + WearStatus(uiHandler.wearGearbox, engineFixerMod._wearGearbox, 100f);
                        uiHandler.wearHeadgasket.text = "Headgasket: " + WearStatus(uiHandler.wearHeadgasket, engineFixerMod._wearHeadgasket, 100f);
                        uiHandler.wearPiston1.text = "Piston1: " + WearStatus(uiHandler.wearPiston1, engineFixerMod._wearPiston1, 100f);
                        uiHandler.wearPiston2.text = "Piston2: " + WearStatus(uiHandler.wearPiston2, engineFixerMod._wearPiston2, 100f);
                        uiHandler.wearPiston3.text = "Piston3: " + WearStatus(uiHandler.wearPiston3, engineFixerMod._wearPiston3, 100f);
                        uiHandler.wearPiston4.text = "Piston4: " + WearStatus(uiHandler.wearPiston4, engineFixerMod._wearPiston4, 100f);
                        uiHandler.wearRockershaft.text = "Rockershaft: " + WearStatus(uiHandler.wearRockershaft, engineFixerMod._wearRockershaft, 100f);
                        uiHandler.wearStarter.text = "Starter: " + WearStatus(uiHandler.wearStarter, engineFixerMod._wearStarter, 100f);
                        uiHandler.wearWaterpump.text = "Waterpump: " + WearStatus(uiHandler.wearWaterpump, engineFixerMod._wearWaterpump, 100f);

                        uiHandler.oilLevel.text = "Oil: " + WearStatus(uiHandler.oilLevel, engineFixerMod._oilLevel, 3f);
                        uiHandler.satsumaFuel.text = "Fuel: " + WearStatus(uiHandler.satsumaFuel, engineFixerMod._satsumaFuel, 36f);
                        if (engineFixerMod._racingWater != null)
                            uiHandler.racingWater.text = "Water: " + WearStatus(uiHandler.racingWater, engineFixerMod._racingWater, 7f);
                        uiHandler.brakeFluidF.text = "Brake F: " + WearStatus(uiHandler.brakeFluidF, engineFixerMod._brakeFluidF, 1f);
                        uiHandler.brakeFluidR.text = "Brake R: " + WearStatus(uiHandler.brakeFluidR, engineFixerMod._brakeFluidR, 1f);
                        uiHandler.cluthFluid.text = "Clutch F: " + WearStatus(uiHandler.cluthFluid, engineFixerMod._cluthFluid, .5f);
                    }

                }
            }

            string WearStatus(Text obj, FsmFloat wear, float max)
            {
                string returnString = "";
                int _wearPercentage = CalcPercentage(wear.Value, max);
                int _colorFixer = 0;
                if (_wearPercentage > 99)
                    _colorFixer = 9;
                else
                    if (_wearPercentage.ToString().Contains("-"))
                    _colorFixer = 0;
                else
                    _colorFixer = _wearPercentage;

                if (_wearPercentage.ToString().Contains("-"))
                    returnString = "UNACCESSABLE";
                else
                    returnString = _wearPercentage.ToString() + "%";

                obj.color = ColorFromGradient(float.Parse(_colorFixer.ToString().Insert(0, "0.")));

                return returnString;
            }

            Color ColorFromGradient(float value)  // float between 0-1
            {
                return gradient.Evaluate(value);
            }

            int CalcPercentage(float val, float max)
            {
                int value = Mathf.RoundToInt((val / max) * 100f);
                return value;
            }
        }
    }
}

