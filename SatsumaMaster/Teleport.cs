using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HutongGames.PlayMaker;
using MSCLoader;

namespace SatsumaMaster
{
    public class Teleport
    {
        GameObject satsuma;
        GameObject player;

        //Enum with teleport types
        public enum tpType
        {
            Player,
            Car
        }

        //Enum with teleport locations
        public enum tpLocation
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

        //Teleport locations etc
        public Vector3 loc_car_garage = new Vector3(-16.5f, 0.5f, 12.1f);
        public Quaternion rot_car_garage = Quaternion.Euler(0f, -1f, 0f);

        public Vector3 loc_car_highway = new Vector3(1829.6f, -7f, -1089.3f);
        public Quaternion rot_car_highway = Quaternion.Euler(0f, 600f, 0f);

        public Vector3 loc_car_mechanic = new Vector3(1542.1f, 5.5f, 721.3f);
        public Quaternion rot_car_mechanic = Quaternion.Euler(0f, 0.8f, 0f);

        public Vector3 loc_car_shop = new Vector3(-1542.1f, 3.9f, 1176.5f);
        public Quaternion rot_car_shop = Quaternion.Euler(0f, 500f, 0f);

        public Vector3 loc_car_strip = new Vector3(-1308.9f, 3.5f, -935.8f);
        public Quaternion rot_car_strip = Quaternion.Euler(0f, 500f, 0f);

        public Vector3 loc_car_poker = new Vector3(-174.3f, -2.8f, 1011.4f);
        public Quaternion rot_car_poker = Quaternion.Euler(0f, 500f, 0f);

        public Vector3 loc_car_strawberry = new Vector3(-1198.3f, 1f, -618.8f);
        public Quaternion rot_car_strawberry = Quaternion.Euler(0f, 0f, 0f);

        public Vector3 loc_car_grandma = new Vector3(444.6f, 3.2f, -1335.6f);
        public Quaternion rot_car_grandma = Quaternion.Euler(0f, 0f, 0f);

        public Vector3 loc_car_ski = new Vector3(-2014.2f, 70.2f, -122f);
        public Quaternion rot_car_ski = Quaternion.Euler(0f, 0f, 0f);

        public Vector3 loc_player_garage = new Vector3(-6.1f, -0.3f, 9.9f);
        public Vector3 loc_player_mechanic = new Vector3(1538.2f, 4.7f, 722f);
        public Vector3 loc_player_highway = new Vector3(1829.4f, -7.5f, -1084.9f);
        public Vector3 loc_player_shop = new Vector3(-1550.7f, 3.2f, 1176.9f);
        public Vector3 loc_player_strip = new Vector3(-1287.4f, 2.2f, -923.6f);
        public Vector3 loc_player_poker = new Vector3(-174.3f, -3.8f, 1011.4f);
        public Vector3 loc_player_cottge = new Vector3(-854.5f, -2.9f, 512.5f);
        public Vector3 loc_player_strawberry = new Vector3(-1210.5f, 0.7f, -631.1f);
        public Vector3 loc_player_grandma = new Vector3(455.2f, 3f, -1336.8f);

        public Teleport(GameObject _player, GameObject _satsuma)
        {
            player = _player;
            satsuma = _satsuma;
        }

        public void TeleportMode(tpType type, tpLocation location)
        {
            switch (type)
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
                                player.transform.position = new Vector3(satsuma.transform.rotation.x, satsuma.transform.rotation.y + 3f, satsuma.transform.rotation.z);
                                break;
                        }
                    }
                    break;
                case tpType.Car:
                    switch (location)
                    {
                        case tpLocation.Home:
                            satsuma.transform.position = loc_car_garage;
                            satsuma.transform.rotation = rot_car_garage;
                            break;
                        case tpLocation.Mechanic:
                            satsuma.transform.position = (loc_car_mechanic);
                            satsuma.transform.rotation = (rot_car_mechanic);
                            break;
                        case tpLocation.Highway:
                            satsuma.transform.position = (loc_car_highway);
                            satsuma.transform.rotation = (rot_car_highway);
                            break;
                        case tpLocation.Shop:
                            satsuma.transform.position = (loc_car_shop);
                            satsuma.transform.rotation = (rot_car_shop);
                            break;
                        case tpLocation.Strip:
                            satsuma.transform.position = (loc_car_strip);
                            satsuma.transform.rotation = (rot_car_strip);
                            break;
                        case tpLocation.Poker:
                            satsuma.transform.position = (loc_car_poker);
                            satsuma.transform.rotation = (rot_car_poker);
                            break;
                        case tpLocation.Strawberry:
                            satsuma.transform.position = (loc_car_strawberry);
                            satsuma.transform.rotation = (rot_car_strawberry);
                            break;
                        case tpLocation.Grandma:
                            satsuma.transform.position = (loc_car_grandma);
                            satsuma.transform.rotation = (rot_car_grandma);
                            break;
                        case tpLocation.Ski:
                            satsuma.transform.position = (loc_car_ski);
                            satsuma.transform.rotation = (rot_car_ski);
                            break;
                    }
                    break;
            }
        }
    }
}
