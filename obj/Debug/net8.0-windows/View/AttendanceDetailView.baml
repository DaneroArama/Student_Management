   M S B A M L   `   `   `  ����M  IStudent_Management, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Student_Management.ViewModelStudent_Management.ViewModel  5      2    ,Student_Management.View.AttendanceDetailView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:Student_Management.ViewModel     5      X TFontAwesome.Sharp, Version=6.6.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10fFfa=http://schemas.awesome.incremented/wpf/xaml/fontawesome.sharp  5      y�5
      �� *ViewVisibility+  � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ../Style/ButtonStyles.xaml?�6%   �� 5      $! ../Style/TextBoxStyles.xaml?�6%   �� 5      $ ../Style/UIColours.xaml?�6%   �� 5      $" ../Style/ComboBoxStyles.xaml?�6%   �� 5      $" ../Style/DataGridStyles.xaml?�6%   %    5      ��5      �� 5   
    �� 
StartPoint$	 1,1&�6    �� EndPoint$	 0,0&�6/   .��5      �    primaryBackColor1 � Color# [  6    � Offset$ 0Q�6F   � 5        primaryBackColor2# [ 6   $ 1Q�6F   5      .��5      � $��30q�6    � RowDefinitions 5!   
   �� 5"      $
��Auto �6   �� 5#      $
��Auto �6   �� 5$      $
��Auto �6   �� 5%      $��* �6   �� 5&      $
��Auto �6   .+�5*   
   �� $��0��6    �� Orientation$ Vertical=�6"   $��0,0,0,0q�69   .+�5+      �� -   5        	 ��Name$	 btnBack��5+      $
��Back��6&   $��75��65   $��30��6@   $
��Left=�5,      $	��Top=�60     RoundedButtonStyle#��[ 5.      $��0,0,0,10q�5/      ��5-      �� *BackCommand+5/      �� 50      -   5        
 ��Name$
 txtDate��50      $��24(�6)   $
��Bold"�67   $��Right=�51      
���������53      �� *SelectedDate+ �� StringFormat$ On {0:dd MMM yyyy}��54      �� 57   
   $��1��6   $ 
Horizontal=�6"   $��0,0,0,0q�6;   .+�58      �� -   5         ��Name$ cmbYear��58      $	��191��59        ComboBoxStyles#��[ 5<      ��5:      �� *Years+ �� SelectedItem 5;      �� *SelectedYear+��5=      �� 5>      .R�5?      �� 
���   ���6$   �� 65   5@      5B      �� 5D      -   5        $ cmbSemester��5D      $	��155��5E      $��10,0,0,0q�5F      #��[ 5I      ��5G      �� *	Semesters+ 5H      �� *SelectedSemester+��5J      �� 5K      .R�5L      �� 
���   ���6$   �� 65   5M      5O      �� 5Q      -   5        $ cmbClass��5Q      $	��188��5R      $��10,0,0,0q�5S      #��[ 5V      ��5T      �� *	Classes+ 5U      �� *SelectedClass+��5W      �� 5X      .R�5Y      �� 
���   ���6$   �� *	ClassName+6?   5Z      5\      5]      �� 5`   
   $��2��6   $ 
Horizontal=�6"   $��	0,10,0,20q�6;   .+�5a      !  FontAwesome.Sharp.IconImage  �  �FontAwesome.Sharp.IconImageBase`1[[FontAwesome.Sharp.IconChar, FontAwesome.Sharp, Version=6.6.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10f]]   Icon$ Search=�6   $��20��5b      $��20��5c        plainTextColor3   
Foreground# [ 5d      � 5f      $	��300��6   .+�5g      �� $��30��5h      
������ $��Medium"�5j      $��
Montserrat)�5k      #��[ 5l       �� 
CaretBrush# [ 5m      #��[ 5n      $��0,0,0,1q�5o      $��Center=�5p      $��5,5,0,0q�5q      ��5g      �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=�5q      �� 5t      $��Right=�6   $��30��5u      $��30��5v      $��0,5,5,0q�5w      ��5x      �� *ClearSearchCommand+y�5y      �� *
SearchText+  StringToVisibilityConverter �� 	Converter# [ .��5z        $ Xmark=�6#   $��20��5{   %   $��20��5|   %   # [ 5}   %   ��5~      �� 5       �� 
TargetType" ��6    .�5�      ��  �� Property � �� �� Value
 ���� 6;   �� 5�       � ��$ 0q�6@   5�      5�      5�      5�      &   System.Windows.Controls.DataGrid  5�   
   -   5          Name$ 
dgStudents��5�      $��3��5�      $��0,10,0,0q�5�         AutoGenerateColumns$ False��5�        CustomDataGridStyle#��[ 5�         CanUserAddRows$ False��5�         CanUserDeleteRows$ False��5�         SelectionMode$ Single=�5�         SelectionUnit$ FullRow=�5�         
IsReadOnly$ False��5�      ��5�      �� *
Students+   Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$ Student Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 5�   #   1  +System.Windows.Controls.DataGridBoundColumn    Binding  5�   #   �� *Name+!   ElementStyle! 5�      �� 5�      " ��6    .�5�      ��  � �� �#2125296;   �� 5�       � ��$ 
Montserrat)�6;   �� 5�       � ��$ Center=�6B   �� 5�       �@��Padding$ 5q�68   5�      5�      4  .System.Windows.Controls.DataGridCheckBoxColumn  5�      $ Present��6)   $	 100 5�   '     5�   '   �� *	IsPresent+$ PropertyChanged=�! 5�      �� 5�      " ��6    .�5�      ��  � ��$ Center=�6D   �� 5�       � ��$ Center=�6B   �� 5�       �@J�IsHitTestVisible$
 True��6A   �� 5�       � ��$ 5q�67   5�      5�      "   RowStyle" 5�      �� 5�      )	  #System.Windows.Controls.DataGridRow" 	 6   # �� Triggers# 5�      P� 5�      $ P� Property$ �@	 
IsSelected% P� Value$
% True��68   . �5�      ��  � �� �#773DFF6;   �� 5�       � ��
 �����6;   5�      P� 5�      $ �@J�IsMouseOver$
% True��69   . �5�      ��  � �� �#F8F9FA6;   5�      5�      5�      �� 5�   
   -   5        $	 
btnConfirm��5�      $��4��6   $��Confirm Attendance��62   $	��200��6O   $��40��6[   $��Center=�5�      $��Bottom=�6.   #��[ 5�      ��5�      �� *ConfirmCommand+5�      5�      5�      