resource "aws_eks_cluster" "cloud-world-cluster" {
  name                          = "cloud-world-cluster"
  version                       = "1.32"
  bootstrap_self_managed_addons = false
  role_arn                      = aws_iam_role.cluster-role.arn
  access_config {
    authentication_mode = "API"
  }
  vpc_config {
    endpoint_private_access = true
    endpoint_public_access  = true
    subnet_ids = [
      aws_subnet.public[0].id,
      aws_subnet.public[1].id
    ]
  }
  compute_config {
    enabled       = true
    node_pools    = ["general-purpose", "system"]
    node_role_arn = aws_iam_role.eks-node-role.arn
  }
  kubernetes_network_config {
    elastic_load_balancing {
      enabled = true
    }
  }
  storage_config {
    block_storage {
      enabled = true
    }
  }
  zonal_shift_config {
    enabled = true
  }
}

resource "aws_eks_addon" "metrics-server" {
  addon_name   = "metrics-server"
  cluster_name = aws_eks_cluster.cloud-world-cluster.name
}
