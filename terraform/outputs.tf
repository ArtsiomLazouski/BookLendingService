output "alb_dns_name" {
  description = "Public URL of the load balancer"
  value       = aws_lb.this.dns_name
}