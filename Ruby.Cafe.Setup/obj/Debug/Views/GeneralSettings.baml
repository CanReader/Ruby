   M S B A M L   `   `   `  ����E  ARuby.Setup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Ruby.Setup
Ruby.Setup  5      &     Ruby.Setup.Views.GeneralSettings   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:Ruby.Setup     5        N� Title$  GeneralSettings��5	       N� 
Background
 �����.��5
      �  � 	Resources 5   
    �� MergedDictionaries �� Source�� 5       5      �� 5      $6 0/Ruby.Setup;component/Styles/OutlineTextBox.xaml?�6)   �� 5      $7 1/Ruby.Setup;component/Styles/OutlineComboBox.xaml?�6)   �� 5      $2 ,/Ruby.Setup;component/Styles/NextButton.xaml?�6)   %    5      .+�5   
   � $	��Top=�6   $	��100��6'   .+�5      �� -   5         ��Name$ 	PageTitle��5      $��General Settings��6+   $��38(�6C   $��Palatino Linotype)�6Q   $��Italic$�6p   
������ $��Center=�6�   $��20,0,0,0q�6�   �� 5      $��	50,0,50,0q�6   $��Bottom=�6+   ���#7070706G   5      � 5   
   -   5         �Name$ Content��5      $��Center=�6    $��Center=�6;    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5      ��  �� Orientation$ Vertical=�6   $��0��60   $��	0,0,130,0q�6@   .+�5      �� $ 
Horizontal=�6   .+�5      �� -   5        $ MachNameDefTxt��5       $��Center=�68   $��22(�6S   $��Machine Name: ��6a   $��0,0,20,0q�6w   
�����p�6�   �� 5       -   5        	 ��Name$	 MachNameBox��5       $�� ��63   $	��200��6;   5!      �� 5"      -   5        
 ��Name$
 
Option1Box��5"      $!��Start RubyCafe with machine��6/   $��0,20,0,0q�6U   $��20(�6g   
�����p�6u   �� 5#      -   5        $
 
Option2Box��5#      $(��"Show close button in the main page��6/   $��0,20,0,0q�6\   $��20(�6n   
�����p�6|   �� 5$      $ 
Horizontal=�6   $��0,20,0,0q�66   .+�5%      �� -   5        $ DefWidthTxt��5%       $��Center=�65   $��22(�6P   $��Width: ��6^   $��0,0,20,0q�6m   
�����p�6   �� 5&      -   5        $	 WidthBox��5&      $�� ��60   $��5,0,0,0q�68   $	��150��6I    �� 	MaxLength$ 4��6U   $��Center=�6c   $��Center=�6~   5'      �� 5(      $ 
Horizontal=�6   $��0,20,0,0q�66   .+�5)      �� -	   5        $ DefHeightTxt��5)       $��Center=�66   $��22(�6Q   $��Height: ��6_   $��0,0,20,0q�6o   
�����p�6�   �� 5*      -
   5        $	 	HeightBox��5*      $�� ��61   $	��150��69   $ 4��6E   $��Center=�6S   $��Center=�6n   5+      5,      �� 5-      $ Vertical=�6   $��1��60   .+�5.      �� $ 
Horizontal=�6   .+�5/      �� -   5        $ DefOrgNameTxt��5/       $��Center=�67   $��22(�6R   $��Organization Name: ��6`   $��0,0,20,0q�6{   
�����p�6�   �� 50      -   5        $	 
OrgNameBox��50      $	��170��62   51      �� 52      -   5         ��Name$ 
OrgTypeBox��52   ,    �� Text$ Organization Type��6@   $��0,20,0,0q�6Y    �� 
IsEditable . $	��200��6}   $��19(�6�   
���iii�$��Center=�6�   ��6   Z� .@�53      �� .��6#   Cafe6)   �� 54      .��6#   
Restaurant6/   55      �� 56      -   5        $ LanguageBox��56   >    . $ Language��6S   $��Center=�6c   $��Center=�6�   $��0,20,0,0q�6�   $	��200��6�   $��19(�6�   
���iii���6   Z� .@�57      �� .��6#   	English6,   �� 58      .��6#   
Türkçe6+   �� 59      .��6#   	Français6-   �� 5:      .��6#   
Deutsche6-   5;      5<      5=      �� 5>   
   -   5         ��Name$ NextBtn��5>      $��	Next Page��6"   5@      5A      5B      