resource "aws_eks_access_entry" "eks-access-entry" {
  cluster_name  = aws_eks_cluster.cloud-world-cluster.name
  principal_arn = "arn:aws:iam::ID:user/UserName"
  type          = "STANDARD"
}

resource "aws_eks_access_policy_association" "eks-access-policy-association" {
  cluster_name  = aws_eks_cluster.cloud-world-cluster.name
  policy_arn    = "arn:aws:eks::aws:cluster-access-policy/AmazonEKSClusterAdminPolicy"
  principal_arn = "arn:aws:iam::ID:user/UserName"

  access_scope {
    type = "cluster"
  }
}