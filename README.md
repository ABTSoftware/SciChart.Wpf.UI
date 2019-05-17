# SciChart.Wpf.UI

Core reactive libraries, custom controls and styles for WPF UI created by SciChart Ltd

Used in the [SciChart WPF Examples Application](https://www.scichart.com/wpf-chart-examples), which demonstrates [High Performance WPF Charts](https://www.scichart.com/wpf-chart-features), available for download at http://www.scichart.com, this library provides bootstrapping with automatic dependency discovery, and some extensions to Reactive (System.Reactive) to allow for easier testing and integration of Reactive into an MVVM Application. 

**May 2019: Release v3.0 is here!** 

In this release we update SciChart.Wpf.UI and SciChart.Wpf.UI.Transitionz to target .NET Core 3 (netcoreapp3.0), as well as .NET Framework 4.5.2

SciChart.UI.Bootstrap and SciChart.UI.Reactive have been downgraded to target both .NET Standard 2.0 as well as .NET Framework v4.5.2 (previous minimum framework was net461). This enables the libraries to be used in a wider range of projects. 

As a side effect to this downgrade, we have had to also downgrade to System.Reactive 3.1.1, since v4.x is no longer compatible with net452. 

Feedback is welcome on the issues tab

**Nov 2018: Release v2.1 is here!** 

See [scichart.com/open-source-scichart-ui-reactive-scichart-wpf-ui-transitionz-v2-1-released](https://www.scichart.com/open-source-scichart-ui-reactive-scichart-wpf-ui-transitionz-v2-1-released/) for details! 

## SciChart.UI.Bootstrap library 

Supporting .NET Standard 2.0 & .NET Framework 4.5.2, this library includes bootstrapper classes for WPF Applications using Unity and allowing automatic dependency discovery across a range of assemblies. 

 - [Bootstrap Library - Overview](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Bootstrap-Library-overview)

## SciChart.UI.Reactive library (Rx / Reactive ViewModels)

Supporting .NET Standard 2.0 & .NET Framework 4.5.2, this library includes three ViewModel types which provide varying degree of observability and dependency injection, allowing you to turn any ViewModel property into an INotifyPropertyChanged property and also an IObservable<T> for reactive extensions integration 

SciChart.UI.Reactive Usage / Wiki: 

 - [Reactive Library - Overview](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Reactive-Library---Overview)
 - [Reactive Library - Rx WhenPropertyChanged](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Reactive-Library---WhenPropertyChanged)

## SciChart.WPF.UI (WPF Helper Class Library)

Supporting .NET Core 3.0  & .NET Framework 4.5.2, this contains core helper classes and useful controls for WPF Applications. 

![SciChart.Wpf.UI BusyPanel, Warnings and Popups](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2018/11/scichart-wpf-ui-isbusy-and-popups.gif)

WIKI / Documentation coming soon.

## SciChart.WPF.UI.Transitionz (Transitionz Animation Library)

Supporting .NET Core 3.0  & .NET Framework 4.5.2, this library includes Transitionz, an animation library which allows for easy animating of Opacity or X,Y Transforms on Visibility change with simple XAML markup extensions

![Transitionz Animations in WPF](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2018/11/transitionz.gif)

Transitionz Usage / Wiki: 

 - [Intro to WPF Transitionz](https://github.com/ABTSoftware/SciChart.Wpf.UI/wiki/Transitionz-Library)
