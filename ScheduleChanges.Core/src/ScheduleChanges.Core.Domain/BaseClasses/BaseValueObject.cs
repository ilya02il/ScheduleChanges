using ScheduleChanges.Core.Domain.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace ScheduleChanges.Core.Domain.BaseClasses;

/// <summary>
/// Базовый класс, от которого должны наследоваться все ValueObject.<br/>
/// Два объекта, унаследованных от типа <see cref="BaseValueObject"/> равны, если значения <b>всех</b> их свойств равны.
/// </summary>
public abstract class BaseValueObject<TInherited> where TInherited : BaseValueObject<TInherited>
{
    private readonly static Func<TInherited, IEnumerable<object>> EqualityComponentsGetter = CompileEqualityComponentsGetter();

    private static Func<TInherited, IEnumerable<object>> CompileEqualityComponentsGetter()
    {
        var propsCollection = typeof(TInherited)
            .GetProperties()
            .Where(prop => prop.GetCustomAttribute<NotIncludeToEqualityComponentsAttribute>() is null);

        var propsCollectionCount = propsCollection.Count();

        var expressionsCollection = new List<Expression>();

        var listVariable = Expression.Variable(typeof(List<object>), "values");
        var listCtorInfo = typeof(List<object>).GetConstructor(new[] { typeof(int) });
        var listInstance = Expression.New(listCtorInfo!, Expression.Constant(propsCollectionCount, typeof(int)));
        var listAssign = Expression.Assign(listVariable, listInstance);

        expressionsCollection.Add(listAssign);

        var listAddMethodInfo = typeof(List<object>).GetMethod("Add", new[] { typeof(object) });

        var inheritedParam = Expression.Parameter(typeof(TInherited), "inherited");
        var listAddExpressions = propsCollection
            .Select(prop =>
            {
                var castedPropValue = Expression.Convert(
                    Expression.Property(inheritedParam, prop),
                    typeof(object));

                return Expression.Call(listVariable, listAddMethodInfo!, castedPropValue);
            });

        expressionsCollection.AddRange(listAddExpressions);

        var returnTarget = Expression.Label(typeof(List<object>));
        var returnExpression = Expression.Return(returnTarget, listVariable, typeof(List<object>));
        var returnLabel = Expression.Label(returnTarget, Expression.Constant(new List<object>()));

        expressionsCollection.Add(returnExpression);
        expressionsCollection.Add(returnLabel);

        var lambdaExpressionBlock = Expression.Block(
            new ParameterExpression[] { listVariable },
            expressionsCollection);

        var lambda = Expression.Lambda<Func<TInherited, IEnumerable<object>>>(lambdaExpressionBlock, inheritedParam);

        return lambda.Compile();
    }

    /// <summary>
    ///     Оператор равенства, показывает равны ли два объекта типа <see cref="BaseValueObject{TInherited}"/>.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> если результат работы метода <see cref="Equals(object)"/> не равен <see langword="false"/>.<br/>
    ///     <see langword="false"/> если выполнено одно из условий:<br/>
    ///     <list type="bullet">
    ///         <item>
    ///             <description>один из объектов имеет значение <see langword="null"/>;</description>
    ///         </item>
    ///         <item>
    ///             <description>результат работы метода <see cref="Equals(object)"/> равен <see langword="false"/>.</description>
    ///         </item>
    ///     </list>
    /// </returns>
    protected static bool EqualOperator(BaseValueObject<TInherited> left, BaseValueObject<TInherited> right)
    {
        if (left is null ^ right is null)
            return false;

        return left?.Equals(right) != false;
    }

    /// <summary>
    /// Оператор неравенства. Возвращает значение противоположное тому, которое вернул бы оператор <seealso cref="EqualOperator(BaseValueObject{TInherited}, BaseValueObject{TInherited})"/>
    /// </summary>
    protected static bool NotEqualOperator(BaseValueObject<TInherited> left, BaseValueObject<TInherited> right) => !EqualOperator(left, right);

    /// <inheritdoc cref="EqualOperator(BaseValueObject{TInherited}, BaseValueObject{TInherited})"/>
    public static bool operator ==(BaseValueObject<TInherited> left, BaseValueObject<TInherited> right) => EqualOperator(left, right);

    /// <inheritdoc cref="NotEqualOperator(BaseValueObject{TInherited}, BaseValueObject{TInherited})"/>
    public static bool operator !=(BaseValueObject<TInherited> left, BaseValueObject<TInherited> right) => NotEqualOperator(left, right);

    /// <summary>
    ///     Показывает равен ли объект, на котором вызывается метод аргументу <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">
    ///     Объект, сравниваемый с объектом <see cref="BaseValueObject{TInherited}"/>, у которого вызывается данный метод.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> если:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>объекты имеют одинаковый тип и <paramref name="obj"/> не равен <see langword="null"/>;</description>
    ///         </item>
    ///         <item>
    ///             <description>если значения свойств, не помеченных аттрибутом <see cref="NotIncludeToEqualityComponentsAttribute"/>, у обоих объектов, равны.</description>
    ///         </item>
    ///     </list>
    ///
    ///     <br/>
    ///     <see langword="false"/> если выполнено одно из условий:<br/>
    ///     <list type="bullet">
    ///         <item>
    ///             <description>объект <paramref name="obj"/> имеет тип отличный от <typeparamref name="TInherited"/>;</description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 если значения свойств, не помеченных аттрибутом <see cref="NotIncludeToEqualityComponentsAttribute"/>, у обоих объектов, не равны.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not TInherited other)
            return false;

        var thisEqualityComponents = EqualityComponentsGetter((TInherited)this);

        return thisEqualityComponents
            .SequenceEqual(EqualityComponentsGetter(other));
    }

    /// <inheritdoc cref="object.GetHashCode()"/>
    public override int GetHashCode()
    {
        return EqualityComponentsGetter((TInherited)this)
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate(GetType().GetHashCode(), (x, y) => x ^ y);
    }
}
