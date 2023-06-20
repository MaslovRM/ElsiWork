using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace sibcite.Elsi.Server
{
  public class ModuleAsyncHandlers
  {

    /// <summary>
    /// Закрыть право подписи сотрудника.
    /// </summary>
    /// <param name="args">Параметры вызова асинхронного обработчика.</param>
    public virtual void CloseSignatureSetting(sibcite.Elsi.Server.AsyncHandlerInvokeArgs.CloseSignatureSettingInvokeArgs args)
    {
      var signatureSettings = Sungero.Docflow.SignatureSettings.GetAll(s => s.Recipient != null &&  s.Recipient.Id == args.EmployeeId
                                                                       && s.Status != Sungero.CoreEntities.DatabookEntry.Status.Closed);
      foreach (var signatureSetting in signatureSettings)
      {
        // Если запись заблокирована, повторить.
        var signatureLock = Locks.GetLockInfo(signatureSetting);
        if (signatureLock.IsLocked)
        {
          if (args.RetryIteration < 10)
            args.Retry = true;
          
          continue;
        }
        
        signatureSetting.Status = Sungero.CoreEntities.DatabookEntry.Status.Closed;
        signatureSetting.Save();
      }
    }

    /// <summary>
    /// Удалить простую задачу.
    /// </summary>
    /// <param name="args">Параметры вызова асинхронного обработчика.</param>
    public virtual void DeleteSimpleTasksibcite(sibcite.Elsi.Server.AsyncHandlerInvokeArgs.DeleteSimpleTasksibciteInvokeArgs args)
    {
      
      if (string.IsNullOrWhiteSpace(args.TaskIds))
        return;
      
      var taskIds = args.TaskIds.Split('|').ToList();
      Logger.DebugFormat(Resources.DeletingSimpleTaskNotificationTaskCount, taskIds.Count());
      if (!taskIds.Any())
        return;
      
      var taskId = taskIds.First();
      var task = Sungero.Workflow.SimpleTasks.GetAll(t => t.Id == int.Parse(taskId)).FirstOrDefault();
      if (task != null)
      {
        try
        {
          var logMessage = string.Format(Resources.DeletingSimpleTaskNotificationLogMessage, task.Id, task.Subject);
          Sungero.Workflow.SimpleTasks.Delete(task);
          Logger.Debug(logMessage);
        }
        catch(Exception ex)
        {
          Logger.ErrorFormat(Resources.DeletingSimpleTaskNotificationLogError, task.Id, task.Subject, ex.Message);
        }
      }
      
      taskIds.Remove(taskId);
      if (taskIds.Any())
      {
        var deleteTaskHandler = AsyncHandlers.DeleteSimpleTasksibcite.Create();
        deleteTaskHandler.TaskIds = string.Join("|", taskIds);
        deleteTaskHandler.ExecuteAsync();
      }
    }
  }
}