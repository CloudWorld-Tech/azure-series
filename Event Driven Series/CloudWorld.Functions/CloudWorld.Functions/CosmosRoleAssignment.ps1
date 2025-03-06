$RG_GROUP = "rg-labs"
$COSMOS_ACCOUNT_NAME = 'cloud-world'
$PRINCIPAL_ID = "45092a33-b2b7-4e43-ac54-569cd08e9503"

$COSMOS_ID = (az cosmosdb show --resource-group $RG_GROUP --name $COSMOS_ACCOUNT_NAME --query id -o tsv).Trim()
$ROLE_ID = (az cosmosdb sql role definition list --resource-group $RG_GROUP --account-name $COSMOS_ACCOUNT_NAME --query "[?roleName=='Cosmos DB Built-in Data Reader'].id" -o tsv).Trim()
az cosmosdb sql role assignment create --resource-group $RG_GROUP --account-name $COSMOS_ACCOUNT_NAME --role-definition-id $ROLE_ID --principal-id $PRINCIPAL_ID --scope $COSMOS_ID --query resourceGroup