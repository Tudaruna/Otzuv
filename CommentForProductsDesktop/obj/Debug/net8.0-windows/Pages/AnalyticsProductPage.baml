   M S B A M L   `   `   `  ����T  PCommentForProductsDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:CommentForProductsDesktop.PagesCommentForProductsDesktop.Pages  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      :    4CommentForProductsDesktop.Pages.AnalyticsProductPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:CommentForProductsDesktop.Pages     5      Dwpf4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      -   5          N� Title$'  !Статиска продукта��5	      $	��450��6H   $	��800��6U   .��5
      � .+�5   
   �� $��Center=�6    �� Orientation$ Vertical=�62   .+�5      �� $��20(�6   $��5q�6&   $
��Bold"�61   $6��0Аналитика оценок продукта��6C   6f   �� 5      $ 
Horizontal=�6   .+�5      �� $��Наименование:��6   $��15(�61   6A   �� 5      $��15(�6.   $
��Bold"�6=   ��6   �� *Name+6Q   5      �� 5      $ 
Horizontal=�6   .+�5      �� $"��Средняя оценка:��6   $��15(�63   6C   �� 5      -   5         ��Name$ SrScoreTextBlock��5      $��15(�66   $
��Bold"�6E   6Y   5      �� 5      $ 
Horizontal=�6   $
��Left=�62   .+�5        LiveCharts.Wpf.PieChart  -   5          Name$ pieChart��5      $	��200��61   $	��200��6=   �� 5      -   5         ��Name$ CommentListView��5      $	��300��64   $	��500��6A   ��5      �� 5      .R�5      �� $ 
Horizontal=�6)   .+�5   "   �� $��5q�6A   $��50��6L   $��70��6W   ��6(   �� *Photo+6e   �� 5   "   $	��600��6-   $ Vertical=�69   .+�5   &   �� $ 
Horizontal=�61   .+�5   *   �� $��Оценка:��64   $��5q�6C   6P   �� 5   *   $��15(�6K   $
��Bold"�6Y   $��5q�6k   ��64   �� *Score+6x   5    '   �� 5!   &   $
��Wrap=�60   $��5q�6b   ��6E   �� *TextComment+6o   �� 5"   &   $ 
Horizontal=�61   .+�5#   *   �� $I��CКомментарий оставлен пользователем ��64   $��5q�6_   $��10(�6j   $
��Thin"�6x   6�   �� 5$   *   $��5q�6P   $��10(�6[   $
��Thin"�6i   ��64   �� *
User.Login+6}   5%   '   5&   #   5'      5(      5*      5+      5,      5-      5.      