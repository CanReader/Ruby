   M S B A M L   `   `   `  ����E  ARuby.Setup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Ruby.Setup.ViewsRuby.Setup.Views  5      %    Ruby.Setup.Views.FirstEmployees   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Ruby.Setup.Views     5        N� Title$  FirstEmployees��5	       N� 
Background
 �����.��5
      �  � 	Resources 5   
    �� MergedDictionaries �� Source�� 5       5      �� 5      $6 0/Ruby.Setup;component/Styles/OutlineTextBox.xaml?�6)   �� 5      $5 //Ruby.Setup;component/Styles/OutlineButton.xaml?�6)   6b   �� 5      $2 ,/Ruby.Setup;component/Styles/NextButton.xaml?�6)   %    5      .+�5   
   � $	��Top=�6   $	��100��6'   .+�5      �� -   5         ��Name$ 	PageTitle��5      $��First Employees��6+   $��38(�6B   $��Palatino Linotype)�6P   $��Italic$�6o   
������ $��Center=�6�   $��20,0,0,0q�6�   �� 5      $��	50,0,50,0q�6   $��Bottom=�6+   ���#7070706G   5      � 5   
   -   5         �Name$ Content��5      $��	0,110,0,0q�6     � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� -   5         ��Name$ EmployeeList��5      $��20,20,10,20q�6,   $��0��6A   ���#7070706g   �� 5       	 �� Orientation$	 Vertical=�6   $��1��60   .+�5!      �� -   5        
 ��Name$
 
EmpNameBox��5!      $��0,0,50,0q�6.   $��Employee Name��6@   �� 5"      -   5         ��Name$ EmpGendrBox��5"      $��	0,30,50,0q�60    �� Text$ Employee Gender��6C    �� 
IsEditable . $��19(�6l   
�������6z   �� 5#      -   5        $
 
EmpMailBox��5#      $��	0,30,50,0q�6.   $��Employee E-mail��6A   �� 5$      -   5        $
 EmpAdressBox��5$      $��	0,30,20,0q�60   $	��200��6C   $��Employee Adress��6P    �� TextWrapping$
 Wrap=�6g   �� 5%      -   5        $
 EmpAuthCodeBox��5%      $��	0,30,50,0q�62   $��	Auth Code��6E   $��Center=�6V   $	��150��6s   $��Center=�6    �� 	MaxLength$ 4��6�   5&      �� 5'      $	 Vertical=�6   $��2��60   .+�5(      �� -	   5        $
 EmpSurnameBox��5(      $��Employee Surname��61   $��0,0,60,0q�6I   (  "System.Windows.Controls.DatePicker  5)      -
   5          Name$ StartDateBox��5)      $��	0,30,19,0q�63   $��19(�6F   
�����p�6T   �� 5*      -   5        $
 EmpPhoneBox��5*      $��Employee Phone��6/   $��	0,33,60,0q�6E   $ 11��6X   �� 5+      -   5         ��Name$ 	AddEmpBtn��5+      $��Add Employee��6-   $��
40,40,40,0q�6D   6X   �� 5,      -   5        $ 	RemEmpBtn��5,      $��Remove Employee��6,   $��
40,20,40,0q�6F   6Z   �� 5-      $	 
Horizontal=�6   $��Center=�62   $��2��6O   $��0,40,0,0q�6_   .+�5.      �� -   5        $ 
AddRoleBtn��5.      $��Add Role��6-   $	��Top=�6@   6X   �� 5/      -   5        $ 
RemRoleBtn��5/      $��Delete Role��6-   $	��Top=�6C   $��30,0,0,0q�6[   6m   50      51      � 52      $��3��6    � RowDefinitions 53      �� 54      $��2* �6$   �� 55      $	��10* �6$   �� 56      $��3* �6$   .+�58      �� -   5        $
 RoleNameBox��58      $��Center=�6/   $��	Role Name��6J   $��	40,0,40,0q�6[   $��0��6n   �� 59      -   5        $ RoleBox��59      $��1��6+   $
��Left=�68   $	��120��6S   $��
10,0,0,150q�6_   5:      �� 5;      -   5        $ PermBox��5;      $��1��6+   $��Right=�68   $	��120��6T   $��
0,0,10,100q�6`   5<      5=      5>      �� 5?   
   -   5        $ NextBtn��5?      $��	Next Page��6"   5@      5A      5B      