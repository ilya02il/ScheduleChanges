namespace ScheduleService.ServiceAPI.Enums;

/// <summary>
/// Перечисление с ролями пользователя.
/// </summary>
internal enum Roles : sbyte
{
    /// <summary>
    /// Значение по умолчанию (роль не указана).
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Роль администратора системы.
    /// </summary>
    Admin = 1,
    
    /// <summary>
    /// Роль менеджера образовательной организации.
    /// </summary>
    EducOrgManager = 2
}