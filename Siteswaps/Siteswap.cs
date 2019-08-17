using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Utils;

namespace Siteswaps
{
    public class Siteswap
    {
        private CyclicArray<int> siteswap;

        public Siteswap(int siteswap) 
        {
            Setup(Split(siteswap));
        }

        public int Period => siteswap.Period;

        private void Setup(IEnumerable<int> siteswap)
        {
            this.siteswap = new CyclicArray<int>(siteswap.ToArray());
        }
      
        public bool IsValid()
        {
            var siteswapPlusPositionModPeriode = siteswap.Enumerate(1).Select(x => (x.position + x.value).Mod(siteswap.Period));
            return siteswapPlusPositionModPeriode.ContainsAllItemsFrom(Enumerable.Range(0,siteswap.Period));
        }
        
        private static IEnumerable<int> Split(int siteswap)
        {
            var list = new List<int>();
            while (siteswap>0)
            {
                list.Add(siteswap.Mod(10)); 
                siteswap /= 10;
            }

            list.Reverse();
            return list;
        }

        public bool IsGroundState() => HasNoRethrow();

        private bool HasNoRethrow() => !siteswap.Enumerate(1).Any(x => x.position + x.value < NumberOfObjects());

        public bool IsExcitedState() => !IsGroundState();

        public decimal NumberOfObjects() => (decimal)siteswap.Enumerate(1).Average(x => x.value);

        public Maybe<Siteswap> CalculateGetIn()
        {
            if (IsGroundState())
            {
                return new Maybe<Siteswap>();
            }

            Siteswap basePattern = new Siteswap((int)NumberOfObjects());

            basePattern
                .Sequence()
                .Take(siteswap.Period);
                
            throw new NotImplementedException();
        }

        private IEnumerable<int> Sequence()
        {
            return siteswap.Sequence();
        }

        public int MaxThrow()
        {
            return siteswap.Enumerate(1).Max(x => x.value);
        }

        public int this[int key] => siteswap[key];

        public CyclicArray<HandStatus> GenerateHandStatus()
        {
            var result = new HandStatus[MaxThrow()];
            for (int i = 0; i < siteswap.Period; i++)
            {
                var test = i + siteswap[i];
                if (test < MaxThrow())
                {
                    result[test] = HandStatus.Full;
                }
            }

            return new CyclicArray<HandStatus>(result);
        }
    }
}