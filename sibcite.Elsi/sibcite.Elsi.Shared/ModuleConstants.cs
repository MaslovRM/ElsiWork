using System;
using Sungero.Core;

namespace sibcite.Elsi.Constants
{
  public static class Module
  {

    // Guid типа документа "Договор".
    [Public]
    public const string ContractTypeGuid = "f37c7e63-b134-4446-9b5b-f8811f6c9666";
    
    #region Прикладные настройки.
    
    /// <summary>
    /// Параметры удаления уведомлений.
    /// </summary>
    public static class DeletingOldNotificationSubjects
    {
      public const string Key = "DeletingOldNotificationSubjects";
      public const string DefaultValue = "Неудачная попытка отправки;Превышено количество попыток вызова обработчика;Ошибка синхронизации;Сгенерированы новые учетные записи;Ошибка заполнения руководителей и головных подразделений;Ошибка обработки пакета из Ario";
    }
    
    /// <summary>
    /// Количество лицензий.
    /// </summary>
    /// TODO. Удалить, когда будет способ подсчета лицензий програмно.
    public static class LicenseCount
    {
      public const string Key = "LicenseCount";
      public const string DefaultValue = "300";
    }
    
    /// <summary>
    /// За сколько рабочих дней предупреждать о завершении договора.
    /// </summary>
    public static class CheckContractExpiringWorkingDays
    {
      [Public]
      public const string Key = "CheckContractExpiringWorkingDays";
      [Public]
      public const string DefaultValue = "3";
    }
    #endregion

  }
}