using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace sibcite.Elsi.Server
{
  public class ModuleJobs
  {

    /// <summary>
    /// Мониторинг старых сеансов пользователей.
    /// </summary>
    public virtual void ToManySessionNoticesibcite()
    {
      var currentSessionsCount = ActiveUsers.GetAll().Count();
      var maxCount = Functions.Module.GetAppliedParamIntValue(Constants.Module.LicenseCount.Key, Constants.Module.LicenseCount.DefaultValue);
      // Если осталось менее 10%.
      if (currentSessionsCount > maxCount*0.9)
      {
        var subject = sibcite.Elsi.Resources.ToManySessionsSubjectFormat(currentSessionsCount, maxCount);
        var task = Sungero.Workflow.SimpleTasks.Create(subject, Roles.Administrators);
        task.AssignmentType = Sungero.Workflow.SimpleTask.AssignmentType.Notice;
        task.ActiveText = Hyperlinks.Get(ActiveUsers.Info);
        task.Start();
      }
    }
    
    /// <summary>
    /// Удаление автоматических уведомлений синхронизации с AX и AD старше 5 р.д.
    /// </summary>
    public virtual void DelitingOldNotificationssibcite()
    {
      var taskSubjectsForDelete = DirRX.AXIntegration.PublicFunctions.Module.Remote.GetDocflowParamsStringValue(Constants.Module.DeletingOldNotificationSubjects.Key);
      var taskIds = new List<string>();
      foreach (var subject in taskSubjectsForDelete.Split(';').Select(s => s.Trim()).Where(s => s.Length > 10))
      {
        taskIds.AddRange(Sungero.Workflow.SimpleTasks.GetAll(t => t.Status == Sungero.Workflow.SimpleTask.Status.Completed &&
                                                             t.AssignmentType == Sungero.Workflow.SimpleTask.AssignmentType.Notice &&
                                                             t.Created <= Calendar.AddWorkingDays(Calendar.Today, -5) &&
                                                             t.Subject.StartsWith(subject)).Select(t => t.Id.ToString()));
      }
      
      // Удалить в асинхронном событии, чтобы избежать ошибок сессий.
      var deleteTaskHandler = AsyncHandlers.DeleteSimpleTasksibcite.Create();
      deleteTaskHandler.TaskIds = string.Join("|", taskIds);
      deleteTaskHandler.ExecuteAsync();
      
    }
  }
}