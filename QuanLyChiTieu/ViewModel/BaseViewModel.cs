using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChiTieu.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged // thông báo khi có sự kiện thay đổi
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Ràng buộc các hành động (commands) từ ViewModel đến giao diện người dùng, cho phép tách biệt giữa logic và UI.

    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute; // deleget nhận vào tham số kiểu T và trả về kiểu bool
        private readonly Action<T> _execute;  // delegate dùng để gán sự kiện vào kiểu T và không trả về gì

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _execute = execute;
        }

        // WPF sẽ tự động gọi CanExecute trên MyCommand để kiểm tra xem nút có thể được nhấn hay không.
        // Nếu CanExecute trả về false, nút sẽ bị vô hiệu hóa.
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
            //Nếu _canExecute == null, phương thức trả về true, nghĩa là lệnh luôn có thể thực thi.
            //Nếu _canExecute khác null, nó sẽ gọi _canExecute với tham số kiểu T, sau đó trả về giá trị bool.
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
            //Phương thức này thực thi lệnh, nhận tham số kiểu object và chuyển đổi sang kiểu T trước khi truyền nó cho _execute.
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
