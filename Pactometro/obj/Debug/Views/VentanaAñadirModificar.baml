   M S B A M L   `   `   `  ����E  APactometro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Pactometro
Pactometro  5   	   (    "Pactometro.VentanaAñadirModificar   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Pactometro     5   	     � Title$  VentanaAñadirModificar��5   	   $	��480��6(   $	��710��65   .��5	      � $��10q�6    � ColumnDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $(��"INFORMACIÓN DEL PROCESO ELECTORAL��6   $��16(�6=   $
��Bold"�6K   $��3��6]   $��0��6q   $��Center=�6~   $��8q�6�   �� 5   
   $��0��6   $��1��6    $ ��Tipo de Proceso Electoral:��6-   $
��Left=�6R   $��Center=�6m   �� 5    
   -   5        $��cmbTipoProceso��5    H   $��1��6   $��1��6#   $	��200��60   $��10q�6<   .@�5!      �� $��Elecciones Generales��6   �� 5"      $!��Elecciones Autonómicas CyL��6   5#      �� 5%   
   $��0��6   $��2��6    $��Fecha:��6-   $��Center=�6>   (  "System.Windows.Controls.DatePicker  5&   
   -   5        $��dpFecha��5&   J   $��1��6   $��2��6%   $	��200��62   $��10q�6>   6J   �� 5)   
   $"��INFORMACIÓN DE LOS PARTIDOS��6   $��16(�67   $
��Bold"�6E   $��3��6W   $��5��6k   $��Center=�6x   $��8q�6�   �� 5+   
    �� Orientation$ 
Horizontal=�6   $��	0,10,0,10q�6.   $��3��6A   $��6��6U   $��Center=�6b   .+�5,      �� $��Partido:��6   $��Center=�6'   �� 5-      -   5        $��
txtPartido��5-   "   $	��200��6   $��	10,0,10,0q�64   �� 5/      $��	Escaños:��6   $��Center=�6'   �� 50      -   5        $��txtEscaños��50   !   $��50��6   $��	10,0,10,0q�63   �� 52      $��Color:��6   $��Center=�6%   �� 54      -   5        $��seleccionadorColor��54      ��55      �� 56      .R�57      �� $ 
Horizontal=�6%   .+�58      �� $��16��6>   $��16��6I   $��0,2,5,2q�6U   ��6(   �� *Name+6U   �� 59      ��6(   �� *Name+5:      5;      5=      �� 5?      -   5        $	��120��5?      $��Añadir Partido��6!   $
��10,0q�6:   6H   5@      �� 5B   
   -   5        $��btnAñadirModificar��5B      $��1��6,   $��8��6<   $	��120��6I   $��30��6U   $��10q�6a   $��Añadir Proceso��6m    �� 	IsDefault . 6�   �� 5D   
   -   5        $��
lvPartidos��5D   P   $��0��6   $��2��6#   $��7��67   $��10q�6D   $	��150��6b    �� SelectionMode$ Single=�6�    � ToolTip$( "Pulsa en una cabecera para ordenar��6�    �� View 5E      �� 5F      .P�5G      �� $��PARTIDO��6%    �� Width$	 200��6^   	 �� DisplayMemberBinding	 66   �� *Nombre+
 �� HeaderTemplate
 5H      �� 5I      .R�5J   "   �� -	   5        $��PARTIDO��5J   ,   6;   5K      5M      �� 5N      $��ESCAÑOS��6%   $ 80��6_   	 66   �� *
Escaños+
 5O      �� 5P      .R�5Q   "   �� -
   5        $��ESCAÑOS��5Q   ,   6;   5R      5T      �� 5U      $��COLOR��6%   $ 80��64    �� CellTemplate 5V      �� 5W      .R�5X   "   �� $��20��6,   $��20��67   ��5Y   &   �� 5Z   *    �� Color 6:   �� *Color+5\   #   5]      5_      5`      5b      �� 5c   
   $ Vertical=�6   $��7��6,   $��2��69   $��Center=�6I   .+�5d      �� -   5        $��btnEliminar��5d      $	��150��6)   $��30��65   $��10q�6A   $��Eliminar Partido��6M   6h   �� 5e      -   5        $��btnModificar��5e      $	��150��6)   $��30��65   $��10q�6A   $��Modificar Partido��6M   6i   5f      5g      5h      