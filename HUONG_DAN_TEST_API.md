# Hướng dẫn test API LMS (PRN232) — Swagger & Postman

Tài liệu này mô tả **thứ tự test** hợp lý (tạo dữ liệu nền → đăng ký → đọc/lọc → cập nhật → xóa tùy nhu cầu).

## Trạng thái dự án (tóm tắt)

- Kiến trúc 3 lớp, 4 loại model, REST URI dạng `/api/{tài-nguyên-số-nhiều}`, envelope JSON, Swagger tiếng Việt.
- Chạy local: `dotnet run --project PRN232.LMS.API`
- **Base URL** (mặc định `launchSettings`):
  - HTTP: `http://localhost:5056`
  - HTTPS: `https://localhost:7179` (có thể cần bỏ qua cảnh báo chứng chỉ dev)

**Swagger UI** (đã cấu hình root): mở trình duyệt tới `http://localhost:5056/` (hoặc URL HTTPS tương ứng).

**Postman**: tạo biến môi trường `{{baseUrl}}` = `http://localhost:5056` (hoặc HTTPS).

**Header chung**

| Header        | Giá trị              |
|---------------|----------------------|
| `Content-Type`| `application/json` |

---

## Thứ tự test đề xuất (logic nghiệp vụ)

1. **Học kỳ** (`semesters`) — nền cho khóa học  
2. **Môn học** (`subjects`) — độc lập, có thể test song song sau bước 1  
3. **Khóa học** (`courses`) — cần `semesterId` hợp lệ  
4. **Sinh viên** (`students`)  
5. **Đăng ký học** (`enrollments`) — cần `studentId` và `courseId`  
6. **GET danh sách** — search, sort, page, size, fields, expand  
7. **GET theo id** — có `fields`, có `expand`  
8. **PUT** — cập nhật  
9. **DELETE** — chỉ khi muốn dọn dữ liệu (cẩn thận ràng buộc FK)

> Nếu DB đã có dữ liệu seed từ trước, bạn có thể **bắt đầu từ bước 6** để kiểm tra đọc/lọc, rồi mới thử POST/PUT/DELETE.

---

## Phần A — Test trên Swagger (theo thứ tự)

### Bước 0: Mở Swagger

1. Chạy API.  
2. Mở `http://localhost:5056/`.  
3. Mở rộng nhóm **Học kỳ** → **POST /api/semesters**.

### Bước 1 — Học kỳ

| Thứ tự | Thao tác | Endpoint | Ghi chú |
|--------|----------|----------|---------|
| 1.1 | **POST** | `/api/semesters` | Body mẫu bên dưới |
| 1.2 | **GET** | `/api/semesters` | Xem danh sách + `pagination` trong `data` |
| 1.3 | **GET** | `/api/semesters/{id}` | Dùng `id` từ 1.1; thử query `?fields=semesterId,semesterName` |
| 1.4 | **PUT** | `/api/semesters/{id}` | Sửa tên hoặc ngày |
| 1.5 | **DELETE** | `/api/semesters/{id}` | *Chỉ nếu không còn khóa học tham chiếu* — nếu FK chặn thì bỏ qua |

**Body POST /api/semesters**

```json
{
  "semesterName": "HK1 2025-2026",
  "startDate": "2025-08-01T00:00:00",
  "endDate": "2025-12-31T23:59:59"
}
```

**GET danh sách có tham số (ví dụ)**

- `/api/semesters?page=1&size=5&sort=semesterName&search=HK`
- `/api/semesters?expand=courses&fields=semesterId,semesterName`

---

### Bước 2 — Môn học

| Thứ tự | Thao tác | Endpoint |
|--------|----------|----------|
| 2.1 | **POST** | `/api/subjects` |
| 2.2 | **GET** | `/api/subjects?search=SUB&page=1&size=10` |
| 2.3 | **GET** | `/api/subjects/{id}?fields=subjectId,subjectCode,credit` |
| 2.4 | **PUT** | `/api/subjects/{id}` |
| 2.5 | **DELETE** | `/api/subjects/{id}` | *Tùy chọn* |

**Body POST /api/subjects**

```json
{
  "subjectCode": "PRN232",
  "subjectName": "Lập trình mạng",
  "credit": 3
}
```

---

### Bước 3 — Khóa học

Dùng `semesterId` thật từ bước 1 (hoặc từ DB).

| Thứ tự | Thao tác | Endpoint |
|--------|----------|----------|
| 3.1 | **POST** | `/api/courses` |
| 3.2 | **GET** | `/api/courses?expand=semester&page=1&size=10` |
| 3.3 | **GET** | `/api/courses/{id}` |
| 3.4 | **PUT** | `/api/courses/{id}` | Nếu `semesterId` sai → mong đợi **400** |
| 3.5 | **DELETE** | `/api/courses/{id}` | *Tùy chọn* |

**Body POST /api/courses**

```json
{
  "courseName": "PRN232 - Nhóm 1",
  "semesterId": 1
}
```

