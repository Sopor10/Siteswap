using System;
using System.Collections;
using System.Collections.Generic;
using Utils;

namespace Siteswaps
{
    public class CyclicArray<T> 
    {
        private T[] array;

        public CyclicArray(T[] array)
        {
            this.array = array;
        }

        public CyclicArray(int size)
        {
            array = new T[size];
            for (int j = 0; j < array.Length; j++)
            {
                array[j] = default;
            }
        }
        public T this[int key]
        {
            get => array[key.Mod(array.Length)];
            set => array[key.Mod(array.Length)] = value;
        }

        public int Period => array.Length ;


        public IEnumerable<(int position, T value)> Enumerate(int i)
        {
            for (var j = 0; j < i; j++)
            {
                for (var k = 0; k < array.Length; k++)
                {
                    yield return (j * array.Length + k, array[k]);

                }

            }
        }

        public IEnumerable<T> Sequence()
        {
            int i = 0;
            while (true)
            {
                yield return this[i];
                i = i.Mod(array.Length);
                i++;
            }
        }
    }
}