using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace sibcite.Elsi.Server
{
  public partial class ModuleInitializer
  {

    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      // Создание прикладных настроек.
      CreateAppliedSettings();
    }
    
    /// <summary>
    /// Создать прикладные настройки.
    /// </summary>
    public static void CreateAppliedSettings()
    {
      CreateAppliedSetting(Constants.Module.DeletingOldNotificationSubjects.Key, Constants.Module.DeletingOldNotificationSubjects.DefaultValue);
      CreateAppliedSetting(Constants.Module.LicenseCount.Key, Constants.Module.LicenseCount.DefaultValue);
      CreateAppliedSetting(Constants.Module.CheckContractExpiringWorkingDays.Key, Constants.Module.CheckContractExpiringWorkingDays.DefaultValue);
    }
    
    /// <summary>
    /// Создать настройку.
    /// </summary>
    public static void CreateAppliedSetting(string paramName, string paramDefaultValue)
    {
      // Записать значение по-умолчанию, если параметра еще не было.
      var currentValue = Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(paramName);
      if (currentValue == null)
        DirRX.AXIntegration.PublicFunctions.Module.Remote.InsertOrUpdateDocflowParam(paramName, paramDefaultValue);
    }
  }
}
