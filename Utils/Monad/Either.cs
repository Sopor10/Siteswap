using System;

namespace Utils.Monad
{
    public class Either<TL, TR>
    {
        private readonly TL left;
        private readonly TR right;
        private readonly bool isLeft;

        private Either(TL left)
        {
            this.left = left;
            isLeft = true;
        }

        private Either(TR right)
        {
            this.right = right;
            isLeft = false;
        }

        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
        
        public virtual T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
            => this.isLeft ? leftFunc(this.left) : rightFunc(this.right);

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

    }
}