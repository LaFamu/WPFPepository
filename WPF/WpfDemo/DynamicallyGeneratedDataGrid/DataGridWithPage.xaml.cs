﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfDemo.DynamicallyGeneratedDataGrid.Bases;

namespace WpfDemo.DynamicallyGeneratedDataGrid
{
    /// <summary>
    /// DataGridWithPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridWithPage : UserControl
    {
        private bool isShowCheckBox;
        /// <summary>
        /// 行点击相应事件
        /// </summary>
        public event Action RowClickAction = null;
        public DataGrid MyDataGrid
        {
            get { return dataGrid; }
        }
        public DataGridWithPage()
        {
            InitializeComponent();
        }

        private void SetOneDataColumn(SetDataColumnsItem columnsItem)
        {
            switch (columnsItem.ColumnType)
            {
                case ColumnTypeEnum.Label:
                    SetOneLabelColumn(columnsItem);
                    break;
                case ColumnTypeEnum.TextBox:
                    SetOneTextBoxColumn(columnsItem);
                    break;
                case ColumnTypeEnum.ComboBox:
                    SetOneComboBoxColumn(columnsItem);
                    break;
                default:
                    throw new Exception($"无法识别的列类型:{columnsItem.ColumnType}");
            }
        }

        private void SetOneComboBoxColumn(SetDataColumnsItem columnsItem)
        {
            if (columnsItem.ComboBoxDataContext == null || string.IsNullOrEmpty(columnsItem.ComboBoxDisplayMemberPath)
               || string.IsNullOrEmpty(columnsItem.BindPath))
            {
                throw new Exception($"列类型为ComboBox的对象，需要填写{nameof(SetDataColumnsItem.ComboBoxDataContext)},{nameof(SetDataColumnsItem.ComboBoxDisplayMemberPath)},{nameof(SetDataColumnsItem.BindPath)}");
            }
            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn();
            dataGridTemplateColumn.Width = new DataGridLength(columnsItem.DataGridLengthValue, columnsItem.DataGridLengthUnitType);
            dataGridTemplateColumn.Header = columnsItem.Header;

            FrameworkElementFactory comboBoxFactory = new FrameworkElementFactory(typeof(ComboBox));
            var bind = new Binding(columnsItem.BindPath);
            bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            comboBoxFactory.SetBinding(ComboBox.SelectedItemProperty, bind);
            comboBoxFactory.SetValue(ComboBox.DisplayMemberPathProperty, columnsItem.ComboBoxDisplayMemberPath);
            //comboBoxFactory.SetValue(ComboBox.DataContextProperty, columnsItem.ComboBoxDataContext);
            comboBoxFactory.SetValue(ComboBox.ItemsSourceProperty, columnsItem.ComboBoxDataContext);

            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = comboBoxFactory;
            dataGridTemplateColumn.CellTemplate = cellTemplate1;
            dataGrid.Columns.Add(dataGridTemplateColumn);
        }

        private void SetOneTextBoxColumn(SetDataColumnsItem columnsItem)
        {
            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn();
            dataGridTemplateColumn.Width = new DataGridLength(columnsItem.DataGridLengthValue, columnsItem.DataGridLengthUnitType);
            dataGridTemplateColumn.Header = columnsItem.Header;

            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));
            FrameworkElementFactory textBoxFactory = new FrameworkElementFactory(typeof(TextBox));
            var bind = new Binding(columnsItem.BindPath);
            bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            textBoxFactory.SetBinding(TextBox.TextProperty, bind);
            //textBoxFactory.SetValue(TextBox.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            gridFactory.AppendChild(textBoxFactory);

            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = gridFactory;
            dataGridTemplateColumn.CellTemplate = cellTemplate1;
            dataGrid.Columns.Add(dataGridTemplateColumn);
        }

        private void SetOneLabelColumn(SetDataColumnsItem columnsItem)
        {
            var textCol1 = new DataGridTextColumn() { Header = columnsItem.Header };
            textCol1.IsReadOnly = true;
            textCol1.Width = new DataGridLength(columnsItem.DataGridLengthValue, columnsItem.DataGridLengthUnitType);
            var bind = new Binding(columnsItem.BindPath);
            if (columnsItem.DisplayEvent != null)
            {
                bind.Converter = columnsItem.DisplayEvent;
            }
            textCol1.Binding = bind;
            dataGrid.Columns.Add(textCol1);
        }

        /// <summary>
        /// 设置表格内容
        /// </summary>
        /// <param name="input">数据列</param>
        /// <param name="isShowOperationColumn">是否显示操作列</param>
        /// <param name="input2">操作列</param>
        /// <param name="isShowCheckBox">是否显示checkbox</param>
        public void SetDataGrid(List<SetDataColumnsItem> input, 
            bool isShowOperationColumn = false, 
            List<OperationInfo> input2 = null, 
            bool isShowCheckBox = false)
        {
            this.isShowCheckBox = isShowCheckBox;
            if (!this.isShowCheckBox)
            {
                dataGrid.Columns.Clear();
            }

            //checkbox的后台绑定写法
            DataGridCheckBoxColumn dataGridCheckBoxColumn = new DataGridCheckBoxColumn();
            var bind = new Binding("IsChecked");
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bind.Mode = BindingMode.TwoWay;
            dataGridCheckBoxColumn.Binding = bind;
            dataGrid.Columns.Add(dataGridCheckBoxColumn);

            //if (isShowCheckBox)
            //{
            //    DataGridTemplateColumn dataGridCheckBoxColumn = new DataGridTemplateColumn();
            //    dataGridCheckBoxColumn.Header = "选择";
            //    dataGridCheckBoxColumn.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
            //    FrameworkElementFactory checkBoxFactory = new FrameworkElementFactory(typeof(System.Windows.Controls.CheckBox));
            //    checkBoxFactory.SetValue(System.Windows.Controls.CheckBox.ContentProperty, "全选");
            //    checkBoxFactory.AddHandler(System.Windows.Controls.CheckBox.ClickEvent, new RoutedEventHandler(CheckBox_Checked));
            //    if (!string.IsNullOrEmpty(checkBoxBindPath))
            //    {
            //        var bind = new Binding(checkBoxBindPath);
            //        bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //        bind.Mode = BindingMode.TwoWay;
            //        checkBoxFactory.SetBinding(System.Windows.Controls.CheckBox.IsCheckedProperty, bind);
            //    }
            //    DataTemplate dataTemplate = new DataTemplate();
            //    dataTemplate.VisualTree = checkBoxFactory;
            //    dataGridCheckBoxColumn.HeaderTemplate = dataTemplate;

            //    FrameworkElementFactory checkBoxFactory2 = new FrameworkElementFactory(typeof(System.Windows.Controls.CheckBox));
            //    DataTemplate dataTemplate2 = new DataTemplate();
            //    dataTemplate2.VisualTree = checkBoxFactory2;
            //    dataGridCheckBoxColumn.CellTemplate = dataTemplate2;
            //    dataGrid.Columns.Add(dataGridCheckBoxColumn);
            //}
            SetDataColumns(input);
            if (isShowOperationColumn && input2 != null)
            {
                SetOperations(input2);
            }

        }

        /// <summary>
        /// 设置数据列
        /// </summary>
        /// <param name="input"></param>
        private void SetDataColumns(List<SetDataColumnsItem> input)
        {
            input = input.OrderBy(x => x.Order).ToList();
            foreach (var item in input)
            {
                SetOneDataColumn(item);
            }
        }

        /// <summary>
        /// 设置操作列
        /// </summary>
        /// <param name="input"></param>
        private void SetOperations(List<OperationInfo> input)
        {
            input = input.OrderBy(x => x.Order).ToList();
            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn();
            dataGridTemplateColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridTemplateColumn.Header = "操作";

            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));
            for (int i = 0; i < input.Count; i++)
            {
                var oper = input[i];
                //判断该按钮是否应该存在
                FrameworkElementFactory col1 = new FrameworkElementFactory(typeof(ColumnDefinition));
                col1.SetValue(ColumnDefinition.WidthProperty, new GridLength(1, GridUnitType.Star));
                gridFactory.AppendChild(col1);

                //https://stackoverflow.com/questions/47401286/how-to-pro-gramatically-add-click-event-to-frameworkelementfactory
                FrameworkElementFactory btn1Factory = new FrameworkElementFactory(typeof(Button));
                btn1Factory.SetValue(Button.ContentProperty, oper.Content);
                btn1Factory.AddHandler(Button.ClickEvent, oper.ExecuteEvent);
                if (oper.CanExecuteEvent != null)
                {
                    //绑定数据源
                    var bind = new Binding("DataContext");
                    //绑定每一行的数据
                    //https://stackoverflow.com/questions/39873228/how-to-bind-to-wpf-datagrid-row-class-instance-and-not-its-property
                    bind.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = typeof(DataGridRow) };
                    //设置成自定义Converter
                    bind.Converter = oper.CanExecuteEvent;
                    btn1Factory.SetBinding(Button.VisibilityProperty, bind);
                }
               

                btn1Factory.SetValue(Grid.ColumnProperty, i);
                gridFactory.AppendChild(btn1Factory);

            }

            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = gridFactory;
            dataGridTemplateColumn.CellTemplate = cellTemplate1;
            dataGrid.Columns.Add(dataGridTemplateColumn);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as System.Windows.Controls.CheckBox;
            if (checkBox.IsChecked.HasValue && checkBox.IsChecked.Value)
            {
                //全选
                SetAllCheckBox(true);
            }
            else
            {
                //取消全选
                SetAllCheckBox(false);
            }
        }

        private void SetAllCheckBox(bool isCheck)
        {
            foreach (var item in dataGrid.Items)
            {
                var col = dataGrid.Columns[0];
                var c = col.GetCellContent(item) as ContentPresenter;
                var checkBox = VisualTreeHelper.GetChild(c, 0) as System.Windows.Controls.CheckBox;
                if (checkBox != null)
                {
                    checkBox.IsChecked = isCheck;
                }
            }
        }

        /// <summary>
        /// 获得选中的数据集合
        /// </summary>
        /// <returns></returns>
        public List<object> GetCheckedDataList()
        {
            if (!isShowCheckBox)
            {
                return null;
            }
            List<object> result = new List<object>();
            //循环
            foreach (var item in dataGrid.Items)
            {
                var col = dataGrid.Columns[0];
                var c = col.GetCellContent(item) as ContentPresenter;
                var checkBox = VisualTreeHelper.GetChild(c, 0) as System.Windows.Controls.CheckBox;
                if (checkBox != null && checkBox.IsChecked.HasValue && checkBox.IsChecked.Value)
                {
                    result.Add(c.Content);
                }
            }
            return result;
        }

        /// <summary>
        /// 点击行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridCell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //https://stackoverflow.com/questions/3426765/single-click-edit-in-wpf-datagrid#3472693
            //var s = dataGrid.SelectedItem;
            RowClickAction?.Invoke();
        }
    }
}
