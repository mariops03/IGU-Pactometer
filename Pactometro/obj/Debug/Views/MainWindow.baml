   M S B A M L   `   `   `  ����E  APactometro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Pactometro
Pactometro  5   	       Pactometro.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Pactometro     5   	     � Title$  
Pactometro��5   	   $	��450��6   $	��800��6)   .��5	      � -   5        $��gridPrincipal��5	       � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��1* �6   �� 5      $	��10* �6   �� 5      $��1* �6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      $
��Auto �6   .+�5   
   o� -   5        $
��menu��5      $��0��6   $��0��6(   $��3��68   .@�5      m� $��Opciones��6    � ToolTip$  Opciones de la aplicación��6)   .@�5      m� -   5        $��Abrir ventana de datos��5      6;   �� 5      m� 5      -   5        $��Salir��5      6*   5      m� 5      -   5        $��menuGraficos��5      $��	Gráficos��6+   $* $Elige entre tres gráficos distintos��6=   .@�5      m� -   5        $��
Gráfico 1��5      6.   m� 5      -   5        $��
Gráfico 2��5      6.   m� 5      -   5        $��
Gráfico 3��5      6.   5      m� 5       -	   5        $��btnExportar��5       $��Exportar como imagen��6*   $, &Exporta el gráfico actual como imagen��6_   5!      �� 5"   
   -
   5        $��	txtTitulo��5"      $��1��6%   $��0��62   $��3��6B   $��Center=�6V   $��20(�6s   $
��Bold"�6�   �� 5#   
   -   5        $��chartCanvas��5#      $��2��6$   $��0��61   $��3��6A   $��10q�6U   
�������6a   �� 5%   
   -   5        $��btnReiniciar��5%      $��3��6%   $��0��62   $��Center=�6B   $	��Top=�6_   $��	Reiniciar��6w   $��25��6�   $	��125��6�   $y�Hidden=�6�   $5 /Pulsa para reiniciar el seleccionador de pactos��6�   �� 5&   
   -   5        $��btnPacto��5&      $��3��6!   $��1��6.   $��Center=�6>   $	��Top=�6[   $��Pacto��6s   $��25��6�   $	��125��6�   $y�Hidden=�6�    �� 	IsDefault . $# Pulsa para completar el pacto��6�   5(      5)      