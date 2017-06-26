cd ./client/Lykke.Service.Assets.Client/
iwr http://localhost:5000/swagger/v1/swagger.json -o Service.Assets.json
autorest --input-file=Service.Assets.json --csharp --namespace=Lykke.Service.Assets.Client --output-folder=./