using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace sibcite.Elsi.Client
{
  public class ModuleFunctions
  {
    /// <summary>
    /// Посмотреть и установить прикладные настройки.
    /// </summary>
    [Public]
    public virtual void ShowAppliedSettings()
    {
      // Проверить права.
      if (!Sungero.CoreEntities.Users.Current.IncludedIn(Roles.Administrators))
      {
        Dialogs.ShowMessage(Resources.AccessDenied, Resources.AppliedSettingsAccessDenied, MessageType.Warning);
        return;
      }
      
      var dialog = Dialogs.CreateInputDialog(Resources.AppliedSettingsDialogTitle);
      
      // Поля.
      var appliedSettings = GetAppliedSettings();
      var settingName = dialog.AddSelect(Resources.AppliedSettingsDialogSetting, true).From(appliedSettings);
      var settingValue = dialog.AddMultilineString(Resources.AppliedSettingsDialogValue, false);
      settingValue.IsEnabled = false;
      
      // Кнопки.
      var saveButton = dialog.Buttons.AddCustom(Resources.AppliedSettingsDialogSave);
      saveButton.IsEnabled = false;
      var cancelButton = dialog.Buttons.AddCustom(Resources.AppliedSettingsDialogCancel);
      cancelButton.IsEnabled = false;
      
      // События полей.
      settingName.SetOnValueChanged((x) =>
                                    {
                                      if (string.IsNullOrWhiteSpace(x.NewValue))
                                      {
                                        settingValue.Value = string.Empty;
                                        settingValue.IsEnabled = false;
                                      }
                                      else
                                      {
                                        settingValue.Value = DirRX.AXIntegration.PublicFunctions.Module.Remote.GetDocflowParamsStringValue(x.NewValue);
                                        settingValue.IsEnabled = true;
                                      }
                                      
                                      saveButton.IsEnabled = false;
                                      cancelButton.IsEnabled = false;
                                      settingName.IsEnabled = true;
                                    }
                                   );
      
      settingValue.SetOnValueChanged((x) =>
                                     {
                                       saveButton.IsEnabled = true;
                                       cancelButton.IsEnabled = true;
                                       settingName.IsEnabled = false;
                                     }
                                    );
      
      // Событие по кнопке.
      dialog.SetOnButtonClick(h =>
                              { if (h.Button == saveButton)
                                {
                                  h.CloseAfterExecute = false;
                                  settingValue.Value = settingValue.Value.Trim();
                                  DirRX.AXIntegration.PublicFunctions.Module.Remote.InsertOrUpdateDocflowParam(settingName.Value, settingValue.Value);
                                  saveButton.IsEnabled = false;
                                  cancelButton.IsEnabled = false;
                                  settingName.IsEnabled = true;
                                }
                                else if (h.Button == cancelButton)
                                {
                                  h.CloseAfterExecute = false;
                                  settingValue.Value = DirRX.AXIntegration.PublicFunctions.Module.Remote.GetDocflowParamsStringValue(settingName.Value);
                                  saveButton.IsEnabled = false;
                                  cancelButton.IsEnabled = false;
                                  settingName.IsEnabled = true;
                                }
                              });
      settingName.Value = appliedSettings.First();
      dialog.Show();
    }
    
    /// <summary>
    /// Получить список прикладных настроек.
    /// </summary>
    /// <returns>Массив строк.</returns>
    public virtual string[] GetAppliedSettings()
    {
      return new string[]{
        Constants.Module.DeletingOldNotificationSubjects.Key,
        Constants.Module.LicenseCount.Key,
        Constants.Module.CheckContractExpiringWorkingDays.Key
      };
    }

  }
}