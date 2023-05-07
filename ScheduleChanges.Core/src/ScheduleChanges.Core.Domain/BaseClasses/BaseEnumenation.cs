namespace ScheduleChanges.Core.Domain.BaseClasses;

/// <summary>
///     Базовый класс перечисления "Java like".
/// </summary>
/// <example>
///     Пример реализации перечисления, наследуемого от <see cref="BaseEnumenation{TValue}"/>:
///     <code>
///         public class Color : BaseEnumenation
///         {
///             public static Color Red      = new Color(0, "#FF0000");
///             public static Color Green    = new Color(1, "#00FF00");
///             public static Color Blue     = new Color(2, "#0000FF");
///         }
///     </code>
/// </example>
public abstract class BaseEnumenation<TValue> : IComparable<TValue>, IComparable
{
	/// <summary>
	/// Идентификатор элемента перечисления
	/// </summary>
	public int Id { get; protected set; }

	/// <summary>
	/// Значение элемента перечисления
	/// </summary>
	public TValue Value { get; protected set; }

	/// <summary>
	/// 	Конструктор, создающий объект перечисления с
	/// 	<see cref="Id"/> = <paramref name="id"/> и
	/// 	<see cref="Value"/> = <paramref name="value"/>
	/// </summary>
	/// <param name="id">
	///     <see cref="Id"/>
	/// </param>
	/// <param name="value">
	///     <see cref="Value"/>
	/// </param>
	protected BaseEnumenation(int id, TValue value) => (Id, Value) = (id, value);

	/// <summary>
	///     Показывает равен ли объект, на котором вызывается метод аргументу <paramref name="obj"/>.
	/// </summary>
	/// <returns>
	///     <see langword="true"/> если объекты имеют одинаковый тип и значения их свойств <see cref="Id"/> равны.
	/// </returns>
	public override bool Equals(object? obj)
	{
		if (obj is not BaseEnumenation<TValue> other)
			return false;

		return Id.Equals(other.Id);
	}

	/// <summary>
	///     Оператор равенства, показывает равны ли два объекта типа <see cref="BaseEnumenation{TValue}"/>.
	/// </summary>
	/// <returns>
	///     <see langword="true"/> если выполнено одно из условий:
	///     <list type="bullet">
	///         <item>
	///             <description>Оба объекта имеют значение <see langword="null"/>;</description>
	///         </item>
	///         <item>
	///             <description>Результат вызова метода <see cref="Equals(object)"/> - <see langword="true"/>.</description>
	///         </item>
	///     </list>
	///
	///     <see langword="false"/> если выполнено одно из условий:
	///     <list type="bullet">
	///         <item>
	///             <description>Один из объектов имеет значение <see langword="null"/>;</description>
	///         </item>
	///         <item>
	///             <description>Результат вызова метода <see cref="Equals(object)"/> - <see langword="false"/>.</description>
	///         </item>
	///     </list>
	/// </returns>
	public static bool operator ==(BaseEnumenation<TValue> left, BaseEnumenation<TValue> right)
	{
		if (left is null && right is null)
			return true;

		if (left is null || right is null)
			return false;

		return left.Equals(right);
	}

	/// <summary>
	/// Оператор неравенства. Возвращает значение противоположное тому, которое вернул бы оператор <seealso cref="operator ==(BaseEnumenation{TValue}, BaseEnumenation{TValue})"/>
	/// </summary>
	public static bool operator !=(BaseEnumenation<TValue> left, BaseEnumenation<TValue> right) => !(left == right);

	/// <summary>
	/// Оператор неявного приведения <see cref="BaseEnumenation{TValue}"/> к типу <see cref="int"/>.
	/// </summary>
	/// <param name="enumenation"></param>
	public static implicit operator int(BaseEnumenation<TValue> enumenation) => enumenation.Id;

	/// <inheritdoc cref="IComparable.CompareTo(object?)"/>
	public int CompareTo(object? obj)
	{
		if (obj is null)
			return -1;

		if (obj is not BaseEnumenation<TValue> other)
			throw new Exception("An object, that was boxed within the 'obj' argument, has a different type.");

		return Id.CompareTo(other.Id);
	}

	/// <inheritdoc cref="IComparable{T}.CompareTo"/>
	public int CompareTo(TValue? other) => CompareTo(other as object);

	/// <summary>
	/// Получить хэш-код объекта с типом, унаследованным от <see cref="BaseEnumenation{TValue}"/>
	/// </summary>
	/// <returns>
	/// Результат работы метода <see cref="GetHashCode()"/> для операции XOR хэш-кода типа, <see cref="Id"/> и хэш-кода <see cref="Value"/>.
	/// </returns>
	public override int GetHashCode() =>
		(GetType().GetHashCode() ^ Id ^ Value?.GetHashCode()).GetHashCode();
}
