   M S B A M L   `   `   `  ����M  IStudent_Management, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Student_Management.ViewModelStudent_Management.ViewModel  5   	   A%clr-namespace:Student_Management.ViewStudent_Management.View  5   R   !    Student_Management.MainView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @	viewModel*clr-namespace:Student_Management.ViewModel     5   	   X TFontAwesome.Sharp, Version=6.6.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10fFfa=http://schemas.awesome.incremented/wpf/xaml/fontawesome.sharp  5   	   1local%clr-namespace:Student_Management.View  6R     � Title$  MainView��5	   	   $	��600��6   $
��1080��6'    � WindowStartupLocation$ CenterScreen=�5
   	    � WindowStyle$
 None=�5   	   
������  � AllowsTransparency$
 True��5   	    � DataContext 5      0   *Student_Management.ViewModel.MainViewModel  5   
    � 	Resources 5       �� DataType0   *Student_Management.ViewModel.HomeViewModel&    Student_Management.View.HomeView4   .Student_Management.ViewModel.StudentsViewModel*   $Student_Management.View.StudentsView4   .Student_Management.ViewModel.SettingsViewModel*   $Student_Management.View.SettingsView6   0Student_Management.ViewModel.AttendanceViewModel,	   &Student_Management.View.AttendanceView��%,  (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)(�� �     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5      	  5      5       .��5"      ��  �� CornerRadius$ 10��6   $��3q�6    J� Opacity$
 0.95Q�63   ��5#   
   �� 5$      	 �� 
StartPoint$		 0,0&�6"   
 �� EndPoint$	
 1,1&�63   .��5%      �    winBorderColor1 � Color# [  6    � Offset$ 0Q�6H   � 5&        winBorderColor2# [ 6   $
 0.75Q�6H   � 5'        winBorderColor3# [ 6   $ 1Q�6H   5(      ._�5+   
   �  � ColumnDefinitions 5,      �� 5.      $	��250 �6#   �� 50      $��* �6#   .+�54   
   �� $ 	10,0,0,10��6   $��0��55      ��56      �� 57      $		 0,0&�6*   $
 1,0.7&�6;   .��58      �   secondaryBackColor1# [ 6'   $ 0Q�6T   � 59        secondaryBackColor2# [ 6'   $ 1Q�6T   5:      ._�5=      �� .+�5?      ��  �� Orientation$ 
Horizontal=�6!   $��35��5@   !   $��
15,25,0,40q�5A   !   .+�5B      �� $��../Images/Logo.png��6    $��30��5C   !   �� 5D      $��Danero��6%     titleColor2#��[ 5E   %   $��20(�5F   %   $��
Montserrat)�5G   %   $��10,0,0,0q�5H   %   $��Medium"�5I   %   $��Center=�5J   %   5K      	� 5M        
menuButton#��[ 6"    
 color1
 � Tag# [ 5N   "    d� 	IsChecked$
 TrueY�5O   "   ��5P   "   �� *ShowHomeViewCommand+.��5Q      �� $ 
Horizontal=�6%   .+�5R      !
  FontAwesome.Sharp.IconImage
  �  �FontAwesome.Sharp.IconImageBase`1[[FontAwesome.Sharp.IconChar, FontAwesome.Sharp, Version=6.6.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10f]]   Icon$
 Home=�6+     menuButtonIcon#��[ 67   �� 5S      $��	Dashboard��6(    	 menuButtonText#��[	 69   5T      5U      	� 5W      #��[ 6"    

 color2# [
 5X   "   ��5Y   "   �� *ShowStudentViewCommand+.��5Z      �� $ 
