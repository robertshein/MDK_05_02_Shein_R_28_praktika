using praktika28_Shein.Classes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using System.Windows.Input;

namespace praktika28_Shein.Models
{
    public class Tasks : Notification
    {
        public int Id { get; set; }

        private string name;
        private int priorityId;
        private DateTime dateExecute;
        private string comment;
        private bool done;
        private bool isEnable;

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                {
                    MessageBox.Show("Наименование не должно быть пустым и не более 50 символов.",
                                    "Некорректный ввод значения", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PriorityId
        {
            get => priorityId;
            set
            {
                if (value <= 0)
                {
                    MessageBox.Show("Выберите корректный приоритет.",
                                    "Некорректный ввод значения", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    priorityId = value;
                    OnPropertyChanged();
                }
            }
        }

        [ForeignKey(nameof(PriorityId))]
        public virtual Priority Priority { get; set; }

        public DateTime DateExecute
        {
            get => dateExecute;
            set
            {
                if (value.Date < DateTime.Now.Date)
                {
                    MessageBox.Show("Дата выполнения не может быть меньше текущей.",
                                    "Некорректный ввод значения", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    dateExecute = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comment
        {
            get => comment;
            set
            {
                if (value != null && value.Length > 1000)
                {
                    MessageBox.Show("Комментарий не должен превышать 1000 символов.",
                                    "Некорректный ввод значения", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Done
        {
            get => done;
            set
            {
                done = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDoneText));
            }
        }

        [NotMapped]
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEnableText));
            }
        }

        [NotMapped]
        public string IsEnableText => IsEnable ? "Сохранить" : "Изменить";

        [NotMapped]
        public string IsDoneText => Done ? "Не выполнено" : "Выполнено";

        [NotMapped]
        public ICommand OnEdit => new RelayCommand(obj =>
        {
            IsEnable = !IsEnable;
            if (!IsEnable)
            {
                var context = (MainWindow.init.DataContext as ViewModels.VM_Pages)?.vm_tasks?.tasksContext;
                context?.SaveChanges();
            }
        });

        [NotMapped]
        public ICommand OnDelete => new RelayCommand(obj =>
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить задачу?",
                                "Предупреждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var vm = MainWindow.init.DataContext as ViewModels.VM_Pages;
                vm?.vm_tasks.Tasks.Remove(this);
                vm?.vm_tasks.tasksContext.Remove(this);
                vm?.vm_tasks.tasksContext.SaveChanges();
            }
        });

        [NotMapped]
        public ICommand OnDone => new RelayCommand(obj =>
        {
            Done = !Done;
            var context = (MainWindow.init.DataContext as ViewModels.VM_Pages)?.vm_tasks?.tasksContext;
            context?.SaveChanges();
        });
    }
}