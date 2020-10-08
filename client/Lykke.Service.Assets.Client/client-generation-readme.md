# Lykke.Service.Assets API Client Generation
> see https://aka.ms/autorest

1) Launch Lykke.Service.Assets on port 5000
2) Run ```autorest client-generation-readme.md``` or execute ```update.ps1```
> see https://aka.ms/autorest
``` yaml 
input-file: http://localhost:5000/swagger/v1/swagger.json

csharp:
  namespace: Lykke.Service.Assets.Client
  output-folder: ./
  output-file: AssetsService.Generated.cs
  client-side-validation: false
```