Horizontal=�6%   .+�5[      
  $ Users=�6+   #��[ 68   �� 5\      $��Students��6(   #��[	 68   5]      5^      	� 5`      #��[ 6"    
 color3# [ 5a   "   ��5b   "   �� *ShowAttendanceViewCommand+.��5c      �� $ 
Horizontal=�6%   .+�5d      
  $ CalendarCheck=�6+   #��[ 6@   �� 5e      $��
Attendance��6(   #��[	 6:   5f      5g      	� 5i      #��[ 6"    
 color4# [ 5j   "   .��5k      �� $ 
Horizontal=�6%   .+�5l      
  $
 Bell=�6+   #��[ 67   �� 5m      $��Notification��6(   #��[	 6<   5n      5o      	� 5r      #��[ 6"    
 color5# [ 5s   "   ��5t   "   �� *ShowSettingsViewCommand+.��5u      �� $ 
Horizontal=�6%   .+�5v      
  $
 Gear=�6+   #��[ 67   �� 5w      $��Setting��6(   #��[	 67   5x      5y      5z      5{      �� 5~      $ 	0,10,10,0��6   $��1��5      ��5�      �� 5�      $		 1,1&�6*   $	
 0,0&�6;   .��5�      �   primaryBackColor1# [ 6'   $ 0Q�6R   � 5�        primaryBackColor2# [ 6'   $ 1Q�6R   5�      ._�5�      �  � RowDefinitions 5�      �� 5�      $��25 �6(   �� 5�      $��35 �6(   �� 5�      $��* �6(   .+�5�      �� -   5         ��Name$ pnlController��5�   !   $��0��5�   !   $ 
Horizontal=�5�   !   $��RightToLeft=�5�   !   
������ $��0,0,5,0q�5�   !   .+�5�      �� -   5         ��Name$ btnClose��5�   !     controlButton#��[ 5�   )   # [ 5�   )   .��5�      
  $ Xmark=�6+     controlButtonIcon#��[ 68   5�      �� 5�      -   5        $ btnMaximize��5�   !   #��[ 5�   !   # [ 5�   !   .��5�      
  $ Square=�6+   #��[ 69   5�      �� 5�      -   5        $ btnMinimize��5�   !   #��[ 5�   )    
 color6# [ 5�   )   .��5�      
  $ WindowMinimize=�6+   #��[ 6A   5�      5�      � 5�      $��1��6    5�      �� 5�      �� 5�      .+�5�      �� $��0��6%   $ 
Horizontal=�5�   %   $��Center=�5�   %   .+�5�      
     
Foreground# [ 5�   )   $��20��5�   )   $��20��5�   )   $��	35,0,10,0q�5�   )    5�   +   �� *Icon+5�   )   �� 5�      #��[ 5�   (   $��
Montserrat)�5�   (   $��Medium"�5�   (   $��Center=�5�   (   ��5�   (   �� *	Caption+5�   (   5�      �� 5�      $��1��6%   $ 
Horizontal=�5�   %   $��RightToLeft=�5�   %   $��Center=�5�   %   $��0,0,10,0q�5�   %   .+�5�      ��   
iconButton#��[ 6%   # [ 5�   %   .��5�   "   
  $ 	AngleDown=�6/     iconButtonIcon#��[ 6@   5�      �� 5�      #��[ 5�   (   $��
Montserrat)�5�   (   $��12(�5�   (   $��Center=�5�   (   ��5�   (   �� * CurrentUserAccount.DisplayName+5�   (   A� 5�      $��30��6&   $��30��5�   &   #��[
 5�   &   $��2��5�   &   $��	10,0,10,0q�5�   &   ��5�   "   �� 5�   &    �� ImageSource$ /Images/Logo.png��61   5�      �� 5�      #��[ 6%   # [ 5�   %   .��5�   &   
  $ Clock=�63   #��[ 5�   3   $��20��5�   3   $��20��5�   3   5�      �� 5�      #��[ 6%   # [ 5�   %   .��5�   &   
  $ Envelope=�63   #��[ 5�   3   $��20��5�   3   $��20��5�   3   5�      �� 5�      #��[ 6%   # [ 5�   %   .��5�   &   
  $
 Bell=�63   #��[ 5�   3   $��20��5�   3   $��20��5�   3   5�      5�      5�      �� 5�      $��2��5�   %   $��25q�5�   %   ��5�   %   �� *CurrentChildView+5�   %   5      5     5     5     5     