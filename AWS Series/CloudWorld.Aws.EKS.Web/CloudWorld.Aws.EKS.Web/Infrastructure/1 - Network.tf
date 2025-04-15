resource "aws_vpc" "vpc-cloud-world-eks" {
  cidr_block = "10.0.0.0/16"
  tags = {
    Name = "vpc-cloud-world-eks"
  }
}

data "aws_availability_zones" "available" {
  state = "available"
}

resource "aws_internet_gateway" "igw" {
  vpc_id = aws_vpc.vpc-cloud-world-eks.id
  tags = {
    Name = "igw"
  }
}

resource "aws_subnet" "public" {
  count                   = 2
  vpc_id                  = aws_vpc.vpc-cloud-world-eks.id
  cidr_block              = cidrsubnet(aws_vpc.vpc-cloud-world-eks.cidr_block, 8, count.index)
  availability_zone       = data.aws_availability_zones.available.names[count.index]
  map_public_ip_on_launch = true
  tags = {
    Name = "public-${count.index}"
  }
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.vpc-cloud-world-eks.id
  tags = {
    Name = "public-route-table"
  }
}

resource "aws_route_table_association" "public" {
  count          = 2
  subnet_id      = aws_subnet.public[count.index].id
  route_table_id = aws_route_table.public.id
}

resource "aws_route" "public_internet_access" {
  route_table_id         = aws_route_table.public.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = aws_internet_gateway.igw.id
}