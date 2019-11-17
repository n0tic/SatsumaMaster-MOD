using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MSCLoader;

namespace SatsumaMaster
{
    public class UIHandler
    {
        SatsumaMaster modParent;

        //Setup assets
        AssetBundle ab;

        //Mod UI
        public GameObject rootUI;
        public GameObject statsPanel, sfxPanel, teleportPanel;
        public Text currentHP, currentSpeed, currentRPM, boostPowerText, currentEngineTemp;
        public Text wearAlternator, wearCrankshaft, wearGearbox, wearHeadgasket, wearPiston1, wearPiston2, wearPiston3, wearPiston4, wearRockershaft, wearStarter, wearWaterpump;
        public Text oilLevel, satsumaFuel, racingWater, brakeFluidF, brakeFluidR, cluthFluid;
        public Text bovVol;
        public Button statsButton, sfxButton, tpButton, boostButton, boostMinusButton, boostPlusButton, engineFixerButton, eightGearButton, transTypeButton, driveTypeButton, espButton, absButton, suspensionbutton, upButton, downButton, rightButton, leftButton, inButton, outButton, bovButton, bovMinusButton, bovPlusButton, bovSFXButton;
        public Button carHome, carMech, carHighway, carShop, carStrip, carPoker, carStrawberry, carGrandma, carSki, playerHome, playerMech, playerHighway, playerShop, playerStrip, playerPoker, playerStrawberry, playerGrandma, playerCottage, playerCar;
    