*(Thay `1` bằng id học kỳ thực tế.)*

---

### Bước 4 — Sinh viên

| Thứ tự | Thao tác | Endpoint |
|--------|----------|----------|
| 4.1 | **POST** | `/api/students` |
| 4.2 | **GET** | `/api/students?search=nguyen&sort=fullName,-dateOfBirth&page=1&size=10` |
| 4.3 | **GET** | `/api/students/{id}?expand=enrollments` |
| 4.4 | **PUT** | `/api/students/{id}` |
| 4.5 | **DELETE** | `/api/students/{id}` | *Tùy chọn* |

**Body POST /api/students**

```json
{
  "fullName": "Nguyen Van A",
  "email": "vana@lms.edu.vn",
  "dateOfBirth": "2003-05-15T00:00:00"
}
```

---

### Bước 5 — Đăng ký học

Dùng `studentId`, `courseId` thật.

| Thứ tự | Thao tác | Endpoint |
|--------|----------|----------|
| 5.1 | **POST** | `/api/enrollments` |
| 5.2 | **GET** | `/api/enrollments?expand=student,course&search=Active&sort=-enrollDate&page=1&size=20` |
| 5.3 | **GET** | `/api/enrollments/{id}?fields=enrollmentId,status&expand=student,course` |
| 5.4 | **PUT** | `/api/enrollments/{id}` | `studentId`/`courseId` sai → **400**; id sai → **404** |
| 5.5 | **DELETE** | `/api/enrollments/{id}` | *Tùy chọn* |

**Body POST /api/enrollments**

```json
{
  "studentId": 1,
  "courseId": 1,
  "enrollDate": "2026-01-20T08:00:00",
  "status": "Active"
}
```

---

### Kiểm tra mã HTTP & thân phản hồi (Swagger)

| Tình huống | HTTP | `success` | `message` / `errors` |
|------------|------|-----------|----------------------|
| Đọc danh sách / chi tiết OK | 200 | `true` | `message` tiếng Việt |
| Tạo mới OK | 201 | `true` | có `data` |
| Xóa OK | 204 | *(không body)* | — |
| Thiếu field / FK sai | 400 | `false` | `errors` có thể null |
| Không tìm thấy id | 404 | `false` | thông báo tiếng Việt |
| Danh sách | 200 | `true` | `data.items` + `data.pagination` |

---

## Phần B — Test trên Postman (cùng thứ tự)

### Chuẩn bị

1. **New → Environment** → tạo biến `baseUrl` = `http://localhost:5056`.  
2. Mỗi request dùng URL: `{{baseUrl}}/api/...`

### Bước 1 — Tạo Collection (gợi ý thư mục)

Tạo collection **PRN232 LMS** với các folder:

1. `01-Hoc-ky`  
2. `02-Mon-hoc`  
3. `03-Khoa-hoc`  
4. `04-Sinh-vien`  
5. `05-Dang-ky-hoc`

Trong mỗi folder, tạo request theo đúng thứ tự như bảng Swagger ở trên (POST → GET list → GET id → PUT → DELETE).

### Cấu hình request mẫu (Postman)

- **Method**: GET / POST / PUT / DELETE  
- **URL**: `{{baseUrl}}/api/students?page=1&size=10`  
- **Body** (POST/PUT): **raw** → **JSON** — dán các JSON mẫu ở phần A  
- **Params** tab: có thể nhập `search`, `sort`, `page`, `size`, `fields`, `expand` thay vì gõ trên URL

### Gợi ý test nhanh Postman (copy URL)

Sau khi có id từ response (hoặc từ DB):

```http
GET {{baseUrl}}/api/enrollments?expand=student,course&page=1&size=5
GET {{baseUrl}}/api/students/1?expand=enrollments
GET {{baseUrl}}/api/courses/1?fields=courseId,courseName
```

### Kiểm tra Postman **Tests** tab (tùy chọn)

Ví dụ cho GET danh sách:

```javascript
pm.test("HTTP 200", () => pm.response.to.have.status(200));
const j = pm.response.json();
pm.test("Envelope", () => {
  pm.expect(j).to.have.property("success", true);
  pm.expect(j.data).to.have.property("pagination");
});
```

---

## Ghi chú cuối

- **Seed**: nếu bảng đã có dữ liệu, POST có thể tạo thêm bản ghi mới; GET vẫn đủ để demo search/sort/paging.  
- **Docker**: khi chạy container, đổi `{{baseUrl}}` thành `http://localhost:8080` (nếu compose map cổng như vậy).  
- Nếu cần file **Postman Collection JSON** export sẵn, có thể yêu cầu thêm để tạo `PRN232.LMS.postman_collection.json` trong repo.

---

**Kết luận:** Về mặt yêu cầu lab đã trao đổi trước đó, phần code API/Swagger đã xong; file này là checklist **thứ tự test** trên Swagger và Postman. Chạy lại `dotnet run`, mở Swagger, đi theo **Phần A** từ bước 1 → 5 là đủ để demo cho giảng viên.
