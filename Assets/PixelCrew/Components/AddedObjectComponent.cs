using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Components
{
    public class AddedObjectComponent : MonoBehaviour
    {
        [SerializeField] private int _countSilverToGold;
        private static int _coinsSilver;
        private static int  _coinsGold;
        
        public void AddedObject()
        {
            _coinsSilver++;

            if (_coinsSilver >= _countSilverToGold)
            {
                _coinsSilver = 0;
                _coinsGold++;
            }
            
            Debug.Log($"Gold Coins: {_coinsGold}, Silver Coins {_coinsSilver}");
        }
    }
}

