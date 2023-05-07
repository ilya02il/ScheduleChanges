namespace ScheduleChanges.Core.Domain.BaseClasses;

/// <summary>
///     Базовый класс сущности, от которого должны наследоваться
/// все остальные сущности
/// </summary>
public abstract class BaseEntity
{
	/// <summary>
	/// Данное свойство идентифицирует сущность. Сущности,
	/// у которых данное свойство равно, считаются равными даже
	/// если остальные поля имеют разные значения.
	/// </summary>
	public virtual Guid Id { get; protected set; } = Guid.NewGuid();

	/// <summary>
	/// Базовый конструктор
	/// </summary>
	protected BaseEntity() { }

	/// <summary>
	/// Конструктор, создающий <see cref="BaseEntity"/>
	/// с <see cref="BaseEntity.Id"/> = id
	/// </summary>
	/// <param name="id">
	/// Идентификатор сущности.
	/// </param>
	protected BaseEntity(Guid id) => Id = id;

	/// <summary>
	///     Показывает равен ли объект, на котором вызывается метод
	/// аргументу <paramref name="obj"/>.
	/// </summary>
	/// <param name="obj">
	///     Объект, сравниваемый с объектом <see cref="BaseEntity"/>,
	/// у которого вызывается данный метод.
	/// </param>
	/// <returns>
	///     <see langword="true"/> если выполнено одно из условий:<br/>
	///     <list type="bullet">
	///         <item>
	///             <description>
	///					объекты имеют один тип и одинаковое
	/// 				значение поля <see cref="Id"/>;
	///				</description>
	///         </item>
	///         <item>
	///             <description>
	///					переменные указывают на один объект в памяти.
	/// 			</description>
	///         </item>
	///     </list>
	///
	///     <see langword="false"/> если выполнено одно из условий:<br/>
	///     <list type="bullet">
	///         <item>
	///             <description>объекты имеют разные типы;</description>
	///         </item>
	///         <item>
	///             <description>
	///					<paramref name="obj"/> не является объектом,
	///					унаследованным от типа <see cref="BaseEntity"/>;
	///				</description>
	///         </item>
	///         <item>
	///             <description>
	///					объекты имеют разные значения свойства <see cref="Id"/>;
	///				</description>
	///         </item>
	///         <item>
	///         	<description>
	///					у одного из объектов свойство <see cref="Id"/> имеет
	///					значение <see langword="default"/>.
	///				</description>
	///         </item>
	///     </list>
	/// </returns>
	public override bool Equals(object? obj)
	{
		if (GetType() != obj?.GetType())
			return false;

		if (obj is not BaseEntity other)
			return false;

		if (ReferenceEquals(this, other))
			return true;

		if (Id.Equals(Guid.Empty) || other.Id.Equals(Guid.Empty))
			return false;

		return Id.Equals(other.Id);
	}

	/// <summary>
	///     Оператор равенства, показывает равны ли два объекта типа <see cref="BaseEntity"/>.
	/// </summary>
	/// <returns>
	///     <see langword="true"/> если <paramref name="left"/> и <paramref name="right"/> имеют значение <see langword="null"/>.<br/>
	///     <see langword="false"/> если или <paramref name="left"/> или <paramref name="right"/> имеет значение <see langword="null"/>.<br/>
	///     Если не выполнены условия, приведенные выше, то возвращается результат работы метода <seealso cref="Equals(object)"/>.
	/// </returns>
	public static bool operator ==(BaseEntity left, BaseEntity right)
	{
		if (left is null && right is null)
			return true;

		if (left is null || right is null)
			return false;

		return left.Equals(right);
	}

	/// <summary>
	/// Оператор неравенства. Возвращает значение противоположное тому, которое вернул бы оператор <seealso cref="operator ==(BaseEntity, BaseEntity)"/>
	/// </summary>
	public static bool operator !=(BaseEntity left, BaseEntity right) => !(left == right);

	/// <inheritdoc cref="object.GetHashCode()"/>
	public override int GetHashCode()
	{
		return (GetType().Name + Id).GetHashCode();
	}
}