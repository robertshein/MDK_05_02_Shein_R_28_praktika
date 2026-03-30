using praktika28_Shein.Classes;
using praktika28_Shein.Context;
using praktika28_Shein.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace praktika28_Shein.ViewModels
{
    public class VM_Tasks : Notification
    {
        public TaskContext tasksContext = new TaskContext();

        public ObservableCollection<Tasks> Tasks { get; set; }
        public VM_Tasks()
        {
            Tasks = new ObservableCollection<Tasks>(tasksContext.Tasks.OrderBy(x => x.Done));
        }
        public RelayCommand OnAddTask
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    // Создаём новую задачу с текущей датой выполнения
                    Tasks newTask = new Tasks
                    {
                        DateExecute = DateTime.Now,
                        Name = "Новая задача",
                        Priority = "Средний",
                        Comment = ""
                    };
                    Tasks.Add(newTask);                     // Добавляем в коллекцию
                    tasksContext.Tasks.Add(newTask);        // Добавляем в контекст
                    tasksContext.SaveChanges();             // Сохраняем в БД
                });
            }
        }
    }
}