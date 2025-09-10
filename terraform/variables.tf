variable "region" {
  description = "AWS region"
  type        = string
  default     = "eu-central-1"
}

variable "service_name" {
  description = "ECS service name"
  type        = string
  default     = "booklend-api"
}

variable "image_url" {
  description = "ECR image URI (account.dkr.ecr.region.amazonaws.com/repo:tag)"
  type        = string
}

variable "cpu"    { type = number, default = 256 }
variable "memory" { type = number, default = 512 }
