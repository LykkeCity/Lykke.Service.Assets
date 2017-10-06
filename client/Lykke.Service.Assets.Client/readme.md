# EthereumSamurai API Client Generation

1) Launch Lykke.Service.Assets on port 5000
2) Run ```autorest readme.md```

``` yaml 
input-file: http://localhost:5000/swagger/v1/swagger.json

csharp:
  namespace: Lykke.Service.Assets.Client
  output-folder: ./
  output-file: AssetsService.Generated.cs
```