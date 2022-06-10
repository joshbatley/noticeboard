provider "aws" {
  region                      = "eu-west-2"
  access_key                  = "fake"
  secret_key                  = "fake"
  skip_credentials_validation = true
  skip_metadata_api_check     = true
  skip_requesting_account_id  = true
  s3_use_path_style           = true

  endpoints {
    dynamodb = "http://localhost:4566"
    s3       = "http://localhost:4566"
  }
}

resource "aws_s3_bucket" "b" {
  bucket = "edgmont-images"
}

resource "aws_s3_bucket_acl" "b" {
  bucket = aws_s3_bucket.b.id
  acl    = "private"
}

resource "aws_dynamodb_table" "d" {
  read_capacity  = 10
  write_capacity = 50
  name           = "edgmont-items"
  hash_key       = "UserId"
  range_key      = "GeneratedDate"

  attribute {
    name = "UserId"
    type = "S"
  }

  attribute {
    name = "GeneratedDate"
    type = "S"
  }
  
  attribute {
    name = "Tag"
    type = "S"
  }

  global_secondary_index {
    name            = "UserId-Tagged-index"
    hash_key        = "UserId"
    range_key       = "Tag"
    projection_type = "ALL"
    read_capacity  = 10
    write_capacity = 50
  }
}