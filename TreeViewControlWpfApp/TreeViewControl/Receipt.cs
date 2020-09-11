using System.Linq;

namespace TreeViewControlWpfApp.TreeViewControl {
    public class Receipt: BaseClass
    {
        private ReceiptItems _item;
        public ReceiptItems Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public Receipt(string name, Folder parent)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = GetDefaultName(parent);
            Name = name;
            Parent = parent;
            Item = new ReceiptItems {Name = name, Path = GetPath(parent)};
        }

        public Receipt(ReceiptItems item, Folder parent)
        {
            var name = item.Name;
            if (string.IsNullOrWhiteSpace(name))
                name = GetDefaultName(parent);
            Item = item;
            Name = name;
            Parent = parent;
        }

        protected sealed override string GetDefaultName(Folder parent)
        {
            var name = "New Receipt";
            var count = 1;
            while (parent.Receipts.Any(f => f.Name == name))
            {
                name = "New Receipt" + count++;
            }
            return name;
        }
    }
}