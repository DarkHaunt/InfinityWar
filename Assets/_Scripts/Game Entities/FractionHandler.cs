using System;
using UnityEngine;


namespace InfinityGame.Fractions
{
    public abstract class FractionHandler : MonoBehaviour
    {
        [SerializeField] protected FractionType _fractionType;


        public FractionType Fraction => _fractionType;

        public bool IsBelongsToFraction(FractionType fraction) => (_fractionType & fraction) != 0;



        public enum FractionType
        {
            None = 0,
            Human = 0x01,
            Zombie = 0x02,
            Demon = 0x04,
            Elf = 0x08
        }
    } 
}
