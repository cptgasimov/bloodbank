   M S B A M L   `   `   `  ����D  @BloodBank, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:BloodBank	BloodBank  5   	       BloodBank.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:BloodBank     5   	   -   5          � Title$  
MainWindow��5   	   $	��500��6   $	��750��6)    � WindowStyle$
 None=�65    � AllowsTransparency .  � 
ResizeMode$ NoResize=�6b   
������  � WindowStartupLocation$ CenterScreen=�6�    � 	Resources 5
       �� 
TargetType �� Triggers P� Property	 P� Value
 �� Property �� Value��%�   '��       �� 5   
   " ��6    5      P� 5       �@J�IsMouseOver	 . . �5      �� 
 �@�Cursor$
 Hand��6/   5      5      5      .��5      �� 
���   �$	��1.5q�6!    �� CornerRadius$ 15��67   
�������._�5   
   �� -   5        $��
mainWindow��5      |�. .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Right=�62    K� ZIndex$ 1��6N   .+�5      �� -   5        $��Minimize��5   �   $��35��6   $��35��6$   
������ $��Right=�6I   $��0 5 15 0q�6e   $��0q�6w   .��5      �� $��pics/minimize.png��6   5      �� 5      -   5        $��Close��5   �   $��35��6   $��35��6$   
������ $��Right=�6I   $��0 5 15 0q�6e   $��0q�6w   .��5      �� $��pics/close.png��6   5       5!      �� 5$      $ 
Horizontal=�6   $��Center=�62   .+�5%      �� $��96��6   $��96��6#   $��Center=�6/   ��5&      �� 5'      " ��6    .�5(      �� 
 � ��$ pics/heart.png��67    5)      P� 5*   "    �@J�IsMouseOver	 . . �5+   &   �� 
 � ��$ pics/heart_blue.png��6?   5,   #   5.      50      �� 52      $��60(�6   $��Center=�6*   $��10 0 0 5q�6E   
Blood Bank6c   53      �� 56      $ Vertical=�6   $��Center=�60   $��0 15 0 0q�6M   .+�57      �� $��Center=�6   $��24(�69   $��0 0 0 7q�6G   $��SemiBold"�6X   Welcome, let's get started6�   �� 58      $��18(�6   $��SemiBold"�6*   -+Enter your username and password to sign in6m   �� 5;      $��15(�6   $
��Left=�6&   $��	78 15 0 0q�6A   .��6T   	Username:6_   �� 5<      -   5        $	��Txt��5<   @   $��15(�6   $	��200��6(   $��3q�64   $��Center=�6K   6h   �� 5?      $��15(�6   $
��Left=�6&   $��79 5 0 0q�6B   .��6T   	Password:6_   G� 5@      -   5        $
��Pass��5@   a   $��15(�6   $��Center=�6,   $	��200��6I   $��3q�6U   6m   �� 5C      
���   �$��32��6-   ���	#FFFF5454$	��100��6Q   $��3q�6]   $��0 15 0 0q�6q   ._�5E      �� -   5        $��SignIn��5E   �   $��Sign In��6   $��18(�6/   
������ $��0q�6V   $��SemiBold"�6j   6�   ��5G      �� 5H      " ��6    .�5I      �� 
 �@��CornerRadius$ 0��6=    5J      P� 5K   "    �@J�IsMouseOver	 . . �5L   &   �� 
 �@��CornerRadius$ 3��6E   5M   #   5O      5Q      5R      �� 5U      $ 
Horizontal=�6   $	��650��62   $��0 65 0 0q�6>   .+�5V      �� -   5        $��About��5V   n   $
��Left=�6   $��15(�67   $��	Underline{�6E   $��About��6a   ��5W      �� 5X      " ��6     5Y      P� 5Z   "    �@J�IsMouseOver	 . . �5[   &   �� 
 � �� �	#FF015DFF6C   �� 5\   &   
 �@�Cursor$
 Hand��6?   5]   #   5_      5a      �� 5c      $��Right=�6   $��15(�68   $��	567 0 0 0q�6F   $��Italic$�6Y   By Cpt6t   5d      5f      5g      5h      