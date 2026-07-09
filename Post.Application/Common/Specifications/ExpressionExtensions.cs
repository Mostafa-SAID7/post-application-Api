using System.Linq.Expressions;

namespace Post.Application.Common.Specifications
{
    /// <summary>
    /// Expression tree combining helpers for specifications
    /// </summary>
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftInvoked = Expression.Invoke(left, parameter);
            var rightInvoked = Expression.Invoke(right, parameter);
            var and = Expression.AndAlso(leftInvoked, rightInvoked);

            return Expression.Lambda<Func<T, bool>>(and, parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftInvoked = Expression.Invoke(left, parameter);
            var rightInvoked = Expression.Invoke(right, parameter);
            var or = Expression.OrElse(leftInvoked, rightInvoked);

            return Expression.Lambda<Func<T, bool>>(or, parameter);
        }
    }
}
