# SciChart.Wpf.UI
Core reactive libraries, custom controls and styles for WPF UI created by SciChart Ltd

**Release v2.1 is here!** See [scichart.com/open-source-scichart-ui-reactive-scichart-wpf-ui-transitionz-v2-1-released](https://www.scichart.com/open-source-scichart-ui-reactive-scichart-wpf-ui-transitionz-v2-1-released/) for details! 

Used in the [SciChart WPF Examples Application](https://www.scichart.com/wpf-chart-examples), which demonstrates [High Performance WPF Charts](https://www.scichart.com/wpf-chart-features), available for download at http://www.scichart.com, this library provides bootstrapping with automatic dependency discovery, and some extensions to Reactive (System.Reactive) to allow for easier testing and integration of Reactive into an MVVM Application. 

## SciChart.UI.Bootstrap library 

Supporting .NET Standard 2.0, this library includes bootstrapper classes for WPF Applications using Unity and allowing automatic dependency discovery across a range of assemblies. 

 - [Bootstrap Library - Overview](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Bootstrap-Library-overview)

## SciChart.UI.Reactive library (Rx / Reactive ViewModels)

Supporting .NET Standard 2.0, this library includes three ViewModel types which provide varying degree of observability and dependency injection, allowing you to turn any ViewModel property into an INotifyPropertyChanged property and also an IObservable<T> for reactive extensions integration 

SciChart.UI.Reactive Usage / Wiki: 

 - [Reactive Library - Overview](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Reactive-Library---Overview)
 - [Reactive Library - Rx WhenPropertyChanged](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Reactive-Library---WhenPropertyChanged)

## SciChart.WPF.UI (WPF Helper Class Library)

Core helper classes and useful controls for WPF Applications. 

!(SciChart.Wpf.UI BusyPanel, Warnings and Popups)[https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2018/11/scichart-wpf-ui-isbusy-and-popups.gif]

WIKI / Documentation coming soon.

## SciChart.WPF.UI.Transitionz (Transitionz Animation Library)

The library also includes Transitionz, an animation library which allows for easy animating of Opacity or X,Y Transforms on Visibility change with simple XAML markup extensions

!(Transitionz Animations in WPF)[https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2018/11/transitionz.gif]

Transitionz Usage / Wiki: 

 - [Intro to WPF Transitionz](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Transitionz-Library)
