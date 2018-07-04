# Lykke.Service.Assets

Assets and asset pairs service

Client: [Nuget](https://www.nuget.org/packages/Lykke.Service.Assets.Client/)

# Client usage

Anyhow, you need to register client types during DI module(s) registration at the program' start up.

Since version 4.1.0 we have two different ways to register the client: the preferred one and the obsolete one.

## Preferred way - using a *container builder*

If you want to use all the advantages of the newest Lykke logging system ([Lykke.Logs](https://github.com/LykkeCity/Lykke.Logs) v5.x and higher), this is the best choice.
Just register the Asset Service client directly in your `ContainerBuilder` instance:

```cs
var builder = new ContainerBuilder();
...
builder.RegisterAssetsClient(
	AssetServiceSettings.Create(
		new Uri(serviceUrl),
		expirationPeriod));
```
where 
* `string serviceUrl` - network location of **Lykke.Service.Assets** working instance;
* `TimeSpan expirationPeriod` - local (client) assets cache expiration time period.

:information_source: It's assumed that you have already added Lykke logging to `ContainerBuilder` before. [Here are instructions](https://github.com/LykkeCity/Lykke.Logs/blob/master/README.md) on how you can to.

## Obsolete way - using a *service collection*

In cases when you need to handle some legacy code and\or keep backward compatibility, you still can register Assets Service client using a `ServiceCollection` instance:

```cs
var services = new ServiceCollection();
...
services.RegisterAssetsClient(
	AssetServiceSettings.Create(
		new Uri(serviceUrl), 
		expirationPeriod),
	log);
```

where parameters are the same, plus:
* `log` - an instance of any type implementing `ILog`. Is usually passed as an input parameter to the module constructor.


_After that you can use (resolve) the followings:_

* `IAssetsservice` - Autorest generated HTTP client for service API;
* `IAssetsServiceWithCache` - cached client for service API, which caches assets and asset pairs with specified expiration period.
