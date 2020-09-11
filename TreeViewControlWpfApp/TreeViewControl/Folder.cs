using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace TreeViewControlWpfApp.TreeViewControl {
    public class Folder: BaseClass
    {
        public ObservableCollection<Folder> Folders {get; set;}
        public ObservableCollection<Receipt> Receipts {get; set;}

        public IList Children =>
            new CompositeCollection
            {
                new CollectionContainer {Collection = Folders},
                new CollectionContainer {Collection = Receipts}
            };

        public Folder(string name, Folder parent)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = GetDefaultName(parent);
            Name = name;
            Parent = parent;
            Folders = new ObservableCollection<Folder>();
            Receipts = new ObservableCollection<Receipt>();
        }

        public void AddFolder() => Folders.Add(new Folder(string.Empty, this) {IsSelected = true});

        public void Add(Folder folder)
        {
            folder.Parent = this;
            folder.IsSelected = true;
            Folders.Add(folder);
        }

        public void Delete(Folder folder) => Folders.Remove(folder);

        public Receipt AddReceipt()
        {
            var receipt = new Receipt(string.Empty, this) {IsSelected = true};
            Receipts.Add(receipt);
            return receipt;
        }

        public void Add(Receipt receipt)
        {
            receipt.Parent = this;
            receipt.IsSelected = true;
            Receipts.Add(receipt);
        }

        public void Delete(Receipt receipt) => Receipts.Remove(receipt);

        protected sealed override string GetDefaultName(Folder parent)
        {
            var folders = parent?.Folders ?? Folders;
            var name = "New Folder";
            var count = 1;
            while (folders.Any(f => f.Name == name))
            {
                name = "New Folder" + count++;
            }
            return name;
        }
    }
}