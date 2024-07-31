# ZaupShop
A simple shop using the chat line. This allows you to have a shop for players to use Uconomy currency to buy items and vehicles. Buying vehicles is turned off by default.

There are 2 commands: /cost, and /buy.
/cost and /buy are meant to be used by everyone that has permissions for them. Just put the command in the permissions file for the group.

Usage:
/buy <i or v>.<item name or id>/[amount] (optional): This will use the same name to id find as /i. i stands for item, v stands for vehicle.  Amount is only available for items and is optional, default is 1.
/cost <i or v>.<item name or id>: Same as above but will display the user the cost of asked for item/vehicle.

Requirements:
- Uconomy (or xpMode)
- Mysql

# Building

*Windows*: The project uses dotnet 4.8, consider installing into your machine, you need visual studio, simple open the solution file open the Build section and hit the build button (ctrl + shift + b) or you can do into powershell the command dotnet build -c Debug if you have installed dotnet 4.8.

*Linux*: Unfortunately versions lower than 6 of dotnet do not have support for linux, the best thing you can do is install dotnet 6 or the lowest possible version on your distro and try to compile using the command dotnet build -c Debug, this can cause problems within rocket loader.

FTM License.
