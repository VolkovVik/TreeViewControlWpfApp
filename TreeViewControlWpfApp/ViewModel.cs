using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using TreeViewControlWpfApp.TreeViewControl;

namespace TreeViewControlWpfApp
{
    class ViewModel: BindableBase
    {
        public ICommand ClickCommand { get;  }
        public ICommand SelectedChangedCommand {get; }
        public ICommand AddReceiptCommand { get; }
        public ICommand DeleteReceiptCommand { get; }
        public ICommand SaveChangesReceiptCommand { get;}

        public ObservableCollection<ReceiptItems> Products { get; set;}

        private ReceiptItems _selectedReceiptItem;

        public ReceiptItems SelectedReceiptItem
        {
            get => _selectedReceiptItem;
            set => SetProperty(ref _selectedReceiptItem, value);
        }

        private string _header;

        public string Header {
            get => _header;
            set => SetProperty(ref _header, value);
        }


        private string _text1;
        public string Text1 {
            get => _text1;
            set => SetProperty( ref _text1,value);
        }


        public ViewModel()
        {
            ClickCommand = new DelegateCommand(ExecuteMethod);
            SelectedChangedCommand = new DelegateCommand<ReceiptItems>(OnSelectedChange);

            AddReceiptCommand = new DelegateCommand<ReceiptItems>(OnAddReceipt);
            DeleteReceiptCommand = new DelegateCommand<List<ReceiptItems>>(OnDeleteReceipt);
            SaveChangesReceiptCommand = new DelegateCommand<List<ReceiptItems>>(OnSaveChangesReceipt);

            Products = new ObservableCollection<ReceiptItems>
            {
                new ReceiptItems {Name = "Receipt1", Path = "111"},
                new ReceiptItems {Name = "Receipt2", Path = "111"},
                new ReceiptItems {Name = "Receipt3", Path = "111"},
                new ReceiptItems {Name = "Receipt4", Path = "111\\222"},
                new ReceiptItems {Name = "Receipt5", Path = "111\\222"},
                new ReceiptItems {Name = "Receipt6", Path = "111\\222"},
                new ReceiptItems {Name = "Receipt7", Path = "111\\333"},
                new ReceiptItems {Name = "Receipt8", Path = "111\\333"},
                new ReceiptItems {Name = "Receipt9", Path = "111\\333"},
                new ReceiptItems {Name = "Receipt10", Path = "111\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt11", Path = "111\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt12", Path = "111\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt13", Path = "111\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt14", Path = "111\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt15", Path = "111\\222\\333\\444\\555"},


                new ReceiptItems {Name = "Receipt16", Path = "211"},
                new ReceiptItems {Name = "Receipt17", Path = "211"},
                new ReceiptItems {Name = "Receipt18", Path = "211"},
                new ReceiptItems {Name = "Receipt19", Path = "211\\222"},
                new ReceiptItems {Name = "Receipt20", Path = "211\\222"},
                new ReceiptItems {Name = "Receipt21", Path = "211\\222"},
                new ReceiptItems {Name = "Receipt22", Path = "211\\222\\333"},
                new ReceiptItems {Name = "Receipt23", Path = "211\\222\\333"},
                new ReceiptItems {Name = "Receipt24", Path = "211\\222\\333"},
                new ReceiptItems {Name = "Receipt25", Path = "211\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt26", Path = "211\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt27", Path = "211\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt28", Path = "211\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt29", Path = "211\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt30", Path = "211\\222\\333\\444\\555"}
            };
            Header = "123456";
        }

        private void OnSaveChangesReceipt( List<ReceiptItems> obj )
        {
            if ( obj == null ) return;

            Text1 = obj.Aggregate("Save changes\r\n", (current, item) => current + item.Name + "  " + item.Path + "\r\n");
        }

        private void OnDeleteReceipt( List<ReceiptItems> obj )
        {
            if ( obj == null ) return;
            Text1 = obj.Aggregate( "Delete\r\n", ( current, item ) => current + item.Name + "  " + item.Path + "\r\n" );
        }

        private void OnAddReceipt(ReceiptItems obj)
        {
            if ( obj == null ) return;
            Text1 = "Add " + obj.Name + "  " + obj.Path;
        }

        private void OnSelectedChange(ReceiptItems obj)
        {
            if (obj == null) return;
            Text1 = "Selected "+obj.Name + "  " + obj.Path;
        }


        private int _current = 0;
        private void ExecuteMethod()
        {
            //if (_current > Products.Count - 1)
            //{
            //    _current = 0;
            //}
            //SelectedReceiptItem = Products[_current++];

            if (Products.Count > 0)
            {
                Products.Clear();
            } else
            {
                Products.AddRange(new []{
                new ReceiptItems {Name = "Receipt1", Path = "111"},
                new ReceiptItems {Name = "Receipt2", Path = "111"},
                new ReceiptItems {Name = "Receipt3", Path = "111"},
                new ReceiptItems {Name = "Receipt4", Path = "111\\222"},
                new ReceiptItems {Name = "Receipt5", Path = "111\\222"},
                new ReceiptItems {Name = "Receipt6", Path = "111\\222"},
                new ReceiptItems {Name = "Receipt7", Path = "111\\333"},
                new ReceiptItems {Name = "Receipt8", Path = "111\\333"},
                new ReceiptItems {Name = "Receipt9", Path = "111\\333"},
                new ReceiptItems {Name = "Receipt10", Path = "111\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt11", Path = "111\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt12", Path = "111\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt13", Path = "111\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt14", Path = "111\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt15", Path = "111\\222\\333\\444\\555"},


                new ReceiptItems {Name = "Receipt16", Path = "211"},
                new ReceiptItems {Name = "Receipt17", Path = "211"},
                new ReceiptItems {Name = "Receipt18", Path = "211"},
                new ReceiptItems {Name = "Receipt19", Path = "211\\222"},
                new ReceiptItems {Name = "Receipt20", Path = "211\\222"},
                new ReceiptItems {Name = "Receipt21", Path = "211\\222"},
                new ReceiptItems {Name = "Receipt22", Path = "211\\222\\333"},
                new ReceiptItems {Name = "Receipt23", Path = "211\\222\\333"},
                new ReceiptItems {Name = "Receipt24", Path = "211\\222\\333"},
                new ReceiptItems {Name = "Receipt25", Path = "211\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt26", Path = "211\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt27", Path = "211\\222\\333\\444"},
                new ReceiptItems {Name = "Receipt28", Path = "211\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt29", Path = "211\\222\\333\\444\\555"},
                new ReceiptItems {Name = "Receipt30", Path = "211\\222\\333\\444\\555"}
            } );
            }
        }
    }
}
