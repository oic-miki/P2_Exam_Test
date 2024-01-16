using P2_Exam_Test.Model;
using P2_Exam_Test.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2_Exam_Test.ViewModel
{
    internal class ListWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ToDoItem> ToDoItems { get; } = new ObservableCollection<ToDoItem>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand NewButtonCommand { get; }
        public ICommand EditButtonCommand { get; }

        public ListWindowViewModel()
        {
            var newButtonCommand = new NewButtonCommandImpl();
            newButtonCommand.CommandExecute += NewButtonCommand_CommandExecute;
            NewButtonCommand = newButtonCommand;
            var editButtonCommand = new EditButtonCommandImpl();
            editButtonCommand.CommandExecute += EditButtonCommand_CommandExecute;
            EditButtonCommand = editButtonCommand;
        }

        private void EditButtonCommand_CommandExecute(object? sender, int? e)
        {
            CommandExecute(e);
        }

        private void NewButtonCommand_CommandExecute(object? sender, EventArgs e)
        {
            CommandExecute(null);
        }

        private void CommandExecute(int? id)
        {
            var editWindow = new EditWindow();
            var editWindowDoneCommand = new EditWindowDoneCommandImpl(editWindow);
            editWindowDoneCommand.CommandExecute += EditWindowDoneCommand_CommandExecute;
            var editWindowDeleteCommand = new EditWindowDeleteCommandImpl(editWindow, id.HasValue);
            editWindowDeleteCommand.CommandExecute += EditWindowDeleteCommand_CommandExecute;
            var editWindowViewModel = new EditWindowViewModel()
            {
                ButtonCaption = id.HasValue? "更新" : "追加",
                WindowCaption = (id.HasValue && ToDoItems.SingleOrDefault(x => x.Id == id)?.Limit != null) ? "編集画面" : "追加画面",
                ToDoItem = ToDoItems.SingleOrDefault(x => x.Id == id)?.Copy() ?? new ToDoItem(),
                DoneCommand = editWindowDoneCommand,
                DeleteCommand = editWindowDeleteCommand,
            };
            editWindow.DataContext = editWindowViewModel;
            editWindow.ShowDialog();
        }

        private void EditWindowDeleteCommand_CommandExecute(object? sender, ToDoItem e)
        {
            if (e.Id.HasValue)
            {
                ToDoItems.Remove(ToDoItems.Single(x => x.Id == e.Id.Value));
            }
        }

        private void EditWindowDoneCommand_CommandExecute(object? sender, ToDoItem e)
        {
            if (e.Id.HasValue)
            {
                var target = ToDoItems.Single(x => x.Id == e.Id.Value);
                target.TaskName = e.TaskName;
                target.Limit = e.Limit;
                target.Done = e.Done;
            }
            else
            {
                e.Id = ToDoItems.Count > 0 ? ToDoItems.Max(x => x.Id) + 1 : 1;
                ToDoItems.Add(e);
            }
        }

        private class NewButtonCommandImpl : ICommand
        {
            public event EventHandler? CanExecuteChanged;

            public event EventHandler? CommandExecute;

            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public void Execute(object? parameter)
            {
                CommandExecute?.Invoke(this, new EventArgs());
            }
        }

        private class EditWindowDoneCommandImpl : ICommand
        {
            public event EventHandler? CanExecuteChanged;

            public event EventHandler<ToDoItem>? CommandExecute;
            private EditWindow EditWindow { get; }

            public EditWindowDoneCommandImpl(EditWindow editWindow)
            {
                EditWindow = editWindow;
            }

            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public void Execute(object? parameter)
            {
                if (parameter is ToDoItem toDoItem && !string.IsNullOrWhiteSpace(toDoItem.TaskName))
                {
                    CommandExecute?.Invoke(this, toDoItem);
                    EditWindow.Close();
                }
                else
                {
                    MessageBox.Show("内容を記入してください", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private class EditWindowDeleteCommandImpl : ICommand
        {
            public event EventHandler? CanExecuteChanged;

            public event EventHandler<ToDoItem>? CommandExecute;
            private EditWindow EditWindow { get; }

            private bool IsEnabled { get; }

            public EditWindowDeleteCommandImpl(EditWindow editWindow, bool isEnabled)
            {
                EditWindow = editWindow;
                IsEnabled = isEnabled;
            }

            public bool CanExecute(object? parameter)
            {
                return IsEnabled;
            }

            public void Execute(object? parameter)
            {
                if (parameter is ToDoItem toDoItem && toDoItem.Id.HasValue)
                {
                    CommandExecute?.Invoke(this, toDoItem);
                }
                EditWindow.Close();
            }
        }

        private class EditButtonCommandImpl : ICommand
        {
            public event EventHandler? CanExecuteChanged;

            public event EventHandler<int?>? CommandExecute;

            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public void Execute(object? parameter)
            {
                CommandExecute?.Invoke(this, parameter as int?);
            }
        }
    }
}
