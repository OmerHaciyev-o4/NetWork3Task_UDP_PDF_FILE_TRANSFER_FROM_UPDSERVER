   M S B A M L   `   `   `  ����D  @UDPClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UDPClient	UDPClient  5   	       UDPClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   #localclr-namespace:UDPClient  5   	     � Title$  
UDP Client��5	   	   $	��450��5
   	   $	��800��5   	    � WindowStartupLocation$ CenterScreen=�5   	   
������.��5      �  � RowDefinitions 5   
   �� 5      �� 5      $��9* �6   .+�5   
   �  � ColumnDefinitions 5      �� 5      �� 5      �� 5      .+�5      �� 
�������
��� � � � ToolTip$ 
Connection��5      $��40��5      $��20(�5       $��DemiBold"�5!      ��5"      �� *ConnectionCommand+��5#      '  !MaterialDesignThemes.Wpf.PackIcon  5$         Kind$ 	PowerPlug=�6.   $��30��6?   $��30��6J   5&      �� 5(      $��1��5)      $ Disconnection��5*      $��20(�5+      $��40��5,      $��DemiBold"�5-      
�������
���  ����50      �� *DisconnectionCommand+��51        52      $ 
Connection=�6.   $��30��6@   $��30��6K   54      �� 56      -   5         ��Name$ NameBox��58      $��2��57      $��25(�59      
�������)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Enter name��5;      5<      � 5>   
   $��1��5?       5@      �� 5A      �� 5B      $	��10* �6    .+�5E      �� 
���   �$��Center=�6A   
�������$��25��6x   ��6   �� *State+6x   �� 5F      -   5         ��Name$ DragDropListBox��5H      $��1��5G      5H      5I      5J      5K      