using P2_Exam_Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2_Exam_Test.ViewModel
{
    internal class EditWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ToDoItem? _toDoItem;
        public ToDoItem ToDoItem
        {
            get => _toDoItem ?? new ToDoItem();
            set
            {
                _toDoItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToDoItem)));
            }
        }

        public string ButtonCaption { get; set; } = string.Empty;

        public string WindowCaption { get; set; } = string.Empty;

        public ICommand? DoneCommand { get; set; }

        public ICommand? DeleteCommand { get; set; }
    }
}
