# API Reference

Base URL: `https://localhost:5250/api`

## Response Format

**Success (200/201/204):**
```json
{
  "id": "uuid",
  "title": "string",
  "content": "string",
  "slug": "string",
  "viewCount": 0,
  "categoryId": "uuid",
  "createdAt": "2026-07-09T00:00:00Z",
  "updatedAt": "2026-07-09T00:00:00Z"
}
```

**Paginated:**
```json
{
  "items": [{...}],
  "totalCount": 100,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 10,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Error (400/404/500):**
```json
{
  "type": "https://api.example.com/errors/not-found",
  "title": "Resource not found",
  "status": 404,
  "detail": "Post with ID 'abc' not found",
  "instance": "/api/posts/abc"
}
```

## Posts Endpoints

### Get All Posts
```
GET /posts?pageNumber=1&pageSize=10
```
Returns: `200 OK` - Paginated list

---

### Get Post by ID
```
GET /posts/{id}
```
Path: `id` (guid)
Returns: 
- `200 OK` - Post details
- `404 Not Found`

---

### Search Posts
```
GET /posts/search?searchTerm=clean&categoryId=abc&pageNumber=1&pageSize=20
```

Query Parameters (all optional):
| Parameter | Type | Purpose |
|-----------|------|---------|
| `searchTerm` | string | Search title and content |
| `categoryId` | guid | Filter by category |
| `tagIds` | string[] | Comma-separated tag IDs |
| `createdAfter` | datetime | Posts after date |
| `createdBefore` | datetime | Posts before date |
| `minViewCount` | int | Minimum views |
| `sortBy` | string | Field: `created`, `updated`, `viewCount` |
| `sortDirection` | int | 0=Ascending, 1=Descending |
| `pageNumber` | int | Page (default: 1) |
| `pageSize` | int | Per page (default: 10) |

Returns: `200 OK` - Paginated results

---

### Create Post
```
POST /posts
```

Request Body:
```json
{
  "title": "string (required, 1-256 chars)",
  "content": "string (required, 10+ chars)",
  "summary": "string (required, 10+ chars)",
  "categoryId": "uuid (required)",
  "tagIds": ["uuid"]
}
```

Returns:
- `201 Created` (includes Location header)
- `400 Bad Request` - Validation errors

---

### Update Post
```
PUT /posts/{id}
```

Path: `id` (guid)

Request Body: Same as Create

Returns:
- `200 OK`
- `400 Bad Request`
- `404 Not Found`

---

### Delete Post (Soft Delete)
```
DELETE /posts/{id}
```

Path: `id` (guid)

Returns:
- `204 No Content`
- `404 Not Found`

---

## Categories Endpoints

### Get All
```
GET /categories
```
Returns: `200 OK` - List

### Get by ID
```
GET /categories/{id}
```
Returns: 
- `200 OK` - Category
- `404 Not Found`

---

## Tags Endpoints

### Get All
```
GET /tags
```
Returns: `200 OK` - List

### Get by ID
```
GET /tags/{id}
```
Returns:
- `200 OK` - Tag
- `404 Not Found`

---

## Error Codes

| Status | Meaning |
|--------|---------|
| 200 | Success |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request / Validation Error |
| 404 | Not Found |
| 500 | Server Error |

## Testing

**Interactive:** [Swagger UI](https://localhost:5250/swagger)

**Tools:** Postman, Insomnia, curl
