using System;

namespace Utils.Monad
{
    public class Either<TL, TR>
    {
        protected readonly TL left;
        protected readonly TR right;
        protected readonly bool isLeft;

        protected Either(TL left)
        {
            this.left = left;
            isLeft = true;
        }

        protected Either(TR right)
        {
            this.right = right;
            isLeft = false;
        }

//        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);
//
//        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
//        
        public virtual T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
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

        protected internal TL GetLeftUnsafe()
        {
            if (!isLeft)
            {
                throw new NotSupportedException();
            }

            return left;
        }

        protected internal TR GetRightUnsafe()
        {
            if (isLeft)
            {
                throw new NotSupportedException();
            }

            return right;
        }

    }
}