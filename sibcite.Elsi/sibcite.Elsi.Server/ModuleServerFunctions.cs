using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace sibcite.Elsi.Server
{
  public class ModuleFunctions
  {
    /// <summary>
    /// Получить значение прикладного параметра в int.
    /// </summary>
    /// <param name="paramName">Парметр.</param>
    /// <param name="defaultValue">Значение по-умолчанию.</param>
    /// <returns>Значение параметра.</returns>
    [Remote, Public]
    public int GetAppliedParamIntValue(string paramName, string defaultValue)
    {
      // Если значение параметра не удалось преобразовать в int, взять значение по умочанию.
      var paramValue = int.Parse(defaultValue);
      int.TryParse(DirRX.AXIntegration.PublicFunctions.Module.Remote.GetDocflowParamsStringValue(paramName), out paramValue);
      return paramValue;
    }

  }
}