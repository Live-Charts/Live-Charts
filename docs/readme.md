# ReadMe

This file explains the custom extended markdown used in the LiveCharts website.

## Directives

The site replaces the following commands with an HTML template:

* **${installFromNuget}**: the install from Nuget template.
* **{if, 0}**: specifies if the next item should be displayed according to the '0' parameter condition

## Variables

Here is the list of the used variables along the documentation, the web site replaces these variables according to the description of each one.

* **{platform}**: indicates the platform where the user currently is (WPF, UWP, Xamarin)