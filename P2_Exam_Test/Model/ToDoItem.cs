using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace P2_Exam_Test.Model
{
    internal class ToDoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int? Id { get; set; }

        private string _taskName = string.Empty;
        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskName)));
            }
        }

        private DateTime? _limit = null;
        public DateTime? Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Limit)));
            }
        }

        private bool _done = false;
        public bool Done
        {
            get => _done;
            set
            {
                _done = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Done)));
            }
        }

        public ToDoItem Copy()
        {
            return new ToDoItem()
            {
                Id = Id,
                Done = Done,
                Limit = Limit,
                TaskName = TaskName
            };
        }
    }
}
