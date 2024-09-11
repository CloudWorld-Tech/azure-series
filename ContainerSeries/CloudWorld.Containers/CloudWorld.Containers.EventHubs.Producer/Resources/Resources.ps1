param (
    [string]$ResourceGroupName = "azure-labs",
    [string]$Location = "eastus2",
    [string]$NamespaceName = "azure-labs-ns",
    [string]$EventHubName = "dapr-hub",
    [string]$ConsumerGroupName = "bg-job",
    [string]$IdentityName = "container-app-mi"
)

try
{
    $MI = az identity show --name $IdentityName -g $ResourceGroupName --query id -o json | ConvertFrom-Json
    if (-not $MI)
    {
        throw "Failed to retrieve Managed Identity ID"
    }

    $Namespace = az eventhubs namespace create --resource-group $ResourceGroupName --name $NamespaceName --location $Location --sku Standard --enable-auto-inflate --maximum-throughput-units 1 --mi-user-assigned $MI --query name -o json | ConvertFrom-Json
    if (-not $Namespace)
    {
        throw "Failed to create Event Hubs namespace"
    }

    $EventHub = az eventhubs eventhub create --resource-group $ResourceGroupName --namespace-name $NamespaceName --name $EventHubName --cleanup-policy Delete --partition-count 1 --query name -o json | ConvertFrom-Json
    if (-not $EventHub)
    {
        throw "Failed to create Event Hub"
    }

    $EVI = az eventhubs namespace show --name $NamespaceName -g $ResourceGroupName --query id -o json | ConvertFrom-Json
    if (-not $EVI)
    {
        throw "Failed to retrieve Event Hubs namespace ID"
    }

    $ConsumerGroup = az eventhubs eventhub consumer-group create --consumer-group-name $ConsumerGroupName --eventhub-name $EventHubName --namespace-name $NamespaceName --resource-group $ResourceGroupName --query name -o json | ConvertFrom-Json
    if (-not $ConsumerGroup)
    {
        throw "Failed to create Consumer Group"
    }

    $MIPrincipalId = az identity show --name $IdentityName -g $ResourceGroupName --query principalId -o json | ConvertFrom-Json
    if (-not $MIPrincipalId)
    {
        throw "Failed to retrieve Managed Identity Principal ID"
    }

    $RoleAssignment = az role assignment create --assignee-object-id $MIPrincipalId --assignee-principal-type ServicePrincipal --role 'Azure Event Hubs Data Owner' --scope $EVI --query createdOn -o json | ConvertFrom-Json
    if (-not $RoleAssignment)
    {
        throw "Failed to create role assignment"
    }

    Write-Output "Script executed successfully."
}
catch
{
    Write-Error $_.Exception.Message
}