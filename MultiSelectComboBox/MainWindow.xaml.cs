using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiSelectComboBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }
    }

    public class MainWindowViewModel : ObservableObject
    {
        private string _selectedText = string.Empty;

        public string SelectedText
        {
            get
            {
                return _selectedText;
            }
            set
            {
                if(_selectedText != value)
                {
                    _selectedText = value;

                    RaisePropertyChanged("SelectedText");
                }
            }
        }

        private ObservableCollection<BookEx> _books;

        public ObservableCollection<BookEx> BookExs
        {
            get
            {
                if(_books == null)
                {
                    _books = new ObservableCollection<BookEx>();

                    _books.CollectionChanged += (sender, e) => 
                    {
                        if(e.OldItems != null)
                        {
                            foreach (BookEx bookEx in e.OldItems)
                            {
                                bookEx.PropertyChanged -= ItemPropertyChanged;
                            }
                        }

                        if(e.NewItems != null)
                        {
                            foreach (BookEx bookEx in e.NewItems)
                            {
                                bookEx.PropertyChanged += ItemPropertyChanged;
                            }
                        }
                    };
                }

                return _books;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsChecked")
            {
                BookEx bookEx = sender as BookEx;

                if(bookEx != null)
                {
                    IEnumerable<BookEx> bookExs = BookExs.Where(b => b.IsChecked == true);

                    StringBuilder builder = new StringBuilder();

                    foreach (BookEx item in bookExs)
                    {
                        builder.Append(item.Book.Name + " ");
                    }

                    SelectedText = builder == null ? string.Empty : builder.ToString();
                }
            }
        }

        public MainWindowViewModel()
        {
            BookExs.Add(new BookEx(new Book() { Name = "C# 6.0 in a Nutshell" }));
            BookExs.Add(new BookEx(new Book() { Name = "C#: Become A Master In C#" }));
            BookExs.Add(new BookEx(new Book() { Name = "C# 7.0 Pocket Reference" }));
            BookExs.Add(new BookEx(new Book() { Name = "C# in Depth, 3rd Edition" }));
        }
    }

    public class Book
    {
        public string Name { get; set; }
    }

    public class BookEx : ObservableObject
    {
        public Book Book { get; private set; }

        private bool _isChecked;

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if(_isChecked != value)
                {
                    _isChecked = value;

                    RaisePropertyChanged("IsChecked");
                }
            }
        }

        public BookEx(Book book)
        {
            Book = book;
        }
    }

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
