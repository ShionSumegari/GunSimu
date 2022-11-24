using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeraJet
{
    [System.Serializable]
    public class PlayerData
    {
        public string _playerId;

        /*-------------PLAYER DATA-------------*/
        public int _currentSkinId;

        public int _currentWalkingId;

        public int _currentRunningId;

        public int _currentCoin;

        public int _currentDiamond;

        public string userName;

        public bool isGuest;

        /*-------------PLAYER SETTINGS DATA-------------*/

        public float _musicVolumeSettings;

        public float _soundFXVolumeSettings;

        public int _qualitySettingsIndex;

        public int _joystickSettings;

        public bool _isNotificationOn;

        public bool _isLiked;

        public bool _isRated;

        /*----------------PERFORMANCE DATA---------*/
        public int _highTravel;

        public int _hightCoinEarned;

        public int _highDiamondEarned;


        /*-------------SHOPPING DATA-------------*/
        public int[] _purchasedWalkingAnims;

        public int[] _purchasedRunningAnims;

        public int[] _purchasedCharacterSkin;

        public PlayerData(PlayerData playerData)
        {
            _currentSkinId = playerData._currentSkinId;
            userName = playerData.userName;
            _currentWalkingId = playerData._currentWalkingId;
            _currentRunningId = playerData._currentRunningId;
            _currentCoin = playerData._currentCoin;
            _currentDiamond = playerData._currentDiamond;
            _musicVolumeSettings = playerData._musicVolumeSettings;
            _soundFXVolumeSettings = playerData._soundFXVolumeSettings;
            _isNotificationOn = playerData._isNotificationOn;
            _isLiked = playerData._isLiked;
            _isRated = playerData._isRated;
            _qualitySettingsIndex = playerData._qualitySettingsIndex;
            _joystickSettings = playerData._joystickSettings;
            isGuest = playerData.isGuest;
            _highTravel = playerData._highTravel;
            _hightCoinEarned = playerData._hightCoinEarned;
            _highDiamondEarned = playerData._highDiamondEarned;
            _purchasedWalkingAnims = playerData._purchasedWalkingAnims;
            _purchasedRunningAnims = playerData._purchasedRunningAnims;
            _purchasedCharacterSkin = playerData._purchasedCharacterSkin;
        }

        public PlayerData()
        {
            _playerId = "guest-01";
            userName = "";
            _currentSkinId = 1207;
            _currentWalkingId = 1;
            _currentRunningId = 2;
            _currentCoin = 0;
            _currentDiamond = 0;
            _musicVolumeSettings = 1f;
            _soundFXVolumeSettings = 1f;
            _isNotificationOn = true;
            _qualitySettingsIndex = 2;
            _joystickSettings = 1;
            isGuest = true;
            _isLiked = false;
            _isRated = false;
            _highTravel = 0;
            _hightCoinEarned = 0;
            _highDiamondEarned = 0;
            _purchasedWalkingAnims = new int[1] { 1 };
            _purchasedRunningAnims = new int[1] { 2 };
            _purchasedCharacterSkin = new int[1] { 1207 };
        }

        public override string ToString()
        {
            return "player id: " + _playerId + "\n" + "Player current skin ID: " + _currentSkinId;
        }
    }
}
