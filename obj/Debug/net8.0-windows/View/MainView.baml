   M S B A M L   `   `   `  ����M  IStudent_Management, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Student_Management.ViewModelStudent_Management.ViewModel  5   	   A%clr-namespace:Student_Management.ViewStudent_Management.View  5   R   !    Student_Management.MainView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @	viewModel*clr-namespace:Student_Management.ViewModel     5   	   X TFontAwesome.Sharp, Version=6.6.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10fFfa=http://schemas.awesome.incremented/wpf/xaml/fontawesome.sharp  5   	   1local%clr-namespace:Student_Management.View  6R     � Title$  MainView��5	   	   $	��600��6   $
��1080��6'    � WindowStartupLocation$ CenterScreen=�5
   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � DataContext 5      0   *Student_Management.ViewModel.MainViewModel  5   
    � 	Resources 5       �� DataType0   *Student_Management.ViewModel.HomeViewModel&    Student_Management.View.HomeView4   .Student_Management.ViewModel.StudentsViewModel*   $Student_Management.View.StudentsView6   0Student_Management.ViewModel.AttendanceViewModel,   &Student_Management.View.AttendanceView1   +Student_Management.ViewModel.MarksViewModel'	   !Student_Management.View.MarksView4
   .Student_Management.ViewModel.SettingsViewModel*   $Student_Management.View.SettingsView��%w  (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)(�� �     5      *, +)(�� �     5      *,
 +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5      	  5      �� 5   
   " 
 6   .R�5         5!      5"      .��5$      ��  �� CornerRadius$ 10��6   $��3q�6   ��5%   
   �� 5&       �� 
StartPoint$	 0,0&�6"   	 �� EndPoint$		 1,1&�63   .��5'      �    winBorderColor1
 � Color#
 [  6    � Offset$ 0Q�6H   � 5(        winBorderColor2#
 [ 6   $
 0.75Q�6H   � 5)        winBorderColor3#
 [ 6   $ 1Q�6H   5*      ._�5-   
   �  � ColumnDefinitions 5.      �� 50      $	��250 �6#   �� 52      $��* �6#   .+�56   
   �� $ 	10,0,0,10��6   $��0��57      ��58      �� 59      $	 0,0&�6*   $	 1,0.7&�6;   .��5:      �   secondaryBackColor1#
 [ 6'   $ 0Q�6T   � 5;        secondaryBackColor2#
 [ 6'   $ 1Q�6T   5<      ._�5?      �� .+�5A      ��  �� Orientation$ 
Horizontal=�6!   $��35��5B   !   $��
15,25,0,40q�5C   !   .+�5D      �� $��../Images/Logo.png��6    $��30��5E   !   �� 5F      $��CS Department��6%     titleColor2#��[ 5G   %   $��20(�5H   %   $��
Montserrat)�5I   %   $��10,0,0,0q�5J   %   $��Medium"�5K   %   $��Center=�5L   %   5M      	� 5O        
menuButton#��[ 6"    
 color1
 � Tag# [ 5P   "    d� 	IsChecked$
 TrueY�5Q   "   ��5R   "   �� *ShowHomeViewCommand+.��5S      �� $ 
Horizontal=�6%   .+�5T      !  FontAwesome.Sharp.IconImage  �  �FontAwesome.Sharp.IconImageBase`1[[FontAwesome.Sharp.IconChar, FontAwesome.Sharp, Version=6.6.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10f]]   Icon$
 Home=�6+     menuButtonIcon#��[ 67   �� 5U      $��	Dashboard��6(    	 menuButtonText#��[	 69   5V      5W      	� 5Y      #��[ 6"    

 color2# [
 5Z   "   ��5[   "   �� *ShowStudentViewCommand+.��5\      �� $ 
Horizontal=�6%   .+�5]        $ Users=�6+   #��[ 68   �� 5^      $��Students��6(   #��[	 68   5_      5`      	� 5b      #��[ 6"    
 color3# [ 5c   "   ��5d   "   �� *ShowAttendanceViewCommand+.��5e      �� $ 
