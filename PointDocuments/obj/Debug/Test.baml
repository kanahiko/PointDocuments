   M S B A M L   `   `   `  ����I  EPointDocuments, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:PointDocumentsPointDocuments  5   	       PointDocuments.Test   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:PointDocuments     5   	   -   5          � Title$
  Test��5   	   $	��450��6   $	��800��6#   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ PointCanvas��5      $��10,10,10,10q�6&   ���	#FFE6E6E66R   �� 5   
   $��3��6   $��10,10,10,10q�6%   .+�5      �� -   5         ��Name$ PointsCountLabel��5      $��Points:��6.   �� 5      -   5         ��Name$ 	AddButton��5      $��Create Points��6(   $
��Left=�6@   $
��0,10q�6[   $	��Top=�6i   $	��100��6�   6�   �� 5      -   5        $ RemoveButton��5      $��Remove Points��6+   $
��Left=�6C   $
��0,10q�6^   $	��Top=�6l   $	��100��6�   6�   �� 5      -   5        $ ClearButton��5      $��Clear Points��6*   $
��Left=�6A   $
��0,10q�6\   $	��Top=�6j   $	��100��6�   6�   �� 5      -   5        $ StartButton��5      $��START��6*   $
��Left=�6:   $
��0,10q�6U   $	��Top=�6c   $	��100��6{   6�   �� 5      -   5         � Minimum$ 50Q�5       � Maximum$
 1000Q�6"    � SmallChange$ 50Q�61    � LargeChange$ 50Q�6B   	 �� TickFrequency$	 50Q�6S   
 �� TickPlacement$
 BottomRight=�6f    � Value$	 500Q�6�   6�   5      5      5      