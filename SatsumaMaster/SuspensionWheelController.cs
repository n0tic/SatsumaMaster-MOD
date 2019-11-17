using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MSCLoader;

namespace SatsumaMaster
{
    public class SuspensionWheelController
    {
        SatsumaMaster modParent;

        public bool SuspensionFixApplied;

        //Wheels & Camber
        private GameObject WHEELRR, WHEELRL, WHEELFR, WHEELFL;
        public float WheelYPosition, WheelXPosition;
        private Vector4 WheelStartYPositions, WheelStartXPositions;
        private float originalFrontWheelYPos, originalFrontWheelXPos, originalRearWheelYPos, originalRearWheelXPos, originalFrontCamber, originalRearCamber;

        public SuspensionWheelController(SatsumaMaster _modParent)
        {
            modParent = _modParent;

            try
            {
                if (WHEELRL == null)
                    WHEELRL = modParent._satsuma.transform.FindChild("RL").transform.GetChild(0).gameObject;
                if (WHEELRR == null)
                    WHEELRR = modParent._satsuma.transform.FindChild("RR").transform.GetChild(0).gameObject;
                if (WHEELFR == null)
                    WHEELFR = modParent._satsuma.transform.FindChild("FR").transform.GetChild(0).transform.GetChild(0).gameObject;
                if (WHEELFL == null)
                    WHEELFL = modParent._satsuma.transform.FindChild("FL").transform.GetChild(0).transform.GetChild(0).gameObject;

                if (WHEELRL != null && WHEELRL != null && WHEELFR != null && WHEELFL != null)
                {
                    originalFrontCamber = modParent._satsuma.GetComponent<Axles>().frontAxle.leftWheel.camber;
                    originalRearCamber = modParent._satsuma.GetComponent<Axles>().rearAxle.leftWheel.camber;
                    originalFrontWheelYPos = WHEELFL.transform.localPosition.y;
                    originalRearWheelYPos = WHEELRL.transform.localPosition.y;
                    originalFrontWheelXPos = WHEELFL.transform.localPosition.x;
                    originalRearWheelXPos = WHEELRL.transform.localPosition.x;

                    WheelStartYPositions = new Vector4(WHEELFR.transform.localPosition.y, WHEELFL.transform.localPosition.y, WHEELRR.transform.localPosition.y, WHEELRL.transform.localPosition.y);
                    WheelStartXPositions = new Vector4(WHEELFR.transform.localPosition.x, WHEELFL.transform.localPosition.x, WHEELRR.transform.localPosition.x, WHEELRL.transform.localPosition.x);
                    modParent.enableWheelMod = true;
                }
            }
            catch (Exception)
            {
                ModConsole.Error("Suspension and Wheel mod failed setup.");
            }
        }

        public void SuspensionFix()
        {
            PlayMakerFSM[] componentsInChildren = modParent._satsuma.GetComponentsInChildren<PlayMakerFSM>();
            foreach (PlayMakerFSM val in componentsInChildren)
            {
                if (val.name == "Suspension")
                {
                    val.enabled = false;
                }
            }
            SuspensionFixApplied = true;
        }

        public void RevertSuspensionFix()
        {
            PlayMakerFSM[] componentsInChildren = modParent._satsuma.GetComponentsInChildren<PlayMakerFSM>();
            foreach (PlayMakerFSM val in componentsInChildren)
            {
                if (val.name == "Suspension")
                {
                    val.enabled = true;
                }
            }
            SuspensionFixApplied = false;
        }

        public void SetWheelCambers(bool IsFront, bool IsRear, int Direction)
        {
            if (Direction == 0)
            {
                if (IsFront)
                {
                    Wheel rightWheel = modParent._satsuma.GetComponent<Axles>().frontAxle.rightWheel;
                    rightWheel.camber += 0.5f;
                    Wheel leftWheel = modParent._satsuma.GetComponent<Axles>().frontAxle.leftWheel;
                    leftWheel.camber -= 0.5f;
                }
                if (IsRear)
                {
                    Wheel rightWheel2 = modParent._satsuma.GetComponent<Axles>().rearAxle.rightWheel;
                    rightWheel2.camber += 0.5f;
                    Wheel leftWheel2 = modParent._satsuma.GetComponent<Axles>().rearAxle.leftWheel;
                    leftWheel2.camber -= 0.5f;
                }
            }
            else
            {
                if (IsFront)
                {
                    Wheel rightWheel3 = modParent._satsuma.GetComponent<Axles>().frontAxle.rightWheel;
                    rightWheel3.camber -= 0.5f;
                    Wheel leftWheel3 = modParent._satsuma.GetComponent<Axles>().frontAxle.leftWheel;
                    leftWheel3.camber += 0.5f;
                }
                if (IsRear)
                {
                    Wheel rightWheel4 = modParent._satsuma.GetComponent<Axles>().rearAxle.rightWheel;
                    rightWheel4.camber -= 0.5f;
                    Wheel leftWheel4 = modParent._satsuma.GetComponent<Axles>().rearAxle.leftWheel;
                    leftWheel4.camber += 0.5f;
                }
            }
        }

        public void SetNewWheelPositions(bool IsDirectionUp, bool IsFrontWheels, bool IsRearWheels)
        {
            if (modParent.enableWheelMod)
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
                ModConsole.Error(modParent.Name + ": Wheels is disabled. Failed setup.");
        }

        public void SetNewWheelXPositions(bool IsDirectionLeft, bool IsFrontWheels, bool IsRearWheels)
        {
            if (modParent.enableWheelMod)
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
                ModConsole.Error(modParent.Name + ": Wheels is disabled. Failed setup.");
        }
    }
}