Horizontal=�6%   .+�5f        $ CalendarCheck=�6+   #��[ 6@   �� 5g      $��
Attendance��6(   #��[	 6:   5h      5i      	� 5k      #��[ 6"    
 color4# [ 5l   "   ��5m   "   �� *ShowMarksViewCommand+.��5n      �� $ 
Horizontal=�6%   .+�5o        $ Marker=�6+   #��[ 69   �� 5p      $��Marks��6(   #��[	 65   5q      5r      	� 5u      #��[ 6"    
 color5# [ 5v   "   .��5w      �� $ 
Horizontal=�6%   .+�5x        $
 Bell=�6+   #��[ 67   �� 5y      $��Notification��6(   #��[	 6<   5z      5{      	� 5}      #��[ 6"    
 color6# [ 5~   "   ��5   "   �� *ShowSettingsViewCommand+.��5�      �� $ 
Horizontal=�6%   .+�5�        $ BookBookmark=�6+   #��[ 6?   �� 5�      $��Academic��6(   #��[	 68   5�      5�      5�      5�      �� 5�      $ 	0,10,10,0��6   $��1��5�      ��5�      �� 5�      $	 1,1&�6*   $		 0,0&�6;   .��5�      �   primaryBackColor1#
 [ 6'   $ 0Q�6R   � 5�        primaryBackColor2#
 [ 6'   $ 1Q�6R   5�      ._�5�      �  � RowDefinitions 5�      �� 5�      $��25 �6(   �� 5�      $��35 �6(   �� 5�      $��* �6(   .+�5�      �� -   5         ��Name$ pnlController��5�   !   $��0��5�   !   $ 
Horizontal=�5�   !   $��RightToLeft=�5�   !   
������ $��0,0,5,0q�5�   !   .+�5�      �� -   5         ��Name$ btnClose��5�   !     controlButton#��[ 5�   )   # [ 5�   )   .��5�        $ Xmark=�6+     controlButtonIcon#��[ 68   5�      �� 5�      -   5        $ btnMaximize��5�   !   #��[ 5�   !   # [ 5�   !   .��5�        $ Square=�6+   #��[ 69   5�      �� 5�      -   5        $ btnMinimize��5�   !   #��[ 5�   )   # [ 5�   )   .��5�        $ WindowMinimize=�6+   #��[ 6A   5�      5�      � 5�      $��1��6    5�      �� 5�      �� 5�      .+�5�      �� $��0��6%   $ 
Horizontal=�5�   %   $��Center=�5�   %   .+�5�           
Foreground# [ 5�   )   $��20��5�   )   $��20��5�   )   $��	35,0,10,0q�5�   )    5�   +   �� *Icon+5�   )   �� 5�      #��[ 5�   (   $��
Montserrat)�5�   (   $��Medium"�5�   (   $��Center=�5�   (   ��5�   (   �� *	Caption+5�   (   5�      �� 5�      $��1��6%   $ 
Horizontal=�5�   %   $��RightToLeft=�5�   %   $��Center=�5�   %   $��0,0,10,0q�5�   %   .+�5�      �� -   5          
iconButton#��[ 5�   %   # [ 5�   %   .��5�   "     $ 	AngleDown=�6/     iconButtonIcon#��[ 6@   5�      � 5�      -   5         �Name$ SignOutPopup��5�   $   $��Bottom=�6:    � 	StaysOpen$ False��6M   .��5�   "   �� 
�������
�������$��1q�6S   $ 5��6g   ._�5�   &   �� .+�5�   *   �� $��Sign Out��61     RoundedButtonStyle#��[ 5�   1   $��5q�5�   1   ��5�   D   �� *SignOutCommand+5�   1   5�   '   5�   #   5�      �� 5�      #��[ 5�   (   $��
Montserrat)�5�   (   $��12(�5�   (   $��Center=�5�   (   ��5�   (   �� * CurrentUserAccount.DisplayName+5�   (   A� 5�      $��30��6&   $��30��5�   &   #��[
 5�   &   $��2��5�   &   $��	10,0,10,0q�5�   &   ��5�   "   �� 5�   &    �� ImageSource$ /Images/Logo.png��61   5�      �� 5�      #��[ 6%   # [ 5�   %   $��0,0,10,0q�5�   %   .��5�   "     $ Clock=�6/   #��[ 5�   /   $��20��5�   /   $��20��5�   /   5�      �� 5     #��[ 6%   # [ 5  %   $��0,0,10,0q�5  %   .��5  "     $ Envelope=�6/   #��[ 5  /   $��20��5  /   $��20��5  /   5     �� 5
     #��[ 6%   # [ 5  %   $��0,0,10,0q�5  %   .��5  "     $
 Bell=�6/   #��[ 5  /   $��20��5  /   $��20��5  /   5     5     5     �� 5     -   5         ��Name$ MainContent��5  %   $��2��6_   $��25q�6l   ��6:   �� *CurrentChildView+6l   5     5     5     5     5     