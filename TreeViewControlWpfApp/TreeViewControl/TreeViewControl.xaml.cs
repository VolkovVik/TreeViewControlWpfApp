using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;

namespace TreeViewControlWpfApp.TreeViewControl
{
    /// <summary>
    /// Interaction logic for TreeViewControl.xaml
    /// </summary>
    public partial class TreeViewControl: UserControl
    {
        public ICommand ControlMouseDoubleClickCommand {get; set;}
        public ICommand ControlSelectedItemChangedCommand {get; set;}
        public ICommand ControlAddFolderCommand {get; set;}
        public ICommand ControlDeleteFolderCommand {get; set;}
        public ICommand ControlRenameFolderCommand {get; set;}
        public ICommand ControlAddReceiptCommand {get; set;}
        public ICommand ControlDeleteReceiptCommand {get; set;}
        public ICommand ControlSaveChangesCommand {get; set;}

        public ObservableCollection<Folder> Folders {get; set;}

        public Folder ControlSelectedFolder {get; set;}
        public Receipt ControlSelectedReceipt {get; set;}

        #region Header Property

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(TreeViewControl),
                new PropertyMetadata("", OnHeaderChanged));

        public string Header
        {
            get => (string) GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnHeaderChanged(e);
        }

        private static void OnHeaderChanged(DependencyPropertyChangedEventArgs e)
        {
            var str = e.NewValue.ToString();
        }

        #endregion Header Property

        #region Receipts Property

        public static readonly DependencyProperty ReceiptsProperty =
            DependencyProperty.Register("Receipts", typeof(ObservableCollection<ReceiptItems>), typeof(TreeViewControl),
                new PropertyMetadata(null, OnReceiptsChanged));

        public ObservableCollection<ReceiptItems> Receipts
        {
            get => (ObservableCollection<ReceiptItems>) GetValue(ReceiptsProperty);
            set => SetValue(ReceiptsProperty, value);
        }

