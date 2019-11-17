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

        //Enum with teleport types
        enum tpType
        {
            Player,
            Car
        }

        //Enum with teleport locations
        enum tpLocation
        {
            Home,
            Mechanic,
            Highway,
            Shop,
            Strip,
            Poker,
            Strawberry,
            Grandma,
            Ski,
            Cottage,
            Car
        }

        //Mod UI
        GameObject _ui;

        //Gradient Text
        Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        //UI References
        GameObject statsPanel, sfxPanel, teleportPanel;
        Text currentHP, currentSpeed, currentRPM, boostPowerText, currentEngineTemp;
        Text wearAlternator, wearCrankshaft, wearGearbox, wearHeadgasket, wearPiston1, wearPiston2, wearPiston3, wearPiston4, wearRockershaft, wearStarter, wearWaterpump;
        Text oilLevel, satsumaFuel, racingWater, brakeFluidF, brakeFluidR, cluthFluid;
        Text bovVol;
        Button statsButton, sfxButton, tpButton, boostButton, boostMinusButton, boostPlusButton, engineFixerButton, eightGearButton, transTypeButton, driveTypeButton, espButton, absButton, suspensionbutton, upButton, downButton, rightButton, leftButton, inButton, outButton, bovButton, bovMinusButton, bovPlusButton, bovSFXButton;
        Button carHome, carMech, carHighway, carShop, carStrip, carPoker, carStrawberry, carGrandma, carSki, playerHome, playerMech, playerHighway, playerShop, playerStrip, playerPoker, playerStrawberry, playerGrandma, playerCottage, playerCar;


        //Mod References
        GameObject _satsuma;
        GameObject player;
        Drivetrain _satsumaDriveTrain;

        //SoundMod
        GameObject _bovSound;
        List<AudioClip>_sfxSounds = new List<AudioClip>();
        int bovChoice = 1;
        float bovVolume = 1f;

        //SixGears
        int rememberGear = 0; // A variable to remember the last gear used.
        bool autoTransEnabled;

        //Suspension Fix
        bool SuspensionFixApplied;

        //Wheels & Camber
        private GameObject WHEELRR, WHEELRL, WHEELFR, WHEELFL;
        public float WheelYPosition, WheelXPosition;
        private Vector4 WheelStartYPositions, WheelStartXPositions;
        private float originalFrontWheelYPos, originalFrontWheelXPos, originalRearWheelYPos, originalRearWheelXPos, originalFrontCamber, originalRearCamber;

        //Power Boost
        float powerMultiplier = 1f;
        float defaultRPM;
        float defaultShiftUpRPM;
        float defaultShiftDownRPM;

        //Six Gears
        Drivetrain.Transmissions lastDrivetype;
        int driveType = 1; // Default forward
        float[] oldRatio;
        private float[] newRatio = new float[]
        {
            /**
             * Default:
             * -4.093f
             * 0f
             * 3673f
             * 2217f
             * 1456f
             * 1f
             */
            -4.093f,// reverse 
            0f,     // neutral
            3.424f, // 1st
            2.141f, // 2nd...
            1.318f, // 3nd...
            1f,     // 4nd... 
            0.90f,  // 5nd... 
            0.85f,  // 6nd... 
        };

        //Teleport locations etc
        private Vector3 loc_car_garage = new Vector3(-16.5f, 0.5f, 12.1f);
        private Quaternion rot_car_garage = Quaternion.Euler(0f, -1f, 0f);

        private Vector3 loc_car_highway = new Vector3(1829.6f, -7f, -1089.3f);
        private Quaternion rot_car_highway = Quaternion.Euler(0f, 600f, 0f);

        private Vector3 loc_car_mechanic = new Vector3(1542.1f, 5.5f, 721.3f);
        private Quaternion rot_car_mechanic = Quaternion.Euler(0f, 0.8f, 0f);

        private Vector3 loc_car_shop = new Vector3(-1542.1f, 3.9f, 1176.5f);
        private Quaternion rot_car_shop = Quaternion.Euler(0f, 500f, 0f);

        private Vector3 loc_car_strip = new Vector3(-1308.9f, 3.5f, -935.8f);
        private Quaternion rot_car_strip = Quaternion.Euler(0f, 500f, 0f);

        private Vector3 loc_car_poker = new Vector3(-174.3f, -2.8f, 1011.4f);
        private Quaternion rot_car_poker = Quaternion.Euler(0f, 500f, 0f);

        private Vector3 loc_car_strawberry = new Vector3(-1198.3f, 1f, -618.8f);
        private Quaternion rot_car_strawberry = Quaternion.Euler(0f, 0f, 0f);

        private Vector3 loc_car_grandma = new Vector3(444.6f, 3.2f, -1335.6f);
        private Quaternion rot_car_grandma = Quaternion.Euler(0f, 0f, 0f);

        private Vector3 loc_car_ski = new Vector3(-2014.2f, 70.2f, -122f);
        private Quaternion rot_car_ski = Quaternion.Euler(0f, 0f, 0f);

        private Vector3 loc_player_garage = new Vector3(-6.1f, -0.3f, 9.9f);
        private Vector3 loc_player_mechanic = new Vector3(1538.2f, 4.7f, 722f);
        private Vector3 loc_player_highway = new Vector3(1829.4f, -7.5f, -1084.9f);
        private Vector3 loc_player_shop = new Vector3(-1550.7f, 3.2f, 1176.9f);
        private Vector3 loc_player_strip = new Vector3(-1287.4f, 2.2f, -923.6f);
        private Vector3 loc_player_poker = new Vector3(-174.3f, -3.8f, 1011.4f);
        private Vector3 loc_player_cottge = new Vector3(-854.5f, -2.9f, 512.5f);
        private Vector3 loc_player_strawberry = new Vector3(-1210.5f, 0.7f, -631.1f);
        private Vector3 loc_player_grandma = new Vector3(455.2f, 3f, -1336.8f);

        //Engine parts
        private FsmFloat _wearAlternator, _wearCrankshaft, _wearGearbox, _wearHeadgasket, _wearPiston1, _wearPiston2, _wearPiston3, _wearPiston4, _wearRockershaft, _wearStarter, _wearWaterpump, _oilLevel, _satsumaFuel, _racingWater, _brakeFluidF, _brakeFluidR, _cluthFluid, _engineTemp;

        //Mod Controller (Need satsuma)
        bool enableSFX, enableBoost, enableEngineFixer, enableWheelMod;
        bool boost, sfx, engineFixer, sixGears;

        bool fixABS, fixESP, fixDrivetype;

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

                Keybind.Add(this, ShowUIBind);

                try
                {
                    // Called once, when mod is loading after game is fully loaded
                    AssetBundle ab = LoadAssets.LoadBundle(this, "satsumamaster.unity3d");
                    GameObject UI = ab.LoadAsset("SatsumaMaster.prefab") as GameObject;
                    _bovSound = ab.LoadAsset("BovSound.prefab") as GameObject;
                    _sfxSounds.Add(ab.LoadAsset("SpoolSound.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("BOV.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("1.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("2.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("3.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("4.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("5.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("6.wav") as AudioClip);
                    _sfxSounds.Add(ab.LoadAsset("7.wav") as AudioClip);
                    ab.Unload(false); //unload when you take all items you need.

                    _bovSound = GameObject.Instantiate(_bovSound);

                    _bovSound.transform.parent = GameObject.Find("PLAYER").transform;
                    _bovSound.transform.localPosition = new Vector3(0f, 0f, 0f);
                    _bovSound.transform.position = new Vector3(0f, 0f, 0f);

                    if (_sfxSounds.Count >= 9)
                        enableSFX = true;

                    _ui = GameObject.Instantiate(UI);
                }
                catch (Exception)
                {
                    ModConsole.Error("Asset load failed. Most likely you failed installing the mod.");
                    return;
                }

                try
                {
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

                    //playerCar.transform.GetChild(0).name = "DEACTIVATED";
                }
                catch (Exception)
                {
                    ModConsole.Error("Mod missmatch with the asset pack?");
                }

                try
                {
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
                    rightButton.onClick.AddListener(delegate { SetNewWheelXPositions(IsDirectionLeft: true, IsFrontWheels: true, IsRearWheels: true); });
                    leftButton.onClick.AddListener(delegate { SetNewWheelXPositions(IsDirectionLeft: false, IsFrontWheels: true, IsRearWheels: true); });
                    upButton.onClick.AddListener(delegate { SetNewWheelPositions(IsDirectionUp: true, IsFrontWheels: true, IsRearWheels: true); });
                    downButton.onClick.AddListener(delegate { SetNewWheelPositions(IsDirectionUp: false, IsFrontWheels: true, IsRearWheels: true); });
                    inButton.onClick.AddListener(delegate { SetWheelCambers(true, true, 0); });
                    outButton.onClick.AddListener(delegate { SetWheelCambers(true, true, 1); });

                    bovButton.onClick.AddListener(delegate { ToggleSFX(); });
                    bovMinusButton.onClick.AddListener(delegate { ChangeBovVolume(-.1f); });
                    bovPlusButton.onClick.AddListener(delegate { ChangeBovVolume(.1f); });
                    bovSFXButton.onClick.AddListener(delegate { ToggleBovSoundSFX(); });

                    carHome.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Home); });
                    carMech.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Mechanic); });
                    carHighway.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Highway); });
                    carShop.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Shop); });
                    carStrip.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Strip); });
                    carPoker.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Poker); });
                    carStrawberry.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Strawberry); });
                    carGrandma.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Grandma); });
                    carSki.onClick.AddListener(delegate { TeleportMode(tpType.Car, tpLocation.Ski); });

                    playerHome.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Home); });
                    playerMech.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Mechanic); });
                    playerHighway.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Highway); });
                    playerShop.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Shop); });
                    playerStrip.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Strip); });
                    playerPoker.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Poker); });
                    playerStrawberry.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Strawberry); });
                    playerGrandma.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Grandma); });
                    playerCottage.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Cottage); });
                    //playerCar.onClick.AddListener(delegate { TeleportMode(tpType.Player, tpLocation.Car); });
                }
                catch (Exception)
                {
                    ModConsole.Error("Mod could not setup buttons. Wierd.."); 
                }

                try
                {
                    ToggleStats();
                    ToggleXFSPanel();
                    ToggleTPPanel();
                    ShowUI();

                    _satsuma = GameObject.Find("SATSUMA(557kg, 248)");
                    player = GameObject.Find("PLAYER");

                    if (_satsuma != null)
                    {
                        _satsumaDriveTrain = _satsuma.GetComponent<Drivetrain>();
                        if (_satsumaDriveTrain != null)
                        {
                            defaultRPM = _satsumaDriveTrain.maxRPM;
                            defaultShiftUpRPM = _satsumaDriveTrain.shiftUpRPM;
                            defaultShiftDownRPM = _satsumaDriveTrain.shiftDownRPM;
                            oldRatio = _satsumaDriveTrain.gearRatios;
                            enableBoost = true;

                            OptimizeEngineFixer();
                        }
                    }

                    if (WHEELRL == null)
                        WHEELRL = _satsuma.transform.FindChild("RL").transform.GetChild(0).gameObject;
                    if (WHEELRR == null)
                        WHEELRR = _satsuma.transform.FindChild("RR").transform.GetChild(0).gameObject;
                    if (WHEELFR == null)
                        WHEELFR = _satsuma.transform.FindChild("FR").transform.GetChild(0).transform.GetChild(0).gameObject;
                    if (WHEELFL == null)
                        WHEELFL = _satsuma.transform.FindChild("FL").transform.GetChild(0).transform.GetChild(0).gameObject;

                    if (WHEELRL != null && WHEELRL != null && WHEELFR != null && WHEELFL != null)
                    {
                        originalFrontCamber = _satsuma.GetComponent<Axles>().frontAxle.leftWheel.camber;
                        originalRearCamber = _satsuma.GetComponent<Axles>().rearAxle.leftWheel.camber;
                        originalFrontWheelYPos = WHEELFL.transform.localPosition.y;
                        originalRearWheelYPos = WHEELRL.transform.localPosition.y;
                        originalFrontWheelXPos = WHEELFL.transform.localPosition.x;
                        originalRearWheelXPos = WHEELRL.transform.localPosition.x;

                        WheelStartYPositions = new Vector4(WHEELFR.transform.localPosition.y, WHEELFL.transform.localPosition.y, WHEELRR.transform.localPosition.y, WHEELRL.transform.localPosition.y);
                        WheelStartXPositions = new Vector4(WHEELFR.transform.localPosition.x, WHEELFL.transform.localPosition.x, WHEELRR.transform.localPosition.x, WHEELRL.transform.localPosition.x);
                        enableWheelMod = true;
                    }

                    SetupGradient();

                    ModConsole.Print(Name + ": Success!");
                    setupOK = true;
                }
                catch(Exception)
                {
                    ModConsole.Error("Last part failed.");
                }
            }
        }

        void ToggleBovSoundSFX()
        {
            bovChoice++;
            if (bovChoice >= _sfxSounds.Count)
                bovChoice = 1;

            bovSFXButton.transform.GetChild(0).GetComponent<Text>().text = "[" + bovChoice + "] BovSound SFX";

            _bovSound.GetComponent<AudioSource>().PlayOneShot(_sfxSounds[bovChoice], .1f);
        }

        void ToggleSuspensionFix()
        {
            if (SuspensionFixApplied)
            {
                RevertSuspensionFix();
                suspensionbutton.transform.GetChild(0).GetComponent<Text>().text = "[U-lock] Suspension";
            }
            else
            {
                SuspensionFix();
                suspensionbutton.transform.GetChild(0).GetComponent<Text>().text = "[Locked] Suspension";
            }
        }

        void ToggleABS()
        {

            fixABS = !fixABS;

            _satsuma.GetComponent<CarController>().ABS = !_satsuma.GetComponent<CarController>().ABS;
            if (_satsuma.GetComponent<CarController>().ABS)
                absButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] ABS";
            else
                absButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] ABS";
        }

        void ToggleESP()
        {
            fixESP = !fixESP;

            _satsuma.GetComponent<CarController>().ESP = !_satsuma.GetComponent<CarController>().ESP;
            _satsuma.GetComponent<CarController>().TCS = !_satsuma.GetComponent<CarController>().TCS;
            if (_satsuma.GetComponent<CarController>().ESP)
                espButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] ESP";
            else
                espButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] ESP";
        }

        void ToggleDriveType()
        {
            fixDrivetype = !fixDrivetype;

            if (driveType < 2)
                driveType++;
            else
                driveType = 0;

            _satsumaDriveTrain.SetTransmission((Drivetrain.Transmissions)driveType);
            driveTypeButton.transform.GetChild(0).GetComponent<Text>().text = "[" + resolveTransType() + "] Drive type";
            lastDrivetype = _satsumaDriveTrain.transmission;
        }

        string resolveTransType()
        {
            if (driveType == 0) return "RWD";
            if (driveType == 1) return "FWD";
            if (driveType == 2) return "AWD";
            return "Error";
        }

        void ToggleTransType()
        {
            autoTransEnabled = !autoTransEnabled;

            _satsumaDriveTrain.automatic = !_satsumaDriveTrain.automatic;
            if (_satsumaDriveTrain.automatic)
            {
                transTypeButton.transform.GetChild(0).GetComponent<Text>().text = "[Auto] Trans type";
                _satsumaDriveTrain.autoReverse = false;
            }
            else
            {
                transTypeButton.transform.GetChild(0).GetComponent<Text>().text = "[Manual] Trans type";
                _satsumaDriveTrain.autoReverse = true;
            }
        }

        void ToggleSixGears()
        {
            sixGears = !sixGears;
            if (sixGears)
            {
                _satsumaDriveTrain.gearRatios = newRatio;
                _satsumaDriveTrain.maxRPM = 8400f;
                eightGearButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] SixGears";
            }
            else
            {
                _satsumaDriveTrain.gearRatios = oldRatio;
                _satsumaDriveTrain.maxRPM = defaultRPM;
                eightGearButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SixGears";
            }
        }

        void ToggleEngineFixer()
        {
            engineFixer = !engineFixer;
            if (engineFixer)
            {
                engineFixerButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] Engine Fixer";
                if (_wearAlternator != null)
                    _wearAlternator.Value = 100f;
                if (_wearCrankshaft != null)
                    _wearCrankshaft.Value = 100f;
                if (_wearGearbox != null)
                    _wearGearbox.Value = 100f;
                if (_wearHeadgasket != null)
                    _wearHeadgasket.Value = 100f;
                if (_wearPiston1 != null)
                    _wearPiston1.Value = 100f;
                if (_wearPiston2 != null)
                    _wearPiston2.Value = 100f;
                if (_wearPiston3 != null)
                    _wearPiston3.Value = 100f;
                if (_wearPiston4 != null)
                    _wearPiston4.Value = 100f;
                if (_wearRockershaft != null)
                    _wearRockershaft.Value = 100f;
                if (_wearStarter != null)
                    _wearStarter.Value = 100f;
                if (_wearWaterpump != null)
                    _wearWaterpump.Value = 100f;
                if (_oilLevel != null)
                    _oilLevel.Value = 3f;
                if (_satsumaFuel != null)
                    _satsumaFuel.Value = 36f;
                if (_racingWater != null)
                    _racingWater.Value = 7f;
                if (_brakeFluidF != null)
                    _brakeFluidF.Value = 1f;
                if (_brakeFluidR != null)
                    _brakeFluidR.Value = 1f;
                if (_cluthFluid != null)
                    _cluthFluid.Value = .5f;
                if (_engineTemp != null)
                    if (_engineTemp.Value < 83f)
                        _engineTemp = 80f;
            }
            else
                engineFixerButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Engine Fixer";
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

        void ChangeBovVolume(float value)
        {
            if (bovVolume <= 1f)
                _bovSound.GetComponent<AudioSource>().volume += value;

            bovVol.text = Math.Round(_bovSound.GetComponent<AudioSource>().volume, 2).ToString();
        }

        void ToggleSFX()
        {
            if (enableSFX)
            {
                sfx = !sfx;
                if (sfx)
                    bovButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] SFX";
                else
                    bovButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] SFX";
            }
            else
            {
                ModConsole.Print(Name + ": BovSound is disabled. Failed setup.");
            }
        }

        void ToggleTPPanel()
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

        void ToggleXFSPanel()
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

        void ToggleStats()
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
            if (enableBoost)
            {
                boost = !boost;
                if (boost)
                {
                    boostButton.transform.GetChild(0).GetComponent<Text>().text = "[ON] Boost power";
                    _satsumaDriveTrain.maxRPM = 8400f;
                    _satsumaDriveTrain.canStall = false;
                    _satsumaDriveTrain.shiftUpRPM = 7000f;
                    _satsumaDriveTrain.shiftDownRPM = 3000f;
                }
                else
                {
                    boostButton.transform.GetChild(0).GetComponent<Text>().text = "[OFF] Boost power";
                    _satsumaDriveTrain.maxRPM = defaultRPM;
                    _satsumaDriveTrain.canStall = true;
                    _satsumaDriveTrain.shiftUpRPM = defaultShiftUpRPM;
                    _satsumaDriveTrain.shiftDownRPM = defaultShiftDownRPM;
                }
            }
            else
            {
                ModConsole.Print(Name + ": SatsumaMaster is disabled. Failed setup.");
            }
        }

        void ChangePower(float value)
        {
            if (powerMultiplier > .5f)
                powerMultiplier += value;
            if (powerMultiplier < .5f)
                powerMultiplier += .1f;
        }

        void SetWheelCambers(bool IsFront, bool IsRear, int Direction)
        {
            if (Direction == 0)
            {
                if (IsFront)
                {
                    Wheel rightWheel = _satsuma.GetComponent<Axles>().frontAxle.rightWheel;
                    rightWheel.camber += 0.5f;
                    Wheel leftWheel = _satsuma.GetComponent<Axles>().frontAxle.leftWheel;
                    leftWheel.camber -= 0.5f;
                }
                if (IsRear)
                {
                    Wheel rightWheel2 = _satsuma.GetComponent<Axles>().rearAxle.rightWheel;
                    rightWheel2.camber += 0.5f;
                    Wheel leftWheel2 = _satsuma.GetComponent<Axles>().rearAxle.leftWheel;
                    leftWheel2.camber -= 0.5f;
                }
            }
            else
            {
                if (IsFront)
                {
                    Wheel rightWheel3 = _satsuma.GetComponent<Axles>().frontAxle.rightWheel;
                    rightWheel3.camber -= 0.5f;
                    Wheel leftWheel3 = _satsuma.GetComponent<Axles>().frontAxle.leftWheel;
                    leftWheel3.camber += 0.5f;
                }
                if (IsRear)
                {
                    Wheel rightWheel4 = _satsuma.GetComponent<Axles>().rearAxle.rightWheel;
                    rightWheel4.camber -= 0.5f;
                    Wheel leftWheel4 = _satsuma.GetComponent<Axles>().rearAxle.leftWheel;
                    leftWheel4.camber += 0.5f;
                }
            }
        }

        void OptimizeEngineFixer()
        {
            if (_satsuma != null)
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

                enableEngineFixer = true;
            }
            else
                return;
        }

        void SuspensionFix()
        {
            PlayMakerFSM[] componentsInChildren = _satsuma.GetComponentsInChildren<PlayMakerFSM>();
            foreach (PlayMakerFSM val in componentsInChildren)
            {
                if (val.name == "Suspension")
                {
                    val.enabled = false;
                }
            }
            SuspensionFixApplied = true;
        }

        void RevertSuspensionFix()
        {
            PlayMakerFSM[] componentsInChildren = _satsuma.GetComponentsInChildren<PlayMakerFSM>();
            foreach (PlayMakerFSM val in componentsInChildren)
            {
                if (val.name == "Suspension")
                {
                    val.enabled = true;
                }
            }
            SuspensionFixApplied = false;
        }

        void ShowUI()
        {
            if (_ui.activeSelf)
            {
                _ui.SetActive(false);
                //FsmVariables.GlobalVariables.FindFsmBool("PlayerInMenu").Value = false;
            }
            else
            {
                //FsmVariables.GlobalVariables.FindFsmBool("PlayerInMenu").Value = true;
                _ui.SetActive(true);
            }
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
                returnString = "ERROR";
            else
                returnString = _wearPercentage.ToString() + "%";
            
            obj.color = ColorFromGradient(float.Parse(_colorFixer.ToString().Insert(0, "0.")));

            return returnString;
        }

        void EngineBoost(bool state)
        {
            if (state)
                if(FsmVariables.GlobalVariables.FindFsmString("PlayerCurrentVehicle").Value == "Satsuma")
                    _satsumaDriveTrain.powerMultiplier = powerMultiplier;
            else
                _satsumaDriveTrain.powerMultiplier = 1f;
        }

        void SetNewWheelPositions(bool IsDirectionUp, bool IsFrontWheels, bool IsRearWheels)
        {
            if(enableWheelMod)
            {
                if (IsDirectionUp)
                {
                    WheelYPosition = -0.005f;
                }
                else
                {
                    if (WHEELRR.transform.localPosition.y < -0.05f)
                        WheelYPosition = 0.005f;
                    else
                        WheelYPosition = 0f;
                }
                if (IsFrontWheels)
                {
                    WHEELFL.transform.localPosition = new Vector3(WHEELFL.transform.localPosition.x, WHEELFL.transform.localPosition.y + WheelYPosition, WHEELFL.transform.localPosition.z);
                    WHEELFR.transform.localPosition = new Vector3(WHEELFR.transform.localPosition.x, WHEELFR.transform.localPosition.y + WheelYPosition, WHEELFR.transform.localPosition.z);
                }
                if (IsRearWheels)
                {
                    WHEELRR.transform.localPosition = new Vector3(WHEELRR.transform.localPosition.x, WHEELRR.transform.localPosition.y + WheelYPosition, WHEELRR.transform.localPosition.z);
                    WHEELRL.transform.localPosition = new Vector3(WHEELRL.transform.localPosition.x, WHEELRL.transform.localPosition.y + WheelYPosition, WHEELRL.transform.localPosition.z);
                }
            }
            else
                ModConsole.Print(Name + ": Wheels is disabled. Failed setup.");
        }

        void SetNewWheelXPositions(bool IsDirectionLeft, bool IsFrontWheels, bool IsRearWheels)
        {
            if (enableWheelMod)
            {
                if (IsDirectionLeft)
                {
                    WheelXPosition = -0.005f;
                }
                else
                {
                    WheelXPosition = 0.005f;
                }
                if (IsFrontWheels)
                {
                    WHEELFL.transform.localPosition = new Vector3(WHEELFL.transform.localPosition.x - WheelXPosition, WHEELFL.transform.localPosition.y, WHEELFL.transform.localPosition.z);
                    WHEELFR.transform.localPosition = new Vector3(WHEELFR.transform.localPosition.x + WheelXPosition, WHEELFR.transform.localPosition.y, WHEELFR.transform.localPosition.z);
                }
                if (IsRearWheels)
                {
                    WHEELRR.transform.localPosition = new Vector3(WHEELRR.transform.localPosition.x + WheelXPosition, WHEELRR.transform.localPosition.y, WHEELRR.transform.localPosition.z);
                    WHEELRL.transform.localPosition = new Vector3(WHEELRL.transform.localPosition.x - WheelXPosition, WHEELRL.transform.localPosition.y, WHEELRL.transform.localPosition.z);
                }
            }
            else
                ModConsole.Print(Name + ": Wheels is disabled. Failed setup.");
        }

        private void TeleportMode(tpType type, tpLocation location)
        {
            switch(type)
            {
                case tpType.Player:
                    if (FsmVariables.GlobalVariables.FindFsmString("PlayerCurrentVehicle").Value != "Satsuma")
                    {
                        switch (location)
                        {
                            case tpLocation.Home:
                                player.transform.position = (loc_player_garage);
                                break;
                            case tpLocation.Mechanic:
                                player.transform.position = (loc_player_mechanic);
                                break;
                            case tpLocation.Highway:
                                player.transform.position = (loc_player_highway);
                                break;
                            case tpLocation.Shop:
                                player.transform.position = (loc_player_shop);
                                break;
                            case tpLocation.Strip:
                                player.transform.position = (loc_player_strip);
                                break;
                            case tpLocation.Poker:
                                player.transform.position = (loc_player_poker);
                                break;
                            case tpLocation.Strawberry:
                                player.transform.position = (loc_player_strawberry);
                                break;
                            case tpLocation.Grandma:
                                player.transform.position = (loc_player_grandma);
                                break;
                            case tpLocation.Cottage:
                                player.transform.position = (loc_player_cottge);
                                break;
                            case tpLocation.Car:
                                player.transform.position = new Vector3(_satsuma.transform.rotation.x, _satsuma.transform.rotation.y + 3f, _satsuma.transform.rotation.z);
                                break;
                        }
                    }
                    break;
                case tpType.Car:
                        switch (location)
                        {
                            case tpLocation.Home:
                                _satsuma.transform.position = loc_car_garage;
                                _satsuma.transform.rotation = rot_car_garage;
                            break;
                            case tpLocation.Mechanic:
                                _satsuma.transform.position = (loc_car_mechanic);
                                _satsuma.transform.rotation = (rot_car_mechanic);
                            break;
                            case tpLocation.Highway:
                                _satsuma.transform.position = (loc_car_highway);
                                _satsuma.transform.rotation = (rot_car_highway);
                            break;
                            case tpLocation.Shop:
                                _satsuma.transform.position = (loc_car_shop);
                                _satsuma.transform.rotation = (rot_car_shop);
                            break;
                            case tpLocation.Strip:
                                _satsuma.transform.position = (loc_car_strip);
                                _satsuma.transform.rotation = (rot_car_strip);
                            break;
                            case tpLocation.Poker:
                                _satsuma.transform.position = (loc_car_poker);
                                _satsuma.transform.rotation = (rot_car_poker);
                            break;
                            case tpLocation.Strawberry:
                                _satsuma.transform.position = (loc_car_strawberry);
                                _satsuma.transform.rotation = (rot_car_strawberry);
                            break;
                            case tpLocation.Grandma:
                                _satsuma.transform.position = (loc_car_grandma);
                                _satsuma.transform.rotation = (rot_car_grandma);
                            break;
                            case tpLocation.Ski:
                                _satsuma.transform.position = (loc_car_ski);
                                _satsuma.transform.rotation = (rot_car_ski);
                            break;
                        }
                    break;
            }
        }
        /*
         * Apparently I cant change the values inside EngineBoost from Update function. This worked though!
         */
        public override void FixedUpdate()
        {
            if (versionOK && setupOK)
                EngineBoost(boost);
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
                    if (_satsumaDriveTrain.transmission != lastDrivetype)
                        _satsumaDriveTrain.SetTransmission(lastDrivetype);
                if (sixGears)
                    if (_satsumaDriveTrain.gearRatios != newRatio)
                    {
                        _satsumaDriveTrain.gearRatios = newRatio;
                        _satsumaDriveTrain.maxRPM = 8400f;
                    }
                if (autoTransEnabled)
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
                    ShowUI();

                if (_ui.activeSelf)
                {
                    boostPowerText.text = Math.Round(powerMultiplier, 2).ToString();
                }

                if (engineFixer)
                {
                    if (_wearAlternator != null)
                        if (CalcPercentage(_wearAlternator.Value, 100f) < 30)
                            _wearAlternator.Value = 100f;
                    if (_wearCrankshaft != null)
                        if (CalcPercentage(_wearCrankshaft.Value, 100f) < 30)
                            _wearCrankshaft.Value = 100f;
                    if (_wearGearbox != null)
                        if (CalcPercentage(_wearGearbox.Value, 100f) < 30)
                            _wearGearbox.Value = 100f;
                    if (_wearHeadgasket != null)
                        if (CalcPercentage(_wearHeadgasket.Value, 100f) < 30)
                            _wearHeadgasket.Value = 100f;
                    if (_wearPiston1 != null)
                        if (CalcPercentage(_wearPiston1.Value, 100f) < 30)
                            _wearPiston1.Value = 100f;
                    if (_wearPiston2 != null)
                        if (CalcPercentage(_wearPiston2.Value, 100f) < 30)
                            _wearPiston2.Value = 100f;
                    if (_wearPiston3 != null)
                        if (CalcPercentage(_wearPiston3.Value, 100f) < 30)
                            _wearPiston3.Value = 100f;
                    if (_wearPiston4 != null)
                        if (CalcPercentage(_wearPiston4.Value, 100f) < 30)
                            _wearPiston4.Value = 100f;
                    if (_wearRockershaft != null)
                        if (CalcPercentage(_wearRockershaft.Value, 100f) < 30)
                            _wearRockershaft.Value = 100f;
                    if (_wearStarter != null)
                        if (CalcPercentage(_wearStarter.Value, 100f) < 30)
                            _wearStarter.Value = 100f;
                    if (_wearWaterpump != null)
                        if (CalcPercentage(_wearWaterpump.Value, 100f) < 30)
                            _wearWaterpump.Value = 100f;
                    if (_oilLevel != null)
                        if (CalcPercentage(_oilLevel.Value, 3f) < 30)
                            _oilLevel.Value = 3f;
                    if (_satsumaFuel != null)
                        if (CalcPercentage(_satsumaFuel.Value, 36f) < 30)
                            _satsumaFuel.Value = 36f;
                    if (_racingWater != null)
                        if (CalcPercentage(_racingWater.Value, 7f) < 30)
                            _racingWater.Value = 7f;
                    if (_brakeFluidF != null)
                        if (CalcPercentage(_brakeFluidF.Value, 1f) < 30)
                            _brakeFluidF.Value = 1f;
                    if (_brakeFluidR != null)
                        if (CalcPercentage(_brakeFluidR.Value, 1f) < 30)
                            _brakeFluidR.Value = 1f;
                    if (_cluthFluid != null)
                        if (CalcPercentage(_cluthFluid.Value, .5f) < 30)
                            _cluthFluid.Value = .5f;
                    if (_engineTemp != null)
                        if (_engineTemp.Value < 83f)
                            _engineTemp = 80f;
                }

                if (sfx && _satsumaDriveTrain.gear != 0 && _satsumaDriveTrain.gear != 1 && _satsumaDriveTrain.gear != rememberGear && _satsumaDriveTrain.gear > rememberGear && _satsumaDriveTrain.rpm > 3500f && Mathf.Round(_satsumaDriveTrain.differentialSpeed) > 30f)
                {
                    _bovSound.GetComponent<AudioSource>().PlayOneShot(_sfxSounds[bovChoice], .1f); // We play the BOV sound with the volume.
                    rememberGear = _satsumaDriveTrain.gear; // We get the current gear and store it as the last used gear.
                }
                else
                    rememberGear = _satsumaDriveTrain.gear; // We get the new gear as the last remembered.

                if (statsPanel.activeSelf)
                {
                    //Add all text updates...
                    currentHP.text = "Current Power: " + Mathf.Round(_satsumaDriveTrain.currentPower).ToString();
                    currentSpeed.text = "Speed: " + Mathf.Round(_satsumaDriveTrain.differentialSpeed).ToString();
                    currentRPM.text = "RPM: " + Mathf.Round(_satsumaDriveTrain.rpm).ToString();
                    currentEngineTemp.text = "Engine temp: " + Mathf.Round(_engineTemp.Value).ToString();

                    if (enableEngineFixer)
                    {
                        wearAlternator.text = "Alternator: " + WearStatus(wearAlternator, _wearAlternator, 100f);
                        wearCrankshaft.text = "Crankshaft: " + WearStatus(wearCrankshaft, _wearCrankshaft, 100f);
                        wearGearbox.text = "Gearbox: " + WearStatus(wearGearbox, _wearGearbox, 100f);
                        wearHeadgasket.text = "Headgasket: " + WearStatus(wearHeadgasket, _wearHeadgasket, 100f);
                        wearPiston1.text = "Piston1: " + WearStatus(wearPiston1, _wearPiston1, 100f);
                        wearPiston2.text = "Piston2: " + WearStatus(wearPiston2, _wearPiston2, 100f);
                        wearPiston3.text = "Piston3: " + WearStatus(wearPiston3, _wearPiston3, 100f);
                        wearPiston4.text = "Piston4: " + WearStatus(wearPiston4, _wearPiston4, 100f);
                        wearRockershaft.text = "Rockershaft: " + WearStatus(wearRockershaft, _wearRockershaft, 100f);
                        wearStarter.text = "Starter: " + WearStatus(wearStarter, _wearStarter, 100f);
                        wearWaterpump.text = "Waterpump: " + WearStatus(wearWaterpump, _wearWaterpump, 100f);

                        oilLevel.text = "Oil: " + WearStatus(oilLevel, _oilLevel, 3f);
                        satsumaFuel.text = "Fuel: " + WearStatus(satsumaFuel, _satsumaFuel, 36f);
                        if (_racingWater != null)
                            racingWater.text = "Water: " + WearStatus(racingWater, _racingWater, 7f);
                        brakeFluidF.text = "Brake F: " + WearStatus(brakeFluidF, _brakeFluidF, 1f);
                        brakeFluidR.text = "Brake R: " + WearStatus(brakeFluidR, _brakeFluidR, 1f);
                        cluthFluid.text = "Clutch F: " + WearStatus(cluthFluid, _cluthFluid, .5f);
                    }

                }
            }
        }
    }
}

