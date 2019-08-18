using System;

namespace Utils.Monad
{
    public struct Either<TL, TR>
    {
        private readonly TL left;
        private readonly TR right;
        private readonly bool isLeft;

        public Either(TL left)
        {
            this.left = left;
            isLeft = true;
            right = default;
        }

        public Either(TR right)
        {
            this.right = right;
            isLeft = false;
            left = default;
        }

        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
        
        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
        {
            if (isLeft)
            {
                return leftFunc(left);
            }
            else
            {
                return rightFunc(right);
            }
        }

        public bool IsLeft => isLeft;

        public TL GetLeftUnsafe()
        {
            if (!isLeft)
            {
                throw new NotSupportedException();
            }

            return left;
        }

        public TR GetRightUnsafe()
        {
            if (isLeft)
            {
                throw new NotSupportedException();
            }

            return right;
        }

        public void DoLeft(Action<TL> func)
        {
            if (isLeft)
            {
                func.Invoke(left);
            }
        }

    }
}