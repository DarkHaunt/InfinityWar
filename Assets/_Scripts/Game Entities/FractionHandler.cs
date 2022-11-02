using System;
using UnityEngine;


namespace InfinityGame.Fractions
{
    public abstract class FractionHandler : MonoBehaviour
    {
        [SerializeField] protected FractionType _fractionTag;


        public FractionType Fraction => _fractionTag;

        public bool IsBelongsToFraction(FractionType fraction) => (_fractionTag & fraction) != 0;



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
