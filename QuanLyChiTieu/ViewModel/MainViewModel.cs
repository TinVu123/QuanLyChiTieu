using System;
using System.Windows.Input;

namespace QuanLyChiTieu.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        // Sample property for binding
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        // Sample command for demonstration
        private RelayCommand<string> _changeTitleCommand;
        public ICommand ChangeTitleCommand
        {
            get
            {
                if (_changeTitleCommand == null)
                {
                    _changeTitleCommand = new RelayCommand<string>(ChangeTitle, CanChangeTitle);
                }
                return _changeTitleCommand;
            }
        }

        private void ChangeTitle(string newTitle)
        {
            Title = newTitle;
        }

        private bool CanChangeTitle(string newTitle)
        {
            return !string.IsNullOrEmpty(newTitle);
        }
    }
}
