   M S B A M L   `   `   `  ����M  IStudent_Management, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Student_Management.ViewStudent_Management.View  5   	   K*clr-namespace:Student_Management.ViewModelStudent_Management.ViewModel  5   	   U/clr-namespace:Student_Management.CustomControls!Student_Management.CustomControls  5   	   '    !Student_Management.View.LoginView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:Student_Management.View     5   	   @	viewModel*clr-namespace:Student_Management.ViewModel     5   	   JCustomControls/clr-namespace:Student_Management.CustomControls     5   	   -   5          � Title$  	LoginView��5	   	   $	��600��6   $	��800��6(    � WindowStyle$
 None=�5
   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   
������  � AllowsTransparency$
 True��5   	    � DataContext 5      1   +Student_Management.ViewModel.LoginViewModel  5   
    � 	Resources 5         BooleanToVisibility��%   &	        �� 5   
   6'   5      y�5      �� 5   
    �� Path$ IsViewVisible�6    �� Mode$ TwoWay=�6'   	 �� 	Converter#	 [  65   .��5      �� 
 �� CornerRadius$
 12��6   ��5   
   �� 5       �� ImageSource$ ../Images/Background.jpg��6    j� Stretch$ UniformToFill=�5       ._�5#   
   �� $
 10��6   $��3q�5$       J� Opacity$
 0.95Q�5%      ��5&      �� 5'       �� 
StartPoint$	 0,0&�6&    �� EndPoint$	 1,1&�67   .��5(      �   winBorderColor1 � Color# [ 6#    � Offset$ 0Q�6L   � 5)        winBorderColor2# [ 6#   $
 0.75Q�6L   � 5*        winBorderColor3# [ 6#   $ 1Q�6L   5+      ��5.      �� 5/      $	 1,0&�6&   $	 0,1&�67   .��50      �   primaryBackColor1# [ 6#   $ 0Q�6N   � 51        primaryBackColor2# [ 6#   $
 0.75Q�6N   52      ._�55      �  � RowDefinitions 56      �� 57      $��40 �6$   �� 58      .+�5:      �  � ColumnDefinitions 5;      �� 5<      �� 5=      $��30 �6+   �� 5>      $��30 �6+   �� 5?      $��5 �6+   .+�5B      �� $��LOG IN��6    
���   �$��16(�5D      $��
Montserrat)�5E      $��0��5F      $��Center=�5G      $��15,0,0,0q�5H      �� 5J      -   5         ��Name$ btnMinimize��5J      $��0q�5K      $��-��5L      $��14(�5M      $��
Montserrat)�5N       � Cursor$
 Hand��5O      $��1��5P      ��5R      �� 5S       �� 
TargetType" ��6$   .�5T   "   ��  �� Property � �� �� Value �#28AEED6?    �� Triggers 5U   "   P� 5V   &    P� Property �@J�IsMouseOver P� Value$
 True��6E   . �5W   *   ��  � �� �#278BEF6G   5X   '   5Z      ��5\      �� 5]       �� 
TargetType" ��6.   .R�5^   "   �� $��24��6)   $��24��64   $
 12��5_   %   #��z��5`   %   ._�5a   &   �� $��Center=�67   $��Center=�5b   3   5c   #   5d      5f      �� 5h      -   5        $ btnClose��5h      $��0q�5i      $��x��5j      $��14(�5k      $��
Montserrat)�5l      $
 Hand��5m      $��2��5n      ��5p      �� 5q      " ��6$   .�5r   "   ��  � �� �#DA34AE6?    5s   "   P� 5t   &    �@J�IsMouseOver$
 True��6E   . �5u   *   ��  � �� �#C62DAE6G   5v   '   5x      ��5z      �� 5{      " ��6.   .R�5|   "   �� $��24��6)   $��24��64   $
 12��5}   %   #��z��5~   %   ._�5   &   �� $��Center=�67   $��Center=�5�   3   5�   #   5�      5�      5�      �� 5�      $��1��5�       �� Orientation$ Vertical=�5�      $��233,0,233,0q�60   .+�5�      �� $��../Images/Logo.png��6   $	��120��5�      $	��120��5�      �� 5�      $��Computer Studies��6    
���   �$��24(�5�      $��SemiBold"�5�      $��
Montserrat)�5�      $��Center=�5�      $��0,10,0,0q�5�      $	��226��6.   �� 5�      $��
Department��6    
���   �$��Center=�5�      $��24(�5�      $��SemiBold"�5�      $��
Montserrat)�5�      $	��154��5�      �� 5�      $#��Dream it, code it, achieve it��6    
���   �$��14(�5�      $��Medium"�5�      $��
Montserrat)�5�      $��Center=�5�      $��0,10,0,0q�5�      �� 5�      $��Username��6    
���   �$��14(�5�      $��Medium"�5�      $��
Montserrat)�5�      $��0,20,0,0q�5�      �� 5�      -   5         ��Name$ txtUser��5�      $��14(�5�      $��Medium"�5�      $��
Montserrat)�5�      
���   � �� 
CaretBrush
 �����
�������$��0,0,0,2q�5�      $��Center=�5�      $��40,5,0,5q�5�      $��35��5�      ��5�      �� *
Username+  �� UpdateSourceTrigger$  PropertyChanged=�.��5�   &   ��5�      �� 5�      $ ../Images/User.png��6)   $ Uniform=�5�   %   ! j� 
AlignmentX$
! Left=�5�   %   " j� Viewport$" 	0,0,30,30�5�   %   # j� ViewportUnits$# Absolute=�5�   %   $ j� TileMode$
$ None=�5�   %   .��5�   .   5�      �� 5�      $��Password��6    
���   �$��14(�5�      $��Medium"�5�      $��
Montserrat)�5�      $��0,40,0,0q�5�      ?   9Student_Management.CustomControls.BindablePasswordControl  5�      $��35��5�   =   %   Password% 5�   =   �� *
Password+$ TwoWay=�$  PropertyChanged=�5�   =   �� 5�      ���#D7596D$��14(�5�      $��Medium"�5�      $��
Montserrat)�5�      $��0,10,0,0q�5�      $
��Wrap=�5�      ��5�       �� *ErrorMessage+5�      �� 5�      -   5        $ btnLogin��5�      $��0q�5�      $��Login��5�      $��16(�5�      $��
Montserrat)�5�      $
 Hand��5�      $��0,25,0,0q�5�      ��5�      �� *LoginCommand+��5�      �� 5�      " ��6$   .�5�   "   ��  � �� �#462AD86?    5�   "   P� 5�   &    �@J�IsMouseOver$
 True��6E   . �5�   *   ��  � �� �#28AEED6G   5�   '   5�      ��5�      �� 5�      " ��6.   .R�5�   "   �� $	��200��6)   $��40��65   $
 20��5�   %   #��z��5�   %   ._�5�   &   �� $��Center=�67   $��Center=�5�   3   5�   #   5�      5�      �� 5�      $ 
Horizontal=�6!   $��Center=�5�      $��0,20,0,0q�5�      .+�5�      �� $��Forget Password?��6$   
���OO/�$��14(�5�       $��Medium"�5�       $��
Montserrat)�5�       �� 5�      $��Reset��6$   
����� �$��14(�5�       $��Medium"�5�       $��
Montserrat)�5�       $
 Hand��5�       $��10,0,0,0q�5�       5      5     5     5     5     5     