        public UIHandler (SatsumaMaster _modParent, AssetBundle assets)
        {
            modParent = _modParent;

            try
            {
                GameObject UI = assets.LoadAsset("SatsumaMastser.prefab") as GameObject;
                rootUI = GameObject.Instantiate(UI);

                statsPanel = GameObject.Find("StatsPanel");
                sfxPanel = GameObject.Find("SFXPanel");
                teleportPanel = GameObject.Find("TPPanel");

                currentHP = GameObject.Find("CurrentPowerText").GetComponent<Text>();
                currentSpeed = GameObject.Find("CurrentSpeedText").GetComponent<Text>();
                currentRPM = GameObject.Find("CurrentRPMText").GetComponent<Text>();
                currentEngineTemp = GameObject.Find("CurrentEngineTempText").GetComponent<Text>();

                wearAlternator = GameObject.Find("AltinatorWearText").GetComponent<Text>();
                wearCrankshaft = GameObject.Find("CrankSharftWearText").GetComponent<Text>();
                wearGearbox = GameObject.Find("GearBoxWearText").GetComponent<Text>();
                wearHeadgasket = GameObject.Find("HeadGasketWearText").GetComponent<Text>();
                wearPiston1 = GameObject.Find("Piston1WearText").GetComponent<Text>();
                wearPiston2 = GameObject.Find("Piston2WearText").GetComponent<Text>();
                wearPiston3 = GameObject.Find("Piston3WearText").GetComponent<Text>();
                wearPiston4 = GameObject.Find("Piston4WearText").GetComponent<Text>();
                wearRockershaft = GameObject.Find("RockerShaftWearText").GetComponent<Text>();
                wearStarter = GameObject.Find("StarterWearText").GetComponent<Text>();
                wearWaterpump = GameObject.Find("WaterPumpWearText").GetComponent<Text>();
                oilLevel = GameObject.Find("OilLevelText").GetComponent<Text>();
                satsumaFuel = GameObject.Find("FuelLevelText").GetComponent<Text>();
                racingWater = GameObject.Find("WaterLevelText").GetComponent<Text>();
                brakeFluidF = GameObject.Find("BreakFluidFText").GetComponent<Text>();
                brakeFluidR = GameObject.Find("BreakFluidRText").GetComponent<Text>();
                cluthFluid = GameObject.Find("ClutchFluidText").GetComponent<Text>();
                boostPowerText = GameObject.Find("BoostPowerText").GetComponent<Text>();

                statsButton = GameObject.Find("ShowStatsButton").GetComponent<Button>();
                sfxButton = GameObject.Find("SFXButton").GetComponent<Button>();
                tpButton = GameObject.Find("TPButton").GetComponent<Button>();
                boostButton = GameObject.Find("BoostButton").GetComponent<Button>();
                boostMinusButton = GameObject.Find("MinusButton").GetComponent<Button>();
                boostPlusButton = GameObject.Find("PlusButton").GetComponent<Button>();
                engineFixerButton = GameObject.Find("EngineFixerButton").GetComponent<Button>();
                eightGearButton = GameObject.Find("GearBoxButton").GetComponent<Button>();
                transTypeButton = GameObject.Find("TransmissionButton").GetComponent<Button>();
                driveTypeButton = GameObject.Find("DrivetypeButton").GetComponent<Button>();
                espButton = GameObject.Find("ESPButton").GetComponent<Button>();
                absButton = GameObject.Find("ABSButton").GetComponent<Button>();

                suspensionbutton = GameObject.Find("SuspensionButton").GetComponent<Button>();
                rightButton = GameObject.Find("SRightButton").GetComponent<Button>();
                leftButton = GameObject.Find("SLeftButton").GetComponent<Button>();
                upButton = GameObject.Find("SUpButton").GetComponent<Button>();
                downButton = GameObject.Find("SDownButton").GetComponent<Button>();
                inButton = GameObject.Find("_CamberIn").GetComponent<Button>();
                outButton = GameObject.Find("_CamberOut").GetComponent<Button>();

                bovButton = GameObject.Find("BovSoundButton").GetComponent<Button>();
                bovMinusButton = GameObject.Find("MinusButton2").GetComponent<Button>();
                bovPlusButton = GameObject.Find("PlusButton2").GetComponent<Button>();
                bovVol = GameObject.Find("BovSoundVolume").GetComponent<Text>();
                bovSFXButton = GameObject.Find("BovSoundSFXButton").GetComponent<Button>();

                carHome = GameObject.Find("tp_CarHome").GetComponent<Button>();
                carMech = GameObject.Find("tp_CarMechanic").GetComponent<Button>();
                carHighway = GameObject.Find("tp_CarHighway").GetComponent<Button>();
                carShop = GameObject.Find("tp_CarToShop").GetComponent<Button>();
                carStrip = GameObject.Find("tp_CarToStrip").GetComponent<Button>();
                carPoker = GameObject.Find("tp_CarToPoker").GetComponent<Button>();
                carStrawberry = GameObject.Find("tp_CarToBerry").GetComponent<Button>();
                carGrandma = GameObject.Find("tp_CarToGrandma").GetComponent<Button>();
                carSki = GameObject.Find("tp_CarToSki").GetComponent<Button>();

                playerHome = GameObject.Find("tp_PlayerToHome").GetComponent<Button>();
                playerMech = GameObject.Find("tp_PlayerToMechanic").GetComponent<Button>();
                playerHighway = GameObject.Find("tp_PlayerToHighway").GetComponent<Button>();
                playerShop = GameObject.Find("tp_PlayerToShop").GetComponent<Button>();
                playerStrip = GameObject.Find("tp_PlayerToStrip").GetComponent<Button>();
                playerPoker = GameObject.Find("tp_PlayerToPoker").GetComponent<Button>();
                playerStrawberry = GameObject.Find("tp_PlayerToBerry").GetComponent<Button>();
                playerGrandma = GameObject.Find("tp_PlayerToGrandma").GetComponent<Button>();
                playerCottage = GameObject.Find("tp_PlayerToCottage").GetComponent<Button>();
                playerCar = GameObject.Find("tp_PlayerToCar").GetComponent<Button>();

                statsButton.onClick.AddListener(delegate { ToggleStats(); });
                sfxButton.onClick.AddListener(delegate { ToggleXFSPanel(); });
                tpButton.onClick.AddListener(delegate { ToggleTPPanel(); });

                boostButton.onClick.AddListener(delegate { ToggleBoost(); });

                boostMinusButton.onClick.AddListener(delegate { ChangePower(-.1f); });
                boostPlusButton.onClick.AddListener(delegate { ChangePower(.1f); });

                engineFixerButton.onClick.AddListener(delegate { ToggleEngineFixer(); });
                eightGearButton.onClick.AddListener(delegate { ToggleSixGears(); });
                transTypeButton.onClick.AddListener(delegate { ToggleTransType(); });
                driveTypeButton.onClick.AddListener(delegate { ToggleDriveType(); });
                espButton.onClick.AddListener(delegate { ToggleESP(); });
                absButton.onClick.AddListener(delegate { ToggleABS(); });

                suspensionbutton.onClick.AddListener(delegate { ToggleSuspensionFix(); });
                rightButton.onClick.AddListener(delegate { modParent.wheelSuspensionMod.SetNewWheelXPositions(IsDirectionLeft: true, IsFrontWheels: true, IsRearWheels: true); });
                leftButton.onClick.AddListener(delegate { modParent.wheelSuspensionMod.SetNewWheelXPositions(IsDirectionLeft: false, IsFrontWheels: true, IsRearWheels: true); });
                upButton.onClick.AddListener(delegate { modParent.wheelSuspensionMod.SetNewWheelPositions(IsDirectionUp: true, IsFrontWheels: true, IsRearWheels: true); });
                downButton.onClick.AddListener(delegate { modParent.wheelSuspensionMod.SetNewWheelPositions(IsDirectionUp: false, IsFrontWheels: true, IsRearWheels: true); });
                inButton.onClick.AddListener(delegate { modParent.wheelSuspensionMod.SetWheelCambers(true, true, 0); });
                outButton.onClick.AddListener(delegate { modParent.wheelSuspensionMod.SetWheelCambers(true, true, 1); });

                bovButton.onClick.AddListener(delegate { ToggleSFX(); });
                bovMinusButton.onClick.AddListener(delegate { ChangeBovVolume(-.1f); });
                bovPlusButton.onClick.AddListener(delegate { ChangeBovVolume(.1f); });
                bovSFXButton.onClick.AddListener(delegate { ToggleBovSoundSFX(); });

                carHome.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Home); });
                carMech.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Mechanic); });
                carHighway.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Highway); });
                carShop.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Shop); });
                carStrip.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Strip); });
                carPoker.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Poker); });
                carStrawberry.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Strawberry); });
                carGrandma.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Grandma); });
                carSki.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Car, Teleport.tpLocation.Ski); });

                playerHome.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Home); });
                playerMech.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Mechanic); });
                playerHighway.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Highway); });
                playerShop.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Shop); });
                playerStrip.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Strip); });
                playerPoker.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Poker); });
                playerStrawberry.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Strawberry); });
                playerGrandma.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Grandma); });
                playerCottage.onClick.AddListener(delegate { modParent.teleportMod.TeleportMode(Teleport.tpType.Player, Teleport.tpLocation.Cottage); });
                //playerCar.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Car); });
            }
            catch (Exception)
            {
                ModConsole.Error("UIHandler failed setup.");
            }
        }

        public void ToggleStats()
        {
            if (statsPanel.activeSelf)
            {
                statsPanel.SetActive(false);
                statsButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Stats & Info";
            }
            else
            {
                if (sfxPanel.activeSelf)
                {
                    sfxPanel.SetActive(false);
                    sfxButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SFX Panel";
                }
                if (teleportPanel.activeSelf)
                {
                    teleportPanel.SetActive(false);
                    tpButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Teleport";
                }

                statsPanel.SetActive(true);
                statsButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] Stats & Info";
            }
        }

        void ToggleBoost()
        {
            if (modParent.enableBoost)
            {
                modParent.boost = !modParent.boost;
                if (modParent.boost)
                {
                    boostButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] Boost power";
                    modParent._satsumaDriveTrain.maxRPM = 8400f;
                    modParent._satsumaDriveTrain.canStall = false;
                    modParent._satsumaDriveTrain.shiftUpRPM = 7000f;
                    modParent._satsumaDriveTrain.shiftDownRPM = 3000f;
                }
                else
                {
                    boostButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Boost power";
                    modParent._satsumaDriveTrain.maxRPM = modParent.defaultRPM;
                    modParent._satsumaDriveTrain.canStall = true;
                    modParent._satsumaDriveTrain.shiftUpRPM = modParent.defaultShiftUpRPM;
                    modParent._satsumaDriveTrain.shiftDownRPM = modParent.defaultShiftDownRPM;
                }
            }
            else
            {
                ModConsole.Error(modParent.Name + ": SatsumaMaster is disabled. Failed setup.");
            }
        }

        public void ToggleXFSPanel()
        {
            if (sfxPanel.activeSelf)
            {
                sfxPanel.SetActive(false);
                sfxButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SFX Panel";
            }
            else
            {
                if (statsPanel.activeSelf)
                {
                    statsPanel.SetActive(false);
                    statsButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Stats & Info";
                }
                if (teleportPanel.activeSelf)
                {
                    teleportPanel.SetActive(false);
                    tpButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Teleport";
                }

                sfxPanel.SetActive(true);
                sfxButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] SFX Panel";
            }
        }

        void ChangePower(float value)
        {
            if (modParent.powerMultiplier > .5f)
                modParent.powerMultiplier += value;
            if (modParent.powerMultiplier < .5f)
                modParent.powerMultiplier += .1f;
        }

        public void ToggleTPPanel()
        {
            if (teleportPanel.activeSelf)
            {
                teleportPanel.SetActive(false);
                tpButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Teleport";
            }
            else
            {
                if (statsPanel.activeSelf)
                {
                    statsPanel.SetActive(false);
                    statsButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Stats & Info";
                }
                if (sfxPanel.activeSelf)
                {
                    sfxPanel.SetActive(false);
                    sfxButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SFX Panel";
                }

                teleportPanel.SetActive(true);
                tpButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] Teleport";
            }
        }

        void ToggleSFX()
        {
            if (modParent.soundControllerMod.enableSFX)
            {
                modParent.soundControllerMod.sfx = !modParent.soundControllerMod.sfx;
                if (modParent.soundControllerMod.sfx)
                    bovButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] SFX";
                else
                    bovButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SFX";
            }
            else
            {
                ModConsole.Error(modParent.Name + ": BovSound is disabled. Failed setup.");
            }
        }

        void ToggleBovSoundSFX()
        {
            modParent.soundControllerMod.bovChoice++;
            if (modParent.soundControllerMod.bovChoice >= modParent.soundControllerMod._sfxSounds.Count)
                modParent.soundControllerMod.bovChoice = 1;

            bovSFXButton.transform.GetChild(0).GetComponent<Text>().text = "[" + modParent.soundControllerMod.bovChoice + "] BovSound SFX";

            modParent.soundControllerMod._bovSound.GetComponent<AudioSource>().PlayOneShot(modParent.soundControllerMod._sfxSounds[modParent.soundControllerMod.bovChoice], .1f);
        }

        void ToggleSuspensionFix()
        {
            if (modParent.wheelSuspensionMod.SuspensionFixApplied)
            {
                modParent.wheelSuspensionMod.RevertSuspensionFix();
                suspensionbutton.transform.GetChild(0).GetComponent<Text>().text = "[U-lock] Suspension";
            }
            else
            {
                modParent.wheelSuspensionMod.SuspensionFix();
                suspensionbutton.transform.GetChild(0).GetComponent<Text>().text = "[Locked] Suspension";
            }
        }

        void ToggleABS()
        {

            modParent.fixABS = !modParent.fixABS;

            modParent._satsuma.GetComponent<CarController>().ABS = !modParent._satsuma.GetComponent<CarController>().ABS;
            if (modParent._satsuma.GetComponent<CarController>().ABS)
                absButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] ABS";
            else
                absButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] ABS";
        }

        public void ShowUI()
        {
            if (rootUI.activeSelf)
            {
                rootUI.SetActive(false);
            }
            else
            {
                rootUI.SetActive(true);
            }
        }

        void ChangeBovVolume(float value)
        {
            if (modParent.soundControllerMod.bovVolume <= 1f)
                modParent.soundControllerMod._bovSound.GetComponent<AudioSource>().volume += value;

            bovVol.text = Math.Round(modParent.soundControllerMod._bovSound.GetComponent<AudioSource>().volume, 2).ToString();
        }

        void ToggleEngineFixer()
        {
            modParent.engineFixer = !modParent.engineFixer;
            if (modParent.engineFixer)
            {
                engineFixerButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] Engine Fixer";
                if (modParent.engineFixerMod._wearAlternator != null)
                    modParent.engineFixerMod._wearAlternator.Value = 100f;
                if (modParent.engineFixerMod._wearCrankshaft != null)
                    modParent.engineFixerMod._wearCrankshaft.Value = 100f;
                if (modParent.engineFixerMod._wearGearbox != null)
                    modParent.engineFixerMod._wearGearbox.Value = 100f;
                if (modParent.engineFixerMod._wearHeadgasket != null)
                    modParent.engineFixerMod._wearHeadgasket.Value = 100f;
                if (modParent.engineFixerMod._wearPiston1 != null)
                    modParent.engineFixerMod._wearPiston1.Value = 100f;
                if (modParent.engineFixerMod._wearPiston2 != null)
                    modParent.engineFixerMod._wearPiston2.Value = 100f;
                if (modParent.engineFixerMod._wearPiston3 != null)
                    modParent.engineFixerMod._wearPiston3.Value = 100f;
                if (modParent.engineFixerMod._wearPiston4 != null)
                    modParent.engineFixerMod._wearPiston4.Value = 100f;
                if (modParent.engineFixerMod._wearRockershaft != null)
                    modParent.engineFixerMod._wearRockershaft.Value = 100f;
                if (modParent.engineFixerMod._wearStarter != null)
                    modParent.engineFixerMod._wearStarter.Value = 100f;
                if (modParent.engineFixerMod._wearWaterpump != null)
                    modParent.engineFixerMod._wearWaterpump.Value = 100f;
                if (modParent.engineFixerMod._oilLevel != null)
                    modParent.engineFixerMod._oilLevel.Value = 3f;
                if (modParent.engineFixerMod._satsumaFuel != null)
                    modParent.engineFixerMod._satsumaFuel.Value = 36f;
                if (modParent.engineFixerMod._racingWater != null)
                    modParent.engineFixerMod._racingWater.Value = 7f;
                if (modParent.engineFixerMod._brakeFluidF != null)
                    modParent.engineFixerMod._brakeFluidF.Value = 1f;
                if (modParent.engineFixerMod._brakeFluidR != null)
                    modParent.engineFixerMod._brakeFluidR.Value = 1f;
                if (modParent.engineFixerMod._cluthFluid != null)
                    modParent.engineFixerMod._cluthFluid.Value = .5f;
                if (modParent.engineFixerMod._engineTemp != null)
                    if (modParent.engineFixerMod._engineTemp.Value < 83f)
                        modParent.engineFixerMod._engineTemp = 80f;
            }
            else
                engineFixerButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Engine Fixer";
        }

        void ToggleESP()
        {
            modParent.fixESP = !modParent.fixESP;

            modParent._satsuma.GetComponent<CarController>().ESP = !modParent._satsuma.GetComponent<CarController>().ESP;
            modParent._satsuma.GetComponent<CarController>().TCS = !modParent._satsuma.GetComponent<CarController>().TCS;
            if (modParent._satsuma.GetComponent<CarController>().ESP)
                espButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] ESP";
            else
                espButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] ESP";
        }

        void ToggleDriveType()
        {
            modParent.fixDrivetype = !modParent.fixDrivetype;

            if (modParent.sixGearsMod.driveType < 2)
                modParent.sixGearsMod.driveType++;
            else
                modParent.sixGearsMod.driveType = 0;

            modParent._satsumaDriveTrain.SetTransmission((Drivetrain.Transmissions)modParent.sixGearsMod.driveType);
            driveTypeButton.transform.GetChild(0).GetComponent<Text>().text = "[" + resolveTransType() + "] Drive type";
            modParent.sixGearsMod.lastDrivetype = modParent._satsumaDriveTrain.transmission;
        }

        string resolveTransType()
        {
            if (modParent.sixGearsMod.driveType == 0) return "RWD";
            if (modParent.sixGearsMod.driveType == 1) return "FWD";
            if (modParent.sixGearsMod.driveType == 2) return "AWD";
            return "Error";
        }

        void ToggleTransType()
        {
            modParent.sixGearsMod.autoTransEnabled = !modParent.sixGearsMod.autoTransEnabled;

            modParent._satsumaDriveTrain.automatic = !modParent._satsumaDriveTrain.automatic;
            if (modParent._satsumaDriveTrain.automatic)
            {
                transTypeButton.transform.GetChild(0).GetComponent<Text>().text = "[Auto] Trans type";
                modParent._satsumaDriveTrain.autoReverse = false;
            }
            else
            {
                transTypeButton.transform.GetChild(0).GetComponent<Text>().text = "[Manual] Trans type";
                modParent._satsumaDriveTrain.autoReverse = true;
            }
        }

        void ToggleSixGears()
        {
            modParent.sixGears = !modParent.sixGears;
            if (modParent.sixGears)
            {
                modParent._satsumaDriveTrain.gearRatios = modParent.sixGearsMod.newRatio;
                modParent._satsumaDriveTrain.maxRPM = 8400f;
                eightGearButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] SixGears";
            }
            else
            {
                modParent._satsumaDriveTrain.gearRatios = modParent.sixGearsMod.oldRatio;
                modParent._satsumaDriveTrain.maxRPM = modParent.defaultRPM;
                eightGearButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SixGears";
            }
        }
    }
}
