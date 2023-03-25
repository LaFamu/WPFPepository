namespace Maui.Demo.Pages.Layouts;

public partial class GridByCodePage : ContentPage
{
    public GridByCodePage()
    {
        InitializeComponent();

        var grid = new Grid();

        //���������У���һ���п��Ϊ1*���ڶ����п��Ϊ2*
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });

        //���������У���һ���и߶�Ϊ1*���ڶ����и߶�Ϊ2*
        grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });

        //����ĸ���Ԫ��
        grid.Add(new Label() { Text = "��һ�е�һ��", BackgroundColor = Color.FromRgb(240, 230, 130) }, 0, 0); //��һ�е�һ��
        grid.Add(new Label() { Text = "��һ�еڶ���", BackgroundColor = Color.FromRgb(238, 232, 170) }, 1, 0); //��һ�еڶ���
        grid.Add(new Label() { Text = "�ڶ��е�һ��", BackgroundColor = Color.FromRgb(255, 218, 185) }, 0, 1); //�ڶ��е�һ��
        grid.Add(new Label() { Text = "�ڶ��еڶ���", BackgroundColor = Color.FromRgb(255, 228, 181) }, 1, 1); //�ڶ��еڶ���

        Content = grid;
    }
}