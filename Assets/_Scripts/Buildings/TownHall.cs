using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.Buildings
{
    public class TownHall : Barrack
    {
        public static TownHall Instantiate(TownHall prefab, Fraction fractionData)
        {
            var townHall = Instantiate(prefab);
            townHall._spriteRenderer.sprite = fractionData.TownHallSprite;

            return townHall;
        }
    } 
}
