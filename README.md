# Lykke.Service.Assets

Assets and asset pairs service

Client: [Nuget](https://www.nuget.org/packages/Lykke.Service.CandlesHistory.Client/)

# Client usage

Register client services in container using extension method:

```cs
IServiceCollection services;
...
services.UseAssetsClient(AssetServiceSettings.Create(new Uri(assetsServiceUrl), cacheExpirationPeriod));
```

Now you can use:

* IAssetsservice - Autorest generated HTTP client for service API
* ICachedAssetsService - Cached client for service API, which caches assets and asset pairs with specified expiration period
