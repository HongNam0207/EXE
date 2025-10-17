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

ALTER TABLE locations DROP CONSTRAINT CK__locations__type__4316F928;

ALTER TABLE locations
ADD CONSTRAINT CK_locations_type
CHECK (type IN ('building', 'service', 'nature', 'food', 'supermarket', 'other'));

SELECT name
FROM sys.check_constraints
WHERE parent_object_id = OBJECT_ID('analytics');

ALTER TABLE analytics DROP CONSTRAINT CK__analytics__actio__628FA481;

ALTER TABLE analytics
ADD CONSTRAINT CK_analytics_action
CHECK (action IN ('view', 'navigate', 'search', 'view_location'));

CREATE TABLE partners (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(255) NOT NULL,
    type NVARCHAR(100) CHECK (type IN ('supplier', 'service', 'investor', 'business_partner')),
    contact_email NVARCHAR(255),
    phone NVARCHAR(50),
    website NVARCHAR(255),
    description NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE()
);

INSERT INTO users (name, email, password_hash, role)
VALUES
(N'Nguyễn Văn A', 'a@student.fpt.edu.vn', 'hashed123', 'student'),
(N'Lê Thị B', 'b@guest.com', 'hashed123', 'guest'),
(N'Trần Văn C', 'c@fpt.edu.vn', 'hashed123', 'admin'),
(N'Ngô Minh D', 'd@student.fpt.edu.vn', 'hashed123', 'student');

DECLARE @uid1 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='a@student.fpt.edu.vn');
DECLARE @uid2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='b@guest.com');
DECLARE @uid3 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='c@fpt.edu.vn');
DECLARE @uid4 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='d@student.fpt.edu.vn');

INSERT INTO auth_providers (user_id, provider, provider_uid, access_token)
VALUES
(@uid1, 'google', 'g_123', 'token_1'),
(@uid2, 'facebook', 'f_456', 'token_2'),
(@uid3, 'google', 'g_789', 'token_3'),
(@uid4, 'github', 'gh_111', 'token_4');


INSERT INTO topics (name, description)
VALUES
(N'Lịch sử FPT', N'Giới thiệu quá trình hình thành và phát triển Đại học FPT'),
(N'Kiến trúc xanh', N'Các công trình thân thiện với môi trường tại campus'),
(N'Ẩm thực sinh viên', N'Các khu ăn uống, căng tin trong trường'),
(N'Hoạt động ngoại khóa', N'Sự kiện, câu lạc bộ và hoạt động sinh viên');


INSERT INTO tours (title, type, location_ids)
VALUES
(N'Tour tham quan khu học tập', 'guide', '["Tòa Beta","Tòa Delta","Thư viện ĐH FPT (Delta)"]'),
(N'Tour khám phá ký túc xá', 'virtual', '["Dom A","Dom B","Dom C"]');

DECLARE @l1 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM locations WHERE name=N'Tòa Alpha');
DECLARE @l2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM locations WHERE name=N'Hồ Đào Cóc');
DECLARE @u1 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='a@student.fpt.edu.vn');
DECLARE @u2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='b@guest.com');

INSERT INTO analytics (user_id, location_id, action)
VALUES
(@u1, @l1, 'view'),
(@u2, @l2, 'search'),
(@u1, @l2, 'view_location');


INSERT INTO partners (name, type, contact_email, phone, website, description)
VALUES
(N'VinAI Research', 'investor', 'contact@vinai.vn', '0901123123', 'https://vinai.vn', N'Hợp tác nghiên cứu AI và thị giác máy tính'),
(N'FPT Software', 'business_partner', 'info@fsoft.com.vn', '02873002222', 'https://fptsoftware.com', N'Đối tác công nghệ chính của dự án AILENS'),
(N'Thế Giới Số', 'supplier', 'sales@thegioiso.vn', '0912345678', 'https://thegioiso.vn', N'Cung cấp thiết bị camera AR'),
(N'Căng Tin Beta', 'service', 'beta@fpt.edu.vn', '0909988776', NULL, N'Đối tác dịch vụ ăn uống tại khu Beta'),
(N'FlexSim VN', 'business_partner', 'contact@flexsim.vn', '0839876543', 'https://flexsim.vn', N'Đối tác mô phỏng hệ thống logistics');

