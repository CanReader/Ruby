   M S B A M L   `   `   `  ����E  ARuby.Setup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Ruby.Setup.ViewsRuby.Setup.Views  5      '    !Ruby.Setup.Views.DatabaseSettings   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Ruby.Setup.Views     5        N� Title$  DatabaseSettings��5	       N� 
Background
 �����.��5      �  � 	Resources 5   
    �� MergedDictionaries �� Source�� 5       5      �� 5      $6 0/Ruby.Setup;component/Styles/OutlineTextBox.xaml?�6)   �� 5      $2 ,/Ruby.Setup;component/Styles/NextButton.xaml?�6)   %    5      .+�5   
   � $	��Top=�6   $	��100��6'   .+�5      �� -   5         ��Name$ 	PageTitle��5      $��Database Settings��6+   $��38(�6D   $��Palatino Linotype)�6R   $��Italic$�6q   
������ $��Center=�6�   $��20,0,0,0q�6�   �� 5      $��	50,0,50,0q�6   $��Bottom=�6+   ���#7070706G   5      � 5   
   -   5         �Name$ Content��5      $��	0,100,0,0q�6    .+�5      �� -   5         ��Name$ 	DBTypeBox��5       �� Text$ Database Type��6*   $
��Left=�6?   $	��Top=�6Z   $��20(�6r   $��	50,50,0,0q�6�   $	��170��6�   	 �� 
IsEditable	 . .@�5      �� .��6   None6%   �� 5      .��6   Sql6$   �� 5      .��6   MySql6&   �� 5      .��6   	SqlLite6(   �� 5      .��6   Oracle6'   5      �� 5       
 �� Orientation$
 Vertical=�6   $��Center=�60   $��Center=�6K   .+�5!      �� $
 
Horizontal=�6   .+�5"      �� -   5        $ DefServerNameTxt��5"       $��Server Name:��6:   $��0,0,50,0q�6N   $��22(�6`   $��Center=�6n   �� 5#      -   5         ��Name$ ServerNameBox��5#      $	��200��66   $
��Left=�6B   6d   5$      �� 5%      $
 
Horizontal=�6   $��0,20,0,0q�66   .+�5&      �� -   5        $ DefDatabaseNameTxt��5&       $��Database Name:��6<   $��22(�6R   $��Center=�6`   $��0,0,20,0q�6{   �� 5'      -   5        $ DatabaseNameBox��5'      $	��200��67   $
��Left=�6C   6e   5(      �� 5)      $
 
Horizontal=�6   $��0,20,0,0q�66   .+�5*      �� -   5        $ DefUserNameTxt��5*       $��
User Name:��68   $��22(�6J   $��Center=�6X   $��0,0,68,0q�6s   �� 5+      -	   5        $ UserNameBox��5+      $	��200��63   $
��Left=�6?   6a   5,      �� 5-      $
 
Horizontal=�6   $��0,20,0,0q�66   .+�5.      �� -
   5        $ DefPasswordTxt��5.       $��	Password:��68   $��22(�6I   $��Center=�6W   $��0,0,85,0q�6r   �� 5/      -   5        $ PasswordBox��5/      $	��200��63   $
��Left=�6?   6a   50      51      52      �� 53   
   -   5        $ ConnectionStringTxt��53      $��Connection String -> {0}��61   $��Bottom=�6Q   $��Stretch=�6l   $��18(�6�   $��30,0,300,30q�6�   $
��Wrap=�6�   �� 54   
   -   5         ��Name$ NextBtn��54      $��	Next Page��6"   55      56      57      