using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MSCLoader;

namespace SatsumaMaster
{
    public class SoundController
    {
        SatsumaMaster modParent;

        //SoundMod
        public GameObject _bovSound;
        public List<AudioClip> _sfxSounds = new List<AudioClip>();
        public int bovChoice = 1;
        public float bovVolume = 1f;
        public bool enableSFX, sfx;

        public SoundController(SatsumaMaster _modParent, AssetBundle assets)
        {
            modParent = _modParent;
            try
            {
                _bovSound = assets.LoadAsset("BovSound.prefab") as GameObject;
                _sfxSounds.Add(assets.LoadAsset("SpoolSound.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("BOV.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("1.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("2.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("3.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("4.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("5.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("6.wav") as AudioClip);
                _sfxSounds.Add(assets.LoadAsset("7.wav") as AudioClip);

                _bovSound = GameObject.Instantiate(_bovSound);

                _bovSound.transform.parent = GameObject.Find("PLAYER").transform;
                _bovSound.transform.localPosition = new Vector3(0f, 0f, 0f);
                _bovSound.transform.position = new Vector3(0f, 0f, 0f);

                if (_sfxSounds.Count >= 9)
                    enableSFX = true;
            }
            catch (Exception)
            {
                ModConsole.Error("SoundMod failed setup.");
            }
        }
    }
}
