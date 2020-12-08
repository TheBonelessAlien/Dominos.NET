# Dominos.NET
**A C# Wrapper for [pizzapi.](https://github.com/ggrammar/pizzapi)**<br>
Order Dominos Pizza using .NET! (Works in the US and Canada, more country support coming soon.)<br>
**Made with [JSON.Net.](https://www.newtonsoft.com/json)**
**DO NOT EDIT THE FILES UNLESS YOU KNOW WHAT YOU ARE DOING.**

# Installation
1. Install [JSON.Net](https://www.newtonsoft.com/json) via NuGet
2. Add the DominosNETSource folder to your project.
3. Get pizza-ing!

# Tutorial

First, add these namespaces:
  ```cs
using DominosNET.Store;
using DominosNET.Customer;
using DominosNET.Address;
using DominosNET.Order;
```
Construct a `Customer`and `Address` object.
```cs
Customer customer = new Customer("2024561111", "Joe", "Biden", "joebiden@yourmom.com");
Address address = new Address("1600 Pennsylvania Avenue NW", "Washington", "DC", "20500", "us", ServiceType.Delivery);
```
Or, if you live in Canada:
```cs
Customer customer = new Customer("6139924793", "Justin", "Trudeau", "justintrudeau@yourmom.com");
Address address = new Address("Wellington Street", "Ottawa", "Ontario", "K1A0A9", "ca", ServiceType.Delivery);
```
