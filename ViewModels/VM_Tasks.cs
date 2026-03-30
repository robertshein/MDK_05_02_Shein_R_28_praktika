using Microsoft.EntityFrameworkCore;
using praktika28_Shein.Classes;
using praktika28_Shein.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace praktika28_Shein.ViewModels
{
    public class VM_Tasks : Notification
    {
        public TaskContext tasksContext = new TaskContext();
        public ObservableCollection<Tasks> Tasks { get; set; }
        public ObservableCollection<Priority> Priorities { get; set; }

        private ICollectionView _view;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    _view?.Refresh();
                }
            }
        }

        public VM_Tasks()
        {
            Priorities = new ObservableCollection<Priority>(
                tasksContext.Priorities.OrderBy(p => p.SortOrder)
            );

            var sortedTasks = tasksContext.Tasks
                .Include(t => t.Priority)
                .OrderBy(t => t.Priority.SortOrder)
                .ThenBy(t => t.DateExecute)
                .ToList();

            Tasks = new ObservableCollection<Tasks>(sortedTasks);

            _view = CollectionViewSource.GetDefaultView(Tasks);
            _view.Filter = obj => Filter(obj as Tasks);
        }

        private bool Filter(Tasks task)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return true;
            return task.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public RelayCommand OnAddTask
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    int defaultPriorityId = Priorities.FirstOrDefault()?.Id ?? 0;

                    Tasks newTask = new Tasks
                    {
                        DateExecute = DateTime.Now,
                        Name = "Новая задача",
                        PriorityId = defaultPriorityId,
                        Comment = ""
                    };

                    Tasks.Add(newTask);                     // Добавляем в коллекцию
                    tasksContext.Tasks.Add(newTask);        // Добавляем в контекст
                    tasksContext.SaveChanges();             // Сохраняем в БД

                    _view?.Refresh();
                });
            }
        }
    }
}