# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2026-07-09

### Added
- Complete documentation structure in `/docs` folder
- GitHub base files (.editorconfig, CODEOWNERS, .gitattributes, LICENSE)
- Clean README with navigation to docs
- Comprehensive API reference documentation

### Changed
- Consolidated documentation to eliminate duplication
- Reorganized docs for better navigation
- Split monolithic README into focused documentation files

### Fixed
- Documentation duplications across multiple files

## [1.0.0] - 2026-06-15

### Added
- Clean Architecture implementation with 4 layers
- CQRS pattern with MediatR
- Advanced search with filtering, sorting, pagination
- Soft delete functionality
- Specification pattern for query logic
- In-process caching with 5-minute TTL
- FluentValidation integration
- Global exception handling
- Request logging middleware
- AutoMapper for DTO mapping
- Swagger/OpenAPI documentation
- Database seeding with sample data
- Entity Framework Core with SQLite

### Features
- Post CRUD operations
- Category management
- Tag management
- Advanced search with multiple filters
- MediatR pipeline behaviors (logging, caching, validation, auth, performance, transactions, exception handling)
- Consistent error response format (RFC 7807)
- Pagination support on all list endpoints
