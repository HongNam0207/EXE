Create database AILENS
use AILENS
-- 1. Bảng users
CREATE TABLE users (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password_hash NVARCHAR(MAX),
    role NVARCHAR(50) CHECK (role IN ('student', 'guest', 'admin')),
    avatar_url NVARCHAR(500),
    login_method NVARCHAR(50) DEFAULT 'email',
    created_at DATETIME DEFAULT GETDATE()
);

-- 2. Bảng auth_providers
CREATE TABLE auth_providers (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    provider NVARCHAR(50),
    provider_uid NVARCHAR(255),
    access_token NVARCHAR(MAX),
    refresh_token NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE()
);

-- 3. Bảng locations
CREATE TABLE locations (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL,
    type NVARCHAR(100) CHECK (type IN ('history', 'nature', 'building', 'service', 'other')),
    description NVARCHAR(MAX),
    coordinates NVARCHAR(MAX),
    image_url NVARCHAR(500),
    qr_code_url NVARCHAR(500),
    glb_model_url NVARCHAR(500),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

-- 4. Bảng topics
CREATE TABLE topics (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX)
);

-- 5. Bảng location_topics
CREATE TABLE location_topics (
    location_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES locations(id),
    topic_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES topics(id),
    PRIMARY KEY (location_id, topic_id)
);

-- 6. Bảng chat_logs
CREATE TABLE chat_logs (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    question NVARCHAR(MAX),
    response NVARCHAR(MAX),
    timestamp DATETIME DEFAULT GETDATE()
);

-- 7. Bảng maps
CREATE TABLE maps (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    location_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES locations(id),
    map_data NVARCHAR(MAX)
);

-- 8. Bảng tours
CREATE TABLE tours (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    title NVARCHAR(255) NOT NULL,
    type NVARCHAR(50) CHECK (type IN ('virtual', 'guide', 'custom')),
    location_ids NVARCHAR(MAX)
);

-- 9. Bảng admin_logs
CREATE TABLE admin_logs (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    admin_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    action NVARCHAR(MAX),
    timestamp DATETIME DEFAULT GETDATE()
);

-- 10. Bảng analytics
CREATE TABLE analytics (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES users(id),
    location_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES locations(id),
    action NVARCHAR(50) CHECK (action IN ('view', 'navigate', 'search')),
    timestamp DATETIME DEFAULT GETDATE()
);


INSERT INTO locations (name, type, description, coordinates)
VALUES
(N'Thư viện ĐH FPT (Delta)', 'building', N'Thư viện – khu học tập trung', '21.014775,105.525468'),
(N'Tòa Beta', 'building', N'Tòa giảng đường chính của sinh viên', '21.013879,105.525489'),
(N'Tòa Alpha', 'building', N'Tòa nhà học vụ FPT University', '21.013328,105.527203'),
(N'Tòa Delta', 'building', N'Tòa phòng lab và khu học nhóm', '21.014535,105.525240'),
(N'Hồ Đào Cóc', 'other', N'Hồ trung tâm, nơi check-in nổi tiếng', '21.014405,105.526616'),
(N'Tượng Thinker', 'other', N'Tượng biểu tượng của Đại học FPT', '21.013408,105.526823'),
(N'Dom A', 'building', N'Ký túc xá sinh viên – khu A', '21.013153,105.525334'),
(N'Dom B', 'building', N'Ký túc xá sinh viên – khu B', '21.012852,105.525618'),
(N'Dom C', 'building', N'Ký túc xá sinh viên – khu C', '21.012652,105.525251'),
(N'Dom D', 'building', N'Ký túc xá sinh viên – khu D', '21.012352,105.525538'),
(N'Dom E', 'building', N'Ký túc xá sinh viên – khu E', '21.012722,105.523762'),
(N'Dom F', 'building', N'Ký túc xá sinh viên – khu F', '21.012407,105.524025'),
(N'Dom G', 'building', N'Ký túc xá sinh viên – khu G', '21.012502,105.523134'),
(N'Dom H', 'building', N'Ký túc xá sinh viên – khu H', '21.012181,105.523440'),
(N'Trường THPT FPT', 'building', N'Cơ sở trung học phổ thông FPT', '21.013361,105.523446'),
(N'Căng tin 1', 'food', N'Khu ăn uống trong tòa Beta', '21.013158,105.524814'),
(N'Căng tin 2', 'food', N'Khu ăn uống phía Dom khu C', '21.013148,105.522603'),
(N'Sân tập lái ô tô Hòa Lạc', 'other', N'Khu tập lái xe', '21.011425,105.524400'),
(N'Điểm đỗ xe buýt Hòa Lạc', 'other', N'Điểm đỗ xe buýt chính khuôn viên', '21.012685,105.527702');