        private static void OnReceiptsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TreeViewControl;
            control?.OnReceiptsChanged(e);
        }

        private void OnReceiptsChanged(DependencyPropertyChangedEventArgs e)
        {
            var items = e.NewValue as ObservableCollection<ReceiptItems>;
            SetFolders(items);
        }

        private void SetFolders(IReadOnlyCollection<ReceiptItems> items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                var paths = item.Path.Split('\\');

                Folder folder = null;
                Folder parent = null;
                var folders = Folders;
                foreach (var path in paths)
                {
                    if (folders.Any(f => f.Name == path))
                    {
                        folder = folders.First(f => f.Name == path);
                    } else
                    {
                        folder = new Folder(path, parent);
                        folders.Add(folder);
                    }
                    parent = folder;
                    folders = folder.Folders;
                }
                folder?.Receipts.Add(new Receipt(item, parent));
            }
        }

        #endregion Receipts Property

        #region SelectedReceipt Property

        public static readonly DependencyProperty SelectedReceiptProperty =
            DependencyProperty.Register("SelectedReceipt", typeof(ReceiptItems), typeof(TreeViewControl),
                new PropertyMetadata(null, OnSelectedItemChanged));

        public ReceiptItems SelectedReceipt
        {
            get => (ReceiptItems) GetValue(SelectedReceiptProperty);
            set => SetValue(SelectedReceiptProperty, value);
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TreeViewControl;
            control?.OnSelectedItemChanged(e);
        }

        private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as ReceiptItems;
            SetSelectedItem(item);
        }

        private void SetSelectedItem(ReceiptItems item)
        {
            if (item == null) return;
            var paths = item.Path.Split('\\');

            Folder folder = null;
            var folders = Folders;
            foreach (var path in paths)
            {
                if (folders.All(f => f.Name != path))
                    return;
                folder = folders.First(f => f.Name == path);
                folder.IsExpanded = true;
                folders = folder.Folders;
            }
            var receipt = folder?.Receipts.FirstOrDefault(r => r.Name == item.Name);
            if (receipt == null) return;
            receipt.IsSelected = true;
        }

        #endregion SelectedReceipt Property

        #region SelectedReceiptChangedCommand Property

        public static readonly DependencyProperty SelectedReceiptChangedCommandProperty =
            DependencyProperty.Register("SelectedReceiptChangedCommand", typeof(DelegateCommand<ReceiptItems>),
                typeof(TreeViewControl));

        public DelegateCommand<ReceiptItems> SelectedReceiptChangedCommand
        {
            get => (DelegateCommand<ReceiptItems>) GetValue(SelectedReceiptChangedCommandProperty);
            set => SetValue(SelectedReceiptChangedCommandProperty, value);
        }

        #endregion SelectedReceiptChangedCommand Property

        #region AddReceiptCommand Property

        public static readonly DependencyProperty AddReceiptCommandProperty =
            DependencyProperty.Register("AddReceiptCommand", typeof(DelegateCommand<ReceiptItems>),
                typeof(TreeViewControl));

        public DelegateCommand<ReceiptItems> AddReceiptCommand
        {
            get => (DelegateCommand<ReceiptItems>) GetValue(AddReceiptCommandProperty);
            set => SetValue(AddReceiptCommandProperty, value);
        }

        #endregion AddReceiptCommand Property

        #region DeleteReceiptCommand Property

        public static readonly DependencyProperty DeleteReceiptCommandProperty =
            DependencyProperty.Register("DeleteReceiptCommand", typeof(DelegateCommand<List<ReceiptItems>>),
                typeof(TreeViewControl));

        public DelegateCommand<List<ReceiptItems>> DeleteReceiptCommand
        {
            get => (DelegateCommand<List<ReceiptItems>>) GetValue(DeleteReceiptCommandProperty);
            set => SetValue(DeleteReceiptCommandProperty, value);
        }

        #endregion DeleteReceiptCommand Property

        #region SaveChangesReceiptCommand Property

        public static readonly DependencyProperty SaveChangesReceiptCommandProperty =
            DependencyProperty.Register("SaveChangesReceiptCommand", typeof(DelegateCommand<List<ReceiptItems>>),
                typeof(TreeViewControl));

        public DelegateCommand<List<ReceiptItems>> SaveChangesReceiptCommand
        {
            get => (DelegateCommand<List<ReceiptItems>>) GetValue(SaveChangesReceiptCommandProperty);
            set => SetValue(SaveChangesReceiptCommandProperty, value);
        }

        #endregion SaveChangesReceiptCommand Property

        public TreeViewControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;

            Folders = new ObservableCollection<Folder>();

            ControlMouseDoubleClickCommand = new DelegateCommand<object>(OnMouseDoubleClick);
            ControlSelectedItemChangedCommand = new DelegateCommand<object>(OnSelectedItemChanged);

            ControlAddFolderCommand = new DelegateCommand<object>(OnAddFolder);
            ControlDeleteFolderCommand = new DelegateCommand<object>(OnDeleteFolder);
            ControlRenameFolderCommand = new DelegateCommand<object>(OnRenameFolder);
            ControlAddReceiptCommand = new DelegateCommand<object>(OnAddReceipt);
            ControlDeleteReceiptCommand = new DelegateCommand<object>(OnDeleteReceipt);
            ControlSaveChangesCommand = new DelegateCommand<object>(OnSaveChanges);
        }

        private void OnAddFolder(object obj)
        {
            if (!(obj is Folder folder)) return;

            folder.IsExpanded = true;
            folder.AddFolder();
        }

        private void OnDeleteFolder(object obj)
        {
            if (!(obj is Folder folder)) return;

            if (folder.Parent == null)
            {
                Folders.Remove(folder);
            } else
            {
                folder.Parent?.Delete(folder);
            }
            var deleted = GetReceiptItems(folder);
            DeleteReceiptCommand.Execute(deleted);
        }

        private void OnRenameFolder(object obj)
        {
            if (!(obj is Folder folder)) return;

            ControlSelectedFolder = folder;
            folder.IsSelected = true;
            folder.IsEdit = true;
        }

        private void OnAddReceipt(object obj)
        {
            Folder currentFolder = null;
            if (obj is Receipt receipt)
            {
                currentFolder = receipt.Parent;
            }
            if (obj is Folder folder)
            {
                folder.IsExpanded = true;
                currentFolder = folder;
            }
            if (currentFolder == null) return;
            var createdReceipt = currentFolder.AddReceipt();
            AddReceiptCommand.Execute(createdReceipt.Item);
        }

        private void OnDeleteReceipt(object obj)
        {
            if (!(obj is Receipt receipt)) return;
            var folder = receipt.Parent;
            folder.Delete(receipt);
            DeleteReceiptCommand.Execute(new List<ReceiptItems> {receipt.Item});
        }

        private void OnSaveChanges(object obj)
        {
            if (obj is Folder folder)
            {
                ControlSelectedFolder = folder;
                folder.IsSelected = true;
                folder.IsEdit = false;

                UpdateReceiptItems( folder );
                var changed = GetReceiptItems(folder);
                SaveChangesReceiptCommand.Execute(changed);
            }
            if (obj is Receipt receipt)
            {
                var currentFolder = receipt.Parent;
                currentFolder.IsSelected = true;
                currentFolder.IsEdit = false;

                receipt.Item.Path = BaseClass.GetPath(currentFolder);
                SaveChangesReceiptCommand.Execute(new List<ReceiptItems> {receipt.Item});
            }
        }

        private void OnMouseDoubleClick(object obj)
        {
            if (obj is Receipt receipt)
            {
                ControlSelectedReceipt = receipt;
            }
            if (obj is Folder folder)
            {
                ControlSelectedFolder = folder;
            }
        }

        private void OnSelectedItemChanged(object obj)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (obj is Receipt receipt)
            {
                ControlSelectedReceipt = receipt;
                SelectedReceipt = receipt.Item;
                SelectedReceiptChangedCommand.Execute(receipt.Item);
                return;
            }
            if (obj is Folder folder)
            {
                ControlSelectedFolder = folder;
            }
        }

        private static List<ReceiptItems> GetReceiptItems(Folder parent)
        {
            var items = new List<ReceiptItems>();
            items.AddRange(parent.Receipts.Select(r => r.Item));
            foreach (var folder in parent.Folders)
            {
                var folderItems = GetReceiptItems(folder);
                items.AddRange(folderItems);
            }
            return items;
        }

        private static void UpdateReceiptItems(Folder parent)
        {
            var path = BaseClass.GetPath(parent);
            foreach (var receipt in parent.Receipts)
            {
                receipt.Item.Path = path;
            }
            foreach (var folder in parent.Folders)
            {
                UpdateReceiptItems(folder);
            }
        }

        #region Drag and Drop operation

        // https://www.codeproject.com/Articles/55168/Drag-and-Drop-Feature-in-WPF-TreeView-Control
        // https://stackoverflow.com/questions/1026179/drag-drop-in-treeview
        // http://www.wpftutorial.net/DragAndDrop.html

        private Point _startPoint;
        private object _draggedItem, _targetItem;

        private void TreeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _startPoint = e.GetPosition(null);
            }
        }

        private void TreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton != MouseButtonState.Pressed) return;
                var currentPosition = e.GetPosition(null);
                if (!(Math.Abs(currentPosition.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) &&
                    !(Math.Abs(currentPosition.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
                    return;
                _draggedItem = TreeViewElement.SelectedItem;
                if (_draggedItem == null || 
                    _draggedItem is Receipt receipt && receipt.IsEdit ||
                    _draggedItem is Folder folder && folder.IsEdit) return;
                var finalDropEffect = DragDrop.DoDragDrop(TreeViewElement, TreeViewElement.SelectedValue, DragDropEffects.Move);
                //Checking target is not null and item is dragging(moving)
                if (finalDropEffect != DragDropEffects.Move || _targetItem == null) return;
                // A Move drop was accepted
                if (!CheckDropTarget(_draggedItem, _targetItem)) return;
                DropItem(_draggedItem, _targetItem);
                _targetItem = null;
                _draggedItem = null;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void TreeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                var currentPosition = e.GetPosition(TreeViewElement);
                if (!(Math.Abs(currentPosition.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) &&
                    !(Math.Abs(currentPosition.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
                    return;
                // Verify that this is a valid drop and then store the drop target
                var item = GetNearestContainer(e.OriginalSource as UIElement);
                e.Effects = CheckDropTarget(_draggedItem, item.DataContext)
                    ? DragDropEffects.Move
                    : DragDropEffects.None;
                e.Handled = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                if (_draggedItem == null) return;

                // Verify that this is a valid drop and then store the drop target
                var targetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (targetItem == null) return;
                _targetItem = targetItem.DataContext;
                e.Effects = DragDropEffects.Move;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static bool CheckDropTarget(object sourceItem, object targetItem) =>
            !(sourceItem.GetType() == targetItem.GetType() && sourceItem == targetItem);

        private void DropItem(object sourceItem, object targetItem)
        {
            var targetFolder = GetDropTargetFolder(targetItem);
            if (targetFolder == null) return;

            if (sourceItem is Receipt receipt)
            {
                receipt.Parent?.Delete(receipt);
                targetFolder.IsExpanded = true;
                targetFolder.Add(receipt);

                receipt.Item.Path = BaseClass.GetPath( targetFolder );
                SaveChangesReceiptCommand.Execute( new List<ReceiptItems> { receipt.Item } );
                return;
            }
            if (sourceItem is Folder folder)
            {
                if (folder.Parent != null)
                {
                    folder.Parent?.Delete(folder);
                } else
                {
                    Folders.Remove(folder);
                }
                targetFolder.IsExpanded = true;
                targetFolder.Add(folder);

                UpdateReceiptItems( folder );
                var changed = GetReceiptItems(folder);
                SaveChangesReceiptCommand.Execute( changed );
            }
        }

        private static Folder GetDropTargetFolder(object targetItem) =>
            targetItem switch
            {
                Receipt receipt => receipt.Parent,
                Folder folder => folder,
                _ => null
            };

        // ReSharper disable once UnusedMember.Local
        private static TObject FindVisualParent<TObject>(UIElement child) where TObject: UIElement
        {
            if (child == null)
            {
                return null;
            }
            var parent = VisualTreeHelper.GetParent(child) as UIElement;
            while (parent != null)
            {
                if (parent is TObject found)
                {
                    return found;
                }
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        private static TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            var container = element as TreeViewItem;
            while (container == null && element != null)
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        #endregion Drag and Drop operation
    }

    public class ReceiptItems
    {
        public string Name {get; set;}
        public string Path {get; set;}
    }
}
