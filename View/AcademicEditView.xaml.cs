using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Student_Management.Model;
using Student_Management.ViewModel;

namespace Student_Management.View
{
    public partial class AcademicEditView : UserControl
    {
        private Point startPoint;
        private bool isDragging;
        private ListViewItem draggedItem;
        private delegate Point GetPositionDelegate(IInputElement element);

        public AcademicEditView()
        {
            InitializeComponent();
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            draggedItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
            isDragging = true;
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedItem != null)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    ListView listView = sender as ListView;
                    if (listView.ItemsSource == (DataContext as AcademicEditViewModel)?.Years)
                    {
                        var draggedData = draggedItem.DataContext as YearConfig;
                        if (draggedData != null)
                        {
                            DragDrop.DoDragDrop(draggedItem, draggedData, DragDropEffects.Move);
                        }
                    }
                    else
                    {
                        var draggedData = draggedItem.DataContext as ClassModel;
                        if (draggedData != null)
                        {
                            DragDrop.DoDragDrop(draggedItem, draggedData, DragDropEffects.Move);
                        }
                    }
                    isDragging = false;
                    draggedItem = null;
                }
            }
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(YearConfig)) && !e.Data.GetDataPresent(typeof(ClassModel)))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(YearConfig)) && !e.Data.GetDataPresent(typeof(ClassModel)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            e.Effects = DragDropEffects.Move;
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;
            var vm = DataContext as AcademicEditViewModel;
            
            if (listView == null || vm == null) return;

            var targetItem = GetNearestContainer(e.GetPosition, listView);
            
            if (e.Data.GetDataPresent(typeof(YearConfig)))
            {
                var sourceItem = e.Data.GetData(typeof(YearConfig)) as YearConfig;
                if (sourceItem != null && targetItem is YearConfig targetYear)
                {
                    var sourceIndex = vm.Years.IndexOf(sourceItem);
                    var targetIndex = vm.Years.IndexOf(targetYear);
                    if (targetIndex != -1 && sourceIndex != targetIndex)
                    {
                        vm.Years.Move(sourceIndex, targetIndex);
                    }
                }
            }
            else if (e.Data.GetDataPresent(typeof(ClassModel)))
            {
                var sourceItem = e.Data.GetData(typeof(ClassModel)) as ClassModel;
                if (sourceItem != null && targetItem is ClassModel targetClass)
                {
                    var sourceIndex = vm.ClassesForSelectedYear.IndexOf(sourceItem);
                    var targetIndex = vm.ClassesForSelectedYear.IndexOf(targetClass);
                    if (targetIndex != -1 && sourceIndex != targetIndex)
                    {
                        vm.ClassesForSelectedYear.Move(sourceIndex, targetIndex);
                    }
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private static object GetNearestContainer(GetPositionDelegate getPosition, ListView listView)
        {
            Point relativePoint = getPosition(listView);
            var element = listView.InputHitTest(relativePoint) as DependencyObject;
            while (element != null)
            {
                if (element is ListViewItem)
                {
                    return (element as ListViewItem).DataContext;
                }
                element = VisualTreeHelper.GetParent(element);
            }
            return null;
        }
    }
}