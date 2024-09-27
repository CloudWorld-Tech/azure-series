$ResourceGroup = 'azure-labs'
$CONTAINER_APP_ENV_NAME = 'container-apps-env'
$StorageAccountName = 'daprcwstgac'
$IdentityName = 'container-app-mi'
$RegistryName = 'azurelabsacr'

az acr build --registry $RegistryName `
--image dapr-consumer:v1 `
--file CloudWorld.Containers.Dapr.Consumer/Dockerfile .

az acr build --registry $RegistryName `
--image dapr-producer:v1 `
--file CloudWorld.Containers.Dapr.Producer/Dockerfile .

$MI = az identity show --name $IdentityName -g $ResourceGroup --query principalId -o json | ConvertFrom-Json
$ACID = az storage account create `--name $StorageAccountName `
--resource-group $ResourceGroup `
--sku Standard_LRS `
--query id -o json | ConvertFrom-Json

az role assignment create --assignee-object-id $MI `
--assignee-principal-type ServicePrincipal `
--role 'Storage Blob Data Contributor' `
--scope $ACID

az containerapp env dapr-component set `
--dapr-component-name pubsub `
--name $CONTAINER_APP_ENV_NAME `
--resource-group $ResourceGroup `
--yaml event-hub.yaml

$MI = az identity show --name container-app-mi -g azure-labs --query id -o json | ConvertFrom-Json

az containerapp create `
--name dapr-consumer `
--resource-group $ResourceGroup  `
--container-name dapr-consumer `
--image azurelabsacr.azurecr.io/dapr-consumer:v1 `
--ingress external `
--target-port 8080  `
--registry-server azurelabsacr.azurecr.io  `
--min-replicas 1 `
--max-replicas 1  `
--environment $CONTAINER_APP_ENV_NAME  `
--registry-identity $MI `
--enable-dapr true `
--dapr-app-id dapr-consumer `
--dapr-app-port 8080 `
--dapr-app-protocol http `
--user-assigned $MI `
--query name

az containerapp create `
--name dapr-producer `
--resource-group $ResourceGroup `
--container-name dapr-producer `
--image azurelabsacr.azurecr.io/dapr-producer:v1 `
--registry-server azurelabsacr.azurecr.io `
--min-replicas 1 `
--max-replicas 1 `
--environment $CONTAINER_APP_ENV_NAME `
--registry-identity $MI `
--enable-dapr true `
--dapr-app-id dapr-producer `
--dapr-app-protocol http `
--user-assigned $MI `
--query name